using Microsoft.Data.SqlClient;
using ServerAspWebApi.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Threading.Tasks;

namespace ServerAspWebApi.Services
{
    public class DataBaseService
    {
        // ����� ���������� �� �������������� � ����� ������   
        // ����� ��� ���� ���� �������� ����� ID ������

        public const string connectionString = "Server=(localdb)\\MSSQLLocalDB;Database=mainDataBase;Trusted_Connection=True;";

        public void TEST_ConnectToDB()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                Debug.WriteLine("����������� �������");
                Debug.WriteLine("�������� �����������:");
                Debug.WriteLine($"\t������ �����������: {connection.ConnectionString}");
                Debug.WriteLine($"\t���� ������: {connection.Database}");
                Debug.WriteLine($"\t������: {connection.DataSource}");
                Debug.WriteLine($"\t������ �������: {connection.ServerVersion}");
                Debug.WriteLine($"\t���������: {connection.State}");
                Debug.WriteLine($"\tWorkstationld: {connection.WorkstationId}");
            }
            Debug.WriteLine("����������� �������...");
            Debug.WriteLine("��������� ��������� ������.");
        }

        public DataBaseService()
        {
            CreateTables();
            InsertRegions();
            InsertSpecializations();
            InsertCabinets();
            InsertPatients();
            InsertDoctors();
        }

        #region CTOR Methods

        private void CreateTables()
        {
            string createRegionsTableQuery = @"
                CREATE TABLE ������� (
                    ����� INT PRIMARY KEY
            )";
            string createSpecializationsTableQuery = @"
                CREATE TABLE ������������� (
                    �������� NVARCHAR(100) PRIMARY KEY
                )";
            string createCabinetesTableQuery = @"
                CREATE TABLE �������� (
                    ����� INT PRIMARY KEY
                )";
            string createPatientsTableQuery = @"
                CREATE TABLE �������� (
                    Id INT PRIMARY KEY IDENTITY(1,1),
                    ������� NVARCHAR(100),
                    ��� NVARCHAR(100),
                    �������� NVARCHAR(100),
                    ����� NVARCHAR(255),
                    ������������ DATE,
                    ��� NVARCHAR(10),
                    ������� INT,
                    FOREIGN KEY (�������) REFERENCES �������(�����)
                )";
            string createDoctorsTableQuery = @"
                CREATE TABLE ����� (
                    Id INT PRIMARY KEY IDENTITY(1,1),
                    ��� NVARCHAR(100),
                    ������� INT,
                    ������������� NVARCHAR(100),
                    ������� INT,
                    FOREIGN KEY (�������) REFERENCES ��������(�����),
                    FOREIGN KEY (�������������) REFERENCES �������������(��������),
                    FOREIGN KEY (�������) REFERENCES �������(�����)
                )";
            if (!this.TableExists("�������"))
            {
                this.ExecuteNonQuery(createRegionsTableQuery);
            }
            else
            {
                Console.WriteLine("������� ������� ��� �������");
            }
            if (!this.TableExists("�������������"))
            {
                this.ExecuteNonQuery(createSpecializationsTableQuery);
            }
            else
            {
                Console.WriteLine("������� ������������� ��� �������");
            }
            if (!this.TableExists("��������"))
            {
                this.ExecuteNonQuery(createCabinetesTableQuery);
            }
            else
            {
                Console.WriteLine("������� �������� ��� �������");
            }
            if (!this.TableExists("��������"))
            {
                this.ExecuteNonQuery(createPatientsTableQuery);
            }
            else
            {
                Console.WriteLine("������� �������� ��� �������");
            }
            if (!this.TableExists("�����"))
            {
                this.ExecuteNonQuery(createDoctorsTableQuery);
            }
            else
            {
                Console.WriteLine("������� ����� ��� �������");
            }
        }

        private bool TableExists(string tableName)
        {
            string query = $"SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = N'{tableName}'";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    int count = (int)command.ExecuteScalar();
                    return count > 0;
                }
            }
        }

        private void InsertRegions()
        {
            string[] regions = { "1", "2", "3", "4", "5" };
            foreach (var region in regions)
            {
                string checkQuery = $"SELECT COUNT(*) FROM ������� WHERE ����� = '{region}'";
                int count = 0;
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(checkQuery, connection))
                    {
                        count = (int)command.ExecuteScalar();
                    }
                }
                if (count == 0)
                {
                    string insertQuery = $"INSERT INTO ������� (�����) VALUES ('{region}')";
                    ExecuteNonQuery(insertQuery);
                    Debug.WriteLine($"��������: {region}");
                }
                else
                {
                    Debug.WriteLine($"������ '{region}' ��� ����������.");
                }
            }
        }

        private void InsertSpecializations()
        {
            string[] specializations = { "��������", "������", "���������" };
            foreach (var specialization in specializations)
            {
                string checkQuery = $"SELECT COUNT(*) FROM ������������� WHERE �������� = N'{specialization}'";
                int count = 0;
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(checkQuery, connection))
                    {
                        count = (int)command.ExecuteScalar();
                    }
                }
                if (count == 0)
                {
                    string insertQuery = $"INSERT INTO ������������� (��������) VALUES (N'{specialization}')";
                    ExecuteNonQuery(insertQuery);
                    Debug.WriteLine($"��������� �������������: {specialization}");
                }
                else
                {
                    Debug.WriteLine($"������������� '{specialization}' ��� ����������.");
                }
            }
        }

        private void InsertCabinets()
        {
            string[] cabinets = { "1", "2", "3", "4", "5" };
            foreach (var cabinet in cabinets)
            {
                string checkQuery = $"SELECT COUNT(*) FROM �������� WHERE ����� = '{cabinet}'";
                int count = 0;
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(checkQuery, connection))
                    {
                        count = (int)command.ExecuteScalar();
                    }
                }
                if (count == 0)
                {
                    string insertQuery = $"INSERT INTO �������� (�����) VALUES ('{cabinet}')";
                    ExecuteNonQuery(insertQuery);
                    Debug.WriteLine($"�������� �������: {cabinet}");
                }
                else
                {
                    Debug.WriteLine($"������� '{cabinet}' ��� ����������.");
                }
            }
        }

        private void InsertPatients()
        {
            var patients = new (string �������, string ���, string ��������, string �����, DateTime ������������, string ���, int �������)[]
            {
                ("������", "����", "��������", "����� 1, ��� 1", new DateTime(1980, 1, 1), "�������", 1),
                ("������", "����", "��������", "����� 2, ��� 2", new DateTime(1990, 2, 2), "�������", 1),
                ("��������", "����", "���������", "����� 3, ��� 3", new DateTime(1985, 3, 3), "�������", 2)
            };
            foreach (var patient in patients)
            {
                string checkQuery = $"SELECT COUNT(*) FROM �������� WHERE ������� = N'{patient.�������}' AND ��� = N'{patient.���}' AND �������� = N'{patient.��������}'";
                int count = 0;
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(checkQuery, connection))
                    {
                        count = (int)command.ExecuteScalar();
                    }
                }
                if (count == 0)
                {
                    string insertQuery = $"INSERT INTO �������� (�������, ���, ��������, �����, ������������, ���, �������) VALUES (N'{patient.�������}', N'{patient.���}', N'{patient.��������}', N'{patient.�����}', '{patient.������������:yyyy-MM-dd}', N'{patient.���}', {patient.�������})";
                    ExecuteNonQuery(insertQuery);
                    Debug.WriteLine($"�������� �������: {patient.�������} {patient.���} {patient.��������}");
                }
                else
                {
                    Debug.WriteLine($"������� '{patient.�������} {patient.���} {patient.��������}' ��� ����������.");
                }
            }
        }

        private void InsertDoctors()
        {
            var patients = new (string ���, int �������, string �������������, int �������)[]
            {
                ("������ ���� ��������", 1, "��������", 1),
                ("������ ���� ��������", 2, "������", 2),
                ("�������� ���� ���������", 3, "���������", 3),
            };
            foreach (var patient in patients)
            {
                string checkQuery = $"SELECT COUNT(*) FROM ����� WHERE ��� = N'{patient.���}' AND ������� = '{patient.�������}' AND ������������� = N'{patient.�������������}'";
                int count = 0;
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(checkQuery, connection))
                    {
                        count = (int)command.ExecuteScalar();
                    }
                }
                if (count == 0)
                {
                    string insertQuery = $"INSERT INTO ����� (���, �������, �������������, �������) VALUES (N'{patient.���}', '{patient.�������}', N'{patient.�������������}', '{patient.�������}')";
                    ExecuteNonQuery(insertQuery);
                    Debug.WriteLine($"�������� ����: {patient.���} {patient.�������} {patient.�������������}");
                }
                else
                {
                    Debug.WriteLine($"���� '{patient.���} {patient.�������} {patient.�������������}' ��� ����������.");
                }
            }
        }

        #endregion

        #region DB Execute's

        private bool ExecuteNonQuery(string query)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        int affectedRows = command.ExecuteNonQuery();
                        return affectedRows > 0; // ���������� true, ���� ��������� ���� �� ���� ������
                    }
                }
            }
            catch (SqlException ex)
            {
                // �������� ������ ��� ������������ �� �� ������ ����������
                Debug.WriteLine($"������ ���������� �������: {ex.Message}");
                return false; // ���������� false � ������ ������
            }
        }

        private async Task ExecuteNonQueryAsync(string query)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        #endregion

        #region ������ Patient

        public bool AddPatient(PatientModel patient)
        {
            string insertQuery = $"INSERT INTO �������� (�������, ���, ��������, �����, ������������, ���, �������) VALUES (N'{patient.LastName}', N'{patient.FirstName}', N'{patient.Patronymic}', N'{patient.Address}', '{patient.DateBirthDay:yyyy-MM-dd}', N'{patient.Sex}', {patient.Region})";
            return ExecuteNonQuery(insertQuery);
        }

        public bool EditPatient(PatientModel patient)
        {
            // false - �� ������� �������� ��� ������� �� ������
            // true - ��� ������
            string selectQuery = "SELECT * FROM �������� WHERE Id = @Id";
            string updateQuery = @"
                UPDATE �������� 
                SET 
                    FirstName = @FirstName,
                    LastName = @LastName,
                    Patronymic = @Patronymic,
                    Address = @Address,
                    DateBirthDay = @DateBirthDay,
                    Sex = @Sex,
                    Region = @Region
                WHERE Id = @Id";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand selectCommand = new SqlCommand(selectQuery, connection))
                {
                    selectCommand.Parameters.AddWithValue("@Id", patient.Id);
                    using (SqlDataReader reader = selectCommand.ExecuteReader())
                    {
                        if (!reader.Read())
                        {
                            return false;
                        }
                    }
                }
                using (SqlCommand updateCommand = new SqlCommand(updateQuery, connection))
                {
                    updateCommand.Parameters.AddWithValue("@FirstName", patient.FirstName);
                    updateCommand.Parameters.AddWithValue("@LastName", patient.LastName);
                    updateCommand.Parameters.AddWithValue("@Patronymic", patient.Patronymic);
                    updateCommand.Parameters.AddWithValue("@Address", patient.Address);
                    updateCommand.Parameters.AddWithValue("@DateBirthDay", patient.DateBirthDay);
                    updateCommand.Parameters.AddWithValue("@Sex", patient.Sex);
                    updateCommand.Parameters.AddWithValue("@Region", patient.Region);
                    int affectedRows = updateCommand.ExecuteNonQuery();
                    return affectedRows > 0; // ���������� true, ���� ���������� ������ �������
                }
            }
        }

        public bool DeletePatient(int patientID)
        {
            string removeQuery = $"DELETE FROM �������� WHERE Id = '{patientID}';";
            return ExecuteNonQuery(removeQuery);
        }

        public async Task<PatientModel> GetPatientByID(int patientID)
        {
            // -������ ��� �������������� ������ ��������� ������ ������ (id) ��������� ������� �� ������ ������,
            PatientModel patient = null;
            string query = $"SELECT * FROM �������� WHERE Id = '{patientID}'";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    if (reader.HasRows) // ���� ���� ������
                    {
                        patient = new PatientModel();
                        while (await reader.ReadAsync()) // ��������� ��������� ������
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

        public async Task<List<PatientModel>> GetPatientsByPageAndSory(int page, string sort)
        {
            // �������� ������ ������������ ��������, ���������� ��������� �� �������, �������� ������� � �� ������ �������
            // ��������� ����������
            // TODO �������� ����� ��� ��������� ��������� ��������

            var sortBy = "Id"; // TODO ���� ������� ENUM �� ��� ������� ������ �������
            var tableName = "��������";
            var countOnPage = "10";

            List<PatientModel> patientsList = new List<PatientModel>();
            string queryGetByPageAndSort =
                @$"WITH SOURCE AS(
                 SELECT ROW_NUMBER() OVER(ORDER BY {sortBy}) AS RowNumber, *
                 FROM {tableName}
                 )
                 SELECT * FROM SOURCE
                 WHERE RowNumber > ({page} * {countOnPage}) - {countOnPage}
                   AND RowNumber <= {page} * {countOnPage}";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryGetByPageAndSort, connection);
                connection.Open();
                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    if (reader.HasRows) // ���� ���� ������
                    {
                        while (await reader.ReadAsync()) // ��������� ��������� ������
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
                            });
                        }
                    }
                }
            }
            return patientsList;
        }
        
        #endregion
    }
}
