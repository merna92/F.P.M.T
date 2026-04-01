# ?? How to Run the API - Quick Start Guide

## ? Prerequisites

- ? Visual Studio 2022 or VS Code
- ? .NET 8 SDK installed
- ? SQL Server Express installed
- ? Git installed

---

## ?? Step-by-Step Setup

### Step 1: Open the Project
```bash
cd C:\Users\ThinkPad\source\repos\software_API
```

Or open with Visual Studio:
- File ? Open ? Folder
- Select `software_API` folder

---

### Step 2: Restore NuGet Packages
```bash
dotnet restore
```

Or in Visual Studio:
- Right-click Project ? Restore NuGet Packages

---

### Step 3: Verify Database Connection

Check `appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=DESKTOP-I3GABO5\\SQLEXPRESS;Database=yad_elawn;..."
  }
}
```

? Database name should be: `yad_elawn`

---

### Step 4: Run the API

#### Option A: Visual Studio
1. Press **F5** or click "Run"
2. API starts on `http://localhost:5000`

#### Option B: Command Line
```bash
dotnet run --launch-profile https
```

#### Option C: .NET CLI
```bash
dotnet run
```

---

### Step 5: Verify API is Running

Open browser:
```
http://localhost:5000/swagger
```

You should see Swagger UI with all endpoints listed.

---

## ?? Quick Test

### Test 1: Health Check
```bash
curl http://localhost:5000/api/health/ping
```

**Expected Response:**
```json
{
  "success": true,
  "message": "Pong! API is alive",
  "status": "OK"
}
```

### Test 2: Get Statistics
```bash
curl http://localhost:5000/api/admin/statistics
```

**Expected:** JSON with statistics

---

## ??? Database Setup

### First Time Setup

If database doesn't exist, create it:

1. Open SQL Server Management Studio
2. Connect to: `DESKTOP-I3GABO5\SQLEXPRESS`
3. Create new Database: `yad_elawn`
4. Run migration (automatic via Entity Framework)

### Clean Database

Run the cleanup script:
1. Open `cleanup-and-seed.sql`
2. Execute in SQL Server Management Studio
3. This clears old data and adds sample records

---

## ?? API Endpoints Summary

| Method | Endpoint | Purpose |
|--------|----------|---------|
| GET | `/api/health/ping` | Health check |
| POST | `/api/auth/register-donor` | Register donor |
| POST | `/api/auth/login` | User login |
| GET | `/api/donations` | List donations |
| POST | `/api/donations/food` | Create food donation |
| GET | `/api/admin/statistics` | Admin stats |

---

## ?? Configuration

### Ports
- **HTTP**: 5000
- **HTTPS**: 5001

### Database
- **Server**: DESKTOP-I3GABO5\SQLEXPRESS
- **Database**: yad_elawn
- **Auth**: Windows Authentication

### CORS
- ? All origins allowed
- ? All HTTP methods allowed
- ? All headers allowed

---

## ?? Troubleshooting

### Issue: Port 5000 already in use
```bash
# Find process using port 5000
netstat -ano | findstr :5000

# Kill the process
taskkill /PID <PID> /F

# Or change port in appsettings.json
"Kestrel": {
  "Endpoints": {
    "Http": { "Url": "http://localhost:5002" }
  }
}
```

### Issue: Database connection failed
1. Check SQL Server is running
2. Verify connection string in `appsettings.json`
3. Run `cleanup-and-seed.sql` to recreate tables

### Issue: Swagger not loading
1. Verify API is running
2. Check: `http://localhost:5000/swagger`
3. If 404, check if Development environment is set

---

## ?? Project Structure

```
software_API/
??? Controllers/          # API endpoints
?   ??? AuthController.cs
?   ??? DonationsController.cs
?   ??? AdminController.cs
?   ??? ... more controllers
??? Data/               # Database models & context
?   ??? YadElawnContext.cs
?   ??? [Entity models]
??? Services/           # Business logic
?   ??? PasswordService.cs
?   ??? EmailService.cs
?   ??? FileService.cs
??? Middleware/         # Custom middleware
?   ??? ExceptionHandlingMiddleware.cs
??? Models/            # DTOs
?   ??? ApiResponse.cs
??? Program.cs         # Application startup
??? appsettings.json   # Configuration
??? cleanup-and-seed.sql  # Database setup
```

---

## ?? Next Steps

1. **Start the API**: Press F5 in Visual Studio
2. **Test endpoints**: Use Swagger UI at `/swagger`
3. **Share API URL**: Send `http://localhost:5000` to frontend team
4. **Integrate Frontend**: Use provided integration guide
5. **Deploy**: When ready, deploy to production

---

## ?? Environment Variables

Optional environment variables for production:
```
ASPNETCORE_ENVIRONMENT=Production
DATABASE_URL=[your-connection-string]
```

---

## ? Ready to Go!

Your API is now ready to use! ??

- **API URL**: `http://localhost:5000`
- **Swagger Docs**: `http://localhost:5000/swagger`
- **Database**: `DESKTOP-I3GABO5\SQLEXPRESS`
- **Database Name**: `yad_elawn`

---

## ?? Support

For issues or questions:
1. Check Swagger documentation
2. Review error logs in Output window
3. Check database connection
4. Verify all NuGet packages are restored

Happy coding! ???
