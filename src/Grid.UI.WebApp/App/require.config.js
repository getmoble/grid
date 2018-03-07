// require.js looks for the following global when initializing
var require = {
    baseUrl: ".",
    paths: {
        "jquery": "static/js/jquery-3.1.0.min",
        "bootstrap": "static/js/bootstrap.min",
        "signals": "static/js/signals.min",
        "crossroads": "static/js/crossroads.min",
        "hasher": "static/js/hasher.min",
        "moment": "static/js/moment.min",
        "summernote": "static/js/summernote.min",
        "bootbox": "static/js/bootbox.min",
        "boostrapDatePicker": "static/js/bootstrap-datetimepicker.min",
        "knockout": "static/js/knockout-3.4.0",
        "komapping": "static/js/knockout.mapping-latest",
        "knockoutCustom": "static/js/knockout-custom",
        "text": "static/js/text",
        "pagedViewModelBase": "App/PagedViewModelBase",
        "createEditViewModelBase": "App/CreateEditViewModelBase",
        "navigationService": "App/navigationService",
        "dataManager": "App/dataManager",
        "interactionService": "App/interactionService"
    },
    shim: {
        'bootstrap': {
            deps: ['jquery']
        },
        'boostrapDatePicker': {
            deps: ['moment', 'jquery', 'bootstrap']
        },
        'bootbox': {
            deps: ['bootstrap']
        }
    }
};