using Microsoft.Data.SqlClient;
using ServerAspWebApi.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ServerAspWebApi.Services
{
    public class DoctorTableEnviroment
    {
        private DataBaseService _dataBaseService;

        public DoctorTableEnviroment(DataBaseService dataBaseService)
        {
            _dataBaseService = dataBaseService;
        }

        public async Task<bool> Add(DoctorModel doctor)
        {
            string insertQuery = $"INSERT INTO Врачи (ФИО, Кабинет, Специализация, Участок) VALUES (N'{doctor.FullName}', '{doctor.Cabinet}', N'{doctor.Specialization}', '{doctor.Region}')";
            return await _dataBaseService.ExecuteNonQueryAsync(insertQuery);
        }

        public async Task<bool> Edit(DoctorModel doctor)
        {
            // false - не удалось обновить или пациент не найден
            // true - все хорошо
            string selectQuery = "SELECT * FROM Врачи WHERE Id = @Id";
            string updateQuery = $@"UPDATE Врачи SET 
                                    ФИО = '{doctor.FullName}',
                                    Специализация = N'{doctor.Specialization}',
                                    Участок = '{doctor.Region}'
                                    WHERE Id = {doctor.Id}";
            try
            {
                using (SqlConnection connection = new SqlConnection(DataBaseService.ConnectionString))
                {
                    await connection.OpenAsync();
                    using (SqlCommand selectCommand = new SqlCommand(selectQuery, connection))
                    {
                        selectCommand.Parameters.AddWithValue("@Id", doctor.Id);
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
            catch
            {
                return false;
            }

        }

        public async Task<bool> Delete(int doctorID)
        {
            string removeQuery = $"DELETE FROM Врачи WHERE Id = '{doctorID}';";
            return await _dataBaseService.ExecuteNonQueryAsync(removeQuery);
        }

        public async Task<DoctorModel> GetByID(int doctorID)
        {
            DoctorModel doctor = null;
            string query = $"SELECT * FROM Врачи WHERE Id = '{doctorID}'";
            using (SqlConnection connection = new SqlConnection(DataBaseService.ConnectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                await connection.OpenAsync();
                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    if (reader.HasRows)
                    {
                        doctor = new DoctorModel();
                        while (await reader.ReadAsync())
                        {
                            doctor.Id = reader.GetInt32(0);
                            doctor.FullName = reader.GetString(1);
                            doctor.Cabinet = reader.GetInt32(2);
                            doctor.Specialization = reader.GetString(3);
                            doctor.Region = reader.GetInt32(4);
                        }

                    }
                }
            }
            return doctor;
        }

        public async Task<List<DoctorModel>> GetListByPageAndSort(int page, string sortBy, int countOnPage = 10)
        {
            List<string> columnsNames = await this.GetColumnNameInTable("Врачи");
            if (!columnsNames.Contains(sortBy))
            {
                // Такого столбца нету чтобы по нему выполнять сортировку
                return null;
            }
            List<DoctorModel> doctorsList = new List<DoctorModel>();
            string queryGetByPageAndSort = @$"WITH SOURCE AS(
                                            SELECT ROW_NUMBER() OVER(ORDER BY {sortBy}) AS RowNumber, *
                                            FROM Врачи
                                            )
                                            SELECT * FROM SOURCE
                                            WHERE RowNumber > ({page} * {countOnPage}) - {countOnPage}
                                              AND RowNumber <= {page} * {countOnPage}";
            using (SqlConnection connection = new SqlConnection(DataBaseService.ConnectionString))
            {
                SqlCommand command = new SqlCommand(queryGetByPageAndSort, connection);
                await connection.OpenAsync();
                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    if (reader.HasRows)
                    {
                        while (await reader.ReadAsync())
                        {
                            doctorsList.Add(new DoctorModel
                            {
                                Id = reader.GetInt32(1),
                                FullName = reader.GetString(2),
                                Cabinet = reader.GetInt32(3),
                                Specialization = reader.GetString(4),
                                Region = reader.GetInt32(5)
                            });
                        }
                    }
                }
            }
            return doctorsList;
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
