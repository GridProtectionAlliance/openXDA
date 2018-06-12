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
var _ = require("lodash");
var stats = require("stats-lite");
var SummaryStat = (function (_super) {
    __extends(SummaryStat, _super);
    function SummaryStat(props) {
        return _super.call(this, props) || this;
    }
    SummaryStat.prototype.render = function () {
        var avg = stats.mean(this.props.data.data.data.map(function (d) { return d[1]; }));
        var median = stats.median(this.props.data.data.data.map(function (d) { return d[1]; }));
        var variance = stats.variance(this.props.data.data.data.map(function (d) { return d[1]; }));
        var stdev = stats.stdev(this.props.data.data.data.map(function (d) { return d[1]; }));
        var sum = stats.sum(this.props.data.data.data.map(function (d) { return d[1]; }));
        var min = _.min(this.props.data.data.data.map(function (d) { return d[1]; }));
        var max = _.max(this.props.data.data.data.map(function (d) { return d[1]; }));
        return (React.createElement("div", null,
            React.createElement("table", { className: "table" },
                React.createElement("thead", null,
                    React.createElement("tr", null,
                        React.createElement("td", null, "Stat"),
                        React.createElement("td", null, "Value"))),
                React.createElement("tbody", null,
                    React.createElement("tr", null,
                        React.createElement("td", null, "Average"),
                        React.createElement("td", null, this.format(avg))),
                    React.createElement("tr", null,
                        React.createElement("td", null, "Median"),
                        React.createElement("td", null, this.format(median))),
                    React.createElement("tr", null,
                        React.createElement("td", null, "Std Dev"),
                        React.createElement("td", null, this.format(stdev))),
                    React.createElement("tr", null,
                        React.createElement("td", null, "Maximum"),
                        React.createElement("td", null, this.format(max))),
                    React.createElement("tr", null,
                        React.createElement("td", null, "Minimum"),
                        React.createElement("td", null, this.format(min))),
                    React.createElement("tr", null,
                        React.createElement("td", null, "Sum"),
                        React.createElement("td", null, this.format(sum)))))));
    };
    SummaryStat.prototype.format = function (val) {
        if (val > 1000000 || val < -1000000)
            return ((val / 1000000) | 0) + "M";
        else if (val > 1000 || val < -1000)
            return ((val / 1000) | 0) + "K";
        else if (val < 0.01)
            return val.toFixed(4);
        else
            return val.toFixed(2);
    };
    return SummaryStat;
}(React.Component));
exports.default = SummaryStat;
//# sourceMappingURL=SummaryStat.js.map