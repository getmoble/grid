ko.components.register('dropdown-list', {
    viewModel: function (params) {
        var self = this;

        self.entity = ko.observable();
        self.labelName = ko.observable();
        self.caption = ko.observable();
        self.data = ko.observableArray();
        //Select value of dropdown
        var selectedValue = params.selectedValue();
        self.selectedId = params.selectedValue;

        //Setting the entity name
        if (params.entity)
            self.entity(params.entity);

        //labelname of the dropdown
        if (params.text)
            self.labelName(params.text);

        //Caption of the dropdown
        if (params.caption)
            self.caption(params.caption);

        //fill the dropdown
        self.fillData = function () {
            self.data.removeAll();
                var result = rapid.dataManager.getData(urls.generic.selectList + self.entity());
                result.done(function (value) {
                    var dataArray = JSLINQ(value.Result)
                                            .Select(function (item) {
                                                var list = new SelectItem();
                                                list.id = item.Id;
                                                list.name = item.Name;
                                                return list;
                                            }).items;
                    self.data.pushAll(dataArray);
                    self.selectedId(selectedValue);
                });            
        };
        self.fillData();
    },
    template: { element: 'dropdown-template' }
});
