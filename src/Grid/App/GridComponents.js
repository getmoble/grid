// Custom Bindings 
ko.bindingHandlers.date = {
    init: function (element, valueAccessor, allBindingsAccessor, viewModel) {
        var jsonDate = valueAccessor();
        var ret = moment(jsonDate).format('MM/DD/YYYY');
        element.innerHTML = ret;
    },
    update: function (element, valueAccessor, allBindingsAccessor, viewModel) {

    }
};

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
ko.bindingHandlers.typeahead = {
    update: function (element, valueAccessor, allBindings, viewModel, bindingContext) {
        // http://stackoverflow.com/a/19366003/1247130 get value to update properly when typeahead choice is selected.
        var templateName = ko.unwrap(allBindings().templateName);
        var mapping = ko.unwrap(allBindings().mappingFunction);
        var displayedProperty = ko.unwrap(allBindings().displayKey);
        var value = allBindings.get("value");
       
        var url = ko.unwrap(valueAccessor()) + '&projectId= ' + viewModel.ProjectId();
        var remoteFilter = ko.unwrap(allBindings.get("remoteFilter"));
        var auth = (allBindings.has("authToken")) ? {
            "Authorization": "Bearer " + ko.unwrap(allBindings().authToken)
        } : {};
        var remoteData = {
            url: url,
            wildcard: '%QUERY',
            ajax: {
                headers: auth
            }
        };
        if (remoteFilter) {
            remoteData.filter = remoteFilter;
        };

        var resultsLimit = allBindings.get("limit") || 10;

        var suggestions = new Bloodhound({
            datumTokenizer: function (token) {
                return Bloodhound.tokenizers.whitespace(token);
            },
            queryTokenizer: Bloodhound.tokenizers.whitespace,
            remote: remoteData,
            limit: resultsLimit
        });

        suggestions.initialize();

        $(element).typeahead("destroy");

        var typeaheadOpts = {
            source: suggestions.ttAdapter(),
            displayKey: displayedProperty || function (item) {
                return item.Value;
            }
        };

        if (templateName) {
            typeaheadOpts.templates = {
                suggestion: function (item) {
                    var temp = document.createElement("div");
                    var model = mapping ? mapping(item) : item;
                    ko.renderTemplate(templateName, model, null, temp, "replaceChildren");

                    return temp;
                }
            };
        }

        $(element)
			.typeahead({
			    hint: true,
			    highlight: true
			}, typeaheadOpts)
		.on("typeahead:selected typeahead:autocompleted", function (e, suggestion) {
		    if (value && ko.isObservable(value)) {
		        value(suggestion.Value);
		        var test = bindingContext.$rawData.TaskId(suggestion.Id);
		        var billingType = bindingContext.$rawData.TaskBillingType(suggestion.TaskBillingType);

		        if (suggestion.TaskBillingType == "NonBillable") {
		            viewModel.WorkType(2);
		        }
		        else {
		            viewModel.WorkType(1);
		        }
		    }
		});
    }
};
// This needs a loader class in the css, which is the spinner
ko.bindingHandlers.loadingWhen = {
    init: function (element) {
        var
            $element = $(element),
            currentPosition = $element.css("position");
            var $loader = $("<div>").addClass("loader").hide();

        //add the loader
        $element.append($loader);
        
        //make sure that we can absolutely position the loader against the original element
        if (currentPosition == "auto" || currentPosition == "static")
            $element.css("position", "relative");

        //center the loader
        $loader.css({
            position: "absolute",
            top: "50%",
            left: "50%",
            "margin-left": -($loader.width() / 2) + "px",
            "margin-top": -($loader.height() / 2) + "px"
        });
    },
    update: function (element, valueAccessor) {
        var isLoading = ko.utils.unwrapObservable(valueAccessor()),
            $element = $(element),
            $childrenToHide = $element.children(":not(div.loader)"),
            $loader = $element.find("div.loader");

        if (isLoading) {
            $childrenToHide.css("visibility", "hidden").attr("disabled", "disabled");
            $loader.show();
        }
        else {
            $loader.fadeOut("fast");
            $childrenToHide.css("visibility", "visible").removeAttr("disabled");
        }
    }
};

