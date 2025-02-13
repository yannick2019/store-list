# StoryList API

## Overview
Web API application built using ASP.NET Core and Entity Framework Core. It provides a RESTful API for managing shopping lists.

## Feature
-	Create, read, update, and delete shopping lists.
-	Add, update, and remove items in a shopping list.
-	CORS support for frontend integration.
-	OpenAPI documentation and Scalar for easy API exploration.

## Prerequisites
-	.NET 9.0 SDK
-	SQL Server

## Installation
1. Clone the repository:
```
git clone "the repository"
cd store-list
```

2. Set up the database connection string in appsettings.json:
```
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=your_server;Database=StoreListDb;User Id=your_user;Password=your_password;"
  }
}
```

3. Restore the dependencies and build the project
```
dotnet restore
dotnet build
```

4. Apply the database update:
```
dotnet ef database update
```

## Running the Application

1. Start the application
```
dotnet run --project StoreList.API
```
2. The API will be available at https://localhost:7019

## Usage
-	Access the OpenAPI documentation at https://localhost:7019/scalar/v1.
-	Use the provided endpoints to manage shopping lists and items.


