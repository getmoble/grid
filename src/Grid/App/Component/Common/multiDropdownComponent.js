
ko.components.register('multiselectdropdown-list', {
    viewModel: function (params) {
        var self = this;

        self.entity = ko.observable();
        self.caption = ko.observable();
        self.data = ko.observableArray();
        self.labelName = ko.observable();
        //Select value of dropdown
        var selectedValue = params.selectedValues();
        self.selectedIds = params.selectedValues;

        //Setting the entity name
        if (params.entity)
            self.entity(params.entity);

        //labelname of the dropdown
        if (params.labelName)
            self.labelName(params.labelName);

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
                                            list.Id = item.Id;
                                            list.Name = item.Name;
                                            return list;
                                        }).items;

              
               
                self.data.pushAll(dataArray);       
                self.selectedIds.pushAll(selectedValue);
            });
        };
   
        self.fillData();    
    },
    template: { element: 'multiselectdropdown-template' }
});
