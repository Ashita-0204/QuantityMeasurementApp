# Quantity Measurement App - Microservices Architecture

This is a **streamlined microservices architecture** for the Quantity Measurement Application with 3 backend services + Redis cache.

## 🏗️ Architecture Overview

```
┌─────────────────────────────────────────────────────────────────┐
│                        API GATEWAY                              │
│                      (Port 5000)                                │
│  Routes: /api/auth, /api/user, /api/quantitymeasurement        │
└───────────────┬─────────────────────┬───────────────────────────┘
                │                     │
       ┌────────▼────────┐   ┌────────▼────────┐
       │  AUTH-SERVICE   │   │   QMA-SERVICE   │
       │   (Port 5001)   │   │   (Port 5002)   │
       │                 │   │                 │
       │  • Login        │   │  • Add          │
       │  • Register     │   │  • Subtract     │
       │  • Google OAuth │   │  • Divide       │
       │  • Profile      │   │  • Compare      │
       │  • Delete Acct  │   │  • Convert      │
       │                 │   │  • History      │
       └────────┬────────┘   └────────┬────────┘
                │                     │
                │                     │
       ┌────────▼─────────────────────▼────────┐
       │          PostgreSQL (Port 5432)       │
       │  Tables: users, measurements          │
       └───────────────────────────────────────┘
                         │
                ┌────────▼────────┐
                │  Redis Cache    │
                │  (Port 6379)    │
                └─────────────────┘
```

### Services Breakdown

| Service | Port | Responsibilities | Database |
|---------|------|------------------|----------|
| **api-gateway** | 5000 | Routes requests, CORS, JWT validation | None |
| **auth-service** | 5001 | Authentication, user management | PostgreSQL (users) |
| **qma-service** | 5002 | Quantity operations, history storage | PostgreSQL (measurements) + Redis |
| **postgres** | 5432 | Persistent data storage | - |
| **redis** | 6379 | Caching layer | - |

## 📋 Prerequisites

### Required Software
- **Docker** (v20.10+) and **Docker Compose** (v2.0+)
- **.NET 10 SDK** (for local development)
- **PostgreSQL** (if running without Docker)
- **Redis** (if running without Docker)

### Optional
- **Postman** or **curl** for API testing
- **DBeaver** or **pgAdmin** for database inspection

## 🚀 Quick Start with Docker

### 1. Clone or Extract the Project

```bash
cd microservices-qma
```

### 2. Configure Environment Variables

Edit the `docker-compose.yml` file and update these values:

```yaml
x-jwt-env: &jwt-env
  Jwt__Key: "your-super-secret-jwt-key-min-32-chars-change-this"

x-google-env: &google-env
  Authentication__Google__ClientId: "YOUR_GOOGLE_CLIENT_ID"
  Authentication__Google__ClientSecret: "YOUR_GOOGLE_CLIENT_SECRET"
```

**Important:** Generate a strong JWT key (minimum 32 characters). Example:
```bash
openssl rand -base64 32
```

### 3. Build and Start All Services

```bash
# Build all Docker images
docker-compose build

# Start all services in detached mode
docker-compose up -d

# View logs
docker-compose logs -f
```

### 4. Verify Services are Running

```bash
# Check container status
docker-compose ps

# Expected output:
# NAME                 STATUS    PORTS
# qma-api-gateway      Up        0.0.0.0:5000->8080/tcp
# qma-auth-service     Up        0.0.0.0:5001->8080/tcp
# qma-service          Up        0.0.0.0:5002->8080/tcp
# qma-postgres         Up        0.0.0.0:5432->5432/tcp
# qma-redis            Up        0.0.0.0:6379->6379/tcp
```

### 5. Access the Services

- **API Gateway Swagger:** http://localhost:5000/swagger
- **Auth Service Swagger:** http://localhost:5001/swagger
- **QMA Service Swagger:** http://localhost:5002/swagger

### 6. Test the API

**Register a new user:**
```bash
curl -X POST http://localhost:5000/api/auth/register \
  -H "Content-Type: application/json" \
  -d '{
    "username": "testuser",
    "email": "test@example.com",
    "password": "Password123"
  }'
```

**Login:**
```bash
curl -X POST http://localhost:5000/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{
    "email": "test@example.com",
    "password": "Password123"
  }'
```

**Convert units (anonymous):**
```bash
curl -X POST http://localhost:5000/api/quantitymeasurement/convert \
  -H "Content-Type: application/json" \
  -d '{
    "value": 100,
    "fromUnit": "centimeter",
    "toUnit": "meter",
    "category": "Length"
  }'
```

### 7. Stop Services

```bash
# Stop all services
docker-compose down

# Stop and remove volumes (WARNING: deletes all data)
docker-compose down -v
```

