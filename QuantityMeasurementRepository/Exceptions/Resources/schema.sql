-- ============================================================
-- UC16: Quantity Measurement App - Complete SQL Server Schema
-- Run this once in SSMS (connected to any DB or master)
-- ============================================================

-- ─────────────────────────────────────────────────────────────
-- 1. CREATE DATABASE
-- ─────────────────────────────────────────────────────────────
IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'QuantityMeasurementDb')
BEGIN
    CREATE DATABASE QuantityMeasurementDb;
END
GO

USE QuantityMeasurementDb;
GO

-- ─────────────────────────────────────────────────────────────
-- 2. CREATE TABLES
-- ─────────────────────────────────────────────────────────────
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'quantity_measurement_entity')
BEGIN
    CREATE TABLE quantity_measurement_entity (
        id                  UNIQUEIDENTIFIER    NOT NULL PRIMARY KEY,
        timestamp           DATETIME2           NOT NULL,
        operation           NVARCHAR(50)        NOT NULL,
        operand1_value      FLOAT               NULL,
        operand1_unit       NVARCHAR(50)        NULL,
        operand1_category   NVARCHAR(50)        NULL,
        operand2_value      FLOAT               NULL,
        operand2_unit       NVARCHAR(50)        NULL,
        operand2_category   NVARCHAR(50)        NULL,
        result_value        FLOAT               NULL,
        result_unit         NVARCHAR(50)        NULL,
        result_category     NVARCHAR(50)        NULL,
        bool_result         BIT                 NULL,
        scalar_result       FLOAT               NULL,
        has_error           BIT                 NOT NULL DEFAULT 0,
        error_message       NVARCHAR(MAX)       NULL
    );
END
GO

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'quantity_measurement_history')
BEGIN
    CREATE TABLE quantity_measurement_history (
        id               INT IDENTITY(1,1)  NOT NULL PRIMARY KEY,
        entity_id        UNIQUEIDENTIFIER   NOT NULL,
        action           NVARCHAR(50)       NOT NULL,
        action_timestamp DATETIME2          NOT NULL,
        CONSTRAINT fk_history_entity
            FOREIGN KEY (entity_id)
            REFERENCES quantity_measurement_entity(id)
            ON DELETE CASCADE
    );
END
GO

-- ─────────────────────────────────────────────────────────────
-- 3. CREATE INDEXES
-- ─────────────────────────────────────────────────────────────
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'idx_entity_operation')
    CREATE INDEX idx_entity_operation  ON quantity_measurement_entity(operation);
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'idx_entity_category')
    CREATE INDEX idx_entity_category   ON quantity_measurement_entity(operand1_category);
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'idx_entity_timestamp')
    CREATE INDEX idx_entity_timestamp  ON quantity_measurement_entity(timestamp);
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'idx_history_entity_id')
    CREATE INDEX idx_history_entity_id ON quantity_measurement_history(entity_id);
GO

-- ─────────────────────────────────────────────────────────────
-- 4. STORED PROCEDURES
-- ─────────────────────────────────────────────────────────────

-- Save a measurement record
CREATE OR ALTER PROCEDURE sp_SaveMeasurement (
    @id                 UNIQUEIDENTIFIER,
    @timestamp          DATETIME2,
    @operation          NVARCHAR(50),
    @operand1_value     FLOAT           = NULL,
    @operand1_unit      NVARCHAR(50)    = NULL,
    @operand1_category  NVARCHAR(50)    = NULL,
    @operand2_value     FLOAT           = NULL,
    @operand2_unit      NVARCHAR(50)    = NULL,
    @operand2_category  NVARCHAR(50)    = NULL,
    @result_value       FLOAT           = NULL,
    @result_unit        NVARCHAR(50)    = NULL,
    @result_category    NVARCHAR(50)    = NULL,
    @bool_result        BIT             = NULL,
    @scalar_result      FLOAT           = NULL,
    @has_error          BIT,
    @error_message      NVARCHAR(MAX)   = NULL
) AS
BEGIN
    INSERT INTO quantity_measurement_entity (
        id, timestamp, operation,
        operand1_value, operand1_unit, operand1_category,
        operand2_value, operand2_unit, operand2_category,
        result_value, result_unit, result_category,
        bool_result, scalar_result, has_error, error_message
    ) VALUES (
        @id, @timestamp, @operation,
        @operand1_value, @operand1_unit, @operand1_category,
        @operand2_value, @operand2_unit, @operand2_category,
        @result_value, @result_unit, @result_category,
        @bool_result, @scalar_result, @has_error, @error_message
    );
END
GO

-- Save a history/audit entry
CREATE OR ALTER PROCEDURE sp_SaveHistory (
    @entity_id        UNIQUEIDENTIFIER,
    @action           NVARCHAR(50),
    @action_timestamp DATETIME2
) AS
BEGIN
    INSERT INTO quantity_measurement_history (entity_id, action, action_timestamp)
    VALUES (@entity_id, @action, @action_timestamp);
END
GO

-- Get all measurements ordered oldest first
CREATE OR ALTER PROCEDURE sp_GetAllMeasurements AS
BEGIN
    SELECT * FROM quantity_measurement_entity
    ORDER BY timestamp ASC;
END
GO

-- Get most recent N measurements
CREATE OR ALTER PROCEDURE sp_GetRecentMeasurements (
    @count INT
) AS
BEGIN
    SELECT TOP (@count) * FROM quantity_measurement_entity
    ORDER BY timestamp DESC;
END
GO

-- Get total count of measurements
CREATE OR ALTER PROCEDURE sp_GetTotalMeasurementCount AS
BEGIN
    SELECT COUNT(*) FROM quantity_measurement_entity;
END
GO

-- Get measurements filtered by operation type
CREATE OR ALTER PROCEDURE sp_GetMeasurementsByOperation (
    @operation NVARCHAR(50)
) AS
BEGIN
    SELECT * FROM quantity_measurement_entity
    WHERE operation = @operation
    ORDER BY timestamp ASC;
END
GO

-- Get measurements filtered by category
CREATE OR ALTER PROCEDURE sp_GetMeasurementsByCategory (
    @category NVARCHAR(50)
) AS
BEGIN
    SELECT * FROM quantity_measurement_entity
    WHERE operand1_category = @category
    ORDER BY timestamp ASC;
END
GO

-- Delete all records (used by tests for cleanup)
CREATE OR ALTER PROCEDURE sp_DeleteAllMeasurements AS
BEGIN
    DELETE FROM quantity_measurement_history;
    DELETE FROM quantity_measurement_entity;
END
GO

USE QuantityMeasurementDb;
SELECT * FROM quantity_measurement_entity;
SELECT COUNT(*) FROM quantity_measurement_entity;