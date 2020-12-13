//******************************************************************************************************
//  SetPointCreator.tsx - Gbtc
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

import * as React from 'react';
import { SPCTools, StaticWizzard, openXDA, Redux, DynamicWizzard } from '../global';
import { useSelector, useDispatch } from 'react-redux';
import { cloneDeep } from 'lodash';
import _ from 'lodash';
import TrendingCard, { ITrendSeries } from '../CommonComponents/Graph';
import { SelectSeverities } from '../store/SeveritySlice';
import { selectAlarmGroup, updateFactor, removeFactor, SelectAlarmFactors, SelectStatisticsChannels, addFactor, SelectSetPointAlarmDays, SelectStatisticsrange, SelectActiveFormula, UpdateFormula, SelectActiveAlarmValue, SelectAllowSlice, updateAlarmGroup, SelectWizardType } from './DynamicWizzardSlice';
import { DynamicTimeRange } from './DynamicTimeRange';
import { AlarmTrendingCard } from './AlarmTrendingCard';
import { SelectAlarmDayByID } from '../store/AlarmDaySlice';
import { SelectSetPointParseStatus, SelectSetPointParseResult } from '../store/SetPointParseSlice';

declare var homePath: string;
declare var apiHomePath: string;

declare var userIsAdmin: boolean;

interface IProps { }

interface DropDownMeter { MeterId: number, Name: string, Channels: Array<openXDA.IChannel> }
interface IChannelGraphProps {ChannelID: number}

const SetPointCreator = (props: IProps) => {
    const isDynamic = useSelector(SelectWizardType);
    const alarmDays = useSelector(SelectSetPointAlarmDays);
    const [plot, setPlot] = React.useState<Array<IChannelGraphProps>>([]);
    const [MeterList, setMeterList] = React.useState<Array<DropDownMeter>>([]);

    const [activeDayID, setActiveDayID] = React.useState<number>(alarmDays.length > 0 ? alarmDays[0].ID : undefined);
    const [activeHour, setActiveHour] = React.useState<number>(undefined);

    const dispatch = useDispatch();

    const factors = useSelector(SelectAlarmFactors);
    const availableChannels = useSelector(SelectStatisticsChannels)
    
    const statisticsTime = useSelector(SelectStatisticsrange);

    // This only triggers on First render to create a List of Available Meters for the Graph Dropdown
    React.useEffect(() => {
        let meterIds = _.uniq(availableChannels.map((item) => item.MeterID))

        setMeterList(meterIds.map(id => {
            return { MeterId: id, Channels: availableChannels.filter(ch => ch.MeterID == id), Name: availableChannels.find(ch => ch.MeterID == id).MeterName }
        }))

        if (alarmDays.length == 0)
            setActiveDayID(undefined);
        else
            setActiveDayID(alarmDays[0].ID)
       return () => { }
    }, [])


    function RemovePlot(channelId: number) {
        setPlot((old) => old.filter(item => item.ChannelID != channelId))
    }

    function AddPlot(channel: openXDA.IChannel) {
        setPlot((old) => _.uniqBy([...old, { ChannelID: channel.ID }], item => item.ChannelID))
    }

    return (
        <div style={{ width: '100%', height: '100%' }}>
            <div className="row" style={{ margin: 0 }}>
                <div className="col">
                    <h2>Create the Alarm Setpoint Computation Model:</h2>
                </div>
            </div>
            <div className="row" style={{ margin: 0 }}>
                <div className="col-9">
                    <div className="row" style={{ margin: 0 }}>
                        {isDynamic== 'dynamic' ?
                            <>
                                {alarmDays.length == 0 ? null :
                                    <div className="col-2" style={{ marginBottom: '16px' }}>
                                        <ul className="nav nav-pills">
                                            {alarmDays.map((item, index) => <li className={"nav-item"} key={index} style={{ width: '100%' }}>
                                                <a className={"nav-link" + (item.ID == activeDayID? ' active' : '')} onClick={() => setActiveDayID(item.ID)}>{item.Name}</a>
                                            </li>)}
                                    </ul>
                                </div>}
                                <div className={"col-" + (alarmDays.length == 0? '12' : '10')}>
                                    <div className="tab-pane fade show active">
                                        <div className="row" style={{ margin: 0 }}>
                                            <SetPointEditor alarmDayID={activeDayID} startHour={activeHour} />
                                            <div className="col-3">
                                                <DynamicTimeRange alarmDayID={activeDayID} setter={setActiveHour}/>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </>
                            :
                            <SetPointEditor alarmDayID={null} startHour={0} label={''} />}
                    </div>
                    
                    <SetPointMessage />
                </div>
                <div className="col-3">
                    <div className="row" style={{ margin: 0 }}>
                        <div className="col">
                            <SeveritySelect />
                        </div>
                    </div>
                    {factors.map((item, i) => <FactorRow key={i} Factor={item} index={i} />)}
                    <div className="row" style={{ margin: 0 }}>
                        <div className="col">
                            <div className="btn-group mr-2" role="group" style={{ padding: '5px' }}>
                                <button type="button" className="btn btn-secondary" disabled={factors.length > 2} onClick={() => dispatch(addFactor())}> Add Level</button>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div className="row" style={{ margin: 0 }}>
                <div className="col">
                    <div className="row" style={{ margin: 0, }}>
                        {plot.map((item, index) => <AlarmTrendingCard ChannelID={item.ChannelID} key={index} Remove={() => RemovePlot(item.ChannelID)} Tstart={statisticsTime.start} Tend={statisticsTime.end} />)}
                    </div>
                    <div className="row" style={{ margin: 0, }}>
                        <div className="col dropdown" style={{ padding: 0 }}>
                            <div style={{ width: '100%', margin: 'auto', marginTop: '4px', height: '3em', textAlign: 'center', background: '#bbbbbb', borderBottomLeftRadius: '10px', borderBottomRightRadius: '10px', }} data-toggle="dropdown">
                                <i className="fa fa-plus fa-2x"></i>
                            </div>
                            <div className="dropdown-menu">
                                <a className={"dropdown-item" + (plot.findIndex(p => p.ChannelID == -1) > -1 ? ' active' : '')} onClick={() => {
                                    if (plot.findIndex(p => p.ChannelID == -1) == -1)
                                        setPlot((old) => [...old, { ChannelID: -1 }])
                                    else
                                        RemovePlot(-1)
                                }}>All Channels Overview</a>
                                <div className="dropdown-divider"></div>
                                {MeterList.map((item, mi) =>
                                    <div key={mi} className="dropdown-submenu dropdown-item">
                                        <a data-toggle="dropdown">{item.Name}</a>
                                        <ul className="dropdown-menu">
                                            {item.Channels.map((ch, i) =>
                                                <li key={i}>
                                                    <a className={"dropdown-item" + (plot.findIndex(p => p.ChannelID == ch.ID) > -1 ? ' active' : '')}
                                                        onClick={() => {
                                                            if (plot.findIndex(p => p.ChannelID == ch.ID) == -1)
                                                                AddPlot(ch)
                                                            else
                                                                RemovePlot(ch.ID)
                                                        }}
                                                    >{ch.Name}</a>
                                                </li>)}
                                        </ul>
                                    </div>)}
                            </div>
                        </div>
                    </div>
                </div>
            </div>
           
            
        </div>      
    );
}

