# 🧮 QuantityMeasurementApp

> 🚀 A scalable, production-ready backend system built with **C# & .NET**, designed to handle real-world measurement operations with precision, performance, and clean architecture.

---

## ✨ Overview

**QuantityMeasurementApp** is an advanced backend application that models and processes physical quantities such as:

- 📏 Length
- ⚖️ Weight
- 🌡️ Temperature
- 🧪 Volume

The project starts with basic equality comparisons and evolves into a **full-fledged enterprise-grade system** featuring:

- 🔐 Secure Authentication using JWT
- 🗄️ Database Persistence (ADO.NET + EF Core)
- ⚡ Multi-layer Caching (Redis + In-Memory)
- 🌐 RESTful API + Console Interface
- 🧪 Extensive Unit & Integration Testing

💡 This project is built with a strong focus on **clean architecture, scalability, and maintainability**, making it ideal for real-world backend systems.

---

## 🚀 Key Features

- ✅ Multi-unit measurement handling
- 🔁 Accurate unit conversions with base normalization
- ➕ Arithmetic operations (Add, Subtract, Divide)
- 🧠 Intelligent validation & exception handling
- 📊 Operation history tracking
- 🔐 Secure API access with JWT
- ⚡ High-performance caching (Redis + Memory)
- 🧪 Comprehensive testing suite

---

## 🚀 Getting Started

### 🔧 Prerequisites

- .NET SDK installed
- SQL Server
- (Optional) Redis

---

### 🛠️ Build the Solution

```bash
dotnet build QuantityMeasurementApp.sln
```

---

### 🖥️ Run Console Application

```bash
dotnet run --project QuantityMeasurementConsoleApp
```

👉 Features:

- Interactive menu-driven system
- Supports all use cases
- Allows repository selection (Cache / Redis / SQL)

---

### 🌐 Run Web API

```bash
dotnet run --project QuantityMeasurementApi
```

👉 Features:

- Swagger UI enabled 📘
- JWT Authentication 🔐
- REST endpoints for all operations

---

## 🏗️ Architecture (N-Tier)

```
Presentation Layer (Console / API)
        ↓
Business Layer (Services)
        ↓
Repository Layer (DB / Cache / Redis)
        ↓
Model Layer (DTOs, Entities, Units)
```

---

## 🔹 Layer Breakdown

### 🎯 Presentation Layer

- Handles user interaction
- Includes:
  - Console UI
  - REST API Controllers

---

### ⚙️ Business Layer

- Core logic of application
- Handles:
  - Validation
  - Arithmetic operations
  - Unit conversions
  - Business rules enforcement

---

### 🗄️ Repository Layer

- Data access abstraction
- Supports:
  - SQL Server
  - Redis Cache
  - In-memory storage

---

### 📦 Model Layer

- Core domain representation
- Includes:
  - DTOs
  - Entities
  - Enums (Units)
  - Custom Exceptions

---

## 🧠 Core Concepts & Principles

- 🧱 Object-Oriented Programming (OOP)
- ♻️ DRY (Don't Repeat Yourself)
- 🧩 SOLID Principles
- 🔁 Generic Programming
- 🔒 Immutability
- 🔗 Dependency Injection
- 🏗️ N-Tier Architecture
- ⚠️ Exception Handling Middleware
- 🔐 Secure Authentication (JWT)

---

## 📚 Use Cases (UC1–UC18)

---

### 🟢 UC1–UC5: Foundations

- Basic equality checks (Feet, Inches)
- Cross-unit comparison
- Generic Quantity abstraction
- Unit conversion logic
- Precision & validation handling

---

### 🟡 UC6–UC10: Arithmetic & Design

- Addition, subtraction, division
- Target unit-based results
- Multi-category measurement support
- Generic + Interface-based architecture

---

### 🔵 UC11–UC14: Advanced Measurements

- Weight operations ⚖️
- Volume handling 🧪
- Temperature conversions 🌡️
- Restricted arithmetic for temperature

---

### 🟣 UC15: N-Tier Architecture

- Layer separation
- DTO-based communication
- Dependency Injection
- Scalable system design

---

### 🟠 UC16: Database Integration

- ADO.NET + EF Core
- SQL persistence
- Transaction handling
- Repository abstraction

---

### 🔴 UC17: Exception Handling & Validation

- Custom exception classes
- Global exception middleware
- Structured error responses
- Safe arithmetic validations

---

### 🟤 UC18: Measurement Command Center

- Centralized operation logging
- Supports:
  - 🧠 In-memory cache
  - ⚡ Redis
  - 🗄️ SQL database
- Tracks:
  - Operation history
  - Usage count
  - Errors

---

## 🔐 Authentication & Security

- JWT-based authentication
- Secure API endpoints
- Password hashing & validation
- Middleware-based token verification

---

## 🌐 API Capabilities

- 🔍 Measurement comparison
- 🔁 Unit conversion
- ➕ Arithmetic operations
- 📊 History tracking
- 🔐 Authentication endpoints

---

## 🧪 Testing Strategy

Run all tests:

```bash
dotnet test
```

### ✔️ Coverage Includes:

- Unit tests
- Integration tests
- Repository layer tests
- Edge case validations
- Arithmetic correctness

---

## 📦 Tech Stack

- 💻 C# / .NET
- 🌐 ASP.NET Web API
- 🗄️ SQL Server
- ⚡ Redis
- 🧪 xUnit

---

## 🎯 Key Highlights

- 🚀 Production-ready backend architecture
- 🧠 Strong domain modeling
- ⚡ High performance with caching
- 🔐 Secure authentication system
- 🧪 Fully tested system
- 📦 Clean, modular, scalable design

---

## 👩‍💻 Author

Built with 💙 for mastering backend development, clean architecture, and real-world system design.

---

## ⭐ Future Enhancements

- 🐳 Docker containerization
- 🚀 CI/CD pipeline integration
- 📊 Interactive dashboard UI
