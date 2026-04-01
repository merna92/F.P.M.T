# Frontend Integration Guide - Yad El-Awn API

## ?? API Base URL

```javascript
const API_BASE_URL = 'http://localhost:5000/api';
```

---

## ?? API Methods

### 1. Authentication Service

#### Register as Donor
```javascript
async function registerDonor(userData) {
  const response = await fetch(`${API_BASE_URL}/auth/register-donor`, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify({
      firstName: userData.firstName,
      lastName: userData.lastName,
      email: userData.email,
      password: userData.password,
      phone: userData.phone,
      address: userData.address
    })
  });
  return response.json();
}
```

#### Login
```javascript
async function login(email, password) {
  const response = await fetch(`${API_BASE_URL}/auth/login`, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify({ email, password })
  });
  const data = await response.json();
  
  if (data.success) {
    localStorage.setItem('userId', data.userId);
    localStorage.setItem('userType', data.userType);
    localStorage.setItem('fullName', data.fullName);
  }
  
  return data;
}
```

---

### 2. Donations Service

#### Get All Donations
```javascript
async function getDonations(status = null) {
  const url = status 
    ? `${API_BASE_URL}/donations?status=${status}`
    : `${API_BASE_URL}/donations`;
  
  const response = await fetch(url);
  return response.json();
}
```

#### Create Food Donation
```javascript
async function createFoodDonation(donationData) {
  const response = await fetch(`${API_BASE_URL}/donations/food`, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify({
      donorId: donationData.donorId,
      locationId: donationData.locationId,
      image: donationData.image,
      productName: donationData.productName,
      foodType: donationData.foodType,
      expiryDate: donationData.expiryDate,
      quantity: donationData.quantity,
      category: donationData.category,
      storageCondition: donationData.storageCondition
    })
  });
  return response.json();
}
```

#### Update Donation Status
```javascript
async function updateDonationStatus(donationId, status) {
  const response = await fetch(`${API_BASE_URL}/donations/${donationId}/status`, {
    method: 'PUT',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify({ status })
  });
  return response.json();
}
```

---

### 3. Beneficiaries Service

#### Get All Beneficiaries
```javascript
async function getBeneficiaries() {
  const response = await fetch(`${API_BASE_URL}/beneficiaries`);
  return response.json();
}
```

#### Get Beneficiary Details
```javascript
async function getBeneficiaryById(beneficiaryId) {
  const response = await fetch(`${API_BASE_URL}/beneficiaries/${beneficiaryId}`);
  return response.json();
}
```

#### Get Beneficiary Matches
```javascript
async function getBeneficiaryMatches(beneficiaryId) {
  const response = await fetch(`${API_BASE_URL}/beneficiaries/${beneficiaryId}/matches`);
  return response.json();
}
```

---

### 4. Donors Service

#### Get All Donors
```javascript
async function getDonors() {
  const response = await fetch(`${API_BASE_URL}/donors`);
  return response.json();
}
```

#### Get Top Donors
```javascript
async function getTopDonors(limit = 10) {
  const response = await fetch(`${API_BASE_URL}/donors/statistics/top-donors?limit=${limit}`);
  return response.json();
}
```

#### Get Donor Donations
```javascript
async function getDonorDonations(donorId) {
  const response = await fetch(`${API_BASE_URL}/donors/${donorId}/donations`);
  return response.json();
}
```

---

### 5. Charities Service

#### Get All Charities
```javascript
async function getCharities() {
  const response = await fetch(`${API_BASE_URL}/charities`);
  return response.json();
}
```

#### Get Charity Matches
```javascript
async function getCharityMatches(charityId) {
  const response = await fetch(`${API_BASE_URL}/charities/${charityId}/matches`);
  return response.json();
}
```

---

### 6. Locations Service

#### Get All Locations
```javascript
async function getLocations() {
  const response = await fetch(`${API_BASE_URL}/locations`);
  return response.json();
}
```

#### Create Location
```javascript
async function createLocation(locationData) {
  const response = await fetch(`${API_BASE_URL}/locations`, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify({
      cityArea: locationData.cityArea,
      gpsCoordinates: locationData.gpsCoordinates
    })
  });
  return response.json();
}
```

---

### 7. Matching Service

#### Get Available Donations
```javascript
async function getAvailableDonations(beneficiaryId) {
  const response = await fetch(`${API_BASE_URL}/matching/available-donations/${beneficiaryId}`);
  return response.json();
}
```

#### Create Match
```javascript
async function createMatch(matchData) {
  const response = await fetch(`${API_BASE_URL}/matching/create-match`, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify({
      donationId: matchData.donationId,
      beneficiaryId: matchData.beneficiaryId,
      charityId: matchData.charityId,
      urgencyLevel: matchData.urgencyLevel,
      distance: matchData.distance
    })
  });
  return response.json();
}
```

