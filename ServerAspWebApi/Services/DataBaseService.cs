using Microsoft.Data.SqlClient;
using System;
using System.Data;
using System.Diagnostics;

namespace ServerAspWebApi.Services
{
    public class DataBaseService
    {
        // ����� ���������� �� �������������� � ����� ������   

        // ����� ��� ���� ���� �������� ����� ID ������
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
    }
}
