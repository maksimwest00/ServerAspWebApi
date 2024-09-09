using Microsoft.AspNetCore.Mvc;
using ServerAspWebApi.Model;
using System.Threading.Tasks;

namespace ServerAspWebApi.Controllers
{
    public abstract class BaseMedicController : ControllerBase
    {
        ////-Добавление записи
        //[HttpPost("add")]
        //public abstract Task<IActionResult> AddRecord([FromBody] object record);

        ////-Редактирование записи
        //[HttpPost("edit")]
        //public abstract Task<IActionResult> EditRecord([FromBody] object model);

        //-Удаление записи
        [HttpPost("delete")]
        public abstract Task<IActionResult> DeleteRecord([FromBody] int id);

        //-Получение записи по ид для редактирования
        [HttpGet("id={id}")]
        public abstract Task<IActionResult> GetRecordByID(int id);

        //-Получения списка записей для формы списка с поддержкой сортировки и постраничного возврата данных
        // (должна быть возможность через параметры указать по какому полю список должен быть отсортирован
        // и так же через параметры указать какой фрагмент списка (страницу) необходимо вернуть)

        [HttpGet]
        public abstract Task<IActionResult> GetListRecordBySortAndPage([FromQuery] int page, [FromQuery] string sort);
    }
}
