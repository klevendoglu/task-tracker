(function () {
    appModule.controller('projectmanagement.views.manager.task.update', [
        '$scope', '$uibModalInstance', 'Upload', 'abp.services.app.projectManagement',
        function ($scope, $modalInstance, uploader, appService) {
            var vm = this;
            var taskId = abp.session.taskId;
            
            appService.getProjectAgents({}).success(function (data) {
                vm.agents = data.items;
            });

            appService.getTask({ id: taskId }).success(function (data) {
                vm.input = data;
            });

            var fetchAttachments = function () {
                appService.getTaskAttachments({ id: taskId }).success(function(data) {
                    vm.taskAttachments = data.items;
                });
            };

            fetchAttachments();

            vm.DeleteTaskAttachment = function (attachment, taskAttachmentId) {
                abp.message.confirm(
                    app.localize('DeleteWarningMessage', attachment.fileName),
                    function (isConfirmed) {
                        if (isConfirmed) {
                            appService.deleteTaskAttachment({
                                id: taskAttachmentId
                            }).success(function () {
                                fetchAttachments();
                                abp.notify.success(app.localize('SuccessfullyDeleted'));
                            });
                        }
                    }
                );
            };

            vm.Save = function (uploads) {
                vm.saving = true;
                vm.input.attachments = app.uploadToLocal(uploads, uploader);

                appService.updateTask(vm.input)
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