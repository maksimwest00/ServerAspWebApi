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
        // Здесь реализация по взаимодействию с базой данных   
        // Пусть как ключ всех операций будет ID записи

        public const string connectionString =
            "Server=(localdb)\\MSSQLLocalDB;Database=mainDataBase;Trusted_Connection=True;";

        public async void ConnectToDB()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                Debug.WriteLine("Подключение открыто");
                Debug.WriteLine("Свойства подключения:");
                Debug.WriteLine($"\tСтрока подключения: {connection.ConnectionString}");
                Debug.WriteLine($"\tБаза данных: {connection.Database}");
                Debug.WriteLine($"\tСервер: {connection.DataSource}");
                Debug.WriteLine($"\tВерсия сервера: {connection.ServerVersion}");
                Debug.WriteLine($"\tСостояние: {connection.State}");
                Debug.WriteLine($"\tWorkstationld: {connection.WorkstationId}");
            }
            Debug.WriteLine("Подключение закрыто...");
            Debug.WriteLine("Программа завершила работу.");
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
                CREATE TABLE Участки (
                    Номер INT PRIMARY KEY IDENTITY(1,1)
                )";
            string createSpecializationsTableQuery = @"
                CREATE TABLE Специализации (
                    Название NVARCHAR(100) PRIMARY KEY
                )";
            string createCabinetesTableQuery = @"
                CREATE TABLE Кабинеты (
                    Номер INT PRIMARY KEY IDENTITY(1,1)
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
                    FOREIGN KEY (Участок) REFERENCES Участки(Номер)
                )";
            string createDoctorsTableQuery = @"
                CREATE TABLE Врачи (
                    Id INT PRIMARY KEY IDENTITY(1,1),
                    ФИО NVARCHAR(100),
                    Кабинет INT,
                    Специализация NVARCHAR(100),
                    Участок INT,
                    FOREIGN KEY (Кабинет) REFERENCES Кабинеты(Номер),
                    FOREIGN KEY (Специализация) REFERENCES Специализации(Название),
                    FOREIGN KEY (Участок) REFERENCES Участки(Номер)
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
                string checkQuery = $"SELECT COUNT(*) FROM Участки WHERE Номер = '{region}'";
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
                    string insertQuery = $"INSERT INTO Участки (Номер) VALUES ('{region}')";
                    ExecuteNonQuery(insertQuery);
                    Debug.WriteLine($"Вставлен: {region}");
                }
                else
                {
                    Debug.WriteLine($"Запись '{region}' уже существует.");
                }
            }
        }

        private void InsertSpecializations()
        {
            string[] specializations = { "Терапевт", "Хирург", "Кардиолог" };
            foreach (var specialization in specializations)
            {
                string checkQuery = $"SELECT COUNT(*) FROM Специализации WHERE Название = '{specialization}'";
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
                    string insertQuery = $"INSERT INTO Специализации (Название) VALUES ('{specialization}')";
                    ExecuteNonQuery(insertQuery);
                    Debug.WriteLine($"Вставлена специализация: {specialization}");
                }
                else
                {
                    Debug.WriteLine($"Специализация '{specialization}' уже существует.");
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
                    string insertQuery = $"INSERT INTO Кабинеты (Номер) VALUES ('{cabinet}')";
                    ExecuteNonQuery(insertQuery);
                    Debug.WriteLine($"Вставлен кабинет: {cabinet}");
                }
                else
                {
                    Debug.WriteLine($"Кабинет '{cabinet}' уже существует.");
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
                string checkQuery = $"SELECT COUNT(*) FROM Пациенты WHERE Фамилия = '{patient.Фамилия}' AND Имя = '{patient.Имя}' AND Отчество = '{patient.Отчество}'";
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
                    string insertQuery = $"INSERT INTO Пациенты (Фамилия, Имя, Отчество, Адрес, ДатаРождения, Пол, Участок) VALUES ('{patient.Фамилия}', '{patient.Имя}', '{patient.Отчество}', '{patient.Адрес}', '{patient.ДатаРождения:yyyy-MM-dd}', '{patient.Пол}', {patient.Участок})";
                    ExecuteNonQuery(insertQuery);
                    Debug.WriteLine($"Вставлен пациент: {patient.Фамилия} {patient.Имя} {patient.Отчество}");
                }
                else
                {
                    Debug.WriteLine($"Пациент '{patient.Фамилия} {patient.Имя} {patient.Отчество}' уже существует.");
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
                string checkQuery = $"SELECT COUNT(*) FROM Врачи WHERE ФИО = '{patient.ФИО}' AND Кабинет = '{patient.Кабинет}' AND Специализация = '{patient.Специализация}'";
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
                    string insertQuery = $"INSERT INTO Врачи (ФИО, Кабинет, Специализация, Участок) VALUES ('{patient.ФИО}', '{patient.Кабинет}', '{patient.Специализация}', '{patient.Участок}')";
                    ExecuteNonQuery(insertQuery);
                    Debug.WriteLine($"Вставлен врач: {patient.ФИО} {patient.Кабинет} {patient.Специализация}");
                }
                else
                {
                    Debug.WriteLine($"Врач '{patient.ФИО} {patient.Кабинет} {patient.Специализация}' уже существует.");
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

        #region Методы Patient

        /// <summary>
        /// Добавление записи
        /// </summary>
        public void AddPatient(PatientModel patient)
        {
            string insertQuery = $"INSERT INTO Пациенты (Фамилия, Имя, Отчество, Адрес, ДатаРождения, Пол, Участок) VALUES ('{patient.LastName}', '{patient.FirstName}', '{patient.Patronymic}', '{patient.Address}', '{patient.DateBirthDay:yyyy-MM-dd}', '{patient.Sex}', {patient.Region})";
            ExecuteNonQuery(insertQuery);
        }

        /// <summary>
        /// Редактирование записи
        /// </summary>
        public void EditPatient(PatientModel patient)
        {
            // TODO
            // Вернуть ID этой записи в Таблице Пациентов
        }

        /// <summary>
        /// Редактирование записи
        /// </summary>
        public void DeletePatient(int patientID)
        {
            string removeQuery = $"DELETE FROM Пациенты WHERE OrderID IS '{patientID}';";
            ExecuteNonQuery(removeQuery);
        }

        /// <summary>
        /// Получение записи по ид для редактирования
        /// </summary>
        public async Task<PatientModel> GetPatientByID(int patientID)
        {
            string query = $"SELECT * FROM Пациенты WHERE Id = '{patientID}'";

            PatientModel patient = null;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    if (reader.HasRows) // если есть данные
                    {
                        patient = new PatientModel();
                        // TODO заполнить patient
                        // выводим названия столбцов
                        string columnName1 = reader.GetName(0);
                        string columnName2 = reader.GetName(1);
                        string columnName3 = reader.GetName(2);
                        Debug.WriteLine($"{columnName1}\t{columnName3}\t{columnName2}");
                        while (await reader.ReadAsync()) // построчно считываем данные
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
        /// Получения списка записей для формы списка с поддержкой сортировки и постраничного возврата данных
        /// (должна быть возможность через параметры указать по какому полю список должен быть отсортирован
        /// и так же через параметры указать какой фрагмент списка (страницу) необходимо вернуть)
        /// </summary>
        public async Task<List<PatientModel>> GetListPatient(int page, string sort)
        {
            // TODO Добавить OrderBy по полю в PatientModel
            // TODO Указать какой фрагмент списка (страницу) необходимо вернуть
            string query = "SELECT * FROM Пациенты";
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
