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
import { selectAffectedChannels, selectfactors, selectSeverity } from './StaticWizzardSlice';
import _ from 'lodash';
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

    React.useEffect(() => {
        let lst = testResult.map(item => {
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
    }, [testResult]);

    React.useEffect(() => { setChannelList((old) => { return _.orderBy(old, [sort], [asc ? "asc" : "desc"]) }) }, [asc, sort]);

    let resultSummary: IResultTable[] = [
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
    ]

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
                        </div>
                    </div>
                </div>
            </div>
            <div className="row" style={{ margin: 0 }}>

            </div>
            <div className="row" style={{ margin: 0 }}>

            </div>
           
            
        </div>      
    );
}

export default TestGroup