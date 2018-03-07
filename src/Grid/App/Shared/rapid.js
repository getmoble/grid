//PushAll function
ko.observableArray.fn.pushAll = function (valuesToPush) {
    var underlyingArray = this();
    this.valueWillMutate();
    ko.utils.arrayPushAll(underlyingArray, valuesToPush);
    this.valueHasMutated();
    return this;  //optional
};

//time ago
ko.bindingHandlers.timeago = {
    init: function (element, valueAccessor, allBindings, viewModel, bindingContext) {
        var $this = $(element);
        var value = ko.utils.unwrapObservable(valueAccessor());
        $this.livestamp(value);
    },
    update: function (element, valueAccessor, allBindings, viewModel, bindingContext) {

    }
};

  
//button busy
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

//HtmlEditor
ko.bindingHandlers.htmlEditor = {
    init: function (element, valueAccessor, allBindingsAccessor) {
        var options = valueAccessor();
        var binding = ko.utils.unwrapObservable(allBindingsAccessor()).value;

        var updateObservable = function (e) {
            binding(e.currentTarget.innerHTML);
        };

        options.onkeydown = options.onkeyup = options.onfocus = options.onblur = updateObservable;

        $(element).summernote(options);
    },
    update: function (element, valueAccessor, allBindingsAccessor, viewModel, bindingContext) {
        var value = ko.unwrap(valueAccessor());
        $(element).summernote('code', value);
    }
};

function ModelBase(child) {
    var self = child;
    self.parseDate = function (date) {
        if (date) {
            var myDate = new Date(parseFloat(/Date\(([^)]+)\)/.exec(date)[1]));
            return myDate;
        } else {
            return '';
        }
    };
    return self;
};

function ListViewModelBase(vm) {
    var self = vm;

    self.data = ko.observableArray();
    self.isBusy = ko.observable(false);
    self.filter = ko.observable('');
    self.setValues = function (params) {

        self.entity = params.entity;
        self.showCreate = params.showCreate;
        self.manageList = params.manageList;
        self.enableFilter = params.enableFilter;

        amplify.subscribe(self.entity + "Created", function () {
            self.init();
        });

       
        amplify.subscribe(self.entity + "Updated", function () {
            self.init();
        });

        amplify.subscribe(self.entity + "Search", function (data) {
            self.init(data);
        });
        amplify.subscribe(self.entity + "Refresh", function () {
            self.init();
        });

        amplify.subscribe(self.entity + "Deleted", function () {
            self.init();
        });
        amplify.subscribe(self.entity + "Delete", function (data) {
            self.SubscribeDelete(data);
        });
        self.init();
    };
    self.init = function (args) {
        self.fillData(args);
     
    };

    
    //self.filldata is to fill the list with the enity 
    self.fillData = function (args) {
        var url = "?";
        self.isBusy(true);
        if (args != null && args != 'undefined') {
            $.each(args, function (k, v) {
                url += v.key + "=" + v.value + "&"
            });
        }
        var searchUrl = urls.generic.list + self.entity + url;
        var result = rapid.dataManager.getData(searchUrl);
        result.done(function (value) {
            rapid.errorManager.checkDataStatus(value, function () {
                self.data.removeAll();
                if (value.Result != null && value.Result != undefined) {
                    self.data.pushAll(value.Result);
                    self.isBusy(false);
                }
                
            });
            self.isBusy(false);
        });
    };

    //self.hasData is to find list has value or not.
    self.hasData = ko.computed(function () {
        if (self.data().length > 0) {
            return true;
        }
        else {
            return false;
        }
    });


    // stringStartSearchWith is used to find the character in the filter search in the 
    var stringStartSearchWith = function (string, startsWith) {
        if (string != undefined) {
            return (string.toLowerCase().indexOf(startsWith) !== -1);
        }
        else
            return false;
    };

    //self.create is the publish create event for the entity.
    self.create = function () {
        rapid.eventManager.publish(self.entity + "Create");
    };

    //self.edit is the publish edit event for the entity.
    self.edit = function (item) {
        $('html, body').animate({ scrollTop: 0 }, 800);
        var jsonData = { entityType: self.entity, entityId: item.Id };
        rapid.eventManager.publish(self.entity + "Update", jsonData);
    };

    //self.details is the publish details event for the entity.
    self.details = function (item) {
        $('html, body').animate({ scrollTop: 0 }, 800);
        var jsonData = { entityType: self.entity, entityId: item.Id };
        rapid.eventManager.publish(self.entity + "Details", jsonData);
    };

    self.employeeDetails = function (item) {
        var jsonData = { entityType: self.entity, entityId: item.Id };
        rapid.eventManager.publish(self.entity + "EmployeeDetails", jsonData);
    };


    self.resetPassword = function (item) {
        var jsonData = { entityType: self.entity, entityId: item.Id };
        rapid.eventManager.publish(self.entity + "ResetPassword", jsonData);
    };

    self.addMember = function (item) {
        $('html, body').animate({ scrollTop: 0 }, 800);
        var jsonData = { entityType: self.entity, entityId: item.Id };
        rapid.eventManager.publish(self.entity + "AddMember", jsonData);
    };


    self.delete = function (item) {
        rapid.modalManager.confirm(rapid.messageManager.deleteWarningMessage('this'), function (result) {
            if (result) {
                var jsonData = { entityType: self.entity, entityId: item.Id };
                rapid.eventManager.publish(self.entity + "Delete", jsonData);
            }

        });
    };
    self.SubscribeDelete = function (data) {
        self.isBusy(true);
        var deleteResult = rapid.dataManager.postData(urls.generic.delete + data.entityType + "s/" + data.entityId);
        deleteResult.done(function (apiResult) {
            if (apiResult.Status) {
                self.isBusy(false);
                rapid.notificationManager.showSuccess(rapid.messageManager.deleteMessage(data.entityType));
                rapid.eventManager.publish(self.entity + "Deleted");
                rapid.eventManager.publish(self.entity + "Cancel");
            } else {
                self.isBusy(false);
                //rapid.modalManager.alert(apiResult.Message);
                bootbox.alert("Entity can't be delete!..");
                rapid.eventManager.publish(self.entity + "Cancel");
            }
        });
    };


    return self;
};

