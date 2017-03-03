//var baseUrl= "http://localhost:57123/api";
var baseUrl = "http://avp2017webapp.azurewebsites.net/api";
var allsubscribericon = L.icon({
    iconUrl: 'subscriberpushpin.png',
    iconSize: [38, 60], // size of the icon
    popupAnchor: [-3, -60]  // point from which the popup should open relative to the iconAnchor
});
var user = L.icon({
    iconUrl: 'subscribers.png',
    iconSize: [38, 60], // size of the icon
    popupAnchor: [-3, -60]  // point from which the popup should open relative to the iconAnchor
});

var lat = 0;
var lon = 0;
var key = ""; //current authorization key
var currentUserID = 0; //current user id
//list o subscriber markers currently displayed on map
var affectedSubscriberMarkers = [];
var affectedSubscriberList = [];
var affectedSubscriberMarkerGroup;
var allsubscriberMarkers=[];
var allsubscribers = { "subscribers": [] };

var incidents = new L.LayerGroup();

var allincidentdetails; //this is hardcoded time being will be received from 

var streets = L.esri.basemapLayer('Streets'),
    topographic = L.esri.basemapLayer('Topographic'),
    Imagery = L.esri.basemapLayer('Imagery');

var baseLayers = {
    "Streets": streets,
    "Topographic": topographic,
    "Imagery": Imagery
};

var earthquake = L.esri.dynamicMapLayer({
    url: 'https://sampleserver3.arcgisonline.com/arcgis/rest/services/Earthquakes/EarthquakesFromLastSevenDays/MapServer',
    opacity: 0.7,
    useCors: false
});

var tsunami = L.esri.dynamicMapLayer({
    url: 'https://maps.ngdc.noaa.gov/arcgis/rest/services/web_mercator/hazards/MapServer',
    opacity: 1.0,
    useCors: false
});

var Naturalhazards = L.esri.dynamicMapLayer({
    url: 'https://igems.doi.gov/arcgis/rest/services/igems_haz/MapServer',
    opacity: 1.0,
    useCors: false
});

var Weatherhazards = L.esri.dynamicMapLayer({
    url: 'https://idpgis.ncep.noaa.gov/arcgis/rest/services/NWS_Climate_Outlooks/cpc_weather_hazards/MapServer',
    opacity: 1.0,
    useCors: false
});

var fire = L.esri.dynamicMapLayer({
    url: 'https://wildfire.cr.usgs.gov/ArcGIS/rest/services/geomac_dyn/MapServer',
    opacity: 1.0,
    useCors: false
});

var flood = L.esri.dynamicMapLayer({
    url: 'https://idpgis.ncep.noaa.gov/arcgis/rest/services/NWS_Observations/ahps_riv_gauges/MapServer',
    opacity: 1.0,
    useCors: false
});

var incidents = {
    "Earthquake": earthquake,
    "Tsunami": tsunami,
    "NaturalHazards": Naturalhazards,
    "WeatherHazards": Weatherhazards,
    "Fire": fire,
    "Flood": flood
};
var map = L.map('map', {
        center: [38.575764, -121.478851],
        zoom: 7,
        layers: [streets, earthquake],
        trackResize: true
});

L.control.layers(baseLayers, incidents).addTo(map);
var searchControl = L.esri.Geocoding.geosearch().addTo(map);
var results = L.layerGroup().addTo(map);
searchControl.on('results', function (data) {
    results.clearLayers();
    for (var i = data.results.length - 1; i >= 0; i--) {
        results.addLayer(L.marker(data.results[i].latlng));
    }
});
map.on('click', function (e) {
    if (circle) {
        map.removeLayer(circle);
    }
    if (clicklocationMarker) {
        map.removeLayer(clicklocationMarker);
    }
    var sel = document.getElementById('ddlRadius');
    var sv = sel.options[sel.selectedIndex].value;
    var radioValue = $("select option:selected").val();
    lat = e.latlng.lat.toFixed(4);
    lon = e.latlng.lng.toFixed(4);
    incidentID = radioValue.slice(0, 3) + Math.floor(Math.random() * 899999 + 100000);
    var incidentdetails = { "incidents": [] };
    incidentdetails.incidents.push({
        "id": incidentID, "Lat": lat,
        "Long": lon, "incidenttype": radioValue, "radius": sv
    });

    var popupcontent = "<div>Type a notification message to send to subscribers in the area:</div>"
        + "<div><textarea rows='2' cols='50' class='form-control' id='NotificationMessage'></textarea></div>"
        + "<div><button class='btn btn-primary' onclick='Notify()'>Notify</button></div>";
    var popLocation = e.latlng;
    circle = L.circle(e.latlng, sv * 1069, {
        color: 'red',
        fillColor: '#f03',
        fillOpacity: 0.5,
        clickable: false
    }).addTo(map)
    clicklocationMarker = L.marker(e.latlng).addTo(map)
    .bindPopup(popupcontent).openPopup();

    ShowAffectedSubscribers();
});

