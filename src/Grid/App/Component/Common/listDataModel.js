function ListData(data) {
    var self = ModelBase(this);

    self.id = data.Id;
    self.name = data.Title || 'No Name';
    self.description = data.Description || 'No Description';
    self.createdOn = data.CreatedOn || '';   
    self.isSelected = ko.observable(false);
};