function PagedListViewModelBase(vm) {
    var self = vm;

    self.data = ko.observableArray();
    self.isBusy = ko.observable(false);
    self.visibleLoadMore = ko.observable(false);
    self.filter = ko.observable('');
    self.totalCount = ko.observable();
    self.page = ko.observable(0);
    self.count = ko.observable(4);
    self.totalPages = ko.observable();


    self.setValues = function (params) {

        self.entity = params.entity;
        self.showCreate = params.showCreate;
        self.manageList = params.manageList;
        self.enableFilter = params.enableFilter;


        amplify.subscribe(self.entity + "Created", function () {
            self.init();
        });

        amplify.subscribe(self.entity + "Updated", function () {
            self.init();
        });

        amplify.subscribe(self.entity + "Search", function (data) {
            self.init(data);
        });
        amplify.subscribe(self.entity + "Refresh", function () {
            self.init();
        });

        amplify.subscribe(self.entity + "Deleted", function () {
            self.init();
        });
        amplify.subscribe(self.entity + "Delete", function (data) {
            self.SubscribeDelete(data);
        });
        self.init();
    };
    self.init = function (args) {
        self.fillData(args);
    };
    //self.filldata is to fill the list with the enity 
    self.fillData = function (args) {       
        var url = "?page=" + self.page() + "&count=" + self.count() + "&";
        self.isBusy(true);
        if (args != null && args != 'undefined') {
            $.each(args, function (k, v) {
                url += v.key + "=" + v.value + "&"
            });
        }
        var searchUrl = urls.generic.searchList + self.entity + url;
        var result = rapid.dataManager.getData(searchUrl);
        result.done(function (value) {
            rapid.errorManager.checkDataStatus(value, function () {
                self.data.removeAll();
                self.page(value.Result.PagingInfo.CurrentPage);
                self.totalCount(value.Result.PagingInfo.TotalItems);
                self.totalPages(value.Result.PagingInfo.TotalPages);
                self.data.pushAll(value.Result.Items);
                self.isBusy(false);
            });
            self.isBusy(false);
        });
    };

    //self.hasData is to find list has value or not.
    self.hasData = ko.computed(function () {
        if (self.data().length > 0) {
            return true;
        }
        else {
            return false;
        }
    });


    // stringStartSearchWith is used to find the character in the filter search in the 
    var stringStartSearchWith = function (string, startsWith) {
        if (string != undefined) {
            return (string.toLowerCase().indexOf(startsWith) !== -1);
        }
        else
            return false;
    };

    //self.create is the publish create event for the entity.
    self.create = function () {
        rapid.eventManager.publish(self.entity + "Create");
    };

    //self.edit is the publish edit event for the entity.
    self.edit = function (item) {
        var jsonData = { entityType: self.entity, entityId: item.Id };
        rapid.eventManager.publish(self.entity + "Update", jsonData);
    };

    //self.details is the publish details event for the entity.
    self.details = function (item) {
        var jsonData = { entityType: self.entity, entityId: item.Id };
        rapid.eventManager.publish(self.entity + "Details", jsonData);
    };


    self.resetPassword = function (item) {
        var jsonData = { entityType: self.entity, entityId: item.Id };
        rapid.eventManager.publish(self.entity + "ResetPassword", jsonData);
    };


    self.delete = function (item) {
        rapid.modalManager.confirm(rapid.messageManager.deleteWarningMessage('this'), function (result) {
            if (result) {
                var jsonData = { entityType: self.entity, entityId: item.Id };
                rapid.eventManager.publish(self.entity + "Delete", jsonData);
            }

        });
    };
    self.SubscribeDelete = function (data) {
        self.isBusy(true);
        var deleteResult = rapid.dataManager.postData(urls.generic.delete + data.entityType + "s/" + data.entityId);
        deleteResult.done(function (apiResult) {
            if (apiResult.Status) {
                self.isBusy(false);
                rapid.notificationManager.showSuccess(rapid.messageManager.deleteMessage(data.entityType));
                rapid.eventManager.publish(self.entity + "Deleted");
            } else {
                self.isBusy(false);
                rapid.modalManager.alert(apiResult.Message);
            }
        });
    };


    self.loadMore = function () {
        if (self.page() < self.totalPages()) {
            self.page(self.page() + 1);           
            self.fillData()
            //rapid.eventManager.publish(self.entity + "Search", args);
          
        }     
    }

  

    return self;
};

