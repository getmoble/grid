define(["require", "exports"], function (require, exports) {
    var self = {};

    self.replaceUrlParam = function (url, paramName, paramValue) {
        var pattern = new RegExp('\\b(' + paramName + '=).*?(&|$)');
        if (url.search(pattern) >= 0) {
            return url.replace(pattern, '$1' + paramValue + '$2');
        }
        return url + (url.indexOf('?') > 0 ? '&' : '?') + paramName + '=' + paramValue;
    }

    self.navigate = function (url) {
        window.location.hash = url;
    }

    self.goBackWithRandom = function (fallback) {
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

    self.goBack = function (fallback) {
        window.history.back();
    }

    return self;
});