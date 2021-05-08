(function () {
    appModule.controller('projectmanagement.views.manager.task.create', [
        '$scope', '$rootScope', '$uibModalInstance', 'Upload', 'abp.services.app.projectManagement',
        function ($scope, $rootScope, $modalInstance, uploader, appService) {
            var vm = this;

            

            vm.input = { projectId: abp.session.projectId };
            
            appService.getProjectAgents({}).success(function (data) {
                vm.agents = data.items;
            });

            vm.Save = function (uploads) {
                vm.saving = true;
                vm.input.attachments = app.uploadToLocal(uploads, uploader);

                appService.createTask(vm.input)
                    .success(function () {
                        abp.notify.info(app.localize('SavedSuccessfully'));
                    }).finally(function () {
                        vm.saving = false;
                        $modalInstance.close();
                        $rootScope.$broadcast('taskCreated');
                    });
            };

            vm.Cancel = function () {
                $modalInstance.dismiss();
            };
        }
    ]);
})();