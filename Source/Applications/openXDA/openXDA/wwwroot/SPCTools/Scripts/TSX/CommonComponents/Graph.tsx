//******************************************************************************************************
//  Graph.tsx - Gbtc
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
//  11/02/2020 - C. Lackner
//       Generated original version of source code.
//
//******************************************************************************************************
import * as React from 'react';
import { clone, isEqual } from 'lodash';

import * as d3 from '../../D3/d3';
import { isNullOrUndefined } from 'util';
import { deepEqual } from 'assert';
import moment from 'moment';

interface IProps {
    allowZoom: boolean,
    keyString: string,
    height: number,
    data: Array<ITrendSeries>,
    xLabel?: string,
    yLabel?: string,
    Tstart?: number,
    Tend?: number,
}

interface IState {

    Tstart: number,
    Tend: number,
    hover: number,
    points: Array<ITrendPoint>
}

export interface ITrendSeries {

    data: Array<[number, number]>,
    color: string,
    label: string,
    lineStyle: ('-' | ':'),
    includeLegend: boolean,
    opacity?: number
}

interface ITrendPoint {
    t: number,
    y: number,
    index: number
}

interface ImousePosition {
    x: number,
    y: number,
    t: number
}

export default class TrendingCard extends React.Component<IProps, IState>{

    yscale: any;
    xscale: any;
    hover: any;
    brush: any;
    paths: any;
    area: any;
    yAxis: any;
    xAxis: any;

    markers: any;
    mouseMarker: any;
    yExp: any;
    xLbl: any;



    mouseDownPos: ImousePosition;

    constructor(props, context) {
        super(props, context);

        this.mouseDownPos = { x: 0, y: 0, t: 0 };
        this.state = {
            Tstart: 0,
            Tend: 0,
            hover: 0,
            points: this.props.data.filter(series => series.includeLegend).map((series, index) => { return { t: series.data[0][0], y: series.data[0][1], index: index } })
        };
    }

    componentDidMount() {
        this.generatePlot();
    }

    componentDidUpdate(prevProps: IProps, prevState: IState) {
        if (!isEqual(prevProps, this.props)) {
            if (this.state.points.length !== this.props.data.filter(series => series.includeLegend).length)
                this.setState({ points: this.props.data.filter(series => series.includeLegend).map((series, index) => { return { t: series.data[0][0], y: series.data[0][1], index: index } }) }, () => this.generatePlot());
            else
                this.generatePlot();
        }
        else if (this.state.Tstart !== prevState.Tstart || this.state.Tend !== prevState.Tend)
            this.updatePlot();
        else if (this.state.hover !== prevState.hover) {
            this.setState({
                points: this.props.data.filter(series => series.includeLegend).map((series, index) => {
                    let i = this.reduceIndex(series.data, series.data.length - 1, 0, this.state.hover)
                    return { t: series.data[i][0], y: series.data[i][1], index: index }
                })
            })
        }
        else if (!isEqual(this.state.points, prevState.points))
            this.updateMouse();

    }

