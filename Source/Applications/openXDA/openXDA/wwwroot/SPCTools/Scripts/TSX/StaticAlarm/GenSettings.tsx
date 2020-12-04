//******************************************************************************************************
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
import FilterObject from '../CommonComponents/Filter';
import { SPCTools, openXDA, Redux, Filter } from '../global';
import Table from '@gpa-gemstone/react-table';
import { cloneDeep, clone } from 'lodash';
import { Input, Select } from '@gpa-gemstone/react-forms';
import MultiSelectTable from '../CommonComponents/MultiSelectTable';
import {  selectChannelCount, updateAlarmGroup, updateSelectedMeters, updateMeasurementType, selectMeasurmentTypeId, updateChannelCount, selectIntervallDataType, updateIntervalldataType, updateVoltageOptions, updatePhaseOptions, selectLoadingPhases, selectAvailablePhases, selectCurrentPhases, selectPhases, selectAvailableVoltages, selectCurrentVoltages, selectVoltages, selectLoadingVoltages, selectSelectedMeterId } from './StaticWizzardSlice'
import { useSelector, useDispatch } from 'react-redux';
import { selectAlarmTypes, selectMeasurmentTypes } from '../store/GeneralSettingsSlice';

declare var homePath: string;
declare var apiHomePath: string;

declare var userIsAdmin: boolean;

interface IProps {}

