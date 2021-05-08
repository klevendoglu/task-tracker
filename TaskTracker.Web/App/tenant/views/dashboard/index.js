(function () {
    appModule.controller('tenant.views.dashboard.index', [
        '$scope', 'abp.services.app.tenantDashboard', 'abp.services.app.widgets',
        function ($scope, tenantDashboardService, widgetsService) {
            var vm = this;
            
            $scope.$on('$viewContentLoaded', function () {
                App.initAjax();
            });

            vm.userId = abp.session.userId;
            vm.currentPage = 1;
            vm.itemsPerPage = app.consts.grid.defaultPageSize;
            vm.userWidgets = [];

            vm.widgets = {
                TaskManagement: 1,
                TaskAgent: 2
            };

            vm.isTaskManager = abp.auth.hasPermission('Pages.TaskTracker.Manager');
            vm.isTaskAgent = abp.auth.hasPermission('Pages.TaskTracker.Agent');

            vm.showWidget = function (widgetId) {
                return vm.userWidgets.indexOf(widgetId) > -1;
            }

            // initialize
            var init = function () {
                var input = { id: abp.session.userId };
                widgetsService.getUserWidgetIds(input)
                    .success(function (data) {
                        vm.userWidgets = data.widgetIds;
                    })
                    .error(function (data, status) {
                        abp.message.error(app.localize(data.message));
                    })
                    .finally(function () {

                    });
            };

            init();
          
        }
    ]);
})();