function ManageViewModelBase(vm) {
    var self = vm;
    self.init = function (entity) {
        self.entity = entity;

        self.showQuickView = ko.observable(false);
        amplify.subscribe(self.entity + "Create", function () {
            if (self.showQuickView() == false) {
                self.showQuickView(true);
            }
        });
        amplify.subscribe(self.entity + "Update", function () {
            if (self.showQuickView() == false) {
                self.showQuickView(true);
            }
        });
        amplify.subscribe(self.entity + "Details", function () {
            if (self.showQuickView() == false) {
                self.showQuickView(true);
            }
        });
        amplify.subscribe(self.entity + "ResetPassword", function () {
            if (self.showQuickView() == false) {
                self.showQuickView(true);
            }
        });
        amplify.subscribe(self.entity + "AddMember", function () {
            if (self.showQuickView() == false) {
                self.showQuickView(true);
            }
        });
        amplify.subscribe(self.entity + "EmployeeDetails", function () {
            if (self.showQuickView() == false) {
                self.showQuickView(true);
            }
        });
        amplify.subscribe(self.entity + "Cancel", function () {
            self.showQuickView(false);
        });
    };
    return self;
};

function FormViewModelBase(vm) {

    var self = vm;

    self.isBusy = ko.observable();

    self.setValue = function (entity, template, model, mode) {
        self.entity = entity;
        self.templateName = ko.observable(template);
        self.selectedData = ko.observable(model);
        self.mode = ko.observable(mode);
    };
    return self;
};

function ErrorManager() {
    var self = this;
    self.checkDataStatus = function (data, callback) {
        if (data.Status == true) {
            callback();

        } else {
            rapid.modalManager.alert(data.Message);
        }
    };
};

