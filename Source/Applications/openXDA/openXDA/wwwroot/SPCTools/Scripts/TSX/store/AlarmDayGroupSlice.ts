﻿//******************************************************************************************************
//  AlarmDayGroupSlice.ts - Gbtc
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
import { Redux, DynamicWizzard } from '../global';
import _ from 'lodash';

// #region [ Thunks ]
export const FetchAlarmDayGroup = createAsyncThunk('AlarmDayGroup/FetchAlarmdayGroup', async (_, { dispatch }) => {
    return await GetAlarmDayGroup();
});
// #endregion


// #region [ Slice ]
export const AlarmDayGroupSlice = createSlice({
    name: 'AlarmDayGroup',
    initialState: {
        Status: 'unitiated',
        Data: [],
        Error: null,
        SortField: 'Description',
        Ascending: true
    } as Redux.State<DynamicWizzard.IAlarmDayGroup>,
    reducers: {
       

    },
    extraReducers: (builder) => {

        builder.addCase(FetchAlarmDayGroup.fulfilled, (state, action) => {
            state.Status = 'idle';
            state.Error = null;
            let tmp = action.payload;
            let grp = _.groupBy(tmp, item => item.ID as number);
            state.Data = Object.keys(grp).map(id => {
                return {
                    ID: parseInt(id),
                    Description: grp[id][0].Description,
                    AlarmDayIDs: grp[id].map(a => a.AlarmDayID as number)
                } as DynamicWizzard.IAlarmDayGroup;
            })
        });
        builder.addCase(FetchAlarmDayGroup.pending, (state, action) => {
            state.Status = 'loading';
        });
        builder.addCase(FetchAlarmDayGroup.rejected, (state, action) => {
            state.Status = 'error';
            state.Error = action.error.message;

        });

    }

});

export default AlarmDayGroupSlice.reducer;
// #endregion

// #region [ Selectors ]
export const SelectAlarmDayGroups = (state: Redux.StoreState) => state.AlarmDayGroup.Data;
export const SelectAlarmDayGroupByID = (state: Redux.StoreState, id) => state.AlarmDayGroup.Data.find(ds => ds.ID === id);
export const SelectAlarmDayGroupsStatus = (state: Redux.StoreState) => state.AlarmDayGroup.Status;


// #endregion

// #region [ Async Functions ]

function GetAlarmDayGroup(): JQuery.jqXHR<any> {
    return $.ajax({
        type: "GET",
        url: `${apiHomePath}api/AlarmDayGroup`,
        contentType: "application/json; charset=utf-8",
        dataType: 'json',
        cache: false,
        async: true
    });
}


// #endregion
