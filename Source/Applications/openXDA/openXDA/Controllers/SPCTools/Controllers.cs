//******************************************************************************************************
//  Controllers.cs - Gbtc
//
//  Copyright © 2020, Grid Protection Alliance.  All Rights Reserved.
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
//  12/03/2020 - Billy Ernest
//       Generated original version of source code.
//
//******************************************************************************************************

using openXDA.Model;
using System.Web.Http;

namespace openXDA.Controllers
{
    [RoutePrefix("api/SPCTools/AlarmGroupView")]
    public class AlarmGroupViewController : ModelController<AlarmGroupView>
    {
        protected override bool ViewOnly => true;
        protected override bool AllowSearch => true;
    }

    [RoutePrefix("api/SPCTools/ChannelOverview")]
    public class ChannelOverviewController : ModelController<ChannelOverview>
    {
        protected override bool ViewOnly => true;
        protected override bool AllowSearch => true;
    }

    [RoutePrefix("api/SPCTools/ChannelAlarmGroup")]
    public class ChannelAlarmGroupController : ModelController<ChannelAlarmGroup>
    {
        protected override bool ViewOnly => true;
        protected override bool AllowSearch => false;
        protected override bool HasParent => true;
        protected override string ParentKey => "ChannelID";
    }



}
