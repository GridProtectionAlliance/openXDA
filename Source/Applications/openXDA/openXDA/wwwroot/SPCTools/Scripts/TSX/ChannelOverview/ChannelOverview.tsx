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
import { SPCTools } from '../global';
import { ReactTable } from '@gpa-gemstone/react-table';
import { useSelector, useDispatch } from 'react-redux';
import { SortChannelOverviews, FilterChannelOverviews, SelectChannelOverviews, SelectChannelOverviewsFilters, SelectChannelOverviewsStatus, SelectChannelOverviewsSortField, SelectChannelOverviewsAscending, FetchChannelOverviews } from '../store/ChannelOverviewSlice';
import { ChangeStatusChannelAlarmGroups, SortChannelAlarmGroups, SelectChannelAlarmGroups, SelectChannelAlarmGroupsStatus, SelectChannelAlarmGroupsSortField, SelectChannelAlarmGroupsAscending, FetchChannelAlarmGroups } from '../store/ChannelAlarmGroupSlice';
import { SearchBar } from '@gpa-gemstone/react-interactive'


const ChannelOverview: React.FunctionComponent = () => {
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


    return (
        <div style={{ width: '100%', height: '100%' }}>
            <SearchBar<SPCTools.IChannelOverview>
                CollumnList={[
                    { label: 'Meter', key: 'Meter', type: 'string', isPivotField: false },
                    { label: 'Channel', key: 'Channel', type: 'string', isPivotField: false },
                    { label: 'Type', key: 'Type', type: 'string', isPivotField: false },
                    { label: 'Phase', key: 'Phase', type: 'string', isPivotField: false },
                    { label: 'Asset', key: 'Asset', type: 'string', isPivotField: false },
                ]}
                SetFilter={(filter) => dispatch(FilterChannelOverviews(filter))}
                Direction='left'
            />
            <div style={{ width: '100%' }}>
                <div className="row" style={{ margin: 0 }}>
                    <div className="col" style={{ height: 'calc( 100% - 136px)', padding: 0, marginLeft: '10px' }}>
                        <ReactTable.Table<SPCTools.IChannelOverview>
                            TableClass="table table-hover"
                            Data={channels}
                            SortKey={coSort}
                            Ascending={coAsc}
                            OnSort={(d) => {
                                if (d.colKey == coSort)
                                    dispatch(SortChannelOverviews({ SortField: coSort, Ascending: !coAsc }));
                                else
                                    dispatch(SortChannelOverviews({ SortField: d.colKey, Ascending: coAsc }));
                            }}
                            OnClick={(d) => setChannelID(d.row.ID)}
                            TheadStyle={{ fontSize: 'smaller', display: 'table', tableLayout: 'fixed', width: '100%' }}
                            TbodyStyle={{ display: 'block', overflowY: 'scroll', maxHeight: window.innerHeight - 190, width: '100%' }}
                            RowStyle={{ fontSize: 'smaller', display: 'table', tableLayout: 'fixed', width: '100%' }}
                            Selected={(item) => item.ID == channelID}
                            KeySelector={item => item.ID}
                        >
                            <ReactTable.Column<SPCTools.IChannelOverview>
                                Key="Meter"
                                Field="Meter"
                                AllowSort={true}
                                HeaderStyle={{ width: 'auto' }}
                                RowStyle={{ width: 'auto' }}
                            > Meter
                            </ReactTable.Column>
                            <ReactTable.Column<SPCTools.IChannelOverview>
                                Key="Type"
                                Field="Type"
                                AllowSort={true}
                                HeaderStyle={{ width: 'auto' }}
                                RowStyle={{ width: 'auto' }}
                            > Type
                            </ReactTable.Column>
                            <ReactTable.Column<SPCTools.IChannelOverview>
                                Key="Phase"
                                Field="Phase"
                                AllowSort={true}
                                HeaderStyle={{ width: 'auto' }}
                                RowStyle={{ width: 'auto' }}
                            > Phase
                            </ReactTable.Column>
                            <ReactTable.Column<SPCTools.IChannelOverview>
                                Key="Asset"
                                Field="Asset"
                                AllowSort={true}
                                HeaderStyle={{ width: 'auto' }}
                                RowStyle={{ width: 'auto' }}
                            > Asset
                            </ReactTable.Column>
                        </ReactTable.Table>
                    </div>
                    <div className="col" style={{ height: 'calc( 100% - 136px)', padding: 0 }}>
                        <ReactTable.Table<SPCTools.IChannelAlarmGroup>
                            TableClass="table table-hover"
                            Data={alarmGroups}
                            SortKey={agSort}
                            Ascending={agAsc}
                            OnSort={(d) => {
                                if (d.colKey == agSort)
                                    dispatch(SortChannelAlarmGroups({ SortField: agSort, Ascending: !agAsc }));
                                else
                                    dispatch(SortChannelAlarmGroups({ SortField: d.colKey, Ascending: agAsc }));
                            }}
                            TheadStyle={{ fontSize: 'smaller', display: 'table', tableLayout: 'fixed', width: '100%' }}
                            TbodyStyle={{ display: 'block', overflowY: 'scroll', maxHeight: window.innerHeight - 190, width: '100%' }}
                            RowStyle={{ fontSize: 'smaller', display: 'table', tableLayout: 'fixed', width: '100%' }}
                            Selected={(item) => false}
                            KeySelector={item => item.ID}
                        >
                            <ReactTable.Column<SPCTools.IChannelAlarmGroup>
                                Key="Name"
                                Field="Name"
                                AllowSort={true}
                                HeaderStyle={{ width: 'auto' }}
                                RowStyle={{ width: 'auto' }}
                            > Alarm Group
                            </ReactTable.Column>
                            <ReactTable.Column<SPCTools.IChannelAlarmGroup>
                                Key="AlarmSeverity"
                                Field="AlarmSeverity"
                                AllowSort={true}
                                HeaderStyle={{ width: 'auto' }}
                                RowStyle={{ width: 'auto' }}
                            > Severity
                            </ReactTable.Column>
                            <ReactTable.Column<SPCTools.IChannelAlarmGroup>
                                Key="TimeInAlarm"
                                Field="TimeInAlarm"
                                AllowSort={true}
                                HeaderStyle={{ width: 'auto' }}
                                RowStyle={{ width: 'auto' }}
                            > Time In Alarm
                            </ReactTable.Column>
                        </ReactTable.Table>
                    </div>
                </div>

            </div>
        </div>
    );
}

export default ChannelOverview