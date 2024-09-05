using Microsoft.Data.SqlClient;
using ServerAspWebApi.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace ServerAspWebApi.Services
{
    public class DataBaseService
    {
        // ����� ���������� �� �������������� � ����� ������   
        // ����� ��� ���� ���� �������� ����� ID ������

        public const string connectionString =
            "Server=(localdb)\\MSSQLLocalDB;Database=mainDataBase;Trusted_Connection=True;";

        public async void ConnectToDB()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
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

        public void TEST_CTOR()
        {
            CreateTables();
            InsertRegions();
            InsertSpecializations();
            InsertCabinets();
            InsertPatients();
            InsertDoctors();
        }

        private void CreateTables()
        {
            string createRegionsTableQuery = @"
                CREATE TABLE ������� (
                    ����� INT PRIMARY KEY IDENTITY(1,1)
                )";
            string createSpecializationsTableQuery = @"
                CREATE TABLE ������������� (
                    �������� NVARCHAR(100) PRIMARY KEY
                )";
            string createCabinetesTableQuery = @"
                CREATE TABLE �������� (
                    ����� INT PRIMARY KEY IDENTITY(1,1)
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
            string query = $"SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = '{tableName}'";

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
                string checkQuery = $"SELECT COUNT(*) FROM ������������� WHERE �������� = '{specialization}'";
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
                    string insertQuery = $"INSERT INTO ������������� (��������) VALUES ('{specialization}')";
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
                string checkQuery = $"SELECT COUNT(*) FROM �������� WHERE ������� = '{patient.�������}' AND ��� = '{patient.���}' AND �������� = '{patient.��������}'";
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
                    string insertQuery = $"INSERT INTO �������� (�������, ���, ��������, �����, ������������, ���, �������) VALUES ('{patient.�������}', '{patient.���}', '{patient.��������}', '{patient.�����}', '{patient.������������:yyyy-MM-dd}', '{patient.���}', {patient.�������})";
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
                string checkQuery = $"SELECT COUNT(*) FROM ����� WHERE ��� = '{patient.���}' AND ������� = '{patient.�������}' AND ������������� = '{patient.�������������}'";
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
                    string insertQuery = $"INSERT INTO ����� (���, �������, �������������, �������) VALUES ('{patient.���}', '{patient.�������}', '{patient.�������������}', '{patient.�������}')";
                    ExecuteNonQuery(insertQuery);
                    Debug.WriteLine($"�������� ����: {patient.���} {patient.�������} {patient.�������������}");
                }
                else
                {
                    Debug.WriteLine($"���� '{patient.���} {patient.�������} {patient.�������������}' ��� ����������.");
                }
            }
        }

        private void ExecuteNonQuery(string query)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        #region ������ Patient

        /// <summary>
        /// ���������� ������
        /// </summary>
        public void AddPatient(PatientModel patient)
        {
            string insertQuery = $"INSERT INTO �������� (�������, ���, ��������, �����, ������������, ���, �������) VALUES ('{patient.LastName}', '{patient.FirstName}', '{patient.Patronymic}', '{patient.Address}', '{patient.DateBirthDay:yyyy-MM-dd}', '{patient.Sex}', {patient.Region})";
            ExecuteNonQuery(insertQuery);
        }

        /// <summary>
        /// �������������� ������
        /// </summary>
        public void EditPatient(PatientModel patient)
        {
            // TODO
            // ������� ID ���� ������ � ������� ���������
        }

        /// <summary>
        /// �������������� ������
        /// </summary>
        public void DeletePatient(int patientID)
        {
            string removeQuery = $"DELETE FROM �������� WHERE OrderID IS '{patientID}';";
            ExecuteNonQuery(removeQuery);
        }

        /// <summary>
        /// ��������� ������ �� �� ��� ��������������
        /// </summary>
        public async Task<PatientModel> GetPatientByID(int patientID)
        {
            string query = $"SELECT * FROM �������� WHERE Id = '{patientID}'";

            PatientModel patient = null;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    if (reader.HasRows) // ���� ���� ������
                    {
                        patient = new PatientModel();
                        // TODO ��������� patient
                        // ������� �������� ��������
                        string columnName1 = reader.GetName(0);
                        string columnName2 = reader.GetName(1);
                        string columnName3 = reader.GetName(2);
                        Debug.WriteLine($"{columnName1}\t{columnName3}\t{columnName2}");
                        while (await reader.ReadAsync()) // ��������� ��������� ������
                        {
                            object id = reader.GetValue(0);
                            object name = reader.GetValue(2);
                            object age = reader.GetValue(1);
                            Debug.WriteLine($"{id} \t{name} \t{age}");
                        }
                    }
                }
            }
            return patient;
        }

        /// <summary>
        /// ��������� ������ ������� ��� ����� ������ � ���������� ���������� � ������������� �������� ������
        /// (������ ���� ����������� ����� ��������� ������� �� ������ ���� ������ ������ ���� ������������
        /// � ��� �� ����� ��������� ������� ����� �������� ������ (��������) ���������� �������)
        /// </summary>
        public async Task<List<PatientModel>> GetListPatient(int page, string sort)
        {
            // TODO �������� OrderBy �� ���� � PatientModel
            // TODO ������� ����� �������� ������ (��������) ���������� �������
            string query = "SELECT * FROM ��������";
            var ans = new List<string>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            ans.Add(reader.GetString(0));
                        }
                    }
                }
            }
            return new List<PatientModel>(); ;
        }

        #endregion

    }
}
