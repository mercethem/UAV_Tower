#include <SPI.h>
#include <RF24.h>
#include <Arduino.h>

// L298N pin tanımlamaları
#define IN4 10    // Sağ-sol motor
#define IN3 3     // Sağ-sol motor
#define ENA 5     // Arka motor hız kontrol
#define IN2 6     // Arka motor
#define IN1 4     // Arka motor


// NRF24 pin tanımlamaları
RF24 radio(8, 7); // NRF24 modülünün CE ve CSN pinleri
const byte address[6] = "00001"; // Adres tanımlaması

void setup() {
    Serial.begin(9600); // Seri haberleşmeyi başlat

    // Pinleri çıkış olarak ayarla
    pinMode(IN1, OUTPUT);
    pinMode(IN2, OUTPUT);
    pinMode(ENA, OUTPUT);
    pinMode(IN3, OUTPUT);
    pinMode(IN4, OUTPUT);
    pinMode(esp32, OUTPUT);

    // NRF24 kurulumu
    radio.begin();
    radio.openReadingPipe(1, address);
    radio.setPALevel(RF24_PA_HIGH);
    radio.startListening(); // Dinleme moduna geç
    goStraight();
    delay(2000);
    // Motorları durdur
    stopMotors();
}

void loop() {
    // Veri varsa oku
    if (radio.available()) {
        char command[20] = ""; // Gelen komut için tampon dizi
        radio.read(&command, sizeof(command));

        // Gelen komut boşsa atla
        if (strlen(command) == 0) {
            return;
        }
       
        // Komutu kontrol et ve ilgili işlevi çağır
        if (strcmp(command, "turnLeft") == 0) {
            turnLeft();
        } else if (strcmp(command, "turnRight") == 0) {
            turnRight();
        } else if (strcmp(command, "start") == 0) {
            goStraight();
        } else if (strcmp(command, "stopMotors") == 0) {
            stopMotors();
        } else {
            Serial.println("Bilinmeyen komut: ");
            Serial.println(command);
        }
    }
}

void turnRight() {
    // Sağa dönüş
    Serial.println("Sağa dönüş");
    digitalWrite(IN1, HIGH); // Arka motor ileri
    digitalWrite(IN2, LOW);
    digitalWrite(IN3, LOW); // Sağ-sol motor sağa dön
    digitalWrite(IN4, HIGH);
    analogWrite(ENA, 235);

}

void turnLeft() {
    // Sola dönüş
    Serial.println("Sola dönüş");
    digitalWrite(IN1, HIGH); // Arka motor ileri
    digitalWrite(IN2, LOW);
    digitalWrite(IN3, HIGH); // Sağ-sol motor sola dön
    digitalWrite(IN4, LOW);
    analogWrite(ENA, 235);


}

void stopMotors() {
    // Her iki motoru durdur
    Serial.println("Motorlar durduruluyor");
    digitalWrite(IN1, LOW);
    digitalWrite(IN2, LOW);
    digitalWrite(IN3, LOW);
    digitalWrite(IN4, LOW);
    analogWrite(ENA, 0);
}

void goStraight() {
    // Düz ileri hareket
    Serial.println("Düz gidiyor");
    digitalWrite(IN1, HIGH); // Arka motor ileri
    digitalWrite(IN2, LOW);
    digitalWrite(IN3, LOW); // Sağ-sol motor düz tut
    digitalWrite(IN4, LOW);
    analogWrite(ENA, 235); // Hız ayarı
}
