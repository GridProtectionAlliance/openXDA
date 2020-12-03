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
import { SPCTools } from '../global';
import Table from '@gpa-gemstone/react-table';
import { DateRangePicker } from '@gpa-gemstone/react-forms';
import { useSelector, useDispatch } from 'react-redux';
import { selectAffectedChannels, selectfactors, selectSeverity, selectTokenizerResponse, selectTokenizerRequest } from './StaticWizzardSlice';
import _, { cloneDeep } from 'lodash';
import TrendingCard, { ITrendSeries } from '../CommonComponents/Graph';
import { selectDateRange, setDate, selectResultSummary, selectIsLoading } from '../Store/HistoryTestSlice';
import { selectSeverities } from '../Store/GeneralSettingsSlice';

declare var homePath: string;
declare var apiHomePath: string;

declare var userIsAdmin: boolean;

interface IProps { }
interface IResultTable { Severity: string, Threshhold: number, NumberRaised: number, TimeInAlarm: number}
interface IChannelList { MeterName: string, Name: string, NumberRaised: number, TimeInAlarm: number, ID: number }


const TestGroup = (props: IProps) => {
    const dispatch = useDispatch();

    const timeRange = useSelector(selectDateRange);
    const [channelList, setChannelList] = React.useState<IChannelList[]>([])
    const allChannels = useSelector(selectAffectedChannels);

    const testResult = useSelector(selectResultSummary)

    const [sort, setSort] = React.useState<keyof IChannelList>('NumberRaised')
    const [asc, setAsc] = React.useState<boolean>(false)

    const factors = useSelector(selectfactors);
    const severityID = useSelector(selectSeverity);
    const severities = useSelector(selectSeverities)

    const loading = useSelector(selectIsLoading);

    // Plot Data is Local since it is not used anywhere else
    const [selectedChannel, setSelectedChannel] = React.useState<number>(-1);

    //Table Data:
    const [resultSummary, setResultSummary] = React.useState<IResultTable[]>([]);
    const [channelSummary, setChannelSummary] = React.useState<IResultTable[]>([]);


    React.useEffect(() => {
        let lst = [];
        if (testResult != undefined)
          lst = testResult.map(item => {
            let bindex = item.FactorTests.findIndex(d => d.Factor == 1.0)
            let cindex = allChannels.findIndex(c => item.ChannelID == c.ID)
            return {
                TimeInAlarm: item.FactorTests[bindex].TimeInAlarm,
                NumberRaised: item.FactorTests[bindex].NumberRaised,
                ID: item.ChannelID,
                Name: (cindex == -1 ? '' : allChannels[cindex].Name),
                MeterName: (cindex == -1 ? '' : allChannels[cindex].MeterName),
            } as IChannelList
        })
        setChannelList(lst)
        createSummaryTable();

    }, [testResult]);

    React.useEffect(() => { setChannelList((old) => { return _.orderBy(old, [sort], [asc ? "asc" : "desc"]) }) }, [asc, sort]);

    React.useEffect(() => {
        createChannelTable();
    }, [testResult, selectedChannel]);

    function createSummaryTable() {
        if (testResult == undefined)
            setResultSummary([]);
        else
            setResultSummary([
                {
                    Severity: severities.find(item => item.ID == severityID).Name,
                    NumberRaised: testResult.map(ch => { return ch.FactorTests.find(i => i.Factor == 1.0).NumberRaised }).reduce((a, b) => a + b, 0),
                    TimeInAlarm: (testResult.map(ch => { return ch.FactorTests.find(i => i.Factor == 1.0).TimeInAlarm }).reduce((a, b) => a + b, 0) / testResult.length),
                    Threshhold: undefined
                },
                ...factors.map(f => {
                    return {

                        Severity: severities.find(item => item.ID == f.SeverityID).Name,
                        NumberRaised: testResult.map(ch => { return ch.FactorTests.find(i => i.Factor == f.Value).NumberRaised }).reduce((a, b) => a + b, 0),
                        TimeInAlarm: (testResult.map(ch => { return ch.FactorTests.find(i => i.Factor == f.Value).TimeInAlarm }).reduce((a, b) => a + b, 0) / testResult.length),
                        Threshhold: undefined
                    }
                })
            ])
    }

    function createChannelTable() {
        if (testResult == undefined || selectedChannel == -1) {
            setChannelSummary([]);
            return;
        }
        let index = testResult.findIndex(ch => ch.ChannelID == selectedChannel);

        if (index == -1) {
            setChannelSummary([]);
            return;
        }
        
        setChannelSummary([
            {
                Severity: severities.find(item => item.ID == severityID).Name,
                NumberRaised: testResult[index].FactorTests.find(i => i.Factor == 1.0).NumberRaised,
                TimeInAlarm: testResult[index].FactorTests.find(i => i.Factor == 1.0).TimeInAlarm,
                Threshhold: undefined
            },
            ...factors.map(f => {
                return {

                    Severity: severities.find(item => item.ID == f.SeverityID).Name,
                    NumberRaised: testResult[index].FactorTests.find(i => i.Factor == f.Value).NumberRaised,
                    TimeInAlarm: testResult[index].FactorTests.find(i => i.Factor == f.Value).TimeInAlarm,
                    Threshhold: undefined
                }
            })
        ])
    }

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
                            <DateRangePicker<SPCTools.IDateRange> Label={''} Record={timeRange} FromField={'start'} ToField={'end'} Setter={(r) => dispatch(setDate(r))} />
                        </div>
                    </div>
                    <div className="row" style={{ margin: 0 }}>
                        <div className="col">
                            {loading ?
                                <div className="text-center" style={{ width: '100%', margin: 'auto' }}>
                                    <div className="spinner-border" role="status"></div>
                                </div> :
                                <Table<IChannelList>
                                    tableStyle={{ height: '100%' }}
                                    cols={[
                                        { key: 'MeterName', label: 'Meter', headerStyle: { width: 'auto' }, rowStyle: { width: 'auto' } },
                                        { key: 'Name', label: 'Channel', headerStyle: { width: 'auto' }, rowStyle: { width: 'auto' } },
                                        { key: 'TimeInAlarm', label: 'Time in Alarm', headerStyle: { width: 'auto' }, rowStyle: { width: 'auto' } },
                                        { key: 'NumberRaised', label: 'Raised', headerStyle: { width: 'auto' }, rowStyle: { width: 'auto' } },
                                    ]}
                                    tableClass="table table-hover"
                                    data={channelList}
                                    sortField={sort}
                                    ascending={asc}
                                    onSort={(d) => {
                                        if (sort === d.col)
                                            setAsc(!asc)
                                        else
                                            setSort(d.col)
                                    }}
                                    onClick={(d) => setSelectedChannel(d.row.ID)}
                                    theadStyle={{ fontSize: 'smaller', display: 'table', tableLayout: 'fixed', width: '100%' }}
                                    tbodyStyle={{ display: 'block', overflowY: 'scroll', maxHeight: window.innerHeight - 300, width: '100%' }}
                                    rowStyle={{ fontSize: 'smaller', display: 'table', tableLayout: 'fixed', width: '100%' }}
                                    selected={(item) => item.ID == selectedChannel}
                                />
                            }
                        </div>
                    </div>
                </div>
                <div className="col-6">
                    <div className="row">
                        <div className="col">
                            {loading ? 
                                <div className="text-center" style={{ width: '100%', margin: 'auto' }}>
                                    <div className="spinner-border" role="status"></div>
                                </div> :
                                <Table<IResultTable>
                                    tableStyle={{ maxHeight: '300px' }}
                                    cols={[
                                        { key: 'Severity', label: 'Severity', headerStyle: { width: 'auto' }, rowStyle: { width: 'auto' }, content: (item) => <p style={{ color: severities.find(s => s.Name == item.Severity).Color }}>{item.Severity}</p> },
                                        { key: 'Threshhold', label: 'Threshhold', headerStyle: { width: 'auto' }, rowStyle: { width: 'auto' }, content: (item) => (item.Threshhold == undefined ? "N/A" : item.Threshhold) },
                                        { key: 'NumberRaised', label: 'Triggered', headerStyle: { width: 'auto' }, rowStyle: { width: 'auto' } },
                                        { key: 'TimeInAlarm', label: 'Time in Alarm', headerStyle: { width: 'auto' }, rowStyle: { width: 'auto' }, content: (item) => item.TimeInAlarm.toFixed(2) + "%" },
                                    ]}
                                    tableClass="table thead-dark table-striped"
                                    data={resultSummary}
                                    sortField={'Severity'}
                                    ascending={false}
                                    onSort={(d) => {
                                    }}
                                    onClick={(d) => { }}
                                    theadStyle={{ fontSize: 'smaller', display: 'table', tableLayout: 'fixed', width: '100%' }}
                                    tbodyStyle={{ display: 'block', overflowY: 'scroll', maxHeight: window.innerHeight - 300, width: '100%' }}
                                    rowStyle={{ fontSize: 'smaller', display: 'table', tableLayout: 'fixed', width: '100%' }}
                                    selected={(item) => false}
                                />
                            }
                        </div>
                    </div>

                    <div className="row">
                        <div className="col">
                            {loading ?
                                <div className="text-center" style={{ width: '100%', margin: 'auto' }}>
                                    <div className="spinner-border" role="status"></div>
                                </div> :
                                <Table<IResultTable>
                                    tableStyle={{ maxHeight: '300px' }}
                                    cols={[
                                        { key: 'Severity', label: 'Severity', headerStyle: { width: 'auto' }, rowStyle: { width: 'auto' }, content: (item) => <p style={{ color: severities.find(s => s.Name == item.Severity).Color }}>{item.Severity}</p> },
                                        { key: 'Threshhold', label: 'Threshhold', headerStyle: { width: 'auto' }, rowStyle: { width: 'auto' }, content: (item) => (item.Threshhold == undefined ? "N/A" : item.Threshhold) },
                                        { key: 'NumberRaised', label: 'Triggered', headerStyle: { width: 'auto' }, rowStyle: { width: 'auto' } },
                                        { key: 'TimeInAlarm', label: 'Time in Alarm', headerStyle: { width: 'auto' }, rowStyle: { width: 'auto' }, content: (item) => item.TimeInAlarm.toFixed(2) + " %" },
                                    ]}
                                    tableClass="table thead-dark table-striped"
                                    data={channelSummary}
                                    sortField={'Severity'}
                                    ascending={false}
                                    onSort={(d) => {
                                    }}
                                    onClick={(d) => { }}
                                    theadStyle={{ fontSize: 'smaller', display: 'table', tableLayout: 'fixed', width: '100%' }}
                                    tbodyStyle={{ display: 'block', overflowY: 'scroll', maxHeight: window.innerHeight - 300, width: '100%' }}
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
                    {selectedChannel != -1 ? < GraphCard ChannelID={selectedChannel} startDate={timeRange.start} endDate={timeRange.end} /> : null}
                </div>
            </div>
            
           
            
        </div>      
    );
}