    generatePlot() {
        // remove the previous SVG object
        d3.select("#trendWindow-" + this.props.keyString + ">svg").remove()

        //add new Plot
        var container = d3.select("#trendWindow-" + this.props.keyString);

        var svg = container.append("svg")
            .attr("width", '100%')
            .attr("height", this.props.height).append("g")
            .attr("transform", "translate(40,10)");

        if (this.props.data.length == 0)
            return;
        //Then Create Axis
        let ymax = Math.max(...this.props.data.map(item => Math.max(...item.data.map(p => p[1]).filter(p => !isNaN(p) && isFinite(p)))).filter(p => !isNaN(p)));;
        let ymin = Math.min(...this.props.data.map(item => Math.min(...item.data.map(p => p[1]).filter(p => !isNaN(p) && isFinite(p)))).filter(p => !isNaN(p)));;

        this.yscale = d3.scaleLinear()
            .domain([ymin, ymax])
            .range([this.props.height - 60, 0]);

        if (!isNullOrUndefined(this.props.Tstart))
            this.stateSetter({ Tstart: this.props.Tstart });
        else
            this.stateSetter({ Tstart: Math.min(...this.props.data.map(item => Math.min(...item.data.map(p => p[0])))) });

        if (!isNullOrUndefined(this.props.Tend))
            this.stateSetter({ Tend: this.props.Tend });
        else
            this.stateSetter({ Tend: Math.max(...this.props.data.map(item => Math.max(...item.data.map(p => p[0])))) });

        this.xscale = d3.scaleLinear()
            .domain([this.state.Tstart, this.state.Tend])
            .range([20, container.node().getBoundingClientRect().width - 100])
            ;

        this.yAxis = svg.append("g").attr("transform", "translate(20,0)").call(d3.axisLeft(this.yscale).tickFormat((d, i) => this.formatValueTick(d)).ticks(4));
        this.xAxis = svg.append("g").attr("transform", "translate(0," + (this.props.height - 60) + ")").call(d3.axisBottom(this.xscale).tickFormat((d, i) => this.formatTimeTick(d)));

        this.xLbl = svg.append("text")
            .attr("transform", "translate(" + ((container.node().getBoundingClientRect().width - 100) / 2) + " ," + (this.props.height - 20) + ")")
            .style("text-anchor", "middle")
            .text("");

        if (this.props.yLabel != null)
            svg.append("text")
                .attr("transform", "rotate(-90)")
                .attr("y", -30)
                .attr("x", -(this.props.height / 2 - 30))
                .attr("dy", "1em")
                .style("text-anchor", "middle")
                .text(this.props.yLabel);



        this.yExp = svg.append("text")
            .style("text-anchor", "end")
            .text("");

        this.updateAxisLabel();

        //Add Hover
        this.hover = svg.append("line")
            .attr("stroke", "#000")
            .attr("x1", 10).attr("x2", 10)
            .attr("y1", 0).attr("y2", this.props.height - 60)
            .style("opacity", 0);

        //Add clip Path
        svg.append("defs").append("svg:clipPath")
            .attr("id", "clip-" + this.props.keyString)
            .append("svg:rect")
            .attr("width", 'calc(100% - 120px)')
            .attr("height", this.props.height - 50)
            .attr("x", 20)
            .attr("y", 0);

        // Add seperate clip Path for Markers that is 5-10 outside the original to make sure we don't cut off half of a marker
        svg.append("defs").append("svg:clipPath")
            .attr("id", "Markerclip-" + this.props.keyString)
            .append("svg:rect")
            .attr("width", 'calc(100% - 110px)')
            .attr("height", this.props.height - 40)
            .attr("x", 15)
            .attr("y", -5);


        this.paths = svg.append("g").attr("id", "path-" + this.props.keyString).attr("clip-path", "url(#clip-" + this.props.keyString + ")");
        this.markers = svg.append("g").attr("id", "marker-" + this.props.keyString).attr("clip-path", "url(#Markerclip-" + this.props.keyString + ")")

        let ctrl = this;

        this.props.data.forEach(row => {
            let ln = this.paths.append("path").datum(row.data.map(p => { return { x: p[0], y: p[1] } })).attr("fill", "none")
                .attr("stroke", row.color)
                .attr("stroke-width", 2.0)
                .attr("opacity", (row.opacity == undefined ? 1 : row.opacity))
                .attr("d", d3.line()
                    .x(function (d) { return ctrl.xscale(d.x) })
                    .y(function (d) { return ctrl.yscale(d.y) })
                    .defined(function (d) {
                        let tx = !isNaN(parseFloat(ctrl.xscale(d.x)));
                        let ty = !isNaN(parseFloat(ctrl.yscale(d.y)));
                        return tx && ty;
                    }))

            if (row.lineStyle == ':')
                ln.attr("stroke-dasharray", ("10,5"));
        });


        this.props.data.filter(series => series.includeLegend).forEach((row, index) =>
            this.markers.append("g").selectAll("circle").data(row.data.map((p, i) => { return { x: p[0], y: p[1], pointIndex: i, dataIndex: index } })).enter().append("circle")
                .attr('cx', function (d) { return ctrl.xscale(d.x) })
                .attr('cy', function (d) { return ctrl.yscale(d.y) })
                .attr('r', 3)
                .style('stroke', '#000000')
                .style('fill', row.color)
                .style('opacity', 0.5)
        );

        this.mouseMarker = svg.append("g").attr("id", "MouseMarker-" + this.props.keyString).attr("clip-path", "url(#Markerclip-" + this.props.keyString + ")")


        this.mouseMarker.selectAll("circle")
            .data(this.state.points).enter().append("circle")
            .attr('cx', function (d) { return ctrl.xscale(d.t) })
            .attr('cy', function (d) { return ctrl.yscale(d.y) })
            .attr('r', 5)
            .style('stroke', '#000000')
            .style('fill', function (d) { return ctrl.props.data.filter(series => series.includeLegend)[d.index].color })
            .style('opacity', 0.6)

        //Add Zoom Window
        this.brush = svg.append("rect")
            .attr("stroke", "#000")
            .attr("x", 10).attr("width", 0)
            .attr("y", 0).attr("height", this.props.height - 60)
            .attr("fill", "black")
            .style("opacity", 0);

        //Add rectangle on top for interaction
        this.area = svg.append("g").append("svg:rect")
            .attr("width", 'calc(100% - 120px)')
            .attr("height", '100%')
            .attr("x", 20)
            .attr("y", 0)
            .style("opacity", 0)
            .on('mousemove', this.mousemove.bind(this))
            .on('mouseout', this.mouseout.bind(this))
            .on('mousedown', this.mouseDown.bind(this))
            .on('mouseup', this.mouseUp.bind(this))
    }

