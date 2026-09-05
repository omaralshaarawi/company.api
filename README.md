# Company API 🏢⚡

A robust, enterprise-grade backend infrastructure built with ASP.NET Core 10 and Entity Framework Core. This API is engineered for secure enterprise asset management, biometric validation schemas, and automated asset auditing. 

## 🚀 Key Features

*   **Employee & Department Management:** Full data operations for organizational structures[cite: 2].
*   **Asset Auditing & Workflow:** Specialized endpoints for assigning and returning assets, backed by database transactions to ensure asset statuses (like "InStock" or "Assigned") never get out of sync[cite: 2].
*   **Biometric Integration:** Dedicated endpoints for fingerprint registration that securely manage biometric data without exposing sensitive raw template data in responses[cite: 2].
*   **Attendance Tracking:** Automated logging for employee check-ins and check-outs via device integration[cite: 2].
*   **Role-Based Security:** Endpoints secured with JSON Web Tokens (JWT), with specific administrative actions restricted by role authorization, and passwords hashed securely via BCrypt[cite: 2].
*   **Advanced Validation:** Complex, database-dependent business rules (e.g., verifying an asset is "InStock" before assignment) handled elegantly via FluentValidation[cite: 2].
*   **Structured Logging:** Production-ready logging pipeline configured with Serilog, capturing events to daily rolling files[cite: 2].

## 🛠️ Technology Stack

*   **Framework:** .NET 10 SDK[cite: 2].
*   **Database:** SQL Server[cite: 2].
*   **ORM:** Entity Framework Core[cite: 2].
*   **Security & Auth:** JWT Bearer Authentication and BCrypt.Net-Next[cite: 2].
*   **Validation:** FluentValidation.AspNetCore[cite: 2].
*   **Logging:** Serilog.AspNetCore[cite: 2].
*   **API Documentation:** Swashbuckle.AspNetCore (Swagger)[cite: 2].

## 📁 Project Structure

*   `Controllers/`: Defines the route handlers and HTTP API endpoints (e.g., `EmployeesController`, `AuthController`)[cite: 2].
*   `Models/`: Entity Framework Core classes that map directly to the SQL Server database tables[cite: 2].
*   `DTOs/`: Data Transfer Objects defining the strict request and response shapes exposed to the client[cite: 2].
*   `Data/`: Contains `CompanyDbContext`, representing the database session and entity relationships[cite: 2].
*   `Services/`: Encapsulated business logic, such as the `TokenService` responsible for generating JWTs[cite: 2].

## ⚙️ Getting Started

### Prerequisites

*   .NET 10 SDK[cite: 2].
*   A running instance of SQL Server[cite: 2].

### Installation & Setup

1.  **Clone the repository:**
    ```bash
    git clone [https://github.com/omaralshaarawi/company.api.git](https://github.com/omaralshaarawi/company.api.git)
    cd company.api
    ```

2.  **Configure the Database Connection:**
    Update `appsettings.json` (or `appsettings.Development.json`) with your SQL Server connection string[cite: 2]:
    ```json
    {
      "ConnectionStrings": {
        "CompanyDb": "Server=YOUR_SERVER;Database=Company;Trusted_Connection=True;TrustServerCertificate=True;"
      }
    }
    ```

3.  **Apply Database Migrations:**
    Ensure your SQL Server database is created and up to date with the latest schema.
    ```bash
    dotnet ef database update
    ```

4.  **Run the Application:**
    ```bash
    dotnet run
    ```

5.  **Test the Endpoints:**
    Once running, navigate to `https://localhost:<port>/swagger` in your browser. This will open the interactive Swagger UI where you can authenticate and test the API endpoints directly[cite: 2].
