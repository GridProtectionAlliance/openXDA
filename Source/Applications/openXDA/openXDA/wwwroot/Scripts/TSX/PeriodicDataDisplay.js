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
var ReactDOM = require("react-dom");
var PeriodicDataDisplay_1 = require("./../TS/Services/PeriodicDataDisplay");
var createBrowserHistory_1 = require("history/createBrowserHistory");
var queryString = require("query-string");
var moment = require("moment");
var _ = require("lodash");
var MeterInput_1 = require("./MeterInput");
var react_datetime_range_picker_1 = require("react-datetime-range-picker");
var Measurement_1 = require("./Measurement");
var PeriodicDataDisplay = (function (_super) {
    __extends(PeriodicDataDisplay, _super);
    function PeriodicDataDisplay(props) {
        var _this = _super.call(this, props) || this;
        _this.history = createBrowserHistory_1.default();
        _this.periodicDataDisplayService = new PeriodicDataDisplay_1.default();
        var query = queryString.parse(_this.history['location'].search);
        _this.state = {
            meterID: (query['meterID'] != undefined ? query['meterID'] : 0),
            startDate: (query['startDate'] != undefined ? query['startDate'] : moment().subtract(7, 'day').format('YYYY-MM-DDTHH:mm:ss')),
            endDate: (query['endDate'] != undefined ? query['endDate'] : moment().format('YYYY-MM-DDTHH:mm:ss')),
            type: (query['type'] != undefined ? query['type'] : "Average"),
            detailedReport: (query['detailedReport'] != undefined ? query['detailedReport'] == "true" : true),
            print: (query['print'] != undefined ? query['print'] == "true" : false),
            measurements: [],
            width: window.innerWidth - 475,
            data: null
        };
        return _this;
    }
    PeriodicDataDisplay.prototype.getData = function () {
        var _this = this;
        this.periodicDataDisplayService.getMeasurementCharacteristics().done(function (data) {
            _this.setState({ data: data });
            _this.createMeasurements(data);
        });
    };
    PeriodicDataDisplay.prototype.createMeasurements = function (data) {
        var _this = this;
        if (data.length == 0)
            return;
        var list = data.map(function (d) {
            return React.createElement(Measurement_1.default, { meterID: _this.state.meterID, startDate: _this.state.startDate, endDate: _this.state.endDate, key: d.ID, data: d, type: _this.state.type, height: 300, stateSetter: function (obj) { return _this.setState(obj, _this.updateUrl); }, detailedReport: _this.state.detailedReport, width: _this.state.width });
        });
        this.setState({ measurements: list }, function () { return _this.updateUrl(); });
    };
    PeriodicDataDisplay.prototype.componentDidMount = function () {
        window.addEventListener("resize", this.handleScreenSizeChange.bind(this));
        if (this.state.meterID != 0)
            this.getData();
    };
    PeriodicDataDisplay.prototype.componentWillUnmount = function () {
        $(window).off('resize');
    };
    PeriodicDataDisplay.prototype.handleScreenSizeChange = function () {
        var _this = this;
        clearTimeout(this.resizeId);
        this.resizeId = setTimeout(function () {
            _this.setState({ width: window.innerWidth - 475 }, function () { _this.createMeasurements(_this.state.data); });
        }, 100);
    };
    PeriodicDataDisplay.prototype.updateUrl = function () {
        var state = _.clone(this.state);
        delete state.measurements;
        delete state.data;
        this.history['push']('PeriodicDataDisplay.cshtml?' + queryString.stringify(state, { encode: false }));
    };
    PeriodicDataDisplay.prototype.render = function () {
        var _this = this;
        var height = window.innerHeight - (this.state.print ? 0 : 60);
        return (React.createElement("div", null,
            React.createElement("div", { className: "screen", style: { height: height, width: window.innerWidth } },
                (!this.state.print ?
                    React.createElement("div", { className: "vertical-menu" },
                        React.createElement("div", { className: "form-group" },
                            React.createElement("label", null, "Meter: "),
                            React.createElement(MeterInput_1.default, { value: this.state.meterID, onChange: function (obj) { return _this.setState({ meterID: obj }); } })),
                        React.createElement("div", { className: "form-group" },
                            React.createElement("label", null, "Time Range: "),
                            React.createElement(react_datetime_range_picker_1.default, { startDate: new Date(this.state.startDate), endDate: new Date(this.state.endDate), onChange: function (obj) { _this.setState({ startDate: moment(obj.start).format('YYYY-MM-DDTHH:mm:ss'), endDate: moment(obj.end).format('YYYY-MM-DDTHH:mm:ss') }); }, inputProps: { style: { width: '100px', margin: '5px' }, className: 'form-control' }, className: 'form', timeFormat: false })),
                        React.createElement("div", { className: "form-group" },
                            React.createElement("label", null, "Data Type: "),
                            React.createElement("select", { onChange: function (obj) { return _this.setState({ type: obj.target.value }); }, className: "form-control", defaultValue: this.state.type },
                                React.createElement("option", { value: "Average" }, "Average"),
                                React.createElement("option", { value: "Maximum" }, "Maximum"),
                                React.createElement("option", { value: "Minimum" }, "Minimum"))),
                        React.createElement("div", { className: "form-group" },
                            React.createElement("label", null,
                                "Detailed Report: ",
                                React.createElement("input", { type: "checkbox", value: this.state.detailedReport, defaultChecked: this.state.detailedReport, onChange: function (e) { _this.setState({ detailedReport: e.target.value }); } }))),
                        React.createElement("div", { className: "form-group" },
                            React.createElement("button", { className: 'btn btn-primary', style: { float: 'right' }, onClick: function () { _this.getData(); } }, "Apply")))
                    : null),
                React.createElement("div", { className: "waveform-viewer", style: { width: window.innerWidth } },
                    React.createElement("div", { className: "list-group", style: { maxHeight: height, overflowY: (this.state.print ? 'visible' : 'auto') } }, this.state.measurements)))));
    };
    return PeriodicDataDisplay;
}(React.Component));
exports.PeriodicDataDisplay = PeriodicDataDisplay;
ReactDOM.render(React.createElement(PeriodicDataDisplay, null), document.getElementById('bodyContainer'));
//# sourceMappingURL=PeriodicDataDisplay.js.map