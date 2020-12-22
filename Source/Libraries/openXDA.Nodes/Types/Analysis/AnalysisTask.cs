//******************************************************************************************************
//  AnalysisTask.cs - Gbtc
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
//  12/15/2020 - Stephen C. Wills
//       Generated original version of source code.
//
//******************************************************************************************************

using openXDA.Model;

namespace openXDA.Nodes.Types.Analysis
{
    internal class AnalysisTask
    {
        public AnalysisTask(FileGroup fileGroup, Meter meter, int priority)
            : this(default, fileGroup, meter, priority)
        {
        }

        public AnalysisTask(int id, FileGroup fileGroup, Meter meter, int priority)
        {
            ID = id;
            FileGroup = fileGroup;
            Meter = meter;
            Priority = priority;
        }

        public int ID { get; }
        public FileGroup FileGroup { get; }
        public Meter Meter { get; }
        public int Priority { get; }
    }
}
