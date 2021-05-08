(function () {
    appModule.controller('projectmanagement.views.agent.tasklog.create', [
        '$scope', '$state', '$stateParams', '$uibModalInstance', 'Upload', 'abp.services.app.projectManagement',
        function ($scope, $state, $stateParams, $modalInstance, uploader, appService) {
            var vm = this;
           
            vm.input = { closeTask:abp.session.isClosed, taskId: abp.session.taskId, projectId: abp.session.projectId };

            vm.Save = function (uploads) {
                vm.saving = true;            
                vm.input.attachments = app.uploadToLocal(uploads, uploader);
                appService.createTaskLog(vm.input)
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