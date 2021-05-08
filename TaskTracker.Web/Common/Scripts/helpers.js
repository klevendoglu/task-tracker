var app = app || {};
(function () {

    var appLocalizationSource = abp.localization.getSource('TaskTracker');
    app.localize = function () {
        return appLocalizationSource.apply(this, arguments);
    };

    app.downloadTempFile = function (file) {
        location.href = abp.appPath + 'File/DownloadTempFile?fileType=' + file.fileType + '&fileToken=' + file.fileToken + '&fileName=' + file.fileName;
    };

    app.createDateRangePickerOptions = function () {
        var todayAsString = moment().format('YYYY-MM-DD');
        var options = {
            locale: {
                format: 'YYYY-MM-DD',
                applyLabel: app.localize('Apply'),
                cancelLabel: app.localize('Cancel'),
                customRangeLabel: app.localize('CustomRange')
            },
            min: '2015-05-01',
            minDate: '2015-05-01',
            max: todayAsString,
            maxDate: todayAsString,
            ranges: {}
        };

        options.ranges[app.localize('Today')] = [moment().startOf('day'), moment().endOf('day')];
        options.ranges[app.localize('Yesterday')] = [moment().subtract(1, 'days').startOf('day'), moment().subtract(1, 'days').endOf('day')];
        options.ranges[app.localize('Last7Days')] = [moment().subtract(6, 'days').startOf('day'), moment().endOf('day')];
        options.ranges[app.localize('Last30Days')] = [moment().subtract(29, 'days').startOf('day'), moment().endOf('day')];
        options.ranges[app.localize('ThisMonth')] = [moment().startOf('month'), moment().endOf('month')];
        options.ranges[app.localize('LastMonth')] = [moment().subtract(1, 'month').startOf('month'), moment().subtract(1, 'month').endOf('month')];

        return options;
    };

    app.getUserProfilePicturePath = function (profilePictureId) {
        return profilePictureId ?
                            (abp.appPath + 'Profile/GetProfilePictureById?id=' + profilePictureId) :
                            (abp.appPath + 'Common/Images/default-profile-picture.png');
    }

    app.getUserProfilePicturePath = function () {
        return abp.appPath + 'Profile/GetProfilePicture?v=' + new Date().valueOf();
    }

    app.getShownLinkedUserName = function (linkedUser) {
        if (!abp.multiTenancy.isEnabled) {
            return linkedUser.userName;
        } else {
            if (linkedUser.tenancyName) {
                return linkedUser.tenancyName + '\\' + linkedUser.username;
            } else {
                return '.\\' + linkedUser.username;
            }
        }
    }

    app.notification = app.notification || {};

    app.notification.getUiIconBySeverity = function (severity) {
        switch (severity) {
            case abp.notifications.severity.SUCCESS:
                return 'fa fa-check';
            case abp.notifications.severity.WARN:
                return 'fa fa-warning';
            case abp.notifications.severity.ERROR:
                return 'fa fa-bolt';
            case abp.notifications.severity.FATAL:
                return 'fa fa-bomb';
            case abp.notifications.severity.INFO:
            default:
                return 'fa fa-info';
        }
    };

    // custom
    app.uploadToS3 = function (uploads, uploader) {
        var attachments = [];
        var refId = Date.now();
        angular.forEach(uploads, function (file) {
            if (file && !file.$error) {
                var attachment = { fileName: file.name, refId: refId, fileFullName: refId + "_" + file.name };
                attachments.push(attachment);

                uploader.upload({
                    url: 'https://test-uploads.s3.amazonaws.com/', //S3 upload url including bucket name
                    method: 'POST',
                    fields: {
                        key: refId.toString() + "_" + file.name, // the key to store the file on S3, could be file name or customized
                        AWSAccessKeyId: 'AKIAJAVEREVFBWOUN7OA',
                        acl: 'private', // sets the access to the uploaded file in the bucket: private or public 
                        policy: 'ewogICJleHBpcmF0aW9uIjogIjIwMjAtMDEtMDFUMDA6MDA6MDBaIiwKICAiY29uZGl0aW9ucyI6IFsKICAgIHsiYnVja2V0IjogImlwZWstdXBsb2FkcyJ9LAogICAgWyJzdGFydHMtd2l0aCIsICIka2V5IiwgIiJdLAogICAgeyJhY2wiOiAicHJpdmF0ZSJ9LAogICAgWyJzdGFydHMtd2l0aCIsICIkQ29udGVudC1UeXBlIiwgIiJdLAogICAgWyJzdGFydHMtd2l0aCIsICIkZmlsZW5hbWUiLCAiIl0sCiAgICBbImNvbnRlbnQtbGVuZ3RoLXJhbmdlIiwgMCwgNTI0Mjg4MDAwXQogIF0KfQ==', // base64-encoded json policy (see article below)
                        signature: 'jT65l50lWKW7tUAKDwNrm2Qex6k=', // base64-encoded signature based on policy string (see article below)
                        "Content-Type": file.type != '' ? file.type : 'application/octet-stream', // content type of the file (NotEmpty)
                        filename: file.name // this is needed for Flash polyfill IE8-9
                    },
                    file: file
                });
            }
        });

        return attachments;
    };

    app.uploadToLocal = function (uploads, uploader) {
        var attachments = [];
        var refId = Date.now();
        angular.forEach(uploads, function (file) {
            if (file && !file.$error) {
                var attachment = { fileName: file.name, refId: refId, fileFullName: refId + "_" + file.name };
                attachments.push(attachment);

                uploader.upload({
                    url: 'handler/UploadHandler.ashx', //Local upload url
                    method: 'POST',
                    fields: {
                        'randomNumber': refId
                    },
                    file: file
                });
            }
        });

        return attachments;
    };

    app.convertToUTC = function (dt) {
        var localDate = new Date(dt);
        var localTime = localDate.getTime();
        var localOffset = localDate.getTimezoneOffset() * 60000;
        return new Date(localTime + localOffset);
    };

    app.adminUserId = 2;

    //app.directive('datepickerLocaldate', ['$parse', function ($parse) {
    //    var directive = {
    //        restrict: 'A',
    //        require: ['ngModel'],
    //        link: link
    //    };
    //    return directive;

    //    function link(scope, element, attr, ctrls) {
    //        var ngModelController = ctrls[0];

    //        // called with a JavaScript Date object when picked from the datepicker
    //        ngModelController.$parsers.push(function (viewValue) {
    //            // undo the timezone adjustment we did during the formatting
    //            viewValue.setMinutes(viewValue.getMinutes() - viewValue.getTimezoneOffset());
    //            // we just want a local date in ISO format
    //            return viewValue.toISOString().substring(0, 10);
    //        });

    //        // called with a 'yyyy-mm-dd' string to format
    //        ngModelController.$formatters.push(function (modelValue) {
    //            if (!modelValue) {
    //                return undefined;
    //            }
    //            // date constructor will apply timezone deviations from UTC (i.e. if locale is behind UTC 'dt' will be one day behind)
    //            var dt = new Date(modelValue);
    //            // 'undo' the timezone offset again (so we end up on the original date again)
    //            dt.setMinutes(dt.getMinutes() + dt.getTimezoneOffset());
    //            return dt;
    //        });
    //    }
    //}]);

})();