using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ServerAspWebApi.Model;
using ServerAspWebApi.Services;
using System.Threading.Tasks;

namespace ServerAspWebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DoctorController : BaseMedicController
    {
        private readonly ILogger<DoctorController> _logger;
        private readonly DoctorTableEnviroment _doctorTableEnviroment;

        public DoctorController(ILogger<DoctorController> logger, DoctorTableEnviroment doctorTableEnviroment)
        {
            _logger = logger;
            _doctorTableEnviroment = doctorTableEnviroment;
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddRecord([FromBody] DoctorModel doctor)
        {
            // Просто добавить в базу данных новую запись с новым ID
            // sample
            // POST запрос на https://localhost:5001/doctor/add
            // в body raw application/json просто json DoctorModel
            // sample json
            //{
            //    "FullName": "FullNameTEST",
            //    "Cabinet": 1,
            //    "Specialization": "Терапевт",
            //    "Region": 2
            // }
            bool result = await _doctorTableEnviroment.Add(doctor);
            if (result)
            {
                return Ok(new { status = $"Добавлена новая запись о враче" });
            }
            return Ok(new { status = "Запись не добавлена, произошла ошибка" });
        }

        public override async Task<IActionResult> DeleteRecord(int id)
        {
            // sample
            // POST запрос на https://localhost:5001/doctor/delete
            // в body raw application/json просто id без json
            bool result = await _doctorTableEnviroment.Delete(id);
            if (result)
            {
                return Ok(new { status = $"Удалена запись о враче {id}" });
            }
            return Ok(new { status = "Запись не удалена, произошла ошибка" }); ;
        }

        [HttpPost("edit")]
        public async Task<IActionResult> EditRecord([FromBody] DoctorModel doctor)
        {
            // sample https://localhost:5001/doctor/edit
            // в body raw application/ json просто json DoctorModel
            // sample json
            //{
            //    "Id": 1,
            //    "FullName": "FullNameTEST",
            //    "Cabinet": 1,
            //    "Specialization": "Терапевт",
            //    "Region": 2
            // }
            bool result = await _doctorTableEnviroment.Edit(doctor);
            if (result)
            {
                return Ok(new { status = $"Отредактирована запись о враче {doctor.Id}" });
            }
            return Ok(new { status = "Запись не отредактирована, произошла ошибка" });
        }

        public override async Task<IActionResult> GetRecordByID(int id)
        {
            // sample https://localhost:5001/doctor/id=2
            DoctorModel findedRecord = await _doctorTableEnviroment.GetByID(id);
            if (findedRecord != null)
            {
                return Ok(findedRecord);
            }
            return Ok(new { status = "Запись не найдена" });
        }

        public override async Task<IActionResult> GetListRecordBySortAndPage(int page, string sort)
        {
            // sample https://localhost:5001/doctor?page=1&sort=Id
            var doctorsList = await _doctorTableEnviroment.GetListByPageAndSort(page, sort);
            if (doctorsList != null)
            {
                return Ok(doctorsList);
            }
            return Ok(new { status = "Записи не найдена" });
        }
    }
}
