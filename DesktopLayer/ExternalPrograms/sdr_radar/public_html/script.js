// Define our global variables
var GoogleMap     = null;
var Planes        = {};
var PlanesOnMap   = 0;
var PlanesOnTable = 0;
var PlanesToReap  = 0;
var SelectedPlane = null;
var SpecialSquawk = false;

var iSortCol=-1;
var bSortASC=true;
var bDefaultSortASC=true;
var iDefaultSortCol=3;

// Radar çizgisi için global değişkenler
var radarLine;
var radarTrails = [];  // İz çizgileri için dizi
var radarAngle = 0;
var NUM_TRAILS = 30;   // İz sayısı
var TRAIL_OPACITY_STEP = 0.8 / NUM_TRAILS;  // Her izin opaklık azalma miktarı
var detectedPlanes = {};  // Tespit edilen uçakları tutmak için

// Get current map settings
CenterLat = Number(localStorage['CenterLat']) || CONST_CENTERLAT;
CenterLon = Number(localStorage['CenterLon']) || CONST_CENTERLON;
ZoomLvl   = Number(localStorage['ZoomLvl']) || CONST_ZOOMLVL;

function fetchData() {
	$.getJSON('/dump1090/data.json', function(data) {
		PlanesOnMap = 0
		SpecialSquawk = false;
		
		// Loop through all the planes in the data packet
		for (var j=0; j < data.length; j++) {
			// Do we already have this plane object in Planes?
			// If not make it.
			if (Planes[data[j].hex]) {
				var plane = Planes[data[j].hex];
			} else {
				var plane = jQuery.extend(true, {}, planeObject);
			}
			
			/* For special squawk tests
			if (data[j].hex == '48413x') {
            	data[j].squawk = '7700';
            } //*/
            
            // Set SpecialSquawk-value
            if (data[j].squawk == '7500' || data[j].squawk == '7600' || data[j].squawk == '7700') {
                SpecialSquawk = true;
            }

			// Call the function update
			plane.funcUpdateData(data[j]);
			
			// Copy the plane into Planes
			Planes[plane.icao] = plane;
		}

		PlanesOnTable = data.length;
	});
}

