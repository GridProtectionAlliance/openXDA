//******************************************************************************************************
//  HistoryTestSlice.tsx - Gbtc
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
import { selectTokenizerRequest, selectAffectedChannels } from '../StaticAlarm/StaticWizzardSlice';

declare var homePath: string;
declare var apiHomePath: string;
declare var userIsAdmin: boolean;


//Thunks to load general data
export const loadTest = createAsyncThunk('HistoryTest/loadTest', async (arg: number, thunkAPI) => {
    if (arg == -1)
        return await loadStaticWizzardTest((thunkAPI.getState() as Redux.StoreState))
    else
        return
    //Eventually we can also load Tests for AlarmGroups out of the DB here
})



export const HistoryTestSlice = createSlice({
    name: 'HistoryTest',
    initialState: {
        loading: false,
        time: {
            start: `${(new Date()).getFullYear() - 1}-${((new Date()).getMonth() + 1).toString().padStart(2, '0')}-${(new Date()).getDate().toString().padStart(2, '0')}`,
            end: `${(new Date()).getFullYear()}-${((new Date()).getMonth() + 1).toString().padStart(2, '0')}-${(new Date()).getDate().toString().padStart(2, '0')}`
        } as SPCTools.IDateRange,
        alarmGroupID: -1,
        result: []
    } as SPCTools.IHistoryTestState,
    reducers: {
        setDate: (state, action: PayloadAction<SPCTools.IDateRange>) => {
            state.time = action.payload;
            return state;
        },
    },
     extraReducers: (builder) => {

         builder.addCase(loadTest.pending, (state, action) => {
             state.loading = true;
             state.result = undefined;
        });
         builder.addCase(loadTest.fulfilled, (state, action) => {
             state.loading = false;
             state.alarmGroupID = action.meta.arg;
             state.result = action.payload
             return state
         });
         builder.addCase(loadTest.rejected, (state, action) => {
             state.loading = false;
             
             // Need Error Message Here
         });
    }

});

export default HistoryTestSlice.reducer;

export const { setDate } = HistoryTestSlice.actions;

//Selectors
export const selectIsLoading = (state: Redux.StoreState) => state.HistoryTest.loading;
export const selectResultSummary = (state: Redux.StoreState) => state.HistoryTest.result;
export const selectDateRange = (state: Redux.StoreState) => state.HistoryTest.time;

// Async Functions

function loadStaticWizzardTest(state: Redux.StoreState): JQuery.jqXHR<SPCTools.IChannelTest[]> {
    let request = {
        AlarmFactors: state.StaticWizzard.AlarmFactors.map(i => i.Value),
        SetPointRequest: selectTokenizerRequest(state),
        Start: state.HistoryTest.time.start,
        End: state.HistoryTest.time.end,
        ChannelID: selectAffectedChannels(state).map(channel => channel.ID)
    };

    let handle = $.ajax({
        type: "POST",
        url: `${apiHomePath}api/SPCTools/AlarmGroupTest/TestWizzard`,
        contentType: "application/json; charset=utf-8",
        dataType: 'json',
        data: JSON.stringify(request),
        cache: false,
        async: true
    });
    return handle;
}

