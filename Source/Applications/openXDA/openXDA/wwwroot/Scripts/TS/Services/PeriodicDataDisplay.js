"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var moment = require("moment");
var PeriodicDataDisplayService = (function () {
    function PeriodicDataDisplayService() {
    }
    PeriodicDataDisplayService.prototype.getData = function (meterID, startDate, endDate, pixels, measurementCharacteristicID, type) {
        return $.ajax({
            type: "GET",
            url: window.location.origin + "/api/PeriodicDataDisplay/GetData?MeterID=" + meterID +
                ("&startDate=" + moment(startDate).format('YYYY-MM-DDTHH:mm:ss')) +
                ("&endDate=" + moment(endDate).format('YYYY-MM-DDTHH:mm:ss')) +
                ("&pixels=" + pixels) +
                ("&MeasurementCharacteristicID=" + measurementCharacteristicID) +
                ("&type=" + type),
            contentType: "application/json; charset=utf-8",
            dataType: 'json',
            cache: true,
            async: true
        });
    };
    PeriodicDataDisplayService.prototype.getMeters = function () {
        return $.ajax({
            type: "GET",
            url: window.location.origin + "/api/PeriodicDataDisplay/GetMeters",
            contentType: "application/json; charset=utf-8",
            dataType: 'json',
            cache: true,
            async: true
        });
    };
    PeriodicDataDisplayService.prototype.getMeasurementCharacteristics = function () {
        return $.ajax({
            type: "GET",
            url: window.location.origin + "/api/PeriodicDataDisplay/GetMeasurementCharacteristics",
            contentType: "application/json; charset=utf-8",
            dataType: 'json',
            cache: true,
            async: true
        });
    };
    return PeriodicDataDisplayService;
}());
exports.default = PeriodicDataDisplayService;
//# sourceMappingURL=PeriodicDataDisplay.js.map