// Initalizes the map and starts up our timers to call various functions
function initialize() {
	// Konum bilgisini al
	if (navigator.geolocation) {
		navigator.geolocation.getCurrentPosition(
			// Konum başarıyla alındığında
			function(position) {
				SiteLat = position.coords.latitude;
				SiteLon = position.coords.longitude;
				
				// Haritayı yeni konuma göre güncelle
				GoogleMap.setCenter(new google.maps.LatLng(SiteLat, SiteLon));
				
				// Eğer radar ve çemberler zaten oluşturulmuşsa, onları temizle ve yeniden oluştur
				if (radarLine) {
					radarLine.setMap(null);
					radarTrails.forEach(function(trail) {
						trail.setMap(null);
					});
					radarTrails = [];
				}
				
				// Radar ve çemberleri yeni konumda oluştur
				if (SiteShow && SiteCircles) {
					// Merkez noktası işaretçisini güncelle
					if (window.siteMarker) {
						window.siteMarker.setPosition(new google.maps.LatLng(SiteLat, SiteLon));
						window.siteMarker.setTitle('Koordinatlar: ' + SiteLat.toFixed(6) + ', ' + SiteLon.toFixed(6));
					} else {
						var siteMarker = new google.maps.LatLng(SiteLat, SiteLon);
						var markerImage = new google.maps.MarkerImage(
							'http://maps.google.com/mapfiles/kml/pal4/icon57.png',
							new google.maps.Size(32, 32),
							new google.maps.Point(0, 0),
							new google.maps.Point(16, 16)
						);
						window.siteMarker = new google.maps.Marker({
							position: siteMarker,
							map: GoogleMap,
							icon: markerImage,
							title: 'Koordinatlar: ' + SiteLat.toFixed(6) + ', ' + SiteLon.toFixed(6),
							zIndex: -99999
						});
					}
					
					// Çemberleri yeniden çiz
					if (window.siteCircles) {
						window.siteCircles.forEach(function(circle) {
							circle.setMap(null);
						});
					}
					window.siteCircles = [];
					for (var i = 0; i < SiteCirclesDistances.length; i++) {
						var circle = drawCircle(window.siteMarker, SiteCirclesDistances[i]);
						if (circle) {
							window.siteCircles.push(circle);
						}
					}
					
					// Radarı yeniden başlat
					initRadar();
				}
			},
			// Konum alınamazsa
			function(error) {
				console.log("Konum alınamadı, varsayılan konum kullanılıyor (Elazığ Havalimanı)");
				if (SiteShow && SiteCircles) {
					var siteMarker = new google.maps.LatLng(SiteLat, SiteLon);
					var markerImage = new google.maps.MarkerImage(
						'http://maps.google.com/mapfiles/kml/pal4/icon57.png',
						new google.maps.Size(32, 32),
						new google.maps.Point(0, 0),
						new google.maps.Point(16, 16)
					);
					window.siteMarker = new google.maps.Marker({
						position: siteMarker,
						map: GoogleMap,
						icon: markerImage,
						title: 'Koordinatlar: ' + SiteLat.toFixed(6) + ', ' + SiteLon.toFixed(6) + ' (Elazığ Havalimanı)',
						zIndex: -99999
					});
					initRadar();
				}
			},
			{
				enableHighAccuracy: true,
				timeout: 5000,
				maximumAge: 0
			}
		);
	} else {
		console.log("Tarayıcı konum desteği yok, varsayılan konum kullanılıyor");
		// Varsayılan koordinatları kullan
		if (SiteShow && SiteCircles) {
			initRadar();
		}
	}

	// Make a list of all the available map IDs
	var mapTypeIds = [];
	for(var type in google.maps.MapTypeId) {
		mapTypeIds.push(google.maps.MapTypeId[type]);
	}
	// Push OSM on to the end
	mapTypeIds.push("OSM");
	mapTypeIds.push("dark_map");

	// Styled Map to outline airports and highways
	var styles = [
		{
			"featureType": "administrative",
			"stylers": [
				{ "visibility": "off" }
			]
		},{
			"featureType": "landscape",
			"stylers": [
				{ "visibility": "off" }
			]
		},{
			"featureType": "poi",
			"stylers": [
				{ "visibility": "off" }
			]
		},{
			"featureType": "road",
			"stylers": [
				{ "visibility": "off" }
			]
		},{
			"featureType": "transit",
			"stylers": [
				{ "visibility": "off" }
			]
		},{
			"featureType": "landscape",
			"stylers": [
				{ "visibility": "on" },
				{ "weight": 8 },
				{ "color": "#000000" }
			]
		},{
			"featureType": "water",
			"stylers": [
			{ "lightness": -74 }
			]
		},{
			"featureType": "transit.station.airport",
			"stylers": [
				{ "visibility": "on" },
				{ "weight": 8 },
				{ "invert_lightness": true },
				{ "lightness": 27 }
			]
		},{
			"featureType": "road.highway",
			"stylers": [
				{ "visibility": "simplified" },
				{ "invert_lightness": true },
				{ "gamma": 0.3 }
			]
		},{
			"featureType": "road",
			"elementType": "labels",
			"stylers": [
				{ "visibility": "off" }
			]
		}
	]

	// Add our styled map
	var styledMap = new google.maps.StyledMapType(styles, {name: "Dark Map"});

	// Define the Google Map
	var mapOptions = {
		center: new google.maps.LatLng(CenterLat, CenterLon),
		zoom: ZoomLvl,
		mapTypeId: google.maps.MapTypeId.ROADMAP,
		mapTypeControl: true,
		streetViewControl: false,
		mapTypeControlOptions: {
			mapTypeIds: mapTypeIds,
			position: google.maps.ControlPosition.TOP_LEFT,
			style: google.maps.MapTypeControlStyle.DROPDOWN_MENU
		}
	};

	GoogleMap = new google.maps.Map(document.getElementById("map_canvas"), mapOptions);

	//Define OSM map type pointing at the OpenStreetMap tile server
	GoogleMap.mapTypes.set("OSM", new google.maps.ImageMapType({
		getTileUrl: function(coord, zoom) {
			return "http://tile.openstreetmap.org/" + zoom + "/" + coord.x + "/" + coord.y + ".png";
		},
		tileSize: new google.maps.Size(256, 256),
		name: "OpenStreetMap",
		maxZoom: 18
	}));

	GoogleMap.mapTypes.set("dark_map", styledMap);
	
	// Listeners for newly created Map
    google.maps.event.addListener(GoogleMap, 'center_changed', function() {
        localStorage['CenterLat'] = GoogleMap.getCenter().lat();
        localStorage['CenterLon'] = GoogleMap.getCenter().lng();
    });
    
    google.maps.event.addListener(GoogleMap, 'zoom_changed', function() {
        localStorage['ZoomLvl']  = GoogleMap.getZoom();
    }); 
	
	// These will run after page is complitely loaded
	$(window).load(function() {
        $('#dialog-modal').css('display', 'inline'); // Show hidden settings-windows content
    });

	// Load up our options page
	optionsInitalize();

	// Did our crafty user need some setup?
	extendedInitalize();
	
	// Setup our timer to poll from the server.
	window.setInterval(function() {
		fetchData();
		refreshTableInfo();
		refreshSelected();
		reaper();
		extendedPulse();
	}, 1000);
}

