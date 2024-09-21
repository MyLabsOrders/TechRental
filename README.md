## About
**TechRental** is an application for renting special vehicles. It provides desktop and web graphical interface and supports work from both user and administrator sides.

## Quick Start
Follow these steps:
1. Clone the repository and open its root folder.
   - `git clone https://github.com/MyLabsOrders/TechRental.git`
   - `cd TechRental`
2. Run server.
   - `docker compose -f server/Docker/docker-compose.yml up -d --build`
4. Run database data generator.
   - `dotnet run --project server/Utils/DatabaseDataGeneration`
6. Run desktop or web application.
   - `dotnet run --project desktop/RentDesktop`
