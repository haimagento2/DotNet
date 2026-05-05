# Blazor Web App with Admin Dashboard

A complete Blazor Web application featuring user authentication and a comprehensive admin dashboard.

## 🚀 Quick Start

### Prerequisites
- .NET 8 SDK or higher
- Visual Studio Code or Visual Studio

### Run the Application

```bash
# Navigate to the project directory
cd BlazorWebApp

# Restore packages
dotnet restore

# Run the application
dotnet run
```

The application will be available at `https://localhost:7254`

### Login Credentials
```
Username: admin
Password: admin
```

## 📋 Features

### Authentication
- Secure login page with form validation
- Cookie-based authentication
- Session management
- Logout functionality

### Admin Dashboard
- **Statistics Overview**: Total users, active sessions, revenue, and alerts
- **Recent Activity**: Track user actions with timestamps
- **System Status**: Monitor database, API, cache, and email services
- **Responsive Design**: Works seamlessly on desktop, tablet, and mobile

### User Interface
- Modern gradient design with Bootstrap styling
- Navigation menu with dynamic links based on authentication status
- Protected routes that require login
- User information display in navigation bar

## 📁 Project Structure

```
BlazorWebApp/
├── Components/
│   ├── Pages/
│   │   ├── Home.razor          # Welcome page
│   │   ├── Login.razor         # Authentication page
│   │   ├── Dashboard.razor     # Admin dashboard
│   │   └── ...other pages
│   ├── Layout/
│   │   ├── MainLayout.razor    # Main layout component
│   │   └── NavMenu.razor       # Navigation menu
│   └── App.razor               # Root component
├── Services/
│   └── AuthenticationService.cs # Authentication logic
├── Program.cs                   # Application configuration
└── appsettings.json            # Configuration file
```

## 🔒 Authentication Details

### Login Flow
1. User navigates to `/login`
2. Enters credentials
3. AuthenticationService validates credentials
4. Creates authentication cookie upon success
5. Redirects to dashboard

### Authorization
- Dashboard page checks `AuthService.IsAuthenticated()` before rendering
- Unauthenticated users are redirected to login page
- Navigation menu shows dashboard link only for authenticated users

## 🛠️ Development

### Build the Project
```bash
dotnet build
```

### Run Tests
```bash
dotnet test
```

## 🔐 Security Considerations

For **Production Deployment**:

1. **Replace Demo Authentication**: Update `ValidateCredentials()` in `AuthenticationService.cs`
2. **Use Real User Store**: Connect to database or identity provider
3. **Implement Password Hashing**: Use BCrypt or similar for password storage
4. **Configure HTTPS**: Enforce HTTPS in production
5. **Add Authorization Policies**: Implement role-based access control
6. **Integrate OAuth/OpenID Connect**: Consider Azure AD, Google, GitHub
7. **Enable CORS**: Configure appropriately for your domain
8. **Set Secure Cookies**: Enable HttpOnly and Secure flags

## 📝 Customization

### Change Default Credentials
Edit `Services/AuthenticationService.cs`:
```csharp
private bool ValidateCredentials(string username, string password)
{
    // Implement your authentication logic here
    return username == "your_username" && password == "your_password";
}
```

### Add Database Authentication
1. Install EF Core: `dotnet add package Microsoft.EntityFrameworkCore`
2. Create DbContext for users
3. Query database in `ValidateCredentials()`
4. Hash passwords securely

### Add More Dashboard Components
Edit `Components/Pages/Dashboard.razor` to add:
- Charts and graphs
- Data tables
- More statistics
- Real-time updates

## 🚀 Deployment

### Deploy to Azure
```bash
dotnet publish -c Release
# Upload to Azure App Service
```

### Docker
Create a `Dockerfile` and deploy containerized application

## 📚 Resources

- [Blazor Documentation](https://learn.microsoft.com/en-us/aspnet/core/blazor/)
- [ASP.NET Core Authentication](https://learn.microsoft.com/en-us/aspnet/core/security/authentication/)
- [Bootstrap Documentation](https://getbootstrap.com/docs/)

## 📄 License

This project is provided as-is for learning and development purposes.

## 🤝 Support

For issues or questions, please refer to the official Blazor and ASP.NET Core documentation.

---

**Happy Coding!** 🎉
