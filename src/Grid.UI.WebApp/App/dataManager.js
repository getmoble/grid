define(["require", "exports", "jquery"], function (require, exports, $) {
    var self = {};

    // Put somewhere in your scripting environment
    if (jQuery.when.all === undefined) {
        jQuery.when.all = function (deferreds) {
            var deferred = new jQuery.Deferred();
            $.when.apply(jQuery, deferreds).then(
                function () {
                    // Extact result from the promise and push further
                    var result = [];
                    $.each(arguments, function(k, v) {
                        result.push(v[0]);
                    });

                    deferred.resolve(Array.prototype.slice.call(result));
                },
                function () {
                    deferred.fail(Array.prototype.slice.call(arguments));
                });

            return deferred;
        }
    }

    self.getData = function (apiUrl, callback) {
        $.getJSON(apiUrl, function (apiResult) {
            callback(apiResult);
        });
    }

    self.getAllData = function (apiUrls, callback) {
        var promises = [];
        $.each(apiUrls, function(k, v) {
            promises.push($.getJSON(v));
        });

        $.when.all(promises).then(function (resultArray) {
            callback(resultArray);
        }, function (e) {
            alert("My ajax failed");
        });
    }

    self.postData = function (apiUrl, data, callback) {
        $.post(apiUrl, data, function (apiResult) {
            callback(apiResult);
        });
    }

    return self;
});