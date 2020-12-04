//******************************************************************************************************
//  global.tsx - Gbtc
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
//  10/20/2020 - C. Lackner
//       Generated original version of source code.
//


//******************************************************************************************************
declare global {
    var homePath: string;
    var userIsAdmin: boolean;
    var apiHomePath: string;
}

export namespace Redux {
    export interface StoreState {
        ChannelOverview: State<SPCTools.IChannelOverview>,
        ChannelAlarmGroup: State<SPCTools.IChannelAlarmGroup>,
        StaticWizzard: StaticWizzard.IState,
        GeneralSettings: SPCTools.ISettingsState,
        HistoryTest: SPCTools.IHistoryTestState,
        AlarmDay: State<DynamicWizzard.IAlarmDay>,
        AlarmDayGroup: State<DynamicWizzard.IAlarmDayGroup>,
        DynamicWizzard: DynamicWizzard.IState,
        MeasurmentType: State<SPCTools.IMeasurementType>,
        AlarmType: State<SPCTools.IAlarmType>,
        WizardPhaseOption: OptionState<openXDA.IPhase>,
        WizardVoltageOption: OptionState<number>,
        SeriesType: State<openXDA.ISeriesType>
        Meter: State<openXDA.IMeter>,
        MeterAlarmGroup: State<SPCTools.IChannelAlarmGroup>,
        AlarmGroupView: State<SPCTools.IAlarmGroupView>,
    }


    interface State<T> {
        Status: SPCTools.Status,
        Data: T[],
        Error: null | string,
        SortField: keyof T,
        Ascending: boolean,
        Filters?: Filter.IFilter<T>[]
    }
}

export namespace Filter {
    export type FieldType = ('string' | 'number' | 'enum' | 'integer' | 'datetime' | 'boolean')
    export interface IField<T> { label: string, key: keyof T, type: FieldType, enum?: Map<number, string> }
    export type OperatorType = ('=' | '<>' | '>' | '<' | '>=' | '<=' | 'LIKE' | 'NOT LIKE' | 'IN' | 'NOT IN')
    export interface IFilter<T> { FieldName: keyof T, SearchText: string, Operator: Filter.OperatorType, Type: Filter.FieldType }

}

interface OptionState<T> {
    Status: SPCTools.Status,
    Data: T[],
    Error: null | string,
    Selected: boolean[]
}

export namespace SPCTools {
    export type Status = 'loading' | 'idle' | 'error' | 'changed' | 'unitiated';

    export type Page = ('Home' | 'Static' | 'Dynamic' | 'Meter' | 'Channel');

    export type LoadingState = ('loading' | 'idle' | 'error')

    export type WizzardTab = ('general' | 'selectData' | 'setpoint' | 'test')

    export type IntervallDataType = ('Minimum' | 'Maximum' | 'Average');

    export interface IAlarmGroupView { ID: number, Name: string, AlarmTypeID: number, Formula: string, Channels: number, Meters: number, AlarmSeverityID: number, AlarmSeverity: string }

    export interface IChannelOverview { ID: number, Meter: string, Channel: string, Type: string, Phase: string, Asset: string }

    export interface IChannelAlarmGroup { ID: number, ChannelID: number, Name: string, AlarmSeverityID: number, AlarmSeverity: string, TimeInAlarm: string }

    export interface IAlarmGroup { ID: number, Name: string, AlarmTypeID: number, Formula: string }

    export interface IStatisticData { StartDate: string, EndDate: string, DataFilter: IDataFilter }

    export interface IDataFilter { FilterZero: boolean, FilterUpper: boolean, UpperLimit: number, FilterLower: boolean, LowerLimit: number }

    export interface IFactor { SeverityID: number, Value: number, ID: number }

    export interface IAlarmType { ID: number, Name: string, Description: string }

    export interface ISeverity { ID: number, Name: string, Color: string }

    export interface Iplot { ChannelID: number, Title: string, Threshhold: number }

    export interface IDateRange { start: string, end: string }

