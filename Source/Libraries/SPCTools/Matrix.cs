//******************************************************************************************************
//  SPCTools/Matrix.cs - Gbtc
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
    public class Matrix : IExpressionOperands
    {
        public List<IAsyncEnumerable<double[]>> Values { get; private set; }

        public Matrix(List<IAsyncEnumerable<double[]>> values)
        {
            Values = values;
        }

        public Matrix(Matrix matrix)
        {
            Values = matrix.Values;
        }

        public static IExpressionOperands operator -(IExpressionOperands obj, Matrix matrix)
        {
            if (obj is Scalar)
            {
                Scalar scalar = obj as Scalar;
                var result = matrix - scalar;
                return new Matrix(result as Matrix);
            }
            else if (obj is Slice)
            {
                Slice slice = obj as Slice;
                var result = matrix - slice;
                return new Matrix(result as Matrix);
            }
            else if (obj is Matrix)
            {
                Matrix mat = obj as Matrix;
                List<IAsyncEnumerable<double[]>> result = mat.Values.Select((row, i) => row.Zip(matrix.Values[i], (matPoint, matrixPoint) => new double[] { matPoint[0], matPoint[1] - matrixPoint[1] })).ToList();
                return new Matrix(result);
            }

            throw new InvalidOperationException("Unexpected type found in Expression");
        }

        public static Matrix operator -(double num, Matrix matrix)
        {
            Scalar scalar = new Scalar(num);
            var result = matrix - scalar;
            return new Matrix(result as Matrix);
        }

        public static Matrix operator -(Matrix matrix, double num)
        {
            Scalar scalar = new Scalar(num);
            var result = matrix - scalar;
            return new Matrix(result as Matrix);
        }

        public static IExpressionOperands operator +(IExpressionOperands obj, Matrix matrix)
        {
            if (obj is Scalar)
            {
                Scalar scalar = obj as Scalar;
                var result = matrix + scalar;
                return new Matrix(result as Matrix);
            }
            else if (obj is Slice)
            {
                Slice slice = obj as Slice;
                var result = matrix + slice;
                return new Matrix(result as Matrix);
            }
            else if (obj is Matrix)
            {
                Matrix mat = obj as Matrix;
                List<IAsyncEnumerable<double[]>> result = mat.Values.Select((row, i) => row.Zip(matrix.Values[i], (matPoint, matrixPoint) => new double[] { matPoint[0], matPoint[1] + matrixPoint[1] })).ToList();
                return new Matrix(result);
            }

            throw new InvalidOperationException("Unexpected type found in Expression");
        }

        public static Matrix operator +(double num, Matrix matrix)
        {
            Scalar scalar = new Scalar(num);
            var result = matrix + scalar;
            return new Matrix(result as Matrix);
        }

        public static Matrix operator +(Matrix matrix, double num)
        {
            Scalar scalar = new Scalar(num);
            var result = matrix + scalar;
            return new Matrix(result as Matrix);
        }

        public static IExpressionOperands operator *(IExpressionOperands obj, Matrix matrix)
        {
            if (obj is Scalar)
            {
                Scalar scalar = obj as Scalar;
                var result = matrix * scalar;
                return new Matrix(result as Matrix);
            }
            else if (obj is Slice)
            {
                Slice slice = obj as Slice;
                var result = matrix * slice;
                return new Matrix(result as Matrix);
            }
            else if (obj is Matrix)
            {
                Matrix mat = obj as Matrix;
                List<IAsyncEnumerable<double[]>> result = mat.Values.Select((row, i) => row.Zip(matrix.Values[i], (matPoint, matrixPoint) => new double[] { matPoint[0], matPoint[1] * matrixPoint[1] })).ToList();
                return new Matrix(result);
            }

            throw new InvalidOperationException("Unexpected type found in Expression");
        }

        public static Matrix operator *(double num, Matrix matrix)
        {
            Scalar scalar = new Scalar(num);
            var result = matrix * scalar;
            return new Matrix(result as Matrix);
        }

        public static Matrix operator *(Matrix matrix, double num)
        {
            Scalar scalar = new Scalar(num);
            var result = matrix * scalar;
            return new Matrix(result as Matrix);
        }

        public static IExpressionOperands operator /(IExpressionOperands obj, Matrix matrix)
        {
            if (obj is Scalar)
            {
                Scalar scalar = obj as Scalar;
                List<IAsyncEnumerable<double[]>> result = matrix.Values.Select(row => row.Select(point => new double[] { point[0], scalar.Value / point[1] })).ToList();
                return new Matrix(result);
            }
            else if (obj is Slice)
            {
                Slice slice = obj as Slice;
                List<IAsyncEnumerable<double[]>> result = matrix.Values.Select((row, i) => row.Select(point => new double[] { point[0], slice.Values[i] / point[1] })).ToList();
                return new Matrix(result);
            }
            else if (obj is Matrix)
            {
                Matrix mat = obj as Matrix;
                List<IAsyncEnumerable<double[]>> result = mat.Values.Select((row, i) => row.Zip(matrix.Values[i], (matPoint, matrixPoint) => new double[] { matPoint[0], matPoint[1] / matrixPoint[1] })).ToList();
                return new Matrix(result);
            }

            throw new InvalidOperationException("Unexpected type found in Expression");
        }

        public static Matrix operator /(double num, Matrix matrix)
        {
            Scalar scalar = new Scalar(num);
            var result = scalar / matrix;
            return new Matrix(result as Matrix);
        }

        public static Matrix operator /(Matrix matrix, double num)
        {
            Scalar scalar = new Scalar(num);
            var result = matrix / scalar;
            return new Matrix(result as Matrix);
        }

        //Unary Operator Overloads
        public static Matrix operator -(Matrix matrix)
        {
            var result = matrix.Values.Select(row => row.Select(point => new double[] { point[0], -point[1] })).ToList();
            return new Matrix(result);
        }

        public static Matrix operator +(Matrix matrix)
        {
            var result = matrix.Values.Select(row => row.Select(point => new double[] { point[0], +point[1] })).ToList();
            return new Matrix(result);
        }
    }
}
