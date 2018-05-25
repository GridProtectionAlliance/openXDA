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
var MeterInput_1 = require("./MeterInput");
var PeriodicDataDisplay1 = (function (_super) {
    __extends(PeriodicDataDisplay1, _super);
    function PeriodicDataDisplay1(props) {
        return _super.call(this, props) || this;
    }
    PeriodicDataDisplay1.prototype.render = function () {
        return (React.createElement("div", null,
            React.createElement("div", null,
                React.createElement(MeterInput_1.default, null))));
    };
    return PeriodicDataDisplay1;
}(React.Component));
exports.PeriodicDataDisplay1 = PeriodicDataDisplay1;
ReactDOM.render(React.createElement(PeriodicDataDisplay1, null), document.getElementById('bodyContainer'));
//# sourceMappingURL=PeriodicDataDisplay1.js.map