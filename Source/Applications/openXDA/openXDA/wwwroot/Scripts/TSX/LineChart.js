"use strict";
var __extends = (this && this.__extends) || (function () {
    var extendStatics = Object.setPrototypeOf ||
        ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
        function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
Object.defineProperty(exports, "__esModule", { value: true });
var React = require("react");
var moment = require("moment");
require("./../flot/jquery.flot.min.js");
require("./../flot/jquery.flot.time.min.js");
var LineChart = (function (_super) {
    __extends(LineChart, _super);
    function LineChart(props) {
        var _this = _super.call(this, props) || this;
        var ctrl = _this;
        _this.options = {
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
                    var ticks = [], start = ctrl.floorInBase(axis.min, axis.delta), i = 0, v = Number.NaN, prev;
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
        };
        return _this;
    }
    LineChart.prototype.componentDidMount = function () {
        this.createDataRows();
    };
    LineChart.prototype.componentWillReceiveProps = function (nextProps) {
        this.createDataRows();
    };
    LineChart.prototype.componentWillUnmount = function () {
        $(this.refs.graph).off("plotselected");
        $(this.refs.graph).off("plotzoom");
        $(this.refs.graph).off("plothover");
    };
    LineChart.prototype.createDataRows = function () {
        var _this = this;
        if (this.plot != undefined)
            $(this.refs.graph).children().remove();
        var ctrl = this;
        var startString = this.props.startDate;
        var endString = this.props.endDate;
        var newVessel = [];
        if (this.props.legend != undefined && Object.keys(this.props.legend).length > 0) {
            $.each(Object.keys(this.props.legend), function (i, key) {
                if (_this.props.legend[key].enabled)
                    newVessel.push({ label: key, data: _this.props.legend[key].data, color: _this.props.legend[key].color });
            });
        }
        newVessel.push([[this.getMillisecondTime(startString), null], [this.getMillisecondTime(endString), null]]);
        this.plot = $.plot($(this.refs.graph), newVessel, this.options);
    };
    LineChart.prototype.render = function () {
        return React.createElement("div", { ref: 'graph', style: { height: 'inherit', width: this.props.width } });
    };
    LineChart.prototype.defaultTickFormatter = function (value, axis) {
        var factor = axis.tickDecimals ? Math.pow(10, axis.tickDecimals) : 1;
        var formatted = "" + Math.round(value * factor) / factor;
        if (axis.tickDecimals != null) {
            var decimal = formatted.indexOf(".");
            var precision = decimal == -1 ? 0 : formatted.length - decimal - 1;
            if (precision < axis.tickDecimals) {
                return (precision ? formatted : formatted + ".") + ("" + factor).substr(1, axis.tickDecimals - precision);
            }
        }
        return formatted;
    };
    ;
    LineChart.prototype.floorInBase = function (n, base) {
        return base * Math.floor(n / base);
    };
    LineChart.prototype.getMillisecondTime = function (date) {
        var milliseconds = moment.utc(date).valueOf();
        return milliseconds;
    };
    LineChart.prototype.getDateString = function (float) {
        var date = moment.utc(float).format('YYYY-MM-DDTHH:mm:ss');
        return date;
    };
    LineChart.prototype.plotSelected = function () {
        var ctrl = this;
        $(this.refs.graph).off("plotselected");
        $(this.refs.graph).bind("plotselected", function (event, ranges) {
            ctrl.props.stateSetter({ startDate: ctrl.getDateString(ranges.xaxis.from), endDate: ctrl.getDateString(ranges.xaxis.to) });
        });
    };
    LineChart.prototype.plotZoom = function () {
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
            if (event.originalEvent.wheelDelta != undefined)
                delta = event.originalEvent.wheelDelta;
            else
                delta = -event.originalEvent.detail;
            deltaMagnitude = Math.abs(delta);
            if (minDelta == null || deltaMagnitude < minDelta)
                minDelta = deltaMagnitude;
            deltaMagnitude /= minDelta;
            deltaMagnitude = Math.min(deltaMagnitude, maxDelta);
            factor = deltaMagnitude / 10;
            if (delta > 0) {
                xmin = xmin * (1 - factor) + xcenter * factor;
                xmax = xmax * (1 - factor) + xcenter * factor;
            }
            else {
                xmin = (xmin - xcenter * factor) / (1 - factor);
                xmax = (xmax - xcenter * factor) / (1 - factor);
            }
            if (xmin == xaxis.options.xmin && xmax == xaxis.options.xmax)
                return;
            ctrl.props.stateSetter({ startDate: ctrl.getDateString(xmin), endDate: ctrl.getDateString(xmax) });
        });
    };
    LineChart.prototype.plotHover = function () {
        var ctrl = this;
        $(this.refs.graph).off("plothover");
        $(this.refs.graph).bind("plothover", function (event, pos, item) {
            ctrl.setState({ hover: pos.x });
        });
    };
    return LineChart;
}(React.Component));
exports.default = LineChart;
//# sourceMappingURL=LineChart.js.map