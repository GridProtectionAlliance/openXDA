//******************************************************************************************************
//  RemoteXDAAsset.cs - Gbtc
//
//  Copyright © 2019, Grid Protection Alliance.  All Rights Reserved.
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
//  05/16/2022 - Gabriel Santos
//       Generated original version of source code.
//
//******************************************************************************************************

using GSF.Data.Model;

namespace openXDA.Model
{
    /// <summary>
    /// Model of a remote asset.
    /// </summary>
    [AllowSearch]
    [DeleteRoles("Administrator")]
    [TableName("AssetsToDataPush"), CustomView(@"
    SELECT
        AssetsToDataPush.ID,
        AssetsToDataPush.RemoteXDAInstanceID,
        AssetsToDataPush.LocalXDAAssetID,
        AssetsToDataPush.RemoteXDAAssetID,
        AssetsToDataPush.RemoteXDAAssetKey,
        AssetsToDataPush.Obsfucate,
        AssetsToDataPush.Synced,
        AssetsToDataPush.RemoteAssetCreatedByDataPusher,
        Asset.AssetName as LocalAssetName,
        Asset.AssetKey as LocalAssetKey
    FROM
        AssetsToDataPush LEFT JOIN
        Asset ON AssetsToDataPush.LocalXDAAssetID = [Asset].ID")]
    public class RemoteXDAAsset : AssetsToDataPush
    {
        public string LocalAssetName { get; set; }
        public string LocalAssetKey { get; set; }
    }
}