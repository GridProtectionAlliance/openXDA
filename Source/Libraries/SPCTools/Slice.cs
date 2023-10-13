//******************************************************************************************************
//  SPCTools/Slice.cs - Gbtc
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
    public class Slice : IArithmeticOperands
    {
        public List<double> Values { get; private set; }

        public Slice(List<double> values)
        {
            Values = values;
        }

        public Slice(Slice slice)
        {
            Values = slice.Values;
        }

        public static implicit operator Slice(List<double> list)
        {
            return new Slice(list);
        }

        // Overload the + operator for Slice
        public static IArithmeticOperands operator +(IArithmeticOperands obj, Slice slice)
        {
            if (obj is Scalar)
            {
                Scalar scalar = obj as Scalar;
                var result = slice + scalar;
                return new Slice(result as Slice);
            }
            else if (obj is Slice)
            {
                Slice s = obj as Slice;
                List<double> result = s.Values.Select((value, index) => value + slice.Values[index]).ToList();
                return new Slice(result);
            }
            else if (obj is Matrix)
            {
                Matrix matrix = obj as Matrix;
                List<List<double[]>> result = matrix.Values.Select((row, i) => row.Select(point => new double[] { point[0], point[1] + slice.Values[i] }).ToList()).ToList();
                return new Matrix(result);
            }

            throw new InvalidOperationException("Unexpected type found in Expression");
        }

        public static Slice operator +(double num, Slice slice)
        {
            Scalar scalar = new Scalar(num);
            var result = scalar + slice;
            return new Slice(result as Slice);
        }

        public static Slice operator +(Slice slice, double num)
        {
            Scalar scalar = new Scalar(num);
            var result = slice + scalar;
            return new Slice(result as Slice);
        }

        // Overload the * operator for Slice
        public static IArithmeticOperands operator *(IArithmeticOperands obj, Slice slice)
        {
            if (obj is Scalar)
            {
                Scalar scalar = obj as Scalar;
                var s = slice * scalar;
                return new Slice(s as Slice);
            }
            else if (obj is Slice)
            {
                Slice s = obj as Slice;
                List<double> result = s.Values.Select((value, index) => value * slice.Values[index]).ToList();
                return new Slice(result);
            }
            else if (obj is Matrix)
            {
                Matrix matrix = obj as Matrix;
                List<List<double[]>> result = matrix.Values.Select((row, i) => row.Select(point => new double[] { point[0], point[1] * slice.Values[i] }).ToList()).ToList();
                return new Matrix(result);
            }

            throw new InvalidOperationException("Unexpected type found in Expression");
        }

        public static Slice operator *(double num, Slice slice)
        {
            Scalar scalar = new Scalar(num);
            var result = slice * scalar;
            return new Slice(result as Slice);
        }

        public static Slice operator *(Slice slice, double num)
        {
            Scalar scalar = new Scalar(num);
            var result = slice * scalar;
            return new Slice(result as Slice);
        }

        // Overload the - operator for Slice
        public static IArithmeticOperands operator -(IArithmeticOperands obj, Slice slice)
        {
            if (obj is Scalar)
            {
                Scalar scalar = obj as Scalar;
                var result = slice.Values.Select(value => scalar.Value - value).ToList();
                return new Slice(result);
            }
            else if (obj is Slice)
            {
                Slice s = obj as Slice;
                var result = s.Values.Select((value, index) => value - slice.Values[index]).ToList();
                return new Slice(result);
            }
            else if (obj is Matrix)
            {
                Matrix matrix = obj as Matrix;
                List<List<double[]>> result = matrix.Values.Select((row, i) => row.Select(point => new double[] { point[0], point[1] - slice.Values[i] }).ToList()).ToList();
                return new Matrix(result);
            }

            throw new InvalidOperationException("Unexpected type found in Expression");
        }



        public static Slice operator -(double num, Slice slice)
        {
            Scalar scalar = new Scalar(num);
            var result = scalar - slice;
            return new Slice(result as Slice);
        }

        public static Slice operator -(Slice slice, double num)
        {
            Scalar scalar = new Scalar(num);
            var result = slice - scalar;
            return new Slice(result as Slice);
        }

        // Overload the / operator for Slice
        public static IArithmeticOperands operator /(IArithmeticOperands obj, Slice slice)
        {
            if (slice.Values.Any(value => value == 0))
            {
                throw new DivideByZeroException("Can not divide by zero");
            }
            else if (obj is Scalar)
            {
                Scalar scalar = obj as Scalar;
                List<double> result = slice.Values.Select(value => scalar.Value / value).ToList();
                return new Slice(result);
            }
            else if (obj is Slice)
            {
                //Probably should add checks to see if the lists are same size..
                Slice s = obj as Slice;
                List<double> result = s.Values.Select((value, index) => value / slice.Values[index]).ToList();
                return new Slice(result);
            }
            else if (obj is Matrix)
            {
                //Probably should add checks to see if the lists are same size..
                Matrix matrix = obj as Matrix;
                List<List<double[]>> result = matrix.Values.Select((row, i) => row.Select(point => new double[] { point[0], point[1] / slice.Values[i] }).ToList()).ToList();
                return new Matrix(result);
            }

            throw new InvalidOperationException("Unexpected type found in Expression");
        }

        public static Slice operator /(double num, Slice slice)
        {
            Scalar scalar = new Scalar(num);
            var result = scalar / slice;
            return new Slice(result as Slice);
        }

        public static Slice operator /(Slice slice, double num)
        {
            Scalar scalar = new Scalar(num);
            var result = slice / scalar;
            return new Slice(result as Slice);
        }

        //Unary Operator Overloads
        public static Slice operator -(Slice num)
        {
            List<double> result = num.Values.Select((value) => -value).ToList();
            return new Slice(result);
        }

        public static Slice operator +(Slice num)
        {
            List<double> result = num.Values.Select((value) => +value).ToList();
            return new Slice(result);
        }

    }
}
