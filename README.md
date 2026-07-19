# 🧑‍💼 CVisionary

> A dynamic portfolio & resume builder platform built with ASP.NET MVC

![ASP.NET MVC](https://img.shields.io/badge/ASP.NET_MVC-512BD4?style=flat-square&logo=dotnet&logoColor=white)
![C#](https://img.shields.io/badge/C%23-239120?style=flat-square&logo=csharp&logoColor=white)
![JavaScript](https://img.shields.io/badge/JavaScript-F7DF1E?style=flat-square&logo=javascript&logoColor=black)
![Sass](https://img.shields.io/badge/Sass-CC6699?style=flat-square&logo=sass&logoColor=white)
![SQL Server](https://img.shields.io/badge/SQL_Server-CC2927?style=flat-square&logo=microsoftsqlserver&logoColor=white)

---

## 📋 Overview

**CVisionary** is a full-stack portfolio and resume management platform that empowers professionals to build and publish their personal profiles online. Users manage all aspects of their professional identity — from work experience and education to projects, skills, certifications, and services — through a clean admin dashboard, with a polished public-facing portfolio page generated automatically.

---

## ✨ Features

- **Personal Profile Management** — Name, title, bio, photo, and contact details
- **Portfolio Showcase** — Add and manage projects with descriptions and links
- **Resume Builder** — Structured education, experience, and skills sections
- **Services Section** — Highlight the services you offer
- **Certifications** — Showcase professional certificates and achievements
- **Language Proficiency** — Display spoken languages and proficiency levels
- **Admin Dashboard** — Full CRUD control over all profile content
- **Public Portfolio Page** — Auto-generated, shareable portfolio URL
- **Repository Pattern** — Clean data access layer with separation of concerns
- **DTOs & ViewModels** — Structured data transfer between layers

---

## 🛠️ Tech Stack

| Layer | Technology |
|---|---|
| Backend | ASP.NET MVC (.NET), C# |
| ORM | Entity Framework Core |
| Database | Microsoft SQL Server |
| Frontend | HTML5, CSS3, JavaScript, Sass |
| Architecture | Repository Pattern, MVC |
| IDE | Visual Studio |

---

## 📁 Project Structure

```
CVisionary/
├── Controllers/
│   ├── AdminController.cs      # Admin dashboard — manage all profile content
│   ├── HomeController.cs       # Landing / home page
│   ├── PortfolioController.cs  # Public portfolio display
│   ├── ResumeController.cs     # Resume/CV view
│   └── ServiceController.cs   # Services section
├── Models/
│   ├── Person.cs / PersonalInfo.cs   # Core profile data
│   ├── Portfolio.cs / Project.cs     # Portfolio & projects
│   ├── Experience.cs                 # Work history
│   ├── Education.cs                  # Academic background
│   ├── Skill.cs                      # Technical & soft skills
│   ├── Certificate.cs               # Certifications
│   ├── Service.cs                    # Professional services
│   ├── Language.cs                   # Language proficiencies
│   └── Resume.cs                     # Full resume model
├── Repositories/            # Data access layer (Repository Pattern)
├── Services/                # Business logic layer
├── DTOs/                    # Data Transfer Objects
├── ViewModels/              # View-specific models
├── Views/                   # Razor views
├── Data/                    # DbContext
└── wwwroot/                 # Static assets (CSS, JS, images)
```

---

## 🚀 Getting Started

### Prerequisites

- [.NET SDK](https://dotnet.microsoft.com/download) (6.0 or higher)
- [SQL Server](https://www.microsoft.com/en-us/sql-server) or SQL Server Express
- [Visual Studio 2022](https://visualstudio.microsoft.com/) (recommended)

### Installation

1. **Clone the repository**
   ```bash
   git clone https://github.com/Ahmad-Mallad/CVisionary.git
   cd CVisionary
   ```

2. **Configure the database connection**

   Update `appsettings.json` with your SQL Server connection string:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=YOUR_SERVER;Database=CVisionaryDB;Trusted_Connection=True;"
     }
   }
   ```

3. **Apply database migrations**
   ```bash
   dotnet ef database update
   ```

4. **Run the application**
   ```bash
   dotnet run
   ```
   Or press **F5** in Visual Studio.

5. Open your browser at `https://localhost:5001`

---

## 🏗️ Architecture

CVisionary follows a clean **layered architecture**:

```
Controllers  →  Services  →  Repositories  →  DbContext  →  SQL Server
     ↕               ↕
  ViewModels        DTOs
```

This separation ensures the codebase remains maintainable, testable, and scalable as new features are added.

---

## 🤝 Contributing

Contributions are welcome! Feel free to open an issue or submit a pull request.

---

## 📄 License

This project is open source and available under the [MIT License](LICENSE).

---

<div align="center">

Made with ❤️ by [Ahmad Mallad](https://github.com/Ahmad-Mallad)

</div>
