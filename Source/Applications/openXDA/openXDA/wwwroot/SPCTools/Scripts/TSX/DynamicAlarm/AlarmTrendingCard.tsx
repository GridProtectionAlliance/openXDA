//******************************************************************************************************
//  AlarmTrendingCard.tsx - Gbtc
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
//  12/06/2020 - C. Lackner
//       Generated original version of source code.
//
//******************************************************************************************************

import TrendingCard, { ITrendSeries } from "../CommonComponents/Graph";
import * as React from 'react';
import { useSelector } from "react-redux";
import { openXDA } from "../global";
import { SelectAffectedChannelByID } from "../store/WizardAffectedChannelSlice";
import { SelectStatisticsFilter, SelectThresholdValues, selectAlarmGroup, SelectAlarmFactors, SelectAllowSlice, SelectStatisticsChannels } from "./DynamicWizzardSlice";
import _ from "lodash";
import moment from "moment";
import { SelectSeverities } from "../store/SeveritySlice";
import { SelectAlarmDays } from "../store/AlarmDaySlice";


interface IProps { ChannelID: number, Remove: () => void, Tstart: string, Tend: string }

export const AlarmTrendingCard = (props: IProps) => {
    const memChannelSelector = React.useMemo<(state) => openXDA.IChannel>(() => {
        if (props.ChannelID > -1)
            return (state) => SelectAffectedChannelByID(state, props.ChannelID)
        else 
            return (state) => null
    }, [props.ChannelID])

    const allChannels = useSelector(SelectStatisticsChannels)
    const [data, setData] = React.useState<Array<ITrendSeries>>([]);
    const [threshhold, setThreshold] = React.useState<Array<ITrendSeries>>([]);
    const alarmValueResults = useSelector(SelectThresholdValues);

    const channel = useSelector(memChannelSelector)
    const [loading, setLoading] = React.useState<boolean>(false);
    const dataFilter = useSelector(SelectStatisticsFilter);

    const severities = useSelector(SelectSeverities);
    const alarmGroup = useSelector(selectAlarmGroup)
    const alarmFactors = useSelector(SelectAlarmFactors)
    const alarmDays = useSelector(SelectAlarmDays)

    React.useEffect(() => {
        setData([]);
        setLoading(true);

        let handle = [];
        if (channel == null)
            handle = allChannels.map(item => getData(item.ID));
        else
            handle.push(getData(props.ChannelID));

        Promise.all(handle).then(() => setLoading(false));

        return () => {
            handle.forEach(h => { if (h != undefined && h.abort != undefined) h.abort();})
        }
    }, [channel])

    React.useEffect(() => {
        CreateThreshhold();
    }, [data, alarmValueResults, severities, alarmGroup, alarmFactors]) 

    function CreateThreshhold() {
        if (alarmValueResults.length == 0 || data.length == 0)
            setThreshold([]);

        let T0 = moment(props.Tstart).startOf('day').valueOf();
        // Start By attempting to grab a single threshhold for any Day
        let regularday = CreateDailyThreshold(T0, undefined);

        //if (moment(props.Tstart).day() == 0 || moment(props.Tstart).day() == 6) {
        let weekday = CreateDailyThreshold(T0, (alarmDays.find(item => item.Name == "WeekDay") == undefined ? -1 : alarmDays.find(item => item.Name == "WeekDay").ID));
        let weekend = CreateDailyThreshold(T0, (alarmDays.find(item => item.Name == "WeekEnd") == undefined ? -1 : alarmDays.find(item => item.Name == "WeekEnd").ID));
        let sunday = CreateDailyThreshold(T0, (alarmDays.find(item => item.Name == "Sunday") == undefined ? -1 : alarmDays.find(item => item.Name == "Sunday").ID));
        let monday = CreateDailyThreshold(T0, (alarmDays.find(item => item.Name == "Monday") == undefined ? -1 : alarmDays.find(item => item.Name == "Monday").ID));
        let tuesday = CreateDailyThreshold(T0, (alarmDays.find(item => item.Name == "Tuesday") == undefined ? -1 : alarmDays.find(item => item.Name == "Tuesday").ID));
        let wednesday = CreateDailyThreshold(T0, (alarmDays.find(item => item.Name == "Wednesday") == undefined ? -1 : alarmDays.find(item => item.Name == "Wednesday").ID));
        let thursday = CreateDailyThreshold(T0, (alarmDays.find(item => item.Name == "Thursday") == undefined ? -1 : alarmDays.find(item => item.Name == "Thursday").ID));
        let friday = CreateDailyThreshold(T0, (alarmDays.find(item => item.Name == "Friday") == undefined ? -1 : alarmDays.find(item => item.Name == "Friday").ID));
        let saturday = CreateDailyThreshold(T0, (alarmDays.find(item => item.Name == "Saturday") == undefined ? -1 : alarmDays.find(item => item.Name == "Saturday").ID));

        //  Form a full week
        let weeklyThreshold = [];
        let i = 0;
        let Toff = 0;
        for (i = 0; i < 7; i = i + 1)
        {
            let d = (moment(props.Tstart).day() + i)%7
            if (d == 0) {
                if (sunday.some(pt => !isNaN(pt[1])))
                    weeklyThreshold.push(...sunday.map(pt => [pt[0] + Toff, pt[1]]));
                else if (weekend.some(pt => !isNaN(pt[1])))
                    weeklyThreshold.push(...weekend.map(pt => [pt[0] + Toff, pt[1]]));
                else
                    weeklyThreshold.push(...regularday.map(pt => [pt[0] + Toff, pt[1]]));
            }
            else if (d == 1) {
                if (monday.some(pt => !isNaN(pt[1])))
                    weeklyThreshold.push(...monday.map(pt => [pt[0] + Toff, pt[1]]));
                else if (weekday.some(pt => !isNaN(pt[1])))
                    weeklyThreshold.push(...weekday.map(pt => [pt[0] + Toff, pt[1]]));
                else
                    weeklyThreshold.push(...regularday.map(pt => [pt[0] + Toff, pt[1]]));
            }
            else if (d == 2) {
                if (tuesday.some(pt => !isNaN(pt[1])))
                    weeklyThreshold.push(...tuesday.map(pt => [pt[0] + Toff, pt[1]]));
                else if (weekday.some(pt => !isNaN(pt[1])))
                    weeklyThreshold.push(...weekday.map(pt => [pt[0] + Toff, pt[1]]));
                else
                    weeklyThreshold.push(...regularday.map(pt => [pt[0] + Toff, pt[1]]));
            }
            else if (d == 3) {
                if (wednesday.some(pt => !isNaN(pt[1])))
                    weeklyThreshold.push(...wednesday.map(pt => [pt[0] + Toff, pt[1]]));
                else if (weekday.some(pt => !isNaN(pt[1])))
                    weeklyThreshold.push(...weekday.map(pt => [pt[0] + Toff, pt[1]]));
                else
                    weeklyThreshold.push(...regularday.map(pt => [pt[0] + Toff, pt[1]]));
            }
            else if (d == 4) {
                if (thursday.some(pt => !isNaN(pt[1])))
                    weeklyThreshold.push(...thursday.map(pt => [pt[0] + Toff, pt[1]]));
                else if (weekday.some(pt => !isNaN(pt[1])))
                    weeklyThreshold.push(...weekday.map(pt => [pt[0] + Toff, pt[1]]));
                else
                    weeklyThreshold.push(...regularday.map(pt => [pt[0] + Toff, pt[1]]));
            }
            else if (d == 5) {
                if (friday.some(pt => !isNaN(pt[1])))
                    weeklyThreshold.push(...friday.map(pt => [pt[0] + Toff, pt[1]]));
                else if (weekday.some(pt => !isNaN(pt[1])))
                    weeklyThreshold.push(...weekday.map(pt => [pt[0] + Toff, pt[1]]));
                else
                    weeklyThreshold.push(...regularday.map(pt => [pt[0] + Toff, pt[1]]));
            }
            else if (d == 6) {
                if (saturday.some(pt => !isNaN(pt[1])))
                    weeklyThreshold.push(...saturday.map(pt => [pt[0] + Toff, pt[1]]));
                else if (weekend.some(pt => !isNaN(pt[1])))
                    weeklyThreshold.push(...weekend.map(pt => [pt[0] + Toff, pt[1]]));
                else
                    weeklyThreshold.push(...regularday.map(pt => [pt[0] + Toff, pt[1]]));
            }
            Toff = Toff + 86400000;
        }

        // For now try to do Regular Days...
        let nWeeks = Math.ceil((moment(props.Tend).valueOf() - moment(props.Tstart).valueOf()) / 604800000.0);

        let fullResult = [];
        
        for (i = 0; i < nWeeks; i = i + 1) {
            fullResult.push(...weeklyThreshold.map(item => [item[0] + i * 604800000, item[1]]))
        }

        setThreshold([{
            lineStyle: ':',
            color: severities.find(item => item.ID == alarmGroup.SeverityID).Color,
            includeLegend: false,
            label: '',
            opacity: 1,
            data: fullResult
        }, ...alarmFactors.map(factorItem => {
            return {
                lineStyle: ':',
                color: severities.find(item => item.ID == factorItem.SeverityID).Color,
                includeLegend: false,
                label: '',
                opacity: 1,
                data: fullResult.map(pt => [pt[0], pt[1] * factorItem.Value])
            } as ITrendSeries
        })]);
    }

    function CreateDailyThreshold(Tday, alarmDayId): number[][] {
        let alarmValues = alarmValueResults.filter(item => (item.AlarmDayID == alarmDayId) || (alarmDayId == undefined && item.AlarmDayID == undefined));
        if (alarmValues.length == 0)
            return [[Tday, NaN], [Tday + 86400000, NaN]]

        _.sortBy(alarmValues, item => item.StartHour);
        let channelIndex = alarmValues[0].Value.findIndex(item => item.ChannelID == props.ChannelID);
        if (channelIndex == -1 && props.ChannelID != -1)
            return [[Tday, NaN], [Tday + 86400000, NaN]];
        if (props.ChannelID == -1 && !alarmValues[0].IsScalar)
            return [[Tday, NaN], [Tday + 86400000, NaN]];

        if (props.ChannelID == -1)
            channelIndex = 0;


        let result = [];
        let i = 0;
        for (i = 0; i < (alarmValues.length - 1); i = i + 1) {
            let lim = alarmValues[i].Value[channelIndex];

            result.push([Tstart + alarmValues[i].StartHour * 3600000, (lim == undefined ? NaN : lim.Value)]);
            result.push([Tstart + alarmValues[i + 1].StartHour * 3600000, (lim == undefined ? NaN : lim.Value)]);
        }

        let lim = alarmValues[alarmValues.length - 1].Value[channelIndex];
        result.push([Tstart + alarmValues[alarmValues.length - 1].StartHour * 3600000, (lim == undefined ? NaN : lim.Value)]);
        result.push([Tstart + 24 * 3600000, (lim == undefined ? NaN : lim.Value)]);

        return result
    }
/*
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
    */
    function getData(channelId: number): JQuery.jqXHR<Array<number[]>> {
        let handle = $.ajax({
            type: "POST",
            url: `${apiHomePath}api/SPCTools/Data/HistoryData/${channelId}/${0}?start=${props.Tstart}&end=${props.Tend}`,
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(dataFilter),
            dataType: 'json',
            cache: false,
            async: true
        });

        handle.done((data) => setData((old) => [...old, {
            color: '#3333ff',
            includeLegend: false,
            label: "",
            lineStyle: '-',
            data: data,
            opacity: (props.ChannelID == -1 ? 0.5 : 1.0)
        }]));
          
        return handle;
    }

    let Tstart = (data.length > 0? data[0].data[0][0] : 0);
    let Tend = (data.length > 0 ? data[0].data[data[0].data.length - 1][0] : 1500);

    return (
        <>
            <div className="row" style={{ margin: 0, width: '100%', textAlign: 'center', background: '#bbbbbb',}}>
                <div className="col-12">
                    <h5 className='float-left'> {(channel != undefined ? channel.MeterName + "-" + channel.Name : "Overview")}</h5>
                    <p className='float-right'> <i className="fa fa-trash-o" onClick={() => props.Remove()}></i> </p>
                </div>
            </div>
            <div className="row" style={{ margin: 0, width: '100%', textAlign: 'center', background: '#bbbbbb', }}>
                <div className="col-12">
                    {loading ?
                        <div className="text-center" style={{ width: '100%', margin: 'auto' }}>
                            <div className="spinner-border" role="status"></div>
                        </div> :
                        <TrendingCard keyString={'Graph-' + props.ChannelID} allowZoom={true} height={125} xLabel={"Time"} Tstart={Tstart} Tend={Tend} data={[...data, ...threshhold]} />
                    }
                </div>
            </div>
        </>
    );
    
    

}
