﻿//******************************************************************************************************
//  MeterDataSet.cs - Gbtc
//
//  Copyright © 2014, Grid Protection Alliance.  All Rights Reserved.
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
//  05/13/2014 - Stephen C. Wills
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using FaultData.DataAnalysis;
using FaultData.DataResources;
using FaultData.DataSets.GTC;
using GSF.Data;
using GSF.Data.Model;
using openXDA.Model;

namespace FaultData.DataSets
{
    public class MeterDataSet : IDataSet
    {
        #region [ Constructors ]

        public MeterDataSet()
        {
            Resources = new Dictionary<Type, object>();
            DataSeries = new List<DataSeries>();
            Digitals = new List<DataSeries>();
            ReportedDisturbances = new List<ReportedDisturbance>();
            Triggers = new List<string>();
        }

        public MeterDataSet(Event evt)
        {
            Resources = new Dictionary<Type, object>();
            DataSeries = new List<DataSeries>();
            Digitals = new List<DataSeries>();
            ReportedDisturbances = new List<ReportedDisturbance>();

            using (AdoDataConnection connection = new AdoDataConnection("systemSettings"))
            {
                Meter = (new TableOperations<Meter>(connection)).QueryRecordWhere("ID = {0}", evt.MeterID);
                Meter.ConnectionFactory = () => new AdoDataConnection("systemSettings");
                CreateDbConnection = () => new AdoDataConnection("systemSettings");
                Configure = obj => { };
                FilePath = (new TableOperations<DataFile>(connection)).QueryRecordWhere("FileGroupID = {0}", evt.FileGroupID).FilePath;
                FileGroup = (new TableOperations<FileGroup>(connection)).QueryRecordWhere("ID = {0}", evt.FileGroupID);
                

                DataGroup dataGroup = ToDataGroup(ChannelData.DataFromEvent(evt.ID, CreateDbConnection));
                DataSeries = dataGroup.DataSeries.Where(ds => ds.SeriesInfo.Channel.MeasurementType.Name != "Digital").ToList();
                Digitals = dataGroup.DataSeries.Where(ds => ds.SeriesInfo.Channel.MeasurementType.Name == "Digital").ToList();
            }
        }

        #endregion

        #region [ Properties ]

        public Func<AdoDataConnection> CreateDbConnection { get; set; }
        public Action<object> Configure { get; set; }
        public string FilePath { get; set; }
        public FileGroup FileGroup { get; set; }
        public bool LoadHistoricConfiguration { get; set; }

        public Meter Meter { get; set; }
        public List<DataSeries> DataSeries { get; set; }
        public List<DataSeries> Digitals { get; set; }
        public List<ReportedDisturbance> ReportedDisturbances { get; set; }
        public List<string> Triggers { get; set; }

        private Dictionary<Type, object> Resources { get; set; }
        public BreakerRestrikeDataSet BreakerRestrikeDataSet { get; set; }
        public AnalysisDataSet AnalysisDataSet { get; set; }

        #endregion

        #region [ Methods ]

        public T GetResource<T>() where T : class, IDataResource, new()
        {
            return GetResource(() => new T());
        }

        public T GetResource<T>(Func<T> resourceFactory) where T : class, IDataResource
        {
            Type type;
            object obj;
            T resource;

            type = typeof(T);

            if (Resources.TryGetValue(type, out obj))
            {
                resource = (T)obj;
            }
            else
            {
                resource = resourceFactory();
                Configure(resource);
                resource.Initialize(this);
                Resources.Add(type, resource);
            }

            return resource;
        }

        private DataGroup ToDataGroup(List<byte[]> data)
        {
            DataGroup dataGroup = new DataGroup();
            dataGroup.FromData(Meter, data);
            return dataGroup;
        }

        #endregion
    }
}