#### Complete Match
```javascript
async function completeMatch(matchId) {
  const response = await fetch(`${API_BASE_URL}/matching/${matchId}/complete`, {
    method: 'PUT',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify({})
  });
  return response.json();
}
```

---

### 8. Admin Service

#### Get Statistics
```javascript
async function getStatistics() {
  const response = await fetch(`${API_BASE_URL}/admin/statistics`);
  return response.json();
}
```

#### Get Unverified Users
```javascript
async function getUnverifiedUsers() {
  const response = await fetch(`${API_BASE_URL}/admin/unverified-users`);
  return response.json();
}
```

#### Verify User
```javascript
async function verifyUser(userId) {
  const response = await fetch(`${API_BASE_URL}/admin/verify-user/${userId}`, {
    method: 'PUT',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify({})
  });
  return response.json();
}
```

#### Get Donations by Type
```javascript
async function getDonationsByType() {
  const response = await fetch(`${API_BASE_URL}/admin/donations-by-type`);
  return response.json();
}
```

---

## ??? Error Handling

All responses include a `success` boolean. Handle errors like this:

```javascript
async function apiCall(url, options = {}) {
  try {
    const response = await fetch(url, options);
    const data = await response.json();
    
    if (!data.success) {
      console.error('API Error:', data.message);
      return null;
    }
    
    return data.data || data;
  } catch (error) {
    console.error('Network Error:', error);
    return null;
  }
}
```

---

## ?? Response Format

### Success Response
```json
{
  "success": true,
  "message": "Operation successful",
  "data": { ... },
  "code": 200
}
```

### Error Response
```json
{
  "success": false,
  "message": "Error description",
  "details": "Additional error info",
  "code": 400
}
```

---

## ?? Local Storage Keys

After login, store these values:
```javascript
localStorage.setItem('userId', response.userId);
localStorage.setItem('userType', response.userType);      // Donor | Beneficiary | Charity | Admin
localStorage.setItem('fullName', response.fullName);
localStorage.setItem('email', response.email);
```

---

## ?? React Example

```javascript
import { useEffect, useState } from 'react';

function App() {
  const [donations, setDonations] = useState([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const fetchDonations = async () => {
      try {
        const response = await fetch('http://localhost:5000/api/donations');
        const data = await response.json();
        
        if (data.success) {
          setDonations(data.data);
        }
      } catch (error) {
        console.error('Error:', error);
      } finally {
        setLoading(false);
      }
    };

    fetchDonations();
  }, []);

  if (loading) return <div>Loading...</div>;

  return (
    <div>
      <h1>Donations</h1>
      {donations.map(donation => (
        <div key={donation.donationId}>
          <h3>{donation.donorName}</h3>
          <p>Status: {donation.status}</p>
        </div>
      ))}
    </div>
  );
}

export default App;
```

---

## ?? Vue Example

```javascript
export default {
  data() {
    return {
      donations: [],
      loading: true
    };
  },
  
  async mounted() {
    try {
      const response = await fetch('http://localhost:5000/api/donations');
      const data = await response.json();
      
      if (data.success) {
        this.donations = data.data;
      }
    } catch (error) {
      console.error('Error:', error);
    } finally {
      this.loading = false;
    }
  },
  
  template: `
    <div v-if="!loading">
      <h1>Donations</h1>
      <div v-for="donation in donations" :key="donation.donationId">
        <h3>{{ donation.donorName }}</h3>
        <p>Status: {{ donation.status }}</p>
      </div>
    </div>
  `
};
```

---

## ?? Testing Tools

### Using cURL
```bash
curl -X GET http://localhost:5000/api/health/ping
```

### Using Postman
1. Create New Request
2. Set URL: `http://localhost:5000/api/[endpoint]`
3. Set Method: GET/POST/PUT/DELETE
4. Add Headers if needed
5. Add Body (JSON) if needed
6. Send

### Using VS Code REST Client
Create `.rest` file:
```
### Get All Donations
GET http://localhost:5000/api/donations

### Register New Donor
POST http://localhost:5000/api/auth/register-donor
Content-Type: application/json

{
  "firstName": "John",
  "lastName": "Doe",
  "email": "john@example.com",
  "password": "Password123",
  "phone": "01234567890",
  "address": "Cairo"
}
```

---

## ? Ready to Integrate!

Your API is fully functional and ready for frontend integration. Use the examples above to get started!

For any issues, check:
1. API is running: `http://localhost:5000`
2. Swagger docs: `http://localhost:5000/swagger`
3. Database is connected
4. Correct response format is expected

**Happy Coding! ??**