// This looks for planes to reap out of the master Planes variable
function reaper() {
	PlanesToReap = 0;
	// When did the reaper start?
	reaptime = new Date().getTime();
	// Loop the planes
	for (var reap in Planes) {
		// Is this plane possibly reapable?
		if (Planes[reap].reapable == true) {
			// Has it not been seen for 5 minutes?
			// This way we still have it if it returns before then
			// Due to loss of signal or other reasons
			if ((reaptime - Planes[reap].updated) > 300000) {
				// Reap it.
				delete Planes[reap];
			}
			PlanesToReap++;
		}
	};
} 

// Refresh the detail window about the plane
function refreshSelected() {
    var selected = false;
	if (typeof SelectedPlane !== 'undefined' && SelectedPlane != "ICAO" && SelectedPlane != null) {
    	selected = Planes[SelectedPlane];
    }
	
	var columns = 2;
	var html = '';
	
	if (selected) {
    	html += '<table id="selectedinfo" width="100%">';
    } else {
        html += '<table id="selectedinfo" class="dim" width="100%">';
    }
	
	// Flight header line including squawk if needed
	if (selected && selected.flight == "") {
	    html += '<tr><td colspan="' + columns + '" id="selectedinfotitle"><b>N/A (' +
	        selected.icao + ')</b>';
	} else if (selected && selected.flight != "") {
	    html += '<tr><td colspan="' + columns + '" id="selectedinfotitle"><b>' +
	        selected.flight + '</b>';
	} else {
	    html += '<tr><td colspan="' + columns + '" id="selectedinfotitle"><b>DUMP1090</b>';
	}
	
	if (selected && selected.squawk == 7500) { // Lets hope we never see this... Aircraft Hijacking
		html += '&nbsp;<span class="squawk7500">&nbsp;Squawking: Aircraft Hijacking&nbsp;</span>';
	} else if (selected && selected.squawk == 7600) { // Radio Failure
		html += '&nbsp;<span class="squawk7600">&nbsp;Squawking: Radio Failure&nbsp;</span>';
	} else if (selected && selected.squawk == 7700) { // General Emergency
		html += '&nbsp;<span class="squawk7700">&nbsp;Squawking: General Emergency&nbsp;</span>';
	} else if (selected && selected.flight != '') {
		// Uçuş kodundaki boşlukları temizle
		var cleanFlight = selected.flight.trim();
		
		// FR24 için
		html += '&nbsp;<a href="https://www.flightradar24.com/' + cleanFlight + '" target="_blank" rel="noopener">[FR24]</a>';
		
		// FlightAware için - IATA/ICAO kodu ile uçuş numarasını birleştir
		var flightParts = cleanFlight.match(/^([A-Z]{2,3})(\d+)$/);
		if (flightParts) {
			var airlineCode = flightParts[1];
			var flightNumber = flightParts[2];
			html += '&nbsp;<a href="https://www.flightaware.com/live/flight/' + airlineCode + flightNumber + '" target="_blank" rel="noopener">[FlightAware]</a>';
		} else {
			// Eğer format uygun değilse direkt uçuş kodunu kullan
			html += '&nbsp;<a href="https://www.flightaware.com/live/flight/' + cleanFlight + '" target="_blank" rel="noopener">[FlightAware]</a>';
		}
	}
	html += '<td></tr>';
	
	if (selected) {
	    if (Metric) {
        	html += '<tr><td>Altitude: ' + Math.round(selected.altitude / 3.2828) + ' m</td>';
        } else {
            html += '<tr><td>Altitude: ' + selected.altitude + ' ft</td>';
        }
    } else {
        html += '<tr><td>Altitude: n/a</td>';
    }
		
	if (selected && selected.squawk != '0000') {
		html += '<td>Squawk: ' + selected.squawk + '</td></tr>';
	} else {
	    html += '<td>Squawk: n/a</td></tr>';
	}
	
	html += '<tr><td>Speed: ' 
	if (selected) {
	    if (Metric) {
	        html += Math.round(selected.speed * 1.852) + ' km/h';
	    } else {
	        html += selected.speed + ' kt';
	    }
	} else {
	    html += 'n/a';
	}
	html += '</td>';
	
	if (selected) {
        html += '<td>ICAO (hex): ' + selected.icao + '</td></tr>';
    } else {
        html += '<td>ICAO (hex): n/a</td></tr>'; // Something is wrong if we are here
    }
    
    html += '<tr><td>Track: ' 
	if (selected && selected.vTrack) {
	    html += selected.track + '&deg;' + ' (' + normalizeTrack(selected.track, selected.vTrack)[1] +')';
	} else {
	    html += 'n/a';
	}
	html += '</td><td>&nbsp;</td></tr>';

	html += '<tr><td colspan="' + columns + '" align="center">Lat/Long: ';
	if (selected && selected.vPosition) {
	    html += selected.latitude + ', ' + selected.longitude + '</td></tr>';
	    
	    // Let's show some extra data if we have site coordinates
	    if (SiteShow) {
            var siteLatLon  = new google.maps.LatLng(SiteLat, SiteLon);
            var planeLatLon = new google.maps.LatLng(selected.latitude, selected.longitude);
            var dist = google.maps.geometry.spherical.computeDistanceBetween (siteLatLon, planeLatLon);
            
            if (Metric) {
                dist /= 1000;
            } else {
                dist /= 1852;
            }
            dist = (Math.round((dist)*10)/10).toFixed(1);
            html += '<tr><td colspan="' + columns + '" align="center">Distance from Site: ' + dist +
                (Metric ? ' km' : ' NM') + '</td></tr>';
        } // End of SiteShow
	} else {
	    if (SiteShow) {
	        html += '<tr><td colspan="' + columns + '" align="center">Distance from Site: n/a ' + 
	            (Metric ? ' km' : ' NM') + '</td></tr>';
	    } else {
    	    html += 'n/a</td></tr>';
    	}
	}

	html += '</table>';
	
	document.getElementById('plane_detail').innerHTML = html;
}

