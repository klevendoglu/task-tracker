(function () {
    appModule.controller('todos.views.creapdate', [
        '$scope', '$uibModalInstance', 'Upload', 'abp.services.app.projectManagement',
        function ($scope, $modalInstance, uploader, appService) {
            var vm = this;
            var todoId = abp.session.todoId;
            vm.input = { title:"" };

            if (todoId > 0) {
                appService.getToDo({ id: todoId }).success(function (data) {
                    vm.input = data;
                });
            }

            vm.Save = function () {
                vm.saving = true;
                appService.creapdateToDo(vm.input)
                    .success(function () {
                        abp.notify.info(app.localize('SavedSuccessfully'));
                        $modalInstance.close();
                    }).finally(function () {
                        vm.saving = false;
                    });
            };

            vm.Cancel = function () {
                $modalInstance.dismiss();
            };
        }
    ]);
})();