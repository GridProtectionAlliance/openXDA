//******************************************************************************************************
//  FaultLocationAlgorithms.cs - Gbtc
//
//  Copyright © 2013, Grid Protection Alliance.  All Rights Reserved.
//
//  Licensed to the Grid Protection Alliance (GPA) under one or more contributor license agreements. See
//  the NOTICE file distributed with this work for additional information regarding copyright ownership.
//  The GPA licenses this file to you under the Eclipse Public License -v 1.0 (the "License"); you may
//  not use this file except in compliance with the License. You may obtain a copy of the License at:
//
//      http://www.opensource.org/licenses/eclipse-1.0.php
//
//  Unless agreed to in writing, the subject software distributed under the License is distributed on an
//  "AS-IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. Refer to the
//  License for the specific language governing permissions and limitations.
//
//  Code Modification History:
//  ----------------------------------------------------------------------------------------------------
//  11/12/2013 - Stephen C. Wills
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Linq;
using GSF;

namespace FaultAlgorithms
{
    internal class FaultLocationAlgorithms
    {
        /// <summary>
        /// Simple algorithm to calculate the distance to a fault that was found in the <see cref="FaultLocationDataSet"/>.
        /// </summary>
        /// <param name="faultDataSet">The data set to be used to find the distance to fault.</param>
        /// <param name="parameters">Extra parameters to the algorithm.</param>
        /// <returns>Set of distance calculations, one for each cycle of data.</returns>
        [FaultLocationAlgorithm]
        private static double[] Simple(FaultLocationDataSet faultDataSet, string parameters)
        {
            FaultType viFaultType;
            ComplexNumber nominalImpedance;

            viFaultType = GetVIFaultType(faultDataSet);
            nominalImpedance = GetNominalImpedance(faultDataSet);

            return faultDataSet.Cycles
                .Select(cycleData => new
                {
                    V = GetFaultVoltage(cycleData, viFaultType),
                    I = GetFaultCurrent(cycleData, viFaultType),
                    Z = nominalImpedance
                })
                .Select(cycle => (cycle.V.Magnitude / cycle.I.Magnitude) / cycle.Z.Magnitude)
                .Select(m => m * faultDataSet.LineDistance)
                .ToArray();
        }

        /// <summary>
        /// Reactance algorithm to calculate the distance to a fault that was found in the <see cref="FaultLocationDataSet"/>.
        /// </summary>
        /// <param name="faultDataSet">The data set to be used to find the distance to fault.</param>
        /// <param name="parameters">Extra parameters to the algorithm.</param>
        /// <returns>Set of distance calculations, one for each cycle of data.</returns>
        [FaultLocationAlgorithm]
        private static double[] Reactance(FaultLocationDataSet faultDataSet, string parameters)
        {
            FaultType viFaultType;
            ComplexNumber nominalImpedance;

            viFaultType = GetVIFaultType(faultDataSet);
            nominalImpedance = GetNominalImpedance(faultDataSet);

            return faultDataSet.Cycles
                .Select(cycleData => new
                {
                    V = GetFaultVoltage(cycleData, viFaultType),
                    I = GetFaultCurrent(cycleData, viFaultType),
                    Z = nominalImpedance
                })
                .Select(cycle => (cycle.V / cycle.I).Imaginary / cycle.Z.Imaginary)
                .Select(m => m * faultDataSet.LineDistance)
                .ToArray();
        }

        /// <summary>
        /// Takagi algorithm for calculating the distance to a fault that was found in the <see cref="FaultLocationDataSet"/>.
        /// </summary>
        /// <param name="faultDataSet">The data set to be used to find the distance to fault.</param>
        /// <param name="parameters">Extra parameters to the algorithm.</param>
        /// <returns>Set of distance calculations, one for each cycle of data.</returns>
        [FaultLocationAlgorithm]
        private static double[] Takagi(FaultLocationDataSet faultDataSet, string parameters)
        {
            FaultType viFaultType;
            ComplexNumber z;

            ComplexNumber[] voltages;
            ComplexNumber[] currents;
            ComplexNumber iPre;

            viFaultType = GetVIFaultType(faultDataSet);
            z = GetNominalImpedance(faultDataSet);

            voltages = faultDataSet.Cycles.Select(cycle => GetFaultVoltage(cycle, viFaultType)).ToArray();
            currents = faultDataSet.Cycles.Select(cycle => GetFaultCurrent(cycle, viFaultType)).ToArray();
            iPre = currents[0];

            return voltages.Zip(currents, (v, i) =>
            {
                ComplexNumber iSupConjugate = (i - iPre).Conjugate;
                return (v * iSupConjugate).Imaginary / (z * i * iSupConjugate).Imaginary;
            })
            .Select(m => m * faultDataSet.LineDistance)
            .ToArray();
        }