var circle, clicklocationMarker;
var incidentID;

function setAuthToken(token, userID) {
    key = token;
    currentUserID = userID;
}
function Notify() {
    var incidentDetails = {
        lat: lat
        , long: lon
        , incidentType: document.getElementById('incidentType').value
        , radius: document.getElementById('ddlRadius').value
    };
    var incidentList = { incidents: [incidentDetails] };

    $.ajax({
        url: baseUrl + "/v1/incident",
        type: "POST",
        headers: { 'Authorization': "Bearer " + key },
        data: JSON.stringify(incidentList),
        async: false,
        contentType: "application/json",
        success: function (data) {
            //alert("Incident Created");
            AddSubscribersToIncident(data.incidents[0].incidentID);
        },
        error: function (xhRequest, ErrorText, thrownError) {
            alert("Failed to create incident");
        }
    });
}
function AddSubscribersToIncident(incidentID) {
    var postdata = { subscriberUnderNotification: [] };
    for (user in affectedSubscriberList) {
        postdata.subscriberUnderNotification.push({
            subscriberId: affectedSubscriberList[user].subscriberId
            , addressId: affectedSubscriberList[user].addressId
            , address: affectedSubscriberList[user].address
            , lat: affectedSubscriberList[user].lat
            , lon: affectedSubscriberList[user].lon
            , name: affectedSubscriberList[user].name
            , incidentID: incidentID
        });
    }

    $.ajax({
        url: baseUrl + "/v1/incident/subscribersundernotification",
        type: "POST",
        headers: { 'Authorization': "Bearer " + key },
        data: JSON.stringify(postdata),
        contentType: "application/json",
        async: false,
        success: function (data) {
            //alert("Successfully posted the subscribers to database..");
            CreateNotification(incidentID);
        },
        error: function (xhRequest, ErrorText, thrownError) {
            alert("Failed to add subscribers to incident.");
        }
    });
}
function CreateNotification(incidentID) {
    var currentDate = new Date();
    var postdata = {
        incidentID: incidentID
        , message: document.getElementById("NotificationMessage").value
        , messageDateTime: new Date().toISOString()
        , sendingUserID: currentUserID
        , notificationID: 0
};

    $.ajax({
        url: baseUrl + "/v1/notification/new",
        type: "POST",
        headers: { 'Authorization': "Bearer " + key },
        data: JSON.stringify(postdata),
        contentType: "application/json",
        async: false,
        success: function (data) {
            SendNotification(incidentID, data.notificationID);
        },
        error: function (xhRequest, ErrorText, thrownError) {
            alert("Failed to create Notification.");
        }
    });
}
function SendNotification(incidentID, notificationID) {
    var currentDate = new Date();
    var postdata = {
        incidentID: incidentID
        , message: document.getElementById("NotificationMessage").value
        , messageDateTime: new Date().toISOString()
        , sendingUserID: currentUserID
        , notificationID: notificationID
    };

    $.ajax({
        url: baseUrl + "/v1/notification/send",
        type: "POST",
        headers: { 'Authorization': "Bearer " + key },
        data: JSON.stringify(postdata),
        contentType: "application/json",
        async: false,
        success: function (data) {
            alert("Successfully sent Notification");
            clearCircleAndMarker()
        },
        error: function (xhRequest, ErrorText, thrownError) {
            alert("Failed to send Notification.");
        }
    });
}
function InitMap() {
    //location.reload(true);
    //document.getElementById("map").innerHTML.reload;
    clearCircleAndMarker();
    GetAllSubscribers();
    setTimeout(function () { map.invalidateSize() }, 200);
}
function clearCircleAndMarker() {
    if (circle) {
        map.removeLayer(circle);
    }
    if (clicklocationMarker) {
        map.removeLayer(clicklocationMarker);
    }
    ClearSubscriberMarkers();
}
function findSelection() {
    var radios = document.getElementsByName('incidentType');

    for (var i = 0, length = radios.length; i < length; i++) {
        if (radios[i].checked) {
            // do whatever you want with the checked radio
            return radios[i].value;

            // only one radio can be logically checked, don't check the rest
            break;
        }
    }
}

function GetAllSubscribers() {
    if (key.length > 0)
    {
        $.ajax({
            url: baseUrl + "/v1/incident/allsubscribers" + "?" + GetUniqueQueryString(),
            type: "GET",
            headers: { 'Authorization': "Bearer " + key },
            data: "JSON",
            async: false,
            contentType: "application/json",
            success: function (data) {
                //console.log(data);
                allsubscribers = data.subscribers;
                //allsubscribers = data;
                //allsubscribers = data;
                //alert("yes got");

            },
            error: function (xhRequest, ErrorText, thrownError) {
                alert("Failed get all subscribers");
            }
        });
    }
}

