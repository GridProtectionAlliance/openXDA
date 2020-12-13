//******************************************************************************************************
//  DynamicWizzardSlice.tsx - Gbtc
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
//  12/04/2020 - C. Lackner
//       Generated original version of source code.
//
//******************************************************************************************************
import { createSlice, createAsyncThunk, PayloadAction, createSelector, Selector } from '@reduxjs/toolkit';
import { SPCTools, StaticWizzard, openXDA, Redux, DynamicWizzard } from '../global';
import _ from 'lodash';
import { FetchAffectedChannels, SelectAffectedChannelCount, SelectAffectedChannels } from '../store/WizardAffectedChannelSlice';
import { LoadSelectedPhase, SelectSelectedPhases } from '../store/WizardPhaseOptionSlice';
import { LoadSelectedVoltages, SelectSelectedVoltages } from '../store/WizardVoltageOptionSlice';
import { SelectAlarmDayGroups } from '../store/AlarmDayGroupSlice';
import { SelectAlarmDays } from '../store/AlarmDaySlice';
import { FetchParsedSetPoint } from '../store/SetPointParseSlice';
import { Reset as ResetParsedSetpoint, UpdateAlarmValues as UpdateAlarmValuesParsedSetpoint, UpdateAlarmValueContent as UpdateAlarmValueContentParsedSetpoint } from '../store/SetPointParseSlice';

declare var homePath: string;
declare var apiHomePath: string;
declare var userIsAdmin: boolean;

// Thunk For Reseting Wizrd
export const ResetWizzard = createAsyncThunk('DynamicWizzard/ResetWizzard', (arg: DynamicWizzard.WizardType, { dispatch, getState }) => {
    dispatch(DynamicWizzardSlice.actions.reset(arg));
    dispatch(FetchAffectedChannels());
    dispatch(ResetParsedSetpoint());
    if (arg == 'static')
        dispatch(updateAlarmValues({ alarmDayID: null, alarmValues: [{ StartHour: 0, EndHour: 24, AlarmDayID: null, Formula: '', Value: NaN, ID: -1, AlarmID: -1 }] }))
})

// Thunk For Updating AlarmValues and sending Data to Tokenizer
export const UpdateFormula = createAsyncThunk('DynamicWizzard/UpdateFormula', (arg: { alarmDayID: number, startHour: number, formula: string }, { dispatch, getState }) => {
    const alarmValue = SelectActiveAlarmValue((getState() as Redux.StoreState), arg.alarmDayID, arg.startHour);
    dispatch(updateAlarmValueContent({ ...alarmValue, Formula: arg.formula }));
    dispatch(FetchParsedSetPoint({ AlarmDayID: arg.alarmDayID, StartHour: arg.startHour }));

});

// Thunk for Updating AlarmValues since we also have to update Results
export const updateAlarmValues = createAsyncThunk('DynamicWizzard/updateLarmValueThunk', (arg: { alarmDayID: number, alarmValues: DynamicWizzard.IAlarmvalue[] }, { dispatch }) => {
    dispatch(DynamicWizzardSlice.actions.updateAlarmValues(arg));
    dispatch(UpdateAlarmValuesParsedSetpoint(arg));
});

export const updateAlarmValueContent = createAsyncThunk('DynamicWizzard/updateAlarmValueContentThunk', (arg: DynamicWizzard.IAlarmvalue, { dispatch }) => {
    dispatch(DynamicWizzardSlice.actions.updateAlarmValueContent(arg));
    dispatch(UpdateAlarmValueContentParsedSetpoint(arg));
});

// Thunk for Saving Wizard result
export const SaveWizard = createAsyncThunk('DynamicWizzard/SaveWizard', async (_, { getState }) => {
    return await GetSave(getState() as Redux.StoreState);
});

// Thunk for Loading An Existing AlarmGroup
export const LoadWizard = createAsyncThunk('DynamicWizzard/LoadWizard', async (arg: number, { dispatch }) => {
    dispatch(LoadSelectedVoltages(arg));
    dispatch(LoadSelectedPhase(arg));
    return await GetLoad(arg);
});


