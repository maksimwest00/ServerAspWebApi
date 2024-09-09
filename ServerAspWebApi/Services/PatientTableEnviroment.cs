using Microsoft.Data.SqlClient;
using ServerAspWebApi.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace ServerAspWebApi.Services
{
    public class PatientTableEnviroment
    {
        private DataBaseService _dataBaseService;

        public PatientTableEnviroment(DataBaseService dataBaseService)
        {
            _dataBaseService = dataBaseService;
        }

        public async Task<bool> Add(PatientModel patient)
        {
            string insertQuery = $"INSERT INTO Пациенты (Фамилия, Имя, Отчество, Адрес, ДатаРождения, Пол, Участок) VALUES (N'{patient.LastName}', N'{patient.FirstName}', N'{patient.Patronymic}', N'{patient.Address}', '{patient.DateBirthDay:yyyy-MM-dd}', N'{patient.Sex}', {patient.Region})";
            return await _dataBaseService.ExecuteNonQueryAsync(insertQuery);
        }

        public async Task<bool> Edit(PatientModel patient)
        {
            // false - не удалось обновить или пациент не найден
            // true - все хорошо
            string selectQuery = "SELECT * FROM Пациенты WHERE Id = @Id";
            string updateQuery = $@"UPDATE Пациенты SET 
                                    Имя = '{patient.FirstName}',
                                    Фамилия = '{patient.LastName}',
                                    Отчество = '{patient.Patronymic}',
                                    Адрес = '{patient.Address}',
                                    ДатаРождения = '{patient.DateBirthDay}',
                                    Пол = '{patient.Sex}',
                                    Участок = '{patient.Region}'
                                    WHERE Id = {patient.Id}";
            using (SqlConnection connection = new SqlConnection(DataBaseService.ConnectionString))
            {
                await connection.OpenAsync();
                using (SqlCommand selectCommand = new SqlCommand(selectQuery, connection))
                {
                    selectCommand.Parameters.AddWithValue("@Id", patient.Id);
                    using (SqlDataReader reader = await selectCommand.ExecuteReaderAsync())
                    {
                        if (!reader.Read())
                        {
                            return false;
                        }
                    }
                }
                using (SqlCommand updateCommand = new SqlCommand(updateQuery, connection))
                {
                    int affectedRows = await updateCommand.ExecuteNonQueryAsync();
                    return affectedRows > 0;
                }
            }
        }

        public async Task<bool> Delete(int patientID)
        {
            string removeQuery = $"DELETE FROM Пациенты WHERE Id = '{patientID}';";
            return await _dataBaseService.ExecuteNonQueryAsync(removeQuery);
        }

        public async Task<PatientModel> GetByID(int patientID)
        {
            PatientModel patient = null;
            string query = $"SELECT * FROM Пациенты WHERE Id = '{patientID}'";
            using (SqlConnection connection = new SqlConnection(DataBaseService.ConnectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                await connection.OpenAsync();
                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    if (reader.HasRows)
                    {
                        patient = new PatientModel();
                        while (await reader.ReadAsync())
                        {
                            patient.Id = reader.GetInt32(0);
                            patient.LastName = reader.GetString(1);
                            patient.FirstName = reader.GetString(2);
                            patient.Patronymic = reader.GetString(3);
                            patient.Address = reader.GetString(4);
                            patient.DateBirthDay = reader.GetDateTime(5);
                            patient.Sex = reader.GetString(6);
                            patient.Region = reader.GetInt32(7);
                        }

                    }
                }
            }
            return patient;
        }

        public async Task<List<PatientModel>> GetListByPageAndSort(int page, string sortBy, int countOnPage = 10)
        {
            List<string> columnsNames = await this.GetColumnNameInTable("Пациенты");
            if (!columnsNames.Contains(sortBy))
            {
                // Такого столбца нету чтобы по нему выполнять сортировку
                return null;
            }
            List<PatientModel> patientsList = new List<PatientModel>();
            string queryGetByPageAndSort = @$"WITH SOURCE AS(
                                            SELECT ROW_NUMBER() OVER(ORDER BY {sortBy}) AS RowNumber, *
                                            FROM Пациенты
                                            )
                                            SELECT * FROM SOURCE
                                            WHERE RowNumber > ({page} * {countOnPage}) - {countOnPage}
                                              AND RowNumber <= {page} * {countOnPage}";
            using (SqlConnection connection = new SqlConnection(DataBaseService.ConnectionString))
            {
                SqlCommand command = new SqlCommand(queryGetByPageAndSort, connection);
                connection.Open();
                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    if (reader.HasRows)
                    {
                        while (await reader.ReadAsync())
                        {
                            for (int i = 1; i < reader.FieldCount; i++)
                            {
                                Debug.WriteLine($"{reader.GetName(i)} {reader.GetValue(i)}");
                            }
                            patientsList.Add(new PatientModel
                            {
                                Id = reader.GetInt32(1),
                                LastName = reader.GetString(2),
                                FirstName = reader.GetString(3),
                                Patronymic = reader.GetString(4),
                                Address = reader.GetString(5),
                                DateBirthDay = reader.GetDateTime(6),
                                Sex = reader.GetString(7),
                                Region = reader.GetInt32(8)
                            });
                        }
                    }
                }
            }
            return patientsList;
        }

        private async Task<List<string>> GetColumnNameInTable(string tableName)
        {
            string query = $"SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = N'{tableName}'";
            List<string> columns = new List<string>();
            using (SqlConnection connection = new SqlConnection(DataBaseService.ConnectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                await connection.OpenAsync();
                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    if (reader.HasRows)
                    {
                        while (await reader.ReadAsync())
                        {
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                columns.Add(reader.GetValue(i).ToString());
                            }
                        }
                    }
                }
            }
            return columns;
        }
    }
}
