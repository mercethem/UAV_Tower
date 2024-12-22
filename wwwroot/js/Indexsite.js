
function openURL(url, button) {
    const newTab = window.open(url, '_blank');
    button.classList.add('active');
    const interval = setInterval(() => {
        if (newTab.closed) {
            button.classList.remove('active');
            clearInterval(interval);
        }
    }, 500);
}
const apiKey = "3583b31f210c210344db810cd563fd6b"; // OpenWeather API anahtarınızı buraya ekleyin
const city = "Elazig"; // Şehir ismini buraya ekleyin

// Hava durumu verisini OpenWeather API'den çekme
async function fetchWeather() {
    try {
        const response = await fetch(`https://api.openweathermap.org/data/2.5/weather?q=${city}&appid=${apiKey}&units=metric`);
        const data = await response.json();

        if (data.cod === 200) {
            const weatherInfo = document.getElementById('weatherInfo');
            const temp = data.main.temp;
            const feelsLike = data.main.feels_like;
            const description = data.weather[0].description;
            const windSpeed = data.wind.speed;
            const humidity = data.main.humidity;
            const pressure = data.main.pressure;
            const visibility = data.visibility / 1000; // Görüş mesafesi km cinsinden
            const clouds = data.clouds.all;
            const sunrise = new Date(data.sys.sunrise * 1000).toLocaleTimeString();
            const sunset = new Date(data.sys.sunset * 1000).toLocaleTimeString();

            // Hava durumu bilgilerini dinamik olarak güncelleme
            const weatherHtml = `
                                <p><strong>Temperature:</strong> ${temp}°C</p>
                                <p><strong>Feels Like:</strong> ${feelsLike}°C</p>
                                <p><strong>Condition:</strong> ${description}</p>
                                <p><strong>Wind Speed:</strong> ${windSpeed} m/s</p>
                                <p><strong>Humidity:</strong> ${humidity}%</p>
                                <p><strong>Pressure:</strong> ${pressure} hPa</p>
                                <p><strong>Visibility:</strong> ${visibility} km</p>
                                <p><strong>Clouds:</strong> ${clouds}%</p>
                                <p><strong>Sunrise:</strong> ${sunrise}</p>
                                <p><strong>Sunset:</strong> ${sunset}</p>
                            `;
            weatherInfo.innerHTML = weatherHtml;
        } else {
            weatherInfo.innerHTML = "Could not retrieve weather data.";
        }
    } catch (error) {
        console.error('Error fetching weather data:', error);
        document.getElementById('weatherInfo').innerHTML = "Error fetching weather data.";
    }
}

// Sayfa yüklendiğinde hava durumu verisini al
window.onload = fetchWeather;

// Hava durumu verisini her 5 dakikada bir güncelle
setInterval(fetchWeather, 300000); // 300000 ms = 5 dakika

// Saat ve tarih bilgisini her saniyede bir güncelle
function updateTimeAndDate() {
    const timeElement = document.getElementById('timeText');
    const dayElement = document.getElementById('dayText');

    const now = new Date();
    const hours = now.getHours();
    const minutes = now.getMinutes();
    const ampm = hours >= 12 ? 'PM' : 'AM';
    const hour = hours % 12 || 12;
    const minute = minutes < 10 ? '0' + minutes : minutes;

    const dayOptions = { weekday: 'long', month: 'long', day: 'numeric' };
    const day = now.toLocaleDateString('en-US', dayOptions);

    timeElement.innerHTML = `${hour}:${minute} <span class="time-sub-text">${ampm}</span>`;
    dayElement.innerHTML = day;
}

setInterval(updateTimeAndDate, 1000); // Saat ve tarihi her saniyede bir güncelle
