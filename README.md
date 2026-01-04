
# EShop Microservices

A .NET-based e-commerce system built using a microservices architecture.  
This project focuses on clean service boundaries, CQRS, and running distributed systems with Docker.

## Architecture

### BuildingBlocks
Shared infrastructure for CQRS, MediatR pipeline behaviors, validation, logging, and exception handling.

### Services

- **Catalog.API**  
  Product catalog management  
  *PostgreSQL + Marten*

- **Basket.API**  
  Shopping basket service  
  *PostgreSQL + Redis*  
  Communicates with the Discount service via **gRPC**

- **Discount.Grpc**  
  Discount calculation service  
  *SQLite + EF Core*

- **API Gateway**  
  YARP-based gateway acting as a single entry point

---

## Run with Docker

```bash
git clone https://github.com/SarathiPrabu/EShopMicroservices.git
cd EShopMicroservices/src

docker compose -p eshop up --build -d
````

---

## Service Ports

* Catalog API: [http://localhost:6000](http://localhost:6000)
* Basket API: [http://localhost:6001](http://localhost:6001)
* Discount gRPC: [https://localhost:6062](https://localhost:6062)

---

## Database Reset (if needed)

> **Note:** This will remove existing SQLite data for the Discount service.

```bash
docker compose down
docker volume rm src_sqlite_discount
docker compose up --build
```

---

## Stop Services

```bash
docker compose down
```

