//******************************************************************************************************
//  WizardAffectedChannelSlice.ts - Gbtc
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
//  12/03/2020 - C. Lackner
//       Generated original version of source code.
//
//******************************************************************************************************

import { createSlice, createAsyncThunk, PayloadAction } from '@reduxjs/toolkit';
import { SPCTools, Redux, openXDA } from '../global';
import _ from 'lodash';
import { selectSelectedMeter, selectMeasurmentTypeID, selectSeriesTypeID } from '../DynamicAlarm/DynamicWizzardSlice';
import { SelectSelectedPhases, SelectAvailablePhases } from './WizardPhaseOptionSlice';
import { SelectAvailableVoltages, SelectSelectedVoltages } from './WizardVoltageOptionSlice';

// #region [ Thunks ]
export const FetchAffectedChannels = createAsyncThunk('WizardAffectedChannel/FetchAffectedChannels', async (_, { getState }) => {
    let meterIDs = selectSelectedMeter(getState() as Redux.StoreState).map(m => m.ID);
    let measurmentTypeID = selectMeasurmentTypeID(getState() as Redux.StoreState);
    let seriesTypeID = selectSeriesTypeID(getState() as Redux.StoreState)
    let phaseSelect = SelectSelectedPhases(getState() as Redux.StoreState)
    let phaseIDs = SelectAvailablePhases(getState() as Redux.StoreState).filter((p, i) => phaseSelect[i]).map(p => p.ID);
    let voltageSelect = SelectSelectedVoltages(getState() as Redux.StoreState)
    let voltages = SelectAvailableVoltages(getState() as Redux.StoreState).filter((p, i) => voltageSelect[i])

    return await GetAffectedChannels(meterIDs, measurmentTypeID, seriesTypeID,voltages,phaseIDs);
});

// #endregion


// #region [ Slice ]
export const WizardAffectedChannelSlice = createSlice({
    name: 'WizardAffectedChannel',
    initialState: {
        Status: 'unitiated',
        Data: [],
        Error: null,
        SortField: 'ID',
        Ascending: true,
        Filters: []
    } as Redux.State<openXDA.IChannel>,
    reducers: {
        SortAffectedChanels: (state, action: PayloadAction<{Ascending: boolean, SortField: string}>) => {
            state.Ascending = action.payload.Ascending;
            state.SortField = action.payload.SortField as keyof openXDA.IChannel;
            state.Data = _.sortBy(state.Data, state.SortField, (state.Ascending ? "ASC" : "DES"));
        }
    },
    extraReducers: (builder) => {

        builder.addCase(FetchAffectedChannels.fulfilled, (state, action) => {
            state.Status = 'idle';
            state.Error = null;

            state.Data = _.uniqBy(JSON.parse(action.payload) as openXDA.IChannel[], item => item.ID);

        });
        builder.addCase(FetchAffectedChannels.pending, (state, action) => {
            state.Status = 'loading';
        });
        builder.addCase(FetchAffectedChannels.rejected, (state, action) => {
            state.Status = 'error';
            state.Error = action.error.message;

        });

    }

});

export const { SortAffectedChanels } = WizardAffectedChannelSlice.actions;
export default WizardAffectedChannelSlice.reducer;
// #endregion

// #region [ Selectors ]
export const SelectAffectedChannels = (state: Redux.StoreState) => state.WizardAffectedChannel.Data;
export const SelectAffectedChannelByID = (state: Redux.StoreState, id: number) => state.WizardAffectedChannel.Data.find(ds => ds.ID === id);
export const SelectAffectedChannelStatus = (state: Redux.StoreState) => state.WizardAffectedChannel.Status;
export const SelectAffectedChannelSortField = (state: Redux.StoreState) => state.WizardAffectedChannel.SortField;
export const SelectAffectedChannelAscending = (state: Redux.StoreState) => state.WizardAffectedChannel.Ascending;
export const SelectAffectedChannelCount = (state: Redux.StoreState) => state.WizardAffectedChannel.Data.length;
export const SelectAffectedChannelsByIDs = (state: Redux.StoreState, id: number[]) => state.WizardAffectedChannel.Data.find(ds => id.findIndex(j => j == ds.ID) > -1);

// #endregion

// #region [ Async Functions ]

function GetAffectedChannels(meterIDs: number[], measurementTypeID: number, seriesTypeID: number, voltages: number[], phaseIDs: number[]): JQuery.jqXHR<string> {
    let filter = [
        {
            FieldName: "MeterID",
            SearchText: `(${(meterIDs.length > 0 ? meterIDs.join(',') : 0)})`,
            Operator: "IN",
            Type: "integer"
        },
        {
            FieldName: "MeasurementTypeID",
            SearchText: `(SELECT ChannelGroupType.MeasurementTypeID FROM ChannelGroupType WHERE ID = ${measurementTypeID})`,
            Operator: "=",
            Type: "integer"
        },
        {
            FieldName: "MeasurementCharacteristicID",
            SearchText: `(SELECT ChannelGroupType.MeasurementCharacteristicID FROM ChannelGroupType WHERE ID = ${measurementTypeID})`,
            Operator: "=",
            Type: "integer"
        },
        {
            FieldName: `(SELECT COUNT(Series.ID) FROM SERIES WHERE Series.ChannelID = ChannelDetail.ID AND Series.SeriesTypeID = ${seriesTypeID})`,
            SearchText: "0",
            Operator: ">",
            Type: "integer"
        },
        {
            FieldName: "PhaseID",
            SearchText: `(${(phaseIDs.length > 0 ? phaseIDs.join(',') : 0)})`,
            Operator: "IN",
            Type: "integer"
        },
        {
            FieldName: "(SELECT VoltageKV FROM Asset WHERE Asset.ID = ChannelDetail.AssetID)",
            SearchText: `(${(voltages.length > 0 ? voltages.join(',') : 0)})`,
            Operator: "IN",
            Type: "integer"
        }
    ];

    return $.ajax({
        type: "POST",
        url: `${apiHomePath}api/Channel/SearchableList`,
        contentType: "application/json; charset=utf-8",
        dataType: 'json',
        data: JSON.stringify({ Searches: filter, OrderBy: 'MeterID', Ascending: false }),
        cache: false,
        async: true
    });
}

// #endregion