        /// <summary>
        /// Novosel et al. algorithm for calculating the distance to a fault that was found in the <see cref="FaultLocationDataSet"/>.
        /// </summary>
        /// <param name="faultDataSet">The data set to be used to find the distance to fault.</param>
        /// <param name="parameters">Extra parameters to the algorithm.</param>
        /// <returns>Set of distance calculations, one for each cycle of data.</returns>
        [FaultLocationAlgorithm]
        private static double[] NovoselEtAl(FaultLocationDataSet faultDataSet, string parameters)
        {
            FaultType viFaultType;
            ComplexNumber z;

            ComplexNumber[] voltages;
            ComplexNumber[] currents;
            ComplexNumber vPre;
            ComplexNumber iPre;

            ComplexNumber loadImpedance;

            viFaultType = GetVIFaultType(faultDataSet);
            z = GetNominalImpedance(faultDataSet);

            voltages = faultDataSet.Cycles.Select(cycle => GetFaultVoltage(cycle, viFaultType)).ToArray();
            currents = faultDataSet.Cycles.Select(cycle => GetFaultCurrent(cycle, viFaultType)).ToArray();
            vPre = voltages[0];
            iPre = currents[0];

            loadImpedance = (vPre / iPre) - z;

            return voltages.Zip(currents, (v, i) =>
            {
                ComplexNumber sourceImpedance = (v - vPre) / (i - iPre);
                ComplexNumber ab = (v / (z * i)) + (loadImpedance / z) + 1;
                ComplexNumber cd = (v / (z * i)) * (1 + (loadImpedance / z));
                ComplexNumber ef = ((i - iPre) / (z * i)) * (1 + ((loadImpedance + sourceImpedance) / z));

                double a = ab.Real, b = ab.Imaginary;
                double c = cd.Real, d = cd.Imaginary;
                double e = ef.Real, f = ef.Imaginary;

                double left = (a - ((e * b) / f));
                double right = Math.Sqrt(left * left - 4.0D * (c - ((e * d) / f)));
                double m1 = (left + right) / 2.0D;
                double m2 = (left - right) / 2.0D;

                if (m1 >= 0.0D && m1 <= 1.0D)
                    return m1;

                return m2;
            })
            .Select(m => m * faultDataSet.LineDistance)
            .ToArray();
        }

        // Gets the fault type used to determine which voltages and currents to use in fault calculations.
        // This method changes the ABC fault type to either AN, BN, or CN based on whichever phase is closest to a pure sine wave.
        private static FaultType GetVIFaultType(FaultLocationDataSet faultDataSet)
        {
            double anError;
            double bnError;
            double cnError;
            double minError;

            if (faultDataSet.FaultType == FaultType.ABC)
            {
                anError = faultDataSet.Cycles.Sum(cycle => cycle.AN.IError);
                bnError = faultDataSet.Cycles.Sum(cycle => cycle.BN.IError);
                cnError = faultDataSet.Cycles.Sum(cycle => cycle.CN.IError);
                minError = Math.Min(Math.Min(anError, bnError), cnError);

                if (anError == minError)
                    return FaultType.AN;

                if (bnError == minError)
                    return FaultType.BN;

                return FaultType.CN;
            }

            return faultDataSet.FaultType;
        }

        // Get the voltage to use in fault calculations for this cycle, based on the fault type.
        private static ComplexNumber GetFaultVoltage(CycleData cycle, FaultType viFaultType)
        {
            switch (viFaultType)
            {
                case FaultType.AN:
                    return cycle.AN.V.Complex;

                case FaultType.BN:
                    return cycle.BN.V.Complex;

                case FaultType.CN:
                    return cycle.CN.V.Complex;

                case FaultType.AB:
                    return cycle.AN.V.Complex - cycle.BN.V.Complex;

                case FaultType.BC:
                    return cycle.BN.V.Complex - cycle.CN.V.Complex;

                case FaultType.CA:
                    return cycle.CN.V.Complex - cycle.AN.V.Complex;

                default:
                    throw new ArgumentOutOfRangeException("viFaultType");
            }
        }

        // Get the current to use in fault calculations for this cycle, based on the fault type.
        private static ComplexNumber GetFaultCurrent(CycleData cycle, FaultType viFaultType)
        {
            switch (viFaultType)
            {
                case FaultType.AN:
                    return cycle.AN.I.Complex;

                case FaultType.BN:
                    return cycle.BN.I.Complex;

                case FaultType.CN:
                    return cycle.CN.I.Complex;

                case FaultType.AB:
                    return cycle.AN.I.Complex - cycle.BN.I.Complex;

                case FaultType.BC:
                    return cycle.BN.I.Complex - cycle.CN.I.Complex;

                case FaultType.CA:
                    return cycle.CN.I.Complex - cycle.AN.I.Complex;

                default:
                    throw new ArgumentOutOfRangeException("viFaultType");
            }
        }

        // Get the nominal impedance value to use in fault calculations, based on the fault type.
        private static ComplexNumber GetNominalImpedance(FaultLocationDataSet faultDataSet)
        {
            switch (faultDataSet.FaultType)
            {
                case FaultType.AN:
                case FaultType.BN:
                case FaultType.CN:
                    return faultDataSet.Zs;

                case FaultType.AB:
                case FaultType.BC:
                case FaultType.CA:
                case FaultType.ABC:
                    return faultDataSet.Z1;

                default:
                    throw new ArgumentOutOfRangeException("faultDataSet", string.Format("Unknown fault type: {0}", faultDataSet.FaultType));
            }
        }
    }
}
