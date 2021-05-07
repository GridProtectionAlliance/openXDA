//******************************************************************************************************
//  Transformer.cs - Gbtc
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

namespace openXDA.Model
{
    [MetadataType(typeof(Asset))]
    public class Transformer: Asset
    {
        #region [ Members ]
       
        #endregion

        #region [ Properties ]
       


        [Required]
        public double PrimaryVoltageKV { get; set; }

        [Required]
        public double SecondaryVoltageKV { get; set; }

        public double TertiaryVoltageKV { get; set; }

        public double PrimaryWinding { get; set; }

        public double SecondaryWinding { get; set; }

        public double TertiaryWinding { get; set; }

        public double Tap { get; set; }

        public double ThermalRating { get; set; }

        public double R0 { get; set; }

        public double R1 { get; set; }

        public double X0 { get; set; }

        public double X1 { get; set; }



        #endregion

        #region [ Methods ]


        public static Transformer DetailedTransformer(Asset asset, AdoDataConnection connection)
        {
            if ((object)connection == null)
                return null;

            TableOperations<Transformer> xfTable = new TableOperations<Transformer>(connection);
            Transformer xf = xfTable.QueryRecordWhere("ID = {0}", asset.ID);

            if (xf == null)
                return null;

            xf.LazyContext = asset.LazyContext;
            xf.ConnectionFactory = asset.ConnectionFactory;

            return xf;
        }

        public static Transformer DetailedTransformer(Asset asset)
        {
            return DetailedTransformer(asset, asset.ConnectionFactory.Invoke());
        }
        #endregion
    }
}
