var allsubscribericon = L.icon({
    iconUrl: 'subscriberpushpin.png',

     iconSize:     [38, 60], // size of the icon
     popupAnchor:  [-3, -60]  // point from which the popup should open relative to the iconAnchor
});
var user= L.icon({
    iconUrl: 'subscribers.png',

     iconSize:     [38, 60], // size of the icon
     popupAnchor:  [-3, -60]  // point from which the popup should open relative to the iconAnchor
});

var key = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJzZWxsaXMiLCJqdGkiOiIyMDRiNGEzOC04M2JjLTQ2MTYtYjVmZi05NTIxNzQwYjk0ODMiLCJpYXQiOjE0ODgzMTc4MjcsIm5iZiI6MTQ4ODMxNzc4NSwiZXhwIjoxNDg4MzIxNjczLCJpc3MiOiJBVlBUb2tlblNlcnZlciIsImF1ZCI6Imh0dHA6Ly9sb2NhbGhvc3Q6NTcxMjMvIn0.qMP2NMhhl0EdvzrxD72xQapX9tw8-crWjI0DAlK3f54";


var incidents = new L.LayerGroup();

var allincidentdetails =   {"allincidents":[]}; //this is hardcoded time being will be received from 
	

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
		"Tsunami":tsunami,
		"NaturalHazards":Naturalhazards,
		"WeatherHazards": Weatherhazards,
		"Fire": fire,
        "Flood": flood
	};
	
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
	var circle,clicklocationMarker;
	var incidentID;
	map.on('click', function(e) {   
	if(circle){
	map.removeLayer(circle);
	}
	if(clicklocationMarker){
	map.removeLayer(clicklocationMarker);
	}
	   var sel = document.getElementById('ddlRadius');
       var sv = sel.options[sel.selectedIndex].value;
	   var radioValue = $("input[name='incidentType']:checked"). val();
	   var lat = e.latlng.lat.toFixed(4);
	   var lon = e.latlng.lng.toFixed(4);
	   incidentID = radioValue.slice(0,3) + Math.floor(Math.random() * 899999 + 100000);
	   var incidentdetails =   {"incidents":[]};
	   incidentdetails.incidents.push({"id" : incidentID , "Lat" : lat, 
									"Long" : lon, "incidenttype": radioValue, "radius" : sv });
	
/* 		allincidentdetails.allincidents.push({"id" : incidentID , "Lat" : lat, 
									"Long" : lon, "incidenttype": radioValue, "radius" : sv }); */
	  
	   var popupcontent = "<p>Incident Type: " + radioValue + " <br/>   Location  (" + lat + "," + lon + ") <br/> Incident ID :"  + incidentID + "<br/> Radius:" + sv + " mile<br/>  </p>";
	  
	  

        var popLocation= e.latlng;
       circle = L.circle(e.latlng, sv*1069, {
		color: 'red',
		fillColor: '#f03',
		fillOpacity: 0.5,
		clickable:false
	}).addTo(map)
		clicklocationMarker=L.marker(e.latlng).addTo(map)
		.bindPopup(popupcontent).openPopup();
		
		  $.ajax({                    
                    url: "http://avp2017webapp.azurewebsites.net/api/v1/incident",
                    type: "POST",
                    headers: { 'Authorization': "Bearer " + key   },
                    data: JSON.stringify(incidentdetails),
                    contentType: "application/json",
                    success: function (data) {
                        alert("Successfully Registered..");
                    },
                    error: function (xhRequest, ErrorText, thrownError) {
                        alert("Failed to process correctly, please try again");
                    }
                });
    });
	
