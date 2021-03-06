(function () {
    appModule.controller('host.views.settings.index', [
        '$scope', 'appSession', 'abp.services.app.hostSettings', 'abp.services.app.commonLookup',
        function ($scope, appSession, hostSettingsService, commonLookupService) {
            var vm = this;
            var usingDefaultTimeZone = false;
            var initialTimeZone = null;

            $scope.$on('$viewContentLoaded', function () {
                App.initAjax();
            });

            vm.loading = false;
            vm.settings = null;
            vm.editions = [];
            vm.testEmailAddress = appSession.user.emailAddress;
            vm.showTimezoneSelection = abp.clock.provider.supportsMultipleTimezone;

            vm.getSettings = function () {
                vm.loading = true;
                hostSettingsService.getAllSettings()
                    .success(function (result) {
                        vm.settings = result;
                        initialTimeZone = vm.settings.general.timezone;
                        usingDefaultTimeZone = vm.settings.general.timezoneForComparison === abp.setting.values["Abp.Timing.TimeZone"];
                    }).finally(function () {
                        vm.loading = false;
                    });
            };

            vm.saveAll = function () {
                hostSettingsService.updateAllSettings(
                    vm.settings
                ).success(function () {
                    abp.notify.info(app.localize('SavedSuccessfully'));

                    if (abp.clock.provider.supportsMultipleTimezone && usingDefaultTimeZone && initialTimeZone !== vm.settings.general.timezone) {
                        abp.message.info(app.localize('TimeZoneSettingChangedRefreshPageNotification')).done(function () {
                            window.location.reload();
                        });
                    }
                });
            };

            vm.sendTestEmail = function () {
                hostSettingsService.sendTestEmail({
                    emailAddress: vm.testEmailAddress
                }).success(function () {
                    abp.notify.info(app.localize('TestEmailSentSuccessfully'));
                });
            };

            vm.getEditionValue = function (item) {
                if (item.value) {
                    return parseInt(item.value);
                }
                return item.value;
            };

            vm.getEditions = function () {
                commonLookupService.getEditionsForCombobox({}).success(function (result) {
                    vm.editions = result.items;
                    vm.editions.unshift({ value: null, displayText: app.localize('NotAssigned') });
                });
            };

            vm.init = function () {
                vm.getEditions();
                vm.getSettings();
            }

            vm.init();
        }
    ]);
})();