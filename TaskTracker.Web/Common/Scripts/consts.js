var app = app || {};
(function () {

    $.extend(app, {
        consts: {
            grid: {
                defaultPageSize: 10,
                defaultPageSizes: [10, 20, 50, 100]
            },
            userManagement: {
                defaultAdminUserName: 'admin',
                defaultAdminUserId: 2
            },
            contentTypes: {
                formUrlencoded: 'application/x-www-form-urlencoded; charset=UTF-8'
            }
        }
    });

})();