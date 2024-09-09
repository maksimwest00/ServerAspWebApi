using ServerAspWebApi.Model;
using ServerAspWebApi.Services;
using System;
using System.Threading.Tasks;

namespace ConsoleAppSQL
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("ConsoleAppSQL!");

            var dbService = new DataBaseService();
            var patientEnv = new PatientTableEnviroment(dbService);
            // EDIT - РАБОТАЕТ
            // DELETE - РАБОТАЕТ
            // ADD - РАБОТАЕТ
            // GETBYID - РАБОТАЕТ
            // GET BY PAGE AND SORT - РАБОТАЕТ

            //Task.Run(async () =>
            //{
            //    string[] columnsNames = await patientEnv.GetColumnNameInTable("Пациенты");
            //    var a = 1;
            //});


            (patientEnv.GetListByPageAndSort(1, "Id")).ContinueWith((patient) =>
            {
                var fasjfjas = patient.Result;
                var asdkaskd = 1;
            });

            var b = Console.ReadLine();
        }
    }
}
