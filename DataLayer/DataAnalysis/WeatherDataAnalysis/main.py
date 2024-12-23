import requests
import psycopg2
from datetime import datetime
import time


API_KEY = "3583b31f210c210344db810cd563fd6b"
CITY_NAME = "Elazığ"
API_URL = f"http://api.openweathermap.org/data/2.5/weather?q={CITY_NAME}&appid={API_KEY}&units=metric"


DB_HOST = "localhost"
DB_PORT = "5432"
DB_NAME = "WeatherDataAnalysis"
DB_USER = "postgres"
DB_PASSWORD = "22"


def fetch_weather_data():
    try:
        response = requests.get(API_URL)
        response.raise_for_status()  
        return response.json()
    except requests.exceptions.RequestException as e:
        print(f"API isteği sırasında hata oluştu: {e}")
        return None


def save_weather_data_to_db(weather_data):
    try:
        conn = psycopg2.connect(
            host=DB_HOST,
            port=DB_PORT,
            database=DB_NAME,
            user=DB_USER,
            password=DB_PASSWORD
        )
        cursor = conn.cursor()

        description = weather_data['weather'][0]['description']
        temperature = weather_data['main']['temp']
        feels_like = weather_data['main']['feels_like']
        humidity = weather_data['main']['humidity']
        wind_speed = weather_data['wind']['speed']
        pressure = weather_data['main']['pressure']
        clouds = weather_data['clouds']['all']
        visibility = weather_data.get('visibility', 0)
        weather_id = weather_data['weather'][0]['id']
        sunrise = datetime.fromtimestamp(weather_data['sys']['sunrise'])
        sunset = datetime.fromtimestamp(weather_data['sys']['sunset'])
        lat = weather_data['coord']['lat']
        lon = weather_data['coord']['lon']
        city = weather_data['name']
        record_datetime = datetime.now()

      
        cursor.execute("""
        CREATE TABLE IF NOT EXISTS weather_data (
            id SERIAL PRIMARY KEY,
            description VARCHAR(255),
            temperature FLOAT,
            feelsLike FLOAT,
            humidity INT,
            windSpeed FLOAT,
            pressure INT,
            clouds INT,
            visibility INT,
            weatherId INT,
            sunrise TIMESTAMP,
            sunset TIMESTAMP,
            lat FLOAT,
            lon FLOAT,
            city VARCHAR(255),
            recordDateTime TIMESTAMP
        )
        """)

       
        cursor.execute("""
        INSERT INTO weather_data (
            description, temperature, feelsLike, humidity, windSpeed, pressure,
            clouds, visibility, weatherId, sunrise, sunset, lat, lon, city, recordDateTime
        ) VALUES (%s, %s, %s, %s, %s, %s, %s, %s, %s, %s, %s, %s, %s, %s, %s)
        """, (
            description, temperature, feels_like, humidity, wind_speed, pressure,
            clouds, visibility, weather_id, sunrise, sunset, lat, lon, city, record_datetime
        ))

        conn.commit()
        print(f"{record_datetime}: Veri başarıyla kaydedildi!")
    except psycopg2.Error as e:
        print(f"Veritabanı hatası: {e}")
    finally:
        if conn:
            cursor.close()
            conn.close()

if __name__ == "__main__":
    while True:
        try:
          
            weather_data = fetch_weather_data()
            if weather_data:
                save_weather_data_to_db(weather_data)
            else:
                print("Veriler alınamadı, tekrar denenecek.")

          
            print("Bir sonraki veri çekme işlemi için 10 dakika bekleniyor...")
            time.sleep(600)  

        except KeyboardInterrupt:
            print("\nProgram durduruldu.")
            break
        except Exception as e:
            print(f"Beklenmeyen bir hata oluştu: {e}")
            break
