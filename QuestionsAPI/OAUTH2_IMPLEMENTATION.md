# OAuth2 Implementation for Questions API

## Overview

This API implements OAuth2 using the **Client Credentials Flow** with JWT Bearer tokens. This allows external services and applications to authenticate and access the `/questions` endpoint.

## Architecture

### Components

1. **Token Endpoint** (`/oauth/token`) - Issues JWT tokens
2. **Protected Endpoint** (`/questions`) - Requires valid JWT Bearer token
3. **JWT Settings** - Configured in `appsettings.json`
4. **OAuth Clients** - Pre-configured client credentials in `appsettings.json`

## OAuth2 Client Credentials Flow

### Step 1: Request Access Token

Send a POST request to `/oauth/token` with client credentials:

```bash
curl -X POST http://localhost:5213/oauth/token \
  -H "Content-Type: application/json" \
  -d '{
    "clientId": "client1",
    "clientSecret": "secret1",
    "grantType": "client_credentials"
  }'
```

**Response:**

```json
{
  "accessToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "tokenType": "Bearer",
  "expiresIn": 3600
}
```

### Step 2: Access Protected Resources

Use the token to call the `/questions` endpoint:

```bash
curl -X GET http://localhost:5213/questions \
  -H "Authorization: Bearer YOUR_ACCESS_TOKEN"
```

**Response:**

```json
[
  {
    "id": 1,
    "text": "What is 2+2?",
    "answer": "4"
  },
  {
    "id": 2,
    "text": "What is the capital of France?",
    "answer": "Paris"
  }
]
```

## Configuration

### appsettings.json

```json
{
  "JwtSettings": {
    "SecretKey": "your-secret-key-must-be-at-least-32-characters-long-for-hs256",
    "Issuer": "QuestionsAPI",
    "Audience": "QuestionsAPI-Clients",
    "ExpirationMinutes": 60
  },
  "OAuthClients": [
    {
      "ClientId": "client1",
      "ClientSecret": "secret1"
    },
    {
      "ClientId": "client2",
      "ClientSecret": "secret2"
    }
  ]
}
```

### Key Points

- **SecretKey**: Must be at least 32 characters for HS256. Change this in production!
- **Issuer**: Token issuer identifier
- **Audience**: Audience for token validation
- **ExpirationMinutes**: Token expiration time
- **OAuthClients**: Add more client credentials as needed

## Implementation Details

### Services

- **TokenService** (`Services/TokenService.cs`) - Generates JWT tokens
- **ITokenService** - Interface for token generation

### DTOs

- **TokenRequest** - Request model for token endpoint
- **TokenResponse** - Response model with access token

### Models

- **OAuthClient** - OAuth client configuration model

## Authentication Flow in Code

1. Client sends credentials to `/oauth/token`
2. Endpoint validates credentials against configured `OAuthClients`
3. If valid, `TokenService.GenerateToken()` creates a JWT
4. Client receives token with expiration time
5. Client includes token in `Authorization: Bearer <token>` header
6. `JwtBearerDefaults.AuthenticationScheme` middleware validates token
7. If valid, request proceeds to `/questions`
8. If invalid/expired, returns 401 Unauthorized

## Security Considerations

### Production Recommendations

1. **Change the SecretKey** - Use a strong, unique 32+ character key
2. **Use HTTPS** - Always use HTTPS in production
3. **Rotate Keys** - Implement key rotation periodically
4. **Store Secrets Securely** - Use environment variables or Azure Key Vault
5. **Monitor Token Usage** - Log and audit token requests
6. **Rate Limiting** - Consider implementing rate limiting on `/oauth/token`
7. **Client Secrets** - Store hashed/encrypted client secrets in production

### Example Environment-Based Configuration

```csharp
// In Program.cs
var secretKey = builder.Configuration["JWT_SECRET_KEY"]
    ?? "default-dev-key"; // Get from environment variable
```

## Testing

### Using VS Code REST Client Extension

See `QuestionsAPI.http` for ready-to-use REST requests

### Using Swagger/OpenAPI

- Navigate to `http://localhost:PORT/swagger`
- Use the "Authorize" button to add Bearer token
- Test endpoints with authentication

### PowerShell Example

```powershell
# Get token
$response = Invoke-RestMethod -Uri "http://localhost:5213/oauth/token" `
  -Method Post `
  -ContentType "application/json" `
  -Body '{"clientId":"client1","clientSecret":"secret1","grantType":"client_credentials"}'

$token = $response.accessToken

# Use token
$headers = @{"Authorization" = "Bearer $token"}
Invoke-RestMethod -Uri "http://localhost:5213/questions" `
  -Headers $headers
```

## Token Claims

Each JWT contains the following claims:

- `nameid` - Client ID
- `client_id` - Client ID (duplicate for compatibility)
- `jti` - Unique token identifier (GUID)
- Standard claims: `iss`, `aud`, `exp`

## Troubleshooting

### 401 Unauthorized on /questions

- Ensure Bearer token is included in Authorization header
- Check token hasn't expired
- Verify token format: `Authorization: Bearer <token>`

### Invalid Credentials on /oauth/token

- Verify ClientId and ClientSecret match configured values
- Grant type must be `client_credentials`

### Invalid Issuer/Audience Error

- Ensure JwtSettings in appsettings.json match Program.cs configuration
