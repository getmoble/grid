
ko.components.register('graph', {
    viewModel: function (params) {

        var self = this;
        if (params.Id !== "undefined" && params.Id !== null) {
            self.projectId = params.Id;
        }

        self.init = function () {
            var chart = c3.generate({
                bindto: '#effortByDate',
                data: {
                    x: 'x',
                    xFormat: '%m/%d/%Y',
                    url: '/PMS/Projects/EffortByDateCSV?projectId=' + self.projectId,


                    type: 'area',

                },
                axis: {
                    x: {
                        type: 'timeseries',
                        tick: {
                            format: '%Y-%m-%d'
                        }
                    }
                },
                legend: {
                    position: 'inset'
                }
            });
        }
        self.init();
    },
    template: "<div id='effortByDate'></div>"


});