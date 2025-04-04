﻿//******************************************************************************************************
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
import { SPCTools } from '../global';
import { useSelector, useDispatch } from 'react-redux';
import moment from 'moment';
import { LoadWizard } from '../Wizard/DynamicWizzardSlice';
import { SearchBar, Search, ToolTip } from '@gpa-gemstone/react-interactive'
import { Paging } from '@gpa-gemstone/react-table';
import { AlarmGroupViewSlice } from '../store/store'
import { ReactTable } from '@gpa-gemstone/react-table';

interface IDetailRow { Content: string, Value: string}

const AlarmGroupHome = (props: { loadAlarm: () => void }) => {
    const dispatch = useDispatch();

    const currentPage = useSelector(AlarmGroupViewSlice.CurrentPage);

    const data = useSelector(AlarmGroupViewSlice.SearchResults);
    const cState = useSelector(AlarmGroupViewSlice.PagedStatus);

    const allPages = useSelector(AlarmGroupViewSlice.TotalPages);
    const totalRecords = useSelector(AlarmGroupViewSlice.TotalRecords);

    const [selectedAlarmGroup, setSelectedAlarmGroup] = React.useState<SPCTools.IAlarmGroupView>(null);
    const [detailedData, setDetailedData] = React.useState<IDetailRow[]>([]);

    const filterableList: Search.IField<SPCTools.IAlarmGroupView>[] = [
        { label: 'Name', key: 'Name', type: 'string', isPivotField: false },
        { label: 'Number of Meters', key: 'Meters', type: 'integer', isPivotField: true },
        { label: 'Number of Channels', key: 'Channels', type: 'integer', isPivotField: true },
        { label: 'Severity', key: 'AlarmSeverity', type: 'string', isPivotField: true }
    ]
    const [search, setSearch] = React.useState<Array<Search.IFilter<SPCTools.IAlarmGroupView>>>([]);

    const [sortKey, setSortKey] = React.useState<keyof SPCTools.IAlarmGroupView>('Name');
    const [ascending, setAscending] = React.useState<boolean>(true);
    const [page, setPage] = React.useState<number>(currentPage);

    const [hover, setHover] = React.useState<('None' | 'Edit')>('None');

    React.useEffect(() => {
        dispatch(AlarmGroupViewSlice.PagedSearch({ sortField: sortKey, ascending, filter: search, page }))
    }, [search, ascending, sortKey, page]);


    React.useEffect(() => {
        if (cState == 'unintiated' || cState == 'changed')
            dispatch(AlarmGroupViewSlice.PagedSearch({ sortField: sortKey, ascending, filter: search }))
    }, [cState, dispatch]);


    React.useEffect(() => {
        if (selectedAlarmGroup != undefined)
            setDetailedData([
                { Content: 'Last Alarm', Value: (selectedAlarmGroup.LastAlarmStart == undefined ? 'N/A' : moment(selectedAlarmGroup.LastAlarmStart).format("MM/DD/YYYY HH:mm"))},
                { Content: 'Status', Value: (selectedAlarmGroup.LastAlarmEnd == undefined && selectedAlarmGroup.LastAlarmStart != undefined ? 'In Alarm' : 'Not Active')},
                { Content: 'Duration', Value: FormatDifference()},
                { Content: 'Meter', Value: selectedAlarmGroup.LastMeter },
                { Content: 'Channel', Value: selectedAlarmGroup.LastChannel},
                { Content: 'Alarm Type', Value: selectedAlarmGroup.AlarmType},
            ])
        else
            setDetailedData([]);
    }, [selectedAlarmGroup])

    function FormatDifference() {
        const d = (selectedAlarmGroup.LastAlarmEnd == undefined ? moment() : moment(selectedAlarmGroup.LastAlarmEnd)).diff(selectedAlarmGroup.LastAlarmStart, 'days', true)
        const h = (d - Math.floor(d)) * 24.0;
        const m = (h - Math.floor(h)) * 60.0;
        return (d >= 1 ? (d.toFixed(0) + "d ") : "") + (h >= 1 ? (h.toFixed(0) + "h ") : "") + (m >= 1 ? (m.toFixed(0) + "m") : "");
    }

    return (
        <div className="container-fluid d-flex h-100 flex-column">
            <div className="row">
                <SearchBar<SPCTools.IAlarmGroupView> CollumnList={filterableList} SetFilter={(flds) => setSearch(flds)} Direction='left'
                    ShowLoading={cState == 'loading'}
                    ResultNote={cState == 'error' ? 'Could not complete Search' : ('Displaying  Alarm Group(s) ' + (50 * page + 1) + ' - ' + (50 * page + data.length)) + ' out of ' + totalRecords}
                />
            </div>
            <div className={'row'} style={{ flex: 1, overflow: 'hidden' }}>
                <div className="col-8" style={{paddingLeft: 0, height: '100%', display: 'flex', flexDirection: 'column'}}>
                    <ReactTable.Table<SPCTools.IAlarmGroupView>
                        TableClass="table table-hover"
                        Data={data}
                        SortKey={sortKey}
                        Ascending={ascending}
                        OnSort={d => {
                            if (d.colKey === sortKey)
                                setAscending(!ascending);
                            else {
                                setAscending(true);
                                setSortKey(d.colField);
                            }
                        }}
                        OnClick={d => { setSelectedAlarmGroup(d.row) }}
                        TableStyle={{ width: '100%', tableLayout: 'fixed', overflow: 'hidden', display: 'flex', flexDirection: 'column', flex: 1 }}
                        TheadStyle={{ fontSize: 'smaller', tableLayout: 'fixed', display: 'table', width: '100%' }}
                        TbodyStyle={{ display: 'block', overflowY: 'auto', flex: 1 }}
                        RowStyle={{ fontSize: 'smaller', display: 'table', tableLayout: 'fixed', width: '100%' }}
                        Selected={(item) => (selectedAlarmGroup == undefined ? false : selectedAlarmGroup.ID == item.ID)}
                        KeySelector={(item) => item.ID}
                    >
                        <ReactTable.Column<SPCTools.IAlarmGroupView>
                            Key={'Name'}
                            AllowSort={true}
                            Field={'Name'}
                            RowStyle={{ width: 'auto' }}
                        >
                        Name
                        </ReactTable.Column> 
                        <ReactTable.Column<SPCTools.IAlarmGroupView>
                            Key={'Meters'}
                            AllowSort={true}
                            Field={'Meters'}
                            RowStyle={{ width: 'auto' }}
                        >
                        Meters
                        </ReactTable.Column> 
                        <ReactTable.Column<SPCTools.IAlarmGroupView>
                            Key={'Channels'}
                            AllowSort={true}
                            Field={'Channels'}
                            RowStyle={{ width: 'auto' }}
                        >
                        Channels
                        </ReactTable.Column> 
                        <ReactTable.Column<SPCTools.IAlarmGroupView>
                            Key={'AlarmSeverity'}
                            AllowSort={true}
                            Field={'AlarmSeverity'}
                            RowStyle={{ width: 'auto' }}
                        >
                        Alarm Severity
                        </ReactTable.Column> 
                        <ReactTable.Column<SPCTools.IAlarmGroupView>
                            Key={'LastAlarmEnd'}
                            AllowSort={true}
                            Field={'LastAlarmEnd'}
                            RowStyle={{ width: 'auto' }}
                            Content={() => 'N/A'}
                        >
                        Time in Alarm
                        </ReactTable.Column> 
                    </ReactTable.Table>
                    <div className="row">
                        <div className="col">
                            <Paging Current={page + 1} Total={allPages} SetPage={(p) => setPage(p - 1)} />
                        </div>
                    </div>
                </div>
                <div className="col-4" style={{ height: '100%', padding: 0, display: 'flex', flexDirection: 'column' }}>
                    <ReactTable.Table<IDetailRow>
                        TableClass="table thead-dark table-striped"
                        Data={detailedData}
                        SortKey={'Content'}
                        Ascending={false}
                        OnSort={() => { /* do nothing */ }}
                        TableStyle={{ width: '100%', tableLayout: 'fixed', overflow: 'hidden', display: 'flex', flexDirection: 'column' }}
                        TheadStyle={{ fontSize: 'smaller', tableLayout: 'fixed', display: 'table', width: '100%' }}
                        TbodyStyle={{ display: 'block', overflowY: 'auto', flex: 1 }}
                        RowStyle={{ fontSize: 'smaller', display: 'table', tableLayout: 'fixed', width: '100%' }}
                        Selected={() => false}
                        KeySelector={(item) => item.Content}
                    >
                        <ReactTable.Column<IDetailRow>
                            Key={'Content'}
                            AllowSort={false}
                            Field={'Content'}
                            RowStyle={{ width: 'auto' }}
                        >
                            {'\u200B'}
                        </ReactTable.Column>
                        <ReactTable.Column<IDetailRow>
                            Key={'Value'}
                            AllowSort={false}
                            Field={'Value'}
                            RowStyle={{ width: 'auto' }}
                        >
                            {'\u200B'}
                        </ReactTable.Column>
                    </ReactTable.Table>
                    <div className="row">
                        <button type="button" className={`btn btn-info mr-2${(selectedAlarmGroup == null ? ' disabled' : '')}`}
                            onClick={() => { if (selectedAlarmGroup != undefined) dispatch(LoadWizard(selectedAlarmGroup.ID)); props.loadAlarm() }}
                            onMouseOver={() => setHover('Edit')} onMouseOut={() => setHover('None')} data-tooltip={'Edit'}>
                            Edit AlarmGroup
                        </button>
                    </div>
                    <ToolTip Position={'top'} Show={hover == 'Edit' && (selectedAlarmGroup == null)} Target={'Edit'}>
                        <p> An Alarm Group must be selected.</p>
                    </ToolTip>
                </div>
            </div>
        </div>

    );
}

export default AlarmGroupHome