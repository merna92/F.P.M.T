# API Response Format Documentation

## Standard Response Structure

All API responses follow a consistent JSON structure with a `success` boolean field at the top level.

### Success Response (Status 200, 201)
```json
{
  "success": true,
  "message": "Operation completed successfully",
  "data": {
    // ... response data
  }
}
```

### Error Response (Status 400, 404, 500)
```json
{
  "success": false,
  "message": "Error message",
  "details": "Additional error details (if applicable)"
}
```

## Status Codes

| Code | Meaning | When Used |
|------|---------|-----------|
| 200 | OK | Successful GET, PUT requests |
| 201 | Created | Successful POST requests (resource created) |
| 400 | Bad Request | Invalid request data, validation errors |
| 401 | Unauthorized | Invalid credentials |
| 404 | Not Found | Resource doesn't exist |
| 500 | Internal Server Error | Unexpected server error |

## Example Responses

### Successful Login
```json
{
  "success": true,
  "message": "Login successful",
  "userId": 1,
  "fullName": "John Doe",
  "email": "john@example.com",
  "userType": "Donor"
}
```

### Validation Error
```json
{
  "success": false,
  "message": "Invalid data",
  "errors": [
    "Invalid email format",
    "Password must be at least 6 characters"
  ]
}
```

### Resource Not Found
```json
{
  "success": false,
  "message": "Donation not found"
}
```

## Common Messages

### Success Messages
- "Registration successful"
- "Login successful"
- "Donation created successfully"
- "Location updated successfully"
- "Match completed successfully"

### Error Messages
- "Invalid data"
- "Invalid credentials"
- "Email already exists"
- "Donor not found"
- "Beneficiary not found"
- "Charity not found"
- "Donation not found"
- "Location not found"
- "Match not found"
- "User not found"
- "Error creating donation"
- "Error creating charity"
- "Error creating match"

## Response Encoding

All responses are UTF-8 encoded with `charset=utf-8` in the Content-Type header:
```
Content-Type: application/json; charset=utf-8
```

This ensures that all text content (including Arabic if needed) is properly displayed.
