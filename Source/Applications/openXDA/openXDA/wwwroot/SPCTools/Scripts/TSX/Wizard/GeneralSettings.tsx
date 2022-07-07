﻿//******************************************************************************************************
//  StaticAlarm.tsx - Gbtc
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
import FilterWindow from '../CommonComponents/Filter';
import { SPCTools, openXDA, Redux, Filter } from '../global';
import Table from '@gpa-gemstone/react-table';
import { cloneDeep, clone } from 'lodash';
import { Input, Select } from '@gpa-gemstone/react-forms';
import MultiSelectTable from '../CommonComponents/MultiSelectTable';
import {  updateAlarmGroup, selectSelectedMeter, selectSelectedMeterASC, selectSelectedMeterSort, sortSelectedMeters, removeMeter, addMeter, selectMeasurmentTypeID, updateMeasurmentTypeID, selectAlarmGroup, selectSeriesTypeID, updateSeriesTypeID, updateAlarmDayGroupID, selectAlarmDayGroupID, SelectWizardType } from './DynamicWizzardSlice'
import { useSelector, useDispatch } from 'react-redux';
import { SelectMeasurmentTypes } from '../store/MeasurmentTypeSlice';
import { SelectAlarmTypes } from '../store/AlarmTypeSlice';
import { SelectAvailablePhasesStatus, SelectAvailablePhases, SelectSelectedPhases, selectPhase, FetchAvailablePhases } from '../store/WizardPhaseOptionSlice';
import { SelectAvailableVoltages, SelectAvailableVoltageStatus, SelectSelectedVoltages, selectVoltage, FetchAvailableVoltages } from '../store/WizardVoltageOptionSlice';
import { SelectSeriesTypes, SelectSeriesTypeStatus } from '../store/SeriesTypeSlice';
import { SelectAlarmDayGroups } from '../store/AlarmDayGroupSlice';
import { FetchAffectedChannels, SelectAffectedChannelCount, SelectAffectedChannelStatus } from '../store/WizardAffectedChannelSlice';


declare var homePath: string;
declare var apiHomePath: string;

declare var userIsAdmin: boolean;

interface IProps {}

