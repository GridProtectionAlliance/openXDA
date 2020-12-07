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

// #region [ Thunks ]
export const FetchParsedSetPoint = createAsyncThunk('SetPointParse/FetchParsedSetPoint', async (param: { AlarmDayID: number, StartHour: number }, { getState, dispatch }) => {
    let dataFilter = SelectStatisticsFilter(getState() as Redux.StoreState);
    let timeRange = SelectStatisticsrange(getState() as Redux.StoreState);
    let channelIDs = SelectStatisticsChannels(getState() as Redux.StoreState).map(ch => ch.ID);

    let token = SelectActiveAlarmValue(getState() as Redux.StoreState, param.AlarmDayID, param.StartHour);

    if (token == undefined)
        return Promise.resolve({ Valid: false, IsScalar: false, Message: "An unidentified error occured when parsing the setpoint", Value: [] });
    if (token.Formula == "") {
        return Promise.resolve({ Valid: false, IsScalar: false, Message: "Expression can not be empty.", Value: [] });
    }
        
    return await GetParsedSetPoint(token, dataFilter, timeRange.start, timeRange.end, channelIDs);
});

// #endregion


// #region [ Slice ]
export const SetpointParseSlice = createSlice({
    name: 'SetPointParse',
    initialState: {
        Status: 'unitiated',
        Response: undefined,
        Error: null,
        
    } as DynamicWizzard.ISetPointParseState,
    reducers: {
       
    },
    extraReducers: (builder) => {

        builder.addCase(FetchParsedSetPoint.fulfilled, (state, action) => {
            state.Status = 'idle';
            state.Error = null;

            state.Response = action.payload;
           

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