function GetUniqueQueryString() {
    var now = new Date();
    return now.getFullYear().toString()
        + now.getMonth().toString()
        + now.getDate().toString()
        + now.getHours().toString()
        + now.getMinutes().toString()
        + now.getSeconds().toString()
        + now.getMilliseconds().toString();
}
//3-1-2017 Shawn Sampo: not used
function ShowAllSubscribers() {
    //Read all subscribers
    if (all_subscriber) {
        map.removeLayer(all_subscriber);
    }

    for (var i = 0; i < allsubscribers.subscribers.length; i++) {
        var tlat = allsubscribers[i].lat;
        var tlon = allsubscribers[i].lon;
        allsubscriberMarkers = L.marker([tlat, tlon], { icon: user, title: allsubscribers.Address }).addTo(map);
    }
}


function showPreviousAffectionNotifications() {

    clearCircleAndMarker();
    $.ajax({
        url: baseUrl + "/v1/incident" + "?" + GetUniqueQueryString(),
        type: "GET",
        headers: { 'Authorization': "Bearer " + key },
        data: "JSON",
        async: false,
        contentType: "application/json",
        success: function (data) {
            allincidentdetails = data;
            //allsubscribers = data;
            //allsubscribers = data;
            //alert("yes got");
            console.log("data", data);

        },
        error: function (xhRequest, ErrorText, thrownError) {
            alert("Failed get previous incidents");
        }
    });

    console.log("allincidentdetails", allincidentdetails);
    console.log(allincidentdetails.incidents.length);
    for (var i = 0; i < allincidentdetails.incidents.length; i++) {
        //alert(allsubscribers.subscribers[0].lat);
        console.log("lon", allincidentdetails.incidents[i].lon);
        console.log("lat", allincidentdetails.incidents[i].lat);

        var itlat = allincidentdetails.incidents[i].lat;
        var itlon = allincidentdetails.incidents[i].lon;
        var itid = allincidentdetails.incidents[i].id;
        var irad = allincidentdetails.incidents[i].radius;
        var ittype = allincidentdetails.incidents[i].incidentType;

        var popupcontent = "<p>Incident Type: " + allincidentdetails.incidents[i].incidentType + " <br/>   Location  (" + itlat + "," + itlon + ") <br/> Incident ID :" + allincidentdetails.incidents[i].id + "<br/> Radius:" + 1069 * allincidentdetails.incidents[i].radius + "br/>  </p>";

        var popLocation = [itlat, itlon];
        circle = L.circle(popLocation, 1069 * irad, {
            color: 'red',
            fillColor: '#f03',
            fillOpacity: 0.5
        }).addTo(map)
        clicklocationMarker = L.marker(popLocation).addTo(map)
        .bindPopup(popupcontent).openPopup();

        if (allincidentdetails.incidents.subscribers != null) {
            for (var j = 0; j < allincidentdetails.incidents.subscribers.length; j++) {
                selSubForNotification.push(L.marker([allincidentdetails[j].incidents.subscribers.lat, allincidentdetails[j].incidents.subscribers.lng], { icon: allsubscribericon, title: allincidentdetails[j].incidents.subscribers.name + "-" + allincidentdetails[j].incidents.subscribers.address }).addTo(map));
            }
        }

    }
}

var selSubForNotification;
var counter_points_in_circle = 0;

function ClearSubscriberMarkers() {
    if (affectedSubscriberMarkerGroup != undefined) {
        for (var marker in affectedSubscriberMarkerGroup._layers) {
            affectedSubscriberMarkerGroup.removeLayer(affectedSubscriberMarkerGroup._layers[marker]._leaflet_id);
        }
        affectedSubscriberMarkers = [];
        affectedSubscriberList = [];
    }
}

//function pointsInCircle() {
function ShowAffectedSubscribers() {
    ClearSubscriberMarkers();

    if (circle !== undefined) {
        // Only run if we have an address entered
        // Lat, long of circle
        var meters_user_set = circle.getRadius();
        var circle_lat_long = circle.getLatLng();
        console.log(allsubscribers);
        affectedSubscriberMarkerGroup = L.layerGroup().addTo(map);
        for (var i = 0; i < allsubscribers.length; i++) {
            var tlat = allsubscribers[i].lat;
            var tlon = allsubscribers[i].lon;
            var subscriberloc = L.latLng(tlat, tlon);
            var distance_from_layer_circle = subscriberloc.distanceTo(circle_lat_long);
            if (distance_from_layer_circle <= meters_user_set) {
                counter_points_in_circle += 1;
                //affectedSubscriberMarkers.push(
                L.marker([tlat, tlon], { icon: allsubscribericon, title: allsubscribers[i].address }).addTo(affectedSubscriberMarkerGroup);
                affectedSubscriberList.push(allsubscribers[i]);
            }
        }
    }
}

