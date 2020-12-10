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
import Table from '@gpa-gemstone/react-table';
import { useSelector } from 'react-redux';
import { SelectWizardType } from '../Wizard/DynamicWizzardSlice';


interface IDocumentation { Name: string, Description: string, Example: string }

export const FunctionHelp = (props: {}) => {
    const type = useSelector(SelectWizardType);

    const codeStyle = {
        borderRadius: '5px',
        padding: '5px',
        fontFamily: 'math',
        background: '#bfbfbf',
        border: '1px solid'
    }

    return <div className="modal modal-xl" id='FunctionHelp' >
        <div className="modal-dialog" >
            <div className="modal-content" >
                <div className="modal-header" >
                    <h4 className="modal-title" >Setpoint Computation Functions </h4>
                     <button type = "button" className = "close" data-dismiss="modal">&times; </button>
                </div>
                <div className="modal-body" >
                    <Table<IDocumentation> cols={[
                        { key: 'Name', label: 'Function', headerStyle: { width: 'auto' }, rowStyle: { width: 'auto' }, content: (item, key) => <p> {item.Name}</p> },
                        { key: 'Description', label: 'Description', headerStyle: { width: 'auto' }, rowStyle: { width: 'auto' }, content: (item, key) => <p> {item.Description}</p> },
                        { key: 'Example', label: 'Example', headerStyle: { width: 'auto' }, rowStyle: { width: 'auto' }, content: (item, key) => <p style={codeStyle}> {item.Example}</p> },
                        
                    ]}
                        tableClass="table table-striped"
                        data={staticData}
                        sortField={''}
                        ascending={true}
                        onSort={() => { }}
                        onClick={(d) => { }}
                        theadStyle={{ fontSize: 'smaller', display: 'table', tableLayout: 'fixed', width: '100%' }}
                        tbodyStyle={{ display: 'block', overflowY: 'scroll', maxHeight: window.innerHeight - 300, width: '100%' }}
                        rowStyle={{ fontSize: 'smaller', display: 'table', tableLayout: 'fixed', width: '100%' }}
                        selected={(item) => false}
                    />
                </div>

            </div>
        </div>
    </div>

}

const staticData = [
    { Name: 'Vbase', Description: 'The Base Voltage of the Channel  in kiloVolts (L-L)', Example: 'Vbase+5' },
    { Name: 'Xmin', Description: 'The Minimum Value during the Trend Data Intervall as Reported by the Field Device', Example: 'Mean(Xmin+5)' },
    { Name: 'Xavg', Description: 'The Average Value during the Trend Data Intervall as Reported by the Field Device', Example: 'Min(Xmin+5)' },
    { Name: 'Xmax', Description: 'The Maximum Value during the Trend Data Intervall as Reported by the Field Device', Example: 'Min(Xmin+5)' },

    { Name: 'Min', Description: 'Finds the Minimum Value in the entire Dataset', Example: 'Min(Vbase)' },
    { Name: 'Max', Description: 'Finds the Maximum Value in the entire Dataset', Example: 'Max(Vbase*Xmin)' },
    { Name: 'Mean', Description: 'Finds the Average Value in the entire Datasete', Example: 'Mean(Vbase*Xmax)' },
    { Name: 'StDev', Description: 'Finds the Standard Deviation of the entire Dataset', Example: 'StDev(Xmin+5)' },
] as IDocumentation[]