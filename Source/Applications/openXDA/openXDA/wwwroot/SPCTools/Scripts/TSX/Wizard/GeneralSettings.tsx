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
import { SearchBar, LoadingIcon } from '@gpa-gemstone/react-interactive'
import { SPCTools, openXDA, Filter } from '../global';
import Table, { SelectTable } from '@gpa-gemstone/react-table';
import { Input, Select, ArrayCheckBoxes } from '@gpa-gemstone/react-forms';
import { updateAlarmGroup, selectSelectedMeter, selectSelectedMeterASC, selectSelectedMeterSort, sortSelectedMeters, removeMeter, addMeter, selectMeasurmentTypeID, updateMeasurmentTypeID, selectAlarmGroup, selectSeriesTypeID, updateSeriesTypeID, updateAlarmDayGroupID, selectAlarmDayGroupID, SelectWizardType } from './DynamicWizzardSlice'
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
                    <span className="float-right"> Number of Channels Selected: {(affectedChannelState != 'idle' ? <LoadingIcon Show={true} Size={40} /> : affectedChannelCount)}</span>
                </div>
            </div>
            <div className="row" style={{ margin: 0, height: 'calc(100% - 218px)' }}>
                <div className="col" style={{ height: '100%' }}>
                    <div className="row" style={{ margin: 0, width: '100%' }}>
                        <h3> Selected Meters </h3>
                    </div>
                    <div className="row" style={{ margin: 0, width: '100%', height: 'calc(100% - 137px)' }}>
                        <div className="col">
                            <Table<openXDA.IMeter>
                                tableStyle={{ height: '100%' }}
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
                    <div style={{ width: '50%', display: 'inline-block', margin: 0 }}>
                        <div className="col" style={{ padding: 0 }}>
                            <div className="btn-group mr-2" role="group">
                            <button type="button" className="btn btn-primary" data-toggle='modal' data-target={'#AddAssetGroup'}>Add Asset Group</button>
                            <button type="button" className="btn btn-primary" data-toggle='modal' data-target={'#AddMeter'} style={{ marginLeft: '10px' }}>Add Meter</button>
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


    return (
        <div className="modal" id='AddMeter'>
            <div className="modal-dialog" style={{ maxWidth: window.innerWidth - 200 }}>
                <div className="modal-content">
                    <div className="modal-header">
                        <h4 className="modal-title">Add Meter</h4>
                        <button type="button" className="close" data-dismiss="modal">&times;</button>
                    </div>
                    <div className="modal-body">
                        <SearchBar<openXDA.IMeter>
                            CollumnList={[
                                { label: 'Name', key: 'Name' as keyof openXDA.IMeter, type: 'string' as Filter.FieldType, isPivotField: false },
                                { label: 'Substation', key: 'Location' as keyof openXDA.IMeter, type: 'string' as Filter.FieldType, isPivotField: false },
                                { label: 'Make', key: 'Make' as keyof openXDA.IMeter, type: 'string' as Filter.FieldType, isPivotField: false },
                                { label: 'Model', key: 'Model' as keyof openXDA.IMeter, type: 'string' as Filter.FieldType, isPivotField: false },
                                { label: 'AssetKey', key: 'AssetKey' as keyof openXDA.IMeter, type: 'string' as Filter.FieldType, isPivotField: false },
                            ]}
                            Direction={'left'}
                            SetFilter={setFilters}
                        >
                        </SearchBar>
                        <SelectTable<openXDA.IMeter>
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
                            onSelection={(selected) => { setSelectedMeterList(selected) }}
                            theadStyle={{ fontSize: 'smaller', display: 'table', tableLayout: 'fixed', width: '100%' }}
                            tbodyStyle={{ display: 'block', overflowY: 'scroll', maxHeight: 500, width: '100%' }}
                            rowStyle={{ fontSize: 'smaller', display: 'table', tableLayout: 'fixed', width: '100%' }}
                            KeyField={'ID'}
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

        handle.done((data) => {
            setSelectedMeterList(JSON.parse(data) as Array<openXDA.IMeter>)
        });

        return handle;
    }

    return (
        <div className="modal" id='AddAssetGroup'>
            <div className="modal-dialog" style={{ maxWidth: window.innerWidth - 200 }}>
                <div className="modal-content">
                    <div className="modal-header">
                        <h4 className="modal-title">Add Asset Group</h4>
                        <button type="button" className="close" data-dismiss="modal">&times;</button>
                    </div>
                    <div className="modal-body">
                        <SearchBar<openXDA.IAssetGroup>
                            CollumnList={[
                                { label: 'Name', key: 'Name' as keyof openXDA.IAssetGroup, type: 'string' as Filter.FieldType, isPivotField: false },
                                { label: 'Num of Meters', key: 'Meters' as keyof openXDA.IAssetGroup, type: 'integer' as Filter.FieldType, isPivotField: false },
                            ]}
                            SetFilter={setFilters}
                            Direction={'left'}
                        >
                        </SearchBar>
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

    const selectOptions = measurementTypes.map((measure) => ({
        Value: measure.ID.toString(),
        Label: measure.DisplayName
    }));

    React.useEffect(() => {
        dispatch(FetchAvailablePhases());
        dispatch(FetchAvailableVoltages());
    }, [measurementTypeID]);

    //Set Alarm Parameters to valid start Values
    React.useEffect(() => {
        if (measurementTypes.findIndex(item => item.ID == measurementTypeID) == -1)
            dispatch(updateMeasurmentTypeID(measurementTypes[0].ID));
    }, [])

    return (
        <>
            <Select
                Record={{ obj: measurementTypeID }}
                Field={"obj"}
                Options={selectOptions}
                Setter={(evt) => dispatch(updateMeasurmentTypeID(evt.obj))}
                Label={'Measurement'}
            ></Select>
        </>
    )
}

