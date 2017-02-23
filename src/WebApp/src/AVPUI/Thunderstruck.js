var app = angular.module('AVPApp', []);
app.controller('AVPController'
, function ($scope, $http) {
    /************************************************************/
    /*                    Properties
    /************************************************************/
    //self-explanatory
    $scope.username = "";
    $scope.isAuthenticated = false;
    $scope.currentRole;

    //isRegistering used on UserProfile page to trigger new registration presentation and logic
    $scope.isRegistering = false;

    //Enumeration of possible pages
    $scope.pages = { Dashboard: "Dashboard", Incidents: "Incidents", UserProfile: "UserProfile" };

    //currentPage has name of authenticated page current displayed in body
    $scope.currentPage = $scope.pages.UserProfile;

    /************************************************************/
    /*                  Methods
    /************************************************************/
    //User Management Methods
    $scope.Login = function () {
        $scope.isAuthenticated = true;
        $scope.roles = $scope.GetRoles();
        //default role to Administrator index=id-1
        $scope.currentRole = $scope.roles[5];
    }
    $scope.GetRoles = function () {
        return [{ roleid: 1, rolename: "Sender" }
        , { roleid: 2, rolename: "Receiver" }
        , { roleid: 3, rolename: "Analyst" }
        , { roleid: 4, rolename: "Notifier" }
        , { roleid: 5, rolename: "Analyst" }
        , { roleid: 6, rolename: "Administrator" }];
    }
    $scope.Register = function () {

    }
    $scope.SelectRole = function (role) {
        $scope.currentRole = role;
    }
    $scope.ShowSelectRole = function () {
        $('#AVPModal').modal('show');
    }
    $scope.HideSelectRole = function () {
        $('#AVPModal').modal('hide');
    }
    //Navigation Methods
    $scope.ShowDashboard = function () {
        $scope.currentPage = $scope.pages.Dashboard;
    }
    $scope.ShowIncidents = function () {
        $scope.currentPage = $scope.pages.Incidents;
    }
    $scope.ShowUserProfile = function () {
        $scope.currentPage = $scope.pages.UserProfile;
    }
});