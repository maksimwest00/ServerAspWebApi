using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ServerAspWebApi.Model;
using ServerAspWebApi.Services;
using System.Threading.Tasks;

namespace ServerAspWebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PatientController : BaseMedicController
    {
        private readonly ILogger<PatientController> _logger;
        private readonly PatientTableEnviroment _patientTableEnviroment;

        public PatientController(ILogger<PatientController> logger, PatientTableEnviroment patientTableEnviroment)
        {
            _logger = logger;
            _patientTableEnviroment = patientTableEnviroment;
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddRecord([FromBody] PatientModel patient)
        {
            // Просто добавить в базу данных новую запись с новым ID
            // sample
            // POST запрос на https://localhost:5001/patient/add
            // в body raw application/json просто json PatientModel
            // sample json
            //{
            //    "FirstName": "FirstNameTest",
	        //    "LastName": "LastNameTest",
	        //    "Patronymic": "PatronymicTest",
	        //    "Address": "AddressTest",
	        //    "Sex": "SexTest",
	        //    "Region": 1
            // }
            bool result = await _patientTableEnviroment.Add(patient);
            if (result)
            {
                return Ok(new { status = $"Добавлена новая запись о пацинте" });
            }
            return Ok(new { status = "Запись не добавлена, произошла ошибка" });
        }

        public override async Task<IActionResult> DeleteRecord(int id)
        {
            // sample
            // POST запрос на https://localhost:5001/patient/delete
            // в body raw application/json просто id без json
            bool result = await _patientTableEnviroment.Delete(id);
            if (result)
            {
                return Ok(new { status = $"Удалена запись о пацинте {id}" });
            }
            return Ok(new { status = "Запись не удалена, произошла ошибка" });
        }

        [HttpPost("edit")]
        public async Task<IActionResult> EditRecord([FromBody] PatientModel patient)
        {
            // sample https://localhost:5001/patient/edit
            // в body raw application/ json просто json PatientModel
            // sample json
            //{
            //    "Id": 3,
            //    "FirstName": "FirstNameTest",
            //    "LastName": "LastNameTest",
            //    "Patronymic": "PatronymicTest",
            //    "Address": "AddressTest",
            //    "Sex": "SexTest",
            //    "Region": 1
            //}
            bool result = await _patientTableEnviroment.Edit(patient as PatientModel);
            if (result)
            {
                return Ok(new { status = $"Отредактирована запись о пацинте {(patient as PatientModel).Id}" });
            }
            return Ok(new { status = "Запись не отредактирована, произошла ошибка" });
        }

        public override async Task<IActionResult> GetRecordByID(int id)
        {
            // sample https://localhost:5001/patient/id=2
            return Ok(await _patientTableEnviroment.GetByID(id));
        }

        public override async Task<IActionResult> GetListRecordBySortAndPage(int page, string sort)
        {
            // sample https://localhost:5001/patient?page=1&sort=Id
            return Ok(await _patientTableEnviroment.GetListByPageAndSort(page, sort));
        }
    }
}
