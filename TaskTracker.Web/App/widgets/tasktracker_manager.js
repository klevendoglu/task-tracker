(function () {
    appModule.controller('widgets.tasktracker.manager', [
        '$scope', '$uibModal', 'uiGridConstants', 'abp.services.app.projectManagement',
        function ($scope, $modal, uiGridConstants, appService) {
            var vm = this;
            var userId = abp.session.userId;

            $scope.$on('$viewContentLoaded', function () {
                App.initAjax();
            });

            vm.openOnly = false;
            vm.isSuperAdmin = abp.auth.hasPermission('Pages.Superadministration');
            vm.isAdminAccount = userId == app.adminUserId;

            vm.loading = false;
            var requestParams = {
                skipCount: 0,
                maxResultCount: app.consts.grid.defaultPageSize,
                sorting: null
            };

            // View model functions
            var fetchGridData = function () {
                if (vm.isSuperAdmin) {
                    vm.loading = true;
                    appService.getProjectWithManagerAndTaskCounts({
                        openOnly: vm.openOnly,
                        filter: vm.filterText,
                        skipCount: requestParams.skipCount,
                        maxResultCount: requestParams.maxResultCount,
                        sorting: requestParams.sorting
                    }).success(function (data) {
                        vm.gridOptions.totalItems = data.totalCount;
                        vm.gridOptions.data = data.items;
                    }).finally(function () {
                        vm.loading = false;
                    });
                }
                else {
                    vm.loading = true;
                    appService.getProjectWithManagerAndTaskCounts({
                        openOnly: vm.openOnly,
                        managerId: userId,
                        filter: vm.filterText,
                        skipCount: requestParams.skipCount,
                        maxResultCount: requestParams.maxResultCount,
                        sorting: requestParams.sorting
                    }).success(function (data) {
                        vm.gridOptions.totalItems = data.totalCount;
                        vm.gridOptions.data = data.items;
                    }).finally(function () {
                        vm.loading = false;
                    });
                }

            };

            // fetch grid data
            fetchGridData();


            vm.View = function (entity) {
                abp.session.projectId = entity.id;
                $modal.open({
                    size: 'lg', // 'sm' for small modal
                    templateUrl: '/App/projectmanagement/views/manager/project/view.cshtml',
                    controller: 'projectmanagement.views.manager.view as vm'
                });
            }

            vm.Create = function () {
                var modalInstance = $modal.open({
                    size: 'lg', // 'sm' for small modal
                    templateUrl: '/App/projectmanagement/views/manager/project/create.cshtml',
                    controller: 'projectmanagement.views.manager.project.create as vm',
                    backdrop: 'static'
                });

                modalInstance.result.then(function (result) {
                    //location.reload();
                    fetchGridData();
                });
            }

            vm.Update = function (item) {
                abp.session.projectId = item.id;
                var modalInstance = $modal.open({
                    size: 'lg', // 'sm' for small modal
                    templateUrl: '/App/projectmanagement/views/manager/project/update.cshtml',
                    controller: 'projectmanagement.views.manager.project.update as vm',
                    backdrop: 'static'
                });

                modalInstance.result.then(function (result) {
                    fetchGridData();
                });
            }

            vm.Delete = function (item) {
                abp.message.confirm(
                    app.localize('DeleteWarningMessage', item.name),
                    function (isConfirmed) {
                        if (isConfirmed) {
                            appService.deleteProject({
                                id: item.id
                            }).success(function () {
                                fetchGridData();
                                abp.notify.success(app.localize('SuccessfullyDeleted'));
                            });
                        }
                    }
                );
            };

            vm.CreateTask = function (item) {
                abp.session.projectId = item.id;
                var modalInstance = $modal.open({
                    size: 'lg', // 'sm' for small modal
                    templateUrl: '/App/projectmanagement/views/manager/task/create.cshtml',
                    controller: 'projectmanagement.views.manager.task.create as vm',
                    backdrop: 'static'
                });

                modalInstance.result.then(function (result) {
                    fetchGridData();
                });
            }

            vm.ViewTasks = function (item) {
                abp.session.projectId = item.id;
                var modalInstance = $modal.open({
                    size: 'lg', // 'sm' for small modal
                    templateUrl: '/App/projectmanagement/views/manager/task/index.cshtml',
                    controller: 'projectmanagement.views.manager.task.index as vm',
                    backdrop: 'static'
                });

                modalInstance.result.then(function (result) {
                    fetchGridData();
                });
            }

            vm.Filter = function () {
                fetchGridData();
            }

            vm.Refresh = function () {
                fetchGridData();
            };

            // Default grid options
            vm.gridOptions = {
                enableHorizontalScrollbar: uiGridConstants.scrollbars.WHEN_NEEDED,
                enableVerticalScrollbar: uiGridConstants.scrollbars.WHEN_NEEDED,
                paginationPageSizes: app.consts.grid.defaultPageSizes,
                paginationPageSize: app.consts.grid.defaultPageSize,
                useExternalPagination: true,
                useExternalSorting: true,
                appScopeProvider: vm,

                rowTemplate: '<div ng-repeat="(colRenderIndex, col) in colContainer.renderedColumns track by col.colDef.name" class="ui-grid-cell" ng-class="{ \'ui-grid-row-header-cell\': col.isRowHeader, \'text-muted\': !row.entity.isActive }"  ui-grid-cell></div>',
                columnDefs: [
                    {
                        name: app.localize('Queue'),
                        field: 'id',
                        width: 55
                        //cellTemplate: '<div class="ui-grid-cell-contents"><a href="#" ng-click="grid.appScope.View(row.entity)" href="javascript:;" title="{{COL_FIELD}}" > <i class="icon-info"></i> </a> </div>'
                    },
                    {
                        name: app.localize('Name'),
                        field: 'name',
                        width: 520,
                    },
                    {
                        name: app.localize('Creator'),
                        field: 'ownersSurname',
                        width: 120,
                    },
                    {
                        name: app.localize('Manager'),
                        field: 'managersText',
                        enableSorting: false,
                        width: 140,
                    },
                    //{
                    //    name: app.localize('SubTaskCount'),
                    //    field: 'taskCount',
                    //    width: 80,
                    //    cellTemplate: '<div class="ui-grid-cell-contents"> <a class="badge" ng-click="grid.appScope.ViewTasks(row.entity)" href="javascript:;" title=" ' + app.localize('TaskLogCount') + '"> {{COL_FIELD}}</a> </div>'
                    //},
                    {
                        name: app.localize('StartTime'),
                        field: 'startTime',
                        cellFilter: 'momentFormat: \'lll\'',
                        width: 150
                    },
                    {
                        name: app.localize('EndTime'),
                        field: 'endTime',
                        cellFilter: 'momentFormat: \'lll\'',
                        width: 140
                    },
                    {
                        name: app.localize('Status'),
                        field: 'statusText',
                        enableSorting: false,
                        width: 75,
                        cellTemplate: '<div class="ui-grid-cell-contents"> <span class="label label-sm label-success" ng-class="{ \'label-danger\': row.entity.isOpen }"> ' + app.localize('{{COL_FIELD}}') + '</span></div>'
                    },
                    {
                        name: app.localize('Actions'),
                        enableSorting: false,
                        width: 340,
                        cellTemplate:
                            '<div class=\"ui-grid-cell-contents\">' +
                            '  <button ng-click="grid.appScope.ViewTasks(row.entity)" class="btn btn-xs btn-primary" ng-class="{ \'green-meadow\': row.entity.isTaskCountGreaterThanZero, \'purple\' : !row.entity.isTaskCountGreaterThanZero }" aria-expanded="false"><i class="fa fa-eye"></i>' + app.localize('SubTasks') + ' </button>' +
                            '  <button ng-click="grid.appScope.Update(row.entity)" class="btn btn-xs btn-primary green" aria-expanded="false"><i class="fa fa-pencil"></i>' + app.localize('Update') + ' </button>' +
                            '  <button ng-click="grid.appScope.View(row.entity)" class="btn btn-xs btn-primary" aria-expanded="false"><i class="fa fa-commenting-o"></i>' + app.localize('Details') + ' </button>' +
                            '  <button ng-click="grid.appScope.Delete(row.entity)" ng-if="row.entity.creatorUserId==' + userId + ' || ' + vm.isAdminAccount + '" class="btn btn-xs btn-primary red-thunderbird" aria-expanded="false"><i class="fa fa-trash-o"></i>' + app.localize('Delete') + ' </button>' +
                            '</div>'
                    },
                ],
                onRegisterApi: function (gridApi) {
                    $scope.gridApi = gridApi;
                    $scope.gridApi.core.on.sortChanged($scope, function (grid, sortColumns) {
                        if (!sortColumns.length || !sortColumns[0].field) {
                            requestParams.sorting = null;
                        } else {
                            requestParams.sorting = sortColumns[0].field + ' ' + sortColumns[0].sort.direction;
                        }

                        fetchGridData();
                    });
                    gridApi.pagination.on.paginationChanged($scope, function (pageNumber, pageSize) {
                        requestParams.skipCount = (pageNumber - 1) * pageSize;
                        requestParams.maxResultCount = pageSize;
                        fetchGridData();
                    });
                },
                data: []
            };
        }]);
})();