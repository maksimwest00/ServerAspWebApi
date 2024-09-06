using ServerAspWebApi.Model;
using ServerAspWebApi.Services;
using System;

namespace ConsoleAppSQL
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("ConsoleAppSQL!");

            var dbService = new DataBaseService();
            
            var a = dbService.TEST_GetByPageQuery("1", "10", "Пациенты", "Имя");
            var b = Console.ReadLine();
        }
    }
}
