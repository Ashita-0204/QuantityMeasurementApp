using Microsoft.Data.SqlClient;
using QuantityMeasurementModel;
using QuantityMeasurementModel.DTOs;
using QuantityMeasurementRepository.Exceptions;
using QuantityMeasurementRepository.Util;
using System.Data;

namespace QuantityMeasurementRepository.Database
{
    public class QuantityMeasurementDatabaseRepository : IQuantityMeasurementRepository
    {
        private readonly ConnectionPool _pool;

        public QuantityMeasurementDatabaseRepository(DatabaseConfig? config = null)
        {
            config ??= new DatabaseConfig();
            _pool = new ConnectionPool(config);
            Console.WriteLine("[DatabaseRepository] Connected to SQL Server.");
        }

        // ── Save ─────────────────────────────────────────────
        public void Save(QuantityMeasurementEntity entity)
        {
            SqlConnection connection = _pool.GetConnection();

            try
            {
                SqlCommand command = new SqlCommand("sp_SaveMeasurement", connection);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@id", entity.Id);
                command.Parameters.AddWithValue("@timestamp", entity.Timestamp);
                command.Parameters.AddWithValue("@operation", entity.Operation);

                command.Parameters.AddWithValue("@operand1_value", (object?)entity.Operand1?.Value ?? DBNull.Value);
                command.Parameters.AddWithValue("@operand1_unit", (object?)entity.Operand1?.Unit ?? DBNull.Value);
                command.Parameters.AddWithValue("@operand1_category", (object?)entity.Operand1?.Category ?? DBNull.Value);

                command.Parameters.AddWithValue("@operand2_value", (object?)entity.Operand2?.Value ?? DBNull.Value);
                command.Parameters.AddWithValue("@operand2_unit", (object?)entity.Operand2?.Unit ?? DBNull.Value);
                command.Parameters.AddWithValue("@operand2_category", (object?)entity.Operand2?.Category ?? DBNull.Value);

                command.Parameters.AddWithValue("@result_value", (object?)entity.Result?.Value ?? DBNull.Value);
                command.Parameters.AddWithValue("@result_unit", (object?)entity.Result?.Unit ?? DBNull.Value);
                command.Parameters.AddWithValue("@result_category", (object?)entity.Result?.Category ?? DBNull.Value);

                command.Parameters.AddWithValue("@bool_result",
                    entity.BoolResult.HasValue ? (object)(entity.BoolResult.Value ? 1 : 0) : DBNull.Value);

                command.Parameters.AddWithValue("@scalar_result", (object?)entity.ScalarResult ?? DBNull.Value);

                command.Parameters.AddWithValue("@has_error", entity.HasError ? 1 : 0);
                command.Parameters.AddWithValue("@error_message",
                    string.IsNullOrEmpty(entity.ErrorMessage) ? DBNull.Value : (object)entity.ErrorMessage);

                command.ExecuteNonQuery();

                SaveHistory(connection, entity.Id, "SAVE");
            }
            catch (Exception ex)
            {
                throw new DatabaseException("Failed to save measurement.", ex);
            }
            finally
            {
                connection.Close();
            }
        }

        // ── GetAll ───────────────────────────────────────────
        public IReadOnlyList<QuantityMeasurementEntity> GetAll()
        {
            SqlConnection connection = _pool.GetConnection();

            try
            {
                SqlCommand command = new SqlCommand("sp_GetAllMeasurements", connection);
                command.CommandType = CommandType.StoredProcedure;

                return ReadEntities(command);
            }
            catch (Exception ex)
            {
                throw new DatabaseException("Failed to get all measurements.", ex);
            }
            finally
            {
                connection.Close();
            }
        }

        // ── GetRecent ────────────────────────────────────────
        public IReadOnlyList<QuantityMeasurementEntity> GetRecent(int count)
        {
            SqlConnection connection = _pool.GetConnection();

            try
            {
                SqlCommand command = new SqlCommand("sp_GetRecentMeasurements", connection);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@count", count);

                return ReadEntities(command);
            }
            catch (Exception ex)
            {
                throw new DatabaseException("Failed to get recent measurements.", ex);
            }
            finally
            {
                connection.Close();
            }
        }

        // ── GetByOperation ───────────────────────────────────
        public IReadOnlyList<QuantityMeasurementEntity> GetByOperation(string operation)
        {
            SqlConnection connection = _pool.GetConnection();

            try
            {
                SqlCommand command = new SqlCommand("sp_GetMeasurementsByOperation", connection);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@operation", operation);

                return ReadEntities(command);
            }
            catch (Exception ex)
            {
                throw new DatabaseException("Failed to get by operation.", ex);
            }
            finally
            {
                connection.Close();
            }
        }

