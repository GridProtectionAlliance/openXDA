//******************************************************************************************************
//  Measurement.tsx - Gbtc
//
//  Copyright © 2018, Grid Protection Alliance.  All Rights Reserved.
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
//  05/29/2018 - Billy Ernest
//       Generated original version of source code.
//
//******************************************************************************************************

import * as React from 'react';
import * as _ from "lodash";
import PeriodicDataDisplayService from './../TS/Services/PeriodicDataDisplay';
import Legend from "./Legend";
import DistributionPlot from './DistributionPlot';
import SummaryStat from './SummaryStat';
import LineChart from './LineChart';
import { PeriodicDataDisplay } from './global'

interface Props {
    meterID: number,
    startDate: string,
    endDate: string,
    key: string,
    data: PeriodicDataDisplay.MeasurementCharateristics,
    type: string,
    height: number,
    detailedReport: boolean,
    width: number,
    returnedMeasurement: () => void
}

const Measurement = (props: Props) => {
    const periodicDataDisplayService = new PeriodicDataDisplayService();
    const [legend, setLegend] = React.useState<PeriodicDataDisplay.Legend>({});
    const [display, setDisplay] = React.useState<'block' | 'none'>('block');

    React.useEffect(() => {
        getData();
    }, [props.data]);

    function getColor(index: number) {
        switch (index % 20) {
            case 0: return "#dcc582";
            case 1: return "#cb4b4b";
            case 2: return "#afd8f8";
            case 3: return "#56a14d";
            case 4: return "#734da1";
            case 5: return "#795548";
            case 6: return "#9e9e9e";
            case 7: return "#a31c73";
            case 8: return "#3a96ca";
            case 9: return "#a00bda";
            case 10: return "#aa9b66";
            case 11: return "#54ec23";
            case 12: return "#6d3827";
            case 13: return "#7b84ae";
            case 14: return "#dceccd";
            case 15: return "#0c87fc";
            case 16: return "#575ddd";
            case 17: return "#5b4f5b";
            case 18: return "#5986e3";
            case 19: return "#cb42b3";

        }
    }

    function getData() {
        periodicDataDisplayService.getData(props.meterID, props.startDate, props.endDate, window.innerWidth, props.data.MeasurementCharacteristicID, props.data.MeasurementTypeID,props.data.HarmonicGroup,props.type).done(data => {
            props.returnedMeasurement();

            if (Object.keys(data).length == 0) {
                setDisplay('none');
                return;
            }

            setDisplay('block');
            createLegendRows(data);
        });

    }

    function createLegendRows(data: PeriodicDataDisplay.ReturnData) {
        let newLegend = _.clone(legend);

        $.each(Object.keys(data), function (i, key) {
            if (newLegend[key] == undefined)
                newLegend[key] = { color: getColor(i), enabled: true, data: data[key] };
            else
                newLegend[key].data = data[key];
        });

        setLegend(newLegend);
    }

    return (
        <div className="list-group-item panel-default" style={{ padding: 0, display: display }}>
            <div className="panel-heading">{props.data.MeasurementCharacteristic} - {props.data.MeasurementType}{(props.data.MeasurementCharacteristic == 'SpectraHGroup' ? ' HG:' + props.data.HarmonicGroup : '')}</div>
            <div className="panel-body">
                <div style={{ height: props.height, float: 'left', width: '100%', marginBottom: '10px'}}>
                    <div style={{ height: 'inherit', width: props.width, position: 'absolute' }}>
                        <LineChart key={`${props.data.MeasurementCharacteristic}-${props.data.MeasurementType}${(props.data.MeasurementCharacteristic == 'SpectraHGroup' ? ' HG:' + props.data.HarmonicGroup : '')}chart`} legend={legend} startDate={props.startDate} endDate={props.endDate} width={props.width}/>
                    </div>
                    <div key={`${props.data.MeasurementCharacteristic}-${props.data.MeasurementType}${(props.data.MeasurementCharacteristic == 'SpectraHGroup' ? ' HG:' + props.data.HarmonicGroup : '')}legend`} className='legend' style={{ position: 'absolute', right: '0' , width: '200px', height: props.height - 38, marginTop: '6px', borderStyle: 'solid', borderWidth: '2px', overflowY: 'auto' }}>
                        <Legend data={legend} callback={() => setLegend(legend)} />
                    </div>
                </div>
                { (props.detailedReport?
                    <div style={{ width: '100%', margin: '10px' }}>
                        <label style={{ width: '100%' }}>Distributions</label>

                        <ul style={{ display: 'inline', position: 'relative', width: '100%', marginTop: '25px', paddingLeft: '0' }}>
                            {Object.keys(legend).map(key => {
                                var data = { key: key, data: legend[key] };
                                return (
                                    <li key={key} style={{ height: '270px', width: '600px', display: 'inline-block' }}>
                                        <table className="table" style={{ height: '270px', width: '600px' }}>
                                            <tbody>
                                                <tr style={{ height: '270px', width: '600px' }}>
                                                    <td style={{ height: 'inherit', width: '50%' }}><DistributionPlot data={data} bins={40} /></td>
                                                    <td style={{ height: 'inherit', width: '50%' }}><SummaryStat data={legend[key]} key={key} /></td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </li>
                                );
                            })}
                        </ul>
                    </div>: null) 
                }
            </div>
            <br />
        </div>
    );

}

export default Measurement;