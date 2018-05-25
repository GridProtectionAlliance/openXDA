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
var PeriodicDataDisplay1_1 = require("./../TS/Services/PeriodicDataDisplay1");
var MeterInput = (function (_super) {
    __extends(MeterInput, _super);
    function MeterInput(props) {
        var _this = _super.call(this, props) || this;
        _this.state = {
            options: []
        };
        _this.periodicDataDisplay1Service = new PeriodicDataDisplay1_1.default();
        return _this;
    }
    MeterInput.prototype.componentDidMount = function () {
        var _this = this;
        this.periodicDataDisplay1Service.getMeters().done(function (data) {
            var options = data.map(function (d) { return React.createElement("option", { value: d.ID }, d.AssetKey); });
            _this.setState({ options: options });
        });
    };
    MeterInput.prototype.render = function () {
        return (React.createElement("div", null, this.state.options));
    };
    return MeterInput;
}(React.Component));
exports.default = MeterInput;
//# sourceMappingURL=MeterInput.js.map