export const DynamicWizzardSlice = createSlice({
    name: 'DynamicWizzard',
    initialState: {
        Step: 'general',
        AlarmGroup: {},
        Status: 'unitiated',
        SelectedMeter: [],
        SelectedMeterSort: 'Name',
        SelectedMeterASC: true,
        Error: null,
        MeasurmentTypeID: 0,
        SeriesTypeID: 0,
        AlarmDayGroupID: 0,
        StatisticsRange: { start: '', end: '' },
        StatisticsFilter: { FilterLower: false, FilterUpper: false, FilterZero: true, LowerLimit: 0, UpperLimit: 0 },
        StatisticsChannelIDs: [],
        AlarmFactors: [],
        AlarmValues: [],
        Type: 'static'

    } as DynamicWizzard.IState,
    reducers: {
        reset: (state, action: PayloadAction<DynamicWizzard.WizardType>) => {

            let dt = new Date();

            state.Step = 'general'
            state.AlarmGroup = { AlarmTypeID: 1, SeverityID: 1, ID: -1, Name: "" }
            state.Status = 'idle'
            state.SelectedMeter = []
            state.MeasurmentTypeID = 1
            state.SeriesTypeID = 1
            state.AlarmDayGroupID = 1
            state.StatisticsRange = {
                start: `${dt.getFullYear() - 1}-${(dt.getMonth() + 1).toString().padStart(2, '0')}-${dt.getDate().toString().padStart(2, '0')}`,
                end: `${dt.getFullYear()}-${(dt.getMonth() + 1).toString().padStart(2, '0')}-${dt.getDate().toString().padStart(2, '0')}`
            }
            state.StatisticsFilter = { FilterLower: false, FilterUpper: false, FilterZero: true, LowerLimit: 0, UpperLimit: 0 }
            state.StatisticsChannelIDs = []
            state.AlarmFactors = []
            state.AlarmValues = []
            state.Type = action.payload
            
        },
        next: (state) => {
            if (state.Step == 'general')
                state.Step = 'selectData'
            else if (state.Step == 'selectData') 
                state.Step = 'setpoint';                
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
        sortSelectedMeters: (state, action: PayloadAction<{ field: keyof openXDA.IMeter, asc: boolean }>) => {
            // Implement later since not strictly neccesarry for now
        },
        addMeter: (state, action: PayloadAction<openXDA.IMeter[]>) => {
            state.SelectedMeter.push(...action.payload);
            state.SelectedMeter = _.uniqBy(state.SelectedMeter, m => m.ID);
        },
        removeMeter: (state, action: PayloadAction<openXDA.IMeter[]>) => {
            state.SelectedMeter = state.SelectedMeter.filter(m => action.payload.findIndex(d => d.ID == m.ID) == -1);
        },
        updateMeasurmentTypeID: (state, action: PayloadAction<number>) => {
            state.MeasurmentTypeID = action.payload
        },
        updateSeriesTypeID: (state, action: PayloadAction<number>) => {
            state.SeriesTypeID = action.payload
        },
        updateAlarmDayGroupID: (state, action: PayloadAction<number>) => {
            state.AlarmDayGroupID = action.payload
        },
        updateStatisticsRange: (state, action: PayloadAction<SPCTools.IDateRange>) => {
            state.StatisticsRange = action.payload
        },
        updateStatisticsFilter: (state, action: PayloadAction<SPCTools.IDataFilter>) => {
            state.StatisticsFilter = action.payload
        },
        updateStatisticChannels: (state, action: PayloadAction<openXDA.IChannel[]>) => {
            state.StatisticsChannelIDs = action.payload.map(ch => ch.ID)
        },
        updateFactor: (state, action: PayloadAction<{ index: number, factor: SPCTools.IFactor }>) => {
            state.AlarmFactors[action.payload.index] = action.payload.factor
        },
        addFactor: (state) => {
            state.AlarmFactors.push({ ID: -1, SeverityID: state.AlarmGroup.SeverityID, Value: 1.05 });
        },
        removeFactor: (state, action: PayloadAction<number>) => {
            state.AlarmFactors.splice(action.payload, 1)
        },
        updateAlarmValues: (state, action: PayloadAction<{ alarmDayID: number, alarmValues: DynamicWizzard.IAlarmvalue[] }>) => {
            if (action.payload.alarmDayID == undefined) 
                state.AlarmValues = [];
            
            else 
                state.AlarmValues = state.AlarmValues.filter(v => v.AlarmDayID != action.payload.alarmDayID);
            state.AlarmValues.push(...action.payload.alarmValues);
        },
        updateAlarmValueContent: (state, action: PayloadAction<DynamicWizzard.IAlarmvalue>) => {
            let index = state.AlarmValues.findIndex(item => item.StartHour == action.payload.StartHour && item.AlarmDayID == action.payload.AlarmDayID);
            if (index == -1)
                state.AlarmValues.push(action.payload);
            else           
                state.AlarmValues[index] = action.payload;
        },
        
    },
    
     extraReducers: (builder) => {
         builder.addCase(SaveWizard.fulfilled, (state, action) => {
             state.Status = 'idle';
             state.Error = null;
             state.Step = 'done'

         });
         builder.addCase(SaveWizard.pending, (state, action) => {
             state.Status = 'loading';
         });
         builder.addCase(SaveWizard.rejected, (state, action) => {
             state.Status = 'error';
             state.Error = action.error.message;

         });
         builder.addCase(LoadWizard.fulfilled, (state, action) => {
             state.Status = 'idle';
             state.Error = null;
             state.Step = 'general'

             let loaded = JSON.parse(action.payload);
             state.AlarmGroup = loaded.AlarmGroup as SPCTools.IAlarmGroup;
             state.SelectedMeter = loaded.SelectedMeter as openXDA.IMeter[];
             state.SeriesTypeID = loaded.SeriesTypeID;
             state.MeasurmentTypeID = loaded.MeasurementTypeID;
             state.AlarmDayGroupID = loaded.AlarmDayGroupID



         });
         builder.addCase(LoadWizard.pending, (state, action) => {
             state.Status = 'loading';
         });
         builder.addCase(LoadWizard.rejected, (state, action) => {
             state.Status = 'error';
             state.Error = action.error.message;

         });

    }

});

export const {
    next, back,
    updateAlarmGroup,
    sortSelectedMeters,
    addMeter, removeMeter,
    updateMeasurmentTypeID,
    updateSeriesTypeID, updateAlarmDayGroupID,
    updateStatisticsRange, updateStatisticsFilter,
    updateStatisticChannels,
    updateFactor, addFactor, removeFactor,
    
} = DynamicWizzardSlice.actions

export default DynamicWizzardSlice.reducer;

// Selectors
export const selectTab = (state: Redux.StoreState) => state.DynamicWizzard.Step;
export const selectStatus = (state: Redux.StoreState) => state.DynamicWizzard.Status;
export const SelectWizardType = (state: Redux.StoreState) => state.DynamicWizzard.Type;


export const selectSelectedMeter = (state: Redux.StoreState) => state.DynamicWizzard.SelectedMeter;
export const selectSelectedMeterASC = (state: Redux.StoreState) => state.DynamicWizzard.SelectedMeterASC;
export const selectSelectedMeterSort = (state: Redux.StoreState) => state.DynamicWizzard.SelectedMeterSort;

export const selectMeasurmentTypeID = (state: Redux.StoreState) => state.DynamicWizzard.MeasurmentTypeID;
export const selectSeriesTypeID = (state: Redux.StoreState) => state.DynamicWizzard.SeriesTypeID;
export const selectAlarmGroup = (state: Redux.StoreState) => state.DynamicWizzard.AlarmGroup;
export const selectAlarmDayGroupID = (state: Redux.StoreState) => state.DynamicWizzard.AlarmDayGroupID;

export const SelectStatisticsrange = (state: Redux.StoreState) => state.DynamicWizzard.StatisticsRange;
export const SelectStatisticsFilter = (state: Redux.StoreState) => state.DynamicWizzard.StatisticsFilter;
export const SelectStatisticsChannels = createSelector(SelectAffectedChannels, (state: Redux.StoreState) => state.DynamicWizzard.StatisticsChannelIDs, (channels, ids) => channels.filter(item => ids.findIndex(i => i == item.ID) > -1));
export const SelectAlarmFactors = (state: Redux.StoreState) => state.DynamicWizzard.AlarmFactors;

export const SelectSetPointAlarmDays = createSelector(selectAlarmDayGroupID, SelectAlarmDayGroups, SelectAlarmDays, (id, alarmDayGroups, alarmDays) => {
    let grp = alarmDayGroups.find(item => item.ID == id);
    return alarmDays.filter(item => grp.AlarmDayIDs.findIndex(i => i == item.ID) > -1);
});

export const SelectAlarmValues = (state: Redux.StoreState, alarmDayId: number) => state.DynamicWizzard.AlarmValues.filter(item => item.AlarmDayID == alarmDayId);
export const SelectActiveAlarmValue = (state: Redux.StoreState, alarmDayId: number, alarmStartHour: number) => state.DynamicWizzard.AlarmValues.find(item => (alarmDayId == null || item.AlarmDayID == alarmDayId) && item.StartHour == alarmStartHour);

export const SelectActiveFormula = (state: Redux.StoreState, alarmDayId: number, alarmStartHour: number) => (SelectActiveAlarmValue(state, alarmDayId, alarmStartHour) != undefined ? SelectActiveAlarmValue(state, alarmDayId, alarmStartHour).Formula : "");

export const SelectAllowSlice = createSelector(SelectAffectedChannelCount, SelectStatisticsChannels, (countAll, statisticsChannels) => (countAll == statisticsChannels.length));
export const SelectThresholdValues = (state: Redux.StoreState) => state.SetPointParse.AlarmValueResults;
export const SelectAllAlarmValues = (state: Redux.StoreState) => state.DynamicWizzard.AlarmValues;

export const selectWizardEror = (state: Redux.StoreState) => state.DynamicWizzard.Error;

export const selectErrors = createSelector(
    ((state: Redux.StoreState) => (state.DynamicWizzard.Step)),
    SelectAffectedChannelCount,
    (state: Redux.StoreState) => (state.DynamicWizzard.AlarmGroup.Name != undefined && state.DynamicWizzard.AlarmGroup.Name.length > 0),
    (state: Redux.StoreState) => (state.DynamicWizzard.SelectedMeter.length > 0),
    (state: Redux.StoreState) => SelectSelectedPhases(state).some(i => i),
    (state: Redux.StoreState) => SelectSelectedVoltages(state).some(i => i),
    SelectStatisticsrange,
    (state: Redux.StoreState) => state.DynamicWizzard.StatisticsChannelIDs.length,
    (state: Redux.StoreState) => (state.DynamicWizzard.AlarmFactors),
    (state: Redux.StoreState) => state.SetPointParse.AlarmValueResults ,
    (step, channelCount, name, meterCount, phaseCount, voltageCount, statisticsRange, statisticsChannelCount, alarmFactors, setPointResults) => {
        let result = [] as StaticWizzard.IRequirement[];

        let setpointsValid = !setPointResults.some(item => item.Value.length == 0 || item.Value.some(pt => isNaN(pt.Value)));
        let setPointScalar = !setPointResults.some(item => !item.IsScalar);
        if (step == 'general') {
            result.push({ text: "A Name is required", complete: (name ? 'complete' : 'required') });
            result.push({ text: "At least 1 Meter needs to be selected", complete: (meterCount ? 'complete' : 'required') });
            result.push({ text: "At least 1 Phase needs to be selected", complete: (phaseCount ? 'complete' : 'required') });
            result.push({ text: "At least 1 Base Voltage needs to be selected", complete: (voltageCount ? 'complete' : 'required') });
            result.push({ text: "The selection needs to result in at least 1 channel", complete: (channelCount ? 'complete' : 'required') });
        }
        else if (step == 'selectData') {
            result.push({ text: "A valid start date has to be selected", complete: (statisticsRange.start != "" ? 'complete' : 'required') });
            result.push({ text: "A valid end date has to be selected", complete: (statisticsRange.end != "" ? 'complete' : 'required') })
            result.push({ text: "At least 1 channel needs to be selected", complete: (statisticsChannelCount > 0 ? 'complete' : 'required') })
            if (!(statisticsChannelCount == channelCount))
                result.push({ text: "A single threshhold will be required for all channels. Not all channels are used as historic datasource", complete: 'warning' })
        }
        else if (step == 'setpoint') {
            result.push({ text: "A valid setpoint expression is required for every period", complete: (setpointsValid ? 'complete' : 'required') });
            if (!(statisticsChannelCount == channelCount))
                result.push({ text: "A single scalar setpoint is required for all channels", complete: (setPointScalar ? 'complete' : 'required') })
            else if (!setPointScalar)
                result.push({ text: "The setpoint expression will result in different threshold for each channel", complete: 'warning' })
            if (alarmFactors.length > 0)
                result.push({ text: "No level can be applied at the original setpoint", complete: (!alarmFactors.some(item => item.Value == 1.0) ? 'complete' : 'required') })
        }
        return result;

    });


// #region [ Async Functions ]

function GetSave(state: Redux.StoreState): JQuery.jqXHR<string> {
    let request = {
        AlarmGroup: selectAlarmGroup(state),
        AlarmValues: SelectAllAlarmValues(state),
        AlarmFactors: SelectAlarmFactors(state),
        ChannelIDs: SelectAffectedChannels(state).map(ch => ch.ID),
        StatisticChannelsID: SelectStatisticsChannels(state).map(ch => ch.ID),
        StatisticsStart: SelectStatisticsrange(state).start,
        StatisticsEnd: SelectStatisticsrange(state).end,
        StatisticsFilter: SelectStatisticsFilter(state),
        SeriesTypeID: selectSeriesTypeID(state)
    }

    return $.ajax({
        type: "POST",
        url: `${apiHomePath}api/SPCTools/Wizard/Save`,
        contentType: "application/json; charset=utf-8",
        dataType: 'json',
        data: JSON.stringify(request),
        cache: false,
        async: true
    });
}

function GetLoad(id: number): JQuery.jqXHR<string> {
    
    return $.ajax({
        type: "GET",
        url: `${apiHomePath}api/SPCTools/Wizard/Load/${id}`,
        contentType: "application/json; charset=utf-8",
        dataType: 'json',
        cache: false,
        async: true
    });
}

// #endregion