// I think we can determine the setpoint type based on Formulas used so I will skip this for now - might have to re-visit this later
const GeneralSettings = (props: IProps) => {
    const group = useSelector((state: Redux.StoreState) => state.StaticWizzard.AlarmGroup);
    const channelCount = useSelector(selectChannelCount);

    const alarmTypes = useSelector(selectAlarmTypes);

    const dispatch = useDispatch();

    const [selectedMeter, setSelectedMeter] = React.useState<Array<openXDA.IMeter>>([]);

    const [initFlag, setInitFlag] = React.useState<boolean>(false)
    const selectedMeterIDs = useSelector(selectSelectedMeterId);

    React.useEffect(() => {
        if (initFlag)
            dispatch(updateSelectedMeters(selectedMeter))
    }, [selectedMeter, initFlag])

    //Inital Rednering to allow loacl initalization of states based on React States
    React.useEffect(() => {

        let handles = [];
        handles = selectedMeterIDs.map(id => loadMeter(id));

        Promise.all(handles).then(d => setInitFlag(true));

    }, [])

    function AddMeters(meters: Array<openXDA.IMeter>) {
        setSelectedMeter((old) => {
            let result = cloneDeep(old);
            meters.forEach(item => {
                let index = result.findIndex(m => m.ID == item.ID);
                if (index == -1)
                    result.push(item);
            });
            return result;
        })
    }

    function RemoveMeter(meter: openXDA.IMeter) {
        setSelectedMeter((old) => {
            let result = cloneDeep(old);
            let index = old.findIndex(m => m.ID == meter.ID);
            if (!(index < 0))
                result.splice(index, 1);

            return result;
        })
    }

    function loadMeter(id: number): JQuery.jqXHR<openXDA.IMeter> {
        let handle = $.ajax({
            type: "GET",
            url: `${apiHomePath}api/MeterDetail/One/${id}`,
            contentType: "application/json; charset=utf-8",
            dataType: 'json',
            cache: false,
            async: true
        });

        handle.done((d) => setSelectedMeter(old => [...old, JSON.parse(d)]));

        return handle;
    }

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
                </div>
            </div>
            <div className="row" style={{ margin: 0 }}>
                <div className="col-9">
                    <h2>Select channels to which the alarm setpoint will be applied:</h2>
                </div>
                <div className="col">
                    <span className="float-right"> Number of Channels Selected: {(channelCount == -1 ? <div className="spinner-border spinner-border-sm" role="status"></div> : channelCount)}</span>
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
                                    { key: 'Name', label: 'Name', headerStyle: { width: '45%' }, rowStyle: { width: '45%' } },
                                    { key: 'Location', label: 'Substation', headerStyle: { width: '45%' }, rowStyle: { width: '45%' } },
                                    { key: null, label: '', headerStyle: { width: '10%' }, rowStyle: { width: '10%' }, content: (item, key) => <div onClick={() => RemoveMeter(item)} style={{ width: '100%', height: '100%', marginTop: '2px', textAlign: 'center' }}><i className="fa fa-trash-o"></i></div> },
                                ]}
                                tableClass="table table-hover"
                                data={selectedMeter}
                                sortField={'ID'}
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
            <AddMeterPopUp setter={AddMeters} />
            <AddAssetgroupPopUp setter={AddMeters} />
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

    function getList(): JQuery.jqXHR<Array<openXDA.IMeter>> {
        let handle = $.ajax({
            type: "POST",
            url: `${apiHomePath}api/MeterDetail/SearchableList`,
            contentType: "application/json; charset=utf-8",
            dataType: 'json',
            data: JSON.stringify({ Searches: filters, OrderBy: sort, Ascending: asc }),
            cache: false,
            async: true
        });

        handle.done((data: Array<openXDA.IMeter>) => {
            setMeterList(data)
        });

        return handle;
    }



    let searchColumns = [
        { label: 'Name', key: 'Name', type: 'string'},
        { label: 'Substation', key: 'Location', type: 'string' },
        { label: 'Make', key: 'Make', type: 'string'  },
        { label: 'Model', key: 'Model', type: 'string' },
        { label: 'AssetKey', key: 'AssetKey', type: 'string'},
    ] as Filter.IField<openXDA.IMeter>[]


    return (
        <div className="modal modal-xl" id='AddMeter'>
            <div className="modal-dialog">
                <div className="modal-content">
                    <div className="modal-header">
                        <h4 className="modal-title">Add Meter</h4>
                        <button type="button" className="close" data-dismiss="modal">&times;</button>
                    </div>
                    <div className="modal-body">
                        <FilterObject<openXDA.IMeter> Id='Filter' CollumnList={searchColumns} defaultCollumn={{ label: 'Name', key: 'Name' , type: 'string'}} SetFilter={setFilters} />
                        <MultiSelectTable<openXDA.IMeter>
                            cols={[
                                { key: 'Name', label: 'Name', headerStyle: { width: 'auto' }, rowStyle: { width: 'auto' } },
                                { key: 'Location', label: 'Substation', headerStyle: { width: 'auto' }, rowStyle: { width: 'auto' } },
                                { key: 'Make', label: 'Make', headerStyle: { width: 'auto' }, rowStyle: { width: 'auto' } },
                                { key: 'Model', label: 'Model', headerStyle: { width: 'auto' }, rowStyle: { width: 'auto' } },
                            ]}
                            tableClass="table table-hover"
                            data={meterList}
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

    function getList(): JQuery.jqXHR<Array<openXDA.IAssetGroup>> {
        let handle = $.ajax({
            type: "POST",
            url: `${apiHomePath}api/SPCTools/AssetGroupView/SearchableList`,
            contentType: "application/json; charset=utf-8",
            dataType: 'json',
            data: JSON.stringify({ Searches: filters, OrderBy: sort, Ascending: asc }),
            cache: false,
            async: true
        });

        handle.done((data: Array<openXDA.IAssetGroup>) => {
            setAssetGroupList(data)
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

        handle.done((data: Array<openXDA.IMeter>) => {
            setSelectedMeterList(data)
        });

        return handle;
    }

    let searchCollumns = [
        { label: 'Name', key: 'Name' , type: 'string'  },
        { label: 'Num of Meters', key: 'Meters', type: 'integer' },
    ] as Filter.IField<openXDA.IAssetGroup>[]


    return (
        <div className="modal modal-lg" id='AddAssetGroup'>
            <div className="modal-dialog">
                <div className="modal-content">
                    <div className="modal-header">
                        <h4 className="modal-title">Add Asset Group</h4>
                        <button type="button" className="close" data-dismiss="modal">&times;</button>
                    </div>
                    <div className="modal-body">
                        <FilterObject<openXDA.IAssetGroup> Id='FilterAssetGroup' CollumnList={searchCollumns} defaultCollumn={{ label: 'Name', key: 'Name', type: 'string'}} SetFilter={setFilters} />
                        <Table<openXDA.IAssetGroup>
                            cols={[
                                { key: 'Name', label: 'Name', headerStyle: { width: 'auto' }, rowStyle: { width: 'auto' } },
                                { key: 'Meters', label: 'Num. of Meters', headerStyle: { width: 'auto' }, rowStyle: { width: 'auto' } },
                                
                            ]}
                            tableClass="table table-hover"
                            data={assetGroupList}
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
                            onClick={(d) => { setSelectedAssetGroupID(d.row.ID) }}
                            theadStyle={{ fontSize: 'smaller', display: 'table', tableLayout: 'fixed', width: '100%' }}
                            tbodyStyle={{ display: 'block', overflowY: 'scroll', maxHeight: window.innerHeight - 300, width: '100%' }}
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
    const measurementTypes = useSelector(selectMeasurmentTypes);
    const measurementTypeID = useSelector(selectMeasurmentTypeId);

    React.useEffect(() => {
        dispatch(updateVoltageOptions());
        dispatch(updatePhaseOptions());
    }, [measurementTypeID])

    return <div className="form-group">
        <label>Measurment Type</label>
        <select
            className="form-control"
            onChange={(evt) => dispatch(updateMeasurementType(parseInt(evt.target.value)))} 
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
    const loading = useSelector(selectLoadingPhases);
    const availablePhases = useSelector(selectAvailablePhases);
    const selectedPhases = useSelector(selectCurrentPhases);

    React.useEffect(() => { dispatch(updateChannelCount()) }, [selectedPhases])

    return (
    <fieldset className="border" style={{ padding: '10px' }}>
        <legend className="w-auto">Phases:</legend>
            {loading ? <div className="spinner-border" role="status"></div> :
                <div className="form-group">
                    {availablePhases.map((cb, i) => (
                        <div key={i} className="form-check form-check-inline">
                            <input
                                className="form-check-input"
                                type="checkbox"
                                checked={selectedPhases[i]}
                                onChange={(evt) => {
                                    let updated = clone(selectedPhases); updated[i] = !updated[i]; dispatch(selectPhases(updated));
                                }}
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
    const loading = useSelector(selectLoadingVoltages);
    const availableVoltages = useSelector(selectAvailableVoltages);
    const selectedVoltages = useSelector(selectCurrentVoltages);

    React.useEffect(() => { dispatch(updateChannelCount()) }, [selectedVoltages])

    return (
        <fieldset className="border" style={{ padding: '10px' }}>
            <legend className="w-auto">Base Voltages:</legend>
            {loading ? <div className="spinner-border" role="status"></div> :
                <div className="form-group">
                    {availableVoltages.map((cb, i) => (
                        <div key={i} className="form-check form-check-inline">
                            <input
                                className="form-check-input"
                                type="checkbox"
                                checked={selectedVoltages[i]}
                                onChange={(evt) => {
                                    let updated = clone(selectedVoltages); updated[i] = !updated[i]; dispatch(selectVoltages(updated));
                                }}
                            />
                            <label className="form-check-label">{cb} kV</label>
                        </div>
                    ))}
                </div>
            }
        </fieldset>
    )


}


const IntervallDataSelect = (props: {}) => {
    const dispatch = useDispatch();
    const intervallData = useSelector(selectIntervallDataType);

    let options = [
        { ID: 'Minimum', Label: 'Minimum' },
        { ID: 'Maximum', Label: 'Maximum' },
        { ID: 'Average', Label: 'Average' },
    ];

    return (
    <fieldset className="border" style={{ padding: '10px' }}>
        <legend className="w-auto">Interval Data Type:</legend>
            <div className="form-group">
                {options.map((cb, i) => (
                    <div key={i} className="form-check form-check-inline">
                        <input
                            className="form-check-input"
                            type="radio"
                            checked={intervallData == cb.ID}
                            onChange={(evt) => dispatch(updateIntervalldataType(cb.ID as SPCTools.IntervallDataType))}
                        />
                        <label className="form-check-label">{cb.Label}</label>
                    </div>
                ))}
            </div>
    </fieldset>)

}

export default GeneralSettings