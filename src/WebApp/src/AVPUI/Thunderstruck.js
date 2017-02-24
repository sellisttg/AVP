var app = angular.module('AVPApp', []);
app.controller('AVPController'
, function ($scope, $http) {
    /************************************************************/
    /*                    Properties
    /************************************************************/
    //constants
    $scope.baseUrl = "http://localhost:57123/api";

    //self-explanatory
    $scope.isAuthenticated = false;
    $scope.currentRole;

    //isRegistering used on UserProfile page to trigger new registration presentation and logic
    $scope.isRegistering = false;

    //Enumeration of possible pages
    $scope.pages = { Dashboard: "Dashboard", Incidents: "Incidents", UserProfile: "UserProfile" };

    //currentPage has name of authenticated page current displayed in body
    $scope.currentPage = $scope.pages.UserProfile;
    //UserProfile
    $scope.userProfile = {
        authToken: ""
        , username: ""
        , password: ""
        , confirmPassword: ""
        , OptIn: { optInEmail: true, optInSMS: true, optInPush: true }
        , address: { streetAddress: "", zipCode: "" }
        , emailAddress: ""
        , phoneNumber: ""
        , pushToken: ""
    };
    /************************************************************/
    /*                  Methods
    /************************************************************/
    //User Management Methods
    $scope.ShowRegister = function() {
        $scope.isRegistering = true;
    }
    $scope.Login = function () {
        var url = $scope.baseUrl + "/v1/sessions";
        var postdata = { UserName: $scope.userProfile.username, password: $scope.userProfile.password };
        $http.post(url, postdata).then(
            function (response) {
                $scope.authToken = response.data.daccess_token;
                $scope.isAuthenticated = true;
                //get list of roles
                $scope.roles = $scope.GetRoles();
                //default role to Administrator index=id-1
                $scope.currentRole = $scope.roles[4];
            });
        /*
        $scope.isAuthenticated = true;
        $scope.roles = $scope.GetRoles();
        //default role to Administrator index=id-1
        $scope.currentRole = $scope.roles[4];
        */
    }
    $scope.GetRoles = function () {
        return [{ roleid: 1, rolename: "Sender" }
        , { roleid: 2, rolename: "Receiver" }
        , { roleid: 3, rolename: "Monitor" }
        , { roleid: 4, rolename: "Analyst" }
        , { roleid: 5, rolename: "Administrator" }];
    }
    $scope.Register = function () {
        //{UserName: "sellis7", password: "abc", emailoptin: true, smsoptin: true, pushoptin: true }
        var url = $scope.baseUrl + "/v1/sessions/register";
        var postdata = {
            UserName: $scope.userProfile.username
            , password: $scope.userProfile.password
            , emailoptin: $scope.userProfile.optInEmail
            , smsoptin: $scope.userProfile.optInSMS
            , pushoptin: $scope.userProfile.optInPush
        };
        $http.post(url, postdata)
            .then(function (response) {
                $scope.authToken = response.data.access_token;
                $scope.isAuthenticated = true;
                //get list of roles
                $scope.roles = $scope.GetRoles();
                //default role to Administrator index=id-1
                $scope.currentRole = $scope.roles[4];
            })
            .catch(function (error) {
                $scope.authToken = error.data;
            });
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