// Right now we have no means to validate the speed is good
// Want to return (n/a) when we dont have it
// TODO: Edit C code to add a valid speed flag
// TODO: Edit js code to use said flag
function normalizeSpeed(speed, valid) {
	return speed	
}

// Returns back a long string, short string, and the track if we have a vaild track path
function normalizeTrack(track, valid){
	x = []
	if ((track > -1) && (track < 22.5)) {
		x = ["North", "N", track]
	}
	if ((track > 22.5) && (track < 67.5)) {
		x = ["North East", "NE", track]
	}
	if ((track > 67.5) && (track < 112.5)) {
		x = ["East", "E", track]
	}
	if ((track > 112.5) && (track < 157.5)) {
		x = ["South East", "SE", track]
	}
	if ((track > 157.5) && (track < 202.5)) {
		x = ["South", "S", track]
	}
	if ((track > 202.5) && (track < 247.5)) {
		x = ["South West", "SW", track]
	}
	if ((track > 247.5) && (track < 292.5)) {
		x = ["West", "W", track]
	}
	if ((track > 292.5) && (track < 337.5)) {
		x = ["North West", "NW", track]
	}
	if ((track > 337.5) && (track < 361)) {
		x = ["North", "N", track]
	}
	if (!valid) {
		x = [" ", "n/a", ""]
	}
	return x
}

