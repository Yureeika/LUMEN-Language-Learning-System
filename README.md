
# LUMEN – Learning Management System

LUMEN is a web-based Learning Management System (LMS) designed to provide an interactive and organized learning environment. It brings together learning resources, assessments, communication tools, and student progress tracking in one platform.

The system aims to support students and administrators by making learning activities easier to manage and access.

## Features

- **User Management:** Student accounts, administrator management, and user authentication.
- **Learning Management:** Subject management, lessons, notes, and bookmarks.
- **Quizzes and Assessments:** Interactive quizzes and daily quiz activities.
- **Progress Tracking:** Leaderboards, badges, certificates, and performance reports.
- **Discussion and Collaboration:** Discussion forums, study groups, and real-time chat.
- **Notifications:** Real-time notifications and updates.
- **Learning Tools:** Virtual coding and language learning labs.
- **Administration:** Audit logs and administrative reporting.

## Technologies Used

| Category | Technology |
|---|---|
| Programming Language | C# |
| Framework | ASP.NET Core MVC (.NET 10) |
| Frontend | HTML, CSS, JavaScript, Bootstrap |
| Database | Microsoft SQL Server |
| Database Management | SQL Server Management Studio (SSMS) |
| ORM | Entity Framework Core |
| Development Environment | Microsoft Visual Studio |

## Demo Login Credentials

The following Super Administrator account is configured in the database seeding code for demonstration and testing purposes.

| Field | Details |
|---|---|
| Role | Super Administrator |
| Email | superadmin@lumen.com |
| Password | Admin@123 |

**Note:** The Super Administrator account is automatically created when the application starts if the account does not already exist in the database.

## Project Structure

```text
LUMEN/
├── Controllers/
├── Models/
├── Views/
├── Data/
├── Migrations/
├── Services/
├── Hubs/
├── wwwroot/
├── LUMEN.csproj
└── README.md
```

## Requirements

To run this project locally, you will need:

- Microsoft Visual Studio
- .NET 10 SDK
- Microsoft SQL Server
- SQL Server Management Studio (SSMS)
- A compatible web browser

## Installation and Setup

1. Clone or download this repository.
2. Open the `LUMEN.slnx` solution file in Microsoft Visual Studio.
3. Ensure that the .NET 10 SDK is installed.
4. Open SQL Server and make sure your database instance is running.
5. Configure the database connection string in `appsettings.json` according to your local SQL Server instance.
6. Restore the required NuGet packages.
7. Build and run the project using Visual Studio.

## Database Configuration

LUMEN uses Microsoft SQL Server for database storage and Entity Framework Core for database operations.

The application uses Entity Framework Core migrations to create and update the database structure automatically when the application starts.

Make sure your SQL Server instance is running and that the database connection string is configured correctly before launching the application.

## Development

This project was developed using Microsoft Visual Studio and SQL Server Management Studio (SSMS).

It demonstrates the development of a web-based learning platform with database integration, user management, interactive learning features, and real-time communication.

## License

This project is intended for educational and demonstration purposes.