const GeneralSettings = (props: IProps) => {
    const isDynamic = useSelector(SelectWizardType);

    const group = useSelector(selectAlarmGroup);
    const alarmTypes = useSelector(SelectAlarmTypes);
    const affectedChannelCount = useSelector(SelectAffectedChannelCount);
    const affectedChannelState = useSelector(SelectAffectedChannelStatus);

    const dispatch = useDispatch();

    const selectedMeter = useSelector(selectSelectedMeter);
    const sort = useSelector(selectSelectedMeterSort);
    const asc = useSelector(selectSelectedMeterASC);

    React.useEffect(() => {
        dispatch(FetchAvailablePhases());
        dispatch(FetchAvailableVoltages());

    }, [selectedMeter]);

    //Set Alarm Parameters to valid start Values
    React.useEffect(() => {
        if (alarmTypes.findIndex(item => item.ID == group.AlarmTypeID) == -1)
            dispatch(updateAlarmGroup({ ...group, AlarmTypeID: alarmTypes[0].ID }));
    }, [])

    return (
        <div style={{ width: '100%', height: '100%' }}>
            <div className="row" style={{ margin: 0 }}>
                <div className="col" style={{ width: '50%' }}>
                    <Input<SPCTools.IAlarmGroup>
                        Record={group}
                        Field={'Name'}
                        Setter={(r) => dispatch(updateAlarmGroup(r))}
                        Label={'Alarm Group Name'}
                        Valid={() => (group.Name != undefined && group.Name.length > 0 ? true : false)} />
                </div>
                <div className="col" style={{ width: '50%' }}>
                </div>
            </div>
            <div className="row" style={{ margin: 0 }}>
                <div className="col" style={{ width: '50%' }}>
                    <Select<SPCTools.IAlarmGroup>
                        Record={group}
                        Field={'AlarmTypeID'}
                        Setter={(r) => dispatch(updateAlarmGroup(r))}
                        Label={'Alarm Setpoint Type'}
                        Options={alarmTypes.map(item => { return { Value: item.ID.toString(), Label: item.Name } })}
                    />
                </div>
                <div className="col" style={{ width: '50%' }}>
                    {isDynamic == 'dynamic' ? <RepeatabilityTypeSelect /> : null}
                </div>
            </div>
            <div className="row" style={{ margin: 0 }}>
                <div className="col-9">
                    <h2>Select channels to which the alarm setpoint will be applied:</h2>
                </div>
                <div className="col">
                    <span className="float-right"> Number of Channels Selected: {(affectedChannelState != 'idle' ? <div className="spinner-border spinner-border-sm" role="status"></div> : affectedChannelCount)}</span>
                </div>
            </div>
            <div className="row" style={{ margin: 0, height: 'calc(100% - 218px)'}}>
                <div className="col" style={{height: '100%'}}>
                    <div className="row" style={{margin: 0, width: '100%'}}>
                        <h3> Selected Meters </h3>
                    </div>
                    <div className="row" style={{ margin: 0, width: '100%', height: 'calc(100% - 137px)' }}>
                        <div className="col">
                            <Table<openXDA.IMeter>
                                tableStyle={{height: '100%'}}
                                cols={[
                                    { key: 'Name', label: 'Name', field: 'Name', headerStyle: { width: '45%' }, rowStyle: { width: '45%' } },
                                    { key: 'Location', label: 'Substation', field: 'Location', headerStyle: { width: '45%' }, rowStyle: { width: '45%' } },
                                    { key: null, label: '', headerStyle: { width: '10%' }, rowStyle: { width: '10%' }, content: (item, key) => <div onClick={() => dispatch(removeMeter([item]))} style={{ width: '100%', height: '100%', marginTop: '2px', textAlign: 'center' }}><i className="fa fa-trash-o"></i></div> },
                                ]}
                                tableClass="table table-hover"
                                data={selectedMeter}
                                sortKey={sort}
                                ascending={asc}
                                onSort={(d) => { dispatch(sortSelectedMeters({ field: d.colField, asc: d.ascending })) }}
                                onClick={(d) => { }}
                                theadStyle={{ fontSize: 'smaller', display: 'table', tableLayout: 'fixed', width: '100%' }}
                                tbodyStyle={{ display: 'block', overflowY: 'scroll', maxHeight: 300, width: '100%' }}
                                rowStyle={{ fontSize: 'smaller', display: 'table', tableLayout: 'fixed', width: '100%' }}
                                selected={(item) => false}
                                />
                        </div>
                    </div>
                    <div className="row" style={{ margin: 0, width: '100%' }}>
                        <div className="col">
                            <div className="btn-group mr-2" role="group" style={{ padding: '5px' }}>
                                <button type="button" className="btn btn-primary" data-toggle='modal' data-target={'#AddAssetGroup'}> Add Asset Group</button>
                            </div>
                        </div>
                    </div>
                    <div className="row" style={{ margin: 0, width: '100%' }}>
                        <div className="col">
                            <div className="btn-group mr-2" role="group" style={{ padding: '5px' }}>
                                <button type="button" className="btn btn-primary" data-toggle='modal' data-target={'#AddMeter'}> Add Meter</button>
                            </div>
                        </div>
                    </div>
                </div>
                <div className="col">
                    <MeasurmentTypeSelect />
                    <IntervallDataSelect />
                </div>
                <div className="col">
                    <PhaseSelect />
                    <VoltageSelect />
                </div>
            </div>
            <AddMeterPopUp setter={(meter) => dispatch(addMeter(meter))} />
            <AddAssetgroupPopUp setter={(meter) => dispatch(addMeter(meter))} />
        </div>      
    );
}

