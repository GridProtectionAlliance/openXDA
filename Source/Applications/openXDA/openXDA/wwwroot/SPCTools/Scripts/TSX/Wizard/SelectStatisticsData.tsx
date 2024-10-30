//******************************************************************************************************
//  SelectStatisticsData.tsx - Gbtc
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
import { SPCTools, openXDA } from '../global';
import { CheckBox, Input } from '@gpa-gemstone/react-forms';
import { TimeFilter } from '@gpa-gemstone/common-pages';
import { useSelector, useDispatch } from 'react-redux';
import _ from 'lodash';
import { SelectStatisticsrange, updateStatisticsRange, updateStatisticsFilter, updateStatisticChannels, SelectStatisticsFilter } from './DynamicWizzardSlice';
import { SelectAffectedChannelSortField, SelectAffectedChannelAscending, SelectAffectedChannels } from '../store/WizardAffectedChannelSlice';
import { SelectTable } from '@gpa-gemstone/react-table';

declare let homePath: string;
declare let apiHomePath: string;
declare let userIsAdmin: boolean;

//Todo
// # Removed Meter table for now due to EPRI Time contraints

const SelectStatisticsData = () => {
    const dispatch = useDispatch();

    const dateRange = useSelector(SelectStatisticsrange)
    const dataFilter = useSelector(SelectStatisticsFilter)

    return (
        <div style={{ width: '100%', height: '100%' }}>
            <div className="row" style={{ margin: 0 }}>
                <div className="col">
                    <h2>Select the Historical Data to use as the basis for creating Alarm Setpoints:</h2>
                </div>
            </div>
            <div className="row" style={{ margin: 0 }}>
                <div className="col-8">
                    <TimeFilter filter={{ start: dateRange.start, end: dateRange.end }} setFilter={(center: string, start: string, end: string) => {
                        dispatch(updateStatisticsRange({ start: start, end: end }));
                    }} showQuickSelect={true} timeZone={'UTC'} dateTimeSetting={'startEnd'} isHorizontal={true} format={"date"} />
                </div>
            </div>
            <div className="row" style={{ margin: 0 }}>
                <div className="col">
                    <h2>Determining Channels to be used as the basis for creating Alarm Setpoints:</h2>
                </div>
            </div>
            <div className="row" style={{ margin: 0 }}>
                {/*
                 This is removed for speeding up Development due to EPRI Deadline
                 <div className="col-4">
                    <div className="btn-group float-right" role="group">
                        <button type="button" className={"btn btn-primary" + (showChannels ? ' active' : '')} disabled={channelCount > 100} onClick={() => setShowChannels(true)}>Show Channels</button>
                        <button type="button" className={"btn btn-primary" + (showChannels ? '' : ' active')} disabled={channelCount < 10} onClick={() => setShowChannels(false)} >Show Meters</button>
                    </div>
                </div>*/}
            </div>
            <div className="row" style={{ margin: 0 }}>
                <div className="col-8">
                    {/*showChannels ? <ChannelTable selectAll={selectAll} /> : <MeterTable selectAll={selectAll} />*/}
                    <ChannelTable />
                </div>
                <div className="col-4">
                    <DataFilter filter={dataFilter} setter={(r) => dispatch(updateStatisticsFilter(r))} />
                </div>
            </div>
        </div>
    );
}

const DataFilter = (props: { filter: SPCTools.IDataFilter, setter: (record: SPCTools.IDataFilter) => void }) => {

    return (
        <>
            <fieldset className="border" style={{ padding: '10px' }}>
                <legend className="w-auto">Filter Data:</legend>
                <div className="form-group">
                    <div className="form-check form-check-inline">
                        <CheckBox
                            Record={props.filter}
                            Field="FilterZero"
                            Setter={props.setter}
                            Label="Remove 0"
                        />
                    </div>
                </div>
                <div className="form-group">
                    <div className="form-check form-check-inline">
                        <CheckBox
                            Record={props.filter}
                            Field="FilterLower"
                            Setter={props.setter}
                            Label="Remove <"
                        />
                        <Input
                            Record={props.filter}
                            Field="LowerLimit"
                            Setter={props.setter}
                            Valid={() => true}  
                            Type="number"
                            Label={''}
                        />
                    </div>
                </div>
                <div className="form-group">
                    <div className="form-check form-check-inline">
                        <CheckBox
                            Record={props.filter}
                            Field="FilterUpper"
                            Setter={props.setter}
                            Label="Remove >"
                        />
                        <Input
                            Record={props.filter}
                            Field="UpperLimit"
                            Setter={props.setter}
                            Valid={() => true} 
                            Type="number"
                            Label={''}
                        />
                    </div>
                </div>
            </fieldset>
        </>
    );
}

