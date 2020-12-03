//******************************************************************************************************
//  ChannelAlarmGroupSlice.ts - Gbtc
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
//  12/03/2020 - Billy Ernest
//       Generated original version of source code.
//
//******************************************************************************************************

import { createSlice, createAsyncThunk } from '@reduxjs/toolkit';
import { SPCTools, Redux } from '../global';
import _ from 'lodash';

// #region [ Thunks ]
export const FetchChannelAlarmGroups = createAsyncThunk('ChannelAlarmGroup/FetchChannelAlarmGroup', async (params: { channelID,sort, ascending }, { dispatch }) => {
    return await GetChannelAlarmGroup(params.channelID, params.sort, params.ascending);
});
// #endregion


// #region [ Slice ]
export const ChannelAlarmGroupSlice = createSlice({
    name: 'ChannelAlarmGroup',
    initialState: {
        Status: 'unitiated',
        Data: [],
        Error: null,
        SortField: 'Name',
        Ascending: true
    } as Redux.State<SPCTools.IChannelAlarmGroup>,
    reducers: {
        SortChannelAlarmGroups: (state, action) => {
            state.Status = 'changed';
            state.Ascending = action.payload.Ascending;
            state.SortField = action.payload.SortField;
        },
        ChangeStatusChannelAlarmGroups: (state) => {
            state.Status = 'changed';
        },

    },
    extraReducers: (builder) => {

        builder.addCase(FetchChannelAlarmGroups.fulfilled, (state, action) => {
            state.Status = 'idle';
            state.Error = null;
            state.Data = JSON.parse(action.payload) as SPCTools.IChannelAlarmGroup[];

        });
        builder.addCase(FetchChannelAlarmGroups.pending, (state, action) => {
            state.Status = 'loading';
        });
        builder.addCase(FetchChannelAlarmGroups.rejected, (state, action) => {
            state.Status = 'error';
            state.Error = action.error.message;

        });

    }

});

export const { SortChannelAlarmGroups, ChangeStatusChannelAlarmGroups} = ChannelAlarmGroupSlice.actions;
export default ChannelAlarmGroupSlice.reducer;
// #endregion

// #region [ Selectors ]
export const SelectChannelAlarmGroups = (state: Redux.StoreState) => state.ChannelAlarmGroup.Data;
export const SelectChannelAlarmGroupByID = (state: Redux.StoreState, id) => state.ChannelAlarmGroup.Data.find(ds => ds.ID === id) as SPCTools.IChannelAlarmGroup;
export const SelectChannelAlarmGroupsStatus = (state: Redux.StoreState) => state.ChannelAlarmGroup.Status;
export const SelectChannelAlarmGroupsSortField = (state: Redux.StoreState) => state.ChannelAlarmGroup.SortField;
export const SelectChannelAlarmGroupsAscending = (state: Redux.StoreState) => state.ChannelAlarmGroup.Ascending;

// #endregion

// #region [ Async Functions ]

function GetChannelAlarmGroup(channelID: number, sort: keyof SPCTools.IChannelAlarmGroup, asc: boolean): JQuery.jqXHR<string> {
    return $.ajax({
        type: "GET",
        url: `${apiHomePath}api/SPCTools/ChannelAlarmGroup/${channelID}/${sort}/${asc ? 1 : 0}`,
        contentType: "application/json; charset=utf-8",
        dataType: 'json',
        cache: false,
        async: true
    });
}


// #endregion
