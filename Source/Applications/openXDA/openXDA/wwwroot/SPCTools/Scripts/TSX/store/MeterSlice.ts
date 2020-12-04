//******************************************************************************************************
//  MeterSlice.ts - Gbtc
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
import { SPCTools, Redux, openXDA } from '../global';
import _ from 'lodash';

// #region [ Thunks ]
export const FetchMeters = createAsyncThunk('Meter/FetchMeter', async (params: { filter, sort, ascending }, { dispatch }) => {
    return await GetMeter(params.filter, params.sort, params.ascending);
});
// #endregion


// #region [ Slice ]
export const MeterSlice = createSlice({
    name: 'Meter',
    initialState: {
        Status: 'unitiated',
        Data: [],
        Error: null,
        SortField: 'Name',
        Ascending: true,
        Filters: []
    } as Redux.State<openXDA.IMeter>,
    reducers: {
        SortMeters: (state, action) => {
            state.Status = 'changed';
            state.Ascending = action.payload.Ascending;
            state.SortField = action.payload.SortField;
        },
        FilterMeters: (state, action) => {
            state.Status = 'changed';
            state.Filters = action.payload;
        }
        
    },
    extraReducers: (builder) => {

        builder.addCase(FetchMeters.fulfilled, (state, action) => {
            state.Status = 'idle';
            state.Error = null;

            state.Data = action.payload as openXDA.IMeter[];

        });
        builder.addCase(FetchMeters.pending, (state, action) => {
            state.Status = 'loading';
        });
        builder.addCase(FetchMeters.rejected, (state, action) => {
            state.Status = 'error';
            state.Error = action.error.message;

        });

    }

});

export const { SortMeters, FilterMeters} = MeterSlice.actions;
export default MeterSlice.reducer;
// #endregion

// #region [ Selectors ]
export const SelectMeters = (state: Redux.StoreState) => state.Meter.Data;
export const SelectMeterByID = (state: Redux.StoreState, id) => state.Meter.Data.find(ds => ds.ID === id) as openXDA.IMeter;
export const SelectMetersStatus = (state: Redux.StoreState) => state.Meter.Status;
export const SelectMetersSortField = (state: Redux.StoreState) => state.Meter.SortField;
export const SelectMetersAscending = (state: Redux.StoreState) => state.Meter.Ascending;
export const SelectMetersFilters = (state: Redux.StoreState) => state.Meter.Filters;

// #endregion

// #region [ Async Functions ]

function GetMeter(filters, sort, asc): JQuery.jqXHR<openXDA.IMeter[]> {
    return $.ajax({
        type: "POST",
        url: `${apiHomePath}api/Meter/SearchableList`,
        contentType: "application/json; charset=utf-8",
        dataType: 'json',
        data: JSON.stringify({ Searches: filters, OrderBy: sort, Ascending: asc }),
        cache: false,
        async: true
    });
}


// #endregion
