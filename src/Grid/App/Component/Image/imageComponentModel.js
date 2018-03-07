function ImageComponent(image) {
    var self = this;

    self.data = ko.observable('');
    self.originalName = ko.observable();
    self.savedName = ko.observable();
    self.index = ko.observable();
    self.width = ko.observable();
    self.height = ko.observable();
    self.file = ko.observable();
    self.propertyBlobType = ko.observable(); 
   
   
};