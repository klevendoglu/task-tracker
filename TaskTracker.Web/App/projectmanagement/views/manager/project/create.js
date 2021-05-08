(function () {
    appModule.controller('projectmanagement.views.manager.project.create', [
        '$scope', '$uibModalInstance', 'Upload', 'abp.services.app.projectManagement', '$filter',
        function ($scope, $modalInstance, uploader, appService, $filter) {
            var vm = this;
            vm.managers = [];
            vm.input = { managerIds: [] };
            appService.getProjectManagers({}).success(function (data) {
                vm.managers = data.items;
            });

            vm.endDateBeforeRender = endDateBeforeRender
            vm.endDateOnSetTime = endDateOnSetTime
            vm.startDateBeforeRender = startDateBeforeRender
            vm.startDateOnSetTime = startDateOnSetTime

            function startDateOnSetTime() {
                $scope.$broadcast('start-date-changed');
            }

            function endDateOnSetTime() {
                $scope.$broadcast('end-date-changed');
            }

            function startDateBeforeRender($dates) {
                if (vm.input.endTime) {
                    var activeDate = moment(vm.input.endTime);

                    $dates.filter(function (date) {
                        return date.localDateValue() >= activeDate.valueOf()
                    }).forEach(function (date) {
                        date.selectable = false;
                    })
                }
            }

            function endDateBeforeRender($view, $dates) {
                if (vm.input.startTime) {
                    var activeDate = moment(vm.input.startTime).subtract(1, $view).add(1, 'minute');

                    $dates.filter(function (date) {
                        return date.localDateValue() <= activeDate.valueOf()
                    }).forEach(function (date) {
                        date.selectable = false;
                    })
                }
            }

            vm.Save = function (uploads) {
                if (!vm.input.managerIds.length) {
                    abp.message.error("Select at least one manager!", "Manager is required");
                    return;
                }
                vm.input.attachments = app.uploadToLocal(uploads, uploader);

                vm.saving = true;
                //vm.input.startTime = $filter('date')(vm.input.startTime, 'yyyy-MM-dd HH:mm');
                //vm.input.endTime = $filter('date')(vm.input.endTime, 'yyyy-MM-dd HH:mm');
                appService.createProject(vm.input)
                    .success(function () {
                        abp.notify.info(app.localize('SavedSuccessfully'));
                    }).finally(function () {
                        vm.saving = false;
                        $modalInstance.close();
                    });
            };

            vm.Cancel = function () {
                $modalInstance.dismiss();
            };
        }
    ]);
})();