## 🖥️ Local Development Setup (Without Docker)

### 1. Start PostgreSQL

```bash
# Using Docker for PostgreSQL only
docker run -d \
  --name qma-postgres \
  -e POSTGRES_USER=postgres \
  -e POSTGRES_PASSWORD=postgres \
  -e POSTGRES_DB=qma_db \
  -p 5432:5432 \
  postgres:16-alpine
```

### 2. Start Redis

```bash
# Using Docker for Redis only
docker run -d \
  --name qma-redis \
  -p 6379:6379 \
  redis:7-alpine
```

### 3. Run Each Service

**Terminal 1 - Auth Service:**
```bash
cd auth-service
dotnet restore
dotnet run
# Runs on http://localhost:5001
```

**Terminal 2 - QMA Service:**
```bash
cd qma-service
dotnet restore
dotnet run
# Runs on http://localhost:5002
```

**Terminal 3 - API Gateway:**
```bash
cd api-gateway
dotnet restore
dotnet run
# Runs on http://localhost:5000
```

### 4. Configure Environment Variables (Local)

Create `appsettings.Development.json` in each service:

**auth-service/appsettings.Development.json:**
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=qma_db;Username=postgres;Password=postgres"
  },
  "Jwt": {
    "Key": "your-development-jwt-key-minimum-32-characters-required",
    "Issuer": "QuantityMeasurementApi",
    "Audience": "QuantityMeasurementApp"
  },
  "Authentication": {
    "Google": {
      "ClientId": "YOUR_GOOGLE_CLIENT_ID",
      "ClientSecret": "YOUR_GOOGLE_CLIENT_SECRET"
    }
  },
  "Frontend": {
    "BaseUrl": "http://localhost:4200"
  }
}
```

**qma-service/appsettings.Development.json:**
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=qma_db;Username=postgres;Password=postgres",
    "Redis": "localhost:6379"
  },
  "Jwt": {
    "Key": "your-development-jwt-key-minimum-32-characters-required",
    "Issuer": "QuantityMeasurementApi",
    "Audience": "QuantityMeasurementApp"
  }
}
```

**api-gateway/appsettings.Development.json:**
```json
{
  "Services": {
    "AuthService": "http://localhost:5001",
    "QmaService": "http://localhost:5002"
  },
  "Jwt": {
    "Key": "your-development-jwt-key-minimum-32-characters-required",
    "Issuer": "QuantityMeasurementApi",
    "Audience": "QuantityMeasurementApp"
  },
  "Frontend": {
    "BaseUrl": "http://localhost:4200"
  }
}
```

## 🌐 API Endpoints Reference

### Authentication Endpoints (via api-gateway)

| Method | Endpoint | Description | Auth Required |
|--------|----------|-------------|---------------|
| POST | `/api/auth/register` | Register new user | No |
| POST | `/api/auth/login` | Login with email/password | No |
| GET | `/api/auth/google-login` | Initiate Google OAuth | No |
| GET | `/api/auth/google-callback` | Google OAuth callback | No |
| POST | `/api/auth/refresh-token` | Refresh JWT token | Yes |

### User Management Endpoints

| Method | Endpoint | Description | Auth Required |
|--------|----------|-------------|---------------|
| GET | `/api/user/profile` | Get user profile | Yes |
| DELETE | `/api/user/account` | Delete user account | Yes |

### Quantity Measurement Endpoints

| Method | Endpoint | Description | Auth Required |
|--------|----------|-------------|---------------|
| POST | `/api/quantitymeasurement/add` | Add two quantities | No |
| POST | `/api/quantitymeasurement/subtract` | Subtract quantities | No |
| POST | `/api/quantitymeasurement/divide` | Divide quantities | No |
| POST | `/api/quantitymeasurement/compare` | Compare quantities | No |
| POST | `/api/quantitymeasurement/convert` | Convert units | No |

### History Endpoints

| Method | Endpoint | Description | Auth Required |
|--------|----------|-------------|---------------|
| POST | `/api/quantitymeasurement/save` | Save operation to history | Yes |
| POST | `/api/quantitymeasurement/save-batch` | Save multiple operations | Yes |
| GET | `/api/quantitymeasurement/history` | Get user's history | Yes |
| DELETE | `/api/quantitymeasurement/history/clear` | Clear all history | Yes |
| DELETE | `/api/quantitymeasurement/history/{id}` | Delete single history item | Yes |

## 🗄️ Database Schema

### users table (auth-service)
```sql
CREATE TABLE users (
    id SERIAL PRIMARY KEY,
    username VARCHAR(255) NOT NULL UNIQUE,
    email VARCHAR(255) NOT NULL UNIQUE,
    password_hash VARCHAR(255) NOT NULL,
    google_id VARCHAR(255),
    created_at TIMESTAMP NOT NULL DEFAULT NOW()
);
```

