//******************************************************************************************************
//  MeterOverview.tsx - Gbtc
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
import { SPCTools, openXDA } from '../global';
import { ReactTable } from '@gpa-gemstone/react-table';
import { useSelector, useDispatch } from 'react-redux';
import { SortMeters, FilterMeters, SelectMeters, SelectMetersFilters, SelectMetersStatus, SelectMetersSortField, SelectMetersAscending, FetchMeters } from '../store/MeterSlice';
import { ChangeStatusMeterAlarmGroups, SortMeterAlarmGroups, SelectMeterAlarmGroups, SelectMeterAlarmGroupsStatus, SelectMeterAlarmGroupsSortField, SelectMeterAlarmGroupsAscending, FetchMeterAlarmGroups } from '../store/MeterAlarmGroupSlice';
import { SearchBar } from '@gpa-gemstone/react-interactive'


const MeterOverview: React.FunctionComponent = () => {
    const dispatch = useDispatch();

    const meters = useSelector(SelectMeters);
    const mStatus = useSelector(SelectMetersStatus);
    const filters = useSelector(SelectMetersFilters);
    const mSort = useSelector(SelectMetersSortField);
    const mAsc = useSelector(SelectMetersAscending);

    const alarmGroups = useSelector(SelectMeterAlarmGroups);
    const agStatus = useSelector(SelectMeterAlarmGroupsStatus);
    const agSort = useSelector(SelectMeterAlarmGroupsSortField);
    const agAsc = useSelector(SelectMeterAlarmGroupsAscending);

    const [MeterID, setMeterID] = React.useState<number>(0);

    React.useEffect(() => {
        if (mStatus != 'unitiated' && mStatus != 'changed') return;
        dispatch(FetchMeters({ filter: filters, sort: mSort, ascending: mAsc }));
    }, [dispatch, mStatus]);

    React.useEffect(() => {
        if (agStatus != 'unitiated' && agStatus != 'changed') return;
        dispatch(FetchMeterAlarmGroups({ MeterID: MeterID, sort: agSort, ascending: agAsc }));
    }, [dispatch, agStatus]);

    React.useEffect(() => {
        dispatch(ChangeStatusMeterAlarmGroups());
    }, [MeterID]);

    return (
        <div style={{ width: '100%', height: '100%' }}>
            <SearchBar<openXDA.IMeter>
                CollumnList={[
                    { label: 'Meter', key: 'Name', type: 'string', isPivotField: false },
                    { label: 'Make', key: 'Make', type: 'string', isPivotField: false },
                    { label: 'Model', key: 'Make', type: 'string', isPivotField: false },
                ]}
                SetFilter={(filter) => dispatch(FilterMeters(filter))}
                Direction='left'
            >
            </SearchBar>
            <div style={{ width: '100%' }}>
                <div className="row" style={{ margin: 0 }}>
                    <div className="col" style={{ height: 'calc( 100% - 136px)', padding: 0, marginLeft: '10px' }}>
                        <ReactTable.Table<openXDA.IMeter>
                            TableClass="table table-hover"
                            Data={meters}
                            SortKey={mSort}
                            Ascending={mAsc}
                            OnSort={(d) => {
                                if (d.colKey == mSort)
                                    dispatch(SortMeters({ SortField: mSort, Ascending: !mAsc }));
                                else
                                    dispatch(SortMeters({ SortField: d.colKey, Ascending: mAsc }));
                            }}
                            OnClick={(d) => setMeterID(d.row.ID)}
                            TheadStyle={{ fontSize: 'smaller', display: 'table', tableLayout: 'fixed', width: '100%' }}
                            TbodyStyle={{ display: 'block', overflowY: 'scroll', maxHeight: window.innerHeight - 190, width: '100%' }}
                            RowStyle={{ fontSize: 'smaller', display: 'table', tableLayout: 'fixed', width: '100%' }}
                            Selected={(item) => item.ID == MeterID}
                            KeySelector={item => item.ID}
                        >
                            <ReactTable.Column<openXDA.IMeter>
                                Key="Name"
                                Field="Name"
                                AllowSort={true}
                                HeaderStyle={{ width: 'auto' }}
                                RowStyle={{ width: 'auto' }}
                            > Meter
                            </ReactTable.Column>
                            <ReactTable.Column<openXDA.IMeter>
                                Key="Make"
                                Field="Make"
                                AllowSort={true}
                                HeaderStyle={{ width: 'auto' }}
                                RowStyle={{ width: 'auto' }}
                            > Make
                            </ReactTable.Column>
                            <ReactTable.Column<openXDA.IMeter>
                                Key="Model"
                                Field="Model"
                                AllowSort={true}
                                HeaderStyle={{ width: 'auto' }}
                                RowStyle={{ width: 'auto' }}
                            > Model
                            </ReactTable.Column>
                        </ReactTable.Table>
                    </div>
                    <div className="col" style={{ height: 'calc( 100% - 136px)', padding: 0 }}>
                        <ReactTable.Table<SPCTools.IMeterAlarmGroup>
                            TableClass="table table-hover"
                            Data={alarmGroups}
                            SortKey={agSort}
                            Ascending={agAsc}
                            OnSort={(d) => {
                                if (d.colKey == agSort)
                                    dispatch(SortMeterAlarmGroups({ SortField: agSort, Ascending: !agAsc }));
                                else
                                    dispatch(SortMeterAlarmGroups({ SortField: d.colKey, Ascending: agAsc }));
                            }}
                            TheadStyle={{ fontSize: 'smaller', display: 'table', tableLayout: 'fixed', width: '100%' }}
                            TbodyStyle={{ display: 'block', overflowY: 'scroll', maxHeight: window.innerHeight - 190, width: '100%' }}
                            RowStyle={{ fontSize: 'smaller', display: 'table', tableLayout: 'fixed', width: '100%' }}
                            Selected={(item) => false}
                            KeySelector={item => item.ID}
                        >
                            <ReactTable.Column<SPCTools.IMeterAlarmGroup>
                                Key="Name"
                                Field="Name"
                                AllowSort={true}
                                HeaderStyle={{ width: 'auto' }}
                                RowStyle={{ width: 'auto' }}
                            > Alarm Group
                            </ReactTable.Column>
                            <ReactTable.Column<SPCTools.IMeterAlarmGroup>
                                Key="AlarmSeverity"
                                Field="AlarmSeverity"
                                AllowSort={true}
                                HeaderStyle={{ width: 'auto' }}
                                RowStyle={{ width: 'auto' }}
                            > Severity
                            </ReactTable.Column>
                            <ReactTable.Column<SPCTools.IMeterAlarmGroup>
                                Key="Channel"
                                Field="Channel"
                                AllowSort={true}
                                HeaderStyle={{ width: 'auto' }}
                                RowStyle={{ width: 'auto' }}
                            > Num. of Channels
                            </ReactTable.Column>
                            <ReactTable.Column<SPCTools.IMeterAlarmGroup>
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

export default MeterOverview