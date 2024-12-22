// --------------------------------------------------------
//
// This file is to configure the configurable settings.
// Load this file before script.js file at gmap.html.
//
// --------------------------------------------------------

// -- Output Settings -------------------------------------
// Show metric values
Metric = false; // true or false

// -- Map settings ----------------------------------------
// The Latitude and Longitude in decimal format
CONST_CENTERLAT = 45.0;
CONST_CENTERLON = 9.0;
// The google maps zoom level, 0 - 16, lower is further out
CONST_ZOOMLVL   = 5;

// -- Marker settings -------------------------------------
// The default marker color
MarkerColor	  = "rgb(127, 127, 127)";
SelectedColor = "rgb(225, 225, 225)";
StaleColor = "rgb(190, 190, 190)";

// -- Site Settings ---------------------------------------
SiteShow = true;

// Varsayılan koordinatlar (Elazığ Havalimanı)
DEFAULT_SITE_LAT = 38.597275;
DEFAULT_SITE_LON = 39.291412;

// Güncel konum bilgisi için değişkenler
SiteLat = DEFAULT_SITE_LAT;  // Başlangıçta varsayılan değerler
SiteLon = DEFAULT_SITE_LON;

SiteCircles = true;
// 10'ar km aralıklarla 100 km'ye kadar daireler
SiteCirclesDistances = new Array(10, 20, 30, 40, 50, 60, 70, 80, 90, 100);  

// Dairelerin görünüm ayarları
SiteCirclesColors = {
    strokeColor: "#00FF00",
    strokeOpacity: 0.3,
    strokeWeight: 1,
    fillColor: "#00FF00",
    fillOpacity: 0.02
};