const SetPointEditor = (props: { alarmDayID: number, startHour: number, label?: string }) => {
    const dispatch = useDispatch();

    const [text, setText] = React.useState<string>("");

    const memFormulaSelector = React.useMemo<(state) => string>(() => {
        return (state) => SelectActiveFormula(state, props.alarmDayID, props.startHour)
    }, [props])
    const memAlarmDaySelector = React.useMemo<(state) => DynamicWizzard.IAlarmDay>(() => {
        return (state) => SelectAlarmDayByID(state, props.alarmDayID)
    }, [props.alarmDayID])
    const memAlarmValueSelector = React.useMemo<(state) => DynamicWizzard.IAlarmvalue>(() => {
        return (state) => SelectActiveAlarmValue(state, props.alarmDayID, props.startHour)
    }, [props])

    const activeFormula = useSelector(memFormulaSelector);
    const alarmDay = useSelector(memAlarmDaySelector);
    const alarmValue = useSelector(memAlarmValueSelector);

    React.useEffect(() => {
        let handle;
        if (props.startHour != undefined)
            handle = setTimeout(() => {
                dispatch(UpdateFormula({ alarmDayID: props.alarmDayID, startHour: props.startHour, formula: text }))
            }, 500);

        return () => { if (handle != undefined) clearTimeout(handle); }

    }, [text]);

    React.useEffect(() => {
        setText(activeFormula)
    }, [activeFormula])

   
    const lbl = (props.label == undefined ? ('(' + (alarmDay == undefined ? '' : (alarmDay.Name + ' ')) + 
        (props.startHour < 10 ? '0' : '') + props.startHour + ' - ' +
        (alarmValue != undefined ? (alarmValue.EndHour != undefined ? ((alarmValue.EndHour > 9 ? '' : '0') + alarmValue.EndHour) : 24) : '') +  ')')
    : props.label)

    return (
        
            <div className="col">
            <div className="form-group">
                <label>SetPoint {lbl}
                </label>
                    <textarea
                        rows={2}
                        className={'form-control'}
                        onChange={(evt) => setText(evt.target.value as string)}
                        value={text}
                    />
                </div>
            </div>)
}

