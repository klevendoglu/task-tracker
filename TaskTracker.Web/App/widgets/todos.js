(function () {
    appModule.controller('widgets.todos', [
        '$scope', '$uibModal', 'abp.services.app.projectManagement',
        function ($scope, $modal, appService) {
            var vm = this;
            var userId = abp.session.userId;

            vm.getIncompleteToDos = function () {
                vm.loading = true;
                appService.getToDos({ creatorUserId: userId }).success(function (data) {
                    vm.incompleteToDos = data.items;
                }).finally(function () {
                    vm.loading = false;
                });
            };

            vm.getCompleteToDos = function () {
                vm.loading = true;
                appService.getToDos({ creatorUserId: userId, isComplete: true }).success(function (data) {
                    vm.completeToDos = data.items;
                }).finally(function () {
                    vm.loading = false;
                });
            };

            vm.getIncompleteToDos();
            vm.getCompleteToDos();

            vm.Create = function () {
                var modalInstance = $modal.open({
                    // size: 'lg', // 'sm' for small modal
                    templateUrl: '/App/projectmanagement/views/todos/creapdate.cshtml',
                    controller: 'todos.views.creapdate as vm',
                    backdrop: 'static'
                });

                modalInstance.result.then(function (result) {
                    vm.getIncompleteToDos();
                });
            };

            vm.Update = function (itemId) {
                abp.session.todoId = itemId;
                var modalInstance = $modal.open({
                    // size: 'lg', // 'sm' for small modal
                    templateUrl: '/App/projectmanagement/views/todos/creapdate.cshtml',
                    controller: 'todos.views.creapdate as vm',
                    backdrop: 'static'
                });

                modalInstance.result.then(function (result) {
                    vm.getIncompleteToDos();
                    abp.session.todoId = null;
                });
            }

            vm.Delete = function (item) {
                abp.message.confirm(
                    app.localize('DeleteWarningMessage', item.title),
                    function (isConfirmed) {
                        if (isConfirmed) {
                            appService.deleteToDo({
                                id: item.id
                            }).success(function () {
                                vm.getIncompleteToDos();
                                vm.getCompleteToDos();
                                abp.notify.success(app.localize('SuccessfullyDeleted'));
                            });
                        }
                    }
                );
            };

            vm.MarkComplete = function (item) {
                abp.message.confirm(
                    app.localize('MarkCompleteConfirmation', item.title),
                    function (isConfirmed) {
                        if (isConfirmed) {
                            appService.markToDoComplete({
                                id: item.id
                            }).success(function() {
                                vm.getIncompleteToDos();
                                vm.getCompleteToDos();
                                abp.notify.success(app.localize('ActionSuccessfullyCompleted'));
                            });
                        }
                    }
                );
            };
        }
    ]);
})();