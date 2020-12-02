//******************************************************************************************************
//  StaticWizzardSlize.tsx - Gbtc
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
//  10/29/2020 - C. Lackner
//       Generated original version of source code.
//
//******************************************************************************************************
import { createSlice, createAsyncThunk, PayloadAction, createSelector } from '@reduxjs/toolkit';
import { SPCTools, StaticWizzard, openXDA } from '../global';
import { stringify } from 'querystring';
import { dispatch } from 'd3';
import RequestHandle from '../store/RequestHandle';
import _ from 'lodash';
import { selectIsLoading } from '../Store/GeneralSettingsSlice';

declare var homePath: string;
declare var apiHomePath: string;
declare var userIsAdmin: boolean;

const handles = new RequestHandle();

// Thunk For Reseting Wizzar
export const ResetWizzard = createAsyncThunk('StaticWizzard/ResetWizzard', (_, thunkAPI) => {
    thunkAPI.dispatch(StaticWizzardSlice.actions.reset((thunkAPI.getState() as SPCTools.RootState).GeneralSettings));

})

//Thunk for Updating MeterList
export const updateSelectedMeters = createAsyncThunk('StaticWizzard/updateSelectedMeters', (meters: openXDA.IMeter[], thunkAPI) => {
    // Update meters
    if (meters.length == (thunkAPI.getState() as SPCTools.RootState).StaticWizzard.SelectedMeterID.length)
        return;

    thunkAPI.dispatch(StaticWizzardSlice.actions.updateSelectedMeters(meters.map(m => m.ID)));

    //Updated available Voltages and Phases
    thunkAPI.dispatch(updateVoltageOptions());
    thunkAPI.dispatch(updatePhaseOptions());

})

//extra thunks to update Voltage and BaseKV
export const updateVoltageOptions = createAsyncThunk('StaticWizzard/updateVoltageOptions', async (_, thunkAPI) => {
    return await getVoltages((thunkAPI.getState() as SPCTools.RootState).StaticWizzard.SelectedMeterID, (thunkAPI.getState() as SPCTools.RootState).StaticWizzard.SelectedMeasurmentTypeID) 
})

export const updatePhaseOptions = createAsyncThunk('StaticWizzard/updatePhaseOptions', async (_, thunkAPI) => {
    return await getPhases((thunkAPI.getState() as SPCTools.RootState).StaticWizzard.SelectedMeterID, (thunkAPI.getState() as SPCTools.RootState).StaticWizzard.SelectedMeasurmentTypeID)
})

//Thunk For Updating channelCount
export const updateChannelCount = createAsyncThunk('StaticWizzard/updateChannelCount', async (_, thunkAPI) => {
    return await getChannelCount((thunkAPI.getState() as SPCTools.RootState).StaticWizzard)
})


// Thunk to Save AlarmGroup
export const SaveAlarmGroup = createAsyncThunk('StaticWizzard/Save', async (_, thunkAPI) => {
    return await saveGroup(thunkAPI.getState() as SPCTools.RootState);

})


