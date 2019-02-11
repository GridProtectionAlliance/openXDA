//******************************************************************************************************
//  TrendingChart.tsx - Gbtc
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
//  07/19/2018 - Billy Ernest
//       Generated original version of source code.
//
//******************************************************************************************************

import * as React from 'react';
 import * as moment from 'moment';
import * as _ from "lodash";
import './../flot/jquery.flot.min.js';
import './../flot/jquery.flot.crosshair.min.js';
import './../flot/jquery.flot.navigate.min.js';
import './../flot/jquery.flot.selection.min.js';
import './../flot/jquery.flot.time.min.js';

export default class TrendingChart extends React.Component<any, any>{
    plot: any;
    zoomId: any;
    props: { startDate: string, endDate: string, data: JSON, type: Array<string>, stateSetter: Function }
    hover: number;
    startDate: string;
    endDate: string;
    state: {}
    options: { canvas: boolean, legend: object, crosshair: object, selection: object, grid: object, xaxis: {mode: string, tickLength: number, reserveSpace: boolean, ticks: Function, tickFormatter: Function, max: number, min: number}, yaxis: object, }

    constructor(props) {
        super(props);

        this.hover = 0;
        this.startDate = props.startDate;
        this.endDate = props.endDate;

        var ctrl = this;

        this.options = {
            canvas: true,
            legend: { show: true },
            crosshair: { mode: "x" },
            selection: { mode: "x" },
            grid: {
                autoHighlight: false,
                clickable: true,
                hoverable: true,
                markings: []
            },
            xaxis: {
                mode: "time",
                tickLength: 10,
                reserveSpace: false,
                ticks: function (axis) {
                    var ticks = [],
                        start = ctrl.floorInBase(axis.min, axis.delta),
                        i = 0,
                        v = Number.NaN,
                        prev;

                    do {
                        prev = v;
                        v = start + i * axis.delta;
                        ticks.push(v);
                        ++i;
                    } while (v < axis.max && v != prev);
                    return ticks;
                },
                tickFormatter: function (value, axis) {
                    if (axis.delta > 3 * 24 * 60 * 60 * 1000)
                        return moment(value).utc().format("MM/DD");
                    else
                        return moment(value).utc().format("MM/DD HH:mm");
                },
                max: null,
                min: null
            },
            yaxis: {
                labelWidth: 50,
                panRange: false,
                //ticks: 1,
                tickLength: 10,
                tickFormatter: function (val, axis) {
                    if (axis.delta > 1000000 && (val > 1000000 || val < -1000000))
                        return ((val / 1000000) | 0) + "M";
                    else if (axis.delta > 1000 && (val > 1000 || val < -1000))
                        return ((val / 1000) | 0) + "K";
                    else
                        return val.toFixed(axis.tickDecimals);
                }
            }
        }

    }


    getColor(key) {
        if (key == "Average") return '#261083';
        if (key == "Minimum") return '#30AF47';
        if (key == "Maximum") return '#DB0404';
        return '#000000';
    }

    createDataRows(props) {
        // if start and end date are not provided calculate them from the data set
        if (this.plot != undefined) $(this.refs.graph).children().remove();

        var ctrl = this;
        var startString = this.startDate;
        var endString = this.endDate;
        var newVessel = [];

        if (props.data != null) {
            $.each(Object.keys(props.data), (i, key) => {
                if(props.type.indexOf(key) >= 0)
                    newVessel.push({ label: key, data: props.data[key], color: this.getColor(key) })
            });
        }
        newVessel.push([[this.getMillisecondTime(startString), null], [this.getMillisecondTime(endString), null]]);
        this.options.xaxis.max = this.getMillisecondTime(endString);
        this.options.xaxis.min = this.getMillisecondTime(startString);

        this.plot = ($ as any).plot($(this.refs.graph), newVessel, this.options);
        this.plotSelected();
        this.plotZoom();
        this.plotHover();
        //this.plotClick();
    }

    componentDidMount() {
        this.createDataRows(this.props);
    }

    componentWillReceiveProps(nextProps) {
        this.startDate = nextProps.startDate;
        this.endDate = nextProps.endDate;
        this.createDataRows(nextProps);
    }

    render() {
        return <div ref={'graph'} style={{ height: 'inherit', width: 'inherit'}}></div>;
    }

    // round to nearby lower multiple of base
    floorInBase(n, base) {
        return base * Math.floor(n / base);
    }

    getMillisecondTime(date) {
        var milliseconds = moment.utc(date).valueOf();
        return milliseconds;
    }

    getDateString(float) {
        var date = moment.utc(float).format('YYYY-MM-DDTHH:mm');
        return date;
    }

    plotSelected() {
        var ctrl = this;
        $(this.refs.graph).off("plotselected");
        $(this.refs.graph).bind("plotselected", function (event, ranges) {
            ctrl.props.stateSetter({ startDate: ctrl.getDateString(ranges.xaxis.from), endDate: ctrl.getDateString(ranges.xaxis.to) });
        });
    }

    plotZoom() {
        var ctrl = this;
        $(this.refs.graph).off("plotzoom");
        $(this.refs.graph).bind("plotzoom", function (event) {
            var minDelta = null;
            var maxDelta = 5;
            var xaxis = ctrl.plot.getAxes().xaxis;
            var xcenter = ctrl.hover;
            var xmin = xaxis.options.min;
            var xmax = xaxis.options.max;
            var datamin = xaxis.datamin;
            var datamax = xaxis.datamax;

            var deltaMagnitude;
            var delta;
            var factor;

            if (xmin == null)
                xmin = datamin;

            if (xmax == null)
                xmax = datamax;

            if (xmin == null || xmax == null)
                return;

            xcenter = Math.max(xcenter, xmin);
            xcenter = Math.min(xcenter, xmax);

            if ((event.originalEvent as any).wheelDelta != undefined)
                delta = (event.originalEvent as any).wheelDelta;
            else
                delta = -(event.originalEvent as any).detail;

            deltaMagnitude = Math.abs(delta);

            if (minDelta == null || deltaMagnitude < minDelta)
                minDelta = deltaMagnitude;

            deltaMagnitude /= minDelta;
            deltaMagnitude = Math.min(deltaMagnitude, maxDelta);
            factor = deltaMagnitude / 10;

            if (delta > 0) {
                xmin = xmin * (1 - factor) + xcenter * factor;
                xmax = xmax * (1 - factor) + xcenter * factor;
            } else {
                xmin = (xmin - xcenter * factor) / (1 - factor);
                xmax = (xmax - xcenter * factor) / (1 - factor);
            }

            if (xmin == xaxis.options.xmin && xmax == xaxis.options.xmax)
                return;

            ctrl.startDate = ctrl.getDateString(xmin);
            ctrl.endDate = ctrl.getDateString(xmax);

            ctrl.createDataRows(ctrl.props);

            clearTimeout(ctrl.zoomId);

            ctrl.zoomId = setTimeout(() => {
                ctrl.props.stateSetter({ startDate: ctrl.getDateString(xmin), endDate: ctrl.getDateString(xmax) });
            }, 250);
        });

    }

    plotHover() {
        var ctrl = this;
        $(this.refs.graph).off("plothover");
        $(this.refs.graph).bind("plothover", function (event, pos, item) {
            ctrl.hover = pos.x;
        });
    }


}