const SetPointMessage = (props: {}) => {

    const status = useSelector(SelectSetPointParseStatus)
    const response = useSelector(SelectSetPointParseResult);
    const allowSlice = useSelector(SelectAllowSlice)
    
    const loading = (status == 'loading');
    const isInfo = loading || (response != undefined && !response.IsScalar && response.Valid && allowSlice);
    const isSuccess = (status == 'idle' && response != undefined && response.IsScalar && response.Valid);
    const title = (isSuccess || isInfo ? "Setpoint Expression is Valid" : "Setpoint Expression is invalid");
    let text = "";
    if (response == undefined)
        text = "An error occured."
    else if (response.Valid && !response.IsScalar && !allowSlice)
        text = "A single threshold is required for all channels because some channels are not selected as historic data source.";
    else if (response.Valid && !response.IsScalar && allowSlice)
        text = "A sepperate Threshhold will be computed for each Channel. if that is not intended an aggregation function such as MIN or MAX needs to be used."
    else
        text = response.Message;
    
    return (
        <>
            <div className="row" style={{ margin: 0 }}>
                <div className="col">
                    <div className={"alert alert-" + (isInfo ? 'info' : isSuccess ? 'success' : 'danger')} role="alert">
                        <div className="row" style={{ margin: 0 }}>
                            <div className="col">
                                {loading ? <div className="spinner-border float-left" role="status" style={{ margin: 'auto' }}></div> : null}
                                <div className="float-left"> <h4 className="alert-heading">{(loading ? 'Evaluating Expression...' : title)}</h4> </div>
                                <button type="button" className="btn btn-light float-right" onClick={() => ($('#FunctionHelp') as any).modal('show')}>
                                    <i style={{ color: '#007BFF' }} className="fa fa-2x fa-question-circle"></i>
                                </button>
                                
                            </div>
                        </div>
                        <div className="row" style={{ margin: 0 }}>
                            <div className="col">
                                <hr />
                               <p>
                                    {(loading ? null : text)}
                                </p>
                            </div>
                        </div>
                    </div>

                </div>
            </div>

        </>)
}

const FactorRow = (props: { Factor: SPCTools.IFactor, index: number }) => {
    const dispatch = useDispatch();
    const severities = useSelector(SelectSeverities)

    return (
        <div className="row" key={"r-" + props.index} style={{ margin: 0 }}>
                <div className="col-4">
                <div className="form-group">
                    <input className={"form-control" + (props.Factor.Factor == 1.0 ? ' is-invalid' : '')}
                        type={'number'}
                        onChange={(evt) => dispatch(updateFactor({ index: props.index, factor: { ...props.Factor, Factor: parseFloat(evt.target.value) } }))}
                        value={props.Factor.Factor} />
                    </div>
                </div>
                <div className="col-6">
                <div className="form-group">
                    <select className="form-control" onChange={(evt) => dispatch(updateFactor({ index: props.index, factor: { ...props.Factor, SeverityID: parseInt(evt.target.value) } }))}
                        value={props.Factor.SeverityID}>
                            {severities.map((a, i) => <option key={i} value={a.ID}> {a.Name} </option>)}
                        </select>
                    </div>
                </div>
                <div className="col-2">
                    <i onClick={() => {dispatch(removeFactor(props.index));}} className="fa fa-trash-o"></i>
                </div>
            </div>)
}

const SeveritySelect = (props: {}) => {
    const alarmGroup = useSelector(selectAlarmGroup);
    const dispatch = useDispatch();
    const severities = useSelector(SelectSeverities)

    return (
        <div className="form-group">
            <label>Alarm Severity</label>
            <select className="form-control" onChange={(evt) => dispatch(updateAlarmGroup({ ...alarmGroup, SeverityID: (parseInt(evt.target.value)) }))} value={alarmGroup.SeverityID}>
                {severities.map((a, i) => ( <option key={i} value={a.ID}> {a.Name} </option>))}
            </select>
        </div>);
}


export default SetPointCreator