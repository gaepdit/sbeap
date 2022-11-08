# Small Business Environmental Assistance Program

This app is used by the SBEAP for tracking their workload, including customers and cases.

[![.NET Test](https://github.com/gaepdit/sbeap/actions/workflows/dotnet-test.yml/badge.svg)](https://github.com/gaepdit/sbeap/actions/workflows/dotnet-test.yml)
[![CodeQL](https://github.com/gaepdit/sbeap/actions/workflows/codeql-analysis.yml/badge.svg)](https://github.com/gaepdit/sbeap/actions/workflows/codeql-analysis.yml)
[![SonarCloud Scan](https://github.com/gaepdit/sbeap/actions/workflows/sonarcloud-scan.yml/badge.svg)](https://github.com/gaepdit/sbeap/actions/workflows/sonarcloud-scan.yml)
[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=gaepdit_sbeap&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=gaepdit_sbeap)
[![Lines of Code](https://sonarcloud.io/api/project_badges/measure?project=gaepdit_sbeap&metric=ncloc)](https://sonarcloud.io/summary/new_code?id=gaepdit_sbeap)

*(Built from template version [v2022.10.14](https://github.com/gaepdit/template-app/tree/v2022.10.14).)*

---

## Background and project requirements

This new application will replace similar functionality previously housed in the IAIP. 

## Info for developers

This is an ASP.NET 6 web application.

### Prerequisites for development

+ [Visual Studio](https://www.visualstudio.com/vs/) or similar
+ [.NET 6.0 SDK](https://dotnet.microsoft.com/download)

### Project organization

The solution contains the following projects:

* **Domain** — A class library containing the data models and business logic.
* **AppServices** — A class library containing the services used by an application to interact with the domain.
* **LocalRepository** — A class library implementing the repository and services without using a database (for local development).
* **Infrastructure** — A class library implementing the repository and services using Entity Framework (for server deployment).
* **WebApp** — The front end web application.

There are also corresponding unit test projects for each, plus a **TestData** project containing test data for development and testing.

### Launch profiles

There are two launch profiles:

* **WebApp Local** — This profile uses data in the "TestData" project and does not connect to any external server. A local user account is used to simulate authentication.

    You can modify some development settings by creating an "appsettings.Local.json" file in the "WebApp" folder to test various scenarios:

    - *AuthenticatedUser* — Simulates a successful login with a test account when `true`. Simulates a failed login when `false`.
    - *AuthenticatedUserIsAdmin* — Applies all App Roles to the logged in account when `true` or no roles when `false`. (Only applies if *AuthenticatedUser* is `true`.)
    - *BuildLocalDb* — Uses LocalDB when `true`. Uses in-memory data when `false`.
    - *UseEfMigrations* - Uses Entity Framework migrations when `true`. Deletes and recreates database when `false`. (Only applies if *BuildLocalDb* is `true`.)

* **WebApp Dev Server** — This profile connects to a remote database server for data and requires an SOG account to log in. *To use this profile, you must add the "appsettings.Development.json" file from the "app-config" repo.*

    Most development should be done using the Local profile. The Dev Server profile is only needed when specifically troubleshooting issues with the database server or SOG account.

Here's a visualization of how each launch profile (plus the `BuildLocalDb` setting) configures the application at runtime.

```mermaid
flowchart LR
    subgraph SPL["'Local' launch profile"]
        direction LR
        D[Domain]
        T["Test Data (in memory)"]
        R[Local Repository]
        A[App Services]
        W([Web App])

        W --> A
        A --> D
        A --> R
        R --> T
        T --> D
    end
```

```mermaid
flowchart LR
    subgraph SPB["'Local' launch profile + 'BuildLocalDb' setting"]
        direction LR
        D[Domain]
        T[Test Data]
        R[Infrastructure]
        A[App Services]
        W([Web App])
        B[(LocalDB)]

        W --> A
        A --> D
        R --> B
        A --> R
        T -->|Seed| B
        B -.-> D
    end
```

```mermaid
flowchart LR
    subgraph SPD["'Dev Server' launch profile (or Production)"]
        direction LR
        D[Domain]
        R[Infrastructure]
        A[App Services]
        W([Web App])
        B[(DB Server)]
        Z[[Azure AD]]

        W --> A
        A --> D
        A --> R
        R ==>|VPN| B -.-> D
        W ==>|SOG| Z
    end
```
