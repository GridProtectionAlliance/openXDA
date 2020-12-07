//******************************************************************************************************
//  SetpointParseSlice.ts - Gbtc
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
//  12/03/2020 - C. Lackner
//       Generated original version of source code.
//
//******************************************************************************************************

import { createSlice, createAsyncThunk, PayloadAction } from '@reduxjs/toolkit';
import { SPCTools, Redux, openXDA, DynamicWizzard } from '../global';
import _ from 'lodash';
import { SelectStatisticsFilter, SelectStatisticsrange, SelectStatisticsChannels, SelectActiveAlarmValue } from '../DynamicAlarm/DynamicWizzardSlice';

interface ParsedSetpointParam { content: DynamicWizzard.ITokenParseResponse, channelIDs: number[] }
// #region [ Thunks ]
export const FetchParsedSetPoint = createAsyncThunk('SetPointParse/FetchParsedSetPoint', async (param: { AlarmDayID: number, StartHour: number }, { getState, dispatch }) => {
    let dataFilter = SelectStatisticsFilter(getState() as Redux.StoreState);
    let timeRange = SelectStatisticsrange(getState() as Redux.StoreState);
    let channelIDs = SelectStatisticsChannels(getState() as Redux.StoreState).map(ch => ch.ID);

    let token = SelectActiveAlarmValue(getState() as Redux.StoreState, param.AlarmDayID, param.StartHour);

    if (token == undefined)
        return Promise.resolve({ content: { Valid: false, IsScalar: false, Message: "An unidentified error occured when parsing the setpoint", Value: [] }, channelIDs: channelIDs } as ParsedSetpointParam);
    if (token.Formula == "") {
        return Promise.resolve({ content: { Valid: false, IsScalar: false, Message: "Expression can not be empty.", Value: [] }, channelIDs: channelIDs } as ParsedSetpointParam);
    }
    let handle = await GetParsedSetPoint(token, dataFilter, timeRange.start, timeRange.end, channelIDs);
    return { content: handle, channelIDs: channelIDs };
});

// #endregion


// #region [ Slice ]
export const SetpointParseSlice = createSlice({
    name: 'SetPointParse',
    initialState: {
        Status: 'idle',
        Response: undefined,
        Error: null,
        AlarmValueResults: []

    } as DynamicWizzard.ISetPointParseState,
    reducers: {
        Reset: (state) => {
            state.AlarmValueResults = [];
        },
        UpdateAlarmValueContent: (state, action: PayloadAction<DynamicWizzard.IAlarmvalue >) => {
            let index = state.AlarmValueResults.findIndex(item => item.StartHour == action.payload.StartHour && item.AlarmDayID == action.payload.AlarmDayID);
            if (index == -1)
                state.AlarmValueResults.push({ AlarmDayID: action.payload.AlarmDayID, StartHour: action.payload.StartHour, Value: [], IsScalar: true });
        },
        UpdateAlarmValues: (state, action: PayloadAction<{ alarmDayID: number, alarmValues: DynamicWizzard.IAlarmvalue[] }>) => {
            if (action.payload.alarmDayID == undefined) 
                state.AlarmValueResults = [];
            else 
                state.AlarmValueResults = state.AlarmValueResults.filter(v => v.AlarmDayID != action.payload.alarmDayID);
            state.AlarmValueResults.push(...action.payload.alarmValues.map(item => { return { AlarmDayID: item.AlarmDayID, StartHour: item.StartHour, Value: [], IsScalar: true } }));
        },
    },
    extraReducers: (builder) => {

        builder.addCase(FetchParsedSetPoint.fulfilled, (state, action) => {
            state.Status = 'idle';
            state.Error = null;

            state.Response = action.payload.content;

            let index = state.AlarmValueResults.findIndex(item => item.StartHour == action.meta.arg.StartHour && item.AlarmDayID == action.meta.arg.AlarmDayID);
            if (index > -1)
                state.AlarmValueResults[index] = {
                    AlarmDayID: action.meta.arg.AlarmDayID,
                    StartHour: action.meta.arg.StartHour,
                    Value: action.payload.channelIDs.map((chID, index) => { return { ChannelID: chID, Value: (action.payload.content.Valid ? action.payload.content.Value[index] : NaN) } }),
                    IsScalar: action.payload.content.IsScalar
                };
            
               
        });
        builder.addCase(FetchParsedSetPoint.pending, (state, action) => {
            state.Status = 'loading';
        });
        builder.addCase(FetchParsedSetPoint.rejected, (state, action) => {
            state.Status = 'error';
            state.Error = action.error.message;

        });

    }

});

export default SetpointParseSlice.reducer;


// #endregion

export const {
    Reset, UpdateAlarmValueContent, UpdateAlarmValues
} = SetpointParseSlice.actions

// #region [ Selectors ]
export const SelectSetPointParseStatus = (state: Redux.StoreState) => state.SetPointParse.Status;
export const SelectSetPointParseResult = (state: Redux.StoreState) => state.SetPointParse.Response;

// #endregion

// #region [ Async Functions ]

function GetParsedSetPoint(token: DynamicWizzard.IAlarmvalue, dataFilter: SPCTools.IDataFilter, start: string, end: string, channelIDs: number[]): JQuery.jqXHR<DynamicWizzard.ITokenParseResponse> {

    let request = {
        TokeValue: token,
        StatisticsFilter: dataFilter,
        StatisticsStart: start,
        StatisticsEnd: end,
        StatisticsChannelID: channelIDs,
    }
    return $.ajax({
        type: "POST",
        url: `${apiHomePath}api/SPCTools/Token/Parse`,
        contentType: "application/json; charset=utf-8",
        dataType: 'json',
        data: JSON.stringify(request),
        cache: false,
        async: true
    });
}

// #endregion
