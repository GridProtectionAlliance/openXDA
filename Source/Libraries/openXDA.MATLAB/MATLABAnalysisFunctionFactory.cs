//******************************************************************************************************
//  MATLABAnalysisFunctionFactory.cs - Gbtc
//
//  Copyright © 2023, Grid Protection Alliance.  All Rights Reserved.
//
//  Licensed to the Grid Protection Alliance (GPA) under one or more contributor license agreements. See
//  the NOTICE file distributed with this work for additional information regarding copyright ownership.
//  The GPA licenses this file to you under the MIT License (MIT), the "License"; you may not use this
//  file except in compliance with the License. You may obtain a copy of the License at:
//
//      http://opensource.org/licenses/MIT
//
//  Unless agreed to in writing, the subject software distributed under the License is distributed on an
//  "AS-IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. Refer to the
//  License for the specific language governing permissions and limitations.
//
//  Code Modification History:
//  ----------------------------------------------------------------------------------------------------
//  03/31/2023 - Stephen C. Wills
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading.Tasks;
using GSF.Collections;
using MathWorks.MATLAB.NET.Arrays;
using AnalysisFunctionKey = System.Tuple<string, string>;

namespace openXDA.MATLAB
{
    public interface IMATLABAnalysisFunctionInvoker : IDisposable
    {
        MWArray[] Invoke(int numArgsOut, MWArray voltage, MWArray current, MWArray analog, MWArray fs, MWArray setting);
    }

    public delegate IMATLABAnalysisFunctionInvoker MATLABAnalysisFunctionInvokerFactory();

    public class MATLABAnalysisFunctionFactory
    {
        #region [ Members ]

        // Nested Types
        private class AnalysisFunctionInvoker : IMATLABAnalysisFunctionInvoker
        {
            private object Instance { get; }
            private AnalysisFunction AnalysisFunction { get; }
            private bool IsDisposed { get; set; }

            public AnalysisFunctionInvoker(object instance, AnalysisFunction analysisFunction)
            {
                Instance = instance;
                AnalysisFunction = analysisFunction;
            }

            public MWArray[] Invoke(int numArgsOut, MWArray voltage, MWArray current, MWArray analog, MWArray fs, MWArray setting) =>
                AnalysisFunction(Instance, numArgsOut, voltage, current, analog, fs, setting);

            public void Dispose()
            {
                if (IsDisposed)
                    return;

                try
                {
                    IDisposable disposable = Instance as IDisposable;
                    disposable?.Dispose();
                }
                finally
                {
                    IsDisposed = true;
                }
            }
        }

        // Delegates
        private delegate MWArray[] AnalysisFunction(object instance, int numArgsOut, MWArray voltage, MWArray current, MWArray analog, MWArray fs, MWArray setting);

        #endregion

        #region [ Properties ]

        private ConcurrentDictionary<AnalysisFunctionKey, Task<MATLABAnalysisFunctionInvokerFactory>> InvokerFactoryLookup { get; }
            = new ConcurrentDictionary<AnalysisFunctionKey, Task<MATLABAnalysisFunctionInvokerFactory>>();

        #endregion

        #region [ Methods ]

        public MATLABAnalysisFunctionInvokerFactory GetAnalysisFunctionInvokerFactory(string assemblyName, string functionName)
        {
            AnalysisFunctionKey key = Tuple.Create(assemblyName, functionName);
            TaskCompletionSource<MATLABAnalysisFunctionInvokerFactory> taskCompletionSource = null;
            Task<MATLABAnalysisFunctionInvokerFactory> localTask = null;

            // This pattern is used to prevent compilation of the same MATLAB analysis function on multiple threads in parallel
            Task<MATLABAnalysisFunctionInvokerFactory> lookupTask = InvokerFactoryLookup.GetOrAdd(key, _ =>
            {
                taskCompletionSource = new TaskCompletionSource<MATLABAnalysisFunctionInvokerFactory>(TaskCreationOptions.RunContinuationsAsynchronously);
                localTask = taskCompletionSource.Task;
                return localTask;
            });

            // If the local task is not returned by InvokerFactoryLookup.GetOrAdd(),
            // then some other thread beat us to the punch so we can skip this
            if (ReferenceEquals(localTask, lookupTask))
            {
                try
                {
                    MATLABAnalysisFunctionInvokerFactory newFactory = BuildInvokerFactory(assemblyName, functionName);
                    taskCompletionSource.TrySetResult(newFactory);
                }
                catch (Exception ex)
                {
                    // The task will always throw the same exception whenever it is awaited.
                    // This means the service must be restarted in order to fix errors.
                    // But it's probably necessary anyway because the MATLAB assembly would need to be reloaded
                    taskCompletionSource.TrySetException(ex);
                }
            }

            return lookupTask.GetAwaiter().GetResult();
        }