function DataManager() {
    var self = this;

    self.ErrorMessage = "Bad Server, something has gone wrong.";

    self.ajaxTransport = function (url, options) {
        var baseUrl = '' + url;
        var deferred = new $.Deferred();

        var defaults = {
            statusCode: {
                500: function (xhr, status, error) {
                    console.log(error);
                },
                404: function (xhr, status, error) {
                    console.log(error);
                },
                403: function (xhr, status, error) {
                    rapid.modalManager.alert("Session Expired, Login again.", function () {
                        rapid.windowManager.Redirect(urls.Account.SignIn);
                    });
                }
            },
            url: baseUrl,
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
            success: function (result) {
                deferred.resolve(result);
            },
            error: function (xhr, status, error) {

                if (error == "Forbidden") {
                    deferred.reject(this, arguments);
                } else {
                    var result = { Status: false, Message: self.ErrorMessage, Result: null };
                    deferred.resolve(result);

                    var report = {
                        errorText: ko.toJSON(xhr),
                        url: url,
                        lineNumber: null
                    };
                    $.ajax({
                        type: "POST",
                        url: "/error/record",
                        dataType: 'json',
                        data: report,
                        cache: false
                    });

                }
            }
        };

        var o = $.extend({}, defaults, options);
        $.ajax(o);
        return deferred.promise();
    };

    self.getData = function (url, tryCache) {
        if (tryCache) {
            var cached = localStorage.getItem(url);
            if (cached) {
                var deferred = new $.Deferred();
                var deserialized = JSON.parse(cached);
                return deferred.resolve(deserialized);
            } else {
                var promise = self.ajaxTransport(url, { type: 'GET' });
                promise.done(function (result) {
                    var serialized = JSON.stringify(result);
                    localStorage.setItem(url, serialized);
                });
                return promise;
            }
        } else {
            return self.ajaxTransport(url, { type: 'GET' });
        }
    };

    self.postData = function (url, data) {
        return self.ajaxTransport(url, { type: 'POST', data: JSON.stringify(data) });
    };

    self.postDeleteData = function (url, data) {
        return self.ajaxTransport(url, { type: 'POST', data: JSON.stringify(data) });
    };

    self.postFile = function (url, formData) {
        var deferred = new $.Deferred();
        $.ajax({
            type: "POST",
            url: url,
            data: formData,
            dataType: 'json',
            contentType: false,
            processData: false,
            success: function (result) {
                deferred.resolve(result);
            },
            error: function () {
                deferred.reject(this, arguments);
            }
        });
        return deferred.promise();
    }
};

function NotificationManager(message) {
    var self = this;

    self.showSuccess = function (message) {
        toastr.success(message);
    };

    self.showError = function (message) {
        toastr.error(message, 'Error');
    };
    self.showWarning = function (message) {
        toastr.warning(message, 'Warning');
    };
    self.showInfo = function (message) {
        toastr.info(message, 'Information');
    };
}
function ModalManager() {
    var self = this;

    self.confirm = function (message, callback) {
        bootbox.confirm(message, callback);
    };

    self.alert = function (message) {
        bootbox.alert(message, function () {
        });
    };
};

function MessageManager() {
    var self = this;

    self.entity = {};

    self.createMessage = function (entity) {
        return entity + " saved successfully.";
    };
    self.updateMessage = function (entity) {
        return entity + " updated successfully.";
    };

    self.deleteWarningMessage = function (entity) {
        return "Are you sure you want to delete " + entity + "?";
    };

    self.deleteMessage = function (entity) {
        return " Deleted successfully.";

      
        };
    }

    rapid = {};
    rapid.eventManager = {};
    rapid.dataManager = new DataManager();

    rapid.modalManager = new ModalManager();
    rapid.notificationManager = new NotificationManager();
    rapid.messageManager = new MessageManager();
    rapid.errorManager = new ErrorManager();

    rapid.eventManager.publish = function (topic, context) {
        amplify.publish(topic, context);
    };

    rapid.eventManager.subscribe = function (topic, context, callback) {
        amplify.subscribe(topic, context, callback);
    };