const AddMeterPopUp = (props: { setter: (meters: Array<openXDA.IMeter>) => void }) => {
    const [filters, setFilters] = React.useState<Array<any>>([]);
    const [meterList, setMeterList] = React.useState<Array<openXDA.IMeter>>([]);
    const [sort, setSort] = React.useState<keyof openXDA.IMeter>("Name");
    const [asc, setAsc] = React.useState<boolean>(false);
    const [selectedMeterList, setSelectedMeterList] = React.useState<Array<openXDA.IMeter>>([]);

    React.useEffect(() => {
        let handle = getList();

        return () => {
            if (handle != undefined && handle.abort != undefined)
                handle.abort();
        }
    }, [filters, sort, asc])

    function getList(): JQuery.jqXHR<string> {
        let handle = $.ajax({
            type: "POST",
            url: `${apiHomePath}api/MeterDetail/SearchableList`,
            contentType: "application/json; charset=utf-8",
            dataType: 'json',
            data: JSON.stringify({ Searches: filters, OrderBy: sort, Ascending: asc }),
            cache: false,
            async: true
        });

        handle.done((data: string) => {
            setMeterList(JSON.parse(data) as openXDA.IMeter[])
        });

        return handle;
    }



    let searchCollumns = [
        { label: 'Name', key: 'Name' as keyof openXDA.IMeter, type: 'string' as Filter.FieldType },
        { label: 'Substation', key: 'Location' as keyof openXDA.IMeter, type: 'string' as Filter.FieldType },
        { label: 'Make', key: 'Make' as keyof openXDA.IMeter, type: 'string' as Filter.FieldType },
        { label: 'Model', key: 'Model' as keyof openXDA.IMeter, type: 'string' as Filter.FieldType },
        { label: 'AssetKey', key: 'AssetKey' as keyof openXDA.IMeter, type: 'string' as Filter.FieldType },
    ]


    return (
        <div className="modal" id='AddMeter'>
            <div className="modal-dialog" style={{ maxWidth: window.innerWidth - 200 }}>
                <div className="modal-content">
                    <div className="modal-header">
                        <h4 className="modal-title">Add Meter</h4>
                        <button type="button" className="close" data-dismiss="modal">&times;</button>
                    </div>
                    <div className="modal-body">
                        <FilterWindow<openXDA.IMeter> Id='Filter' CollumnList={searchCollumns} defaultCollumn={{ label: 'Name', key: 'Name' as keyof openXDA.IMeter, type: 'string' as Filter.FieldType }} Direction={'right'} SetFilter={setFilters} />
                        <MultiSelectTable<openXDA.IMeter>
                            cols={[
                                { key: 'Name', label: 'Name', field: 'Name', headerStyle: { width: 'auto' }, rowStyle: { width: 'auto' } },
                                { key: 'Location', label: 'Substation', field: 'Location', headerStyle: { width: 'auto' }, rowStyle: { width: 'auto' } },
                                { key: 'Make', label: 'Make', field: 'Make', headerStyle: { width: 'auto' }, rowStyle: { width: 'auto' } },
                                { key: 'Model', label: 'Model', field: 'Model', headerStyle: { width: 'auto' }, rowStyle: { width: 'auto' } },
                            ]}
                            tableClass="table table-hover"
                            data={meterList}
                            sortKey={sort}
                            ascending={asc}
                            onSort={(d) => {
                                if (d.colKey == sort) {
                                    setAsc(!asc);
                                }
                                else {
                                    setAsc(asc);
                                    setSort(d.colField);
                                }
                            }}
                            onClick={(d) => { }}
                            theadStyle={{ fontSize: 'smaller', display: 'table', tableLayout: 'fixed', width: '100%' }}
                            tbodyStyle={{ display: 'block', overflowY: 'scroll', maxHeight: 500, width: '100%' }}
                            rowStyle={{ fontSize: 'smaller', display: 'table', tableLayout: 'fixed', width: '100%' }}
                            selected={(item) => false}
                            primaryKey={'ID'}
                            updateSelection={(selected) => { setSelectedMeterList(selected) }}
                            selectAll={false}
                        />
                    </div>

                    <div className="modal-footer">
                        <button type="button" className="btn btn-primary" data-dismiss="modal" onClick={() => props.setter(selectedMeterList)} disabled={selectedMeterList.length < 1} >Add</button>
                        <button type="button" className="btn btn-danger" data-dismiss="modal">Close</button>
                    </div>

                </div>
            </div>

        </div>
        );
}

