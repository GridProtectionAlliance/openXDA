//******************************************************************************************************
//  CapBank.cs - Gbtc
//
//  Copyright © 2017, Grid Protection Alliance.  All Rights Reserved.
//
//  Licensed to the Grid Protection Alliance (GPA) under one or more contributor license agreements. See
//  the NOTICE file distributed with this work for additional information regarding copyright ownership.
//  The GPA licenses this file to you under the MIT License (MIT), the "License"; you may
//  not use this file except in compliance with the License. You may obtain a copy of the License at:
//
//      http://opensource.org/licenses/MIT
//
//  Unless agreed to in writing, the subject software distributed under the License is distributed on an
//  "AS-IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. Refer to the
//  License for the specific language governing permissions and limitations.
//
//  Code Modification History:
//  ----------------------------------------------------------------------------------------------------
//  12/13/2019 - Christoph Lackner
//      Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using GSF.Data;
using GSF.Data.Model;
using Newtonsoft.Json;

/// <summary>
/// Relay:
///     34, 35 compensated
///     
/// </summary>
namespace openXDA.Model
{
    /// <summary>
    /// The following Lines are from Asset:
    /// 18 : VoltageKV
    /// 
    /// The following Lines are Systemwide Settings
    /// 33
    /// </summary>
    [MetadataType(typeof(Asset))]
    public class CapBank: Asset
    {
        #region [ Members ]

        private List<CapBankRelay> m_relays;

        #endregion

        #region [ Properties ]

        /// <summary>
        /// Line 17
        /// </summary>
        public int NumberOfBanks { get; set; }

        /// <summary>
        /// Line 19
        /// </summary>
        public double CapacitancePerBank { get; set; }

        /// <summary>
        /// Line 20
        /// </summary>
        public string CktSwitcher { get; set; }

        /// <summary>
        /// Line 21
        /// </summary>
        public double MaxKV { get; set; }

        /// <summary>
        /// Line 22
        /// </summary>
        public double UnitKV { get; set; }

        /// <summary>
        /// Line 23
        /// </summary>
        public double UnitKVAr { get; set; }

        /// <summary>
        /// Line 24
        /// </summary>
        public double NegReactanceTol { get; set; }
        
        /// <summary>
        /// Line 25
        /// </summary>
        public double PosReactanceTol { get; set; }

        /// <summary>
        /// Line 26 (Fuseless) Line 31 (Fused)
        /// </summary>
        public int Nparalell { get; set; }

        /// <summary>
        /// Line 27 (Fuseless) Line 30 (Fused)
        /// </summary>
        public int Nseries { get; set; }

        /// <summary>
        /// Line 28
        /// </summary>
        public int NSeriesGroup { get; set; }

        /// <summary>
        /// Line 29
        /// </summary>
        public int NParalellGroup { get; set; }

        /// <summary>
        /// Determines if the Bank is Fused or Fuseless
        /// </summary>
        public bool Fused { get; set; }

        /// <summary>
        /// Line 37
        /// </summary>
        public double VTratioBus { get; set; }

        /// <summary>
        /// Line 38
        /// </summary>
        public int NumberLVCaps { get; set; }

        /// <summary>
        /// Line 39
        /// </summary>
        public int NumberLVUnits { get; set; }

        /// <summary>
        /// Line 40
        /// </summary>
        public double LVKVAr { get; set; }

        /// <summary>
        /// Line 41
        /// </summary>
        public double LVKV { get; set; }

        /// <summary>
        /// Line 42
        /// </summary>
        public double LVNegReactanceTol { get; set; }

        /// <summary>
        /// Line 43
        /// </summary>
        public double LVPosReactanceTol { get; set; }

        /// <summary>
        /// Line 48
        /// </summary>
        public double UpperXFRRatio { get; set; }

        /// <summary>
        /// Line 49
        /// </summary>
        public double LowerXFRRatio { get; set; }

        /// <summary>
        /// Line 50
        /// </summary>
        public int NLowerGroups { get; set; }

        /// <summary>
        /// Line 60
        /// </summary>
        public double Nshorted { get; set; }

        /// <summary>
        /// Line 62
        /// </summary>
        public int BlownFuses { get; set; }

        /// <summary>
        /// Line 63
        /// </summary>
        public int BlownGroups { get; set; }

        /// <summary>
        /// Line 64
        /// </summary>
        public double ShortedGroups { get; set; }


        /// <summary>
        /// Line 44 => 800 250
        /// </summary>
        public int RelayPTRatioSecondary { get; set; }

        /// <summary>
        /// Line 44 => 800 250
        /// </summary>
        public int RelayPTRatioPrimary { get; set; }
        /// <summary>
        /// Line 45
        /// </summary>
        public double Rv { get; set; }

        /// <summary>
        /// Line 46
        /// </summary>
        public double Rh { get; set; }

        /// <summary>
        /// Wattage for Rh 
        /// </summary>
        public double Sh { get; set; }

        /// <summary>
        /// Compensated => used for Line 55
        /// </summary>
        public bool Compensated { get; set; }

        /// <summary>
        ///  number of series capacitor groups from midstack VT position to ground (Fused only)
        /// </summary>
        public int NMidStackGround { get; set; }

        [JsonIgnore]
        [NonRecordField]
        public List<CapBankRelay> ConnectedRelays
        {
            get
            {
                return m_relays ?? (m_relays = QueryRelays());
            }
            set
            {
                m_relays = value;
            }

        }

        #endregion

        #region [ Methods ]

        private List<CapBankRelay> QueryRelays()
        {
            List<CapBankRelay> connectedRelays;

            using (AdoDataConnection connection = ConnectionFactory?.Invoke())
            {
                connectedRelays = GetRelays(connection)?
                    .Select(LazyContext.GetRelay)
                    .ToList();
            }

            if ((object)connectedRelays != null)
            {
                foreach (CapBankRelay relay in connectedRelays)
                {
                    relay.LazyContext = LazyContext;
                }
            }

            return connectedRelays;
        }


        public static CapBank DetailedCapBank (Asset asset, AdoDataConnection connection)
        {
            if ((object)connection == null)
                return null;

            TableOperations<CapBank> capBankTable = new TableOperations<CapBank>(connection);
            CapBank capBank = capBankTable.QueryRecordWhere("ID = {0}", asset.ID);
            capBank.LazyContext = asset.LazyContext;
            capBank.ConnectionFactory = asset.ConnectionFactory;

            return capBank;
        }

        public IEnumerable<CapBankRelay> GetRelays(AdoDataConnection connection)
        {
            if ((object)connection == null)
                return null;

            TableOperations<CapBankRelay> relayTable = new TableOperations<CapBankRelay>(connection);
            return relayTable.QueryRecordsWhere(@" ID in (
                (SELECT ChildID FROM AssetConnection LEFT JOIN Asset ON AssetConnection.ChildID = Asset.ID WHERE Asset.AssetTypeID = (SELECT ID FROM AssetType WHERE Name = 'CapacitorBankRelay') 
                    AND AssetConnection.ParentID = {0} )
                UNION 
                (SELECT ParentID FROM AssetConnection LEFT JOIN Asset ON AssetConnection.ParentID = Asset.ID WHERE Asset.AssetTypeID = (SELECT ID FROM AssetType WHERE Name = 'CapacitorBankRelay')
                    AND AssetConnection.ChildID = {0} ) )
           ", ID, ID);
        }


        public static CapBank DetailedCapBank(Asset asset)
        {
            return DetailedCapBank(asset, asset.ConnectionFactory.Invoke());
        }

        #endregion
    }
}
