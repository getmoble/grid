define(["require", "exports", "jquery", 'knockout', 'signals', 'crossroads', 'hasher', './routes'], function (require, exports, $, ko, signals, crossroads, hasher, routes) {

    // This module configures crossroads.js, a routing library. If you prefer, you
    // can use any other routing library (or none at all) as Knockout is designed to
    // compose cleanly with external libraries.
    //
    // You *don't* have to follow the pattern established here (each route entry
    // specifies a 'page', which is a Knockout component) - there's nothing built into
    // Knockout that requires or even knows about this technique. It's just one of
    // many possible ways of setting up client-side routes.
    var Router = (function () {
        function Router(config) {
            var _this = this;
            this.currentRoute = ko.observable({});
            // Configure Crossroads route handlers
            $.each(config.routes, function (k, v) {
                crossroads.addRoute(v.url, function (requestParams) {
                    _this.currentRoute($.extend(requestParams, v.params));
                });
            });
            // Activate Crossroads
            crossroads.normalizeFn = crossroads.NORM_AS_OBJECT;
            hasher.changed.add(function (hash) { return crossroads.parse(hash); }); //add hash change listener
            hasher.initialized.add(function (hash) { return crossroads.parse(hash); }); //add initialized listener (to grab initial value in case it is already set)
            hasher.init(); //initialize hasher (start
        }
        return Router;
    }());

    // Create and export router instance
    var routerInstance = new Router(routes);

    return routerInstance;
});
