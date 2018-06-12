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
var PeriodicDataDisplay_1 = require("./../TS/Services/PeriodicDataDisplay");
var Legend_1 = require("./Legend");
var DistributionPlot_1 = require("./DistributionPlot");
var SummaryStat_1 = require("./SummaryStat");
var LineChart_1 = require("./LineChart");
var Measurement = (function (_super) {
    __extends(Measurement, _super);
    function Measurement(props) {
        var _this = _super.call(this, props) || this;
        _this.state = {
            legend: {},
            hover: null,
            display: 'block',
            distributions: []
        };
        _this.periodicDataDisplayService = new PeriodicDataDisplay_1.default();
        return _this;
    }
    Measurement.prototype.getColor = function (index) {
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
    };
    Measurement.prototype.componentWillReceiveProps = function (nextProps) {
        var props = _.clone(this.props);
        var nextPropsClone = _.clone(nextProps);
        delete props.data;
        delete props.stateSetter;
        delete props.height;
        delete nextPropsClone.data;
        delete nextPropsClone.stateSetter;
        delete nextPropsClone.height;
        if (!(_.isEqual(props, nextPropsClone))) {
            this.getData(nextProps);
        }
    };
    Measurement.prototype.componentDidMount = function () {
        this.getData(this.props);
    };
    Measurement.prototype.getData = function (props) {
        var _this = this;
        this.periodicDataDisplayService.getData(props.meterID, props.startDate, props.endDate, window.innerWidth, props.data.ID, this.props.type).done(function (data) {
            if (Object.keys(data).length == 0) {
                _this.setState({ display: 'none' });
                return;
            }
            _this.setState({ display: 'block' });
            var legend = _this.createLegendRows(data);
            _this.createDistributionPlots(legend);
        });
    };
    Measurement.prototype.createDistributionPlots = function (legend) {
        var distributions = Object.keys(legend).map(function (key) {
            var data = { key: key, data: legend[key] };
            return (React.createElement("li", { key: key, style: { height: '270px', width: '600px', display: 'inline-block' } },
                React.createElement("table", { className: "table", style: { height: '270px', width: '600px' } },
                    React.createElement("tbody", null,
                        React.createElement("tr", { style: { height: '270px', width: '600px' } },
                            React.createElement("td", { style: { height: 'inherit', width: '50%' } },
                                React.createElement(DistributionPlot_1.default, { data: data, bins: 40 })),
                            React.createElement("td", { style: { height: 'inherit', width: '50%' } },
                                React.createElement(SummaryStat_1.default, { data: data })))))));
        });
        this.setState({ distributions: distributions });
    };
    Measurement.prototype.createLegendRows = function (data) {
        var ctrl = this;
        var legend = (this.state.legend != undefined ? this.state.legend : {});
        $.each(Object.keys(data), function (i, key) {
            if (legend[key] == undefined)
                legend[key] = { color: ctrl.getColor(i), enabled: true, data: data[key] };
            else
                legend[key].data = data[key];
        });
        this.setState({ legendRows: legend });
        return legend;
    };
    Measurement.prototype.render = function () {
        var _this = this;
        return (React.createElement("div", { id: this.props.data.ID, className: "list-group-item panel-default", style: { padding: 0, display: this.state.display } },
            React.createElement("div", { className: "panel-heading" }, this.props.data.Name),
            React.createElement("div", { className: "panel-body" },
                React.createElement("div", { style: { height: this.props.height, float: 'left', width: '100%', marginBottom: '10px' } },
                    React.createElement("div", { id: 'graph', style: { height: 'inherit', width: this.props.width, position: 'absolute' } },
                        React.createElement(LineChart_1.default, { legend: this.state.legendRows, startDate: this.props.startDate, endDate: this.props.endDate, width: this.props.width })),
                    React.createElement("div", { id: 'legend', className: 'legend', style: { position: 'absolute', right: '0', width: '200px', height: this.props.height - 38, marginTop: '6px', borderStyle: 'solid', borderWidth: '2px', overflowY: 'auto' } },
                        React.createElement(Legend_1.default, { data: this.state.legendRows, callback: function () {
                                _this.setState({ legend: _this.state.legend });
                            } }))),
                (this.props.detailedReport ?
                    React.createElement("div", { style: { width: '100%', margin: '10px' } },
                        React.createElement("label", { style: { width: '100%' } }, "Distributions"),
                        React.createElement("ul", { style: { display: 'inline', position: 'relative', width: '100%', marginTop: '25px', paddingLeft: '0' } }, this.state.distributions)) : null)),
            React.createElement("br", null)));
    };
    return Measurement;
}(React.Component));
exports.default = Measurement;
//# sourceMappingURL=Measurement.js.map