const PhaseSelect = (props: {}) => {
    const dispatch = useDispatch();
    const loading = useSelector(SelectAvailablePhasesStatus);
    const availablePhases = useSelector(SelectAvailablePhases);
    const selectedPhases = useSelector(SelectSelectedPhases);

    const checkboxesData = availablePhases.map((phase) => ({
        ID: phase.ID.toString(),
        Label: `${phase.Name}`
    }));

    const selectedPhaseIDs = availablePhases
        .filter((phase, index) => selectedPhases[index])
        .map(phase => phase.ID.toString());

    const onCheckboxChange = (updatedRecord: { selected: string[] }) => {

        const selectedIndices = updatedRecord.selected.map(id =>
            availablePhases.findIndex(phase => phase.ID.toString() === id)
        );

        dispatch(selectPhase(selectedIndices));
    };

    React.useEffect(() => { dispatch(FetchAffectedChannels()); }, [selectedPhases])

    return (
        <fieldset className="border" style={{ padding: '10px' }}>
            <legend className="w-auto">Phases:</legend>
            {loading != 'idle' ? <div className="spinner-border" role="status"></div> :
                <ArrayCheckBoxes
                    Record={{ selected: selectedPhaseIDs }}
                    Field="selected"
                    Setter={onCheckboxChange}
                    Checkboxes={checkboxesData}
                    Label={''}
                />
            }
        </fieldset>
    )
}

const VoltageSelect = (props: {}) => {
    const dispatch = useDispatch();
    const loading = useSelector(SelectAvailableVoltageStatus);
    const availableVoltages = useSelector(SelectAvailableVoltages);
    const selectedVoltages = useSelector(SelectSelectedVoltages);

    const checkboxesData = availableVoltages.map((voltage) => ({
        ID: voltage.toString(),
        Label: `${voltage} kV`
    }));

    // Determine the IDs of the selected voltages.
    const selectedVoltageIDs = availableVoltages.filter((voltage, index) => selectedVoltages[index])
        .map(voltage => voltage.toString());

    const onCheckboxChange = (updatedRecord: { selected: string[] }) => {
        // Map the IDs back to their integer values.
        const selectedIndices = updatedRecord.selected.map(voltageStr => availableVoltages.indexOf(parseFloat(voltageStr)));
        dispatch(selectVoltage(selectedIndices));
    };

    React.useEffect(() => { dispatch(FetchAffectedChannels()); }, [selectedVoltages])
    return (
        <fieldset className="border" style={{ padding: '10px' }}>
            <legend className="w-auto">Base Voltages:</legend>
            {loading != 'idle' ? <div className="spinner-border" role="status"></div> :
                <ArrayCheckBoxes
                    Record={{ selected: selectedVoltageIDs }}
                    Field="selected"
                    Setter={onCheckboxChange}
                    Checkboxes={checkboxesData}
                    Label={''}
                />
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

    const selectOptions = AlarmDayGroups.map((alarm) => ({
        Value: alarm.ID.toString(),
        Label: alarm.Description.toString()
    }));

    return (
        <>
            <Select
                Record={{ obj: alarmDayGroupID }}
                Field={"obj"}
                Options={selectOptions}
                Setter={(evt) => dispatch(updateAlarmDayGroupID(evt.obj))}
                Label={'Alarm Cycle'}
            ></Select>
        </>
    )
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

    const handleSelectionChange = (selectedRecord: { seriesTypeID: number }) => {
        dispatch(updateSeriesTypeID(selectedRecord.seriesTypeID));
    }

    const selectOptions = seriesTypes.map(s => ({
        Value: s.ID.toString(),
        Label: s.Name.toString()
    }))

    return (
        <>
            <Select<{ seriesTypeID: number }>
                Record={{ seriesTypeID }}
                Field="seriesTypeID"
                Setter={handleSelectionChange}
                Options={selectOptions}
                Label="Interval Data Type"
            />
        </>
    )
}

export default GeneralSettings