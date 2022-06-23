namespace ToDoListPlus.Services
{
    public class TodoListPlusService
    {
        public bool Gravar(Models.TodoItem item)
        {
            bool sucesso = false;

            string strbd = Environment.GetEnvironmentVariable("BD_STRING_CONEXAO");
            Npgsql.NpgsqlConnection conexao = new
                    Npgsql.NpgsqlConnection(strbd);


            Npgsql.NpgsqlCommand cmd = conexao.CreateCommand();
            conexao.Open();
            cmd.CommandText = @"select count(*)
                                from todolist
                                where id = @id";
            cmd.Parameters.AddWithValue("@id", item.Id);

            int jaExiste = Convert.ToInt32(cmd.ExecuteScalar());

            cmd.Parameters.Clear();

            if (jaExiste == 0)
            {
                cmd.CommandText = "insert into todolist (id, tarefa) values (@id, @tarefa)";
            }
            else
            {
                cmd.CommandText = @"update todolist
                                    set tarefa = @tarefa
                                    where id = @id";
            }

            cmd.Parameters.AddWithValue("@id", item.Id);
            cmd.Parameters.AddWithValue("@tarefa", item.Tarefa);

            cmd.ExecuteNonQuery();

            conexao.Close();
            sucesso = true;
            return sucesso;
        }

        public List<Models.TodoItem> ObterTodos()
        {
            var itens = new List<Models.TodoItem>();

            string strbd = Environment.GetEnvironmentVariable("BD_STRING_CONEXAO");
            Npgsql.NpgsqlConnection conexao = new
                    Npgsql.NpgsqlConnection(strbd);


            Npgsql.NpgsqlCommand cmd = conexao.CreateCommand();

            cmd.CommandText = @"select * from todolist";

            conexao.Open();

            var dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                var item = new Models.TodoItem();
                item.Id = Convert.ToInt32(dr["id"]);
                item.Tarefa = (dr["tarefa"].ToString());
                itens.Add(item);
            }

            conexao.Close();

            return itens;
        }

        public bool Excluir(int id)
        {
            bool sucesso = false;

            string strbd = Environment.GetEnvironmentVariable("BD_STRING_CONEXAO");
            Npgsql.NpgsqlConnection conexao = new
                    Npgsql.NpgsqlConnection(strbd);

            try
            {
                Npgsql.NpgsqlCommand cmd = conexao.CreateCommand();
                cmd.CommandText = @"delete from todolist where id = @id";
                cmd.Parameters.AddWithValue("@id", id);
                conexao.Open();
                cmd.ExecuteNonQuery();
                sucesso = true;
            }
            catch (Exception ex)
            {

            }
            finally
            {
                conexao.Close();
            }
            return sucesso;
        }

        public Models.TodoItem ObterPorId(int id)
        {
            Models.TodoItem item =null;

            string strbd = Environment.GetEnvironmentVariable("BD_STRING_CONEXAO");
            Npgsql.NpgsqlConnection conexao = new
                    Npgsql.NpgsqlConnection(strbd);


            Npgsql.NpgsqlCommand cmd = conexao.CreateCommand();

            cmd.CommandText = @"select * from todolist where id=@id";
            cmd.Parameters.AddWithValue("@id", id);
            conexao.Open();

            var dr = cmd.ExecuteReader();
            if(dr.Read())
            {
                item = new Models.TodoItem();
                item.Id = Convert.ToInt32(dr["id"]);
                item.Tarefa = (dr["tarefa"].ToString());
            }

            conexao.Close();

            return item;
        }

    }
}
