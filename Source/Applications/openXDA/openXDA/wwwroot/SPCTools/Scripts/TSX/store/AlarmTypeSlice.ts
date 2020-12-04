//******************************************************************************************************
//  AlarmTypeSlice.ts - Gbtc
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
import { SPCTools, Redux, DynamicWizzard, openXDA } from '../global';
import _ from 'lodash';

// #region [ Thunks ]
export const FetchAlarmType = createAsyncThunk('AlarmType/FetchAlarmType', async (_, { dispatch }) => {
    return await GetAlarmTypes();
});
// #endregion


// #region [ Slice ]
export const AlarmTypeSlice = createSlice({
    name: 'AlarmType',
    initialState: {
        Status: 'unitiated',
        Data: [],
        Error: null,
        SortField: 'ID',
        Ascending: true
    } as Redux.State<SPCTools.IAlarmType>,
    reducers: {
       

    },
    extraReducers: (builder) => {

        builder.addCase(FetchAlarmType.fulfilled, (state, action) => {
            state.Status = 'idle';
            state.Error = null;
            state.Data = JSON.parse(action.payload) as SPCTools.IAlarmType[];

        });
        builder.addCase(FetchAlarmType.pending, (state, action) => {
            state.Status = 'loading';
        });
        builder.addCase(FetchAlarmType.rejected, (state, action) => {
            state.Status = 'error';
            state.Error = action.error.message;

        });

    }

});

export default AlarmTypeSlice.reducer;
// #endregion

// #region [ Selectors ]
export const SelectAlarmTypes = (state: Redux.StoreState) => state.AlarmType.Data;
export const SelectAlarmTypeByID = (state: Redux.StoreState, id) => state.AlarmType.Data.find(ds => ds.ID === id);
export const SelectAlarmTypeStatus = (state: Redux.StoreState) => state.AlarmType.Status;


// #endregion

// #region [ Async Functions ]

function GetAlarmTypes(): JQuery.jqXHR<string> {
    // not really an int but this avoids some of the replacment going on server side
    let filter = [{
        FieldName: "Name",
        SearchText: "('Upper Limit','Lower Limit')",
        Operator: "IN",
        Type: "integer"
    }];

    return $.ajax({
        type: "POST",
        url: `${apiHomePath}api/AlarmType/SearchableList`,
        contentType: "application/json; charset=utf-8",
        dataType: 'json',
        data: JSON.stringify({ Searches: filter, OrderBy: 'ID', Ascending: false  }),
        cache: false,
        async: true
    });
}


// #endregion