ko.bindingHandlers.datepicker = {
    init: function (element, valueAccessor, allBindingsAccessor, viewModel) {
        var unwrap = ko.utils.unwrapObservable;
        var dataSource = valueAccessor();
        var binding = allBindingsAccessor();
        var options = {
            keyboardNavigation: true,
            todayHighlight: true,
            autoclose: true,
            //daysOfWeekDisabled: [0, 6],
            format: 'mm/dd/yyyy'
        };
        if (binding.datePickerOptions) {
            options = $.extend(options, binding.datePickerOptions);
        }
        $(element).datepicker(options);
        $(element).datepicker('update', dataSource());
        $(element).on("changeDate", function (ev) {
            var observable = valueAccessor();
            if ($(element).is(':focus')) {
                // Don't update while the user is in the field...
                // Instead, handle focus loss
                $(element).one('blur', function (ev) {
                    var dateVal = $(element).datepicker("getDate");
                    observable(new Date(dateVal.getTime() + Math.abs(dateVal.getTimezoneOffset() * 60000)));
                });
            }
            else {
                observable(new Date(ev.date.getTime() + Math.abs(ev.date.getTimezoneOffset() * 60000)));
            }
        });
        //handle removing an element from the dom
        ko.utils.domNodeDisposal.addDisposeCallback(element, function () {
            $(element).datepicker('remove');
        });
    },
    update: function (element, valueAccessor) {
        var value = ko.utils.unwrapObservable(valueAccessor());
        // $(element).datepicker('update', value);
        $(element).datepicker("setValue", new Date(value.getTime + Math.abs(value.getTimezoneOffset * 60000)));

    }
};
ko.validation.makeBindingHandlerValidatable('datepicker');


ko.bindingHandlers.htmlEditor = {
    init: function (element, valueAccessor, allBindingsAccessor) {
        var options = valueAccessor();
        var binding = ko.utils.unwrapObservable(allBindingsAccessor()).value;
        var updateObservable = function(e) {
            binding(e.currentTarget.innerHTML);
        };

        options.onkeydown = options.onkeyup = options.onfocus = options.onblur = updateObservable;

        $(element).summernote(options);
        $(element).next().on('focusout', ".note-codable", function () {
            if ($(element).summernote('codeview.isActivated')) {
                $(element).summernote('codeview.deactivate');
            }
        });

    },
    update: function (element, valueAccessor, allBindingsAccessor, viewModel, bindingContext) {
        var value = ko.unwrap(valueAccessor());
        $(element).summernote('code', value);

        $(element).next().on('focusout', ".note-codable", function () {
            if ($(element).summernote('codeview.isActivated')) {
                $(element).summernote('codeview.deactivate');
            }
        });
    }
};


ko.bindingHandlers.truncatedText = {
    update: function (element, valueAccessor, allBindingsAccessor) {
        var originalText = ko.utils.unwrapObservable(valueAccessor()) || '';
        var length = ko.utils.unwrapObservable(allBindingsAccessor().maxTextLength) || 50;
        var truncatedText = originalText.length > length ? originalText.substring(0, length) + "..." : originalText;
        // updating text binding handler to show truncatedText
        ko.bindingHandlers.text.update(element, function () {
            return truncatedText;
        });
    }
};


//datestring is to show date on label
ko.bindingHandlers.dateString = {
    update: function (element, valueAccessor, allBindingsAccessor, viewModel) {
        var value = valueAccessor();
        var timeRequired = allBindingsAccessor.get('time');
        var valueUnwrapped = ko.utils.unwrapObservable(value);
        if (valueUnwrapped) {
            var date = new Date(valueUnwrapped);
            var day = date.getDate(); //Date of the month: 2 in our example
            var month = date.getMonth() + 1; //Month of the Year: 0-based index, so 1 in our example
            var year = date.getFullYear() //Year: 2013
            if (timeRequired) {
                var time = date.toLocaleTimeString();
                $(element).text(month + '/' + day + '/' + year + ' ' + time);
            }
            else {
                $(element).text(month + '/' + day + '/' + year);
            }
        }
    }
};

var templateFromUrlLoader = {
    loadTemplate: function (name, templateConfig, callback) {
        if (templateConfig.fromUrl) {
            // Uses jQuery's ajax facility to load the markup from a file
            var fullUrl = '/Templates/' + templateConfig.fromUrl;
            $.get(fullUrl, function (markupString) {              
                // We need an array of DOM nodes, not a string.
                // We can use the default loader to convert to the
                // required format.
                ko.components.defaultLoader.loadTemplate(name, markupString, callback);
            });
        } else {
            // Unrecognized config format. Let another loader handle it.
            callback(null);
        }
    }
};

