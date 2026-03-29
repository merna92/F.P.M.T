# ?? Yad El-Awn Backend - Railway Deployment Guide

## ? What's Done

? **Backend API** - Fully functional .NET 8 API  
? **PostgreSQL Support** - Auto-configured for Railway  
? **All Endpoints** - Auth, Donations, Matching, Admin, etc.  
? **Database Initialization** - Auto-migrations on startup  
? **Health Check** - `/api/health` endpoint  

---

## ?? Quick Start on Railway

### Step 1: Go to Railway
```
https://railway.app/
```

### Step 2: Login with GitHub
- Click "Login with GitHub"
- Authorize the app

### Step 3: Create New Project
1. Click **"+ New Project"**
2. Select **"Deploy from GitHub repo"**
3. Choose **"F.P.M.T"**

### Step 4: Add PostgreSQL
1. Click **"+ Add"**
2. Select **"Add Service"** ? **"Database"** ? **"PostgreSQL"**
3. Click **"Deploy"**

### Step 5: Wait for Deployment
Railway will:
- Build the application
- Create PostgreSQL database
- Auto-configure DATABASE_URL
- Deploy and run your API

---

## ? What Happens Automatically

? Railway reads `Program.cs`  
? Railway sees `Dockerfile`  
? Railway builds your app  
? Railway creates PostgreSQL  
? Your API is live! ??  

---

## ?? Your API URL

After deployment, you get:
```
https://your-app-name.up.railway.app/api
```

### Test Health Check
```
GET https://your-app-name.up.railway.app/api/health
```

### Response
```json
{
  "success": true,
  "status": "API is running",
  "database": "Connected",
  "totalUsers": 4,
  "timestamp": "2024-01-15T10:30:00Z",
  "environment": "Production"
}
```

---

## ?? API Endpoints

### Authentication
```
POST /api/auth/register-donor
POST /api/auth/register-beneficiary
POST /api/auth/login
```

### Donations
```
GET /api/donations
POST /api/donations/food
POST /api/donations/medicine
POST /api/donations/clothes
PUT /api/donations/{id}/status
```

### Matching
```
POST /api/matching/create-match
GET /api/matching/available-donations/{beneficiaryId}
```

### Admin
```
GET /api/admin/statistics
GET /api/admin/unverified-users
PUT /api/admin/verify-user/{userId}
```

### Health
```
GET /api/health
POST /api/health/seed-data
```

---

## ?? Environment Setup

Railway automatically sets:
```
ASPNETCORE_ENVIRONMENT = production
DATABASE_URL = postgres://user:password@host:port/db
```

---

## ?? Database

**Automatically Created:**
- PostgreSQL 15
- 5GB free storage
- Auto-backups
- Fully managed

**Tables:**
- Users, Donors, Beneficiaries, Charities
- Donations, Food, Medicine, Clothes
- Locations, Matches, Messages
- Audit logs and more...

---

## ?? Security

? HTTPS Enabled  
? CORS Configured  
? Password Hashing  
? Environment Variables  
? Error Handling  

---

## ?? Monitoring

### Check Logs
In Railway Dashboard:
1. Go to your project
2. Click "Deployments"
3. View live logs

### Health Endpoint
```
GET /api/health
```

---

## ?? Troubleshooting

### If Build Fails
1. Check GitHub commits pushed
2. View Railway logs
3. Ensure .NET 8 is specified

### If Database Error
1. Check Railway PostgreSQL is running
2. Verify DATABASE_URL is set
3. Check Program.cs uses Npgsql

### If API Not Responding
1. Check Dockerfile exists
2. Verify port is correct (Railway assigns it)
3. View deployment logs

---

## ?? Next Steps

1. ? Backend deployed on Railway
2. ?? Build Frontend (React/Vue)
3. ?? Connect Frontend to API
4. ?? Test all endpoints
5. ?? Launch!

---

## ?? Support

- **Railway Docs**: https://docs.railway.app/
- **PostgreSQL Docs**: https://www.postgresql.org/docs/
- **ASP.NET Docs**: https://docs.microsoft.com/aspnet/

---

**Your Backend is Ready! ??**

Use this API URL in your Frontend:
```javascript
const API_URL = 'https://your-app-name.up.railway.app/api';
```
