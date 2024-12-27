#include "esp_camera.h"
#include <WiFi.h>

// Wi-Fi bilgileri
#define WIFI_SSID "*******"
#define WIFI_PASSWORD "**********"

// Kamera modeli
#define CAMERA_MODEL_AI_THINKER // PSRAM var

#include "camera_pins.h"

void startCameraServer();

void setup() {
  Serial.begin(115200);
  Serial.println();

  // Kamera yapılandırması
  camera_config_t config;
  config.ledc_channel = LEDC_CHANNEL_0;
  config.ledc_timer = LEDC_TIMER_0;
  config.pin_d0 = Y2_GPIO_NUM;
  config.pin_d1 = Y3_GPIO_NUM;
  config.pin_d2 = Y4_GPIO_NUM;
  config.pin_d3 = Y5_GPIO_NUM;
  config.pin_d4 = Y6_GPIO_NUM;
  config.pin_d5 = Y7_GPIO_NUM;
  config.pin_d6 = Y8_GPIO_NUM;
  config.pin_d7 = Y9_GPIO_NUM;
  config.pin_xclk = XCLK_GPIO_NUM;
  config.pin_pclk = PCLK_GPIO_NUM;
  config.pin_vsync = VSYNC_GPIO_NUM;
  config.pin_href = HREF_GPIO_NUM;
  config.pin_sscb_sda = SIOD_GPIO_NUM;
  config.pin_sscb_scl = SIOC_GPIO_NUM;
  config.pin_pwdn = PWDN_GPIO_NUM;
  config.pin_reset = RESET_GPIO_NUM;
  config.xclk_freq_hz = 20000000;
  config.pixel_format = PIXFORMAT_JPEG;

  // PSRAM kontrolü
  if (psramFound()) {
    config.frame_size = FRAMESIZE_VGA;  // 640x480 çözünürlük
    config.jpeg_quality = 15;           // Orta kalite, hızlı akış
    config.fb_count = 2;
  } else {
    config.frame_size = FRAMESIZE_QVGA; // 320x240 çözünürlük
    config.jpeg_quality = 20;           // Daha düşük kalite
    config.fb_count = 1;
  }

  // Kamera başlatma
  esp_err_t err = esp_camera_init(&config);
  if (err != ESP_OK) {
    Serial.printf("Kamera başlatılamadı, hata kodu: 0x%x", err);
    ESP.restart();
  }

  sensor_t *s = esp_camera_sensor_get();
  s->set_framesize(s, FRAMESIZE_VGA); // VGA çözünürlük

  // Wi-Fi bağlantısı
  WiFi.begin(WIFI_SSID, WIFI_PASSWORD);
  Serial.print("Wi-Fi bağlanıyor...");

  int retry = 0;
  while (WiFi.status() != WL_CONNECTED && retry < 10) {
    delay(500);
    Serial.print(".");
    retry++;
  }

  if (WiFi.status() != WL_CONNECTED) {
    Serial.println("Wi-Fi bağlantısı başarısız, yeniden başlatılıyor...");
    ESP.restart();
  }

  Serial.println("\nWi-Fi bağlantısı başarılı!");
  Serial.print("IP Adresi: ");
  Serial.println(WiFi.localIP());

  // Kamera sunucusunu başlat
  startCameraServer();
  Serial.println("Kamera hazır!");
  Serial.print("Görüntü için 'http://");
  Serial.print(WiFi.localIP());
  Serial.println("/stream' adresine gidin.");
}

void loop() {
  delay(10000); // Döngü boşta
}