export const StaticWizzardSlice = createSlice({
    name: 'StaticWizzard',
    initialState: {
        Step: 'general',
        AlarmGroup: {},

        AvailableVoltages: [],
        SelectedVoltages: [],
        AvailablePhases: [],
        SelectedPhases: [],

        SelectedChannelCount: 0,
        LoadingVoltages: false,
        LoadingPhases: false,
        LoadingChannelCount: false,

        SelectedIntervallDataType: 'Min',
        SelectedMeasurmentTypeID: 0,

        SelectedMeterID: [],
        StatSource: {},
        StatChannels: [],
        AlarmFactors: [],

        SelectedSeverityID: 0,
        AffectedChannels: []
    } as StaticWizzard.IState,
    reducers: {
        reset: (state, action: PayloadAction<SPCTools.ISettingsState>) => {
            let alarmTypeID = (action.payload.AlarmTypes[0] != undefined ? action.payload.AlarmTypes[0].ID : 0)
            let measurmentTypeID = (action.payload.MeasurementTypes[0] != undefined ? action.payload.MeasurementTypes[0].ID : 0)
            let severityID = (action.payload.Severities[0] != undefined ? action.payload.Severities[0].ID : 0)

            let dt = new Date();
            let startDate = `${dt.getFullYear() - 1}-${(dt.getMonth() + 1).toString().padStart(2, '0')}-${dt.getDate().toString().padStart(2, '0')}`;
            let endDate = `${dt.getFullYear()}-${(dt.getMonth() + 1).toString().padStart(2, '0')}-${dt.getDate().toString().padStart(2, '0')}`;

            state.Step = 'general'
            state.AlarmGroup = { AlarmTypeID: alarmTypeID, Formula: "", ID: -1, Name: "" }

            state.AvailableVoltages = []
            state.SelectedVoltages = []
            state.AvailablePhases = []
            state.SelectedPhases = []
            state.SelectedChannelCount = 0
            state.SelectedIntervallDataType = 'Min'
            state.SelectedMeasurmentTypeID = measurmentTypeID
            state.SelectedMeterID = [];
            state.StatSource = {
                DataFilter: { FilterLower: false, LowerLimit: 0, UpperLimit: 0, FilterUpper: false, FilterZero: false },
                StartDate: startDate,
                EndDate: endDate,
            }
            state.AlarmFactors = []
            state.StatChannels = []
            state.SelectedSeverityID = severityID
            state.SetPointEvaluation = undefined
            state.AffectedChannels = []
        },
        next: (state) => {
            if (state.Step == 'general')
                state.Step = 'selectData'
            else if (state.Step == 'selectData')
                state.Step = 'setpoint'
            else if (state.Step == 'setpoint')
                state.Step = 'test'
        },
        back: (state) => {
            if (state.Step == 'selectData')
                state.Step = 'general'
            else if (state.Step == 'setpoint')
                state.Step = 'selectData'
            else if (state.Step == 'test')
                state.Step = 'setpoint'

        },
        updateAlarmGroup: (state, action: PayloadAction<SPCTools.IAlarmGroup>) => {
            state.AlarmGroup = action.payload;
        },
        updateSelectedMeters: (state, action: PayloadAction<number[]>) => {
            state.SelectedMeterID = action.payload;
        },
        updateMeasurementType: (state, action: PayloadAction<number>) => {
            state.SelectedMeasurmentTypeID = action.payload;
        },
        updateIntervalldataType: (state, action: PayloadAction<SPCTools.IntervallDataType>) => {
            state.SelectedIntervallDataType = action.payload;
        },
        selectPhases: (state, action: PayloadAction<boolean[]>) => {
            state.SelectedPhases = action.payload;
        },
        selectVoltages: (state, action: PayloadAction<boolean[]>) => {
            state.SelectedVoltages = action.payload;
        },
        updateStatistics: (state, action: PayloadAction<SPCTools.IStatisticData>) => {
            state.StatSource = action.payload;
        },
        updateStatChannels: (state, action: PayloadAction<openXDA.IChannel[]>) => {
            state.StatChannels = action.payload;
        },
        updateAllChannels: (state, action: PayloadAction<openXDA.IChannel[]>) => {
            state.AffectedChannels = action.payload;
        },
        updateSeverity: (state, action: PayloadAction<number>) => {
            state.SelectedSeverityID = action.payload;
        },
        updateFactor: (state, action: PayloadAction<{ index: number, factor: SPCTools.IFactor }>) => {
            state.AlarmFactors[action.payload.index] = action.payload.factor
        },
        addFactor: (state) => {
            state.AlarmFactors.push({ ID: -1, SeverityID: state.SelectedSeverityID, Value: 1.0 });
        },
        removeFactor: (state, action: PayloadAction<number>) => {
            state.AlarmFactors.splice(action.payload, 1)
        },
        updateSetPointEval: (state, action: PayloadAction<SPCTools.ITokenizerResponse>) => {
            state.SetPointEvaluation = action.payload;
        },
    },
     extraReducers: (builder) => {

         builder.addCase(updateVoltageOptions.pending, (state, action) => {
             state.LoadingVoltages = true;
         });
         builder.addCase(updateVoltageOptions.rejected, (state, action) => {
             state.LoadingVoltages = false;
             state.AvailableVoltages = [];
             state.SelectedVoltages = [];
         });
         builder.addCase(updateVoltageOptions.fulfilled, (state, action) => {
             state.LoadingVoltages = false;
             let currentVoltages = state.AvailableVoltages.filter((m, i) => state.SelectedVoltages[i]);
             state.AvailableVoltages = action.payload;
             state.SelectedVoltages = state.AvailableVoltages.map(item => currentVoltages.findIndex(i => i == item) > -1);
         });

         builder.addCase(updatePhaseOptions.pending, (state, action) => {
             state.LoadingPhases = true;
         });
         builder.addCase(updatePhaseOptions.rejected, (state, action) => {
             state.LoadingPhases= false;
             state.AvailablePhases = [];
             state.SelectedPhases = [];
         });
         builder.addCase(updatePhaseOptions.fulfilled, (state, action) => {
             state.LoadingPhases = false;
             let currentPhases = state.AvailablePhases.filter((p, i) => state.SelectedPhases[i]).map(p => p.ID);
             state.AvailablePhases = action.payload;
             state.SelectedPhases = state.AvailablePhases.map(item => currentPhases.findIndex(i => i == item.ID) > -1);
         });

         builder.addCase(updateChannelCount.pending, (state, action) => {
             state.LoadingChannelCount = true;
         });
         builder.addCase(updateChannelCount.rejected, (state, action) => {
             state.LoadingChannelCount = false;
             state.SelectedChannelCount = 0;
         });
         builder.addCase(updateChannelCount.fulfilled, (state, action) => {
             state.LoadingChannelCount = false;
             state.SelectedChannelCount = action.payload;
         });

    }

});

