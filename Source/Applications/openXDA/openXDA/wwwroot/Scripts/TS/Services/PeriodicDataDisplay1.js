"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var PeriodicDataDisplay1Service = (function () {
    function PeriodicDataDisplay1Service() {
    }
    PeriodicDataDisplay1Service.prototype.getData = function (filters) {
        return $.ajax({
            type: "GET",
            url: window.location.origin + "/api/PeriodicDataDisplay/GetData?eventId=" + filters.eventId +
                ("" + (filters.startDate != undefined ? "&startDate=" + filters.startDate : "")) +
                ("" + (filters.endDate != undefined ? "&endDate=" + filters.endDate : "")) +
                ("&pixels=" + filters.pixels) +
                ("&type=" + filters.type),
            contentType: "application/json; charset=utf-8",
            dataType: 'json',
            cache: true,
            async: true
        });
    };
    PeriodicDataDisplay1Service.prototype.getMeters = function () {
        return $.ajax({
            type: "GET",
            url: window.location.origin + "/api/PeriodicDataDisplay/GetMeters",
            contentType: "application/json; charset=utf-8",
            dataType: 'json',
            cache: true,
            async: true
        });
    };
    return PeriodicDataDisplay1Service;
}());
exports.default = PeriodicDataDisplay1Service;
//# sourceMappingURL=PeriodicDataDisplay1.js.map