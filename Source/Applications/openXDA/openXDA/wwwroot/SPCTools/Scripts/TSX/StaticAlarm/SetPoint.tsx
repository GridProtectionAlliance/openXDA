//******************************************************************************************************
//  StaticAlarm.tsx - Gbtc
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
import { SPCTools, StaticWizzard, openXDA, Redux } from '../global';
import { Select } from '@gpa-gemstone/react-forms';
import { useSelector, useDispatch } from 'react-redux';
import { addFactor, removeFactor, selectSeverity, updateSeverity, selectfactors, updateFactor, selectAlarmType, selectHistoricChannelList, selectStatStart, selectStatEnd, selectdataFilter, selectTokenizerRequest, updateSetPointEval, selectTokenizerResponse, selectfullHistoricDataSet } from './StaticWizzardSlice';
import { cloneDeep } from 'lodash';
import _ from 'lodash';
import TrendingCard, { ITrendSeries } from '../CommonComponents/Graph';
import { selectSeverities, selectAlarmTypes } from '../Store/GeneralSettingsSlice';

declare var homePath: string;
declare var apiHomePath: string;

declare var userIsAdmin: boolean;

interface IProps { }

interface SelectableMeter { MeterId: number, Name: string, Channels: Array<openXDA.IChannel> }
//Todo
// # Fix Plots
// # Fix Threshhold stuff