// Register it
ko.components.loaders.unshift(templateFromUrlLoader);


// Base Classes
function PagedViewModelBase(vm) {
    var self = vm;

    self.isBusy = ko.observable();

    self.entity = '';

    self.filters = ko.observableArray([]);
    self.data = ko.observableArray([]);

    self.loadAll = function () {
        self.isBusy(true);
        var apiUrl = '/Api/' + self.entity;

        if (self.filters().length > 0) {
            apiUrl = apiUrl + "?";
            // Add all filters
            $.each(self.filters(), function (key, val) {
                apiUrl = apiUrl + val;
            });
        }
        $.getJSON(apiUrl, function (apiResult) {
            if (apiResult.Status) {
                self.data.removeAll();
                $.each(apiResult.Result, function (key, val) {
                    self.data.push(val);
                });
                self.isBusy(false);
            }
            else
            {
                self.isBusy(false);
                bootbox.alert(apiResult.Message, function () {

                });
            }
        });
    };

    self.delete = function (row) {
        bootbox.confirm("Are you sure, you want to delete this?", function (result) {
            if (result) {
                var deleteUrl = '/Api/' + self.entity + "/Delete/" + row.Id;
                $.post(deleteUrl, function (apiResult) {
                    if (apiResult.Status) {
                        bootbox.alert(apiResult.Message, function() {
                            self.loadAll();
                        });
                    } else {
                        bootbox.alert(apiResult.Message, function () {

                        });
                    }
                });
            }
        });
    }
    self.details = function (row) {
        var detailsUrl = self.entity + "/Details/" + row.Id;
        window.location = detailsUrl;
    }
    self.edit = function (row) {
        var editUrl = self.entity + "/Edit/" + row.Id;
        window.location = editUrl;
    }

    self.init = function (entity) {
        self.entity = entity;
        self.loadAll();
    }

    return self;
}
function CreateEditViewModelBase(vm) {

    var self = vm;

    self.isBusy = ko.observable();

    self.entity = '';
    self.selected = null;


    self.replaceUrlParam = function (url, paramName, paramValue) {
        var pattern = new RegExp('\\b(' + paramName + '=).*?(&|$)');
        if (url.search(pattern) >= 0) {
            return url.replace(pattern, '$1' + paramValue + '$2');
        }
        return url + (url.indexOf('?') > 0 ? '&' : '?') + paramName + '=' + paramValue;
    }

    self.goBackAndRefresh = function (fallback) {
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

            window.history.back();
        }
    }

    self.submitForm = function (formElement) {
        self.isBusy(true);
        var updateUrl = '/Api/' + self.entity + "/Update";
        var payload = $(formElement).serialize();
        $.post(updateUrl, payload, function (apiResult) {
            self.isBusy(false);
            if (apiResult.Status) {
                bootbox.alert(apiResult.Message, function () {
                    self.goBackAndRefresh();
                });
            }
            else {
                bootbox.alert(apiResult.Message, function () {
                    
                });
            }
        });
    }

    self.init = function (entity, model) {
        self.entity = entity;
        self.selected = model;
    }

    self.load = function(id) {
        self.isBusy(true);
        var apiUrl = '/Api/' + self.entity + "/Get/" + id;

        $.getJSON(apiUrl, function (apiResult) {
            if (apiResult.Status) {
                ko.mapping.fromJS(apiResult.Result, {}, self.selected);
                self.isBusy(false);
            }
            else {
                bootbox.alert(apiResult.Message, function () {

                });
            }
        });
    }

    return self;
}
function ListingViewModelBase(vm) {
    var self = vm;

    self.data = ko.observableArray();
    self.isBusy = ko.observable(false);
    self.filter = ko.observable('');
    self.setValues = function (params) {

        self.entity = params.entity;
        self.showCreate = params.showCreate;
        self.manageList = params.manageList;
        self.enableFilter = params.enableFilter;

        amplify.subscribe(self.entity + "Search", function (data) {
            self.init(data);
        });
        amplify.subscribe(self.entity + "Refresh", function () {
            self.init();
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
                self.data.pushAll(value.Result);
                self.isBusy(false);
            });
            self.isBusy(false);
        });
    };

    self.delete = function (row) {
        bootbox.confirm("Are you sure, you want to delete this?", function (result) {
            if (result) {
                var deleteUrl = '/Api/' + self.entity + "/Delete/" + row.Id;
                $.post(deleteUrl, function (apiResult) {
                    if (apiResult.Status) {
                        bootbox.alert(apiResult.Message, function () {
                            self.loadAll();
                        });
                    } else {
                        bootbox.alert(apiResult.Message, function () {

                        });
                    }
                });
            }
        });
    }
    self.details = function (item) {
        var jsonData = { entityType: self.entity, entityId: item.Id };
        window.location = self.entity +"s" + "/Details/" + item.Id;
    };
    self.edit = function (row) {
        var editUrl = self.entity + "/Edit/" + row.Id;
        window.location = editUrl;
    }
    
    return self;
};