### measurements table (qma-service)
```sql
CREATE TABLE measurements (
    id UUID PRIMARY KEY,
    user_id INTEGER NOT NULL,
    operation VARCHAR(50) NOT NULL,
    timestamp TIMESTAMP NOT NULL,
    operand1_value DOUBLE PRECISION,
    operand1_unit VARCHAR(50),
    operand1_category VARCHAR(50),
    operand2_value DOUBLE PRECISION,
    operand2_unit VARCHAR(50),
    result_value DOUBLE PRECISION,
    result_unit VARCHAR(50),
    result_category VARCHAR(50),
    bool_result BOOLEAN,
    scalar_result DOUBLE PRECISION
);

CREATE INDEX idx_measurements_user_id ON measurements(user_id);
CREATE INDEX idx_measurements_timestamp ON measurements(timestamp);
```

## 🔧 Configuration Guide

### Google OAuth Setup

1. Go to [Google Cloud Console](https://console.cloud.google.com/)
2. Create a new project or select existing
3. Enable Google+ API
4. Create OAuth 2.0 credentials
5. Add authorized redirect URIs:
   - Local: `http://localhost:5001/signin-google`
   - Production: `https://your-domain.com/signin-google`
6. Copy Client ID and Client Secret to your configuration

### JWT Configuration

The JWT key must be:
- Minimum 32 characters
- Same across all services
- Kept secret (never commit to git)

Generate a secure key:
```bash
# Using openssl
openssl rand -base64 32

# Using PowerShell
[Convert]::ToBase64String((1..32 | ForEach-Object { Get-Random -Maximum 256 }))
```

## 🐛 Troubleshooting

### Issue: Services can't connect to database

**Solution:**
```bash
# Check if PostgreSQL is running
docker-compose ps postgres

# View PostgreSQL logs
docker-compose logs postgres

# Restart PostgreSQL
docker-compose restart postgres
```

### Issue: Port already in use

**Solution:**
```bash
# Find process using port 5000 (Linux/Mac)
lsof -ti:5000 | xargs kill -9

# Find process using port 5000 (Windows)
netstat -ano | findstr :5000
taskkill /PID <PID> /F
```

### Issue: Migrations not running

**Solution:**
```bash
# Manually run migrations
cd auth-service
dotnet ef database update

cd ../qma-service
dotnet ef database update
```

### Issue: Redis connection failed

**Solution:**
```bash
# Check if Redis is running
docker-compose ps redis

# Test Redis connection
docker exec -it qma-redis redis-cli ping
# Should return: PONG
```

### Issue: CORS errors in browser

**Solution:**
Update `api-gateway/appsettings.json` or `docker-compose.yml` to include your frontend URL:
```yaml
Frontend__BaseUrl: "http://localhost:4200"  # or your Angular dev server URL
```

## 📦 Building for Production

### 1. Update docker-compose.yml

```yaml
# Change exposed ports to internal only
# Remove port mappings except for api-gateway

api-gateway:
  ports:
    - "80:8080"  # or use nginx reverse proxy

auth-service:
  # Remove ports section
  expose:
    - "8080"
```

### 2. Use Production Database

```yaml
postgres:
  environment:
    POSTGRES_PASSWORD: ${POSTGRES_PASSWORD}  # Use env variable
```

### 3. Enable HTTPS

Use a reverse proxy like Nginx or Caddy in front of the api-gateway.

## 📊 Monitoring and Logs

### View logs for all services
```bash
docker-compose logs -f
```

### View logs for specific service
```bash
docker-compose logs -f auth-service
docker-compose logs -f qma-service
docker-compose logs -f api-gateway
```

### Health Checks
```bash
# PostgreSQL
docker exec qma-postgres pg_isready -U postgres

# Redis
docker exec qma-redis redis-cli ping

# API Gateway
curl http://localhost:5000/swagger

# Auth Service
curl http://localhost:5001/swagger

# QMA Service
curl http://localhost:5002/swagger
```

## 🔐 Security Best Practices

1. **Never commit secrets** - Use environment variables or secret managers
2. **Change default passwords** - Update PostgreSQL and Redis passwords
3. **Use HTTPS in production** - Enable SSL/TLS
4. **Rate limiting** - Implement rate limiting on api-gateway
5. **Input validation** - All inputs are validated
6. **SQL injection protection** - Using parameterized queries (EF Core)
7. **XSS protection** - Outputs are properly escaped

## 📝 License

MIT License - see LICENSE file for details

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Submit a pull request

## 📧 Support

For issues and questions, please create an issue in the GitHub repository.
