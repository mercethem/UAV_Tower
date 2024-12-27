#include <SPI.h>
#include <RF24.h>
#include <Arduino.h>

RF24 radio(8, 7); // NRF24 modülünün CE ve CSN pinleri
const byte address[6] = "00001"; // Adres tanımlaması

void setup() {
    Serial.begin(9600);
    radio.begin();
    radio.openWritingPipe(address);
    radio.setPALevel(RF24_PA_HIGH);
    radio.stopListening(); // Başlangıçta dinlemeyi durdur
}

void loop() {
    if (Serial.available()) {
        String command = Serial.readStringUntil('\n');
        
        if (command == "turnLeft" || command == "turnRight" || command == "start" || command == "goStraight" || command == "stopMotors") {
            radio.stopListening();
            char cmd[20]; // Komutun uzunluğuna göre bir dizi tanımla
            command.toCharArray(cmd, sizeof(cmd)); // String'i char dizisine çevir
            radio.write(&cmd, sizeof(cmd));
            Serial.print("Gönderildi: ");
            Serial.println(command);
            radio.startListening();
        }
    }
}