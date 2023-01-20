# Template Application

This repository contains a template for use in creating new web applications.
(Current template version [v2023.1.20](https://github.com/gaepdit/template-app/tree/v2023.1.20).)


[![.NET Test](https://github.com/gaepdit/template-app/actions/workflows/dotnet-test.yml/badge.svg)](https://github.com/gaepdit/template-app/actions/workflows/dotnet-test.yml)
[![CodeQL](https://github.com/gaepdit/template-app/actions/workflows/codeql-analysis.yml/badge.svg)](https://github.com/gaepdit/template-app/actions/workflows/codeql-analysis.yml)
[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=gaepdit_template-app&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=gaepdit_template-app)
[![Lines of Code](https://sonarcloud.io/api/project_badges/measure?project=gaepdit_template-app&metric=ncloc)](https://sonarcloud.io/summary/new_code?id=gaepdit_template-app)

*[The SonarCloud badges require a SonarCloud project to be configured.]*

---

## Template setup and use

The following steps describe how to use the template for a new application.

### Initialize the template files

* Run the "create-sln.ps1" file to create a new solution file.
* Rename or search and replace the following terms. *(Caution: not all of these will be visible in the Visual Studio solution view.)*
    - "MY_APP_NAME" - Replace with the readable display name of the app.
    - `MyAppRoot`:
        - Rename the root namespace for the app.
        - Update the `<RootNamespace>` element in each "csproj" file.
        - Update the namespace in the "_ViewImports.cshtml" file.
        - Update the exclusions in the coverlet commands in the "sonarcloud-scan.yml" file.
        - Update the exclusions in the "finecodecoverage-settings.xml" file.
    - "template-app" - Search and replace with the repository name. This will affect the following:
        - The LocalDB database name in various connection strings.
        - The project key in the "sonarcloud-scan.yml" workflow file.
        - The comment URL path in the "lighthouse-analysis.yml" workflow file.
        - The URLs in the GitHub and SonarCloud badges above.

### Customize the application

* Update this README file with information about the new application.
* Change the branding colors in "src\WebApp\wwwroot\css\site.css".

### Prepare for deployment

* Create server-specific settings and config files and add copies to the "app-config" repository.
* Create Web Deploy Publish Profiles for each web server using the "Example-Server.pubxml" file as an example.
* Configure the following external services as needed:
    - [Azure App registration](https://portal.azure.com/#view/Microsoft_AAD_RegisteredApps/ApplicationsListBlade) to manage employee authentication. *(Add configuration settings in the "AzureAd" section in a server settings file.)*
    - [Raygun](https://app.raygun.com/) for crash reporting and performance monitoring. *(Add the API key to the "RaygunSettings" section in a server settings file.)*
    - [SonarCloud](https://sonarcloud.io/projects) for code quality and security scanning. *(Update the project key in the "sonarcloud-scan.yml" workflow file and in the badges above.)*
    - [Better Uptime](https://betterstack.com/better-uptime) for site uptime monitoring. *(No app configuration needed.)*

---

## Background and project requirements

TODO

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
* **EfRepository** — A class library implementing the repository and services using LocalDb and Entity Framework.
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
        R[EfRepository]
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
        R[EfRepository]
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
