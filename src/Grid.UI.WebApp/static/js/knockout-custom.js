define(["require", "exports", 'knockout', 'jquery', 'moment', 'boostrapDatePicker'], function (require, exports, ko, $, moment, datetimepicker) {

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

            var url = ko.unwrap(valueAccessor());
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
    ko.bindingHandlers.dateTimePicker = {
        init: function (element, valueAccessor, allBindingsAccessor) {
            //initialize datepicker with some optional options
            var options = allBindingsAccessor().dateTimePickerOptions || {};
            $(element).datetimepicker(options);

            //when a user changes the date, update the view model
            ko.utils.registerEventHandler(element, "dp.change", function (event) {
                var value = valueAccessor();
                if (ko.isObservable(value)) {
                    if (event.date != null && !(event.date instanceof Date)) {
                        value(event.date.toDate());
                    } else {
                        value(event.date);
                    }
                }
            });

            ko.utils.domNodeDisposal.addDisposeCallback(element, function () {
                var picker = $(element).data("DateTimePicker");
                if (picker) {
                    picker.destroy();
                }
            });
        },
        update: function (element, valueAccessor, allBindings, viewModel, bindingContext) {

            var picker = $(element).data("DateTimePicker");
            //when the view model is updated, update the widget
            if (picker) {
                var koDate = ko.utils.unwrapObservable(valueAccessor());

                //in case return from server datetime i am get in this form for example /Date(93989393)/ then fomat this
                koDate = (typeof (koDate) !== 'object') ? moment(koDate) : koDate;

                picker.date(koDate);
            }
        }
    };

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
});


