function ViewModelBase(self) {
    self.parseDate = function (date) {
        if (date) {
            var myDate = new Date(parseFloat(/Date\(([^)]+)\)/.exec(date)[1]));
            return myDate;
        } else {
            return '';
        }
    };

    self.replaceUrlParam = function(url, paramName, paramValue) {
        var pattern = new RegExp('\\b(' + paramName + '=).*?(&|$)');
        if (url.search(pattern) >= 0) {
            return url.replace(pattern, '$1' + paramValue + '$2');
        }
        return url + (url.indexOf('?') > 0 ? '&' : '?') + paramName + '=' + paramValue;
    }

    self.goBackAndRefresh = function (fallback) {
        var backLocation = document.referrer;
        if (backLocation) {
            if (backLocation.indexOf('randomParam') > -1) {
                self.replaceUrlParam(backLocation, 'randomParam', new Date().getTime());
            } else {
                if (backLocation.indexOf("?") > -1) {
                    backLocation += "&randomParam=" + new Date().getTime();
                } else {
                    backLocation += "?randomParam=" + new Date().getTime();
                }
            }

            window.location.assign(backLocation);
        } else {
            window.location.href = fallback;
        }
    }

    self.isBusy = ko.observable(false);

    return self;
}