    // Assume Upper or Lower Setpoint everywhere not both - would need to make some adjustments in Controller and DataStructures for 2 setpoints/Alarm
    export interface ITokenizerRequest {
        Value: string,
        DataFilter: IDataFilter,
        Channels: number[],
        StartDate: string,
        EndDate: string,

    }

    export interface ITokenizerResponse { Valid: boolean, Message: string, IsScalar: boolean, Value: number[] }

    
    export interface IChannelTest { ChannelID: number, Threshhold: number, FactorTests: IFactorTest[] }

    export interface IFactorTest { NumberRaised: number, TimeInAlarm: number, Factor: number }

    //Billy called this ChannelGroupType but that just gets confusing in context of SPC Tools since it is just a single dropdown and we don't actually care about Channel Groups in this context
    export interface IMeasurementType { ID: number, DisplayName: string, MeasurementTypeID: number, MeasurementCharacteristicID: number }

    export interface ISettingsState {
        Tab: Page,
        AlarmTypes: SPCTools.IAlarmType[],
        MeasurementTypes: SPCTools.IMeasurementType[],
        Severities: SPCTools.ISeverity[],
        loadingAlarmTypes: boolean,
        loadingMeasurementTypes: boolean,
        loadingSeverities: boolean,
    }

    export interface IHistoryTestState {
        loading: boolean,
        time: IDateRange,
        alarmGroupID: number,
        result: IChannelTest[],
        
    }

}

export namespace StaticWizzard {
    export interface IState {
        Step: SPCTools.WizzardTab,
        AlarmGroup: SPCTools.IAlarmGroup,

        AvailableVoltages: number[],
        SelectedVoltages: boolean[],
        AvailablePhases: openXDA.IPhase[],
        SelectedPhases: boolean[],

        SelectedChannelCount: number,
        LoadingVoltages: boolean,
        LoadingPhases: boolean,
        LoadingChannelCount: boolean,

        SelectedIntervallDataType: SPCTools.IntervallDataType,
        SelectedMeasurmentTypeID: number,

        SelectedMeterID: number[],

        StatSource: SPCTools.IStatisticData,
        StatChannels: openXDA.IChannel[],

        SelectedSeverityID: number,

        AlarmFactors: SPCTools.IFactor[],
        SetPointEvaluation: SPCTools.ITokenizerResponse,

        AffectedChannels: openXDA.IChannel[]
        //SetPoints: ISetpoint[],
        
        //HistoricData: SPCTools.IExtendedHistorySettings,
        //TestResults: SPCTools.IExtendedTestSettings,
        
    }

    export interface IRequirement {
        complete: ('complete'|'required'|'warning'),
        text: string
    }

    /*export interface ISetpoint {
        type: setPointType,
        text: string,
        parsed: SPCTools.IParsedSetpoint,
        loading: boolean
    }*/

    export type setPointType = ('upper'|'lower')
}

export namespace DynamicWizzard {

    export interface IState {
        Step: SPCTools.WizzardTab,
        Status: SPCTools.Status,

        AlarmGroup: SPCTools.IAlarmGroup,
        SelectedMeter: openXDA.IMeter[],
        SelectedMeterSort: keyof openXDA.IMeter,
        SelectedMeterASC: boolean,

        MeasurmentTypeID: number,
        SeriesTypeID: number,
    }

    export interface IAlarmDayGroup { ID: number, Description: string, AlarmDayIDs: number[] }

    export interface IAlarmDay { ID: number, Name: string }

}
export namespace openXDA {

    // The following openXDA Modles are partial only since We don't neccesarrily need everything in SPC Tools

    export interface IMeter { ID: number, Name: string, Location: string, AssetKey: string, Make: string, Model: string }

    export interface IAssetGroup { ID: number, Name: string, Meters: number }

    export interface IPhase { ID: number, Name: string, Description: string }

    export interface IChannel { ID: number, MeterID: number, Name: string, AssetKey: string, MeterName: string, Phase: string }

    interface ISeriesType {ID: number, Name: string, Description: string}
}
