ko.components.register('singleimage', {
    viewModel: function SingleImageViewModel(params) {

        var self = this;
        self.imageData = params.datas;
        self.entity = params.entity;
        self.isShowurl = ko.observable(params.showUrl);
        self.isFileSelected = ko.observable(false);
        self.isDeleteAll = ko.observable(false);
        self.isUploading = ko.observable(false);
        self.imageId = ko.observable();
        self.canvasId = ko.observable();
        self.fileUploaded = ko.observable(false);
        self.showFileUpload = ko.observable(false);
        self.imageCountType = ko.observable('Multiple');
        self.showUpload = params.showUpload;



        if (params.showUpload != false && params.showUpload != true) {
            self.showUpload = true;
        }

        self.selectAndUpload = function (files) {

            if (files.length > 0) {
                if (files) {
                    $.each(files, function (key, value) {
                        var imgcomp = new ImageComponent();
                        imgcomp.file(value);
                        self.showImage(imgcomp.file);
                    });
                }
            }
        };

        self.showImage = function (file) {
            if (file()) {
                self.isFileSelected(true);
                var name = file().name.replace(/[\. ,:-]+/g, ".").split('.');
                self.canvasId("canvas" + name[0]);
                self.imageId("image" + name[0]);
                readData(file());
            }

        };
        function readData(f) {
            var reader = new FileReader();
            reader.onload = function (e) {
                var i = new Image();
                i.src = e.target.result;
                i.onload = function () {
                    var imgcomp = new ImageComponent();
                    if (self.imageCountType() == 'Single') {
                        self.imageData().data(e.target.result);
                        self.imageData().originalName(f.name);
                        self.imageData().width(i.width);
                        self.imageData().height(i.height);
                    }
                    if (self.imageCountType() == 'Multiple') {
                        self.imageData.data(e.target.result);
                        self.imageData.originalName(f.name);
                        self.imageData.width(i.width);
                        self.imageData.height(i.height);
                    }
                };

            }
            reader.readAsDataURL(f);
        }
        if (params.datas != null && params.datas != '') {
            if (params.datas.file)
            { self.showImage(params.datas.file); }


            else {
                self.showFileUpload(true);
                self.imageCountType = ko.observable('Single');
            }
        }

        self.resizeImage = function (callback) {
            self.isFileSelected(true);
            var MAX_WIDTH = 1000;
            var MAX_HEIGHT = 1000;
            var width;
            var height;
            var ratio = 0;
            if (self.imageCountType() == 'Single') {
                width = self.imageData().width();
                height = self.imageData().height();
            }
            if (self.imageCountType() == 'Multiple') {
                width = self.imageData.width();
                height = self.imageData.height();
            }
            if (width > MAX_WIDTH) {
                ratio = MAX_WIDTH / width;
                height = height * ratio;    // Reset height to match scaled image
                width = width * ratio;    // Reset width to match scaled image
            }

            // Check if current height is larger than max
            if (height > MAX_HEIGHT) {
                ratio = MAX_HEIGHT / height;
                width = width * ratio;    // Reset width to match scaled image
                height = height * ratio;    // Reset height to match scaled image
            }


            var canvas = $('#' + ko.toJS(self.canvasId()))[0];
            var context = canvas.getContext('2d');
            var img = new Image();
            img.onload = function () {
                canvas.height = height;
                canvas.width = width;
                context.drawImage(img, 0, 0, width, height);
                $('#imgCropped').val(canvas.toDataURL());
                callback(true);
            };
            if (self.imageCountType() == 'Single') {
                img.src = self.imageData().data();
            }
            if (self.imageCountType() == 'Multiple') {
                img.src = self.imageData.data();
            }
        };

        self.uploadData = function () {
            self.isUploading(true);
            self.fileUploaded(true);
            self.resizeImage(function (result) {
                if (result) {
                    var dataURL = document.getElementById(self.canvasId()).toDataURL();
                    var blobBin = atob(dataURL.split(',')[1]);
                    var array = [];
                    for (var i = 0; i < blobBin.length; i++) {
                        array.push(blobBin.charCodeAt(i));
                    }
                    var file = new Blob([new Uint8Array(array)], { type: 'image/png' });

                    var formdata = new FormData();
                    formdata.append("Photo", file);
                    if (self.imageCountType() == 'Single') {
                        formdata.append("FileName", self.imageData().originalName());
                    }
                    if (self.imageCountType() == 'Multiple') {
                        formdata.append("FileName", self.imageData.originalName());
                    }
                    var fileUploadResult = rapid.dataManager.postFile(urls.employees.Uploads, formdata);
                    fileUploadResult.done(function (docResponse) {
                        self.isUploading(false);
                        self.fileUploaded(true);
                        if (self.imageCountType() == 'Single') {
                            self.imageData().propertyBlobType(0);
                            self.imageData().data("");
                            self.imageData().savedName(docResponse.Result.SavedName);
                        }
                        if (self.imageCountType() == 'Multiple') {
                            self.imageData.propertyBlobType(0);
                            self.imageData.data("");
                            self.imageData.savedName(docResponse.Result.SavedName);
                        }
                        if (self.entity)
                            amplify.publish(self.entity + "Uploaded");
                    });
                }
            });
        };
        amplify.subscribe("uploadFiles", function () {
            self.fileUploaded(true);
            self.uploadData();
        });
    },
    template: {
        fromUrl: 'Form/FormContent?type=Image'
    }
});
