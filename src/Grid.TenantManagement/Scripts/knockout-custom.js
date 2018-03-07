ko.bindingHandlers.ladda = {
    init: function (element, valueAccessor) {
        var l = Ladda.create(element);

        ko.computed({
            read: function () {
                var state = ko.unwrap(valueAccessor());
                if (state)
                    l.start();
                else
                    l.stop();
            },
            disposeWhenNodeIsRemoved: element
        });
    }
};

var vm = {
    busy: ko.observable(),
    submit: function () {
        vm.busy(true);
        setTimeout(function () {
            vm.busy(false);
        }, 1000);
    }
};