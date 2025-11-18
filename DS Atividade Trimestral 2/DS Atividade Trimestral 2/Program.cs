using System;
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
                novopaciente.SolicitarNome();

                Console.Clear();
                Console.WriteLine("-----    Adicionar    -----");
                novopaciente.SolicitarIdade();

                Console.Clear();
                Console.WriteLine("-----    Adicionar    -----");
                novopaciente.SolicitarEstado();

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
            Console.WriteLine("Aqui está a lista de todos os pacientes na fila, em ordem dependendo de 1. quando entraram, 2. sua idade e 3. seu estado físico atual\n");
            Paciente[] pacientes = RetornarPacientes();

            foreach (Paciente paciente in pacientes)
            {
                if (paciente != null)
                {
                    paciente.CalcularPreferencial();
                }
            }

            for (int i = pacientes.Length-1; i >= 0; i--)
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

            Console.WriteLine("Nome - Idade - Estado - Nº Preferencial\n");
            foreach (Paciente paciente in pacientes)
            {
                if (paciente != null)
                {
                    Console.WriteLine($"{paciente.nome} - {paciente.idade} - {paciente.estado} - {paciente.preferencial}");
                }
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
        }
        static void PaginaSaida()
        {
            connection.Close();
            Console.WriteLine("Obrigado por utilizar a fila de hospital virtual! Aperte qualquer botão para que o aplicativo se feche.\n\t\t\t- Ryan \"BlankH\" Goncalves");
            Console.ReadKey();
        }
    }
}