export const {
    next, back,
    updateAlarmGroup,
    updateMeasurementType,
    updateIntervalldataType,
    selectPhases, selectVoltages,
    updateStatistics,
    updateStatChannels,
    updateAllChannels,
    updateSeverity,
    updateFactor, addFactor, removeFactor,
    updateSetPointEval,

} = StaticWizzardSlice.actions

export default StaticWizzardSlice.reducer;

// Selectors
export const SelectTab = (state: SPCTools.RootState) => state.StaticWizzard.Step;

export const selectStatus = (state: SPCTools.RootState) => (selectIsLoading(state) ? 'loading' : 'idle') as SPCTools.LoadingState;

export const selectChannelCount = (state: SPCTools.RootState) => (state.StaticWizzard.LoadingChannelCount? -1 : state.StaticWizzard.SelectedChannelCount);

export const selectMeasurmentTypeId = (state: SPCTools.RootState) => state.StaticWizzard.SelectedMeasurmentTypeID;
export const selectIntervallDataType = (state: SPCTools.RootState) => state.StaticWizzard.SelectedIntervallDataType;

export const selectLoadingVoltages = (state: SPCTools.RootState) => state.StaticWizzard.LoadingVoltages;
export const selectAvailableVoltages = (state: SPCTools.RootState) => state.StaticWizzard.AvailableVoltages;
export const selectCurrentVoltages = (state: SPCTools.RootState) => state.StaticWizzard.SelectedVoltages;


export const selectLoadingPhases = (state: SPCTools.RootState) => state.StaticWizzard.LoadingPhases;
export const selectAvailablePhases = (state: SPCTools.RootState) => state.StaticWizzard.AvailablePhases;
export const selectCurrentPhases = (state: SPCTools.RootState) => state.StaticWizzard.SelectedPhases;

export const selectStatisticSettings = (state: SPCTools.RootState) => state.StaticWizzard.StatSource;

export const selectSeverity = (state: SPCTools.RootState) => state.StaticWizzard.SelectedSeverityID;
export const selectfactors = (state: SPCTools.RootState) => state.StaticWizzard.AlarmFactors;
export const selectAlarmType = createSelector(
    (state: SPCTools.RootState) => state.StaticWizzard.AlarmGroup.AlarmTypeID,
    (state: SPCTools.RootState) => state.GeneralSettings.AlarmTypes,
    (selectedID, alarmTypes) => {
        return alarmTypes.find(item => item.ID == selectedID)
    });

export const selectHistoricChannelList = (state: SPCTools.RootState) => state.StaticWizzard.StatChannels;

export const selectStatStart = (state: SPCTools.RootState) => state.StaticWizzard.StatSource.StartDate;
export const selectStatEnd = (state: SPCTools.RootState) => state.StaticWizzard.StatSource.EndDate;
export const selectdataFilter = (state: SPCTools.RootState) => state.StaticWizzard.StatSource.DataFilter;

