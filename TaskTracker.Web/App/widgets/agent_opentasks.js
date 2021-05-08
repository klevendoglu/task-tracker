(function () {
    appModule.controller('widgets.agent.opentasks', [
        '$scope', '$rootScope', '$state', '$stateParams', '$uibModal', 'abp.services.app.projectManagement',
        function ($scope, $rootScope, $state, $stateParams, $modal, appService) {
            var vm = this;

            vm.currentPage = 1;
            vm.itemsPerPage = app.consts.grid.defaultPageSize;
            vm.openOnly = false;

            vm.requestParams = {
                culture: abp.localization.currentCulture.name,
                skipCount: 0,
                maxResultCount: vm.itemsPerPage,
                sorting: null,
                openOnly: false
            };

            $scope.$on('taskCreated', function (event) { vm.getTasks() });

            vm.isSuperAdmin = abp.auth.hasPermission('Pages.Superadministration');

            if (vm.isSuperAdmin) {
                vm.getTasks = function () {
                    vm.loading = true;
                    vm.requestParams.skipCount = (vm.currentPage - 1) * vm.itemsPerPage;
                    vm.requestParams.openOnly = vm.openOnly;
                    vm.requestParams.filter = vm.filter;
                    appService.getTasksWithLogs(vm.requestParams)
                        .success(function (data) {
                            vm.tasks = data.items;
                            vm.totalItems = data.totalCount;
                            vm.showPagination = data.totalCount > vm.itemsPerPage;
                        }).finally(function () {
                            vm.loading = false;
                        });
                };
            }
            else {
                vm.getTasks = function () {
                    vm.loading = true;
                    vm.requestParams.skipCount = (vm.currentPage - 1) * vm.itemsPerPage;
                    vm.requestParams.agentId = abp.session.userId;
                    vm.requestParams.openOnly = vm.openOnly;
                    vm.requestParams.filter = vm.filter;
                    appService.getTasksWithLogs(vm.requestParams)
                        .success(function (data) {
                            vm.tasks = data.items;
                            vm.totalItems = data.totalCount;
                            vm.showPagination = data.totalCount > vm.itemsPerPage;
                        }).finally(function () {
                            vm.loading = false;
                        });
                };
            }



            vm.getTasks();

            vm.CreateLog = function (item) {
                abp.session.taskId = item.id;
                abp.session.projectId = item.projectId;
                abp.session.isClosed = item.isClosed;
                var modalInstance = $modal.open({
                    size: 'lg', // 'sm' for small modal
                    templateUrl: '/App/projectmanagement/views/agent/tasklog/create.cshtml',
                    controller: 'projectmanagement.views.agent.tasklog.create as vm',
                    backdrop: 'static'
                });

                modalInstance.result.then(function (result) {
                    vm.getTasks();
                });
            }

            vm.Filter = function () {
                vm.getTasks();
            }

            vm.ViewLogs = function (itemId) {
                abp.session.taskId = itemId;
                var modalInstance = $modal.open({
                    size: 'lg', // 'sm' for small modal
                    templateUrl: '/App/projectmanagement/views/agent/tasklog/index.cshtml',
                    controller: 'projectmanagement.views.agent.tasklog.index as vm',
                    backdrop: 'static'
                });

                modalInstance.result.then(function (result) {
                    vm.getTasks();
                });
            }

            // common
            vm.localize = function (input) {
                return app.localize(input);
            }
        }
    ]);
})();