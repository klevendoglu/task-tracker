(function () {
    appModule.controller('projectmanagement.views.agent.index', [
        '$scope', '$state', '$uibModal', '$stateParams', 'uiGridConstants', 'abp.services.app.projectManagement', '$filter',
        function ($scope, $state, $modal, $stateParams, uiGridConstants, appService, $filter) {
            var vm = this;
            var userId = abp.session.userId;

            $scope.$on('$viewContentLoaded', function () {
                App.initAjax();
            });

            vm.loading = false;
            vm.openOnly = false;
            var requestParams = {
                skipCount: 0,
                maxResultCount: app.consts.grid.defaultPageSize,
                sorting: null,
                openOnly: false
            };

            vm.isSuperAdmin = abp.auth.hasPermission('Pages.Superadministration');


            // View model functions
            var fetchGridData = function () {
                if (vm.isSuperAdmin) {
                    vm.loading = true;                   
                    appService.getTasksWithLogs({
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
                    appService.getTasksWithLogs({
                        openOnly: vm.openOnly,
                        filter: vm.filterText,
                        agentId:userId,
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
                abp.session.taskId = entity.id;
                $modal.open({
                    size: 'lg', // 'sm' for small modal
                    templateUrl: '/App/projectmanagement/views/shared/viewtask.cshtml',
                    controller: 'projectmanagement.views.shared.viewtask as vm'
                });
            }



            vm.CreateLog = function (item) {
                abp.session.taskId = item.id;
                abp.session.projectId = item.projectId;
                var modalInstance = $modal.open({
                    size: 'lg', // 'sm' for small modal
                    templateUrl: '/App/projectmanagement/views/agent/tasklog/create.cshtml',
                    controller: 'projectmanagement.views.agent.tasklog.create as vm',
                    backdrop: 'static'
                });

                modalInstance.result.then(function (result) {
                    fetchGridData();
                });
            }

            vm.ViewLogs = function (item) {
                abp.session.taskId = item.id;
                var modalInstance = $modal.open({
                    size: 'lg', // 'sm' for small modal
                    templateUrl: '/App/projectmanagement/views/agent/tasklog/index.cshtml',
                    controller: 'projectmanagement.views.agent.tasklog.index as vm',
                    backdrop: 'static'
                });

                modalInstance.result.then(function (result) {
                    fetchGridData();
                });
            }



            vm.Filter = function () {
                fetchGridData();
            }

            // Default grid options
            vm.gridOptions = {
                enableHorizontalScrollbar: uiGridConstants.scrollbars.NEVER,
                enableVerticalScrollbar: uiGridConstants.scrollbars.WHEN_NEEDED,
                paginationPageSizes: app.consts.grid.defaultPageSizes,
                paginationPageSize: app.consts.grid.defaultPageSize,
                enableRowSelection: false,
                enableSelectAll: false,
                useExternalPagination: true,
                useExternalSorting: true,
                appScopeProvider: vm,

                rowTemplate: '<div ng-repeat="(colRenderIndex, col) in colContainer.renderedColumns track by col.colDef.name" class="ui-grid-cell" ng-class="{ \'ui-grid-row-header-cell\': col.isRowHeader, \'text-muted\': !row.entity.isActive }"  ui-grid-cell></div>',
                columnDefs: [
                    {
                        name: '',
                        field: 'id',
                        width: 25,
                        cellTemplate: '<div class="ui-grid-cell-contents"><a href="#" ng-click="grid.appScope.View(row.entity)" href="javascript:;" title="{{COL_FIELD}}" > <i class="icon-info"></i> </a> </div>'
                    },
                    {
                        name: app.localize('Projects'),
                        field: 'project.name',
                        width: 300
                    },
                    {
                        name: app.localize('Tasks'),
                        field: 'name',
                        width: 300
                    },
                    {
                        name: app.localize('Creator'),
                        field: 'creatorUser.surname',
                        width: 110
                    },
                    {
                        name: app.localize('ManagerNotes'),
                        field: 'description',
                        width: 140
                    },
                    {
                        name: app.localize('AssignTime'),
                        field: 'assignTime',
                        cellFilter: 'momentFormat: \'lll\'',
                        width: 150
                    },
                    {
                        name: app.localize('EstimatedDays'),
                        field: 'estimatedDays',
                        width: 120
                    },
                    {
                        name: app.localize('OverDue'),
                        field: 'overDue',
                        width: 120,
                        cellTemplate: '<div class="ui-grid-cell-contents"> <span class="label label-sm label-success" ng-class="{ \'label-danger\': row.entity.overDue > 0 , \'label-warning\': row.entity.overDue == 0 || row.entity.overDue == -1 }"> {{COL_FIELD}}</span></div>'
                    },
                    {
                        name: app.localize('TaskLogCount'),
                        field: 'taskLogCount',
                        width: 100,
                        cellTemplate: '<div class="ui-grid-cell-contents"> <a class="badge" ng-click="grid.appScope.ViewLogs(row.entity)" href="javascript:;" title=" ' + app.localize('TaskLogCount') + '"> {{COL_FIELD}}</a> </div>'
                    },
                    {
                        name: app.localize('Status'),
                        field: 'statusText',
                        width: 80,
                        cellTemplate: '<div class="ui-grid-cell-contents"> <span class="label label-sm label-success" ng-class="{ \'label-danger\': row.entity.isOpen }"> ' + app.localize('{{COL_FIELD}}') + '</span></div>'
                    },
                     {
                         name: app.localize('Actions'),
                         enableSorting: false,
                         width: 120,
                         cellTemplate:
                             '<div class=\"ui-grid-cell-contents\">' +
                             '  <div class="btn-group dropdown" uib-dropdown="">' +
                             '    <button class="btn btn-xs btn-primary blue" uib-dropdown-toggle="" aria-haspopup="true" aria-expanded="false"><i class="fa fa-cog"></i> ' + app.localize('Actions') + ' <span class="caret"></span></button>' +
                             '    <ul uib-dropdown-menu>' +
                             '      <li><a ng-click="grid.appScope.CreateLog(row.entity)">' + app.localize('CreateLog') + '</a></li>' +
                             '      <li><a ng-click="grid.appScope.View(row.entity)">' + app.localize('View') + '</a></li>' +
                             '    </ul>' +
                             '  </div>' +
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