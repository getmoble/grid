﻿ko.components.register('ticketsubcategorieslist', {
    viewModel: function (params) {
        var self = ListViewModelBase(this);
        self.setValues(params);
    },
    template: { fromUrl: 'List?type=ticketsubcategories' }
});