// Company -> Permission
ko.components.register('permissionslist', {
    viewModel: function (params) {
        var self = PagedViewModelBase(this);

        if (typeof (params.RoleId) !== "undefined" && params.RoleId !== null) {
            self.filters.push("RoleId=" + params.RoleId);
        }

        self.init('Permissions');
    },
    template: { fromUrl: 'List?type=Permissions' }
});

// Company -> EmailTemplates
ko.components.register('jobopeningslist', {
    viewModel: function (params) {
        var self = PagedViewModelBase(this);

        self.init('JobOpenings');
    },
    template: { fromUrl: 'List?type=JobOpenings' }
});

// Company -> EmailTemplates
ko.components.register('publicjobopeningslist', {
    viewModel: function (params) {
        var self = PagedViewModelBase(this);

        self.init('JobOpenings');
    },
    template: { fromUrl: 'List?type=JobOpenings&Mode=ReadOnly' }
});


// Company -> EmailTemplates
ko.components.register('candidatedesignationslist', {
    viewModel: function (params) {
        var self = PagedViewModelBase(this);
        self.init('CandidateDesignations');
    },
    template: { fromUrl: 'List?type=CandidateDesignations' }
});

// Company -> EmailTemplates
ko.components.register('roundslist', {
    viewModel: function (params) {
        var self = PagedViewModelBase(this);
        self.init('Rounds');
    },
    template: { fromUrl: 'List?type=Rounds' }
});


// Company -> EmailTemplates
ko.components.register('offerslist', {
    viewModel: function (params) {
        var self = PagedViewModelBase(this);
        self.init('Offers');
    },
    template: { fromUrl: 'List?type=Offers' }
});

// Company -> EmailTemplates
ko.components.register('vendorslist', {
    viewModel: function (params) {
        var self = PagedViewModelBase(this);
        self.init('Vendors');
    },
    template: {
        fromUrl: 'List?type=Vendors'
    }
});

// Company -> EmailTemplates
ko.components.register('crmaccountslist', {
    viewModel: function (params) {
        var self = PagedViewModelBase(this);
        self.init('CRMAccounts');
    },
    template: {
        fromUrl: 'List?type=CRMAccounts'
    }
});

ko.components.register('myleavelist', {
    viewModel: function (params) {
        var self = PagedViewModelBase(this);
        self.filters.push("Mine=true");
        self.init('Leaves');
    },
    template: {
        fromUrl: 'List?type=Leaves&Mode=ReadOnly'
    }
});

ko.components.register('myteamleavelist', {
    viewModel: function (params) {
        var self = PagedViewModelBase(this);
        self.filters.push("Team=true");
        self.init('Leaves');
    },
    template: {
        fromUrl: 'List?type=TeamLeaves&Mode=ReadOnly'
    }
});

ko.components.register('myticketslist', {
    viewModel: function (params) {
        var self = PagedViewModelBase(this);
        self.filters.push("Mine=true");
        self.init('Tickets');
    },
    template: {
        fromUrl: 'List?type=Tickets&Mode=ReadOnly'
    }
});


// Company -> EmailTemplates
ko.components.register('categorieslist', {
    viewModel: function (params) {
        var self = PagedViewModelBase(this);
        self.init('Categories');
    },
    template: {
        fromUrl: 'List?type=Categories'
    }
});

ko.components.register('categoryupdate', {
    viewModel: function (params) {
        var self = CreateEditViewModelBase(this);

        var vm = {
            Id: ko.observable(0),
            Title: ko.observable(),
            Description: ko.observable(),
            IsPublic: ko.observable(true),
            ParentCategoryId: ko.observable()
        }

        self.init("Categories", vm);

        if (typeof (params.Id) !== "undefined" && params.Id !== null) {
            self.load(params.Id);
        }

    },
    template: {
        fromUrl: 'Form?type=Categories'
    }
});

