//******************************************************************************************************
//  LineChart.tsx - Gbtc
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
//  06/07/2018 - Billy Ernest
//       Generated original version of source code.
//
//******************************************************************************************************

import * as React from 'react';
import * as ReactDOM from 'react-dom';
import * as moment from 'moment';
import * as _ from "lodash";
import './../flot/jquery.flot.min.js';
//import './../flot/jquery.flot.crosshair.min.js';
//import './../flot/jquery.flot.navigate.min.js';
//import './../flot/jquery.flot.selection.min.js';
import './../flot/jquery.flot.time.min.js';

export default class LineChart extends React.Component<any, any>{
    options: object;
    plot: any;

    constructor(props) {
        super(props);
        var ctrl = this;

        this.options = {
            canvas: true,
            legend: { show: false },
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
                        return moment(value).utc().format("MM/DD");
                }
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

    componentDidMount() {

        this.createDataRows();
    }

    componentWillReceiveProps(nextProps) {
        this.createDataRows();
    }



    componentWillUnmount() {
        $(this.refs.graph).off("plotselected");
        $(this.refs.graph).off("plotzoom");
        $(this.refs.graph).off("plothover");

    }


    createDataRows() {
        // if start and end date are not provided calculate them from the data set

        if (this.plot != undefined) $(this.refs.graph).children().remove();

        var ctrl = this;
        var startString = this.props.startDate;
        var endString = this.props.endDate;
        var newVessel = [];
        if (this.props.legend != undefined && Object.keys(this.props.legend).length > 0) {
            $.each(Object.keys(this.props.legend), (i, key) => {
                if (this.props.legend[key].enabled)
                    newVessel.push({ label: key, data: this.props.legend[key].data, color: this.props.legend[key].color })
            });
        }

        newVessel.push([[this.getMillisecondTime(startString), null], [this.getMillisecondTime(endString), null]]);
        this.plot = ($ as any).plot($(this.refs.graph), newVessel, this.options);
        //this.plotSelected();
        //this.plotZoom();
        //this.plotHover();
        //this.plotClick();
    }


    render() {
        return <div ref={'graph'} style={{ height: 'inherit', width: this.props.width}}></div>;
    }


    defaultTickFormatter(value, axis) {

        var factor = axis.tickDecimals ? Math.pow(10, axis.tickDecimals) : 1;
        var formatted = "" + Math.round(value * factor) / factor;

        // If tickDecimals was specified, ensure that we have exactly that
        // much precision; otherwise default to the value's own precision.

        if (axis.tickDecimals != null) {
            var decimal = formatted.indexOf(".");
            var precision = decimal == -1 ? 0 : formatted.length - decimal - 1;
            if (precision < axis.tickDecimals) {
                return (precision ? formatted : formatted + ".") + ("" + factor).substr(1, axis.tickDecimals - precision);
            }
        }

        return formatted;
    };

    // round to nearby lower multiple of base
    floorInBase(n, base) {
        return base * Math.floor(n / base);
    }

    getMillisecondTime(date) {
        var milliseconds = moment.utc(date).valueOf();
        return milliseconds;
    }

    getDateString(float) {
        var date = moment.utc(float).format('YYYY-MM-DDTHH:mm:ss');
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
            var xcenter = ctrl.state.hover;
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

            ctrl.props.stateSetter({ startDate: ctrl.getDateString(xmin), endDate: ctrl.getDateString(xmax) });

        });

    }

    plotHover() {
        var ctrl = this;
        $(this.refs.graph).off("plothover");
        $(this.refs.graph).bind("plothover", function (event, pos, item) {
            ctrl.setState({ hover: pos.x });
        });
    }

}