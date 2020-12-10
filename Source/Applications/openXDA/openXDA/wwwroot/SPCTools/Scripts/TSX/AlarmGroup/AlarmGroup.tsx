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
import FilterObject from '../CommonComponents/Filter';
import { SPCTools, Filter } from '../global';
import Table from '@gpa-gemstone/react-table';
import { useSelector, useDispatch } from 'react-redux';
import { SortAlarmGroups, FilterAlarmGroups, SelectAlarmGroups, SelectAlarmGroupsFilters, SelectAlarmGroupsStatus, SelectAlarmGroupsSortField, SelectAlarmGroupsAscending, FetchAlarmGroupViews } from '../store/AlarmGroupViewSlice';
import moment from 'moment';

interface IDetailRow {Content: string, Value: string}

const AlarmGroupHome: React.FunctionComponent = (props: {}) => {
    const dispatch = useDispatch();

    const alarmGroups = useSelector(SelectAlarmGroups);
    const agStatus = useSelector(SelectAlarmGroupsStatus);
    const filters = useSelector(SelectAlarmGroupsFilters);
    const agSort = useSelector(SelectAlarmGroupsSortField);
    const agAsc = useSelector(SelectAlarmGroupsAscending);
    const [selectedAlarmGroup, seSelectedAlarmGroup] = React.useState<SPCTools.IAlarmGroupView>(null);
    const [detailedData, setDetailedData] = React.useState<IDetailRow[]>([]);

    React.useEffect(() => {
        if (agStatus != 'unitiated' && agStatus != 'changed') return;
        dispatch(FetchAlarmGroupViews({ filter: filters, sort: agSort, ascending: agAsc }));
    }, [dispatch, agStatus]);


    React.useEffect(() => {
        if (selectedAlarmGroup != undefined)
            setDetailedData([
                { Content: 'Last Alarm', Value: (selectedAlarmGroup.LastAlarmStart == undefined ? 'N/A' : moment(selectedAlarmGroup.LastAlarmStart).format("MM/DD/YYYY HH:mm")) },
                { Content: 'Status', Value: (selectedAlarmGroup.LastAlarmEnd == undefined && selectedAlarmGroup.LastAlarmStart != undefined ? 'In Alarm' : 'Not Active') },
                { Content: 'Duration', Value: FormatDifference() },
                { Content: 'Meter', Value: selectedAlarmGroup.LastMeter },
                { Content: 'Channel', Value: selectedAlarmGroup.LastChannel },
                { Content: 'Alarm Type', Value: selectedAlarmGroup.AlarmType },
            ])
        else
            setDetailedData([]);
    }, [selectedAlarmGroup])

    function FormatDifference() {
        let d = (selectedAlarmGroup.LastAlarmEnd == undefined ? moment() : moment(selectedAlarmGroup.LastAlarmEnd)).diff(selectedAlarmGroup.LastAlarmStart, 'days', true)
        let h = (d - Math.floor(d))*24.0;
        let m = (h - Math.floor(h)) * 60.0;
        return (d >= 1 ? (d.toFixed(0) + "d ") : "") + (h >= 1 ? (h.toFixed(0) + "h ") : "") + (m >= 1 ? (m.toFixed(0) + "m") : "");
    }

    let searchColumns = [
        { label: 'Name', key: 'Name', type: 'string' },
        { label: 'Number of Meters', key: 'Meters', type: 'integer' },
        { label: 'Number of Channels', key: 'Channels', type: 'integer' },
        { label: 'Severity', key: 'AlarmSeverity', type: 'string' },

    ] as Filter.IField<SPCTools.IAlarmGroupView>[]
    return (
        <div style={{ width: '100%', height: '100%' }}>
            <FilterObject<SPCTools.IAlarmGroupView> Id='Filter' CollumnList={searchColumns} SetFilter={(filter) => dispatch(FilterAlarmGroups(filter))} Direction='left'/>
            <div style={{ width: '100%' }}>
                <div className="row" style={{ margin: 0 }}>
                        <div className="col-8">
                            <Table<SPCTools.IAlarmGroupView>
                                cols={[
                                    { key: 'Name', label: 'Alarm Group', headerStyle: { width: 'auto' }, rowStyle: { width: 'auto' } },
                                    { key: 'Meters', label: 'Number of Meter', headerStyle: { width: 'auto' }, rowStyle: { width: 'auto' } },
                                    { key: 'Channels', label: 'Number of Channels', headerStyle: { width: 'auto' }, rowStyle: { width: 'auto' } },
                                    { key: 'AlarmSeverity', label: 'Severity', headerStyle: { width: 'auto' }, rowStyle: { width: 'auto' } },    
                                    { key: null, label: 'Time in Alarm', headerStyle: { width: 'auto' }, rowStyle: { width: 'auto' }, content: () => 'N/A' },
                                ]}
                                tableClass="table table-hover"
                                data={alarmGroups}
                                sortField={agSort}
                                ascending={agAsc}
                                onSort={(d) => {
                                    if (d.col == agSort)
                                        dispatch(SortAlarmGroups({ SortField: agSort, Ascending: !agAsc }));
                                    else
                                        dispatch(SortAlarmGroups({ SortField: d.col, Ascending: agAsc }));
                                }}
                                onClick={(d) => { seSelectedAlarmGroup(d.row) }}
                                theadStyle={{ fontSize: 'smaller', display: 'table', tableLayout: 'fixed', width: '100%' }}
                                tbodyStyle={{ display: 'block', overflowY: 'scroll', maxHeight: window.innerHeight - 300, width: '100%' }}
                                rowStyle={{ fontSize: 'smaller', display: 'table', tableLayout: 'fixed', width: '100%' }}
                                selected={(item) => (selectedAlarmGroup == undefined? false : selectedAlarmGroup.ID == item.ID) }
                            />
                        </div>

                    <div className="col-4" style={{ height: 'calc( 100% - 136px)', padding: 0 }}>
                        <Table<IDetailRow>
                            cols={[
                                { key: 'Content', label: '', headerStyle: { width: 'auto' }, rowStyle: { width: 'auto' } },
                                { key: 'Value', label: '', headerStyle: { width: 'auto' }, rowStyle: { width: 'auto' } }
                            ]}
                            tableClass="table thead-dark table-striped"
                            data={detailedData}
                            sortField={'Content'}
                            ascending={false}
                            onSort={(d) => { }}
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
             
    );
}

export default AlarmGroupHome