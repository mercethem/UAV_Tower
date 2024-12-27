import cv2
import numpy as np
import urllib.request
import serial
import serial.tools.list_ports
import time

# ESP32-CAM URL
url = "http://192.168.233.103/capture"

# Seri portları tarayıp Arduino'yu bul

def connect_to_arduino():
    ports = list(serial.tools.list_ports.comports())
    if not ports:
        print("Hiçbir seri port bulunamadı. Arduino bağlı mı?")
        return None

    for port in ports:
        if "USB-SERIAL" in port.description or "CH340" in port.description:
            try:
                arduino = serial.Serial(port.device, 9600)
                time.sleep(2)  # Bağlantının oturması için bekle
                print(f"Bağlantı kuruldu: {port.device}")
                return arduino
            except Exception as e:
                print(f"{port.device} portuna bağlanılamadı: {e}")

    print("Arduino bulunamadı.")
    return None

# Arduino bağlantısını kur
arduino = connect_to_arduino()
if arduino is None:
    exit()  # Arduino bulunmazsa çık

# Renk aralıkları (HSV formatında)
color_ranges = {
    "red": (np.array([0, 120, 70]), np.array([10, 255, 255])),
    "blue": (np.array([100, 150, 0]), np.array([140, 255, 255])),
    "green": (np.array([40, 40, 40]), np.array([70, 255, 255]))
}

# Kullanıcıdan hedef rengi al
def get_target_color():
    while True:
        target_color = input("Tespit etmek istediğiniz rengi girin (red, blue, green): ").strip().lower()
        if target_color in color_ranges:
            return target_color
        else:
            print("Geçersiz renk. Lütfen 'red', 'blue' veya 'green' girin.")

# Kullanıcıdan renk seçimi al
target_color = get_target_color()

# 'start' komutunu Arduino'ya gönder
if arduino and arduino.is_open:
    arduino.write("start\n".encode())

# Önceki merkez pozisyonu saklamak için
previous_center = None
no_color_detected = 0  # Renk algılanmazsa sayacı başlat

while True:
    try:
        # ESP32-CAM'den görüntüyü çek
        img_resp = urllib.request.urlopen(url, timeout=5)
        img_np = np.array(bytearray(img_resp.read()), dtype=np.uint8)
        frame = cv2.imdecode(img_np, -1)

        # Görüntüyü HSV'ye çevir
        hsv = cv2.cvtColor(frame, cv2.COLOR_BGR2HSV)

        # Hedef renk aralığını belirle
        lower, upper = color_ranges.get(target_color, (None, None))

        if lower is not None and upper is not None:
            mask = cv2.inRange(hsv, lower, upper)
            contours, _ = cv2.findContours(mask, cv2.RETR_EXTERNAL, cv2.CHAIN_APPROX_SIMPLE)

            if contours:
                no_color_detected = 0  # Renk algılandı, sayacı sıfırla

                for contour in contours:
                    if cv2.contourArea(contour) > 200:  # Daha küçük konturları da algılamak için eşik düşürüldü
                        (x, y), radius = cv2.minEnclosingCircle(contour)
                        center = (int(x), int(y))
                        radius = int(radius)

                        cv2.circle(frame, center, radius, (0, 255, 255), 2)
                        cv2.circle(frame, center, 5, (0, 255, 0), -1)

                        if previous_center is not None:
                            dx = center[0] - previous_center[0]
                            dy = center[1] - previous_center[1]

                            # Yön tespiti
                            if abs(dx) > 15:
                                if dx > 0:
                                    direction = "turnRight"
                                else:
                                    direction = "turnLeft"
                                cv2.putText(frame, direction, (10, 30), cv2.FONT_HERSHEY_SIMPLEX, 1, (0, 0, 255), 2)
                                print(direction)
                                if arduino and arduino.is_open:
                                    arduino.write((direction + "\n").encode())

                            elif abs(dx) <= 15 and abs(dy) <= 15:
                                direction = "goStraight"
                                cv2.putText(frame, direction, (10, 30), cv2.FONT_HERSHEY_SIMPLEX, 1, (0, 255, 0), 2)
                                print(direction)
                                if arduino and arduino.is_open:
                                    arduino.write((direction + "\n").encode())

                        previous_center = center
                        break
            if not contours:
                # Eğer hiçbir renk bulunmazsa motorları hemen durdur
                direction = "stopMotors"
                print("Renk algılanamadı, motorları durdur.")
                if arduino and arduino.is_open:
                    arduino.write((direction + "\n").encode())

        cv2.imshow('Color Detection and Tracking', frame)
        if cv2.waitKey(1) & 0xFF == ord('q'):
            break

    except urllib.error.URLError as e:
        print(f"ESP32-CAM bağlantı hatası: {e}")
        time.sleep(1)
    except serial.SerialException as e:
        print(f"Arduino bağlantı hatası: {e}")
        arduino = connect_to_arduino()
        time.sleep(1)

# Kaynakları serbest bırak
cv2.destroyAllWindows()
if arduino and arduino.is_open:
    arduino.close()
