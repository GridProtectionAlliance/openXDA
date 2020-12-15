//******************************************************************************************************
//  Requirements.tsx - Gbtc
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
//  12/03/2020 - C. Lackner
//       Generated original version of source code.
//
//******************************************************************************************************

import * as React from 'react';
import * as ReactDOM from 'react-dom';
import _ from 'lodash';
import { StaticWizzard } from '../global';


export const Requirements = (props: StaticWizzard.IRequirement) => {
    return <div>
        <p style={{ width: '350px'}}>
            {props.complete == 'complete' ? <i style={{ marginRight: '10px', color: '#28A745' }} className="fa fa-check-circle"></i> : (props.complete == 'required' ? <i style={{ marginRight: '10px', color: '#dc3545' }} className="fa fa-exclamation-circle"></i> : <i style={{ marginRight: '10px', color: '#ffc107' }} className="fa fa-exclamation-triangle"></i>)}
            {props.text}
        </p>
    </div>
}