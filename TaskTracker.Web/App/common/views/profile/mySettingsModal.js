(function () {
    appModule.controller('common.views.profile.mySettingsModal', [
        '$scope', 'appSession', '$uibModalInstance', 'abp.services.app.profile', 'abp.services.app.widgets',
        function ($scope, appSession, $uibModalInstance, profileService, widgetsService) {
            var vm = this;

            var initialTimezone = null;

            vm.saving = false;
            vm.user = null;
            vm.canChangeUserName = true;
            vm.showTimezoneSelection = abp.clock.provider.supportsMultipleTimezone;

            vm.save = function () {
                vm.saving = true;
                profileService.updateCurrentUserProfile(vm.user)
                    .success(function () {
                        appSession.user.name = vm.user.name;
                        appSession.user.surname = vm.user.surname;
                        appSession.user.userName = vm.user.userName;
                        appSession.user.emailAddress = vm.user.emailAddress;

                        abp.notify.info(app.localize('SavedSuccessfully'));

                        $uibModalInstance.close();
                        
                        if (abp.clock.provider.supportsMultipleTimezone && initialTimezone !== vm.user.timezone) {
                            abp.message.info(app.localize('TimeZoneSettingChangedRefreshPageNotification')).done(function() {
                                window.location.reload();
                            });
                        }

                    }).finally(function () {
                        vm.saving = false;
                    });
            };

            vm.cancel = function () {
                $uibModalInstance.dismiss();
                window.location.reload();
            };

            // custom
            vm.saveWidgetSettings = function () {
                var widgetIds = [];
                for (var i = 0; i < vm.widgets.length; i++) {
                    if (vm.widgets[i].isSelected)
                        widgetIds.push(vm.widgets[i].id);
                }
                var input = {
                    userId: abp.session.userId,
                    widgetIds: widgetIds
                };

                widgetsService.createUserWidgets(input)
                   .success(function (data) {
                       abp.notify.info(app.localize('SavedSuccessfully'));
                   })
                   .error(function (data, status) {
                       abp.message.error(app.localize(data.message));
                   })
                   .finally(function () {
                       vm.loading = false;
                       $uibModalInstance.close();
                   });
            };

            function init() {
                profileService.getCurrentUserProfileForEdit({
                    id: appSession.user.id
                }).success(function (result) {
                    vm.user = result;
                    vm.canChangeUserName = vm.user.userName != app.consts.userManagement.defaultAdminUserName;
                    initialTimezone = vm.user.timezone;
                });
            }

            // custom

            widgetsService.getWidgets({
                id: appSession.user.id
            }).success(function (data) {
                vm.widgets = data;
            });

            init();
        }
    ]);
})();