// Refeshes the larger table of all the planes
function refreshTableInfo() {
	var html = '<table id="tableinfo" width="100%">';
	html += '<thead style="background-color: #BBBBBB; cursor: pointer;">';
	html += '<td onclick="setASC_DESC(\'0\');sortTable(\'tableinfo\',\'0\');">ICAO</td>';
	html += '<td onclick="setASC_DESC(\'1\');sortTable(\'tableinfo\',\'1\');">Flight</td>';
	html += '<td onclick="setASC_DESC(\'2\');sortTable(\'tableinfo\',\'2\');" ' +
	    'align="right">Squawk</td>';
	html += '<td onclick="setASC_DESC(\'3\');sortTable(\'tableinfo\',\'3\');" ' +
	    'align="right">Altitude</td>';
	html += '<td onclick="setASC_DESC(\'4\');sortTable(\'tableinfo\',\'4\');" ' +
	    'align="right">Speed</td>';
        // Add distance column header to table if site coordinates are provided
        if (SiteShow && (typeof SiteLat !==  'undefined' || typeof SiteLon !==  'undefined')) {
            html += '<td onclick="setASC_DESC(\'5\');sortTable(\'tableinfo\',\'5\');" ' +
                'align="right">Distance</td>';
        }
	html += '<td onclick="setASC_DESC(\'5\');sortTable(\'tableinfo\',\'6\');" ' +
	    'align="right">Track</td>';
	html += '<td onclick="setASC_DESC(\'6\');sortTable(\'tableinfo\',\'7\');" ' +
	    'align="right">Msgs</td>';
	html += '<td onclick="setASC_DESC(\'7\');sortTable(\'tableinfo\',\'8\');" ' +
	    'align="right">Seen</td></thead><tbody>';
	for (var tablep in Planes) {
		var tableplane = Planes[tablep]
		if (!tableplane.reapable) {
			var specialStyle = "";
			// Is this the plane we selected?
			if (tableplane.icao == SelectedPlane) {
				specialStyle += " selected";
			}
			// Lets hope we never see this... Aircraft Hijacking
			if (tableplane.squawk == 7500) {
				specialStyle += " squawk7500";
			}
			// Radio Failure
			if (tableplane.squawk == 7600) {
				specialStyle += " squawk7600";
			}
			// Emergancy
			if (tableplane.squawk == 7700) {
				specialStyle += " squawk7700";
			}
			
			if (tableplane.vPosition == true) {
				html += '<tr class="plane_table_row vPosition' + specialStyle + '">';
			} else {
				html += '<tr class="plane_table_row ' + specialStyle + '">';
		    }
		    
			html += '<td>' + tableplane.icao + '</td>';
			html += '<td>' + tableplane.flight + '</td>';
			if (tableplane.squawk != '0000' ) {
    			html += '<td align="right">' + tableplane.squawk + '</td>';
    	    } else {
    	        html += '<td align="right">&nbsp;</td>';
    	    }
    	    
    	    if (Metric) {
    			html += '<td align="right">' + Math.round(tableplane.altitude / 3.2828) + '</td>';
    			html += '<td align="right">' + Math.round(tableplane.speed * 1.852) + '</td>';
    	    } else {
    	        html += '<td align="right">' + tableplane.altitude + '</td>';
    	        html += '<td align="right">' + tableplane.speed + '</td>';
    	    }
                        // Add distance column to table if site coordinates are provided
                        if (SiteShow && (typeof SiteLat !==  'undefined' || typeof SiteLon !==  'undefined')) {
                        html += '<td align="right">';
                            if (tableplane.vPosition) {
                                var siteLatLon  = new google.maps.LatLng(SiteLat, SiteLon);
                                var planeLatLon = new google.maps.LatLng(tableplane.latitude, tableplane.longitude);
                                var dist = google.maps.geometry.spherical.computeDistanceBetween (siteLatLon, planeLatLon);
                                    if (Metric) {
                                        dist /= 1000;
                                    } else {
                                        dist /= 1852;
                                    }
                                dist = (Math.round((dist)*10)/10).toFixed(1);
                                html += dist;
                            } else {
                            html += '0';
                            }
                            html += '</td>';
                        }
			
			html += '<td align="right">';
			if (tableplane.vTrack) {
    			 html += normalizeTrack(tableplane.track, tableplane.vTrack)[2];
    			 // html += ' (' + normalizeTrack(tableplane.track, tableplane.vTrack)[1] + ')';
    	    } else {
    	        html += '&nbsp;';
    	    }
    	    html += '</td>';
			html += '<td align="right">' + tableplane.messages + '</td>';
			html += '<td align="right">' + tableplane.seen + '</td>';
			html += '</tr>';
		}
	}
	html += '</tbody></table>';

	document.getElementById('planes_table').innerHTML = html;

	if (SpecialSquawk) {
    	$('#SpecialSquawkWarning').css('display', 'inline');
    } else {
        $('#SpecialSquawkWarning').css('display', 'none');
    }

	// Click event for table
	$('#planes_table').find('tr').click( function(){
		var hex = $(this).find('td:first').text();
		if (hex != "ICAO") {
			selectPlaneByHex(hex);
			refreshTableInfo();
			refreshSelected();
		}
	});

	sortTable("tableinfo");
}

