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
import { DateRangePicker } from '@gpa-gemstone/react-forms';
import MultiSelectTable from '../CommonComponents/MultiSelectTable';
import { useSelector, useDispatch } from 'react-redux';
import _ from 'lodash';
import { SelectStatisticsrange, updateStatisticsRange, updateStatisticsFilter, updateStatisticChannels, SelectStatisticsFilter } from './DynamicWizzardSlice';
import { SelectAffectedChannelSortField, SelectAffectedChannelAscending, SortAffectedChanels, SelectAffectedChannels } from '../store/WizardAffectedChannelSlice';

declare var homePath: string;
declare var apiHomePath: string;

declare var userIsAdmin: boolean;

interface IProps { }

//Todo
// # Removed Meter table for now due to EPRI Time contraints

const SelectStatisticsData = (props: IProps) => {
    const dispatch = useDispatch();

    const dateRange = useSelector(SelectStatisticsrange)
    const dataFilter = useSelector(SelectStatisticsFilter)
    
    const [selectAll, setSelectAll] = React.useState<boolean>(false);
   

    return (
        <div style={{ width: '100%', height: '100%' }}>
            <div className="row" style={{ margin: 0 }}>
                <div className="col">
                    <h2>Select the Historical Data to use as the basis for creating Alarm Setpoints:</h2>
                </div>
            </div>
            <div className="row" style={{ margin: 0 }}>
                <div className="col-6">
                    <DateRangePicker<SPCTools.IDateRange> Label={''} Record={dateRange} FromField={'start'} ToField={'end'} Setter={(r) => dispatch(updateStatisticsRange(r))} />
                </div>
            </div>
            <div className="row" style={{ margin: 0 }}>
                <div className="col">
                    <h2>Determining Channels to be used as the basis for creating Alarm Setpoints:</h2>
                </div>
            </div>
            <div className="row" style={{ margin: 0}}>
                <div className="col-4">
                    <div className="form-check">
                        <input type="checkbox" className="form-check-input" onChange={() => setSelectAll(!selectAll)} checked={selectAll} />
                        <label className="form-check-label">Select All Channels</label>
                    </div>
                </div>
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
                    <ChannelTable selectAll={selectAll} />
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
        <fieldset className="border" style={{ padding: '10px' }}>
            <legend className="w-auto">Filter Data:</legend>
            <div className="form-group">
                <div className="form-check form-check-inline">
                    <input className="form-check-input" type="checkbox" checked={props.filter.FilterZero} onChange={() => props.setter({ ...props.filter, FilterZero: !props.filter.FilterZero })}/>
                    <label className="form-check-label">Remove 0</label>
                </div>
            </div>
            <div className="form-group">
                <div className="form-check form-check-inline">
                    <input className="form-check-input" type="checkbox" checked={props.filter.FilterLower} onChange={() => props.setter({ ...props.filter, FilterLower: !props.filter.FilterLower })} />
                    <label className="form-check-label">Remove &lt; <input type="number" value={props.filter.LowerLimit} onChange={(evt) => props.setter({ ...props.filter, LowerLimit: parseFloat(evt.target.value) })} /></label>
                </div>
            </div>
            <div className="form-group">
                <div className="form-check form-check-inline">
                    <input className="form-check-input" type="checkbox" checked={props.filter.FilterUpper} onChange={() => props.setter({ ...props.filter, FilterUpper: !props.filter.FilterUpper })} />
                    <label className="form-check-label">Remove &gt; <input type="number" value={props.filter.UpperLimit} onChange={(evt) => props.setter({ ...props.filter, UpperLimit: parseFloat(evt.target.value) })} /></label>
                </div>
            </div>
        </fieldset>);
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

const ChannelTable = (props: { selectAll: boolean }) => {
    const dispatch = useDispatch();
    const channelList = useSelector(SelectAffectedChannels);
    const sort = useSelector(SelectAffectedChannelSortField);
    const asc = useSelector(SelectAffectedChannelAscending);

    return (
        <div style={{ width: '100%', height: '100%' }}>
            <MultiSelectTable<openXDA.IChannel>
                cols={[
                    { key: 'MeterName', label: 'Meter', headerStyle: { width: 'auto' }, rowStyle: { width: 'auto' } },
                    { key: 'AssetKey', label: 'Asset', headerStyle: { width: 'auto' }, rowStyle: { width: 'auto' } },
                    { key: 'Name', label: 'Channel', headerStyle: { width: 'auto' }, rowStyle: { width: 'auto' } },
                    { key: 'Phase', label: 'Phase', headerStyle: { width: 'auto' }, rowStyle: { width: 'auto' } },
                ]}
                tableClass="table table-hover"
                data={channelList}
                sortField={sort}
                ascending={asc}
                onSort={(d) => dispatch(SortAffectedChanels({ Ascending: d.ascending, SortField: d.col }))}
                onClick={(d) => { }}
                theadStyle={{ fontSize: 'smaller', display: 'table', tableLayout: 'fixed', width: '100%' }}
                tbodyStyle={{ display: 'block', overflowY: 'scroll', maxHeight: window.innerHeight - 300, width: '100%' }}
                rowStyle={{ fontSize: 'smaller', display: 'table', tableLayout: 'fixed', width: '100%' }}
                selected={(item) => false}
                primaryKey={'ID'}
                updateSelection={(selected) => { dispatch(updateStatisticChannels(selected));}}
                selectAll={props.selectAll}
            />
        </div>
    )
}

export default SelectStatisticsData