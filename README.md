# BlogApp - My .NET Journey at AUCA

Welcome to my BlogApp! This project is the culmination of what I've been learning in the **.NET Class at AUCA (American University of Central Asia)**. It's a fully functional blogging platform built to demonstrate my understanding of web development, database management, and security in the .NET ecosystem.

## 🚀 Project Overview

The goal of this project was to move beyond basic console apps and build a real-world web application. I chose to build a BlogApp where users can:
- **Register and Login**: Secure account creation and authentication.
- **Create & Manage Blogs**: A full CRUD system for sharing stories and thoughts.
- **Categories**: Organize posts into different categories for better navigation.
- **Personalized Profiles**: Every post is linked to a specific blogger.

## 🛠️ What I Learned (The Technical Details)

Throughout this course, I've dived deep into several key technologies:

1.  **ASP.NET Core Razor Pages**: I learned how to use Razor Pages to create dynamic, server-side rendered web pages. It was interesting to see how the code-behind (`.cshtml.cs`) files handle logic while the `.cshtml` handles the UI.
2.  **Database Management (ADO.NET)**: Instead of relying on high-level abstractions immediately, I wanted to understand how things work under the hood. I used `Microsoft.Data.SqlClient` to write manual SQL queries, handle connections, and manage data retrieval using `SqlDataReader` and `SqlCommand`.
3.  **Security with BCrypt**: One of the most important things I learned was never to store passwords in plain text. I implemented `BCrypt.Net-Next` to hash and salt passwords during registration and verify them during login.
4.  **Authentication & Sessions**: I implemented Cookie-based authentication and used ASP.NET Core Sessions to keep users logged in and manage their state across different pages.
5.  **Relational Data**: I worked with multiple tables (`Blog`, `Bloggers`, `BlogCategory`) and learned how to use SQL `JOINs` to pull related data together for the UI.

## 🏗️ How to Run This Project

If you want to try this out locally, here's what you need:

1.  **Database Setup**:
    - This app uses SQL Server. You'll need to create a database named `TUESDAY_BLOG_DB`.
    - You can find the connection string in the `PageModel` files (default is `(localdb)\MSSQLLocalDB`). Update it to match your local setup if necessary.
2.  **Dependencies**:
    - Make sure you have the .NET 10.0 SDK installed.
    - Run `dotnet restore` to pull in the NuGet packages (BCrypt and SqlClient).
3.  **Launch**:
    - Just hit `F5` in Visual Studio or run `dotnet watch` in your terminal!

## 🎓 Acknowledgments

A big thanks to my instructor at **AUCA** for the guidance throughout the .NET course. This project has been a great way to put theory into practice and build something I'm proud of.

---
*Created as part of the AUCA .NET Course curriculum.*
