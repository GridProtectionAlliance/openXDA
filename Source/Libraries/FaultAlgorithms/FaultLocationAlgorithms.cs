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
//  --------------------------------------------------------------------------------------------------- 
//  Portions of this work are derived from "openFLE" which is an Electric Power Research Institute, Inc.
//  (EPRI) copyrighted open source software product released under the BSD license.  openFLE carries
//  the following copyright notice: Version 1.0 - Copyright 2012 ELECTRIC POWER RESEARCH INSTITUTE, INC.
//  All rights reserved.
//  ---------------------------------------------------------------------------------------------------
//
//  The algorithms and methods in this class come from the following sources, among others:
//
//  1. IEEE-C37.114 (2005), IEEE Guide for Determining Fault Location on AC Transmission and
//         Distribution Lines
//  2. EPRI Product Report 1024381 (2013), Distribution Fault Location Support Tools, Algorithms and
//         Implementation Approaches
//  3. Novosel D., Hart DG, Saha MM et al. (1994) Optimal fault location for transmission systems. 
//         ABB Rev(8) 20-27.
//  4. Takagi T, Yamakosi Y, Yamura M et al. (1981) Development of a new type fault locator using the
//         one-terminal voltage and current data.  IEEE Transactions on PAS, 101(8):2892-2898
//  5. Eriksson L., Saha MM, Rockefeller GD et al. (1985) An accurate fault locator with compensation
//         for apparent reactance in the fault resistance resulting from remote-end infeed. IEEE
//         Power Engineering Review, 5(2): 44
//
//  Code Modification History:
//  ----------------------------------------------------------------------------------------------------
//  11/12/2013 - Stephen C. Wills
//       Generated original version of source code.
//  11/21/2013 - Stephen C. Wills
//       Added the ModifiedTakagi and NovoselEtAl fault location methods.
//  12/11/2014 - Stephen C. Wills
//       Added the Eriksson fault location method.
//
//******************************************************************************************************

using System;
using System.Linq;
using GSF;
using GSF.Units;

namespace FaultAlgorithms
{
    /// <summary>
    /// Class containing algorithms for determining fault location based on fault data.
    /// </summary>
    public class FaultLocationAlgorithms
    {
        #region [ Fault Location Methods ]
        
