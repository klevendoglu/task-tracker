(function ($) {
    $(function () {
        var _hostSettingsService = abp.services.app.hostSettings;
        var _initialTimeZone = $('#GeneralSettingsForm [name=Timezone]').val();
        var _usingDefaultTimeZone = $('#GeneralSettingsForm [name=TimezoneForComparison]').val() === abp.setting.values["Abp.Timing.TimeZone"];
        
        var _$tabPanel = $('#SettingsTabPanel');

        var _$smtpCredentialFormGroups = _$tabPanel
            .find('input[name=SmtpDomain],input[name=SmtpUserName],input[name=SmtpPassword]')
            .closest('.form-group');

        var _$tenantSettingsCheckboxes = _$tabPanel
           .find('input[name=IsNewRegisteredTenantActiveByDefault],input[name=UseCaptchaOnRegistration]')
           .closest('.md-checkbox');

        function toggleSmtpCredentialFormGroups() {
            if ($('#Settings_SmtpUseDefaultCredentials').is(':checked')) {
                _$smtpCredentialFormGroups.slideUp('fast');
            } else {
                _$smtpCredentialFormGroups.slideDown('fast');
            }
        }

        function toggleTenantManagementFormGroups() {
            if (!$('#Setting_AllowSelfRegistration').is(':checked')) {
                _$tenantSettingsCheckboxes.slideUp('fast');
            } else {
                _$tenantSettingsCheckboxes.slideDown('fast');
            }
        }

        toggleSmtpCredentialFormGroups();
        toggleTenantManagementFormGroups();

        $('#Settings_SmtpUseDefaultCredentials').change(function () {
            toggleSmtpCredentialFormGroups();
        });

        $('#Setting_AllowSelfRegistration').change(function () {
            toggleTenantManagementFormGroups();
        });

        $('#SaveAllSettingsButton').click(function () {
            _hostSettingsService.updateAllSettings({
                general: $('#GeneralSettingsForm').serializeFormToObject(),
                tenantManagement: $('#TenantManagementSettingsForm').serializeFormToObject(),
                userManagement: $('#UserManagementSettingsForm').serializeFormToObject(),
                email: $('#EmailSmtpSettingsForm').serializeFormToObject()
            }).done(function () {
                abp.notify.info(app.localize('SavedSuccessfully'));

                var newTimezone = $('#GeneralSettingsForm [name=Timezone]').val();
                if (abp.clock.provider.supportsMultipleTimezone &&
                        _usingDefaultTimeZone &&
                        _initialTimeZone !== newTimezone) {
                    abp.message.info(app.localize('TimeZoneSettingChangedRefreshPageNotification')).done(function () {
                        window.location.reload();
                    });
                }
            });
        });

        $('#SendTestEmailButton').click(function () {
            _hostSettingsService.sendTestEmail({
                emailAddress: $('#TestEmailAddressInput').val()
            }).done(function () {
                abp.notify.info(app.localize('TestEmailSentSuccessfully'));
            });
        });
    });
})(jQuery);