        #endregion

        #region [ Static ]

        // Static Methods

        private static MATLABAnalysisFunctionInvokerFactory BuildInvokerFactory(string assemblyName, string functionName)
        {
            Assembly assembly = Assembly.LoadFrom(assemblyName);
            Type instanceType = assembly.GetTypes()[0];
            MethodInfo analysisMethod = FindAnalysisMethod(instanceType, functionName);

            string typeName = instanceType.Name;
            LambdaExpression lambdaExpression = BuildLambda(instanceType, analysisMethod);
            AnalysisFunction analysisFunction = BuildAnalysisFunction(typeName, functionName, lambdaExpression);

            return () =>
            {
                object instance = Activator.CreateInstance(instanceType);
                return new AnalysisFunctionInvoker(instance, analysisFunction);
            };
        }

        // MATLAB produces multiple overloads of the analysis method to support the calling conventions used by the MATLAB programming language.
        // This uses reflection to select the most appropriate analysis method from the C# dll produced by MATLAB.
        private static MethodInfo FindAnalysisMethod(Type instanceType, string methodName)
        {
            BindingFlags methodFlags =
                BindingFlags.Instance |
                BindingFlags.Public |
                BindingFlags.DeclaredOnly;

            return instanceType
                .GetMethods(methodFlags)
                .Where(method => method.Name == methodName)
                .MaxBy(method => method.GetParameters().Length);
        }

        // Builds a lambda expression that can be compiled to a delegate.
        private static LambdaExpression BuildLambda(Type instanceType, MethodInfo analysisMethod)
        {
            ParameterInfo[] parameters = analysisMethod.GetParameters();

            // Parameters are passed in this order to match the signature of the AnalysisFunction delegate
            ParameterExpression[] parameterExpressions =
            {
                Expression.Parameter(typeof(object), "instance"),
                Expression.Parameter(typeof(int), "numArgsOut"),
                Expression.Parameter(typeof(MWArray), "Voltage"),
                Expression.Parameter(typeof(MWArray), "Current"),
                Expression.Parameter(typeof(MWArray), "Analog"),
                Expression.Parameter(typeof(MWArray), "Fs"),
                Expression.Parameter(typeof(MWArray), "Setting")
            };

            Dictionary<string, ParameterExpression> parameterExpressionLookup = parameterExpressions
                .ToDictionary(expr => expr.Name, StringComparer.OrdinalIgnoreCase);

            ParameterExpression TryFind(ParameterInfo parameter) =>
                parameterExpressionLookup.TryGetValue(parameter.Name, out ParameterExpression parameterExpression)
                    ? parameterExpression
                    : throw new InvalidOperationException($"MATLAB analytic {analysisMethod.Name} requires unsupported parameter: {parameter.Name}");

            // MATLAB function arguments are matched AnalysisFunction parameters by name.
            // This ensures arguments are supplied to the MATLAB function call in the order they are specified by the MATLAB function
            List<ParameterExpression> argumentExpressions = parameters
                .Select(TryFind)
                .ToList();

            UnaryExpression convertExpression = Expression.Convert(parameterExpressions[0], instanceType);
            MethodCallExpression callExpression = Expression.Call(convertExpression, analysisMethod, argumentExpressions);
            return Expression.Lambda(typeof(AnalysisFunction), callExpression, parameterExpressions);
        }

        // Compiles the lambda into a delegate to make calling the function from C# easy and fast.
        private static AnalysisFunction BuildAnalysisFunction(string typeName, string functionName, LambdaExpression lambdaExpression)
        {
            Type methodType = typeof(AnalysisFunction);
            MethodInfo invokeMethod = methodType.GetMethod("Invoke");

            Type[] parameterTypes = invokeMethod
                .GetParameters()
                .Select(parameter => parameter.ParameterType)
                .ToArray();

            AssemblyName assemblyName = new AssemblyName(typeName);
            AssemblyBuilder assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);
            ModuleBuilder moduleBuilder = assemblyBuilder.DefineDynamicModule(typeName);
            TypeBuilder typeBuilder = moduleBuilder.DefineType(typeName, TypeAttributes.Public);
            MethodBuilder methodBuilder = typeBuilder.DefineMethod(functionName, MethodAttributes.Static, invokeMethod.ReturnType, parameterTypes);
            lambdaExpression.CompileToMethod(methodBuilder);

            Type createdType = typeBuilder.CreateType();
            MethodInfo createdMethod = createdType.GetMethods(BindingFlags.NonPublic | BindingFlags.Static)[0];
            Delegate analysisFunction = Delegate.CreateDelegate(typeof(AnalysisFunction), createdMethod);
            return (AnalysisFunction)analysisFunction;
        }

        #endregion
    }
}
