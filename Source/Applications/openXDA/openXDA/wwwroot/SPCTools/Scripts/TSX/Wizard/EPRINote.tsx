//******************************************************************************************************
//  EPRINote.tsx - Gbtc
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
//  12/12/2020 - C. Lackner
//       Generated original version of source code.
//
//******************************************************************************************************

import * as React from 'react';
import { SPCTools, DynamicWizzard } from '../global';
import Table from '@gpa-gemstone/react-table';
import { DateRangePicker } from '@gpa-gemstone/react-forms';
import { useSelector, useDispatch } from 'react-redux';
import _, { cloneDeep } from 'lodash';
import TrendingCard, { ITrendSeries } from '../CommonComponents/Graph';
import { selectAlarmGroup, selectSeriesTypeID, SelectAlarmFactors, SelectStatisticsFilter, SelectStatisticsChannels, SelectStatisticsrange, SelectAllAlarmValues } from './DynamicWizzardSlice';
import { SelectAffectedChannels } from '../store/WizardAffectedChannelSlice';
import { SelectSeverities } from '../store/SeveritySlice';
import { AlarmTrendingCard } from './AlarmTrendingCard';

declare var homePath: string;
declare var apiHomePath: string;

declare var userIsAdmin: boolean;

interface IProps { }

const EPRINote = (props: IProps) => {

    return (
        <div style={{ width: '100%', height: '100%' }}>
            <div className="row" style={{ margin: 0 }}>
                <div className="col">
                    {/*<img src="/Images/GPA-Logo.png" className="img-fluid" alt="Grid Protection Alliance" style={{width: '30%'}} />*/}
                    <img src="/Images/EPRILogo.png" className="img-fluid" alt="EPRI" style={{ width: '60%' }}/>
                </div>
            </div>
            <div className="row" style={{ margin: 0 }}>
                <div className="col">
                    <h3> Dynamic Alarms are only available in the EPRI Propriatery Version of SPC Tools</h3>
                </div>
            </div>
            <div className="row" style={{ margin: 0 }}>
                <div className="col">
                    <p> For more information contact Tom Cooke (TCOOKE@epri.com), EPRI Support or GPA (info@GridProtectionAlliance.org).</p>
                </div>
            </div>
           
            
        </div>      
    );
}

export default EPRINote
