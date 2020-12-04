//******************************************************************************************************
//  GeneralSettingsSlice.tsx - Gbtc
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
import { createSlice, createAsyncThunk, PayloadAction } from '@reduxjs/toolkit';
import { SPCTools, StaticWizzard, openXDA, Redux } from '../global';
import RequestHandle from './RequestHandle';
import _ from 'lodash';

declare var homePath: string;
declare var apiHomePath: string;
declare var userIsAdmin: boolean;

const handles = new RequestHandle();

//Thunks to load general data
export const fetchMeasurmentTypes = createAsyncThunk('GeneralSettings/fetchMeasurementTypes', async (_,thunkAPI) => {
    return await getMeasurementTypes()
})
export const fetchAlarmTypes = createAsyncThunk('GeneralSettings/fetchAlarmTypes', async (_, thunkAPI) => {
    return await getAlarmTypes()
})
export const fetchSeverities = createAsyncThunk('GeneralSettings/fetchSeverities', async (_, thunkAPI) => {
    return await getSeverities()
})


export const GeneralSettingsSlice = createSlice({
    name: 'GeneralSettings',
    initialState: {
        Tab: 'Home',
        AlarmTypes: [],
        MeasurementTypes: [],
        Phases: [],
        BaseVoltages: [],
        Severities: [],
        loadingAlarmTypes: false,
        loadingMeasurementTypes: false,
        loadingSeverities: false,
    } as SPCTools.ISettingsState,
    reducers: {
        setPage: (state, action: PayloadAction<SPCTools.Page>) => {
            state.Tab = action.payload;
            return state;
        }
    },
     extraReducers: (builder) => {

         builder.addCase(fetchMeasurmentTypes.pending, (state, action) => {
             state.loadingMeasurementTypes = true;
        });
         builder.addCase(fetchMeasurmentTypes.fulfilled, (state, action) => {
             state.loadingMeasurementTypes = false;
             state.MeasurementTypes = JSON.parse(action.payload);
         });
         builder.addCase(fetchMeasurmentTypes.rejected, (state, action) => {
             state.loadingMeasurementTypes = false;
             // Need Error Message Here
         });


         builder.addCase(fetchSeverities.pending, (state, action) => {
             state.loadingSeverities = true;
         });
         builder.addCase(fetchSeverities.fulfilled, (state, action) => {
             state.loadingSeverities = false;
             state.Severities = action.payload;
         });
         builder.addCase(fetchSeverities.rejected, (state, action) => {
             state.loadingSeverities = false;
             // Need Error Message Here
         });

         builder.addCase(fetchAlarmTypes.pending, (state, action) => {
             state.loadingAlarmTypes = true;
         });
         builder.addCase(fetchAlarmTypes.fulfilled, (state, action) => {
             state.loadingAlarmTypes = false;
             state.AlarmTypes = JSON.parse(action.payload);
         });
         builder.addCase(fetchAlarmTypes.rejected, (state, action) => {
             state.loadingAlarmTypes = false;
               // Need Error Message Here
         });
    }

});

export default GeneralSettingsSlice.reducer;

export const { setPage } = GeneralSettingsSlice.actions;

//Selectors
export const selectIsLoading = (state: Redux.StoreState) => state.GeneralSettings.loadingAlarmTypes || state.GeneralSettings.loadingMeasurementTypes || state.GeneralSettings.loadingSeverities;

export const selectPage = (state: Redux.StoreState) => state.GeneralSettings.Tab;

export const selectAlarmTypes = (state: Redux.StoreState) => state.GeneralSettings.AlarmTypes;
export const selectMeasurmentTypes = (state: Redux.StoreState) => state.GeneralSettings.MeasurementTypes;
export const selectSeverities = (state: Redux.StoreState) => state.GeneralSettings.Severities;
// Async Functions

function getMeasurementTypes(): JQuery.jqXHR<string> {
    let handle = $.ajax({
        type: "GET",
        url: `${apiHomePath}api/ChannelGroupType`,
        contentType: "application/json; charset=utf-8",
        dataType: 'json',
        cache: false,
        async: true
    });

    return handle;
}

function getAlarmTypes(): JQuery.jqXHR<string> {
    let handle = $.ajax({
        type: "GET",
        url: `${apiHomePath}api/AlarmType`,
        contentType: "application/json; charset=utf-8",
        dataType: 'json',
        cache: false,
        async: true
    });
    return handle;
}

function getSeverities(): JQuery.jqXHR<Array<SPCTools.ISeverity>> {

    let handle = $.ajax({
        type: "GET",
        url: `${apiHomePath}api/SPCTools/StaticAlarmCreation/Severity`,
        contentType: "application/json; charset=utf-8",
        dataType: 'json',
        cache: false,
        async: true
    });
    return handle;
}