function clearCircleAndMarker()
{
	if(circle){
	map.removeLayer(circle);
	}
	if(clicklocationMarker){
	map.removeLayer(clicklocationMarker);
	}
}
function findSelection()
{
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

var all_subscriber;
var allsubscribers =   {"subscribers":[]};

function ShowAllSubscribers()
{
//Read all subscribers
 if (all_subscriber){
 map.removeLayer(all_subscriber);
 }
 
/* 	  allsubscribers.subscribers.push({"subscriberid" : 1234 ,
									  "AddressID" : 3456,
									  "Address" : "2015 J street Sacramento CA",
									  "lat" : 38.576791,
								      "lon" : -121.478957
									  },
									  {"subscriberid" : 12342 ,
									  "AddressID" : 34562,
									  "Address" : "7735 Roseville Rd a Sacramento CA",
									  "lat" : 38.705902,
								      "lon" : -121.327112
									  },
									  {"subscriberid" : 2342 ,
									  "AddressID" : 4562,
									  "Address" : "15875 CA-16 Capay CA",
									  "lat" : 38.709288,
								      "lon" : -122.121253
									  },
									  {"subscriberid" : 20342 ,
									  "AddressID" : 40562,
									  "Address" : "9549 Heinlein Way Sacramento, CA",
									  "lat" : 38.465,
								      "lon" : -121.341
									  }); */
    //console.log(allsubscribers);
	
			  $.ajax({                    
                    url: "http://avp2017webapp.azurewebsites.net/api/v1/incident/allsubscribers",
                    type: "GET",
                    headers: { 'Authorization': "Bearer " +  key },
                    data:"JSON",
					async: false,
                    contentType: "application/json",
                    success: function (data) {
					//console.log(data);
					allsubscribers=data;
					//allsubscribers = data;
					//allsubscribers = data;
                        //alert("yes got");
						
                    },
                    error: function (xhRequest, ErrorText, thrownError) {
                        alert("Failed to process correctly, please try again");
                    }
                });
	
	
	
	//console.log (allsubscribers);
	for(var i=0; i<allsubscribers.subscribers.length; i++) {
	//alert(allsubscribers.subscribers[0].lat);
		var tlat = allsubscribers.subscribers[i].lat;
		var tlon = allsubscribers.subscribers[i].lon;
	//console.log(tlat);
	//console.log(tlon);
	
	all_subscriber=L.marker([tlat,tlon], {icon: user,title:allsubscribers.subscribers[i].Address}).addTo(map);
	/* L.CircleMarker([tlat,tlon],{radius: 5,fillColor: "#A3C990",color: "#000",weight: 1,opacity: 1,fillOpacity: 0.4 }).addTo(map); */
  }
}
	
	
	  
									
function showPreviousAffectionNotifications()
{

	clearCircleAndMarker();
				  $.ajax({                    
                    url: "http://avp2017webapp.azurewebsites.net/api/v1/incident",
                    type: "GET",
                    headers: { 'Authorization': "Bearer " +  key },
                    data:"JSON",
					async: false,
                    contentType: "application/json",
                    success: function (data) {
					allincidentdetails=data;
					//allsubscribers = data;
					//allsubscribers = data;
                      alert("yes got");
						
                    },
                    error: function (xhRequest, ErrorText, thrownError) {
                        alert("Failed to process correctly, please try again");
                    }
                });
	
	for(var i=0; i< allincidentdetails.allincidents.length; i++) {
	//alert(allsubscribers.subscribers[0].lat);
	var tlat = Number(allincidentdetails.allincidents[i].Lat);
	var tlon = Number(allincidentdetails.allincidents[i].Long);

	var popupcontent = "<p>Incident Type: " + allincidentdetails.allincidents[i].incidenttype + " <br/>   Location  (" + tlat + "," + tlon + ") <br/> Incident ID :"  + allincidentdetails.allincidents[i].id + "<br/> Radius:" + 1069 *allincidentdetails.allincidents[i].radius + "br/>  </p>";
	
	var popLocation= [tlat,tlon];
       circle = L.circle(popLocation, 1069 * allincidentdetails.allincidents[i].radius, {
		color: 'red',
		fillColor: '#f03',
		fillOpacity: 0.5
	}).addTo(map)
		clicklocationMarker=L.marker(popLocation).addTo(map)
		.bindPopup(popupcontent).openPopup();
	}
}

var selSubForNotification;
var counter_points_in_circle = 0;

function pointsInCircle() {
 if (all_subscriber){
 map.removeLayer(all_subscriber);
 }
 
 if (selSubForNotification){
 map.removeLayer(selSubForNotification);
 }
 

    if (circle !== undefined) {
        // Only run if we have an address entered
        // Lat, long of circle
		var meters_user_set = circle.getRadius();
        var circle_lat_long = circle.getLatLng();
		//console.log("circle lat lon", circle_lat_long);
		//console.log("circle radius", meters_user_set);
        
        
		console.log (allsubscribers);
		var affectedusers={"incident":{} , "subscriberUnderNotification":[]};
		affectedusers.incident.push=({"incidentID":incidentID});
	for(var i=0; i<allsubscribers.subscribers.length; i++) {
		//alert(allsubscribers.subscribers[0].lat);
		var tlat = allsubscribers.subscribers[i].lat;
		var tlon = allsubscribers.subscribers[i].lon;
		//console.log(tlat);
		//console.log(tlon);
		var subscriberloc= L.latLng(tlat,tlon);
		var subscriberUnderNotification;
		var distance_from_layer_circle = subscriberloc.distanceTo(circle_lat_long);
		console.log(distance_from_layer_circle);
		if (distance_from_layer_circle <= meters_user_set) {
					counter_points_in_circle += 1;
					selSubForNotification=L.marker([tlat,tlon], {icon: allsubscribericon,title:allsubscribers.subscribers[i].Address}).addTo(map);
					affectedusers.subscriberUnderNotification.push=({"subscriberId": allsubscribers.subscribers[i].subscriberId,"addressId": allsubscribers.subscribers[i].addressId});
			}
	console.log(affectedusers);
	document.getElementById("noOfAffectedusers").innerHTML = "No of affected users  " + counter_points_in_circle; 
	counter_points_in_circle = 0;
      }
    }
  }



/* 

({"id" : incidentID , "Lat" : lat, 
									"Long" : lon, "incidenttype": radioValue, "radius" : sv })


{affectedusers=[{su
									
									incidentID
{
  "incident": {
    "incidentID": 1,
    "incidentName": "",
    "lat": 37.787998199462891,
    "long": -119.71800231933594,
    "incidentType": "Tsunami",
    "radius": 30,
    "id": "Tsu470731"
  },
  "subscriberUnderNotification": [
    {
      "subscriberId": 19,
      "addressId": 5,
      "address": "3456 J St , CA 87655",
      "lat": 0,
      "lon": 0,
      "name": ""
    },

	{
		subscriberUnderNotification:[{"subscriberId": 19, "addressId": 5,"incidentID": 1},{"subscriberId": 19, "addressId": 5,"incidentID": 1}]
	}
	
	
	
	 */