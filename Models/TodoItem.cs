namespace ToDoListPlus.Models
{
    public class TodoItem
    {

        public int Id { get; set; }
        public string Tarefa { get; set; }

        public TodoItem()
        {

        }


        public(bool, string) Validar()
        {
            if(Id<=0)
            {
                return (false, "ID inválido!");
            }
            else 
            if (string.IsNullOrEmpty(Tarefa))
            {
                return (false, "Digite o nome da tarefa!");
            }
            return (true, "");
        }

    }
}
