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
require("./../flot/jquery.flot.min.js");
var stats = require("stats-lite");
var DistributionPlot = (function (_super) {
    __extends(DistributionPlot, _super);
    function DistributionPlot(props) {
        return _super.call(this, props) || this;
    }
    DistributionPlot.prototype.componentDidMount = function () {
        var stat = stats.histogram(this.props.data.data.data.map(function (d) { return d[1]; }), this.props.bins);
        var totalCount = stat.values.reduce(function (t, x) { return t + x; });
        var options = {
            series: {
                bars: {
                    show: true,
                    fill: true,
                    lineWidth: 0,
                    fillColor: '#517ec6',
                    barWidth: stat.binWidth
                }
            },
            canvas: true,
            legend: { show: false },
            grid: {
                show: true,
                autoHighlight: false,
                borderWidth: { top: 0, right: 0, bottom: 1, left: 0 },
                labelMargin: 0,
                axisMargin: 0,
                minBorderMargin: 0
            },
            yaxis: { tickLength: 0 },
            xaxis: {
                tickLength: 0,
                reserveSpace: false,
                ticks: 4,
                min: stat.binLimits[0],
                max: stat.binLimits[1],
                tickFormatter: function (val, axis) {
                    if (axis.delta > 1000000 && (val > 1000000 || val < -1000000))
                        return ((val / 1000000) | 0) + "M";
                    else if (axis.delta > 1000 && (val > 1000 || val < -1000))
                        return ((val / 1000) | 0) + "K";
                    else
                        return val.toFixed(axis.tickDecimals);
                }
            },
        };
        var data = stat.values.map(function (d, i) {
            var bin = stat.binLimits[0] + i * stat.binWidth;
            return [bin, (d / totalCount) * 100];
        });
        $.plot($(this.refs.chartHolder), [{ label: this.props.data.key, data: data }], options);
    };
    DistributionPlot.prototype.render = function () {
        return (React.createElement("div", { style: { height: 'inherit', margin: '10px' } },
            React.createElement("label", { style: { textAlign: 'center' } }, this.props.data.key),
            React.createElement("div", { ref: "chartHolder", style: { height: 'calc(100% - 20px)', width: 'inherit' } })));
    };
    return DistributionPlot;
}(React.Component));
exports.default = DistributionPlot;
//# sourceMappingURL=DistributionPlot.js.map