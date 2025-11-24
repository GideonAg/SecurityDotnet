# SecurityDotnet

A secure authentication and authorization API built with ASP.NET Core 9 and .NET 9. This project demonstrates JWT token-based authentication, role-based access control, and password reset functionality.

## Features

- **User Registration**: Register new users with email, password, and role assignment
- **User Login**: Authenticate users and issue JWT access tokens
- **Refresh Token**: Generate new access tokens using refresh tokens
- **Password Reset**: Send password reset codes to user emails
- **Role-Based Authorization**: Protect endpoints with role-based access control
- **JWT Authentication**: Secure API endpoints using JWT bearer tokens
- **Password Hashing**: Passwords are securely hashed using ASP.NET Core Identity

## Tech Stack

- **Framework**: ASP.NET Core 9
- **Language**: C# 13.0
- **Database**: SQL Server (Express)
- **Authentication**: JWT (JSON Web Tokens)
- **Email**: MailKit (SMTP)
- **ORM**: Entity Framework Core

## Prerequisites

- .NET 9 SDK
- SQL Server Express (or SQL Server)
- Visual Studio 2022 or any .NET-compatible IDE
- MailKit for email functionality (optional for password reset)

## Installation

1. **Clone the repository**

2. **Configure the database connection**
   
   Update `appsettings.json`:

3. **Apply database migrations**

4. **Run the application**

The API will be available at:
   - HTTP: `http://localhost:5174`
   - HTTPS: `https://localhost:7294`
