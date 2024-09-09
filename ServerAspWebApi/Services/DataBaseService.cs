using Microsoft.Data.SqlClient;
using ServerAspWebApi.Model;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace ServerAspWebApi.Services
{
    public class DataBaseService
    {
        // Здесь реализация по взаимодействию с базой данных   
        // Пусть как ключ всех операций будет ID записи

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
                CREATE TABLE Участки (
                    Номер INT PRIMARY KEY
            )";
            string createSpecializationsTableQuery = @"
                CREATE TABLE Специализации (
                    Название NVARCHAR(100) PRIMARY KEY
                )";
            string createCabinetesTableQuery = @"
                CREATE TABLE Кабинеты (
                    Номер INT PRIMARY KEY
                )";
            string createPatientsTableQuery = @"
                CREATE TABLE Пациенты (
                    Id INT PRIMARY KEY IDENTITY(1,1),
                    Фамилия NVARCHAR(100),
                    Имя NVARCHAR(100),
                    Отчество NVARCHAR(100),
                    Адрес NVARCHAR(255),
                    ДатаРождения DATE,
                    Пол NVARCHAR(10),
                    Участок INT,
                    FOREIGN KEY (Участок) REFERENCES Участки(Номер) ON DELETE CASCADE ON UPDATE CASCADE
                )";
            string createDoctorsTableQuery = @"
                CREATE TABLE Врачи (
                    Id INT PRIMARY KEY IDENTITY(1,1),
                    ФИО NVARCHAR(100),
                    Кабинет INT,
                    Специализация NVARCHAR(100),
                    Участок INT,
                    FOREIGN KEY (Кабинет) REFERENCES Кабинеты(Номер) ON DELETE CASCADE ON UPDATE CASCADE,
                    FOREIGN KEY (Специализация) REFERENCES Специализации(Название) ON DELETE CASCADE ON UPDATE CASCADE,
                    FOREIGN KEY (Участок) REFERENCES Участки(Номер) ON DELETE CASCADE ON UPDATE CASCADE
                )";
            if (!this.TableExists("Участки"))
            {
                this.ExecuteNonQuery(createRegionsTableQuery);
            }
            if (!this.TableExists("Специализации"))
            {
                this.ExecuteNonQuery(createSpecializationsTableQuery);
            }
            if (!this.TableExists("Кабинеты"))
            {
                this.ExecuteNonQuery(createCabinetesTableQuery);
            }
            if (!this.TableExists("Пациенты"))
            {
                this.ExecuteNonQuery(createPatientsTableQuery);
            }
            if (!this.TableExists("Врачи"))
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
                string checkQuery = $"SELECT COUNT(*) FROM Участки WHERE Номер = '{region}'";
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
                    string insertQuery = $"INSERT INTO Участки (Номер) VALUES ('{region}')";
                    ExecuteNonQuery(insertQuery);
                }
            }
        }

        private void InsertSpecializations()
        {
            string[] specializations = { "Терапевт", "Хирург", "Кардиолог" };
            foreach (var specialization in specializations)
            {
                string checkQuery = $"SELECT COUNT(*) FROM Специализации WHERE Название = N'{specialization}'";
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
                    string insertQuery = $"INSERT INTO Специализации (Название) VALUES (N'{specialization}')";
                    ExecuteNonQuery(insertQuery);
                }
            }
        }

        private void InsertCabinets()
        {
            string[] cabinets = { "1", "2", "3", "4", "5" };
            foreach (var cabinet in cabinets)
            {
                string checkQuery = $"SELECT COUNT(*) FROM Кабинеты WHERE Номер = '{cabinet}'";
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
                    string insertQuery = $"INSERT INTO Кабинеты (Номер) VALUES ('{cabinet}')";
                    ExecuteNonQuery(insertQuery);
                }
            }
        }

        private void InsertPatients()
        {
            var patients = new (string Фамилия, string Имя, string Отчество, string Адрес, DateTime ДатаРождения, string Пол, int Участок)[]
            {
                ("Иванов", "Иван", "Иванович", "Улица 1, дом 1", new DateTime(1980, 1, 1), "Мужской", 1),
                ("Петров", "Петр", "Петрович", "Улица 2, дом 2", new DateTime(1990, 2, 2), "Мужской", 1),
                ("Сидорова", "Анна", "Сидоровна", "Улица 3, дом 3", new DateTime(1985, 3, 3), "Женский", 2)
            };
            foreach (var patient in patients)
            {
                string checkQuery = $"SELECT COUNT(*) FROM Пациенты WHERE Фамилия = N'{patient.Фамилия}' AND Имя = N'{patient.Имя}' AND Отчество = N'{patient.Отчество}'";
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
                    string insertQuery = $"INSERT INTO Пациенты (Фамилия, Имя, Отчество, Адрес, ДатаРождения, Пол, Участок) VALUES (N'{patient.Фамилия}', N'{patient.Имя}', N'{patient.Отчество}', N'{patient.Адрес}', '{patient.ДатаРождения:yyyy-MM-dd}', N'{patient.Пол}', {patient.Участок})";
                    ExecuteNonQuery(insertQuery);
                }
            }
        }

        private void InsertDoctors()
        {
            var patients = new (string ФИО, int Кабинет, string Специализация, int Участок)[]
            {
                ("Иванов Иван Иванович", 1, "Терапевт", 1),
                ("Петров Петр Петрович", 2, "Хирург", 2),
                ("Сидорова Анна Сидоровна", 3, "Кардиолог", 3),
            };
            foreach (var patient in patients)
            {
                string checkQuery = $"SELECT COUNT(*) FROM Врачи WHERE ФИО = N'{patient.ФИО}' AND Кабинет = '{patient.Кабинет}' AND Специализация = N'{patient.Специализация}'";
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
                    string insertQuery = $"INSERT INTO Врачи (ФИО, Кабинет, Специализация, Участок) VALUES (N'{patient.ФИО}', '{patient.Кабинет}', N'{patient.Специализация}', '{patient.Участок}')";
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
                        return affectedRows > 0; // Возвращаем true, если затронуто хотя бы одна строка
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

        #region Методы Patient Table

        public bool AddPatient(PatientModel patient)
        {
            string insertQuery = $"INSERT INTO Пациенты (Фамилия, Имя, Отчество, Адрес, ДатаРождения, Пол, Участок) VALUES (N'{patient.LastName}', N'{patient.FirstName}', N'{patient.Patronymic}', N'{patient.Address}', '{patient.DateBirthDay:yyyy-MM-dd}', N'{patient.Sex}', {patient.Region})";
            return ExecuteNonQuery(insertQuery);
        }

        #endregion
    }
}