// Credit goes to a co-worker that needed a similar functions for something else
// we get a copy of it free ;)
function setASC_DESC(iCol) {
	if(iSortCol==iCol) {
		bSortASC=!bSortASC;
	} else {
		bSortASC=bDefaultSortASC;
	}
}

function sortTable(szTableID,iCol) { 
	//if iCol was not provided, and iSortCol is not set, assign default value
	if (typeof iCol==='undefined'){
		if(iSortCol!=-1){
			var iCol=iSortCol;
                } else if (SiteShow && (typeof SiteLat !==  'undefined' || typeof SiteLon !==  'undefined')) {
                        var iCol=5;
		} else {
			var iCol=iDefaultSortCol;
		}
	}

	//retrieve passed table element
	var oTbl=document.getElementById(szTableID).tBodies[0];
	var aStore=[];

	//If supplied col # is greater than the actual number of cols, set sel col = to last col
	if (typeof oTbl.rows[0] !== 'undefined' && oTbl.rows[0].cells.length <= iCol) {
		iCol=(oTbl.rows[0].cells.length-1);
    }

	//store the col #
	iSortCol=iCol;

	//determine if we are delaing with numerical, or alphanumeric content
	var bNumeric = false;
	if ((typeof oTbl.rows[0] !== 'undefined') &&
	    (!isNaN(parseFloat(oTbl.rows[0].cells[iSortCol].textContent ||
	    oTbl.rows[0].cells[iSortCol].innerText)))) {
	    bNumeric = true;
	}

	//loop through the rows, storing each one inro aStore
	for (var i=0,iLen=oTbl.rows.length;i<iLen;i++){
		var oRow=oTbl.rows[i];
		vColData=bNumeric?parseFloat(oRow.cells[iSortCol].textContent||oRow.cells[iSortCol].innerText):String(oRow.cells[iSortCol].textContent||oRow.cells[iSortCol].innerText);
		aStore.push([vColData,oRow]);
	}

	//sort aStore ASC/DESC based on value of bSortASC
	if (bNumeric) { //numerical sort
		aStore.sort(function(x,y){return bSortASC?x[0]-y[0]:y[0]-x[0];});
	} else { //alpha sort
		aStore.sort();
		if(!bSortASC) {
			aStore.reverse();
	    }
	}

	//rewrite the table rows to the passed table element
	for(var i=0,iLen=aStore.length;i<iLen;i++){
		oTbl.appendChild(aStore[i][1]);
	}
	aStore=null;
}

function selectPlaneByHex(hex) {
	// If SelectedPlane has something in it, clear out the selected
	if (SelectedPlane != null) {
		Planes[SelectedPlane].is_selected = false;
		Planes[SelectedPlane].funcClearLine();
		Planes[SelectedPlane].markerColor = MarkerColor;
		// If the selected has a marker, make it not stand out
		if (Planes[SelectedPlane].marker) {
			Planes[SelectedPlane].marker.setIcon(Planes[SelectedPlane].funcGetIcon());
		}
	}

	// If we are clicking the same plane, we are deselected it.
	if (String(SelectedPlane) != String(hex)) {
		// Assign the new selected
		SelectedPlane = hex;
		Planes[SelectedPlane].is_selected = true;
		// If the selected has a marker, make it stand out
		if (Planes[SelectedPlane].marker) {
			Planes[SelectedPlane].funcUpdateLines();
			Planes[SelectedPlane].marker.setIcon(Planes[SelectedPlane].funcGetIcon());
		}
	} else { 
		SelectedPlane = null;
	}
    refreshSelected();
    refreshTableInfo();
}

