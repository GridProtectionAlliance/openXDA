//******************************************************************************************************
//  ChannelOverview.tsx - Gbtc
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

import * as React from 'react';
import FilterObject from '../CommonComponents/Filter';
import { SPCTools, Filter } from '../global';
import Table from '@gpa-gemstone/react-table';
import { useSelector, useDispatch } from 'react-redux';
import { SortChannelOverviews, FilterChannelOverviews, SelectChannelOverviews, SelectChannelOverviewsFilters,SelectChannelOverviewsStatus, SelectChannelOverviewsSortField, SelectChannelOverviewsAscending, FetchChannelOverviews } from '../store/ChannelOverviewSlice';
import { ChangeStatusChannelAlarmGroups, SortChannelAlarmGroups, SelectChannelAlarmGroups, SelectChannelAlarmGroupsStatus, SelectChannelAlarmGroupsSortField, SelectChannelAlarmGroupsAscending, FetchChannelAlarmGroups } from '../store/ChannelAlarmGroupSlice';


const ChannelOverview: React.FunctionComponent = (props: {}) => {
    const dispatch = useDispatch();

    const channels = useSelector(SelectChannelOverviews);
    const coStatus = useSelector(SelectChannelOverviewsStatus);
    const filters = useSelector(SelectChannelOverviewsFilters);
    const coSort = useSelector(SelectChannelOverviewsSortField);
    const coAsc = useSelector(SelectChannelOverviewsAscending);

    const alarmGroups = useSelector(SelectChannelAlarmGroups);
    const agStatus = useSelector(SelectChannelAlarmGroupsStatus);
    const agSort = useSelector(SelectChannelAlarmGroupsSortField);
    const agAsc = useSelector(SelectChannelAlarmGroupsAscending);

    const [channelID, setChannelID] = React.useState<number>(0);

    React.useEffect(() => {
        if (coStatus != 'unitiated' && coStatus != 'changed') return;
        dispatch(FetchChannelOverviews({ filter: filters, sort: coSort, ascending: coAsc }));
    }, [dispatch, coStatus]);

    React.useEffect(() => {
        if (agStatus != 'unitiated' && agStatus != 'changed') return;
        dispatch(FetchChannelAlarmGroups({ channelID: channelID, sort: agSort, ascending: agAsc }));
    }, [dispatch, agStatus]);

    React.useEffect(() => {
        dispatch(ChangeStatusChannelAlarmGroups());
    }, [channelID]);

    let searchColumns = [
        { label: 'Meter', key: 'Meter', type: 'string' },
        { label: 'Channel', key: 'Channel', type: 'string'},
        { label: 'Type', key: 'Type', type: 'string' },
        { label: 'Phase', key: 'Phase', type: 'string'},
        { label: 'Asset', key: 'Asset', type: 'string'},
    ] as Filter.IField<SPCTools.IChannelOverview>[];

    return (
        <div style={{ width: '100%', height: '100%' }}>
            <FilterObject<SPCTools.IChannelOverview> Id='Filter' CollumnList={searchColumns} SetFilter={(filter) => dispatch(FilterChannelOverviews(filter))} Direction='right'/>
            <div style={{ width: '100%' }}>
                <div className="row" style={{ margin: 0 }}>
                    <div className="col" style={{height: 'calc( 100% - 136px)', padding: 0, marginLeft: '10px' }}>
                        <Table<SPCTools.IChannelOverview>
                            cols={[
                                { key: 'Meter', label: 'Meter', headerStyle: { width: 'auto' }, rowStyle: { width: 'auto' } },
                                { key: 'Channel', label: 'Channel', headerStyle: { width: 'auto' }, rowStyle: { width: 'auto' } },
                                { key: 'Type', label: 'Type', headerStyle: { width: 'auto' }, rowStyle: { width: 'auto' } },
                                { key: 'Phase', label: 'Phase', headerStyle: { width: 'auto' }, rowStyle: { width: 'auto' } }, 
                                { key: 'Asset', label: 'Asset', headerStyle: { width: 'auto' }, rowStyle: { width: 'auto' } },    
                            ]}
                            tableClass="table table-hover"
                            data={channels}
                            sortField={coSort}
                            ascending={coAsc}
                            onSort={(d) => {
                                if (d.col == coSort)
                                    dispatch(SortChannelOverviews({ SortField: coSort, Ascending: !coAsc }));
                                else
                                    dispatch(SortChannelOverviews({ SortField: d.col, Ascending: coAsc }));
                            }}
                            onClick={(d) => setChannelID(d.row.ID)}
                            theadStyle={{ fontSize: 'smaller', display: 'table', tableLayout: 'fixed', width: '100%' }}
                            tbodyStyle={{ display: 'block', overflowY: 'scroll', maxHeight: window.innerHeight - 190, width: '100%' }}
                            rowStyle={{ fontSize: 'smaller', display: 'table', tableLayout: 'fixed', width: '100%' }}
                            selected={(item) => item.ID == channelID }
                        />
                    </div>

                    <div className="col" style={{ height: 'calc( 100% - 136px)', padding: 0 }}>
                        <Table<SPCTools.IChannelAlarmGroup>
                            cols={[
                                { key: 'Name', label: 'AlarmGroup', headerStyle: { width: 'auto' }, rowStyle: { width: 'auto' } },
                                { key: 'AlarmSeverity', label: 'Severity', headerStyle: { width: 'auto' }, rowStyle: { width: 'auto' } },
                                { key: 'TimeInAlarm', label: 'Time In Alarm', headerStyle: { width: 'auto' }, rowStyle: { width: 'auto' } },
                            ]}
                            tableClass="table table-hover"
                            data={alarmGroups}
                            sortField={agSort}
                            ascending={agAsc}
                            onSort={(d) => {
                                if (d.col == agSort)
                                    dispatch(SortChannelAlarmGroups({ SortField: agSort, Ascending: !agAsc }));
                                else
                                    dispatch(SortChannelAlarmGroups({ SortField: d.col, Ascending: agAsc }));
                            }}
                            onClick={(d) => { }}
                            theadStyle={{ fontSize: 'smaller', display: 'table', tableLayout: 'fixed', width: '100%' }}
                            tbodyStyle={{ display: 'block', overflowY: 'scroll', maxHeight: window.innerHeight - 190, width: '100%' }}
                            rowStyle={{ fontSize: 'smaller', display: 'table', tableLayout: 'fixed', width: '100%' }}
                            selected={(item) => false}
                        />

                    </div>
                </div>

            </div>
        </div>      
    );
}

export default ChannelOverview