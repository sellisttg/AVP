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
    $scope.error = "";

    //isRegistering used on UserProfile page to trigger new registration presentation and logic
    $scope.isRegistering = false;

    //Enumeration of possible pages
    $scope.pages = { Dashboard: "Dashboard", Incidents: "Incidents", UserProfile: "UserProfile" };

    //currentPage has name of authenticated page current displayed in body
    $scope.currentPage = $scope.pages.UserProfile;
    //UserProfile
    $scope.userProfile = {
        authToken: ""
        , userID: 0
        , username: ""
        , password: ""
        , confirmPassword: ""
        , optIn: { optInEmail: true, optInSMS: true, optInPush: true }
        , address: { userAddressID: 0, streetAddress: "", city: "", state: "", zipCode: "" }
        , emailAddress: {emailAddressID: 0, emailAddress: ""}
        , sms: {smsLocationID: 0 , phoneNumber: ""}
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
                $scope.authToken = response.data.access_token;
                $scope.isAuthenticated = true;
                //get list of roles
                $scope.roles = $scope.GetRoles();
                //default role to Administrator index=id-1
                $scope.currentRole = $scope.roles[4];
                $scope.GetUserProfile();
                $scope.error = "";
            })
            .catch(function (error) {
                $scope.error = "Login Failed";
            });
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
            , emailoptin: $scope.userProfile.optIn.optInEmail
            , smsoptin: $scope.userProfile.optIn.optInSMS
            , pushoptin: $scope.userProfile.optIn.optInPush
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
                $scope.error = error.statusText;
            });
    }
    $scope.GetUserProfile = function () {
        var url = $scope.baseUrl + "/v1/profile" + "?" + $scope.GetUniqueQueryString();
        //$http.get(url, { headers: [{ authorization: "Bearer " + $scope.authToken }, { ContentType : "application/json; charset=utf-8" }] }).then(
        $http.get(url, { headers: { "authorization": "Bearer " + $scope.authToken } }).then(
            function (response) {
                $scope.userProfile.username = response.data.userName;
                $scope.userProfile.userID = response.data.userID;
                $scope.userProfile.optIn.optInEmail = response.data.emailOptIn;
                $scope.userProfile.optIn.optInSMS = response.data.smsOptIn;
                $scope.userProfile.optIn.optInPush = response.data.pushOptIn;
                $scope.GetUserAddress();
                $scope.GetEmailAddress();
                //$scope.GetSMS();
            });
    }
    $scope.GetUserAddress = function () {
        var url = $scope.baseUrl + "/v1/useraddress" + "?" + $scope.GetUniqueQueryString();
        $http.get(url, { headers: { authorization: "Bearer " + $scope.authToken } }).then(
            function (response) {
                if (response.data.length > 0) {
                    $scope.userProfile.address.userAddressID = response.data[0].userAdressID;
                    $scope.userProfile.address.streetAddress = response.data[0].streetAddress;
                    $scope.userProfile.address.zipCode = response.data[0].zip;
                }
            });
    }
    $scope.GetEmailAddress = function () {
        var url = $scope.baseUrl + "/v1/emaillocation" + "?" + $scope.GetUniqueQueryString();
        $http.get(url, { headers: { authorization: "Bearer " + $scope.authToken } }).then(
            function (response) {
                if (response.data.length > 0) {
                    $scope.userProfile.emailAddress.emailAddressID = response.data[0].userEmailLocationID;
                    $scope.userProfile.emailAddress.emailAddress = response.data[0].emailAddress;
                }
            });
    }
    $scope.GetSMS = function () {
        var url = $scope.baseUrl + "/v1/smslocation" + "?" + $scope.GetUniqueQueryString();
        $http.get(url, { headers: { authorization: "Bearer " + $scope.authToken } }).then(
            function (response) {
                if (response.data.length > 0) {
                    $scope.userProfile.phoneNumber = response.data[0].phoneNumber;
                }
            });
    }
    $scope.SaveUserInfo = function () {
        $scope.SaveUserProfile();
    }
    $scope.SaveUserProfile = function () {
        var url = $scope.baseUrl + "/v1/profile";
        var postdata = {
            userID: $scope.userProfile.userID
            , userName: $scope.userProfile.username
            , smsoptin: $scope.userProfile.optIn.optInSMS
            , emailoptin: $scope.userProfile.optIn.optInEmail
            , pushoptin: $scope.userProfile.optIn.optInPush
        };
        $http.post(url, postdata, { headers: { authorization: "Bearer " + $scope.authToken } })
            .then(function (response) {
                $scope.error = "Saved";
                $scope.SaveAddress();
                $scope.SaveEmailAddress();
                //$scope.SaveSMS();
            })
            .catch(function (error) {
                $scope.error = error;
            });
    }
    $scope.SaveAddress = function () {
        if ($scope.userProfile.address.streetAddress.length > 0 || $scope.userProfile.address.zipCode.length > 0)
        {
            var url = $scope.baseUrl + "/v1/useraddress";
            var postdata = {
                userAddressID: $scope.userProfile.address.userAddressID
                , userID: $scope.userProfile.userID
                , streetAddress: $scope.userProfile.address.streetAddress
                , city: ""
                , state: "CA"
                , zip: $scope.userProfile.address.zipCode
                , latitude: 0
                , longitude: 0
            };
            if (postdata.userAddressID == 0) {
                $http.put(url, postdata, { headers: { authorization: "Bearer " + $scope.authToken } })
                .then(function (response) {
                    $scope.userProfile.address.userAddressID = response.data.userAddressID;
                    $scope.error = "Saved";
                })
                .catch(function (error) {
                    $scope.error = error.statusText;
                })
            }
            else {
                $http.post(url, postdata, { headers: { authorization: "Bearer " + $scope.authToken } })
                .then(function (response) {
                    $scope.error = "Saved";
                })
                .catch(function (error) {
                    $scope.error = error.statusText;
                })
            }
        }
    }
    $scope.SaveEmailAddress = function () {
        if ($scope.userProfile.emailAddress.emailAddress.length > 0) {
            var url = $scope.baseUrl + "/v1/emaillocation";
            var postdata = {
                userEmailLocationID: $scope.userProfile.emailAddress.emailAddressID
                , userID: $scope.userProfile.userID
                , emailAddress: $scope.userProfile.emailAddress.emailAddress
                , userAddressID: $scope.userProfile.address.userAddressID
            };
            if (postdata.userEmailLocationID == 0) {
                $http.put(url, postdata, { headers: { authorization: "Bearer " + $scope.authToken } })
                .then(function (response) {
                    $scope.userProfile.emailAddress.emailAddressID = response.data.userEmailLocationID;
                    $scope.error = "Saved";
                })
                .catch(function (error) {
                    $scope.error = error.statusText;
                })
            }
            else {
                $http.post(url, postdata, { headers: { authorization: "Bearer " + $scope.authToken } })
                .then(function (response) {
                    $scope.error = "Saved";
                })
                .catch(function (error) {
                    $scope.error = error.statusText;
                })
            }
        }
    }
    $scope.SaveSMS = function () {
        if ($scope.userProfile.sms.phoneNumber.length > 0) {
            var url = $scope.baseUrl + "/v1/smslocation";
            var postdata = {
                userSmsLocationID: $scope.userProfile.sms.smsLocationID
                , userID: $scope.userProfile.userID
                , phoneNumber: $scope.userProfile.sms.phoneNumber
                , userAddressID: $scope.userProfile.address.userAddressID
            };
            if (postdata.userSmsLocationID == 0) {
                $http.put(url, postdata, { headers: { authorization: "Bearer " + $scope.authToken } })
                .then(function (response) {
                    $scope.userProfile.sms.smsLocationID = response.data.userSmsLocationID;
                    $scope.error = "Saved";
                })
                .catch(function (error) {
                    $scope.error = error.statusText;
                })
            }
            else {
                $http.post(url, postdata, { headers: { authorization: "Bearer " + $scope.authToken } })
                .then(function (response) {
                    $scope.error = "Saved";
                })
                .catch(function (error) {
                    $scope.error = error.statusText;
                })
            }
        }
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
    $scope.GetUniqueQueryString = function () {
        var now = new Date();
        return now.getFullYear().toString()
            + now.getMonth().toString()
            + now.getDate().toString()
            + now.getHours().toString()
            + now.getMinutes().toString()
            + now.getSeconds().toString()
            + now.getMilliseconds().toString();
    }
});