export const selectTokenizerResponse = (state: SPCTools.RootState) => state.StaticWizzard.SetPointEvaluation;

export const selectfullHistoricDataSet = (state: SPCTools.RootState) => state.StaticWizzard.StatChannels.length == state.StaticWizzard.SelectedChannelCount;

export const selectAffectedChannels = (state: SPCTools.RootState) => state.StaticWizzard.AffectedChannels;

export const selectTokenizerRequest = createSelector(
    (state: SPCTools.RootState) => state.StaticWizzard.StatSource.DataFilter,
    (state: SPCTools.RootState) => state.StaticWizzard.StatChannels,
    (state: SPCTools.RootState) => state.StaticWizzard.StatSource.StartDate,
    (state: SPCTools.RootState) => state.StaticWizzard.StatSource.EndDate,
    (state: SPCTools.RootState) => state.StaticWizzard.AlarmGroup.Formula,
    (filter, channels, start, end, value) => {
        return { Value: value, Channels: channels.map(item => item.ID), DataFilter: filter, EndDate: end, StartDate: start } as SPCTools.ITokenizerRequest;
    });

export const selectErrors = createSelector(
    (state: SPCTools.RootState) => state.StaticWizzard.Step,
    (state: SPCTools.RootState) => state.StaticWizzard.SelectedChannelCount > 0,
    (state: SPCTools.RootState) => state.StaticWizzard.AlarmGroup.Name.length > 0,
    (state: SPCTools.RootState) => state.StaticWizzard.SelectedMeterID.length > 0,
    (state: SPCTools.RootState) => state.StaticWizzard.SelectedVoltages.some(i => i),
    (state: SPCTools.RootState) => state.StaticWizzard.SelectedPhases.some(i => i),
    (state: SPCTools.RootState) => state.StaticWizzard.StatSource.StartDate != "",
    (state: SPCTools.RootState) => state.StaticWizzard.StatSource.EndDate != "",
    (state: SPCTools.RootState) => state.StaticWizzard.StatChannels.length > 0,
    (state: SPCTools.RootState) => state.StaticWizzard.StatChannels.length == state.StaticWizzard.SelectedChannelCount,
    (state: SPCTools.RootState) => state.StaticWizzard.SetPointEvaluation == undefined ? false : state.StaticWizzard.SetPointEvaluation.Valid ,
    (state: SPCTools.RootState) => state.StaticWizzard.SetPointEvaluation == undefined ? false : state.StaticWizzard.SetPointEvaluation.IsScalar,
    (step, channelCount, name, meterCount, voltageCount, phaseCount,
        startDate, endDate, selectedhistory, fullHistory, setPoint, scalarSetpoint) => {
        let result = [] as StaticWizzard.IRequirement[];
        if (step == 'general') {
            result.push({ text: "A Name is required", complete: (name ? 'complete' : 'required') });
            result.push({ text: "At least 1 Meter needs to be selected", complete: (meterCount ? 'complete' : 'required') });
            result.push({ text: "At least 1 Phase needs to be selected", complete: (phaseCount ? 'complete' : 'required') });
            result.push({ text: "At least 1 Base Voltage needs to be selected", complete: (voltageCount ? 'complete' : 'required') });
            result.push({ text: "The Selection needs to result in at least 1 Channel", complete: (channelCount ? 'complete' : 'required') });
        }
        else if (step == 'selectData') {
            result.push({ text: "A valid start date has to be selected", complete: (startDate ? 'complete' : 'required') });
            result.push({ text: "A valid end date has to be selected", complete: (endDate ? 'complete' : 'required') })
            result.push({ text: "At least 1 Channel needs to be selected", complete: (selectedhistory ? 'complete' : 'required') })
            if (!fullHistory)
                result.push({ text: "A single threshhold will be required for all Channels <br> since there are some Channels that are not selected as historic datasource", complete: 'warning' })
        }
        else if (step == 'setpoint') {
            result.push({ text: "A valid setpoint Expression is Required", complete: (setPoint ? 'complete' : 'required') });
            if (!fullHistory)
                result.push({ text: "A single scalar setpoint is required for all Channels", complete: (scalarSetpoint ? 'complete' : 'required') })
            else if (!scalarSetpoint)
                result.push({ text: "The setpoint expression will result in a different threshold for each Channel", complete: 'warning' })
            
        }
        return result;

    });

