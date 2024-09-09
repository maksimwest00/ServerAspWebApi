using Microsoft.Data.SqlClient;
using ServerAspWebApi.Model;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace ServerAspWebApi.Services
{
    public class DataBaseService
    {
        // ����� ���������� �� �������������� � ����� ������   
        // ����� ��� ���� ���� �������� ����� ID ������

        public const string ConnectionString = "Server=(localdb)\\MSSQLLocalDB;Database=mainDataBase;Trusted_Connection=True;";

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
                    FOREIGN KEY (�������) REFERENCES �������(�����) ON DELETE CASCADE ON UPDATE CASCADE
                )";
            string createDoctorsTableQuery = @"
                CREATE TABLE ����� (
                    Id INT PRIMARY KEY IDENTITY(1,1),
                    ��� NVARCHAR(100),
                    ������� INT,
                    ������������� NVARCHAR(100),
                    ������� INT,
                    FOREIGN KEY (�������) REFERENCES ��������(�����) ON DELETE CASCADE ON UPDATE CASCADE,
                    FOREIGN KEY (�������������) REFERENCES �������������(��������) ON DELETE CASCADE ON UPDATE CASCADE,
                    FOREIGN KEY (�������) REFERENCES �������(�����) ON DELETE CASCADE ON UPDATE CASCADE
                )";
            if (!this.TableExists("�������"))
            {
                this.ExecuteNonQuery(createRegionsTableQuery);
            }
            if (!this.TableExists("�������������"))
            {
                this.ExecuteNonQuery(createSpecializationsTableQuery);
            }
            if (!this.TableExists("��������"))
            {
                this.ExecuteNonQuery(createCabinetesTableQuery);
            }
            if (!this.TableExists("��������"))
            {
                this.ExecuteNonQuery(createPatientsTableQuery);
            }
            if (!this.TableExists("�����"))
            {
                this.ExecuteNonQuery(createDoctorsTableQuery);
            }
        }

        private bool TableExists(string tableName)
        {
            string query = $"SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = N'{tableName}'";
            using (SqlConnection connection = new SqlConnection(ConnectionString))
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
                using (SqlConnection connection = new SqlConnection(ConnectionString))
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
                using (SqlConnection connection = new SqlConnection(ConnectionString))
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
                using (SqlConnection connection = new SqlConnection(ConnectionString))
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
                using (SqlConnection connection = new SqlConnection(ConnectionString))
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
                using (SqlConnection connection = new SqlConnection(ConnectionString))
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
                }
            }
        }

        #endregion

        #region DB Execute's

        public bool ExecuteNonQuery(string query)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        int affectedRows = command.ExecuteNonQuery();
                        return affectedRows > 0; // ���������� true, ���� ��������� ���� �� ���� ������
                    }
                }
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> ExecuteNonQueryAsync(string query)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    await connection.OpenAsync();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        int affectedRows = await command.ExecuteNonQueryAsync();
                        return affectedRows > 0;
                    }
                }
            }
            catch
            {
                return false;
            }
        }

        #endregion

        #region ������ Patient Table

        public bool AddPatient(PatientModel patient)
        {
            string insertQuery = $"INSERT INTO �������� (�������, ���, ��������, �����, ������������, ���, �������) VALUES (N'{patient.LastName}', N'{patient.FirstName}', N'{patient.Patronymic}', N'{patient.Address}', '{patient.DateBirthDay:yyyy-MM-dd}', N'{patient.Sex}', {patient.Region})";
            return ExecuteNonQuery(insertQuery);
        }

        #endregion
    }
}
