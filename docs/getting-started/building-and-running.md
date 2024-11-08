---
title: Building and Running the Application
layout: home
parent: Getting Started
nav_order: 100
---

# {{ page.title }}

## Prerequisites

- .NET Core SDK
- Node.js and npm
- Angular CLI

## Installation

1. **Clone the repository:**

   ```bash
   git clone https://github.com/sharplogic/LightNap.git
   cd LightNap
   ```

2. **Back-End Setup:**

   - Navigate to the `src` directory:

     ```bash
     cd src
     ```

   - Restore .NET Core dependencies:

     ```bash
     dotnet restore
     ```

   - Run the application:

     ```bash
     dotnet run --project LightNap.WebApi
     ```

    {: .important }
    The application will log the error and quit if anything in the startup or seeding process fails. This
    includes database migrations and user/role seeding. Please check the logs if a deployment fails to start.

3. **Front-End Setup:**

   - In a separate terminal, navigate to the `lightnap-ng` directory:

     ```bash
     cd src/lightnap-ng
     ```

   - Install Angular dependencies:

     ```bash
     npm install
     ```

   - Run the Angular application:

     ```bash
     ng serve
     ```

## Usage

- Access the application at `http://localhost:4200`.
- By default, an administrator account is created with:
  - **Email**: `admin@admin.com`
  - **UserName**: `admin`
  - **Password**: `A2m!nPassword`