export const selectChannelRequest = createSelector(
    (state: SPCTools.RootState) => state.StaticWizzard.SelectedMeterID,
    (state: SPCTools.RootState) => state.StaticWizzard.SelectedMeasurmentTypeID,
    (state: SPCTools.RootState) => state.StaticWizzard.AvailableVoltages,
    (state: SPCTools.RootState) => state.StaticWizzard.SelectedVoltages,
    (state: SPCTools.RootState) => state.StaticWizzard.AvailablePhases,
    (state: SPCTools.RootState) => state.StaticWizzard.SelectedPhases,

    (MeterID, MeasurmentTypeID, availableVoltages, selectedVoltages, availablePhases, selectedPhases) => {
        return {
            MeterID: MeterID, MeasurmentTypeID: MeasurmentTypeID,
            BaseVoltage: availableVoltages.filter((p, i) => selectedVoltages[i]),
            PhaseID: availablePhases.filter((p, i) => selectedPhases[i]).map(p => p.ID)
        }
    });

// Async Functions

function getPhases(meterIDs: number[], measurmentTypeID: number): JQuery.jqXHR<Array<openXDA.IPhase>> {

    let handle = $.ajax({
        type: "POST",
        url: `${apiHomePath}api/SPCTools/StaticAlarmCreation/AvailablePhases`,
        contentType: "application/json; charset=utf-8",
        dataType: 'json',
        data: JSON.stringify({ MeterID: meterIDs, MeasurmentTypeID: measurmentTypeID }),
        cache: false,
        async: true
    });
    return handle;
}

function getVoltages(meterIDs: number[], measurmentTypeID: number): JQuery.jqXHR<Array<number>> {

    let handle = $.ajax({
        type: "POST",
        url: `${apiHomePath}api/SPCTools/StaticAlarmCreation/AvailableVoltages`,
        contentType: "application/json; charset=utf-8",
        dataType: 'json',
        data: JSON.stringify({ MeterID: meterIDs, MeasurmentTypeID: measurmentTypeID }),
        cache: false,
        async: true
    });

    return handle;
}

function getChannelCount(state: StaticWizzard.IState): JQuery.jqXHR<number> {
    

    let handle = $.ajax({
        type: "POST",
        url: `${apiHomePath}api/SPCTools/StaticAlarmCreation/AffectedChannels`,
        contentType: "application/json; charset=utf-8",
        dataType: 'json',
        data: JSON.stringify({
            MeterID: state.SelectedMeterID, MeasurmentTypeID: state.SelectedMeasurmentTypeID,
            BaseVoltage: state.AvailableVoltages.filter((p, i) => state.SelectedVoltages[i]), PhaseID: state.AvailablePhases.filter((p, i) => state.SelectedPhases[i]).map(p => p.ID)
        }),
        cache: false,
        async: true
    });

    return handle;
}



function saveGroup(state: SPCTools.RootState): JQuery.jqXHR {
    /*let setpointEval = {
        startTime: state.StaticWizzard.HistoricData.StartDate,
        endTime: state.StaticWizzard.HistoricData.EndDate,
        channelIds: state.StaticWizzard.HistoricData.ChannelList.map((item: openXDA.IChannel) => item.ID),
        setPoint: state.StaticWizzard.SetPoints[0].text
    };

    let handle = $.ajax({
        type: "POST",
        url: `${apiHomePath}api/SPCTools/StaticAlarmCreation/SaveGroup`,
        contentType: "application/json; charset=utf-8",
        dataType: 'json',
        data: JSON.stringify({
            generalSettings: state.StaticWizzard.AlarmGroup,
            setpointEvaluation: setpointEval,
            start: state.StaticWizzard.TestResults.StartDate,
            end: state.StaticWizzard.TestResults.EndDate,
            factorList: [...state.StaticWizzard.SeverityOptions.Factors, { SeverityID: state.StaticWizzard.SeverityOptions.SeverityID, Value: 1.0, ID: -1 }]
        }),
        cache: false,
        async: true
    });
    return handle;*/
    return null;
}