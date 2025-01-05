# UAV Tower (Unmanned Aerial Vehicle Tower)

**Developed by:**
- Ethem MERÇ (Scrum Master)
- Berkan BAĞIT
- Hivda KORKMAZ

## Features

The project is structured around four key components:

    RF technology
    PC technology
    Image processing
    Embedded systems

The project documentation can be accessed through the following link:
> https://github.com/mercethem/UAV_Tower/tree/main/Documents


- **Real-Time Data Visualization:**
  - Detects the positions of aircraft using ADS-B (Automatic Dependent Surveillance-Broadcast) data and presents this data as a real-time radar image on a map. The highest detected flight altitude in the current system is 41,000 feet.
  - Can visualize in radar and thermal modes using satellite imagery.
  - Images are refreshed every two hours.
  
- **Database Integration:**  
  - Aircraft data can be stored in three different database management systems: PostgreSQL, MongoDB, and Redis.
  - Database backup operations are automatically performed each time the program is started and on the 1st of every month.
  
- **Weather Data:**  
  - In addition to aircraft data, the project can receive weather data, visualize it, and create graphs.

- **High Performance:**  
  - The system processes flight data quickly and effectively to provide real-time information to users.

## Requirements

- **Docker:** The databases run in Docker containers.
- **Veritabanları:** PostgreSQL, MongoDB, Redis (can be installed via Docker).
- **Dump1090 Calibration: Dump1090 calibration needs to be performed to receive flight data.

## Installation

The project runs in an environment where the required databases are hosted on Docker. You can quickly set up and run the project by following these steps.

### 1. Clone the Project with Git Client :octocat:
 
```bash
cd C:\
```

```bash
git clone https://github.com/mercethem/UAV_Tower.git
```

### 2. Download Docker Images 🐋 

You can use the following commands to pull the necessary Docker images for the UAV Tower databases:

> https://hub.docker.com/r/ethemmerc/uav_tower/tags
    
```bash
docker pull ethemmerc/uav_tower:my-mongodb
```
    
```bash
docker pull ethemmerc/uav_tower:my-redis
```
    
```bash
docker pull ethemmerc/uav_tower:my-postgres
```
### 3. Dump1090 Calibration 📡

To receive the data stream, Dump1090 needs to be properly calibrated. Run the following calibration command:

```bash
--interactive --net --net-ro-port 30002 --net-beast --mlat --gain 48.0 --quiet --ppm 53 --net-http-port 30003
```
Usage

After setting up the project, you can receive and visualize flight data. Flight data can be provided through port 30003.


### 4. Sending Flight Data 🛫

Provide the flight data in the correct format.
Transmit the data through port 30003.
Once the data is received, it will be displayed instantly on the visualization screen.

You can view satellite images in radar and thermal modes in two different ways from the main screen. The images will refresh every two hours.

> Database Backup: The system automatically performs a database backup each time it is started and at 00:00 on the 1st of every month.

-----------------------------------

License

This project is licensed under the MIT License. For more details, please refer to the LICENSE file.

Acknowledgements

> :warning: We would like to thank especially Salvatore Sanfilippo and the sat24 team, as well as all those who have supported the successful execution of this project. Their strong features and help have made significant contributions to our project.


