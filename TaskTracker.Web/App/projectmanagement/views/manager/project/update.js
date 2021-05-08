(function () {
    appModule.controller('projectmanagement.views.manager.project.update', [
        '$scope', '$uibModalInstance', 'Upload', 'abp.services.app.projectManagement', '$sce', '$filter',
        function ($scope, $modalInstance, uploader, appService, $sce, $filter) {
            var vm = this;

            vm.manager = {};
            vm.managers = [];
            vm.manager.selected = [];
            vm.input = { };

            var projectId = abp.session.projectId;
            appService.getProject({ id: projectId }).success(function (data) {
                vm.input = data;
                vm.input.closeProject = data.isClosed;
                vm.input.isSystemAdmin = abp.session.userId == app.adminUserId;
            });

            appService.getProjectManagers({}).success(function (data) {
                vm.managers = data.items;
            });

            appService.getSelectedProjectManagers({ id: projectId }).success(function (result) {
                vm.manager.selected = result.items;
            });

            var fetchAttachments = function () {
                appService.getProjectAttachments({ id: projectId }).success(function (data) {
                    vm.projectAttachments = data.items;
                });
            };

            fetchAttachments();

            vm.DeleteProjectAttachment = function (attachment, projectAttachmentId) {
                abp.message.confirm(
                    app.localize('DeleteWarningMessage', attachment.fileName),
                    function (isConfirmed) {
                        if (isConfirmed) {
                            appService.deleteProjectAttachment({
                                id: projectAttachmentId
                            }).success(function () {
                                fetchAttachments();
                                abp.notify.success(app.localize('SuccessfullyDeleted'));
                            });
                        }
                    }
                );
            };

            vm.Save = function (uploads) {
                if (vm.manager.selected == null || !vm.manager.selected.length) {
                    abp.message.error(app.localize('SelectAtLeastOneManager'), app.localize('ManagerIsRequired'));
                    return;
                }
                vm.input.attachments = app.uploadToLocal(uploads, uploader);
                vm.input.selectedManagers = vm.manager.selected;
                vm.saving = true;
                appService.updateProject(vm.input)
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