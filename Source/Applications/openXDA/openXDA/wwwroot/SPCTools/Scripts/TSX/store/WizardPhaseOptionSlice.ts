//******************************************************************************************************
//  WizardPhaseOptionSlice.ts - Gbtc
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

import { createSlice, createAsyncThunk, PayloadAction } from '@reduxjs/toolkit';
import { SPCTools, Redux, openXDA } from '../global';
import _ from 'lodash';
import { selectSelectedMeter, selectMeasurmentTypeID, selectSeriesTypeID } from '../DynamicAlarm/DynamicWizzardSlice';
import { SelectSeriesTypes } from './SeriesTypeSlice';

// #region [ Thunks ]
export const FetchAvailablePhases = createAsyncThunk('WizardPhaseOption/FetchChannelOverview', async ( _, { getState }) => {
    let meterIDs = selectSelectedMeter(getState() as Redux.StoreState).map(m => m.ID);
    let measurmentTypeID = selectMeasurmentTypeID(getState() as Redux.StoreState);
    let seriesTypeID = SelectSeriesTypes(getState() as Redux.StoreState).map(item => item.ID)
    return await getAvailablePhases(meterIDs, measurmentTypeID, seriesTypeID);
});
// #endregion


// #region [ Slice ]
export const WizardPhaseOptionSlice = createSlice({
    name: 'WizardPhaseOption',
    initialState: {
        Status: 'unitiated',
        Data: [],
        Error: null,
        Selected: [],
        
    } as Redux.OptionState<openXDA.IPhase>,
    reducers: {
        selectPhase: (state, action: PayloadAction<openXDA.IPhase>) => {
            let index = state.Data.findIndex(ph => ph.ID == action.payload.ID);
            state.Selected[index] = !state.Selected[index]
        }
        
    },
    extraReducers: (builder) => {

        builder.addCase(FetchAvailablePhases.fulfilled, (state, action) => {
            state.Status = 'idle';
            state.Error = null;
            let oldState = state.Data;
            state.Data = JSON.parse(action.payload) as openXDA.IPhase[];
            state.Selected = state.Data.map(ph => oldState.findIndex(item => item.ID == ph.ID) > -1)
        });
        builder.addCase(FetchAvailablePhases.pending, (state, action) => {
            state.Status = 'loading';
        });
        builder.addCase(FetchAvailablePhases.rejected, (state, action) => {
            state.Status = 'error';
            state.Error = action.error.message;

        });

    }

});

export const { selectPhase } = WizardPhaseOptionSlice.actions;
export default WizardPhaseOptionSlice.reducer;
// #endregion

// #region [ Selectors ]
export const SelectAvailablePhases = (state: Redux.StoreState) => state.WizardPhaseOption.Data;
export const SelectSelectedPhases = (state: Redux.StoreState) => state.WizardPhaseOption.Selected;
export const SelectAvailablePhasesStatus = (state: Redux.StoreState) => state.WizardPhaseOption.Status;


// #endregion

// #region [ Async Functions ]

function getAvailablePhases(meterIDs: number[], measurementTypeID: number, seriesTypeIDs: number[]): JQuery.jqXHR<string> {
    let sqlFilter = `(SELECT Channel.PhaseID FROM Channel WHERE Channel.MeterID in (${(meterIDs.length > 0? meterIDs.join(',') : 0)}) AND `
    sqlFilter = sqlFilter + `Channel.MeasurementTypeID = (SELECT ChannelGroupType.MeasurementTypeID FROM ChannelGroupType WHERE ID = ${measurementTypeID}) AND `
    sqlFilter = sqlFilter + `Channel.MeasurementCharacteristicID = (SELECT ChannelGroupType.MeasurementCharacteristicID FROM ChannelGroupType WHERE ID = ${measurementTypeID}) AND `
    sqlFilter = sqlFilter + `(SELECT COUNT(Series.ID) FROM SERIES WHERE Series.ChannelID = Channel.ID AND Series.SeriesTypeID IN (${(seriesTypeIDs.length > 0?seriesTypeIDs.join(',') : 0)})) > 0)`

    let filter = [{
        FieldName: "ID",
        SearchText: `(${sqlFilter})`,
        Operator: "IN",
        Type: "integer"
    }];

    return $.ajax({
        type: "POST",
        url: `${apiHomePath}api/Phase/SearchableList`,
        contentType: "application/json; charset=utf-8",
        dataType: 'json',
        data: JSON.stringify({ Searches: filter, OrderBy: 'Name', Ascending: false }),
        cache: false,
        async: true
    });
}


// #endregion
