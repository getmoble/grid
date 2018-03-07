define(["require", "exports", 'knockout', 'jquery'], function (require, exports, ko, $) {

    var self = {};

    self.isBusy = ko.observable();

    self.entity = '';
    self.selected = null;


    self.replaceUrlParam = function (url, paramName, paramValue) {
        var pattern = new RegExp('\\b(' + paramName + '=).*?(&|$)');
        if (url.search(pattern) >= 0) {
            return url.replace(pattern, '$1' + paramValue + '$2');
        }
        return url + (url.indexOf('?') > 0 ? '&' : '?') + paramName + '=' + paramValue;
    }

    self.goBackAndRefresh = function (fallback) {
        var backLocation = document.referrer;
        if (backLocation) {
            if (backLocation.indexOf('random') > -1) {
                self.replaceUrlParam(backLocation, 'random', new Date().getTime());
            } else {
                if (backLocation.indexOf("?") > -1) {
                    backLocation += "&random=" + new Date().getTime();
                } else {
                    backLocation += "?random=" + new Date().getTime();
                }
            }

            window.location.assign(backLocation);
        } else {

            // window.location.href = fallback;
            window.history.back();
        }
    }

    self.submitForm = function (formElement) {
        self.isBusy(true);
        var updateUrl = '/Api/' + self.entity + "/Update";
        var payload = $(formElement).serialize();
        $.post(updateUrl, payload, function (apiResult) {
            self.isBusy(false);
            if (apiResult.Status) {
                bootbox.alert(apiResult.Message, function () {
                    self.goBackAndRefresh();
                });
            }
            else {
                bootbox.alert(apiResult.Message, function () {

                });
            }
        });
    }

    self.init = function (entity, model) {
        self.entity = entity;
        self.selected = model;
    }

    self.load = function (id) {
        self.isBusy(true);
        var apiUrl = '/Api/' + self.entity + "/Get/" + id;

        $.getJSON(apiUrl, function (apiResult) {
            if (apiResult.Status) {
                ko.mapping.fromJS(apiResult.Result, {}, self.selected);
                self.isBusy(false);
            }
            else {
                bootbox.alert(apiResult.Message, function () {

                });
            }
        });
    }

    return self;
});