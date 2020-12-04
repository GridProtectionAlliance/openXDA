//******************************************************************************************************
//  SeriesTypeSlice.ts - Gbtc
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
import { SPCTools, Redux, openXDA } from '../global';
import _ from 'lodash';

// #region [ Thunks ]
export const FetchSeriesType = createAsyncThunk('SeriesType/FetchSeriesType', async (_, { dispatch }) => {
    return await GetSeriesType();
});
// #endregion


// #region [ Slice ]
export const SeriesTypeSlice = createSlice({
    name: 'SeriesType',
    initialState: {
        Status: 'unitiated',
        Data: [],
        Error: null,
        SortField: 'Name',
        Ascending: true
    } as Redux.State<openXDA.ISeriesType>,
    reducers: {
       

    },
    extraReducers: (builder) => {

        builder.addCase(FetchSeriesType.fulfilled, (state, action) => {
            state.Status = 'idle';
            state.Error = null;
            state.Data = JSON.parse(action.payload) as openXDA.ISeriesType[];

        });
        builder.addCase(FetchSeriesType.pending, (state, action) => {
            state.Status = 'loading';
        });
        builder.addCase(FetchSeriesType.rejected, (state, action) => {
            state.Status = 'error';
            state.Error = action.error.message;

        });

    }

});

export default SeriesTypeSlice.reducer;
// #endregion

// #region [ Selectors ]
export const SelectSeriesTypes = (state: Redux.StoreState) => state.SeriesType.Data;
export const SelectSeriesTypeByID = (state: Redux.StoreState, id) => state.SeriesType.Data.find(ds => ds.ID === id);
export const SelectSeriesTypeStatus = (state: Redux.StoreState) => state.SeriesType.Status;
export const SelectSeriesTypeSortField = (state: Redux.StoreState) => state.SeriesType.SortField;
export const SelectSeriesTypesAscending = (state: Redux.StoreState) => state.SeriesType.Ascending;

// #endregion

// #region [ Async Functions ]

function GetSeriesType(): JQuery.jqXHR<string> {
    // not really an int but this avoids some of the replacment going on server side
    let filter = [{
        FieldName: "Name",
        SearchText: "('Minimum','Maximum','Average')",
        Operator: "IN",
        Type: "integer"
    }];

    return $.ajax({
        type: "POST",
        url: `${apiHomePath}api/SeriesType/SearchableList`,
        contentType: "application/json; charset=utf-8",
        dataType: 'json',
        data: JSON.stringify({ Searches: filter, OrderBy: 'ID', Ascending: true }),
        cache: false,
        async: true
    });
}


// #endregion
