//******************************************************************************************************
//  AlarmDaySlice.ts - Gbtc
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

import { createSlice, createAsyncThunk } from '@reduxjs/toolkit';
import { SPCTools, Redux, DynamicWizzard } from '../global';
import _ from 'lodash';

// #region [ Thunks ]
export const FetchAlarmDay = createAsyncThunk('AlarmDay/FetchAlarmday', async (_, { dispatch }) => {
    return await GetAlarmDay();
});
// #endregion


// #region [ Slice ]
export const AlarmDaySlice = createSlice({
    name: 'AlarmDay',
    initialState: {
        Status: 'unitiated',
        Data: [],
        Error: null,
        SortField: 'Name',
        Ascending: true
    } as Redux.State<DynamicWizzard.IAlarmDay>,
    reducers: {
       

    },
    extraReducers: (builder) => {

        builder.addCase(FetchAlarmDay.fulfilled, (state, action) => {
            state.Status = 'idle';
            state.Error = null;
            state.Data = JSON.parse(action.payload) as DynamicWizzard.IAlarmDay[];

        });
        builder.addCase(FetchAlarmDay.pending, (state, action) => {
            state.Status = 'loading';
        });
        builder.addCase(FetchAlarmDay.rejected, (state, action) => {
            state.Status = 'error';
            state.Error = action.error.message;

        });

    }

});

export default AlarmDaySlice.reducer;
// #endregion

// #region [ Selectors ]
export const SelectChannelAlarmGroups = (state: Redux.StoreState) => state.ChannelAlarmGroup.Data;
export const SelectChannelAlarmGroupByID = (state: Redux.StoreState, id) => state.ChannelAlarmGroup.Data.find(ds => ds.ID === id) as SPCTools.IChannelAlarmGroup;
export const SelectChannelAlarmGroupsStatus = (state: Redux.StoreState) => state.ChannelAlarmGroup.Status;
export const SelectChannelAlarmGroupsSortField = (state: Redux.StoreState) => state.ChannelAlarmGroup.SortField;
export const SelectChannelAlarmGroupsAscending = (state: Redux.StoreState) => state.ChannelAlarmGroup.Ascending;

// #endregion

// #region [ Async Functions ]

function GetAlarmDay(): JQuery.jqXHR<string> {
    return $.ajax({
        type: "GET",
        url: `${apiHomePath}api/AlarmDay`,
        contentType: "application/json; charset=utf-8",
        dataType: 'json',
        cache: false,
        async: true
    });
}


// #endregion
