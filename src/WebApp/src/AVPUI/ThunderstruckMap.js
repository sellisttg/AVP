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

var key = "";

var all_subscriber;
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

GetAllSubscribers();

var map = L.map('map', {
    center: [38.575764, -121.478851],
    zoom: 7,
    layers: [streets, earthquake]
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

//var mapleft = $('#map').left();

//$('#map').css("width", ($(window).width()));
//$(window).on("resize", resize);
////resize();
//function resize() {    
//    $('#map').css("width", $(window).width());
//    $('#map').css("left", 100);
//}

var circle, clicklocationMarker;
var incidentID;
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
    var lat = e.latlng.lat.toFixed(4);
    var lon = e.latlng.lng.toFixed(4);
    incidentID = radioValue.slice(0, 3) + Math.floor(Math.random() * 899999 + 100000);
    var incidentdetails = { "incidents": [] };
        incidentdetails.incidents.push({
            "id": incidentID, "Lat": lat,
            "Long": lon, "incidenttype": radioValue, "radius": sv
    });

    //var popupcontent = "<p>Notification Type: " + radioValue + " <br/>   Location  (" + lat + "," + lon + ") <br/> Incident ID :" + incidentID + "<br/> Radius:" + sv + " mile<br/>  <button class='btn btn-primary'>Notify</button></p>";
        var popupcontent = "<div>Send a notification to</div>"
            + "<div>subscribers in the area:</div>"
            + "<div><input type='text' style='width:200px;height:40px' class='form-control' maxlength='140' id=''></input></div>"
            + "<div><button class='btn btn-primary'>Notify</button></div>";
    var popLocation = e.latlng;
    circle = L.circle(e.latlng, sv * 1069, {
        color: 'red',
        fillColor: '#f03',
        fillOpacity: 0.5,
        clickable: false
    }).addTo(map)
    clicklocationMarker = L.marker(e.latlng).addTo(map)
    .bindPopup(popupcontent).openPopup();

    $.ajax({
        url: "http://avp2017webapp.azurewebsites.net/api/v1/incident",
        type: "POST",
        headers: { 'Authorization': "Bearer " + key },
        data: JSON.stringify(incidentdetails),
        async: false,
        contentType: "application/json",
        success: function (data) {
            //alert("Successfully Registered..");
            ShowAffectedSubscribers();
        },
        error: function (xhRequest, ErrorText, thrownError) {
            alert("Failed to process correctly, please try again");
        }
    });
});
function setAuthToken(token) {
    key = token;
}
function RefreshMap() {
    location.reload(true);
}
function clearCircleAndMarker() {
    if (circle) {
        map.removeLayer(circle);
    }
    if (clicklocationMarker) {
        map.removeLayer(clicklocationMarker);
    }
    if (selSubForNotification) {
        map.removeLayer(selSubForNotification);
    }

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
            url: "http://avp2017webapp.azurewebsites.net/api/v1/incident/allsubscribers",
            type: "GET",
            headers: { 'Authorization': "Bearer " + key },
            data: "JSON",
            async: false,
            contentType: "application/json",
            success: function (data) {
                //console.log(data);
                allsubscribers = data;
                //allsubscribers = data;
                //allsubscribers = data;
                //alert("yes got");

            },
            error: function (xhRequest, ErrorText, thrownError) {
                alert("Failed to process correctly, please try again");
            }
        });
    }
}

//3-1-2017 Shawn Sampo: not used
function ShowAllSubscribers() {
    //Read all subscribers
    if (all_subscriber) {
        map.removeLayer(all_subscriber);
    }

    //console.log (allsubscribers);
    for (var i = 0; i < allsubscribers.subscribers.length; i++) {
        //alert(allsubscribers.subscribers[0].lat);
        var tlat = allsubscribers.subscribers[i].lat;
        var tlon = allsubscribers.subscribers[i].lon;
        //console.log(tlat);
        //console.log(tlon);

        all_subscriber = L.marker([tlat, tlon], { icon: user, title: allsubscribers.subscribers[i].Address }).addTo(map);
        /* L.CircleMarker([tlat,tlon],{radius: 5,fillColor: "#A3C990",color: "#000",weight: 1,opacity: 1,fillOpacity: 0.4 }).addTo(map); */
    }
}


function showPreviousAffectionNotifications() {

    clearCircleAndMarker();
    $.ajax({
        url: "http://avp2017webapp.azurewebsites.net/api/v1/incident",
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
            alert("Failed to process correctly, please try again");
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
                selSubForNotification = L.marker([allincidentdetails[j].incidents.subscribers.lat, allincidentdetails[j].incidents.subscribers.lng], { icon: allsubscribericon, title: allincidentdetails[j].incidents.subscribers.name + "-" + allincidentdetails[j].incidents.subscribers.address }).addTo(map);
            }
        }

    }
}

var selSubForNotification;
var counter_points_in_circle = 0;

//function pointsInCircle() {
function ShowAffectedSubscribers() {
    if (all_subscriber) {
        map.removeLayer(all_subscriber);
    }

    if (selSubForNotification) {
        map.removeLayer(selSubForNotification);
    }


    if (circle !== undefined) {
        // Only run if we have an address entered
        // Lat, long of circle
        var meters_user_set = circle.getRadius();
        var circle_lat_long = circle.getLatLng();
        //console.log("circle lat lon", circle_lat_long);
        //console.log("circle radius", meters_user_set);


        //console.log (allsubscribers);
        var affectedusers = { "SubscriberUnderNotification": [] };
        console.log(allsubscribers);
        for (var i = 0; i < allsubscribers.subscribers.length; i++) {
            //alert(allsubscribers.subscribers[0].lat);
            var tlat = allsubscribers.subscribers[i].lat;
            var tlon = allsubscribers.subscribers[i].lon;
            //console.log(tlat);
            //console.log(tlon);
            var subscriberloc = L.latLng(tlat, tlon);

            var distance_from_layer_circle = subscriberloc.distanceTo(circle_lat_long);
            //console.log(distance_from_layer_circle);
            if (distance_from_layer_circle <= meters_user_set) {
                counter_points_in_circle += 1;

                selSubForNotification = L.marker([tlat, tlon], { icon: allsubscribericon, title: allsubscribers.subscribers[i].Address }).addTo(map);
                //console.log("subsriberID:",allsubscribers.subscribers[i].subscriberId);
                //console.log("addressId:", allsubscribers.subscribers[i].addressId);
                affectedusers.SubscriberUnderNotification.push({ "subscriberId": allsubscribers.subscribers[i].subscriberId, "addressId": allsubscribers.subscribers[i].addressId, "incidentId": incidentID });
            }

        }
        //document.getElementById("noOfAffectedusers").innerHTML = "No of affected users  " + counter_points_in_circle;
        //counter_points_in_circle = 0;
        // console.log(affectedusers);
        $.ajax({
            url: "http://avp2017webapp.azurewebsites.net/api/v1/incident/subscribersundernotification",
            type: "POST",
            headers: { 'Authorization': "Bearer " + key },
            data: JSON.stringify(affectedusers),
            contentType: "application/json",
            async: false,
            success: function (data) {
                //alert("Successfully posted the subscribers to database..");
            },
            error: function (xhRequest, ErrorText, thrownError) {
                alert("Failed to process correctly, please try again");
            }
        });

    }
}