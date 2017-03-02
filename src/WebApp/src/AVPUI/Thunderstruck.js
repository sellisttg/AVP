var app = angular.module('AVPApp', []);
app.controller('AVPController'
, function ($scope, $http) {
    /************************************************************/
    /*                    Properties
    /************************************************************/
    //constants
    $scope.baseUrl = "http://avp2017webapp.azurewebsites.net/api";
    //$scope.baseUrl = "http://localhost:57123/api";
    
    //self-explanatory
    $scope.isAuthenticated = false;
    $scope.currentRole;
    $scope.error = "";
    $scope.states = [];

    //isRegistering used on UserProfile page to trigger new registration presentation and logic
    $scope.isRegistering = false;

    //Enumeration of possible pages
    $scope.pages = { Dashboard: "Dashboard", Incidents: "Incidents", UserProfile: "UserProfile" };

    //currentPage has name of authenticated page current displayed in body
    $scope.currentPage = $scope.pages.UserProfile;

    //Housekeeping
    $scope.initUserProfile = function () {
        return {
            authToken: ""
            , userID: 0
            , username: ""
            , password: ""
            , confirmPassword: ""
            , name: ""
            , optIn: { optInEmail: true, optInSMS: true, optInPush: true }
            , address: { userAddressID: 0, streetAddress: "", city: "", state: "", zipCode: "" }
            , emailAddress: { emailAddressID: 0, emailAddress: "" }
            , sms: { smsLocationID: 0, phoneNumber: "" }
            , pushToken: ""
        }
    };
    //UserProfile
    $scope.userProfile = $scope.initUserProfile();
    
    /************************************************************/
    /*                  Methods
    /************************************************************/
    //User Management Methods
    $scope.ShowRegister = function() {
        $scope.isRegistering = true;
    }
    $scope.Logout = function () {
        $scope.userProfile = $scope.initUserProfile();
        $scope.isAuthenticated = false;
    }
    $scope.Login = function () {
        var url = $scope.baseUrl + "/v1/sessions";
        var postdata = { UserName: $scope.userProfile.username, password: $scope.userProfile.password };
        $scope.error = "";
        $http.post(url, postdata).then(
            function (response) {
                $scope.authToken = response.data.access_token;
                $scope.isAuthenticated = true;
                //get list of roles
                $scope.roles = $scope.GetRoles();
                //default role to Administrator index=id-1
                $scope.currentRole = $scope.roles[4];
                $scope.GetUserProfile();
                setAuthToken($scope.authToken);
                /* Add this to map script to allow the map to share the token and make calls to the WebApi service
                function setAuthToken(token) {
                    key = token;
                }
                */
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
            , name: $scope.userProfile.name
        };
        $scope.error = "";
        $http.post(url, postdata)
            .then(function (response) {
                $scope.authToken = response.data.access_token;
                $scope.isRegistering = false;
                $scope.isAuthenticated = true;
                //get list of roles
                $scope.roles = $scope.GetRoles();
                //default role to Administrator index=id-1
                $scope.currentRole = $scope.roles[4];
                $scope.GetUserProfile();
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
                $scope.userProfile.name = response.data.name;
                $scope.userProfile.optIn.optInEmail = response.data.emailOptIn;
                $scope.userProfile.optIn.optInSMS = response.data.smsOptIn;
                $scope.userProfile.optIn.optInPush = response.data.pushOptIn;
                $scope.states = $scope.getStates();
                $scope.GetUserAddress();
                $scope.GetEmailAddress();
                $scope.GetSMS();
            });
    }
    $scope.GetUserAddress = function () {
        var url = $scope.baseUrl + "/v1/useraddress" + "?" + $scope.GetUniqueQueryString();
        $http.get(url, { headers: { authorization: "Bearer " + $scope.authToken } }).then(
            function (response) {
                if (response.data.length > 0) {
                    $scope.userProfile.address.userAddressID = response.data[0].userAddressID;
                    $scope.userProfile.address.streetAddress = response.data[0].streetAddress;
                    $scope.userProfile.address.city = response.data[0].city;
                    $scope.userProfile.address.state = response.data[0].state;
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
                    $scope.userProfile.sms.smsLocationID = response.data[0].userSmsLocationID;
                    $scope.userProfile.sms.phoneNumber = response.data[0].phoneNumber;
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
            , name: $scope.userProfile.name
        };
        $scope.error = "";
        $http.post(url, postdata, { headers: { authorization: "Bearer " + $scope.authToken } })
            .then(function (response) {
                $scope.error = "Saved";
                //Geocode address to get Lat and Long, then save address
                var geocoder = new google.maps.Geocoder();
                var lat, lon;
                var address = $scope.userProfile.address.streetAddress + ", " + $scope.userProfile.address.city + ", " + $scope.userProfile.address.state + " " + $scope.userProfile.address.zipCode;
                geocoder.geocode({ 'address': address }, function (results, status) {
                    if (status === 'OK') {
                        lat = results[0].geometry.location.lat();
                        lon = results[0].geometry.location.lng();
                        $scope.SaveAddress(lat, lon);
                        //alert('lat::::(' + lat + ",lon:::::" + lon + ')');
                    } else {
                        $scope.SaveAddress(0,0);
                    }
                });
            })
            .catch(function (error) {
                $scope.error = error;
            });
    }
    $scope.SaveAddress = function (lat, long) {
        if ($scope.userProfile.address.streetAddress.length > 0 || $scope.userProfile.address.zipCode.length > 0)
        {
            var url = $scope.baseUrl + "/v1/useraddress";
            var postdata = {
                userAddressID: $scope.userProfile.address.userAddressID
                , userID: $scope.userProfile.userID
                , streetAddress: $scope.userProfile.address.streetAddress
                , city: $scope.userProfile.address.city
                , state: $scope.userProfile.address.state
                , zip: $scope.userProfile.address.zipCode
                , latitude: lat
                , longitude: long
            };

            //if address not already saved, insert it
            if (postdata.userAddressID == 0) {
                $http.put(url, postdata, { headers: { authorization: "Bearer " + $scope.authToken } })
                .then(function (response) {
                    $scope.userProfile.address.userAddressID = response.data.userAddressID;
                    //Now that you have an address ID, save the email and SMS locations.
                    $scope.error = "Saved";
                    $scope.SaveEmailAddress();
                    $scope.SaveSMS();
                })
                .catch(function (error) {
                    $scope.error = error.statusText;
                })
            }
            //if address already saved, update it
            else {
                $http.post(url, postdata, { headers: { authorization: "Bearer " + $scope.authToken } })
                .then(function (response) {
                    $scope.error = "Saved";
                    $scope.SaveEmailAddress();
                    $scope.SaveSMS();
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
    $scope.getStates = function () {
        return [
            { state: 'AL', description: 'Alabama' }
            , { state: 'AK', description: 'Alaska' }
            , { state: 'AZ', description: 'Arizona' }
            , { state: 'AR', description: 'Arkansas' }
            , { state: 'CA', description: 'California' }
            , { state: 'CO', description: 'Colorado' }
            , { state: 'CT', description: 'Connecticut' }
            , { state: 'DE', description: 'Delaware' }
            , { state: 'FL', description: 'Florida' }
            , { state: 'GA', description: 'Georgia' }
            , { state: 'HI', description: 'Hawaii' }
            , { state: 'ID', description: 'Idaho' }
            , { state: 'IL', description: 'Illinois' }
            , { state: 'IN', description: 'Indiana' }
            , { state: 'IA', description: 'Iowa' }
            , { state: 'KS', description: 'Kansas' }
            , { state: 'KY', description: 'Kentucky' }
            , { state: 'LA', description: 'Louisiana' }
            , { state: 'ME', description: 'Maine' }
            , { state: 'MD', description: 'Maryland' }
            , { state: 'MA', description: 'Massachusetts' }
            , { state: 'MI', description: 'Michigan' }
            , { state: 'MN', description: 'Minnesota' }
            , { state: 'MS', description: 'Mississippi' }
            , { state: 'MO', description: 'Missouri' }
            , { state: 'MT', description: 'Montana' }
            , { state: 'NE', description: 'Nebraska' }
            , { state: 'NV', description: 'Nevada' }
            , { state: 'NH', description: 'New Hampshire' }
            , { state: 'NJ', description: 'New Jersey' }
            , { state: 'NM', description: 'New Mexico' }
            , { state: 'NY', description: 'New York' }
            , { state: 'NC', description: 'North Carolina' }
            , { state: 'ND', description: 'North Dakota' }
            , { state: 'OH', description: 'Ohio' }
            , { state: 'OK', description: 'Oklahoma' }
            , { state: 'OR', description: 'Oregon' }
            , { state: 'PA', description: 'Pennsylvania' }
            , { state: 'RI', description: 'Rhode Island' }
            , { state: 'SC', description: 'South Carolina' }
            , { state: 'SD', description: 'South Dakota' }
            , { state: 'TN', description: 'Tennessee' }
            , { state: 'TX', description: 'Texas' }
            , { state: 'UT', description: 'Utah' }
            , { state: 'VT', description: 'Vermont' }
            , { state: 'VA', description: 'Virginia' }
            , { state: 'WA', description: 'Washington' }
            , { state: 'WV', description: 'West Virginia' }
            , { state: 'WI', description: 'Wisconsin' }
            , { state: 'WY', description: 'Wyoming' }
            , { state: 'AS', description: 'American Samoa' }
            , { state: 'DC', description: 'District of Columbia' }
        ]
    }
});