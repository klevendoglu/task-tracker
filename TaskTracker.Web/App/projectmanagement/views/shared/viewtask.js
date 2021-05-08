(function () {
    appModule.controller('projectmanagement.views.shared.viewtask', [
        '$scope', '$uibModalInstance', 'abp.services.app.projectManagement', '$sce',
        function ($scope, $modalInstance, appService, $sce) {
            var vm = this;
            
            appService.getTaskWithAttachmentsAndLogs({ id: abp.session.taskId }).success(function (data) {
                vm.output = data;
                vm.output.bodyText = $sce.trustAsHtml(vm.output.bodyText);
            });
            
            vm.Cancel = function () {
                $modalInstance.dismiss();
            };

            vm.Localize = function (toLocalize) {
                return app.localize(toLocalize);
            };
        }
    ]);
})();