const AddAssetgroupPopUp = (props: { setter: (meters: Array<openXDA.IMeter>) => void }) => {
    const [filters, setFilters] = React.useState<Array<any>>([]);
    const [assetGroupList, setAssetGroupList] = React.useState<Array<openXDA.IAssetGroup>>([]);
    const [sort, setSort] = React.useState<keyof openXDA.IAssetGroup>("Name");
    const [asc, setAsc] = React.useState<boolean>(false);
    const [selectedAssetGroupID, setSelectedAssetGroupID] = React.useState<number>(-1);
    const [selectedMeterList, setSelectedMeterList] = React.useState<Array<openXDA.IMeter>>([]);
 
    
    React.useEffect(() => {
        let handle = getList();

        return () => {
            if (handle != undefined && handle.abort != undefined)
                handle.abort();
        }
    }, [filters, sort, asc])

    React.useEffect(() => {
        let handle = null;
        if (selectedAssetGroupID > -1)
            handle = getMeters();

        return () => { if (handle != undefined && handle.abort != undefined) handle.abort(); };

    }, [selectedAssetGroupID])

    function getList(): JQuery.jqXHR<string> {
        let handle = $.ajax({
            type: "POST",
            url: `${apiHomePath}api/SPCTools/AssetGroupView/SearchableList`,
            contentType: "application/json; charset=utf-8",
            dataType: 'json',
            data: JSON.stringify({ Searches: filters, OrderBy: sort, Ascending: asc }),
            cache: false,
            async: true
        });

        handle.done((data) => {
            setAssetGroupList(JSON.parse(data) as openXDA.IAssetGroup[])
        });

        return handle;
    }

    function getMeters(): JQuery.jqXHR<Array<openXDA.IMeter>> {
        let handle = $.ajax({
            type: "GET",
            url: `${apiHomePath}api/SPCTools/AssetGroupView/${selectedAssetGroupID}/Meters`,
            contentType: "application/json; charset=utf-8",
            cache: false,
            async: true
        });

        handle.done((data ) => {
            setSelectedMeterList(JSON.parse(data) as Array<openXDA.IMeter>)
        });

        return handle;
    }

    let searchCollumns = [
        { label: 'Name', key: 'Name' as keyof openXDA.IAssetGroup, type: 'string' as Filter.FieldType },
        { label: 'Num of Meters', key: 'Meters' as keyof openXDA.IAssetGroup, type: 'integer' as Filter.FieldType },
    ]


    return (
        <div className="modal" id='AddAssetGroup'>
            <div className="modal-dialog" style={{ maxWidth: window.innerWidth - 200 }}>
                <div className="modal-content">
                    <div className="modal-header">
                        <h4 className="modal-title">Add Asset Group</h4>
                        <button type="button" className="close" data-dismiss="modal">&times;</button>
                    </div>
                    <div className="modal-body">
                        <FilterWindow<openXDA.IAssetGroup> Id='FilterAssetGroup' CollumnList={searchCollumns} defaultCollumn={{ label: 'Name', key: 'Name' as keyof openXDA.IAssetGroup, type: 'string' as Filter.FieldType }} SetFilter={setFilters} Direction={'right'} />
                        <Table<openXDA.IAssetGroup>
                            cols={[
                                { key: 'Name', label: 'Name', field: 'Name', headerStyle: { width: 'auto' }, rowStyle: { width: 'auto' } },
                                { key: 'Meters', label: 'Num. of Meters', field: 'Meters', headerStyle: { width: 'auto' }, rowStyle: { width: 'auto' } },
                                
                            ]}
                            tableClass="table table-hover"
                            data={assetGroupList}
                            sortKey={sort}
                            ascending={asc}
                            onSort={(d) => {
                                if (d.colField == sort) {
                                    setAsc(!asc);
                                }
                                else {
                                    setAsc(asc);
                                    setSort(d.colField);
                                }
                            }}
                            onClick={(d) => { setSelectedAssetGroupID(d.row.ID) }}
                            theadStyle={{ fontSize: 'smaller', display: 'table', tableLayout: 'fixed', width: '100%' }}
                            tbodyStyle={{ display: 'block', overflowY: 'scroll', maxHeight: 500, width: '100%' }}
                            rowStyle={{ fontSize: 'smaller', display: 'table', tableLayout: 'fixed', width: '100%' }}
                            selected={(item) => item.ID == selectedAssetGroupID}
                        />
                    </div>

                    <div className="modal-footer">
                        <button type="button" className="btn btn-primary" data-dismiss="modal" onClick={() => props.setter(selectedMeterList)} disabled={selectedAssetGroupID < 0} >Add</button>
                        <button type="button" className="btn btn-danger" data-dismiss="modal">Close</button>
                    </div>

                </div>
            </div>

        </div>
    );
}

const MeasurmentTypeSelect = (props: {}) => {
    const dispatch = useDispatch();
    const measurementTypes = useSelector(SelectMeasurmentTypes);
    const measurementTypeID = useSelector(selectMeasurmentTypeID);

    React.useEffect(() => {
        dispatch(FetchAvailablePhases());
        dispatch(FetchAvailableVoltages());
    }, [measurementTypeID]);

    //Set Alarm Parameters to valid start Values
    React.useEffect(() => {
        if (measurementTypes.findIndex(item => item.ID == measurementTypeID) == -1)
            dispatch(updateMeasurmentTypeID(measurementTypes[0].ID));
    }, [])

    return <div className="form-group">
        <label>Measurment</label>
        <select
            className="form-control"
            onChange={(evt) => dispatch(updateMeasurmentTypeID(parseInt(evt.target.value)))} 
            value={measurementTypeID}
        >
        {measurementTypes.map((a, i) => (
            <option key={i} value={a.ID}>
                {a.DisplayName}
            </option>
        ))}
        </select>
    </div>
}

