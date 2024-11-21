//******************************************************************************************************
//  FunctionHelp.tsx - Gbtc
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
//  12/05/2020 - C. Lackner
//       Generated original version of source code.
//
//******************************************************************************************************

import * as React from 'react';
import * as ReactDOM from 'react-dom';
import _ from 'lodash';
import { StaticWizzard } from '../global';
import { ReactTable } from '@gpa-gemstone/react-table';
import { useSelector } from 'react-redux';
import { SelectWizardType } from '../Wizard/DynamicWizzardSlice';


interface IDocumentation { Name: string, Description: string, Example: string }

export const FunctionHelp = () => {
    const type = useSelector(SelectWizardType);

    const codeStyle = {
        borderRadius: '5px',
        padding: '5px',
        fontFamily: 'math',
        background: '#bfbfbf',
        border: '1px solid'
    }

    return <div className="modal" id='FunctionHelp' >
        <div className="modal-dialog" style={{ maxWidth: window.innerWidth - 200 }}>
            <div className="modal-content" >
                <div className="modal-header" >
                    <h4 className="modal-title" >Setpoint Computation Functions </h4>
                     <button type = "button" className = "close" data-dismiss="modal">&times; </button>
                </div>
                <div className="modal-body" style={{ maxHeight: 'calc(100vh - 210px)', overflowY: 'hidden', display: 'flex' }}>
                    <ReactTable.Table<IDocumentation>
                        TableClass="table table-striped"
                        Data={(type == 'dynamic' ? [...staticData, ...dynamicData] : staticData)}
                        SortKey={''}
                        Ascending={true}
                        OnSort={() => { /* do nothing */ }}
                        TableStyle={{ width: '100%', tableLayout: 'fixed', overflow: 'hidden', display: 'flex', flexDirection: 'column', flex: 1 }}
                        TheadStyle={{ fontSize: 'smaller', tableLayout: 'fixed', display: 'table', width: '100%' }}
                        TbodyStyle={{ display: 'block', overflowY: 'auto', flex: 1 }}
                        RowStyle={{ fontSize: 'smaller', display: 'table', tableLayout: 'fixed', width: '100%' }}
                        Selected={(item) => false}
                        KeySelector={item => item.Name}
                    >
                        <ReactTable.Column<IDocumentation>
                            Key="Name"
                            Field="Name"
                            AllowSort={false}
                            HeaderStyle={{ width: 'auto' }}
                            RowStyle={{ width: 'auto' }}
                            Content={row => (<p>{row.item.Name}</p>)}
                        > Function
                        </ReactTable.Column>
                        <ReactTable.Column<IDocumentation>
                            Key="Description"
                            Field="Description"
                            AllowSort={false}
                            HeaderStyle={{ width: 'auto' }}
                            RowStyle={{ width: 'auto' }}
                            Content={row => (<p>{row.item.Description}</p>)}
                        > Description
                        </ReactTable.Column>
                        <ReactTable.Column<IDocumentation>
                            Key="Example"
                            Field="Example"
                            AllowSort={false}
                            HeaderStyle={{ width: 'auto' }}
                            RowStyle={{ width: 'auto' }}
                            Content={row => (<p style={codeStyle}> {row.item.Example}</p>)}
                        > Example
                        </ReactTable.Column>
                    </ReactTable.Table>
                </div>

            </div>
        </div>
    </div>

}

const staticData = [
    { Name: 'Vbase', Description: 'The Base Voltage of the Channel  in kiloVolts (L-L)', Example: 'Vbase+5' },
    { Name: 'Xmin', Description: 'The Minimum Value during the Trend Data Interval  as Reported by the Field Device', Example: 'Mean(Xmin+5)' },
    { Name: 'Xavg', Description: 'The Average Value during the Trend Data Interval  as Reported by the Field Device', Example: 'Min(Xmin+5)' },
    { Name: 'Xmax', Description: 'The Maximum Value during the Trend Data Interval  as Reported by the Field Device', Example: 'Min(Xmin+5)' },
    { Name: 'Abs', Description: 'Gets the absolute value of the content', Example: 'Abs(Vbase)' },
    { Name: 'Min', Description: 'Finds the Minimum Value in the entire Dataset', Example: 'Min(Vbase)' },
    { Name: 'Max', Description: 'Finds the Maximum Value in the entire Dataset', Example: 'Max(Vbase*Xmin)' },
    { Name: 'Mean', Description: 'Finds the Average Value in the entire Dataset', Example: 'Mean(Vbase*Xmax)' },
    { Name: 'StDev', Description: 'Finds the Standard Deviation of the entire Dataset', Example: 'StDev(Xmin+5)' },
    { Name: 'ChannelMin', Description: 'Finds the Minimum Value in the entire Dataset for each channel separately', Example: 'ChannelMin(Vbase)' },
    { Name: 'ChannelMax', Description: 'Finds the Maximum Value in the entire Dataset for each channel separately', Example: 'ChannelMax(Vbase*Xmin)' },
    { Name: 'ChannelMean', Description: 'Finds the Average Value in the entire Dataset for each channel separately', Example: 'ChannelMean(Vbase*Xmax)' },
    { Name: 'ChannelStDev', Description: 'Finds the Standard Deviation of the entire Dataset for each channel separately', Example: 'ChannelStDev(Xmin+5)' },
] as IDocumentation[]

const dynamicData = [
    { Name: 'WindowMin', Description: 'Finds the Minimum Value in the selected timeframe', Example: 'WindowMin(Vbase*Xmax)' },
    { Name: 'WindowMax', Description: 'Finds the Maximum Value in the selected timeframe', Example: 'WindowMax(Vbase*Xmin)' },
    { Name: 'WindowMean', Description: 'Finds the Average Value in the selected timeframe', Example: 'WindowMean(Vbase*Xmax)' },
    { Name: 'WindowStDev', Description: 'Finds the Standard Deviation of the selected timeframe', Example: 'WindowStDev(Xmin+5)' },
] as IDocumentation[]