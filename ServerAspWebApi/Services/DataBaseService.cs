using Microsoft.Data.SqlClient;
using System;
using System.Data;
using System.Diagnostics;

namespace ServerAspWebApi.Services
{
    public class DataBaseService
    {
        // Здесь реализация по взаимодействию с базой данных   

        // Пусть как ключ всех операций будет ID записи
        public DataBaseService()
        {
            ConnectToDB();
        }

        public async void ConnectToDB()
        {
            string connectionString =
                "Server=(localdb)\\MSSQLLocalDB;Database=myDataBase;Trusted_Connection=True;";
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
    }
}
