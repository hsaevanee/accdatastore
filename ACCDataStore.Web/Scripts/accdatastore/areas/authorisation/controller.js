angular.module('root.controllers', ['ngSanitize'])

.controller('rootCtrl', function ($scope, $rootScope) {
    $rootScope.pageTitle = "User Settings";
})

.controller('indexCtrl', function ($scope, $rootScope, indexService) {

        $scope.mIndex = {};

        $scope.doLogin = function () {

            $scope.bShowError = false;

            indexService.login($scope.mIndex).then(function (response) {

                $rootScope.bShowLoading = false;

                if (response.data.IsRedirect) {

                    window.location.href = response.data.RedirectUrl;

                } else {

                    $scope.bShowError = true;

                    $scope.errorType = response.data.ErrorType;

                    $scope.errorMessage = response.data.ErrorMessage;

                }

            }, function (response) {

                $rootScope.bShowLoading = false;

            });

        };

    })

.controller('listCtrl', function ($scope, $rootScope, $state, $stateParams, userSettingsService) {
    $rootScope.pageTitle = "List User";
    $scope.mList = {};

    userSettingsService.getAll().then(function (response) {
        $scope.mList = response.data;
    }, function (response) {
    });

    $scope.doAdd = function () {
        $state.go('add');
    }

    $scope.doView = function (eUser) {
        $state.go('view', { nID: eUser.ID });
    }

    $scope.doEdit = function (eUser) {
        $state.go('edit', { nID: eUser.ID });
    }

    $scope.doDelete = function (eUser) {
        if (confirm("Confirm delete ?")) {
            userSettingsService.delete(eUser.ID).then(function (response) {
                $scope.mList = response.data;
            }, function (response) {
            });
        }
    }
})

.controller('addCtrl', function ($scope, $rootScope, $state, $stateParams, userSettingsService) {
    $rootScope.pageTitle = "Add User";
    $scope.mAddEditView = {};

    $scope.isShowSaveButton = true;

    userSettingsService.getDefault().then(function (response) {
        $scope.mAddEditView = response.data;
    }, function (response) {
    });

    $scope.doSave = function (eUser) {
        userSettingsService.save(eUser).then(function (response) {
            $state.go('list');
        }, function (response) {
        });
    }

    $scope.doCancel = function () {
        $state.go('list');
    }
})

.controller('editCtrl', function ($scope, $rootScope, $state, $stateParams, userSettingsService) {
    $rootScope.pageTitle = "Edit User";
    $scope.mAddEditView = {};

    $scope.isShowSaveButton = true;

    userSettingsService.getByID($stateParams.nID).then(function (response) {
        $scope.mAddEditView = response.data;
    }, function (response) {
    });

    $scope.doSave = function (eUser) {
        userSettingsService.save(eUser).then(function (response) {
            $state.go('list');
        }, function (response) {
        });
    }

    $scope.doCancel = function () {
        $state.go('list');
    }
})

.controller('viewCtrl', function ($scope, $rootScope, $state, $stateParams, userSettingsService) {
    $rootScope.pageTitle = "View User";
    $scope.mAddEditView = {};

    $scope.isShowSaveButton = false;

    userSettingsService.getByID($stateParams.nID).then(function (response) {
        $scope.mAddEditView = response.data;
    }, function (response) {
    });

    $scope.doCancel = function () {
        $state.go('list');
    }
});