/*
const MeterTable = (props: { selectAll: boolean }) => {
    const allMeterIDs = useSelector((state: SPCTools.RootState) => state.StaticWizzard.AlarmGroup.MeterIDs)
    const dispatch = useDispatch();
    const allChannels = useSelector((state: SPCTools.RootState) => state.StaticWizzard.HistoricData.ChannelList)
    const [allMeters, setAllMeters] = React.useState<Array<SPCTools.IMeter>>([]);
    const [selectedMeter, setSelectedMeters] = React.useState<Array<SPCTools.IMeter>>([]);
    const [sort, setSort] = React.useState<keyof SPCTools.IMeter>('AssetKey');
    const [asc, setAsc] = React.useState<boolean>(false);
    const generalSettings = useSelector((state: SPCTools.RootState) => state.StaticWizzard.AlarmGroup)

    React.useEffect(() => {
        setAllMeters((old) => { return _.orderBy(old, [sort], [asc ? "asc" : "desc"]) })
    }, [sort, asc])

    React.useEffect(() => {
        let handles = [];
        if (allMeters.length == 0)
            handles = allMeterIDs.map(id => getMeter(id));

        return () => {
            handles.forEach(h => { if (h != undefined && h.abort != undefined) h.abort(); })
        }
    }, [props]);

    React.useEffect(() => {
        dispatch(removeChannel(allChannels.filter(item => selectedMeter.findIndex(m => m.ID == item.MeterID) == -1)));
        let handles = [];
        if (selectedMeter.length > 0)
            handles = selectedMeter.map(m => getChannels(m.ID));


    }, [selectedMeter])

    function getMeter(id: number): JQuery.jqXHR<string> {
        let handle = $.ajax({
            type: "GET",
            url: `${apiHomePath}api/MeterDetail/One/${id}`,
            contentType: "application/json; charset=utf-8",
            dataType: 'json',
            cache: false,
            async: true
        });

        handle.done(data => {
            setAllMeters((old) => [...old, ...JSON.parse(data)])
            
        })
        return handle;
    }


    function getChannels(Meterid: number): JQuery.jqXHR<Array<SPCTools.IMeter>> {

        let handle = $.ajax({
            type: "POST",
            url: `${apiHomePath}api/SPCTools/StaticAlarmCreation/Channel/${Meterid}`,
            contentType: "application/json; charset=utf-8",
            dataType: 'json',
            data: JSON.stringify(generalSettings),
            cache: false,
            async: true
        });

        handle.done(data => {
            dispatch(addChannel(data))
        });

        return handle;
    }

    return (
        <div style={{ width: '100%', height: '100%' }}>
            <MultiSelectTable<SPCTools.IMeter>
                cols={[
                    { key: 'Name', label: 'Name', headerStyle: { width: 'auto' }, rowStyle: { width: 'auto' } },
                    { key: 'Location', label: 'Substation', headerStyle: { width: 'auto' }, rowStyle: { width: 'auto' } },
                    { key: 'Make', label: 'Make', headerStyle: { width: 'auto' }, rowStyle: { width: 'auto' } },
                    { key: 'Model', label: 'Model', headerStyle: { width: 'auto' }, rowStyle: { width: 'auto' } },
                ]}
                tableClass="table table-hover"
                data={allMeters}
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
                onClick={(d) => { }}
                theadStyle={{ fontSize: 'smaller', display: 'table', tableLayout: 'fixed', width: '100%' }}
                tbodyStyle={{ display: 'block', overflowY: 'scroll', maxHeight: window.innerHeight - 300, width: '100%' }}
                rowStyle={{ fontSize: 'smaller', display: 'table', tableLayout: 'fixed', width: '100%' }}
                selected={(item) => false}
                primaryKey={'ID'}
                updateSelection={(selected) => setSelectedMeters(selected)}
                selectAll={props.selectAll}
                />
            </div>
        )
}
*/

const ChannelTable = () => {
    const dispatch = useDispatch();
    const channelList = useSelector(SelectAffectedChannels);
    const sort = useSelector(SelectAffectedChannelSortField);
    const asc = useSelector(SelectAffectedChannelAscending);

    const [selectAllCounter, setSelectAllCounter] = React.useState(0);
    const [allSelected, setAllSelected] = React.useState(false);
    const selectAllText = React.useMemo(() => allSelected ? "Deselect All" : "Select All", [allSelected]);

    return (
        <div style={{ width: '100%', height: '100%' }}>
            <a href="#" className="link-primary" onClick={() => setSelectAllCounter(selectAllCounter + 1)}>{selectAllText}</a>
            <SelectTable<openXDA.IChannel>
                cols={[
                    { key: 'MeterName', label: 'Meter', field: 'MeterName', headerStyle: { width: 'auto' }, rowStyle: { verticalAlign: 'middle', width: 'auto' } },
                    { key: 'AssetKey', label: 'Asset', field: 'AssetKey', headerStyle: { width: 'auto' }, rowStyle: { verticalAlign: 'middle', width: 'auto' } },
                    { key: 'Name', label: 'Channel', field: 'Name', headerStyle: { width: 'auto' }, rowStyle: { verticalAlign: 'middle', width: 'auto' } },
                    { key: 'Phase', label: 'Phase', field: 'Phase', headerStyle: { width: 'auto' }, rowStyle: { verticalAlign: 'middle', width: 'auto' } },
                ]}
                tableClass="table table-hover table-sm"
                data={channelList}
                sortKey={sort}
                ascending={asc}
                theadStyle={{ fontSize: 'smaller', display: 'table', tableLayout: 'fixed', width: '100%' }}
                tbodyStyle={{ display: 'block', overflowY: 'scroll', maxHeight: 400, width: '100%' }}
                rowStyle={{ fontSize: 'smaller', display: 'table', tableLayout: 'fixed', width: '100%' }}
                KeyField={'ID'}
                onSelection={(selected) => { setAllSelected(selected.length === channelList.length); dispatch(updateStatisticChannels(selected)); }}
                selectAllCounter={selectAllCounter}
            />
        </div>
    )
}

export default SelectStatisticsData