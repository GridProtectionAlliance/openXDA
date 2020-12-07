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
import { SelectStatisticsFilter } from "./DynamicWizzardSlice";


interface IProps { ChannelID: number, Remove: () => void, Tstart: string, Tend: string }

export const AlarmTrendingCard = (props: IProps) => {
    const memChannelSelector = React.useMemo<(state) => openXDA.IChannel>(() => {
        if (props.ChannelID > -1)
            return (state) => SelectAffectedChannelByID(state, props.ChannelID)
        else 
            return (state) => null
    }, [props.ChannelID])

    const [data, setData] = React.useState<Array<ITrendSeries>>([]);
    const [threshhold, setThreshold] = React.useState<Array<ITrendSeries>>([]);
    const channel = useSelector(memChannelSelector)
    const [loading, setLoading] = React.useState<boolean>(false);
    const dataFilter = useSelector(SelectStatisticsFilter);

    React.useEffect(() => {
        setData([]);
        setLoading(true);

        let handle = [];
        if (channel == null)
            handle = [];
        else
            handle.push(getData(props.ChannelID));

        Promise.all(handle).then(() => setLoading(false));

        return () => {
            handle.forEach(h => { if (h != undefined && h.abort != undefined) h.abort();})
        }
    }, [channel])
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
                        <TrendingCard keyString={'Graph-' + props.ChannelID} allowZoom={false} height={125} xLabel={"Time"} Tstart={Tstart} Tend={Tend} data={[...data, ...threshhold]} />
                    }
                </div>
            </div>
        </>
    );
    
    

}
