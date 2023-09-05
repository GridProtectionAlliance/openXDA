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

import  { SPCTools } from "../global";
import * as React from 'react';
import { useSelector } from "react-redux";
import { openXDA } from "../global";
import { SelectAffectedChannelByID } from "../store/WizardAffectedChannelSlice";
import { SelectStatisticsFilter, SelectThresholdValues, selectAlarmGroup, SelectAlarmFactors, SelectStatisticsChannels } from "./DynamicWizzardSlice";
import _ from "lodash";
import moment from "moment";
import { SelectSeverities } from "../store/SeveritySlice";
import { SelectAlarmDays } from "../store/AlarmDaySlice";
import { Line, Plot } from '@gpa-gemstone/react-graph'
import { TrashCan } from '@gpa-gemstone/gpa-symbols';

interface IProps { ChannelID: number, Remove?: () => void, Tstart: string, Tend: string }

export const AlarmTrendingCard = (props: IProps) => {
    const memChannelSelector = React.useMemo<(state) => openXDA.IChannel>(() => {
        if (props.ChannelID > -1)
            return (state) => SelectAffectedChannelByID(state, props.ChannelID)
        else 
            return (state) => null
    }, [props.ChannelID])

    const allChannels = useSelector(SelectStatisticsChannels)
    const [data, setData] = React.useState<Array<SPCTools.ITrendSeries>>([]);
    const [threshhold, setThreshold] = React.useState<Array<SPCTools.ITrendSeries>>([]);
    const alarmValueResults = useSelector(SelectThresholdValues);

    const channel = useSelector(memChannelSelector)
    const [loading, setLoading] = React.useState<boolean>(false);
    const dataFilter = useSelector(SelectStatisticsFilter);

    const severities = useSelector(SelectSeverities);
    const alarmGroup = useSelector(selectAlarmGroup)
    const alarmFactors = useSelector(SelectAlarmFactors)
    const alarmDays = useSelector(SelectAlarmDays)
    const divref = React.useRef(null);
    let filteredThreshold: SPCTools.ITrendSeries[] = [];

    if (threshhold.length > 0) {
        filteredThreshold = threshhold.filter(series => !series.data.some(point => isNaN(point[0]) || isNaN(point[1])));
    }
    const [Width, SetWidth] = React.useState<number>(0);
    React.useLayoutEffect(() => { SetWidth(divref?.current?.offsetWidth-25 ?? 0) });

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
                data: fullResult.map(pt => [pt[0], pt[1] * factorItem.Factor])
            } as SPCTools.ITrendSeries
        })]);

    }

    /* Generates a section of an alarm for 1 Day returning an array of points (t,y) with y corresponding to the Alarm. */
    /* Tday is the start of day. the result should cover t=Tday to t=Tday+864000000 which is the start of the next day */
    function CreateDailyThreshold(Tday, alarmDayId): number[][] {

        /* alarmValues are the sections of the alarm that apply on this day. */
        let alarmValues = alarmValueResults.filter(item => (item.AlarmDayID == alarmDayId) || (alarmDayId == undefined && item.AlarmDayID == undefined));

        /* If no alarm applies on this day we return NaN */
        if (alarmValues.length == 0)
            return [[Tday, NaN], [Tday + 86400000, NaN]]

        /* sort by earliest section */
        _.sortBy(alarmValues, item => item.StartHour);
        let channelIndex = alarmValues[0].Value.findIndex(item => item.ChannelID == props.ChannelID);

        /* An Alarm can be a scalar (same for any channel, e.g. 500) or an array (e.g. Vbase) where each Channel has a differnt Value. If it is not a scalar and no channel is specifed we can't compute an actual Value */
        if (channelIndex == -1 && !alarmValues[0].IsScalar)
            return [[Tday, NaN], [Tday + 86400000, NaN]];

        /* If no channel is specified and it is a scalar we can use any channel (since it is the same value for all of them */
        if (channelIndex == -1 && alarmValues[0].IsScalar)
            channelIndex = 0;


        let result = [];
        let i = 0;
        for (i = 0; i < (alarmValues.length - 1); i = i + 1) {
            /* for each little section we compute the actual number based on the channel to be used */
            let lim = alarmValues[i].Value[channelIndex];

            /* an add 2 points at the beginning and beinning of the next section (end of this one) */
            result.push([Tstart + alarmValues[i].StartHour * 3600000, (lim == undefined ? NaN : lim.Value)]);
            result.push([Tstart + alarmValues[i + 1].StartHour * 3600000, (lim == undefined ? NaN : lim.Value)]);
        }
        let lim = alarmValues[alarmValues.length - 1].Value[channelIndex];
        result.push([Tstart + alarmValues[alarmValues.length - 1].StartHour * 3600000, (lim == undefined ? NaN : lim.Value)]);
        result.push([Tstart + 24 * 3600000, (lim == undefined ? NaN : lim.Value)]);

        return result
    }

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

        handle.done((data) => {
            setData((old) => [...old, {
                color: '#3333ff',
                includeLegend: false,
                label: "",
                lineStyle: '-',
                data: data,
                opacity: (props.ChannelID == -1 ? 0.5 : 1.0)
            }]);
        });
          
        return handle;
    }

    let Tstart: number = (data.length > 0? (data[0].data.length > 0? data[0].data[0][0] : 0) : 0);
    let Tend: number = (data.length > 0 ? (data[0].data.length > 0? data[0].data[data[0].data.length - 1][0] : 1500) : 1500);


    return (
        <>
            <div className="row" style={{ margin: 0, width: '100%', textAlign: 'center', background: '#bbbbbb',}}>
                <div className="col-12">
                    <h5 className='float-left'> {(channel != undefined ? channel.MeterName + "-" + channel.Name : "Overview")}</h5>
                    <p className='float-right'> {props.Remove != undefined ? <a title='Remove Graph' className="btn" onClick={() => props.Remove()}>{TrashCan}</a> : null} </p>
                </div>
            </div>
            <div className="row" style={{ margin: 0, width: '100%', textAlign: 'center', background: '#bbbbbb' }}>
                <div className="col-12" ref={divref}>
                    {loading ?
                        <div className="text-center" style={{ width: '100%', margin: 'auto' }}>
                            <div className="spinner-border" role="status"></div>
                        </div> : 
                        <Plot height={250} width={Width} defaultTdomain={[Tstart, Tend]} Tlabel={'Time'} zoom={true} showMouse={false} useMetricFactors={false}>
                            {(data.length > 0 && data[0].data.length > 0) ? data.map((series, i) => <Line key={i} data={series.data} color={series.color} lineStyle={series.lineStyle} />)
                            : <></>
                            }

                            {(filteredThreshold.length > 0 && filteredThreshold[0].data.length > 0) ? filteredThreshold.map((series, i) => <Line key={i} data={series.data} color={series.color} lineStyle={series.lineStyle} />)
                            : <></>
                            }

                        </Plot>
                    }
                </div>
            </div>
        </>
    );
    
}
