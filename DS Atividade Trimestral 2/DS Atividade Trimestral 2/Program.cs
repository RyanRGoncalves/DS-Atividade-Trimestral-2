using System;
using System.Diagnostics.Eventing.Reader;
using MySql.Data.MySqlClient;

namespace DS_Atividade_Trimestral_2
{
    class Program
    {
        public static MySqlConnection connection = new MySqlConnection("server=localhost;user=root;database=filadhospital;port=3307");
        static Paciente[] RetornarPacientes()
        {
            MySqlDataReader rdr = new MySqlCommand("SELECT * FROM paciente;", connection).ExecuteReader();
            Paciente[] pacientes = new Paciente[15];

            int contador = 0;
            while (rdr.Read())
            {
                pacientes[contador] = new Paciente(int.Parse(rdr[0].ToString()), int.Parse(rdr[1].ToString()), rdr[2].ToString(), rdr[3].ToString());
                contador++;
            }
            rdr.Close();
            return pacientes;
        }
        static void Main()
        {
            try
            {
                connection.Open();
                AlterarPagina(0, "Bem vindo a fila de hospital digital!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro encontrado: {ex.Message}");
                Console.WriteLine("Mande este erro para o operador, o programa será fechado quando um botão for digitado.");
                Console.ReadKey();
            }
        }
        public static void AlterarPagina(int numeropagina, string texto = null)
        {
            Console.Clear();
            if (texto != null)
            {
                Console.WriteLine(texto+"\n");
            }
            switch (numeropagina)
            {
                case 0:
                    PaginaMenu();
                    break;
                case 1:
                    PaginaAdicionar();
                    break;
                case 2:
                    PaginaVisualizar();
                    break;
                case 3:
                    PaginaAlterar();
                    break;
                case 4:
                    PaginaRemover();
                    break;
                case 5:
                    PaginaSaida();
                    break;
            }
        }
        static void PaginaMenu()
        {
            Console.WriteLine("-----    Menu    -----");
            Console.WriteLine("Escolha o que deseja fazer:");
            Console.WriteLine("1. Adicionar um paciente\n2. Visualizar todos os pacientes\n3. Atualizar os dados de um paciente\n4. Remover um paciente\n5/Q. Sair");
            string answer = Console.ReadLine();
            if (int.TryParse(answer, out int page) && page > 0 && page < 6)
            {
                AlterarPagina(page);
            }
            else if (answer.ToUpper() == "Q")
            {
                AlterarPagina(5);
            }
            else
            {
                AlterarPagina(0, "Opção invalida.");
            }
        }
        static void PaginaAdicionar()
        {
            if (!(int.Parse(new MySqlCommand("SELECT COUNT(*) FROM paciente;", connection).ExecuteScalar().ToString()) >= 15))
            {
                Paciente novopaciente = new Paciente();
                Console.WriteLine("-----    Adicionar    -----");
                novopaciente.nome = Paciente.SolicitarNome();

                Console.Clear();
                Console.WriteLine("-----    Adicionar    -----");
                novopaciente.idade = Paciente.SolicitarIdade();
                if (novopaciente.idade == -1)
                {
                    AlterarPagina(0, "Nenhum paciente adicionado.");
                }

                Console.Clear();
                Console.WriteLine("-----    Adicionar    -----");
                novopaciente.estado = Paciente.SolicitarEstado();
                if (novopaciente.estado == "invalido")
                {
                    AlterarPagina(0, "Nenhum paciente adicionado.");
                }

                try
                {
                    MySqlCommand command = new MySqlCommand($"INSERT INTO paciente(idade_paciente, nome_paciente, estado_paciente) VALUES ({novopaciente.idade}, '{novopaciente.nome}', '{novopaciente.estado}');", connection);
                    command.ExecuteNonQuery();
                }
                catch (MySqlException e)
                {
                    AlterarPagina(0, e.Message);
                }

                AlterarPagina(0, $"Paciente adicionado: {novopaciente.nome}, {novopaciente.idade}, {novopaciente.estado}");
            }
            else
            {
                Console.WriteLine("Há muitos pacientes na lista! Por favor, remova um paciente antes de adicionar mais.");
                Console.WriteLine("Digite qualquer botão para voltar ao Menu.");
                Console.ReadKey();
                AlterarPagina(0);
            }
        }
        static void PaginaVisualizar()
        {
            Console.WriteLine("-----    Visualizar    -----");
            Console.WriteLine("Aqui está a lista de todos os pacientes na fila\nEm ordem dependendo de seu preferencial.");
            Paciente[] pacientes = RetornarPacientes();
            if (pacientes[0] != null)
            {
                Console.WriteLine($"Há {new MySqlCommand("SELECT count(*) FROM paciente;", connection).ExecuteScalar()} pessoa(s) dentro da fila.");

                foreach (Paciente paciente in pacientes)
                {
                    if (paciente != null)
                    {
                        paciente.CalcularPreferencial();
                    }
                }

                for (int i = pacientes.Length - 1; i >= 0; i--)
                {
                    if (pacientes[i] == null)
                    {
                        continue;
                    }
                    for (int j = 0; j < i; j++)
                    {
                        if (pacientes[j].preferencial < pacientes[i].preferencial)
                        {
                            Paciente aux = pacientes[i];
                            pacientes[i] = pacientes[j];
                            pacientes[j] = aux;
                        }
                    }
                }

                Console.WriteLine("\nNº na fila - Nome - Idade - Estado - Nº Preferencial\n");
                for (int i=0; i < pacientes.Length; i++)
                {
                    if (pacientes[i] != null)
                    {
                        Console.WriteLine($"{i+1} - {pacientes[i].nome} - {pacientes[i].idade} - {pacientes[i].estado} - {pacientes[i].preferencial}");
                    }
                }
            }
            else
            {
                Console.WriteLine("\nNão há nenhum paciente atualmente, adicione um paciente antes de continuar.");
            }

            Console.WriteLine("\nDigite qualquer tecla para voltar ao menu.");
            Console.ReadKey();
            AlterarPagina(0);
        }
        static void PaginaAlterar()
        {
            Console.WriteLine("-----    Alterar    -----");
            Console.WriteLine("Está pagina´é utilizada para alterar as informações de um usuario.\nDigite o ID do usuario para Alterar-lo, digite 0 para voltar ao menu.\n");

            Console.WriteLine("ID - Nome\n");
            foreach (Paciente paciente in RetornarPacientes())
            {
                if (paciente != null)
                {
                    Console.WriteLine($"{paciente.id} - {paciente.nome}");
                }
            }

            string resposta = Console.ReadLine();

            if (int.TryParse(resposta, out int id) && id > 0 && id < 16 && int.Parse(new MySqlCommand($"SELECT COUNT(*) FROM paciente WHERE id_paciente = {id}", connection).ExecuteScalar().ToString()) != 0)
            {
                Console.Clear();
                Console.WriteLine("-----    Alterar    -----");
                Console.WriteLine("Você está atualmente visualizando um paciente em especifico.\nDigite o nome do campo que deseja alterar. Digite 0 para voltar ao menu.\n");

                MySqlDataReader rdr = new MySqlCommand($"SELECT nome_paciente, idade_paciente, estado_paciente FROM paciente WHERE id_paciente = {resposta};", connection).ExecuteReader();
                rdr.Read();

                Console.WriteLine("Campo: dado\n");
                Console.WriteLine($"Nome: {rdr[0]}\nIdade: {rdr[1]}\nEstado: {rdr[2]}");
                rdr.Close();
                resposta = Console.ReadLine().ToUpper();

                if (resposta == "NOME" || resposta == "IDADE" || resposta == "ESTADO")
                {
                    Console.Clear();
                    Console.WriteLine("-----    Alterar    -----");
                    if (resposta == "NOME")
                    {
                        string novonome = Paciente.SolicitarNome();
                        new MySqlCommand($"UPDATE paciente SET nome_paciente = \"{novonome}\" WHERE id_paciente = {id}", connection).ExecuteNonQuery();
                    }
                    else if (resposta == "IDADE")
                    {
                        int novaidade = Paciente.SolicitarIdade();
                        new MySqlCommand($"UPDATE paciente SET nome_paciente = \"{novaidade}\" WHERE id_paciente = {id}", connection).ExecuteNonQuery();
                    }
                    else if (resposta == "ESTADO")
                    {
                        string novoestado = Paciente.SolicitarEstado();
                        new MySqlCommand($"UPDATE paciente SET nome_paciente = \"{novoestado}\" WHERE id_paciente = {id}", connection).ExecuteNonQuery();
                    }
                    AlterarPagina(0, "Paciente alterado com sucesso.");
                }
                else if (resposta == "0")
                {
                    AlterarPagina(0, "Paciente não foi alterado.");
                }
                else
                {
                    AlterarPagina(3, "Valor invalido!");
                }
            }
            else if (resposta == "0")
            {
                AlterarPagina(0, "Nenhum paciente alterado.");
            }
            else
            {
                AlterarPagina(3, "Este valor é invalido.");
            }
        }
        static void PaginaRemover()
        {
            Console.WriteLine("-----    Remover    -----");
            Console.WriteLine("Está pagina´é utilizada para remover um usuario.\nDigite o ID do usuario para remover-lo, digite 0 para voltar ao menu.\n");

            Console.WriteLine("ID - Nome\n");
            Paciente[] pacientes = RetornarPacientes();
            foreach (Paciente paciente in pacientes)
            {
                if (paciente != null)
                {
                    Console.WriteLine($"{paciente.id} - {paciente.nome}");
                }
            }

            string resposta = Console.ReadLine();

            if (int.TryParse(resposta, out int id) && id > 0 && id < 16 && int.Parse(new MySqlCommand($"SELECT COUNT(*) FROM paciente WHERE id_paciente = {id}", connection).ExecuteScalar().ToString()) != 0)
            {
                Console.Clear();
                Console.WriteLine("-----    Remover    -----");
                Console.WriteLine("Você tem certeza que deseja remover o paciente selecionado?\nPressione 1 para sim, 0 para não.");

                resposta = Console.ReadLine();
                if (resposta == "0")
                {
                    AlterarPagina(0, "Nenhum paciente foi removido");
                }
                else if (resposta == "1")
                {
                    new MySqlCommand($"DELETE FROM paciente WHERE id_paciente = {id}", connection).ExecuteScalar();
                    AlterarPagina(0, "O paciente selecionado foi removido.");
                }
                else
                {
                    AlterarPagina(4, "Valor invalido.");
                }
            }
            else if (resposta == "0")
            {
                AlterarPagina(0, "Nenhum paciente foi removido.");
            }
            else
            {
                AlterarPagina(4, "Este valor é invalido.");
            }
        }
        static void PaginaSaida()
        {
            connection.Close();
            Console.WriteLine("Obrigado por utilizar a fila de hospital virtual! Aperte qualquer botão para que o aplicativo se feche.\n\t\t\t- Ryan \"BlankH\" Goncalves");
            Console.ReadKey();
        }
    }
}