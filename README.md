# QuantityMeasurementApp

## Overview

QuantityMeasurementApp is a C# console application designed to model and manipulate physical measurements such as length, weight, volume, and temperature. The application supports equality comparison, unit conversion, arithmetic operations, and database persistence while demonstrating strong software design principles and scalable architecture.

The system evolves progressively through multiple use cases (UC1–UC16). Each use case incrementally improves the architecture and introduces new capabilities.

The project demonstrates the following software engineering principles:

- Object-Oriented Programming (OOP)
- DRY (Don't Repeat Yourself)
- SOLID Principles
- Generic Programming
- Immutability
- N-Tier Architecture
- Database Integration using ADO.NET

The application begins with simple measurement comparisons and evolves into a scalable, maintainable architecture capable of supporting multiple measurement categories and persistence.

---

# Use Cases

---

# UC1: Feet Measurement Equality

## Description

This use case introduces the basic functionality for comparing two length measurements expressed in feet.

## Features

- Compares two values measured in feet
- Handles null comparisons safely
- Ensures type-safe comparisons
- Validates numeric inputs

## Concepts Applied

- Equality comparison
- Defensive programming
- Basic object-oriented modeling

---

# UC2: Feet and Inches Equality

## Description

This use case extends the system by introducing inches as an additional unit of measurement.

## Features

- Supports multiple length units
- Enables comparison between different units
- Performs validation and type checking
- Expands the measurement model

## Concepts Applied

- Multi-unit measurement handling
- Cross-unit comparison
- Input validation

---

# UC3: Generic Quantity Class (DRY Principle)

## Description

This use case introduces a reusable quantity class representing measurements using a numeric value and an associated unit.

## Features

- Introduces a reusable quantity representation
- Introduces an enumeration for supported length units
- Removes duplicated comparison logic
- Supports cross-unit equality comparison

## Concepts Applied

- DRY principle
- Encapsulation
- Enumeration-based unit representation
- Value-based equality

---

# UC4: Extended Unit Support

## Description

This use case expands the system to support additional units without changing the core equality logic.

## Features

- Adds additional length units
- Maintains compatibility with existing logic
- Demonstrates extensibility of the system

## Concepts Applied

- Scalable design
- Enum extensibility
- Open-Closed Principle

---

# UC5: Unit-to-Unit Conversion

## Description

This use case introduces a conversion mechanism allowing measurements to be converted between different units.

## Features

- Converts values between supported units
- Uses base-unit normalization
- Ensures accurate conversion calculations
- Provides reusable conversion functionality

## Concepts Applied

- Base unit normalization
- Conversion abstraction
- Precision handling

---

# UC6: Addition of Two Length Units

## Description

This use case introduces arithmetic addition between measurement quantities.

## Features

- Supports addition of quantities within the same category
- Performs automatic unit conversion before calculation
- Maintains immutability of measurement objects
- Returns results in a consistent unit representation

## Concepts Applied

- Arithmetic operations on value objects
- Unit normalization
- Immutable object design

---

# UC7: Addition with Target Unit Specification

## Description

This use case enhances addition functionality by allowing results to be returned in a specified unit.

## Features

- Supports specifying a target unit
- Uses method overloading for flexible operations
- Performs necessary conversion before returning results

## Concepts Applied

- Method overloading
- Flexible API design
- Conversion abstraction

---

# UC8: Subtraction of Two Length Units

## Description

This use case introduces subtraction between measurement quantities.

## Features

- Supports subtraction between quantities
- Performs automatic unit conversion
- Supports negative results
- Maintains object immutability

## Concepts Applied

- Arithmetic subtraction
- Floating-point tolerance handling
- Immutable value objects

---

# UC9: Multi-Category Measurement Support

## Description

This use case expands the application to support multiple measurement categories.

Separate classes represent different measurement types.

## Features

- Supports length, volume, and weight measurements
- Each category defines its own units
- Prevents cross-category arithmetic operations
- Maintains strong type safety

## Concepts Applied

- Domain modeling
- Category separation
- Type-safe measurement handling

---

# UC10: Generic Quantity Class with Unit Interface

## Description

This use case refactors the multi-category architecture using generics and a common unit interface.

## Features

- Introduces a generic quantity class
- Introduces a shared unit interface
- Eliminates duplicated logic across measurement categories
- Supports extensibility for future categories

## Concepts Applied

- Generic programming
- Interface abstraction
- Open-Closed Principle

---

# UC11: Multi-Category Measurement Using Generic Quantity

## Description

This use case improves the generic architecture by enforcing compile-time type safety for measurement operations.

## Features

- Uses generics to represent measurement categories
- Ensures operations occur only between compatible quantities
- Supports scalable measurement modeling

## Concepts Applied

- Type-safe generics
- Interface-based design
- Reusable domain architecture

---

# UC12: Arithmetic Operations on Quantities

## Description

This use case introduces arithmetic operations for generic quantities.

## Features

- Supports addition, subtraction, and division
- Performs base unit conversion before arithmetic operations
- Allows result unit specification
- Maintains immutability and validation

## Concepts Applied

- Generic arithmetic operations
- Unit normalization
- Domain logic encapsulation

---

# UC13: Centralized Arithmetic Logic (DRY Principle)

## Description

This use case refactors arithmetic operations to eliminate duplicated logic.

## Features

- Introduces a centralized arithmetic processing method
- Uses an operation enumeration to determine arithmetic behavior
- Ensures consistent validation and conversion

## Concepts Applied

- DRY principle
- Centralized processing
- Maintainable architecture

---

# UC14: Temperature Measurement with Selective Arithmetic Support

## Description

This use case introduces temperature measurement support while restricting unsupported arithmetic operations.

## Features

- Adds temperature measurement support
- Allows equality checks and conversions
- Prevents unsupported arithmetic operations
- Provides controlled operation validation

## Concepts Applied

- Selective capability implementation
- Interface-based feature control
- Safe operation validation

---

# UC15: N-Tier Architecture Refactoring

## Description

This use case restructures the application into an N-Tier architecture to improve scalability, maintainability, and separation of concerns.

## Architecture Layers

### Presentation Layer

Handles user interaction and presentation logic.

### Service Layer

Contains business logic and orchestrates measurement operations.

### Repository Layer

Handles data access and persistence operations.

### Domain Model Layer

Contains core domain objects such as quantities and unit definitions.

## Key Architectural Concepts

### Data Transfer Objects

DTOs transfer data between layers without exposing internal domain objects.

### Dependency Injection

Dependencies are provided to components instead of being created internally.

### Error Handling as Data

Errors are represented as structured responses rather than uncontrolled exceptions.

### Immutability

Measurement objects remain immutable to ensure consistent behavior.

## SOLID Principles Applied

- Single Responsibility Principle
- Open Closed Principle
- Liskov Substitution Principle
- Interface Segregation Principle
- Dependency Inversion Principle

## Benefits

- Improved maintainability
- Improved testability
- Better separation of concerns
- Scalable architecture

---

# UC16: Database Integration with ADO.NET for Quantity Measurement Persistence

## Description

This use case integrates database persistence into the application using ADO.NET.

Measurement operations and results can now be stored and retrieved from a relational database.

## Features

- Database connectivity using ADO.NET
- Connection pooling support
- Parameterized SQL queries
- Structured exception handling
- Separation of persistence logic from business logic

## Database Design

A relational schema stores measurement operations, operands, units, and results.

## Persistence Layer Responsibilities

- Managing database connections
- Executing SQL commands
- Handling transactions
- Mapping database records to data objects

## Configuration Management

Database connection details are externalized to support flexible deployment.

## Testing Support

Supports testing using mock repositories and test databases.

## Performance Considerations

- Efficient database connections
- Optimized SQL queries
- Transaction management
- Resource cleanup
