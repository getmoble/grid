define(["require", "exports", "bootbox"], function (require, exports, bootbox) {
    var self = {};

    self.alert = function (message) {
        bootbox.alert(message, function () {

        });
    }

    self.confirm = function (message, callback) {
        bootbox.confirm(message, function (result) {
            callback(result);
        });
    }

    return self;
});