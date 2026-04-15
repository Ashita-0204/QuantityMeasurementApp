# 🚀 QUICK START GUIDE

Follow these exact steps to get the microservices running.

## Prerequisites Check

```bash
# Check Docker
docker --version
# Should show: Docker version 20.x or higher

# Check Docker Compose
docker-compose --version
# Should show: Docker Compose version 2.x or higher
```

## Step 1: Configure JWT Secret

Edit `docker-compose.yml` and change this line:

```yaml
Jwt__Key: "your-super-secret-jwt-key-min-32-chars-change-this-in-production"
```

Generate a secure key:
```bash
# Option 1: Using openssl (Linux/Mac)
openssl rand -base64 32

# Option 2: Using PowerShell (Windows)
[Convert]::ToBase64String((1..32 | ForEach-Object { Get-Random -Maximum 256 }))

# Option 3: Just use this for testing (NOT FOR PRODUCTION)
test-jwt-key-12345678901234567890123456789012
```

## Step 2: Build and Start

```bash
# Navigate to the project directory
cd microservices-qma

# Build all services
docker-compose build

# Start all services
docker-compose up -d

# Check if all services are running
docker-compose ps
```

Expected output:
```
NAME                 STATUS    PORTS
qma-api-gateway      Up        0.0.0.0:5000->8080/tcp
qma-auth-service     Up        0.0.0.0:5001->8080/tcp
qma-service          Up        0.0.0.0:5002->8080/tcp
qma-postgres         Up        0.0.0.0:5432->5432/tcp
qma-redis            Up        0.0.0.0:6379->6379/tcp
```

## Step 3: Test the API

### Option A: Using Browser (Swagger UI)

1. Open: http://localhost:5000/swagger
2. Try the `/api/auth/register` endpoint
3. Fill in test data:
   ```json
   {
     "username": "testuser",
     "email": "test@example.com",
     "password": "Password123"
   }
   ```
4. Click "Execute"

### Option B: Using curl

**Register:**
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

Copy the `token` from the response. You'll need it for authenticated requests.

**Convert Units (no auth needed):**
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

**Save to History (auth required):**
```bash
# Replace YOUR_TOKEN_HERE with the token from login
curl -X POST http://localhost:5000/api/quantitymeasurement/save \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer YOUR_TOKEN_HERE" \
  -d '{
    "operation": "Convert",
    "data": {
      "value": 100,
      "fromUnit": "centimeter",
      "toUnit": "meter",
      "category": "Length"
    },
    "result": {
      "success": true,
      "operation": "Convert",
      "operand1": {
        "value": 100,
        "unit": "centimeter",
        "category": "Length"
      },
      "result": {
        "value": 1,
        "unit": "meter",
        "category": "Length"
      }
    }
  }'
```

**Get History:**
```bash
curl -X GET http://localhost:5000/api/quantitymeasurement/history \
  -H "Authorization: Bearer YOUR_TOKEN_HERE"
```

## Step 4: View Logs

```bash
# All services
docker-compose logs -f

# Specific service
docker-compose logs -f api-gateway
docker-compose logs -f auth-service
docker-compose logs -f qma-service
```

## Step 5: Stop Services

```bash
# Stop all services
docker-compose down

# Stop and remove all data (WARNING: deletes database)
docker-compose down -v
```

## Troubleshooting

### Services won't start

```bash
# Check logs
docker-compose logs

# Restart specific service
docker-compose restart auth-service
```

### Port already in use

```bash
# Linux/Mac - Kill process on port 5000
lsof -ti:5000 | xargs kill -9

# Windows - Find and kill process
netstat -ano | findstr :5000
taskkill /PID <PID> /F
```

### Database connection failed

```bash
# Check if PostgreSQL is ready
docker exec qma-postgres pg_isready -U postgres

# If not ready, restart it
docker-compose restart postgres

# Wait 10 seconds, then restart dependent services
docker-compose restart auth-service qma-service
```

### Clear all data and restart fresh

```bash
docker-compose down -v
docker-compose up -d --build
```

## API Endpoints Summary

| Endpoint | Method | Auth Required | Description |
|----------|--------|---------------|-------------|
| `/api/auth/register` | POST | No | Create account |
| `/api/auth/login` | POST | No | Get JWT token |
| `/api/user/profile` | GET | Yes | Get user info |
| `/api/quantitymeasurement/convert` | POST | No | Convert units |
| `/api/quantitymeasurement/add` | POST | No | Add quantities |
| `/api/quantitymeasurement/history` | GET | Yes | Get history |
| `/api/quantitymeasurement/save` | POST | Yes | Save to history |

## Next Steps

1. **Connect your Angular frontend** - Update environment.ts with:
   ```typescript
   apiUrl: 'http://localhost:5000'
   ```

2. **Enable Google OAuth** (optional):
   - Get credentials from Google Cloud Console
   - Update `docker-compose.yml`:
     ```yaml
     Authentication__Google__ClientId: "YOUR_CLIENT_ID"
     Authentication__Google__ClientSecret: "YOUR_CLIENT_SECRET"
     ```

3. **Deploy to production** - See README.md for deployment guide

## Useful Commands

```bash
# Rebuild specific service
docker-compose build auth-service
docker-compose up -d auth-service

# View database
docker exec -it qma-postgres psql -U postgres -d qma_db

# Inside psql:
\dt                              # List tables
SELECT * FROM users;             # View users
SELECT * FROM measurements;      # View measurements
\q                               # Quit

# Test Redis
docker exec -it qma-redis redis-cli
PING                             # Should return PONG
KEYS *                           # List all keys
exit
```
