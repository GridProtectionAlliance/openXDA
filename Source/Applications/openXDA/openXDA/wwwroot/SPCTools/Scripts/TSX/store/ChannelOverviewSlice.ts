//******************************************************************************************************
//  ChannelOverviewSlice.ts - Gbtc
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
export const FetchChannelOverviews = createAsyncThunk('ChannelOverview/FetchChannelOverview', async (params: { filter, sort, ascending }, { dispatch }) => {
    return await GetChannelOverview(params.filter, params.sort, params.ascending);
});
// #endregion


// #region [ Slice ]
export const ChannelOverviewSlice = createSlice({
    name: 'ChannelOverview',
    initialState: {
        Status: 'unitiated',
        Data: [],
        Error: null,
        SortField: 'Meter',
        Ascending: true,
        Filters: []
    } as Redux.State<SPCTools.IChannelOverview>,
    reducers: {
        SortChannelOverviews: (state, action) => {
            state.Status = 'changed';
            state.Ascending = action.payload.Ascending;
            state.SortField = action.payload.SortField;
        },
        FilterChannelOverviews: (state, action) => {
            state.Status = 'changed';
            state.Filters = action.payload;
        }
        
    },
    extraReducers: (builder) => {

        builder.addCase(FetchChannelOverviews.fulfilled, (state, action) => {
            state.Status = 'idle';
            state.Error = null;

            state.Data = JSON.parse(action.payload) as SPCTools.IChannelOverview[];

        });
        builder.addCase(FetchChannelOverviews.pending, (state, action) => {
            state.Status = 'loading';
        });
        builder.addCase(FetchChannelOverviews.rejected, (state, action) => {
            state.Status = 'error';
            state.Error = action.error.message;

        });

    }

});

export const { SortChannelOverviews, FilterChannelOverviews} = ChannelOverviewSlice.actions;
export default ChannelOverviewSlice.reducer;
// #endregion

// #region [ Selectors ]
export const SelectChannelOverviews = (state: Redux.StoreState) => state.ChannelOverview.Data;
export const SelectChannelOverviewByID = (state: Redux.StoreState, id) => state.ChannelOverview.Data.find(ds => ds.ID === id) as SPCTools.IChannelOverview;
export const SelectChannelOverviewsStatus = (state: Redux.StoreState) => state.ChannelOverview.Status;
export const SelectChannelOverviewsSortField = (state: Redux.StoreState) => state.ChannelOverview.SortField;
export const SelectChannelOverviewsAscending = (state: Redux.StoreState) => state.ChannelOverview.Ascending;
export const SelectChannelOverviewsFilters = (state: Redux.StoreState) => state.ChannelOverview.Filters;

// #endregion

// #region [ Async Functions ]

function GetChannelOverview(filters, sort, asc): JQuery.jqXHR<string> {
    return $.ajax({
        type: "POST",
        url: `${apiHomePath}api/SPCTools/ChannelOverview/SearchableList`,
        contentType: "application/json; charset=utf-8",
        dataType: 'json',
        data: JSON.stringify({ Searches: filters, OrderBy: sort, Ascending: asc }),
        cache: false,
        async: true
    });
}


// #endregion
