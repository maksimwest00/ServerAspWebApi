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
        private readonly DataBaseService _dataBaseService;

        public DoctorController(ILogger<DoctorController> logger, DataBaseService dataBaseService)
        {
            _logger = logger;
            _dataBaseService = dataBaseService;
        }

        public override async Task<IActionResult> AddRecord(PatientModel patient)
        {
            // Просто добавить в базу данных новую запись с новым ID
            return Ok();
        }

        public override async Task<IActionResult> DeleteRecord(int id)
        {
            // Просто удалить из базы данных запись по ID
            return Ok();
        }

        public override async Task<IActionResult> EditRecord<PatientModel>(PatientModel model)
        {
            // Просто по ID найти в базе данных запись и обновить ее
            return Ok();
        }

        public override async Task<IActionResult> GetRecordByID(int id)
        {
            // Вернуть объект который содержит только ссылки (id) связанных записей из других таблицы
            return Ok();
        }

        public override async Task<IActionResult> GetListRecordBySortAndPage(int page, string sort)
        {
            // объект списка не должен содержать внешних ссылок, вместо них необходимо возвращать значение из связанной таблицы  (т.е. не id специализации врача, а название).

            // https://localhost:5001/patient?page=1&sort=mysort
            var a = page;
            var b = sort;
            var c = 1;
            return Ok();
        }
    }
}
