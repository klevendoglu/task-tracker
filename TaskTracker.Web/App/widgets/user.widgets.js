
appModule.controller('user.widget.manager', [
   '$scope', '$state', '$uibModal', 'abp.services.app.widgets', function (
    $scope, $state, $modal, appService) {
       /*Private Members*/
       var vm = this;

       /*public members*/
       vm.loading = false;
       vm.userWidgets = [];
       vm.widgets = [];

       /* Private methods */
       var loadDetails = function () {
           vm.loading = true;
           var input = {
               id: abp.session.userId
           };

           appService.getWidgets(input)
               .success(function (data) {
                   vm.widgets = data;
               })
               .error(function (data, status) {
                   abp.message.error(app.localize(data.message));
               })
               .finally(function () {
                   vm.loading = false;
               });

           appService.getUserWidgetIds(input)
                    .success(function (data) {
                        vm.userWidgets = data.widgetIds;
                    })
                    .error(function (data, status) {
                        abp.message.error(app.localize(data.message));
                    })
                    .finally(function () {

                    });
       };

       /* Public methods */
       vm.save = function () {
           var widgetIds = [];
           for (var i = 0; i < vm.widgets.length; i++) {
               if (vm.widgets[i].isSelected)
                   widgetIds.push(vm.widgets[i].id);
           }
           var input = {
               userId: abp.session.userId,
               widgetIds: widgetIds
           };

           appService.createUserWidgets(input)
              .success(function (data) {
                  abp.notify.info(app.localize('SavedSuccessfully'));
              })
              .error(function (data, status) {
                  abp.message.error(app.localize(data.message));
              })
              .finally(function () {
                  vm.loading = false;
                  loadDetails(); 
              });
       };

       vm.cancel = function () {
           $modalInstance.dismiss();
       }
       
       vm.showWidget = function (widgetId) {
           return vm.userWidgets.indexOf(widgetId) > -1;
       }

       vm.Localize = function (toLocalize) {
           return app.localize(toLocalize);
       };

       /*Constructor*/
       function init() {
           loadDetails();
       };

       init();
   }]);