//******************************************************************************************************
//  SPCTools.tsx - Gbtc
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
//  10/20/2020 - C. Lackner
//       Generated original version of source code.
//
//******************************************************************************************************

import * as React from 'react';
import * as ReactDOM from 'react-dom';
import Filter, { FieldType } from '../CommonComponents/Filter';
import { SPCTools } from '../global';
import Table from '@gpa-gemstone/react-table';

const AlarmGroupHome: React.FunctionComponent = (props: {}) => {
    const [filters, setFilters] = React.useState<Array<any>>([]);
    const [alarmGroupList, setAlarmGroupList] = React.useState<Array<SPCTools.IAlarmGroupView>>([]);
    const [sort, setSort] = React.useState < keyof SPCTools.IAlarmGroupView>("Name");
    const [asc, setAsc] = React.useState<boolean>(false);

    React.useEffect(() => {
        let handle = getList();
        
        return () => {
            if (handle != undefined && handle.abort != undefined)
                handle.abort();
        }
    }, [filters, sort, asc])

    function getList(): JQuery.jqXHR<Array<SPCTools.IAlarmGroupView>> {
        let handle = $.ajax({
            type: "POST",
            url: `${apiHomePath}api/SPCTools/AlarmGroupView/SearchableList`,
            contentType: "application/json; charset=utf-8",
            dataType: 'json',
            data: JSON.stringify({ Searches: filters, OrderBy: sort, Ascending: asc }),
            cache: false,
            async: true
        });

        handle.done((data: Array<SPCTools.IAlarmGroupView>) => {
            setAlarmGroupList(data)
        });

        return handle;
    }



    let searchCollumns = [{ label: 'Name', key: 'Name' as keyof SPCTools.IAlarmGroupView, type: 'string' as FieldType }]
    return (
        <div style={{ width: '100%', height: '100%' }}>
            <Filter<SPCTools.IAlarmGroupView> Id='Filter' CollumnList={searchCollumns} SetFilter={setFilters} Direction='right'/>
            <div style={{ width: '100%' }}>
                <div className="row" style={{ margin: 0 }}>
                <div className="col-8" style={{height: 'calc( 100% - 136px)', padding: 0, marginLeft: '10px' }}>
                    <Table<SPCTools.IAlarmGroupView>
                        cols={[
                            { key: 'Name', label: 'Alarm Group', headerStyle: { width: 'auto' }, rowStyle: { width: 'auto' } },
                            { key: 'Meters', label: 'Number of Meter', headerStyle: { width: 'auto' }, rowStyle: { width: 'auto' } },
                            { key: 'Channels', label: 'Number of Channels', headerStyle: { width: 'auto' }, rowStyle: { width: 'auto' } },
                            { key: 'AlarmSeverity', label: 'Severity', headerStyle: { width: 'auto' }, rowStyle: { width: 'auto' } },    
                            { key: null, label: 'Time in Alarm', headerStyle: { width: 'auto' }, rowStyle: { width: 'auto' }, content: () => 'N/A' },
                        ]}
                        tableClass="table table-hover"
                        data={alarmGroupList}
                        sortField={sort}
                        ascending={asc}
                        onSort={(d) => {
                            if (d.col == sort) {
                                setAsc(!asc);
                            }
                            else {
                                setAsc(asc);
                                setSort(d.col);
                            }
                        }}
                        onClick={(d) => {  }}
                        theadStyle={{ fontSize: 'smaller', display: 'table', tableLayout: 'fixed', width: '100%' }}
                        tbodyStyle={{ display: 'block', overflowY: 'scroll', maxHeight: window.innerHeight - 300, width: '100%' }}
                        rowStyle={{ fontSize: 'smaller', display: 'table', tableLayout: 'fixed', width: '100%' }}
                        selected={(item) => false }
                    />
                    </div>
                </div>
                <div className="col-4" style={{ height: 'calc( 100% - 136px)', padding: 0 }}>
                    {/* This is where we will put the details eventually */}
                </div>

            </div>
        </div>      
    );
}

export default AlarmGroupHome