ko.components.register('jobopeningupdate', {
    viewModel: function (params) {
        var self = CreateEditViewModelBase(this);

        var vm = {
            Id: ko.observable(0),
            Title: ko.observable(),
            Description: ko.observable(),
            NoOfVacancies: ko.observable(),
            OpeningStatus: ko.observable(),
            JobDescriptionPath: ko.observable()
        }

        self.init("JobOpenings", vm);

        if (typeof (params.Id) !== "undefined" && params.Id !== null) {
            self.load(params.Id);
        }

    },
    template: {
        fromUrl: 'Form?type=JobOpenings'
    }
});


ko.components.register('ticketupdate', {
    viewModel: function (params) {
        var self = CreateEditViewModelBase(this);

        var vm = {
            Id: ko.observable(0),
            TicketCategoryId: ko.observable(0),
            TicketSubCategoryId: ko.observable(0),
            Title: ko.observable(),
            Description: ko.observable(),
            DueDate: ko.observable()
        }

        self.init("Tickets", vm);

        if (typeof (params.Id) !== "undefined" && params.Id !== null) {
            self.load(params.Id);
        }

    },
    template: {
        fromUrl: 'Form?type=Tickets'
    }
});

ko.components.register('articleupdate', {
    viewModel: function (params) {
        var self = CreateEditViewModelBase(this);

        var vm = {
            Id: ko.observable(0),
            CategoryId: ko.observable(),
            Title: ko.observable(),
            Summary: ko.observable(),
            Content: ko.observable(),
            ArticleVersion: ko.observable(),
            Keywords: ko.observable(),
            IsFeatured: ko.observable()
        }

        self.init("Articles", vm);

        if (typeof (params.Id) !== "undefined" && params.Id !== null) {
            self.load(params.Id);
        }

    },
    template: {
        fromUrl: 'Form?type=Articles'
    }
});


ko.components.register('userupdate', {
    viewModel: function (params) {
        var self = CreateEditViewModelBase(this);

        var vm = {
            Id: ko.observable(0),
            DepartmentId: ko.observable(),
            EmployeeCode: ko.observable(),
            Username: ko.observable(),
            Password: ko.observable(),
            Person : {
                FirstName: ko.observable(),
                MiddleName: ko.observable(),
                LastName: ko.observable(),
                Gender: ko.observable(),
                Email: ko.observable(),
                SecondaryEmail: ko.observable(),
                PhoneNo: ko.observable(),
                Address: ko.observable(),
                CommunicationAddress: ko.observable(),
                PassportNo: ko.observable(),
                DateOfBirth: ko.observable(),
                BloodGroup: ko.observable(),
                MaritalStatus: ko.observable(),
                MarriageAnniversary: ko.observable()
            },
            LocationId: ko.observable(),
            DesignationId: ko.observable(),
            ShiftId: ko.observable(),
            ReportingPersonId: ko.observable(),
            Experience: ko.observable(),
            DateOfJoin: ko.observable(),
            ConfirmationDate: ko.observable(),
            DateOfResignation: ko.observable(),
            LastDate: ko.observable(),
            OfficialEmail: ko.observable(),
            OfficialPhone: ko.observable(),
            OfficialMessengerId: ko.observable(),
            EmployeeStatus: ko.observable(),
            TechnologyIds: ko.observable(),
            RequiresTimeSheet: ko.observable(),
            Salary: ko.observable(),
            Bank: ko.observable(),
            BankAccountNumber: ko.observable(),
            PANCard: ko.observable(),
            PaymentMode: ko.observable(),
            RoleIds: ko.observable(),
        }

        self.init("Users", vm);

        if (typeof (params.Id) !== "undefined" && params.Id !== null) {
            self.load(params.Id);
        }

    },
    template: {
        fromUrl: 'Form?type=Users'
    }
});



// Company -> EmailTemplates
ko.components.register('userslist', {
    viewModel: function (params) {
        var self = PagedViewModelBase(this);
        self.init('Users');
    },
    template: {
        fromUrl: 'List?type=Users'
    }
});


// Company -> EmailTemplates
ko.components.register('userdirectory', {
    viewModel: function (params) {
        var self = PagedViewModelBase(this);

        self.init('Users');
    },
    template: { fromUrl: 'List?type=Users&Mode=ReadOnly' }
});

// Company -> EmailTemplates
ko.components.register('knowledgebase', {
    viewModel: function (params) {
        var self = PagedViewModelBase(this);

        self.init('Articles');
    },
    template: { fromUrl: 'List?type=Articles&Mode=ReadOnly' }
});

