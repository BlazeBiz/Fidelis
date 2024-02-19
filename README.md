# Iron Marauder

_Iron Marauder_ is an application created to demonstrate architecture and coding concepts. It allows the user to search, edit, and create Customers and Sales Orders.

The _Iron Marauder_ application is purely for demonstration. The app isn't complete enough to allow you to accomplish anything meaningful. The database is a tiny subset of an actual ERP application. The database and user interface were chosen to be enough to demonstrate meaningful concepts, without being overly complex, thus detracting from its usefulness for education.

This document contains information about the application architecture, its Azure hosting environment, and specific coding techniques that may interest a developer.

* Architecture
    * Database
    * API server
    * Client
* Hosting setup
    * Database
    * API server
    * Client
* Interesting features
    * Database
        - database project
        - deployment scripts and environment variables
        - test and production data
        - Stored procedures
            - Complex parameters JSON
            - Complex result sets
        - Publish profiles (test and production)
    * API server
        - Code generation
            - Partial classes, base folder, and custom properties and methods
            - Stored proc execution
            - Custom properties (e.g., Customer vs. CustomerId)
        - Dependency injection - multiple levels
        - Controllers
        - Services
        - Models
        - Data
        - Publish profile (Azure)
    * Client
        - Routing
            - Push, replace, parameters, 
            - Route styling (exact match)
            - Page title
        - Configurable navigation panel
        - TODO: State and local storage
            - Maybe lookup tables, like State, Payment Terms
        - TODO: Responsive design
        - 


