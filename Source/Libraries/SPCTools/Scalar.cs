//******************************************************************************************************
//  SPCTools/Scalar.cs - Gbtc
//
//  Copyright © 2018, Grid Protection Alliance.  All Rights Reserved.
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
//  10/12/2023 - Preston Crawford
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;


namespace SPCTools
{
    public class Scalar : IExpressionOperands
    {
        public double Value { get; private set; }

        public Scalar(double value)
        {
            Value = value;
        }

        public Scalar(Scalar scalar)
        {
            Value = scalar.Value;
        }

        // Overload the + operator
        public static IExpressionOperands operator +(IExpressionOperands obj, Scalar scalar)
        {
            if (obj is Scalar)
            {
                Scalar s = obj as Scalar;
                return new Scalar(s.Value + scalar.Value);
            }
            else if (obj is Slice)
            {
                Slice slice = obj as Slice;
                List<double> result = slice.Values.Select((value, index) => value + scalar.Value).ToList();
                return new Slice(result);
            }
            else if (obj is Matrix)
            {
                Matrix matrix = obj as Matrix;
                List<List<double[]>> result = matrix.Values.Select(row => row.Select(point => new double[] { point[0], point[1] + scalar.Value }).ToList()).ToList();
                return new Matrix(result);
            }

            throw new InvalidOperationException("Unexpected type found in Expression");
        }

        public static Scalar operator +(double num, Scalar scalar)
        {
            return new Scalar(num + scalar.Value);
        }

        public static Scalar operator +(Scalar scalar, double num)
        {
            return new Scalar(num + scalar.Value);
        }

        // Overload the - operator for Scalar cover both cases here since order of operations matter in subtraction
        public static IExpressionOperands operator -(IExpressionOperands obj, Scalar scalar)
        {
            if (obj is Scalar)
            {
                Scalar s = obj as Scalar;
                return new Scalar(s.Value - scalar.Value);
            }
            else if (obj is Slice)
            {
                Slice slice = obj as Slice;
                List<double> result = slice.Values.Select((value, index) => value - scalar.Value).ToList();
                return new Slice(result);
            }
            else if (obj is Matrix)
            {
                Matrix matrix = obj as Matrix;
                var result = matrix.Values.Select(row => row.Select(point => new double[] { point[0], point[1] - scalar.Value }).ToList()).ToList();
                return new Matrix(result);
            }

            throw new InvalidOperationException("Unexpected type found in Expression");
        }

        public static Scalar operator -(double num, Scalar scalar)
        {
            return new Scalar(num - scalar.Value);
        }

        public static Scalar operator -(Scalar scalar, double num)
        {
            return new Scalar(num - scalar.Value);
        }


        // Overload the * operator for Slice
        public static IExpressionOperands operator *(IExpressionOperands obj, Scalar scalar)
        {
            if (obj is Scalar)
            {
                Scalar s = obj as Scalar;
                return new Scalar(s.Value * scalar.Value);
            }
            else if (obj is Slice)
            {
                Slice slice = obj as Slice;
                List<double> result = slice.Values.Select((value, index) => value * scalar.Value).ToList();
                return new Slice(result);
            }
            else if (obj is Matrix)
            {
                Matrix matrix = obj as Matrix;
                List<List<double[]>> result = matrix.Values.Select(row => row.Select(point => new double[] { point[0], point[1] * scalar.Value }).ToList()).ToList();
                return new Matrix(result);
            }

            throw new InvalidOperationException("Unexpected type found in Expression");
        }

        public static Scalar operator *(double num, Scalar scalar)
        {
            return new Scalar(num * scalar.Value);
        }

        public static Scalar operator *(Scalar scalar, double num)
        {
            return new Scalar(num * scalar.Value);
        }

        // Overload the / operator for Scalar
        public static IExpressionOperands operator /(IExpressionOperands obj, Scalar scalar)
        {
            if (scalar.Value == 0)
                throw new DivideByZeroException("Can not divide by zero");

            if (obj is Scalar)
            {
                Scalar s = obj as Scalar;
                return new Scalar(s.Value / scalar.Value);
            }
            else if (obj is Slice)
            {
                Slice slice = obj as Slice;
                List<double> result = slice.Values.Select(value => value / scalar.Value).ToList();
                return new Slice(result);
            }
            else if (obj is Matrix)
            {
                Matrix matrix = obj as Matrix;
                List<List<double[]>> result = matrix.Values.Select(row => row.Select(point => new double[] { point[0], point[1] / scalar.Value }).ToList()).ToList();
                return new Matrix(result);
            }

            throw new InvalidOperationException("Unexpected type found in Expression");
        }

        public static Scalar operator /(double num, Scalar scalar)
        {
            return new Scalar(num / scalar.Value);
        }

        public static Scalar operator /(Scalar scalar, double num)
        {
            return new Scalar(scalar.Value / num);
        }

        //Unary operators
        public static Scalar operator -(Scalar scalar)
        {
            return new Scalar(-scalar.Value);
        }
        public static Scalar operator +(Scalar scalar)
        {
            return new Scalar(+scalar.Value);
        }

    }
}
