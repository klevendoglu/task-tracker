(function () {
    appModule.controller('projectmanagement.views.manager.task.index', [
        '$scope', '$uibModal', '$uibModalInstance', 'uiGridConstants', 'abp.services.app.projectManagement',
        function ($scope, $modal, $modalInstance, uiGridConstants, appService) {
            var vm = this;
            var projectId = abp.session.projectId;
            vm.projectId = projectId;

            $scope.$on('$viewContentLoaded', function () {
                App.initAjax();
            });

            vm.loading = false;

            var fetchData = function() {
                vm.loading = true;
                appService.getTasksWithLogCounts({
                    projectId: projectId,
                    filter: vm.filterText,
                    maxResultCount: 50
                }).success(function(data) {
                    vm.tasks = data.items;
                    if (data.items != null) {
                      vm.tasksProjectName = data.items[0].projectName;
                    }                  
                }).finally(function() {
                    vm.loading = false;
                });
            };

            // fetch data
            fetchData();

            vm.View = function (itemId) {
                abp.session.taskId = itemId;
                $modal.open({
                    size: 'lg', // 'sm' for small modal
                    templateUrl: '/App/projectmanagement/views/shared/viewtask.cshtml',
                    controller: 'projectmanagement.views.shared.viewtask as vm'
                });
            }

            vm.Create = function (itemId) {
                abp.session.projectId = itemId;
                var modalInstance = $modal.open({
                    size: 'lg', // 'sm' for small modal
                    templateUrl: '/App/projectmanagement/views/manager/task/create.cshtml',
                    controller: 'projectmanagement.views.manager.task.create as vm',
                    backdrop: 'static'
                });

                modalInstance.result.then(function (result) {
                    fetchData();
                });
            }

            vm.Update = function (item) {
                abp.session.taskId = item.id;
                var modalInstance = $modal.open({
                    size: 'lg', // 'sm' for small modal
                    templateUrl: '/App/projectmanagement/views/manager/task/update.cshtml',
                    controller: 'projectmanagement.views.manager.task.update as vm',
                    backdrop: 'static'
                });

                modalInstance.result.then(function (result) {
                    fetchData();
                });
            }

            vm.Poke = function (itemId) {
                abp.message.confirm(
                    app.localize('PokeAgent'),
                    function (isConfirmed) {
                        if (isConfirmed) {
                            appService.pokeAgent({
                                id: itemId
                            }).success(function () {
                                abp.notify.success(app.localize('ActionSuccessfullyCompleted'));
                            });
                        }
                    }
                );
            };

            vm.Delete = function (itemId) {
                abp.message.confirm(
                    app.localize('DeleteWarningMessage', itemId),
                    function (isConfirmed) {
                        if (isConfirmed) {
                            appService.deleteTask({
                                id: itemId
                            }).success(function () {
                                fetchData();
                                abp.notify.success(app.localize('SuccessfullyDeleted'));
                            });
                        }
                    }
                );
            };

            vm.ViewTaskLogs = function (itemId) {
                abp.session.taskId = itemId;
                var modalInstance = $modal.open({
                    size: 'lg', // 'sm' for small modal
                    templateUrl: '/App/projectmanagement/views/agent/tasklog/index.cshtml',
                    controller: 'projectmanagement.views.agent.tasklog.index as vm',
                    backdrop: 'static'
                });

                modalInstance.result.then(function (result) {
                    fetchData();
                });
            }

            vm.Cancel = function () {
                $modalInstance.close();
            };

            vm.Filter = function () {
                fetchData();
            }

            vm.Localize = function (toLocalize) {
                return app.localize(toLocalize);
            };

        }]);
})();