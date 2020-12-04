//******************************************************************************************************
//  AlarmGroupViewSlice.ts - Gbtc
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
export const FetchAlarmGroupViews = createAsyncThunk('AlarmGroupView/FetchAlarmGroupView', async (params: { filter, sort, ascending }, { dispatch }) => {
    return await GetAlarmGroupView(params.filter, params.sort, params.ascending);
});
// #endregion


// #region [ Slice ]
export const AlarmGroupViewSlice = createSlice({
    name: 'AlarmGroup',
    initialState: {
        Status: 'unitiated',
        Data: [],
        Error: null,
        SortField: 'Name',
        Ascending: true,
        Filters: []
    } as Redux.State<SPCTools.IAlarmGroupView>,
    reducers: {
        SortAlarmGroups: (state, action) => {
            state.Status = 'changed';
            state.Ascending = action.payload.Ascending;
            state.SortField = action.payload.SortField;
        },
        FilterAlarmGroups: (state, action) => {
            state.Status = 'changed';
            state.Filters = action.payload;
        }
        
    },
    extraReducers: (builder) => {

        builder.addCase(FetchAlarmGroupViews.fulfilled, (state, action) => {
            state.Status = 'idle';
            state.Error = null;

            state.Data = JSON.parse(action.payload) as SPCTools.IAlarmGroupView[];

        });
        builder.addCase(FetchAlarmGroupViews.pending, (state, action) => {
            state.Status = 'loading';
        });
        builder.addCase(FetchAlarmGroupViews.rejected, (state, action) => {
            state.Status = 'error';
            state.Error = action.error.message;

        });

    }

});

export const { SortAlarmGroups, FilterAlarmGroups } = AlarmGroupViewSlice.actions;
export default AlarmGroupViewSlice.reducer;
// #endregion

// #region [ Selectors ]
export const SelectAlarmGroups = (state: Redux.StoreState) => state.AlarmGroupView.Data;
export const SelectAlarmGroupByID = (state: Redux.StoreState, id) => state.AlarmGroupView.Data.find(ds => ds.ID === id);
export const SelectAlarmGroupsStatus = (state: Redux.StoreState) => state.AlarmGroupView.Status;
export const SelectAlarmGroupsSortField = (state: Redux.StoreState) => state.AlarmGroupView.SortField;
export const SelectAlarmGroupsAscending = (state: Redux.StoreState) => state.AlarmGroupView.Ascending;
export const SelectAlarmGroupsFilters = (state: Redux.StoreState) => state.AlarmGroupView.Filters;

// #endregion

// #region [ Async Functions ]

function GetAlarmGroupView(filters, sort, asc): JQuery.jqXHR<string> {
    return $.ajax({
        type: "POST",
        url: `${apiHomePath}api/SPCTools/AlarmGroupView/SearchableList`,
        contentType: "application/json; charset=utf-8",
        dataType: 'json',
        data: JSON.stringify({ Searches: filters, OrderBy: sort, Ascending: asc }),
        cache: false,
        async: true
    });
}


// #endregion