// Small Components
ko.components.register('userimage', {
    viewModel: function (params) {

        var self = this;

        self.isBusy = ko.observable();
        self.image = ko.observable();
        self.name = ko.observable();

        if (typeof (params.UserId) !== "undefined" && params.UserId !== null) {
            self.isBusy(true);
            var apiUrl = '/Api/Employee/ProfileImage/' + params.UserId;
            $.getJSON(apiUrl, function (data) {
                self.image(data.ImageUrl);
                self.name(data.Name);
                self.isBusy(false);
            });
        }
    },
    template: '<div data-bind="loadingWhen: isBusy"><img data-bind="attr: { src: image, alt: name }" width="300"></div>'
});

ko.components.register('mycalendar', {
    viewModel: function (params) {
        var self = this;

        $('#calendar').fullCalendar({
            eventClick: function (calEvent, jsEvent, view) {

            },
            dayClick: function (date, jsEvent, view) {
            },

            eventSources: [
                {
                    url: '/Api/Calendar/Attendance',
                    color: 'grey',
                    textColor: 'white'
                },
                {
                    url: '/Api/Calendar/TimeSheets',
                    color: 'blue',
                    textColor: 'white'
                },
                {
                    url: '/Api/Calendar/Tasks',
                    color: 'green',
                    textColor: 'white'
                },
                {
                    url: '/Api/Calendar/Holidays',
                    color: 'orange',
                    textColor: 'white'
                },
                {
                    url: '/Api/Calendar/Leaves',
                    color: 'red',
                    textColor: 'white'
                }
            ]
        });
    },
    template: "<div id='calendar'></div>"
});



ko.components.register('ticketslist', {
    viewModel: function (params) {
        var self = PagedViewModelBase(this);
        self.init('Tickets');
    },
    template: {
        fromUrl: 'List?type=Tickets'
    }
});


ko.components.register('articlelist', {
    viewModel: function (params) {
        var self = PagedViewModelBase(this);
        self.init('Articles');
    },
    template: {
        fromUrl: 'List?type=Articles'
    }
});


ko.components.register('tasklist', {
    viewModel: function (params) {
        var self = PagedViewModelBase(this);
        self.init('Tasks');
    },
    template: {
        fromUrl: 'List?type=Tasks'
    }
});

ko.components.register('projectupdate', {
    viewModel: function (params) {
        var self = CreateEditViewModelBase(this);

        var vm = {
            Id: ko.observable(0),
            ClientId: ko.observable(),
            Title: ko.observable(),
            Description: ko.observable(),
            StartDate: ko.observable(),
            EndDate: ko.observable(),
            Status: ko.observable(),
            Billing: ko.observable(),
            ExpectedBillingAmount: ko.observable(),
            Rate: ko.observable(),
            ParentId: ko.observable(),
            IsPublic: ko.observable(),
            InheritMembers: ko.observable(),
            IsClosed: ko.observable()
        }

        self.init("Projects", vm);

        if (typeof (params.Id) !== "undefined" && params.Id !== null) {
            self.load(params.Id);
        }

    },
    template: {
        fromUrl: 'Form?type=Projects'
    }
});

ko.components.register('taskupdate', {
    viewModel: function (params) {
        var self = CreateEditViewModelBase(this);

        var vm = {
            Id: ko.observable(0),
            Title: ko.observable(),
            Description: ko.observable(),
            ExpectedTime: ko.observable(),
            ActualTime: ko.observable(),
            TaskStatus: ko.observable(),
            Priority: ko.observable(),
            AssigneeId: ko.observable(),
            StartDate: ko.observable(),
            DueDate: ko.observable(),
            ProjectId: ko.observable(),
            ParentId: ko.observable()
        }

        self.init("Tasks", vm);

        if (typeof (params.Id) !== "undefined" && params.Id !== null) {
            self.load(params.Id);
        }

    },
    template: {
        fromUrl: 'Form?type=Tasks'
    }
});


function Location() {
    this.Title = ko.observable();
    this.Phone = ko.observable();
    this.Address = ko.observable();
    this.CreatedOn = ko.observable();
}

ko.components.register('locationform',
{
    viewModel: function (params) {
        var selectedItem = new Location();
        var self = CreateEditViewModelBase(this);
        self.init(selectedItem);
    },
    template: '<form data-bind="submit: submitForm">' +
        '<input type="text" name="Title" data-bind="Title">' +
        '</form>'
});