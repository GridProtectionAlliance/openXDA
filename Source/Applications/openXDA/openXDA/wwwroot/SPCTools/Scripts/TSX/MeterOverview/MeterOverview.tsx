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
import FilterObject from '../CommonComponents/Filter';
import { SPCTools, openXDA, Filter } from '../global';
import Table from '@gpa-gemstone/react-table';
import { useSelector, useDispatch } from 'react-redux';
import { SortMeters, FilterMeters, SelectMeters, SelectMetersFilters, SelectMetersStatus, SelectMetersSortField, SelectMetersAscending, FetchMeters } from '../store/MeterSlice';
import { ChangeStatusMeterAlarmGroups, SortMeterAlarmGroups, SelectMeterAlarmGroups, SelectMeterAlarmGroupsStatus, SelectMeterAlarmGroupsSortField, SelectMeterAlarmGroupsAscending, FetchMeterAlarmGroups } from '../store/MeterAlarmGroupSlice';


const MeterOverview: React.FunctionComponent = (props: {}) => {
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

    let searchCollumns = [
        { label: 'Meter', key: 'Name', type: 'string'},
        { label: 'Make', key: 'Make', type: 'string' },
        { label: 'Model', key: 'Make', type: 'string' },
    ] as Filter.IField<openXDA.IMeter>[];

    return (
        <div style={{ width: '100%', height: '100%' }}>
            <FilterObject<openXDA.IMeter> Id='Filter' CollumnList={searchCollumns} SetFilter={(filter) => dispatch(FilterMeters(filter))} Direction='right' />
            <div style={{ width: '100%' }}>
                <div className="row" style={{ margin: 0 }}>
                    <div className="col" style={{ height: 'calc( 100% - 136px)', padding: 0, marginLeft: '10px' }}>
                        <Table<openXDA.IMeter>
                            cols={[
                                { key: 'Name', label: 'Meter', headerStyle: { width: 'auto' }, rowStyle: { width: 'auto' } },
                                { key: 'Make', label: 'Make', headerStyle: { width: 'auto' }, rowStyle: { width: 'auto' } },
                                { key: 'Model', label: 'Model', headerStyle: { width: 'auto' }, rowStyle: { width: 'auto' } },
                            ]}
                            tableClass="table table-hover"
                            data={meters}
                            sortField={mSort}
                            ascending={mAsc}
                            onSort={(d) => {
                                if (d.col == mSort)
                                    dispatch(SortMeters({ SortField: mSort, Ascending: !mAsc }));
                                else
                                    dispatch(SortMeters({ SortField: d.col, Ascending: mAsc }));
                            }}
                            onClick={(d) => setMeterID(d.row.ID)}
                            theadStyle={{ fontSize: 'smaller', display: 'table', tableLayout: 'fixed', width: '100%' }}
                            tbodyStyle={{ display: 'block', overflowY: 'scroll', maxHeight: window.innerHeight - 190, width: '100%' }}
                            rowStyle={{ fontSize: 'smaller', display: 'table', tableLayout: 'fixed', width: '100%' }}
                            selected={(item) => item.ID == MeterID}
                        />
                    </div>

                    <div className="col" style={{ height: 'calc( 100% - 136px)', padding: 0 }}>
                        <Table<SPCTools.IMeterAlarmGroup>
                            cols={[
                                { key: 'Name', label: 'AlarmGroup', headerStyle: { width: 'auto' }, rowStyle: { width: 'auto' } },
                                { key: 'AlarmSeverity', label: 'Severity', headerStyle: { width: 'auto' }, rowStyle: { width: 'auto' } },
                                { key: 'Channel', label: 'Num. of Channels', headerStyle: { width: 'auto' }, rowStyle: { width: 'auto' } },
                                { key: 'TimeInAlarm', label: 'Time In Alarm', headerStyle: { width: 'auto' }, rowStyle: { width: 'auto' } },
                            ]}
                            tableClass="table table-hover"
                            data={alarmGroups}
                            sortField={agSort}
                            ascending={agAsc}
                            onSort={(d) => {
                                if (d.col == agSort)
                                    dispatch(SortMeterAlarmGroups({ SortField: agSort, Ascending: !agAsc }));
                                else
                                    dispatch(SortMeterAlarmGroups({ SortField: d.col, Ascending: agAsc }));
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

export default MeterOverview