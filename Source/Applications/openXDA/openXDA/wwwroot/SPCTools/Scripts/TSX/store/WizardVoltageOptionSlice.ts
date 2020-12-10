//******************************************************************************************************
//  WizardVoltageOptionSlice.ts - Gbtc
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

import { createSlice, createAsyncThunk, PayloadAction } from '@reduxjs/toolkit';
import { SPCTools, Redux, openXDA } from '../global';
import _ from 'lodash';
import { selectSelectedMeter, selectMeasurmentTypeID } from '../Wizard/DynamicWizzardSlice';
import { SelectSeriesTypes } from './SeriesTypeSlice';

// #region [ Thunks ]
export const FetchAvailableVoltages = createAsyncThunk('WizardVoltageOption/FetchAvailableVoltages', async (_, { getState }) => {
    let meterIDs = selectSelectedMeter(getState() as Redux.StoreState).map(m => m.ID);
    let measurmentTypeID = selectMeasurmentTypeID(getState() as Redux.StoreState);
    let seriesTypeID = SelectSeriesTypes(getState() as Redux.StoreState).map(item => item.ID)
    return await getAvailableVoltages(meterIDs, measurmentTypeID, seriesTypeID);
});
// #endregion


// #region [ Slice ]
export const WizardVoltageOptionSlice = createSlice({
    name: 'WizardVoltageOption',
    initialState: {
        Status: 'unitiated',
        Data: [],
        Error: null,
        Selected: [],
        
    } as Redux.OptionState<number>,
    reducers: {
        selectVoltage: (state, action: PayloadAction<number>) => {
            let index = state.Data.findIndex(v => v == action.payload);
            state.Selected[index] = !state.Selected[index]
        }
        
    },
    extraReducers: (builder) => {

        builder.addCase(FetchAvailableVoltages.fulfilled, (state, action) => {
            state.Status = 'idle';
            state.Error = null;
            let oldState =state.Data.filter((v,i) => state.Selected[i]);
            state.Data = _.uniq(JSON.parse(action.payload).map(item => item.VoltageKV) as number[]);
            state.Selected = state.Data.map(ph => oldState.findIndex(item => item == ph) > -1)
        });
        builder.addCase(FetchAvailableVoltages.pending, (state, action) => {
            state.Status = 'loading';
        });
        builder.addCase(FetchAvailableVoltages.rejected, (state, action) => {
            state.Status = 'error';
            state.Error = action.error.message;

        });

    }

});

export const { selectVoltage } = WizardVoltageOptionSlice.actions;
export default WizardVoltageOptionSlice.reducer;
// #endregion

// #region [ Selectors ]
export const SelectAvailableVoltages = (state: Redux.StoreState) => state.WizardVoltageOption.Data;
export const SelectSelectedVoltages = (state: Redux.StoreState) => state.WizardVoltageOption.Selected;
export const SelectAvailableVoltageStatus = (state: Redux.StoreState) => state.WizardVoltageOption.Status;


// #endregion

// #region [ Async Functions ]

function getAvailableVoltages(meterIDs: number[], measurementTypeID: number, seriesTypeID: number[]): JQuery.jqXHR<string> {
    let sqlFilter = `(SELECT Channel.AssetID FROM Channel WHERE Channel.MeterID in (${(meterIDs.length > 0 ? meterIDs.join(',') : 0)}) AND `
    sqlFilter = sqlFilter + `Channel.MeasurementTypeID = (SELECT ChannelGroupType.MeasurementTypeID FROM ChannelGroupType WHERE ID = ${measurementTypeID}) AND `
    sqlFilter = sqlFilter + `Channel.MeasurementCharacteristicID = (SELECT ChannelGroupType.MeasurementCharacteristicID FROM ChannelGroupType WHERE ID = ${measurementTypeID}) AND `
    sqlFilter = sqlFilter + `(SELECT COUNT(Series.ID) FROM SERIES WHERE Series.ChannelID = Channel.ID AND Series.SeriesTypeID IN (${(seriesTypeID.length > 0 ? seriesTypeID.join(',') : 0)})) > 0)`

    let filter = [{
        FieldName: "ID",
        SearchText: `(${sqlFilter})`,
        Operator: "IN",
        Type: "integer"
    }];

    return $.ajax({
        type: "POST",
        url: `${apiHomePath}api/Asset/SearchableList`,
        contentType: "application/json; charset=utf-8",
        dataType: 'json',
        data: JSON.stringify({ Searches: filter, OrderBy: 'VoltageKV', Ascending: true }),
        cache: false,
        async: true
    });
}


// #endregion