    updatePlot() {
        //Update Axis
        this.xscale.domain([this.state.Tstart, this.state.Tend]);
        this.yscale.domain(this.getYlimit());
        this.yAxis.transition().duration(1000).call(d3.axisLeft(this.yscale).ticks(4).tickFormat((d, i) => this.formatValueTick(d)));
        this.xAxis.transition().duration(1000).call(d3.axisBottom(this.xscale).tickFormat((d, i) => this.formatTimeTick(d)));

        //Set Colors, update Visibility and Points
        let ctrl = this;
        this.paths.selectAll('path')
            .transition()
            .duration(1000)
            .attr("d", d3.line()
                .x(function (d) {
                    return ctrl.xscale(d.x)
                })
                .y(function (d) {
                    return ctrl.yscale(d.y)
                })
                .defined(function (d) {
                    let tx = !isNaN(parseFloat(ctrl.xscale(d.x)));
                    let ty = !isNaN(parseFloat(ctrl.yscale(d.y)));
                    return tx && ty;
                })
            );

        this.updateAxisLabel();

        this.markers.selectAll('circle')
            .transition()
            .duration(1000)
            .attr("cx", function (d) { return ctrl.xscale(d.x) })
            .attr("cy", function (d) { return ctrl.yscale(d.y) })

        this.updateMouse();
    }

    updateMouse() {
        let ctrl = this;
        this.mouseMarker.selectAll("circle").style('opacity', 0.0);

        this.mouseMarker.selectAll("circle").data(this.state.points).enter().append("circle")
            .attr('cx', function (d) { return ctrl.xscale(d.t) })
            .attr('cy', function (d) { return ctrl.yscale(d.y) })
            .attr('r', 5)
            .style('stroke', '#000000')
            .style('fill', function (d) { return ctrl.props.data.filter(series => series.includeLegend)[d.index].color })
            .style('opacity', 0.0);

        this.mouseMarker.selectAll("circle").exit().remove();


        this.mouseMarker.selectAll("circle")
            .attr("cx", function (d) { return ctrl.xscale(d.t) })
            .attr("cy", function (d) { return ctrl.yscale(d.y) })
            .transition().duration(1000).style("opacity", 1.0)

    }


    updateAxisLabel() {
        let lim = this.getYlimit().map(p => Math.abs(p));
        let h = Math.max(...lim);

        let exp = Math.floor(Math.log10(h));
        let tripple = Math.floor(exp / 3);
        if (tripple == 0)
            this.yExp.text("");
        else
            this.yExp.text("x10^" + (tripple * 3).toFixed(0));

        h = this.state.Tend - this.state.Tstart;
        h = h / 1000.0;

        let tUnit = ""
        if (h < 15)
            tUnit = " (ms)"
        else if (h < 2 * 60)
            tUnit = " (s)"
        else if (h < 30 * 60)
            tUnit = " (min:sec)"
        else if (h < 60 * 60)
            tUnit = " (min)"
        else if (h < 30 * 60 * 60)
            tUnit = " (hr:min)"
        else if (h < 2 * 24 * 60 * 60)
            tUnit = " (hr)"
        else if (h < 30 * 24 * 60 * 60)
            tUnit = " (m/d hr)"
        else if (h < 20 * 30 * 24 * 60 * 60)
            tUnit = " (m/d)"



        if (this.props.xLabel != null)
            this.xLbl.text(this.props.xLabel + tUnit);

        else
            this.xLbl.text("Time" + tUnit);

    }

    getYlimit() {

        let ymin = Number.MAX_VALUE;
        let ymax = -Number.MAX_VALUE;

        this.props.data.forEach(item => {
            item.data.forEach(p => {
                if (p[0] > this.state.Tstart && p[0] < this.state.Tend) {
                    if (p[1] > ymax)
                        ymax = p[1];
                    if (p[1] < ymin)
                        ymin = p[1];
                }
            })
        })

        return [ymin, ymax];
    }