const GraphCard = (props: { ChannelID: number, startDate: string, endDate: string  }) => {

    const [data, setData] = React.useState<Array<ITrendSeries>>([]);
    const [threshhold, setThreshold] = React.useState<Array<ITrendSeries>>([]);
   

    const secerityID = useSelector(selectSeverity);
    const severities = useSelector(selectSeverities)
    const factors = useSelector(selectfactors);

    const threshHoldResults = useSelector(selectTokenizerResponse);
    const threshHoldRequest = useSelector(selectTokenizerRequest);

    React.useEffect(() => {
        setData([]);
        let handle = getData(props.ChannelID);
        return () => {
            if (handle != undefined && handle.abort != undefined)
                handle.abort()
        }
    }, [props])

    React.useEffect(() => {

        const Tstart = (data.length > 0 ? data[0].data[0][0] : 0);
        const Tend = (data.length > 0 ? data[0].data[data[0].data.length - 1][0] : 1500);
        let index = 0;
        if (!threshHoldResults.IsScalar)
            index = threshHoldRequest.Channels.findIndex(item => item == props.ChannelID);

        const threshHold = threshHoldResults.Value[index];

        let sp = {
            color: severities.find(item => item.ID == secerityID).Color,
            includeLegend: false,
            label: "",
            lineStyle: ':',
            data: [[Tstart, threshHold], [Tend, threshHold]],
            opacity: 1.0
        } as ITrendSeries;

        setThreshold([sp, ...factors.filter(f => f.Value != 1.0).map(f => {
            return {
                color: severities.find(item => item.ID == f.SeverityID).Color,
                includeLegend: false,
                label: "",
                lineStyle: ':',
                data: [[Tstart, threshHold * f.Value], [Tend, threshHold * f.Value]],
                opacity: 1.0
            } as ITrendSeries;
        })])

    }, [data])

    function getData(channelId: number): JQuery.jqXHR<Array<number[]>> {
        let handle = $.ajax({
            type: "POST",
            url: `${apiHomePath}api/SPCTools/StaticAlarmCreation/GetData/${channelId}?start=${props.startDate}&end=${props.endDate}`,
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({}),
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
                opacity: (props.ChannelID == -1 ? 0.5 : 1.0)
            }
            updated.push(series)
            return updated;
        }))
        return handle;
    }

    let Tstart = (data.length > 0 ? data[0].data[0][0] : 0);
    let Tend = (data.length > 0 ? data[0].data[data[0].data.length - 1][0] : 1500);

    return (            
            <div className="row" style={{ margin: 0, width: '100%', textAlign: 'center', background: '#bbbbbb', }}>
                <div className="col-12">
                    <TrendingCard keyString={'Graph-' + props.ChannelID} allowZoom={false} height={125} xLabel={"Time"} Tstart={Tstart} Tend={Tend} data={[...data, ...threshhold]} />
                </div>
            </div>
    );

}
export default TestGroup