        /// <summary>
        /// Simple algorithm to calculate the distance to a fault that was found in the <see cref="FaultLocationDataSet"/>.
        /// </summary>
        /// <param name="faultDataSet">The data set to be used to find the distance to fault.</param>
        /// <param name="parameters">Extra parameters to the algorithm.</param>
        /// <returns>Set of distance calculations, one for each cycle of data.</returns>
        [FaultLocationAlgorithm]
        public static double[] Simple(FaultLocationDataSet faultDataSet, string parameters)
        {
            ComplexNumber nominalImpedance;

            nominalImpedance = GetNominalImpedance(faultDataSet);

            return faultDataSet.Cycles
                .Select(cycleData => new
                {
                    V = GetFaultVoltage(cycleData, faultDataSet.FaultType),
                    I = GetFaultCurrent(cycleData, faultDataSet.FaultType),
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
        public static double[] Reactance(FaultLocationDataSet faultDataSet, string parameters)
        {
            ComplexNumber nominalImpedance;

            nominalImpedance = GetNominalImpedance(faultDataSet);

            return faultDataSet.Cycles
                .Select(cycleData => new
                {
                    V = GetFaultVoltage(cycleData, faultDataSet.FaultType),
                    I = GetFaultCurrent(cycleData, faultDataSet.FaultType),
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
        public static double[] Takagi(FaultLocationDataSet faultDataSet, string parameters)
        {
            ComplexNumber z;

            ComplexNumber[] voltages;
            ComplexNumber[] currents;
            ComplexNumber iPre;

            z = GetNominalImpedance(faultDataSet);

            voltages = faultDataSet.Cycles.Select(cycle => GetFaultVoltage(cycle, faultDataSet.FaultType)).ToArray();
            currents = faultDataSet.Cycles.Select(cycle => GetFaultCurrent(cycle, faultDataSet.FaultType)).ToArray();
            iPre = GetFaultCurrent(faultDataSet.PrefaultCycle, faultDataSet.FaultType);

            return voltages.Zip(currents, (v, i) =>
            {
                ComplexNumber iSupConjugate = (i - iPre).Conjugate;
                return (v * iSupConjugate).Imaginary / (z * i * iSupConjugate).Imaginary;
            })
            .Select(m => m * faultDataSet.LineDistance)
            .ToArray();
        }

        /// <summary>
        /// Takagi algorithm, substituting zero sequence current for superposition current, for
        /// calculating the distance to a fault that was found in the <see cref="FaultLocationDataSet"/>.
        /// </summary>
        /// <param name="faultDataSet">The data set to be used to find the distance to fault.</param>
        /// <param name="parameters">Extra parameters to the algorithm.</param>
        /// <returns>Set of distance calculations, one for each cycle of data.</returns>
        [FaultLocationAlgorithm]
        public static double[] ModifiedTakagi(FaultLocationDataSet faultDataSet, string parameters)
        {
            ComplexNumber z;

            ComplexNumber[] voltages;
            ComplexNumber[] currents;
            ComplexNumber[] zeros;

            z = GetNominalImpedance(faultDataSet);

            voltages = faultDataSet.Cycles.Select(cycle => GetFaultVoltage(cycle, faultDataSet.FaultType)).ToArray();
            currents = faultDataSet.Cycles.Select(cycle => GetFaultCurrent(cycle, faultDataSet.FaultType)).ToArray();
            zeros = faultDataSet.Cycles.Select(cycle => 3 * CycleData.CalculateSequenceComponents(cycle.AN.I, cycle.BN.I, cycle.CN.I)[0]).ToArray();

            return voltages.Zip(currents, (v, i) => new
            {
                V = v,
                I = i
            })
            .Zip(zeros, (vi, zero) =>
            {
                ComplexNumber v = vi.V;
                ComplexNumber i = vi.I;
                Angle t = (i / zero).Angle;

                ComplexNumber ejt = new ComplexNumber(t, 1.0D);

                return (v * zero.Conjugate * ejt).Imaginary / (z * i * zero.Conjugate * ejt).Imaginary;
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
        public static double[] Novosel(FaultLocationDataSet faultDataSet, string parameters)
        {
            ComplexNumber z;

            ComplexNumber[] voltages;
            ComplexNumber[] currents;
            ComplexNumber vPre;
            ComplexNumber iPre;

            ComplexNumber loadImpedance;

            z = GetNominalImpedance(faultDataSet);

            voltages = faultDataSet.Cycles.Select(cycle => GetFaultVoltage(cycle, faultDataSet.FaultType)).ToArray();
            currents = faultDataSet.Cycles.Select(cycle => GetFaultCurrent(cycle, faultDataSet.FaultType)).ToArray();
            vPre = GetFaultVoltage(faultDataSet.PrefaultCycle, faultDataSet.FaultType);
            iPre = GetFaultCurrent(faultDataSet.PrefaultCycle, faultDataSet.FaultType);

            loadImpedance = (vPre / iPre) - z;

            return voltages.Zip(currents, (v, i) =>
            {
                ComplexNumber sourceImpedance = faultDataSet.ZSrc;

                // TODO: Test to determine the effect of using -(v - vPre) / (i - iPre) instead
                if (IsNaN(sourceImpedance))
                    sourceImpedance = (v - vPre) / (i - iPre);

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

                if (MinDistance(m1, 0.0D, 1.0D) < MinDistance(m2, 0.0D, 1.0D))
                    return m1;

                return m2;
            })
            .Select(m => m * faultDataSet.LineDistance)
            .ToArray();
        }

        /// <summary>
        /// Eriksson algorithm for calculating the distance to a fault that was found in the <see cref="FaultLocationDataSet"/>.
        /// </summary>
        /// <param name="faultDataSet">The data set to be used to find the distance to fault.</param>
        /// <param name="parameters">Extra parameters to the algorithm.</param>
        /// <returns>Set of distance calculations, one for each cycle of data.</returns>
        [FaultLocationAlgorithm]
        public static double[] Eriksson(FaultLocationDataSet faultDataSet, string parameters)
        {
            ComplexNumber z;

            ComplexNumber[] voltages;
            ComplexNumber[] currents;
            ComplexNumber iPre;

            ComplexNumber sourceImpedance;
            ComplexNumber remoteImpedance;

            sourceImpedance = faultDataSet.ZSrc;
            remoteImpedance = faultDataSet.ZRem;

            if (IsNaN(sourceImpedance) || IsNaN(remoteImpedance))
                return null;

            z = GetNominalImpedance(faultDataSet);

            voltages = faultDataSet.Cycles.Select(cycle => GetFaultVoltage(cycle, faultDataSet.FaultType)).ToArray();
            currents = faultDataSet.Cycles.Select(cycle => GetFaultCurrent(cycle, faultDataSet.FaultType)).ToArray();
            iPre = GetFaultCurrent(faultDataSet.PrefaultCycle, faultDataSet.FaultType);

            return voltages.Zip(currents, (v, i) =>
            {
                ComplexNumber ab = (v / (i * z)) + 1 + (remoteImpedance / z);
                ComplexNumber cd = (v / (i * z)) * ((remoteImpedance / z) + 1);
                ComplexNumber ef = ((i - iPre) / (i * z)) * (((sourceImpedance + remoteImpedance) / z) + 1);

                double a = ab.Real, b = ab.Imaginary;
                double c = cd.Real, d = cd.Imaginary;
                double e = ef.Real, f = ef.Imaginary;

                double left = (a - ((e * b) / f));
                double right = Math.Sqrt(left * left - 4.0D * (c - ((e * d) / f)));
                double m1 = (left + right) / 2.0D;
                double m2 = (left - right) / 2.0D;

                if (MinDistance(m1, 0.0D, 1.0D) < MinDistance(m2, 0.0D, 1.0D))
                    return m1;

                return m2;
            })
            .Select(m => m * faultDataSet.LineDistance)
            .ToArray();
        }

        /// <summary>
        /// Double-ended algorithm for calculating the distance to a fault that was found in the <see cref="FaultLocationDataSet"/>.
        /// </summary>
        /// <param name="localFaultDataSet">The data set used to find the distance to the fault.</param>
        /// <param name="remoteFaultCycle">The cycle of data from the remote station used in the double-ended distance algorithm.</param>
        /// <param name="parameters">Extra parameters to the algorithm.</param>
        /// <returns>A set of distance calculations, one for each cycle of data in <paramref name="localFaultDataSet"/>.</returns>
        public static ComplexNumber[] DoubleEnded(FaultLocationDataSet localFaultDataSet, CycleData remoteFaultCycle, string parameters)
        {
            FaultType faultType;
            ComplexNumber vfs;
            ComplexNumber ifs;
            ComplexNumber z;

            faultType = localFaultDataSet.FaultType;
            vfs = GetDoubleEndedFaultVoltage(remoteFaultCycle, faultType);
            ifs = GetDoubleEndedFaultCurrent(remoteFaultCycle, faultType);
            z = localFaultDataSet.Z1;

            return localFaultDataSet.Cycles
                .Select(cycleData => new
                {
                    Vns = GetDoubleEndedFaultVoltage(cycleData, faultType),
                    Ins = GetDoubleEndedFaultCurrent(cycleData, faultType)
                })
                .Select(cycle => (cycle.Vns - vfs + z * ifs) / (z * (cycle.Ins + ifs)))
                .Select(m => m * localFaultDataSet.LineDistance)
                .ToArray();
        }

        #endregion

        #region [ Helper Methods ]

        // Get the voltage to use in fault calculations for this cycle, based on the fault type.
        public static ComplexNumber GetFaultVoltage(CycleData cycle, FaultType viFaultType)
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
                case FaultType.ABG:
                    return cycle.AN.V.Complex - cycle.BN.V.Complex;

                case FaultType.BC:
                case FaultType.BCG:
                    return cycle.BN.V.Complex - cycle.CN.V.Complex;

                case FaultType.CA:
                case FaultType.CAG:
                    return cycle.CN.V.Complex - cycle.AN.V.Complex;

                case FaultType.ABC:
                    if (cycle.AN.I.Error < cycle.BN.I.Error && cycle.AN.I.Error < cycle.CN.I.Error)
                        return cycle.AN.V.Complex;

                    if (cycle.BN.I.Error < cycle.CN.I.Error)
                        return cycle.BN.V.Complex;

                    return cycle.CN.V.Complex;

                default:
                    throw new ArgumentOutOfRangeException("viFaultType");
            }
        }

        // Get the current to use in fault calculations for this cycle, based on the fault type.
        public static ComplexNumber GetFaultCurrent(CycleData cycle, FaultType viFaultType)
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
                case FaultType.ABG:
                    return cycle.AN.I.Complex - cycle.BN.I.Complex;

                case FaultType.BC:
                case FaultType.BCG:
                    return cycle.BN.I.Complex - cycle.CN.I.Complex;

                case FaultType.CA:
                case FaultType.CAG:
                    return cycle.CN.I.Complex - cycle.AN.I.Complex;

                case FaultType.ABC:
                    if (cycle.AN.I.Error < cycle.BN.I.Error && cycle.AN.I.Error < cycle.CN.I.Error)
                        return cycle.AN.I.Complex;

                    if (cycle.BN.I.Error < cycle.CN.I.Error)
                        return cycle.BN.I.Complex;

                    return cycle.CN.I.Complex;

                default:
                    throw new ArgumentOutOfRangeException("viFaultType");
            }
        }

        // Get the voltage used in double-ended fault calculations for this cycle, based on the fault type.
        private static ComplexNumber GetDoubleEndedFaultVoltage(CycleData cycle, FaultType faultType)
        {
            ComplexNumber[] sequenceComponents;

            sequenceComponents = CycleData.CalculateSequenceComponents(cycle.AN.V, cycle.BN.V, cycle.CN.V);

            if (faultType == FaultType.ABC)
                return sequenceComponents[1];

            return sequenceComponents[2];
        }

        // Get teh current to use in fault calculations for this cycle, based on the fault type.
        private static ComplexNumber GetDoubleEndedFaultCurrent(CycleData cycle, FaultType faultType)
        {
            ComplexNumber[] sequenceComponents;

            sequenceComponents = CycleData.CalculateSequenceComponents(cycle.AN.I, cycle.BN.I, cycle.CN.I);

            if (faultType == FaultType.ABC)
                return sequenceComponents[1];

            return sequenceComponents[2];
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
                case FaultType.ABG:
                case FaultType.BCG:
                case FaultType.CAG:
                case FaultType.ABC:
                    return faultDataSet.Z1;

                default:
                    throw new ArgumentOutOfRangeException("faultDataSet", string.Format("Unknown fault type: {0}", faultDataSet.FaultType));
            }
        }

        // Gets the minimum distance between the value n and the set of values m.
        private static double MinDistance(double n, params double[] m)
        {
            return m.Min(x => Math.Abs(n - x));
        }

        // Determines whether one of either the real or imaginary
        // components of the given complex number is NaN.
        private static bool IsNaN(ComplexNumber num)
        {
            return double.IsNaN(num.Real) || double.IsNaN(num.Imaginary);
        }

        #endregion
    }
}
