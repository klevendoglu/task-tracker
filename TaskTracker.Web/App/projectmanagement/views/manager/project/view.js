(function () {
    appModule.controller('projectmanagement.views.manager.view', [
        '$scope', '$uibModalInstance', 'abp.services.app.projectManagement', '$sce', '$filter',
        function ($scope, $modalInstance, appService, $sce, $filter) {
            var vm = this;            

            appService.getProjectWithTasksAndManagersAndAttachments({ id: abp.session.projectId }).success(function (data) {
                vm.output = data;
                //vm.output.startTime = app.convertToUTC(vm.output.startTime);
                //vm.output.endTime = app.convertToUTC(vm.output.endTime);
                vm.output.bodyText = $sce.trustAsHtml(vm.output.bodyText);
            });
            
            vm.Cancel = function () {
                $modalInstance.dismiss();
            };
        }
    ]);
})();