const StaticSetPoint = (props: IProps) => {
    const [plot, setPlot] = React.useState<Array<SPCTools.Iplot>>([]);
    const [MeterList, setMeterList] = React.useState<Array<SelectableMeter>>([]);

    const [setPoint, setSetPoint] = React.useState<string>("");
    const [loadingSetpoint, setLoadingSetpoint] = React.useState<boolean>(false);
    
    const tokenizerRequest = useSelector(selectTokenizerRequest);
    const tokenizerResponse = useSelector(selectTokenizerResponse);

    const dispatch = useDispatch();

    const factors = useSelector(selectfactors);
    const selectedAlarmType = useSelector(selectAlarmType)
    const historicChannels = useSelector(selectHistoricChannelList);

    // Plot Data and Setpoint Data is Local since it is not used anywhere else

    // This only triggers on First render
    React.useEffect(() => {
        let meterIds = _.uniq(historicChannels.map((item) => item.MeterID))

        
        setMeterList(meterIds.map(id => {
            return { MeterId: id, Channels: historicChannels.filter(ch => ch.MeterID == id), Name: historicChannels.find(ch => ch.MeterID == id).MeterName }
        }))

       return () => { }
    }, [])

    function AddPlot(channel: openXDA.IChannel) {
        setPlot((old) => {
            let updated = cloneDeep(old);
            let threshold = 0;
            if (tokenizerResponse != undefined && tokenizerResponse.Valid) {
                let index = tokenizerRequest.Channels.findIndex(i => i == channel.ID)
                threshold = (index > -1 ? tokenizerResponse.Value[index] : 0);
            }
            
            updated.push({ ChannelID: channel.ID, Title: (channel.MeterName + " - " + channel.Name), Threshhold: threshold })
            return updated;
        })
    }

    function RemovePlot(channelId: number) {
        setPlot((old) => {
            let updated = cloneDeep(old);
           
            let index = updated.findIndex(ch => ch.ChannelID == channelId);

            if (index > -1)
                updated.splice(index,1)
            return updated;
        })
    }

    React.useEffect(() => {
        // Load Results of setpoint Tokenizer
        setLoadingSetpoint(true);
        let h = loadTokenizer();
        return () => {if (h != undefined && h.abort != undefined) h.abort(); }

    }, [setPoint])

    function loadTokenizer() {

        if (setPoint == undefined || setPoint == "") {
            setLoadingSetpoint(false);
            return undefined;
        }
            
        let f = setPoint
        let handle = $.ajax({
            type: "POST",
            url: `${apiHomePath}api/SPCTools/StaticAlarmCreation/ParseSetPoint`,
            contentType: "application/json; charset=utf-8",
            dataType: 'json',
            data: JSON.stringify({ ...tokenizerRequest, Value: f }),
            cache: false,
            async: true
        });

        
        handle.done((d: SPCTools.ITokenizerResponse) => {
            setLoadingSetpoint(false);
            dispatch(updateSetPointEval({ response: d, setPoint: f }));
            if (d.Valid && d.IsScalar)
                setPlot((old) => {
                    return  old.map(item => { return { ...item, Threshhold: d.Value[0] } })
                })
            else if (d.Valid)
                setPlot((old) => {
                    return old.map(item => {
                        let index = tokenizerRequest.Channels.findIndex(i => i == item.ChannelID)
                        return { ...item, Threshhold: (index == -1 ? NaN : d.Value[index]) };
                    })
                })
        })
        return handle;
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
                    {selectedAlarmType.Name != 'Lower Limit' ? <SetPointWindow type={'upper'} setter={setSetPoint} /> : null}
                    {selectedAlarmType.Name != 'Upper Limit' ? <SetPointWindow type={'lower'} setter={setSetPoint} /> : null}
                    <HelpWindow loading={loadingSetpoint} />
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
                        {plot.map((item, index) => <GraphCard key={index} {...item} remove={() => RemovePlot(item.ChannelID)} />)}
                    </div>
                    <div className="row" style={{ margin: 0, }}>
                        <div className="col dropdown" style={{ padding: 0 }}>
                            <div style={{ width: '100%', margin: 'auto', marginTop: '4px', height: '3em', textAlign: 'center', background: '#bbbbbb', borderBottomLeftRadius: '10px', borderBottomRightRadius: '10px', }} data-toggle="dropdown">
                                <i className="fa fa-plus fa-2x"></i>
                            </div>
                            <div className="dropdown-menu">
                                <a className={"dropdown-item" + (plot.findIndex(p => p.ChannelID == -1) > -1 ? ' active' : '')} onClick={() => {
                                    if (plot.findIndex(p => p.ChannelID == -1) == -1)
                                        setPlot((old) => {
                                            let updated = cloneDeep(old);
                                            let threshold = 0;
                                            if (tokenizerResponse != undefined && tokenizerResponse.Valid && tokenizerResponse.IsScalar)
                                                threshold = tokenizerResponse.Value[0];
                                            
                                            updated.push({ ChannelID: -1, Title: 'Overview', Threshhold: threshold })
                                            return updated;
                                        })
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

const SetPointWindow = (props: { type: StaticWizzard.setPointType, setter: (value: string) => void }) => {
    const [text, setText] = React.useState<string>("");
    const alarmGroup = useSelector((state: Redux.StoreState) => state.StaticWizzard.AlarmGroup);
    React.useEffect(() => {
        let handle = setTimeout(() => {
           props.setter(text)
        }, 500);

        return () => { clearTimeout(handle); }

    }, [text])

    //set intial Text to existing Formula if it exists
    React.useEffect(() => {
        setText(alarmGroup.Formula)
    }, [])

    return (
        <div className="row" style={{ margin: 0 }}>
            <div className="col">
                <div className="form-group">
                    <label>{props.type}</label>
                    <textarea
                        rows={2}
                        className={'form-control'}
                        onChange={(evt) => setText(evt.target.value as string)}
                        value={text}
                    />
                </div>
            </div>
        </div>)
}

const HelpWindow = (props: {loading: boolean}) => {
    const error = useSelector(selectTokenizerResponse)
    const allowSlice = useSelector(selectfullHistoricDataSet)

    // This is to check whether we selected all Channels when we selected Historic channels when we have a slice
    const isInfo = props.loading || (error != undefined && !error.IsScalar && error.Valid && allowSlice)
    const isSuccess = (error != undefined && error.Valid && error.IsScalar)

    const title = (isSuccess || isInfo ? "Setpoint Expression is Valid" : "Setpoint Expression is invalid");
    let text = "";
    if (error == undefined)
        text = "Expression can not be empty."
    else if (error.Valid && !error.IsScalar && !allowSlice)
        text = "A single threshold is required for all Channels because some Channels are not seletced as historic data source.";
    else if (error.Valid && !error.IsScalar && allowSlice)
        text = "A sepperate Threshhold will be computed for each Channel. if that is not intended an aggregation function such as MIN or MAX needs to be used."
    else
        text = error.Message;
  
    return (
        <>
            <div className="row" style={{ margin: 0 }}>
                <div className="col">
                    <div className={"alert alert-" + (isInfo ? 'info' : isSuccess ? 'success' : 'danger')} role="alert">
                        <div className="row" style={{ margin: 0 }}>
                            <div className="col">
                                    <div className="float-left"> <h4 className="alert-heading">{(props.loading ? 'Evaluating Expression...' : title)}</h4> </div>
                                    {props.loading ? <div className="spinner-border float-right" role="status" style={{ margin: 'auto' }}></div> : null}
                            </div>
                        </div>
                        <div className="row" style={{ margin: 0 }}>
                            <div className="col">
                               <p>
                                    {(props.loading ? null : text)}
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
    const severities = useSelector(selectSeverities);

    return (
        <div className="row" key={"r-" + props.index} style={{ margin: 0 }}>
                <div className="col-4">
                <div className="form-group">
                    <input className={"form-control" + (props.Factor.Value == 1.0 ? ' is-invalid': '')} type={'number'} onChange={(evt) => dispatch(updateFactor({ index: props.index, factor: { ...props.Factor, Value: parseFloat(evt.target.value) } }))}
                        value={props.Factor.Value} />
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

const GraphCard = (props: { ChannelID: number, Title: string, Threshhold: number, remove: () => void }) => {

    const [data, setData] = React.useState<Array<ITrendSeries>>([]);
    const [threshhold, setThreshold] = React.useState<Array<ITrendSeries>>([]);
    const startDate = useSelector(selectStatStart)
    const endDate = useSelector(selectStatEnd)
    const dataFilter = useSelector(selectdataFilter)
    const allchannels = useSelector(selectHistoricChannelList)

    const secerityID = useSelector(selectSeverity);
    const severities = useSelector(selectSeverities)
    const factors = useSelector(selectfactors);

    React.useEffect(() => {
        setData([]);
        let handle = [];
        if (props.ChannelID == -1)
            handle.push(...allchannels.map((item => getData(item.ID))));
        else
            handle.push( getData(props.ChannelID));
        return () => {
            handle.forEach(h => { if (h != undefined && h.abort != undefined) h.abort();})
        }
    }, [props.ChannelID])

    React.useEffect(() => {

        const Tstart = (data.length > 0 ? data[0].data[0][0] : 0);
        const Tend = (data.length > 0 ? data[0].data[data[0].data.length - 1][0] : 1500);

        let sp = {
            color: severities.find(item => item.ID == secerityID).Color,
            includeLegend: false,
            label: "",
            lineStyle: ':',
            data: [[Tstart, props.Threshhold], [Tend, props.Threshhold]],
            opacity: 1.0
        } as ITrendSeries;

        setThreshold([sp, ...factors.filter(f => f.Value != 1.0).map(f => {
            return {
                color: severities.find(item => item.ID == f.SeverityID).Color,
                includeLegend: false,
                label: "",
                lineStyle: ':',
                data: [[Tstart, props.Threshhold * f.Value], [Tend, props.Threshhold * f.Value]],
                opacity: 1.0
            } as ITrendSeries;
        })])

    }, [props.Threshhold, secerityID, factors, severities, data])

    function getData(channelId: number): JQuery.jqXHR<Array<number[]>> {
        let handle = $.ajax({
            type: "POST",
            url: `${apiHomePath}api/SPCTools/StaticAlarmCreation/GetData/${channelId}?start=${startDate}&end=${endDate}`,
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(dataFilter),
            dataType: 'json',
            cache: false,
            async: true
        });

        handle.done((data) => setData((old) => {
            let updated = cloneDeep(old);
            let series: ITrendSeries = {
                color: '#3333ff',
                includeLegend: false,
                label: "",
                lineStyle: '-',
                data: data,
                opacity: (props.ChannelID == -1? 0.5: 1.0)
            }
            updated.push(series)
            return updated;
        }))
        return handle;
    }

    let Tstart = (data.length > 0? data[0].data[0][0] : 0);
    let Tend = (data.length > 0 ? data[0].data[data[0].data.length - 1][0] : 1500);

    return (
        <>
            <div className="row" style={{ margin: 0, width: '100%', textAlign: 'center', background: '#bbbbbb',}}>
                <div className="col-12">
                    <h5 className='float-left'> {props.Title}</h5>
                    <p className='float-right'> <i className="fa fa-trash-o" onClick={() => { props.remove() }}></i> </p>
                </div>
            </div>
            <div className="row" style={{ margin: 0, width: '100%', textAlign: 'center', background: '#bbbbbb', }}>
                <div className="col-12">
                    <TrendingCard keyString={'Graph-' + props.ChannelID} allowZoom={false} height={125} xLabel={"Time"} Tstart={Tstart} Tend={Tend} data={[...data, ...threshhold]} />
                </div>
            </div>
        </>
    );

}

const SeveritySelect = (props: {}) => {
    const secerityID = useSelector(selectSeverity);
    const dispatch = useDispatch();
    const severities = useSelector(selectSeverities)

    return (
        <div className="form-group">
            <label>Alarm Severity</label>
            <select className="form-control" onChange={(evt) => dispatch(updateSeverity(parseInt(evt.target.value)))} value={secerityID}>
                {severities.map((a, i) => ( <option key={i} value={a.ID}> {a.Name} </option>))}
            </select>
        </div>);
}


export default StaticSetPoint