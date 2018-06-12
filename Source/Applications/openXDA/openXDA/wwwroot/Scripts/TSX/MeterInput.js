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
var PeriodicDataDisplay_1 = require("./../TS/Services/PeriodicDataDisplay");
var MeterInput = (function (_super) {
    __extends(MeterInput, _super);
    function MeterInput(props) {
        var _this = _super.call(this, props) || this;
        _this.state = {
            select: null
        };
        _this.periodicDataDisplayService = new PeriodicDataDisplay_1.default();
        return _this;
    }
    MeterInput.prototype.componentDidMount = function () {
        var _this = this;
        this.periodicDataDisplayService.getMeters().done(function (data) {
            if (data.length == 0)
                return;
            var value = (_this.props.value ? _this.props.value : data[0].ID);
            var options = data.map(function (d) { return React.createElement("option", { key: d.ID, value: d.ID }, d.AssetKey); });
            var select = React.createElement("select", { className: 'form-control', onChange: function (e) { _this.props.onChange(e.target.value); }, defaultValue: value }, options);
            _this.props.onChange(value);
            _this.setState({ select: select });
        });
    };
    MeterInput.prototype.render = function () {
        return this.state.select;
    };
    return MeterInput;
}(React.Component));
exports.default = MeterInput;
//# sourceMappingURL=MeterInput.js.map