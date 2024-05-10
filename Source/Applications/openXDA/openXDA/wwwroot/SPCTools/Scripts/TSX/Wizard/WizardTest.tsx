﻿//******************************************************************************************************
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
import { SPCTools, DynamicWizzard } from '../global';
import Table from '@gpa-gemstone/react-table';
import { DateRangePicker } from '@gpa-gemstone/react-forms';
import { useSelector, useDispatch } from 'react-redux';
import _, { cloneDeep } from 'lodash';
import {
    selectAlarmGroup, selectSeriesTypeID, SelectAlarmFactors,
    SelectStatisticsFilter, SelectStatisticsChannels, SelectStatisticsrange,
    SelectAllAlarmValues } from './DynamicWizzardSlice';
import { SelectAffectedChannels } from '../store/WizardAffectedChannelSlice';
import { SelectSeverities } from '../store/SeveritySlice';
import { AlarmTrendingCard } from './AlarmTrendingCard';
import { LoadingIcon, Warning } from '@gpa-gemstone/react-interactive';

declare var homePath: string;
declare var apiHomePath: string;

declare var userIsAdmin: boolean;

interface IProps { }
interface IResultTable { Severity: string, Threshhold: number, NumberRaised: number, TimeInAlarm: number}
interface IChannelList { MeterName: string, Name: string, NumberRaised: number, TimeInAlarm: number, ID: number }

type preLoadState = 'uninitialized' | 'confirm' | 'loading';

const defaultTimeRange = {
    start: `${(new Date()).getFullYear()}-${((new Date()).getMonth()).toString().padStart(2, '0')}-${(new Date()).getDate().toString().padStart(2, '0')}`,
    end: `${(new Date()).getFullYear()}-${((new Date()).getMonth() + 1).toString().padStart(2, '0')}-${(new Date()).getDate().toString().padStart(2, '0')}`
} as SPCTools.IDateRange


