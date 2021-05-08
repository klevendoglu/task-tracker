(function () {
    appModule.controller('projectmanagement.views.agent.tasklog.update', [
        '$scope', '$state', '$stateParams', '$uibModalInstance', 'Upload', 'abp.services.app.projectManagement', '$sce', 
        function ($scope, $state, $stateParams, $modalInstance, uploader, appService, $sce) {
            var vm = this;
            var taskLogId = abp.session.taskLogId;
            var projectId = abp.session.projectId;
            vm.input = { id: taskLogId };           
            
            appService.getTaskLogWithTaskAndAttachments({ id: taskLogId }).success(function (data) {
                vm.input = data;
                vm.input.closeTask = data.projectTask.isClosed;
            });

            var fetchAttachments = function () {
                appService.getTaskLogAttachments({ id: taskLogId }).success(function (data) {
                    vm.taskLogAttachments = data.items;
                });
            };

            fetchAttachments();

            vm.DeleteTasklogAttachment = function (attachment, tasklogAttachmentId) {
                abp.message.confirm(
                    app.localize('DeleteWarningMessage', attachment.fileName),
                    function (isConfirmed) {
                        if (isConfirmed) {
                            appService.deleteTaskLogAttachment({
                                id: tasklogAttachmentId
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
                vm.input.taskName = abp.session.taskName;

                appService.updateTaskLog(vm.input)
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