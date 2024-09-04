using Microsoft.AspNetCore.Mvc;
using ServerAspWebApi.Model;

namespace ServerAspWebApi.Controllers
{
    public abstract class BaseMedicController : ControllerBase
    {
        //-Добавление записи
        [HttpPost("add")]
        public abstract IActionResult AddRecord([FromBody] PatientModel patient);

        //-Редактирование записи
        [HttpPost("edit")]
        public abstract IActionResult EditRecord<T>([FromBody] T model);

        //-Удаление записи
        [HttpPost("delete")]
        public abstract IActionResult DeleteRecord([FromBody] int id);

        //-Получение записи по ид для редактирования
        [HttpGet("id={id}")]
        public abstract IActionResult GetRecordByID(int id);

        //-Получения списка записей для формы списка с поддержкой сортировки и постраничного возврата данных
        // (должна быть возможность через параметры указать по какому полю список должен быть отсортирован
        // и так же через параметры указать какой фрагмент списка (страницу) необходимо вернуть)

        [HttpGet]
        public abstract IActionResult GetListRecordBySortAndPage([FromQuery] int page, [FromQuery] string sort);
    }
}
