# Blazor Web App with Authentication - Project Instructions

## Project Overview
This is a Blazor Web Application with built-in authentication and an admin dashboard. The project uses server-side rendering with cookie-based authentication.

## Key Features
- User authentication with login page
- Admin dashboard with statistics and system status
- Role-based access control
- Secure session management
- Responsive design using Bootstrap

## Default Credentials
- **Username**: admin
- **Password**: admin

## Project Structure
```
BlazorWebApp/
├── Components/
│   ├── Pages/
│   │   ├── Home.razor
│   │   ├── Login.razor
│   │   ├── Dashboard.razor
│   │   ├── Counter.razor
│   │   ├── Weather.razor
│   │   └── Error.razor
│   ├── Layout/
│   │   ├── MainLayout.razor
│   │   └── NavMenu.razor
│   ├── App.razor
│   └── Routes.razor
├── Services/
│   └── AuthenticationService.cs
├── Program.cs
├── appsettings.json
└── appsettings.Development.json
```

## Building and Running the Project
1. Navigate to the project directory:
   ```bash
   cd BlazorWebApp
   ```

2. Restore dependencies:
   ```bash
   dotnet restore
   ```

3. Build the project:
   ```bash
   dotnet build
   ```

4. Run the application:
   ```bash
   dotnet run
   ```

5. Open your browser and navigate to:
   ```
   https://localhost:7254 (or http://localhost:5256 for HTTP)
   ```

## Usage
1. Click "Login" in the navigation bar
2. Enter credentials (admin/admin)
3. Upon successful login, you'll be redirected to the admin dashboard
4. View statistics, recent activity, and system status
5. Click "Logout" to end your session

## Authentication Flow
1. User enters credentials on the login page
2. AuthenticationService validates credentials
3. If valid, a cookie is created and the user is authenticated
4. Dashboard requires authentication and redirects to login if not authenticated
5. Logout clears the authentication cookie

## Customization
To modify authentication:
- Update `ValidateCredentials()` in `Services/AuthenticationService.cs` to use your own authentication method
- Connect to a database for real user credentials
- Add more claims for role-based authorization
- Integrate with Azure AD or other identity providers

## Troubleshooting
- **Login fails**: Ensure you're using credentials: admin/admin
- **Cannot access dashboard**: Make sure you're logged in first
- **Port already in use**: The default HTTPS port is 7254. You can change it in Properties/launchSettings.json

## Technical Stack
- .NET 8+
- Blazor Web App (Server-side rendering)
- ASP.NET Core Authentication (Cookies)
- Bootstrap for styling

## Security Notes
- For production, use stronger authentication (Azure AD, OAuth2, etc.)
- Store passwords securely (use hashing and salting)
- Implement HTTPS enforcement
- Add CSRF protection (already enabled via UseAntiforgery)
- Implement proper authorization policies for sensitive operations