        // ── GetByCategory ────────────────────────────────────
        public IReadOnlyList<QuantityMeasurementEntity> GetByCategory(string category)
        {
            SqlConnection connection = _pool.GetConnection();

            try
            {
                SqlCommand command = new SqlCommand("sp_GetMeasurementsByCategory", connection);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@category", category);

                return ReadEntities(command);
            }
            catch (Exception ex)
            {
                throw new DatabaseException("Failed to get by category.", ex);
            }
            finally
            {
                connection.Close();
            }
        }

        // ── GetTotalCount ────────────────────────────────────
        public int GetTotalCount()
        {
            SqlConnection connection = _pool.GetConnection();

            try
            {
                SqlCommand command = new SqlCommand("sp_GetTotalMeasurementCount", connection);
                command.CommandType = CommandType.StoredProcedure;

                object? result = command.ExecuteScalar();
                return result == null ? 0 : Convert.ToInt32(result);
            }
            catch (Exception ex)
            {
                throw new DatabaseException("Failed to get total count.", ex);
            }
            finally
            {
                connection.Close();
            }
        }

        // ── DeleteAll ────────────────────────────────────────
        public void Clear() => DeleteAll();

        public void DeleteAll()
        {
            SqlConnection connection = _pool.GetConnection();

            try
            {
                SqlCommand command = new SqlCommand("sp_DeleteAllMeasurements", connection);
                command.CommandType = CommandType.StoredProcedure;

                command.ExecuteNonQuery();

                Console.WriteLine("[DatabaseRepository] All measurements deleted.");
            }
            catch (Exception ex)
            {
                throw new DatabaseException("Failed to delete all measurements.", ex);
            }
            finally
            {
                connection.Close();
            }
        }

        // ── Helpers ──────────────────────────────────────────

        private static List<QuantityMeasurementEntity> ReadEntities(SqlCommand command)
        {
            List<QuantityMeasurementEntity> list = new List<QuantityMeasurementEntity>();

            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                QuantityMeasurementEntity entity = MapRow(reader);
                list.Add(entity);
            }

            reader.Close();
            return list;
        }

        private static QuantityMeasurementEntity MapRow(SqlDataReader reader)
        {
            Guid id = (Guid)reader["id"];
            DateTime timestamp = Convert.ToDateTime(reader["timestamp"]);
            string operation = reader["operation"].ToString()!;
            bool hasError = Convert.ToBoolean(reader["has_error"]);
            string errorMsg = reader["error_message"] == DBNull.Value ? "" : reader["error_message"].ToString()!;

            QuantityDTO? operand1 = ReadDTO(reader, "operand1_value", "operand1_unit", "operand1_category");
            QuantityDTO? operand2 = ReadDTO(reader, "operand2_value", "operand2_unit", "operand2_category");
            QuantityDTO? result = ReadDTO(reader, "result_value", "result_unit", "result_category");

            bool? boolResult = reader["bool_result"] == DBNull.Value ? null : Convert.ToBoolean(reader["bool_result"]);
            double? scalarResult = reader["scalar_result"] == DBNull.Value ? null : Convert.ToDouble(reader["scalar_result"]);

            return QuantityMeasurementEntity.Reconstruct(
                id, timestamp, operation,
                operand1, operand2, result,
                boolResult, scalarResult,
                hasError, errorMsg);
        }

        private static QuantityDTO? ReadDTO(SqlDataReader reader, string valueCol, string unitCol, string categoryCol)
        {
            if (reader[valueCol] == DBNull.Value)
                return null;

            return new QuantityDTO(
                Convert.ToDouble(reader[valueCol]),
                reader[unitCol].ToString()!,
                reader[categoryCol].ToString()!
            );
        }

        private static void SaveHistory(SqlConnection connection, Guid entityId, string action)
        {
            SqlCommand command = new SqlCommand(
                "INSERT INTO quantity_measurement_history (entity_id, action, action_timestamp) VALUES (@id,@action,@ts)",
                connection);

            command.Parameters.AddWithValue("@id", entityId);
            command.Parameters.AddWithValue("@action", action);
            command.Parameters.AddWithValue("@ts", DateTime.UtcNow);

            command.ExecuteNonQuery();
        }

        public string GetPoolStatistics() => _pool.GetStatistics();

        public void ReleaseResources()
        {
            Console.WriteLine("[DatabaseRepository] Resources released.");
        }
    }
}