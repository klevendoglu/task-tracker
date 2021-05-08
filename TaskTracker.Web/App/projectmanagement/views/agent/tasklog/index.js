(function () {
    appModule.controller('projectmanagement.views.agent.tasklog.index', [
        '$scope', '$uibModal', '$uibModalInstance', 'uiGridConstants', 'abp.services.app.projectManagement',
        function ($scope, $modal, $modalInstance, uiGridConstants, appService) {
            var vm = this;
            vm.userId = abp.session.userId;
            var taskId = abp.session.taskId;

            $scope.$on('$viewContentLoaded', function () {
                App.initAjax();
            });

            vm.loading = false;
            
            var fetchData = function () {
                vm.loading = true;
                appService.getTaskLogsWithTaskAndAttachments({
                    taskId: taskId,
                    filter: vm.filterText,
                    maxResultCount: 50
                }).success(function (data) {
                    vm.taskLogs = data.items;
                    vm.taskName = data.items[0].projectTask.name;
                }).finally(function () {
                    vm.loading = false;
                });
            };

            // fetch data
            fetchData();

            vm.Update = function (item) {
                abp.session.taskLogId = item.id;
                //abp.session.taskName = item.projectTask.name;
                //abp.session.projectId = item.projectTask.projectId;
                var modalInstance = $modal.open({
                    size: 'lg', // 'sm' for small modal
                    templateUrl: '/App/projectmanagement/views/agent/tasklog/update.cshtml',
                    controller: 'projectmanagement.views.agent.tasklog.update as vm',
                    backdrop: 'static'
                });

                modalInstance.result.then(function (result) {
                    fetchData();
                });
            }

            vm.Delete = function (itemId) {
                abp.message.confirm(
                    app.localize('DeleteWarningMessage', itemId),
                    function (isConfirmed) {
                        if (isConfirmed) {
                            appService.deleteTaskLog({
                                id: itemId
                            }).success(function () {
                                fetchData();
                                abp.notify.success(app.localize('SuccessfullyDeleted'));
                            });
                        }
                    }
                );
            };

            vm.Cancel = function () {
                $modalInstance.close();
            };

            vm.Filter = function () {
                fetchData();
            }

            vm.convertToUTC = function (dt) {
                var localDate = new Date(dt);
                var localTime = localDate.getTime();
                var localOffset = localDate.getTimezoneOffset() * 60000;
                return new Date(localTime + localOffset);
            };
            
        }]);
})();