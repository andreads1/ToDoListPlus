using Microsoft.AspNetCore.Mvc;

namespace ToDoListPlus.Controllers
{
    public class ToDoListPlusController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Salvar([FromBody] System.Text.Json.JsonElement dados)
        {
            bool sucesso = false;
            string msg = "";
            int id=0;
            string tarefa="";

            try
            {
                id = Convert.ToInt32(dados.GetProperty("id").ToString());
                tarefa = dados.GetProperty("tarefa").ToString();
            }
            catch 
            {
                msg = "Campos possuem valores inválidos!!";
            }

            if(msg=="")
            {
                Models.TodoItem item = new Models.TodoItem();
                item.Id = id;
                item.Tarefa = tarefa;

                (sucesso, msg) = item.Validar();

                if(sucesso)
                {
                    Services.TodoListPlusService service = new Services.TodoListPlusService(); //ou só = new()
                    sucesso = service.Gravar(item);

                    if (!sucesso)
                        msg = "Houve algum erro!";
                    else
                        msg = "Tarefa salva com sucesso";
                }

            }

            var obj = new
            {
                sucesso = sucesso,
                msg = msg
            };

            return Json(obj);

        }

        public IActionResult ObterTodos()
        {
            Services.TodoListPlusService service = new();
            return Json(service.ObterTodos());
        }

        public IActionResult Excluir(int id)
        {
            bool sucesso = false;
            string msg = "";

            if (id <= 0)
            {
                msg = "Item para exclusão não selecionado!";
            }
            else
            {
                Services.TodoListPlusService service = new();
                sucesso = service.Excluir(id);

                if (sucesso)
                {
                    msg = "Item excluído com sucesso!";
                }
                else
                {
                    msg = "Não foi possível realizar a exclusão!";
                }
            }

            var obj = new
            {
                sucesso = sucesso,
                msg = msg
            };

            return Json(obj);
        }

        public IActionResult ObterPorId(int id)
        {
            Services.TodoListPlusService service = new();
            return Json(service.ObterPorId(id));
        }

    }
}