const PhaseSelect = (props: {}) => {
    const dispatch = useDispatch();
    const loading = useSelector(SelectAvailablePhasesStatus);
    const availablePhases = useSelector(SelectAvailablePhases);
    const selectedPhases = useSelector(SelectSelectedPhases);

    React.useEffect(() => { dispatch(FetchAffectedChannels()); }, [selectedPhases])

    return (
    <fieldset className="border" style={{ padding: '10px' }}>
        <legend className="w-auto">Phases:</legend>
            {loading != 'idle' ? <div className="spinner-border" role="status"></div> :
                <div className="form-group">
                    {availablePhases.map((cb, i) => (
                        <div key={i} className="form-check form-check-inline">
                            <input
                                className="form-check-input"
                                type="checkbox"
                                checked={selectedPhases[i]}
                                onChange={(evt) => dispatch(selectPhase(cb))}
                            />
                            <label className="form-check-label">{cb.Name}</label>
                        </div>
                    ))}
                </div>
            }
    </fieldset>
    )


}

const VoltageSelect = (props: {}) => {
    const dispatch = useDispatch();
    const loading = useSelector(SelectAvailableVoltageStatus);
    const availableVoltages = useSelector(SelectAvailableVoltages);
    const selectedVoltages = useSelector(SelectSelectedVoltages);

    React.useEffect(() => { dispatch(FetchAffectedChannels()); }, [selectedVoltages])

    return (
        <fieldset className="border" style={{ padding: '10px' }}>
            <legend className="w-auto">Base Voltages:</legend>
            {loading != 'idle' ? <div className="spinner-border" role="status"></div> :
                <div className="form-group">
                    {availableVoltages.map((cb, i) => (
                        <div key={i} className="form-check form-check-inline">
                            <input
                                className="form-check-input"
                                type="checkbox"
                                checked={selectedVoltages[i]}
                                onChange={(evt) => dispatch(selectVoltage(cb))}
                            />
                            <label className="form-check-label">{cb} kV</label>
                        </div>
                    ))}
                </div>
            }
        </fieldset>
    )


}

const RepeatabilityTypeSelect = (props: {}) => {
    const dispatch = useDispatch();
    const AlarmDayGroups = useSelector(SelectAlarmDayGroups);
    const alarmDayGroupID = useSelector(selectAlarmDayGroupID);

    React.useEffect(() => {
        dispatch(FetchAvailablePhases());
        dispatch(FetchAvailableVoltages());
    }, [alarmDayGroupID]);

    return <div className="form-group">
        <label>Alarm Cycle</label>
        <select
            className="form-control"
            onChange={(evt) => dispatch(updateAlarmDayGroupID(parseInt(evt.target.value)))}
            value={alarmDayGroupID}
        >
            {AlarmDayGroups.map((a, i) => (
                <option key={i} value={a.ID}>
                    {a.Description}
                </option>
            ))}
        </select>
    </div>
}


const IntervallDataSelect = (props: {}) => {
    const dispatch = useDispatch();
    const seriesTypes = useSelector(SelectSeriesTypes);
    const loading = useSelector(SelectSeriesTypeStatus);
    const seriesTypeID = useSelector(selectSeriesTypeID);

    React.useEffect(() => {
        dispatch(FetchAffectedChannels());
    }, [seriesTypeID])

    //Set Alarm Parameters to valid start Values
    React.useEffect(() => {
        if (seriesTypes.findIndex(item => item.ID == seriesTypeID) == -1 && seriesTypes.length > 0)
            dispatch(updateSeriesTypeID(seriesTypes[0].ID));
    }, [])

    return (
    <fieldset className="border" style={{ padding: '10px' }}>
            <legend className="w-auto">Interval Data Type:</legend>
            {loading != 'idle' ? <div className="spinner-border" role="status"></div> :
                <div className="form-group">
                    {seriesTypes.map((cb, i) => (
                        <div key={i} className="form-check form-check-inline">
                            <input
                                className="form-check-input"
                                type="radio"
                                checked={seriesTypeID == cb.ID}
                                onChange={(evt) => dispatch(updateSeriesTypeID(cb.ID))}
                            />
                            <label className="form-check-label">{cb.Name}</label>
                        </div>
                    ))}
                </div>}
    </fieldset>)

}

export default GeneralSettings