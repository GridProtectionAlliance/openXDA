//******************************************************************************************************
//  Legend.tsx - Gbtc
//
//  Copyright © 2018, Grid Protection Alliance.  All Rights Reserved.
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
//  03/09/2018 - Billy Ernest
//       Generated original version of source code.
//
//******************************************************************************************************

import * as React from 'react';
import * as _ from "lodash";
import { PeriodicDataDisplay } from './global'

interface Props {
    data: PeriodicDataDisplay.Legend,
    callback: () => void
}
const Legend = (props: Props) => {
    if (props.data == null || Object.keys(props.data).length == 0) return null;

    let rows = Object.keys(props.data).sort().map(row => {
        return <Row key={row} label={row} color={props.data[row].color} enabled={props.data[row].enabled} callback={() => {
            props.data[row].enabled = !props.data[row].enabled;
            props.callback();
        }} />
    });

    return (
        <div>
            <table>
                <tbody>
                    {rows}
                </tbody>
            </table>
        </div>
    );
}

const Row = (props) => {
    return (
        <tr onClick={props.callback} style={{cursor: 'pointer'}}>
            <td>
                <div style={{ border: '1px solid #ccc', padding: '1px' }} >
                    <div style={{ width: ' 4px', height: '4px', border: '5px solid', borderColor: (props.enabled ? convertHex(props.color, 100) : convertHex(props.color, 50)), overflow: 'hidden' }} >
                    </div>
                </div>
            </td>
            <td>
                <span style={{color: props.color, fontSize: 'smaller', fontWeight: 'bold', whiteSpace: 'nowrap'}}>{props.label}</span>
            </td>
        </tr>
    );
}

function convertHex(hex, opacity) {
    hex = hex.replace('#', '');
    var r = parseInt(hex.substring(0, 2), 16);
    var g = parseInt(hex.substring(2, 4), 16);
    var b = parseInt(hex.substring(4, 6), 16);

    var result = 'rgba(' + r + ',' + g + ',' + b + ',' + opacity / 100 + ')';
    return result;
}

export default Legend;