const WizardTest = (props: IProps) => {
    const handle = React.useRef<JQuery.jqXHR>(undefined);

    const [loading, setLoading] = React.useState<SPCTools.Status>('unitiated');
    const [error, setError] = React.useState<string>('');
    const [response, setResponse] = React.useState<SPCTools.IChannelTest[]>([])
    const [timeRange, setTimeRange] = React.useState<SPCTools.IDateRange>(defaultTimeRange);
    const [effectiveTimeRange, setEffectiveTimeRange] = React.useState<SPCTools.IDateRange>(defaultTimeRange)

    const alarmGroup = useSelector(selectAlarmGroup);
    const seriesTypeID = useSelector(selectSeriesTypeID);
    const allChannels = useSelector(SelectAffectedChannels);
    const statFilter = useSelector(SelectStatisticsFilter);
    const statChannels = useSelector(SelectStatisticsChannels);
    const statTimeRange = useSelector(SelectStatisticsrange);
    const alarmVaues = useSelector(SelectAllAlarmValues);

    const [sort, setSort] = React.useState<keyof IChannelList>('NumberRaised')
    const [asc, setAsc] = React.useState<boolean>(false)

    const alarmFactors = useSelector(SelectAlarmFactors);
    const severities = useSelector(SelectSeverities)

    // Plot Data is Local since it is not used anywhere else
    const [selectedChannel, setSelectedChannel] = React.useState<number>(-1);

    //Table Data:
    const [resultSummary, setResultSummary] = React.useState<IResultTable[]>([]);
    const [channelSummary, setChannelSummary] = React.useState<IResultTable[]>([]);
    const [channelList, setChannelList] = React.useState<IChannelList[]>([])

    // Logic for large DatasetWarning
    const [userConfirm, setUserConfirm] = React.useState<preLoadState>('uninitialized');

    React.useEffect(() => {
        if (userConfirm == 'confirm') {
            const nChannels = channelList.length;
            const nDays = (Date.parse(timeRange.end) - Date.parse(timeRange.start)) / (1000.0 * 60.0 * 60.0 * 24.0);

            // Adjust statement below - if true it will not show the warning (e.g. for small datasets)
            if (nDays * nChannels < 2)
                setUserConfirm('loading');
        }
        if (userConfirm == 'loading') {
            const h = LoadTest();
            return () => { if (h != null && h.abort != null) h.abort(); };
        }
    }, [userConfirm]);

    React.useEffect(() => { setChannelList((old) => { return _.orderBy(old, [sort], [asc ? "asc" : "desc"]) }) }, [asc, sort]);

    React.useEffect(() => {
        UpdateChannelTable();
    }, [selectedChannel]);

    function LoadTest(): JQuery.jqXHR {

        setLoading('loading');
        setEffectiveTimeRange(timeRange);

        let request = {
            AlarmFactors: alarmFactors.map(item => item.Factor),
            Start: timeRange.start,
            End: timeRange.end,
            ChannelID: allChannels.map(item => item.ID),
            TokenValues: alarmVaues,
            StatisticsFilter: statFilter,
            StatisticsStart: statTimeRange.start,
            StatisticsEnd: statTimeRange.end,
            StatisticsChannelID: statChannels.map(item => item.ID),
            AlarmTypeID: alarmGroup.AlarmTypeID
        };

        let h = $.ajax({
            type: "POST",
            url: `${apiHomePath}api/SPCTools/Data/Test/${seriesTypeID}`,
            contentType: "application/json; charset=utf-8",
            dataType: 'json',
            data: JSON.stringify(request),
            cache: false,
            async: true
        });

        h.then((data) => updateData(data)).catch((_, status) => {
            setLoading('error');
            setError(status);
        });
        return h;
    }

    function updateData(data) {
        setLoading('idle');
        setChannelList(data.map(item => {
            let bindex = item.FactorTests.findIndex(d => d.Factor == 1.0)
            let cindex = allChannels.findIndex(c => item.ChannelID == c.ID)
            return {
                TimeInAlarm: item.FactorTests[bindex].TimeInAlarm * 100.0,
                NumberRaised: item.FactorTests[bindex].NumberRaised,
                ID: item.ChannelID,
                Name: (cindex == -1 ? '' : allChannels[cindex].Name),
                MeterName: (cindex == -1 ? '' : allChannels[cindex].MeterName),
            } as IChannelList
        }));

        setResultSummary([
            {
                Severity: severities.find(item => item.ID == alarmGroup.SeverityID).Name,
                NumberRaised: data.map(ch => { return ch.FactorTests.find(i => i.Factor == 1.0).NumberRaised }).reduce((a, b) => a + b, 0),
                TimeInAlarm: (data.map(ch => { return ch.FactorTests.find(i => i.Factor == 1.0).TimeInAlarm }).reduce((a, b) => a + b, 0) / data.length) * 100.0,
                Threshhold: undefined
            },
            ...alarmFactors.map(f => {
                return {

                    Severity: severities.find(item => item.ID == f.SeverityID).Name,
                    NumberRaised: data.map(ch => { return ch.FactorTests.find(i => i.Factor == f.Factor).NumberRaised }).reduce((a, b) => a + b, 0),
                    TimeInAlarm: (data.map(ch => { return ch.FactorTests.find(i => i.Factor == f.Factor).TimeInAlarm }).reduce((a, b) => a + b, 0) / data.length) * 100.0,
                    Threshhold: undefined
                }
            })
        ]);

        setResponse(data as SPCTools.IChannelTest[]);
        setSelectedChannel(-1);
    }

    function UpdateChannelTable() {

        if (response.length == 0 || selectedChannel == -1) {
            setChannelSummary([]);
            return;
        }
        let index = response.findIndex(ch => ch.ChannelID == selectedChannel);

        if (index == -1) {
            setChannelSummary([]);
            return;
        }

        setChannelSummary([
            {
                Severity: severities.find(item => item.ID == alarmGroup.SeverityID).Name,
                NumberRaised: response[index].FactorTests.find(i => i.Factor == 1.0).NumberRaised,
                TimeInAlarm: response[index].FactorTests.find(i => i.Factor == 1.0).TimeInAlarm *100.0,
                Threshhold: undefined
            },
            ...alarmFactors.map(f => {
                return {

                    Severity: severities.find(item => item.ID == f.SeverityID).Name,
                    NumberRaised: response[index].FactorTests.find(i => i.Factor == f.Factor).NumberRaised,
                    TimeInAlarm: response[index].FactorTests.find(i => i.Factor == f.Factor).TimeInAlarm * 100.0,
                    Threshhold: undefined
                }
            })
        ])
    }

    let validStartDate = !isNaN(new Date(timeRange.start).getTime()) && timeRange.start != null;
    let validEndDate = !isNaN(new Date(timeRange.end).getTime()) && timeRange.end != null &&
        (new Date(timeRange.end) > new Date(timeRange.start) || !validStartDate)

    return (
        <div style={{ width: '100%', height: '100%' }}>
            <div className="row" style={{ margin: 0 }}>
                <div className="col-6">
                    <div className="row" style={{ margin: 0 }}>
                        <div className="col">
                            <h2>Test results of Alarm calculation:</h2>
                        </div>
                    </div>
                    <div className="row" style={{ margin: 0 }}>
                        <div className="col">
                            <DateRangePicker<SPCTools.IDateRange> Label={''} Record={timeRange} FromField={'start'} ToField={'end'} Setter={(r) => { setLoading('changed'); setTimeRange(r); }} Disabled={loading == 'loading'} Valid={ () => true} />
                        </div>
                    </div>
                    <div className="row" style={{ margin: 0 }}>
                        <div className="col">
                            <button type="button" className={"btn btn-primary btn-block"} disabled={loading != 'changed' || !validEndDate || !validStartDate} onClick={() => setUserConfirm('confirm')}>
                                Test
                            </button>
                        </div>
                        
                    </div>
                    <div className="row" style={{ margin: 0 }}>
                        <div className="col">
                            {loading == 'loading' ?
                                <LoadingIcon Show={true} Size={40} />:
                                <Table<IChannelList>
                                    tableStyle={{ height: '100%' }}
                                    cols={[
                                        { key: 'MeterName', label: 'Meter', field: 'MeterName', headerStyle: { width: 'auto' }, rowStyle: { width: 'auto' } },
                                        { key: 'Name', label: 'Channel', field: 'Name', headerStyle: { width: 'auto' }, rowStyle: { width: 'auto' } },
                                        { key: 'NumberRaised', label: 'Raised', field: 'NumberRaised', headerStyle: { width: 'auto' }, rowStyle: { width: 'auto' } },
                                        { key: 'TimeInAlarm', label: 'Time in Alarm (%)', headerStyle: { width: 'auto' }, rowStyle: { width: 'auto' }, content: (item) => item.TimeInAlarm.toFixed(2) + "%" },
                                        
                                    ]}
                                    tableClass="table table-hover"
                                    data={channelList}
                                    sortKey={sort}
                                    ascending={asc}
                                    onSort={(d) => {
                                        if (sort === d.colKey)
                                            setAsc(!asc)
                                        else
                                            setSort(d.colField)
                                    }}
                                    onClick={(d) => setSelectedChannel(d.row.ID)}
                                    theadStyle={{ fontSize: 'smaller', display: 'table', tableLayout: 'fixed', width: '100%' }}
                                    tbodyStyle={{ display: 'block', overflowY: 'scroll', maxHeight: 400, width: '100%' }}
                                    rowStyle={{ fontSize: 'smaller', display: 'table', tableLayout: 'fixed', width: '100%' }}
                                    selected={(item) => item.ID == selectedChannel}
                                />
                            }
                        </div>
                    </div>
                </div>
                <div className="col-6">
                    
                    <div className="row" style={{ margin: 0 }}>
                        <div className="col">
                            <h3>Overview Alarm:</h3 >
                        </div>
                    </div>

                    <div className="row">
                        <div className="col">
                            {loading == 'loading' ? 
                                <LoadingIcon Show={true} Size={40} /> :
                                <Table<IResultTable>
                                    tableStyle={{ maxHeight: '300px' }}
                                    cols={[
                                        { key: 'Severity', label: 'Severity', headerStyle: { width: 'auto' }, rowStyle: { width: 'auto' }, content: (item) => <p style={{ color: severities.find(s => s.Name == item.Severity).Color }}>{item.Severity}</p> },
                                        { key: 'Threshhold', label: 'Threshhold', headerStyle: { width: 'auto' }, rowStyle: { width: 'auto' }, content: (item) => (item.Threshhold == undefined ? "N/A" : item.Threshhold) },
                                        { key: 'NumberRaised', label: 'Raised', field: 'NumberRaised', headerStyle: { width: 'auto' }, rowStyle: { width: 'auto' } },
                                        { key: 'TimeInAlarm', label: 'Time in Alarm (%)', headerStyle: { width: 'auto' }, rowStyle: { width: 'auto' }, content: (item) => item.TimeInAlarm.toFixed(2) + "%" },
                                    ]}
                                    tableClass="table thead-dark table-striped"
                                    data={resultSummary}
                                    sortKey={'Severity'}
                                    ascending={false}
                                    onSort={(d) => {
                                    }}
                                    onClick={(d) => { }}
                                    theadStyle={{ fontSize: 'smaller', display: 'table', tableLayout: 'fixed', width: '100%' }}
                                    tbodyStyle={{ display: 'block', overflowY: 'scroll', maxHeight: 240, width: '100%' }}
                                    rowStyle={{ fontSize: 'smaller', display: 'table', tableLayout: 'fixed', width: '100%' }}
                                    selected={(item) => false}
                                />
                            }
                        </div>
                    </div>
                    <div className="row" style={{ margin: 0 }}>
                        <div className="col">
                            {selectedChannel > -1 ? <h3>{channelList.find(item => item.ID == selectedChannel).MeterName} - {channelList.find(item => item.ID == selectedChannel).Name}:</h3> : null}
                        </div>
                    </div>
                    <div className="row">
                        <div className="col">
                            {loading == 'loading' ?
                                <LoadingIcon Show={true} Size={40} /> :
                                <Table<IResultTable>
                                    tableStyle={{ maxHeight: '300px' }}
                                    cols={[
                                        { key: 'Severity', label: 'Severity', headerStyle: { width: 'auto' }, rowStyle: { width: 'auto' }, content: (item) => <p style={{ color: severities.find(s => s.Name == item.Severity).Color }}>{item.Severity}</p> },
                                        { key: 'Threshhold', label: 'Threshhold', headerStyle: { width: 'auto' }, rowStyle: { width: 'auto' }, content: (item) => (item.Threshhold == undefined ? "N/A" : item.Threshhold) },
                                        { key: 'NumberRaised', label: 'Raised', field: 'NumberRaised', headerStyle: { width: 'auto' }, rowStyle: { width: 'auto' } },
                                        { key: 'TimeInAlarm', label: 'Time in Alarm (%)', headerStyle: { width: 'auto' }, rowStyle: { width: 'auto' }, content: (item) => item.TimeInAlarm.toFixed(2) + " %" },
                                    ]}
                                    tableClass="table thead-dark table-striped"
                                    data={channelSummary}
                                    sortKey={'Severity'}
                                    ascending={false}
                                    onSort={(d) => {
                                    }}
                                    onClick={(d) => { }}
                                    theadStyle={{ fontSize: 'smaller', display: 'table', tableLayout: 'fixed', width: '100%' }}
                                    tbodyStyle={{ display: 'block', overflowY: 'scroll', maxHeight: 240, width: '100%' }}
                                    rowStyle={{ fontSize: 'smaller', display: 'table', tableLayout: 'fixed', width: '100%' }}
                                    selected={(item) => false}
                                />
                            }
                        </div>
                    </div>

                </div>
            </div>
            <div className="row" style={{ margin: 0 }}>
                <div className="col">
                    {selectedChannel != -1 && (loading == 'idle' || loading == 'changed') ? <AlarmTrendingCard Tend={effectiveTimeRange.end} Tstart={effectiveTimeRange.start} ChannelID={selectedChannel} /> : null}
                </div>
            </div>
            <Warning  Title={'This operation may take some time to process.'}
                CallBack={(c) => { if (c) setUserConfirm('loading'); else setUserConfirm('uninitialized'); }}
                Show={userConfirm == 'confirm'}
                Message={'This operation may take some time to process. To speed up testing these setpoints please select a shorter timeframe or fewer channels.'}
                ShowCancel={true}
            />
        </div>
    );
}

export default WizardTest
