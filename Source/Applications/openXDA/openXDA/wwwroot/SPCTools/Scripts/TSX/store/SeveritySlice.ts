//******************************************************************************************************
//  SeveritySlice.ts - Gbtc
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
export const FetchSeverities = createAsyncThunk('Severity/FetchSeverities', async (_, { dispatch }) => {
    return await GetAlarmSeverities();
});
// #endregion


// #region [ Slice ]
export const SeveritySlice = createSlice({
    name: 'Severity',
    initialState: {
        Status: 'unitiated',
        Data: [],
        Error: null,
        SortField: 'Name',
        Ascending: true
    } as Redux.State<SPCTools.ISeverity>,
    reducers: {
       

    },
    extraReducers: (builder) => {

        builder.addCase(FetchSeverities.fulfilled, (state, action) => {
            state.Status = 'idle';
            state.Error = null;
            state.Data = JSON.parse(action.payload) as SPCTools.ISeverity[];

        });
        builder.addCase(FetchSeverities.pending, (state, action) => {
            state.Status = 'loading';
        });
        builder.addCase(FetchSeverities.rejected, (state, action) => {
            state.Status = 'error';
            state.Error = action.error.message;

        });

    }

});

export default SeveritySlice.reducer;
// #endregion

// #region [ Selectors ]
export const SelectSeverities = (state: Redux.StoreState) => state.Severity.Data;
export const SelectSeverityByID = (state: Redux.StoreState, id) => state.Severity.Data.find(ds => ds.ID === id);
export const SelectSeverityStatus = (state: Redux.StoreState) => state.Severity.Status;

// #endregion

// #region [ Async Functions ]

function GetAlarmSeverities(): JQuery.jqXHR<string> {
    return $.ajax({
        type: "GET",
        url: `${apiHomePath}api/SPCTools/AlarmSeverity`,
        contentType: "application/json; charset=utf-8",
        dataType: 'json',
        cache: false,
        async: true
    });
}


// #endregion