    formatTimeTick(d: number) {
        let TS = moment(d);
        let h = this.state.Tend - this.state.Tstart
        h = h / 1000.0;

        if (h < 15)
            return TS.format("SSS.SS")
        if (h < 30)
            return TS.format("ss.SS")
        if (h < 2 * 60)
            return TS.format("ss")
        if (h < 30 * 60)
            return TS.format("mm:ss")
        if (h < 60 * 60)
            return TS.format("mm")
        if (h < 30 * 60 * 60)
            return TS.format("hh:mm")
        if (h < 2 * 24 * 60 * 60)
            return TS.format("hh")
        if (h < 30 * 24 * 60 * 60)
            return TS.format("MM/DD hh")
        if (h < 20 * 30 * 24 * 60 * 60)
            return TS.format("MM/DD")
        if (h < 365 * 15 * 24 * 60 * 60)
            return TS.format("MM YYYY")

        return TS.format("YYYY")

    }

    formatValueTick(d: number) {
        let lim = this.getYlimit().map(p => Math.abs(p));
        let h = Math.max(...lim);
        let val = d;

        let exp = Math.floor(Math.log10(h));
        let tripple = Math.floor(exp / 3);
        if (tripple !== 0)
            val = d / (10 ** (tripple * 3));

        if (Math.abs(val) >= 100)
            return val.toFixed(1);
        if (Math.abs(val) >= 10)
            return val.toFixed(2);
        return val.toFixed(3);

    }

    mousemove() {

        let x = d3.mouse(this.area.node())[0];
        this.hover.attr("x1", x)
            .attr("x2", x);

        this.hover.style("opacity", 1);

        if (this.props.allowZoom) {
            let w = this.mouseDownPos.x - x;

            if (x < this.mouseDownPos.x)
                this.brush.attr("x", x).attr("width", w)
            else
                this.brush.attr("x", this.mouseDownPos.x).attr("width", -w)
        }

        let t = this.xscale.invert(x)
        this.setState({ hover: t })
    }

    mouseout() {
        this.brush.style("opacity", 0);
        this.hover.style("opacity", 0);
        this.setState({ hover: 0 });
        this.mouseMarker.selectAll("circle").style("opacity", 0);
    }

    mouseDown() {
        this.mouseDownPos = {
            x: d3.mouse(this.area.node())[0],
            y: d3.mouse(this.area.node())[1],
            t: this.xscale.invert(d3.mouse(this.area.node())[0])
        };

        if (this.props.allowZoom)
            this.brush
                .attr("x", this.mouseDownPos.x)
                .attr("width", 0)
                .style("opacity", 0.25)
    }

    mouseUp() {
        if (this.props.allowZoom) {
            this.brush.style("opacity", 0);

            let x = d3.mouse(this.area.node())[0];
            let t = this.xscale.invert(x);

            let dT = Math.abs(t - this.mouseDownPos.t);
            if (dT < 10)
                return;

            if (t < this.mouseDownPos.t)
                this.stateSetter({ Tstart: t, Tend: this.mouseDownPos.t });
            else
                this.stateSetter({ Tstart: this.mouseDownPos.t, Tend: t });
        }
    }

    render() {
        return (
            <div>
                <div id={"trendWindow-" + this.props.keyString} style={{ height: this.props.height, float: 'left', width: '100%' }}></div>
                <div id={"legendWindow-" + this.props.keyString} style={{ float: 'left', width: '100%', display: 'flex' }}>
                    {(this.state.points.length == this.props.data.filter(series => series.includeLegend).length ? this.state.points.map(pt => LegendEntry(this.props.data.filter(series => series.includeLegend)[pt.index], pt)) : null)}
                </div>
            </div>);
    }

    stateSetter(obj) {
        this.setState(obj);
    }



    reduceIndex(series: Array<[number, number]>, upper: number, lower: number, t: number): number {
        if (upper == lower)
            return upper;

        if (t >= series[upper][0])
            return upper;
        if (t <= series[lower][0])
            return lower;

        const middle = Math.ceil((upper + lower) / 2.0);
        if (t >= series[middle][0])
            return this.reduceIndex(series, upper, middle, t);
        else
            return this.reduceIndex(series, middle - 1, lower, t);
    }

}

const LegendEntry = (data: ITrendSeries, point: ITrendPoint) => {
    return (
        <div key={data.label} style={{ display: 'flex', alignItems: 'center', marginRight: '20px' }}>
            {(data.lineStyle == '-' ?
                <div style={{ width: ' 10px', height: 0, borderTop: '2px solid', borderRight: '10px solid', borderBottom: '2px solid', borderLeft: '10px solid', borderColor: data.color, overflow: 'hidden', marginRight: '5px' }}></div> :
                <div style={{ width: ' 10px', height: '4px', borderTop: '0px solid', borderRight: '3px solid', borderBottom: '0px solid', borderLeft: '3px solid', borderColor: data.color, overflow: 'hidden', marginRight: '5px' }}></div>)}
            <label style={{ marginTop: '0.5rem' }}> {data.label} ({point.y})</label>
        </div>
    );
}

