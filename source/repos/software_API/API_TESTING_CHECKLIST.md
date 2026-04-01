# ?? API Testing Checklist - Yad El-Awn

## ? Server Status
- [x] Build Successful
- [x] Server Running on http://localhost:5000
- [x] HTTPS on https://localhost:5001
- [x] Swagger Available on /swagger

---

## ?? Testing Endpoints

### 1?? **Health Endpoints** ?
```
GET /api/health
GET /api/health/ping
GET /api/health/check
GET /api/health/info
```

**Expected:** All return 200 OK with JSON response

---

### 2?? **Authentication Endpoints**
```
POST /api/auth/register-donor
POST /api/auth/register-beneficiary
POST /api/auth/login
```

**Test Data:**
```json
{
  "firstName": "Test",
  "lastName": "User",
  "email": "test@example.com",
  "password": "Password123",
  "phone": "01234567890",
  "address": "Cairo"
}
```

**Expected:** 200 OK with user data

---

### 3?? **Donations Endpoints**
```
GET /api/donations
POST /api/donations/food
POST /api/donations/medicine
POST /api/donations/clothes
PUT /api/donations/{id}/status
DELETE /api/donations/{id}
```

**Test Data for Food:**
```json
{
  "donorId": 2,
  "locationId": 1,
  "image": "image_url.jpg",
  "productName": "Rice",
  "foodType": "Grains",
  "expiryDate": "2025-12-31",
  "quantity": "10 kg",
  "category": "Food",
  "storageCondition": "Room temperature"
}
```

---

### 4?? **Beneficiaries Endpoints**
```
GET /api/beneficiaries
GET /api/beneficiaries/{id}
GET /api/beneficiaries/{id}/matches
PUT /api/beneficiaries/{id}/verify
```

**Expected:** 200 OK with beneficiary data

---

### 5?? **Donors Endpoints**
```
GET /api/donors
GET /api/donors/{id}
GET /api/donors/{id}/donations
GET /api/donors/statistics/top-donors
PUT /api/donors/{id}/verify
```

**Expected:** 200 OK with donor data

---

### 6?? **Charities Endpoints**
```
GET /api/charities
GET /api/charities/{id}
GET /api/charities/{id}/matches
POST /api/charities
PUT /api/charities/{id}
PUT /api/charities/{id}/verify
```

**Expected:** 200 OK with charity data

---

### 7?? **Locations Endpoints**
```
GET /api/locations
GET /api/locations/{id}
POST /api/locations
PUT /api/locations/{id}
DELETE /api/locations/{id}
```

**Test Data:**
```json
{
  "cityArea": "Cairo - New Cairo",
  "gpsCoordinates": "30.0196,31.2021"
}
```

---

### 8?? **Matching Endpoints**
```
GET /api/matching/available-donations/{beneficiaryId}
POST /api/matching/create-match
GET /api/matching/{id}
GET /api/matching/matches-for-charity/{charityId}
PUT /api/matching/{id}/complete
```

**Test Data:**
```json
{
  "donationId": 1,
  "beneficiaryId": 4,
  "charityId": 6,
  "urgencyLevel": "High",
  "distance": 5.5
}
```

---

### 9?? **Admin Endpoints**
```
GET /api/admin/statistics
GET /api/admin/unverified-users
GET /api/admin/pending-donations
GET /api/admin/audit-logs
GET /api/admin/donations-by-type
GET /api/admin/top-locations
PUT /api/admin/verify-user/{userId}
POST /api/admin/audit-log
```

**Expected:** 200 OK with admin data

---

## ?? Response Format

All successful responses follow this format:
```json
{
  "success": true,
  "message": "Operation successful",
  "data": { ... },
  "code": 200
}
```

Error responses:
```json
{
  "success": false,
  "message": "Error description",
  "details": "Additional error info"
}
```

---

## ?? CORS Configuration

? **Enabled for:**
- All Origins (*)
- All Methods (GET, POST, PUT, DELETE)
- All Headers

---

## ??? Database Configuration

| Item | Value |
|------|-------|
| **Type** | SQL Server |
| **Server** | DESKTOP-I3GABO5\SQLEXPRESS |
| **Database** | yad_elawn |
| **Connection** | Trusted Connection |

---

## ?? Ready for Frontend

### URL Configuration for Frontend:
```javascript
const API_URL = 'http://localhost:5000/api';
// or for HTTPS:
// const API_URL = 'https://localhost:5001/api';
```

### Example API Call:
```javascript
async function loginUser(email, password) {
  const response = await fetch(`${API_URL}/auth/login`, {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json'
    },
    body: JSON.stringify({ email, password })
  });
  
  const data = await response.json();
  return data;
}
```

---

## ? Pre-Flight Checklist

- [x] Build successful
- [x] Server running
- [x] Swagger documentation available
- [x] Database connected
- [x] All endpoints implemented
- [x] CORS enabled
- [x] Error handling in place
- [x] Response format standardized
- [x] Sample data loaded
- [x] Ready for Frontend Integration

---

## ?? Next Steps

1. **Share API URL** with Frontend Team: `http://localhost:5000`
2. **Swagger Doc**: `http://localhost:5000/swagger`
3. **Test Endpoints** using Postman/Insomnia
4. **Integrate** with Frontend using provided API URL

---

**API is Production-Ready! ?**