function resetMap() {
    // Reset localStorage values
    localStorage.removeItem('CenterLat');
    localStorage.removeItem('CenterLon');
    localStorage.removeItem('ZoomLvl');
    
    // Reset to default values
    CenterLat = CONST_CENTERLAT;
    CenterLon = CONST_CENTERLON;
    ZoomLvl = CONST_ZOOMLVL;
    
    // Reset the map view
    GoogleMap.setZoom(parseInt(CONST_ZOOMLVL));
    GoogleMap.setCenter(new google.maps.LatLng(CONST_CENTERLAT, CONST_CENTERLON));
    
    // Clear selected plane
    if (SelectedPlane) {
        selectPlaneByHex(SelectedPlane); // This will deselect it
    }
    
    // Clear all markers and lines
    for (var plane in Planes) {
        if (Planes[plane].marker) {
            Planes[plane].marker.setMap(null);
            Planes[plane].marker = null;
        }
        if (Planes[plane].line) {
            Planes[plane].line.setMap(null);
            Planes[plane].line = null;
        }
    }
    
    // Reset Planes object
    Planes = {};
    PlanesOnMap = 0;
    PlanesOnTable = 0;
    
    // Refresh the display
    refreshTableInfo();
    refreshSelected();
}

function drawCircle(marker, distance) {
    if (typeof distance === 'undefined') {
        return false;
    }
    
    if (!(!isNaN(parseFloat(distance)) && isFinite(distance)) || distance < 0) {
        return false;
    }
    
    distance *= 1000.0; // km'yi metreye çevir
    
    // Add circle overlay and bind to marker
    var circle = new google.maps.Circle({
        map: GoogleMap,
        radius: distance,
        fillColor: SiteCirclesColors.fillColor,
        fillOpacity: SiteCirclesColors.fillOpacity,
        strokeColor: SiteCirclesColors.strokeColor,
        strokeOpacity: SiteCirclesColors.strokeOpacity,
        strokeWeight: SiteCirclesColors.strokeWeight
    });
    circle.bindTo('center', marker, 'position');
}

// Radar çizgisini oluştur ve döndür
function initRadar() {
    var center = new google.maps.LatLng(SiteLat, SiteLon);
    var radius = 100 * 1000; // 100 km'yi metreye çeviriyoruz
    
    // Ana radar çizgisini oluştur
    radarLine = new google.maps.Polyline({
        path: [
            center,
            google.maps.geometry.spherical.computeOffset(center, radius, radarAngle)
        ],
        geodesic: true,
        strokeColor: '#00FF00',
        strokeOpacity: 0.8,
        strokeWeight: 2,
        map: GoogleMap
    });

    // İz çizgilerini oluştur
    for (var i = 0; i < NUM_TRAILS; i++) {
        var trail = new google.maps.Polyline({
            path: [
                center,
                google.maps.geometry.spherical.computeOffset(center, radius, radarAngle - (i * 2))
            ],
            geodesic: true,
            strokeColor: '#00FF00',
            strokeOpacity: Math.max(0.8 - (i * TRAIL_OPACITY_STEP), 0),
            strokeWeight: 2,
            map: GoogleMap
        });
        radarTrails.push(trail);
    }

    // Radar animasyonu
    setInterval(function() {
        radarAngle = (radarAngle + 2) % 360;
        var endPoint = google.maps.geometry.spherical.computeOffset(center, radius, radarAngle);
        
        // Ana çizgiyi güncelle
        radarLine.setPath([center, endPoint]);

        // İz çizgilerini güncelle
        for (var i = 0; i < NUM_TRAILS; i++) {
            var trailAngle = (radarAngle - ((i + 1) * 2) + 360) % 360;
            var trailEnd = google.maps.geometry.spherical.computeOffset(center, radius, trailAngle);
            radarTrails[i].setPath([center, trailEnd]);
        }

        // Uçakları kontrol et ve radar çizgisinin rengini değiştir
        var isNearPlane = false;
        for (var hex in Planes) {
            var plane = Planes[hex];
            if (plane.marker && plane.circles && plane.circles.length > 0) {
                var planeAngle = google.maps.geometry.spherical.computeHeading(center, plane.marker.getPosition());
                if (planeAngle < 0) planeAngle += 360;
                
                var angleDiff = Math.abs(radarAngle - planeAngle);
                if (angleDiff < 5 || angleDiff > 355) {
                    isNearPlane = true;
                    break;
                }
            }
        }

        // Radar çizgisinin rengini güncelle
        if (isNearPlane) {
            radarLine.setOptions({
                strokeColor: '#FF0000',
                strokeOpacity: 1.0
            });
            
            // 1 saniye sonra yeşile geri dön
            setTimeout(function() {
                radarLine.setOptions({
                    strokeColor: '#00FF00',
                    strokeOpacity: 0.8
                });
            }, 1000);
        }
    }, 50);
}
