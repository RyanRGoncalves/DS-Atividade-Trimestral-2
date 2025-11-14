using System;
using MySql.Data.MySqlClient;

namespace DS_Atividade_Trimestral_2
{
    class Program
    {
        static MySqlConnection connection = new MySqlConnection("server=localhost;user=root;database=filadhospital;port=3307");
        static void Main()
        {
            try
            {
                connection.Open();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro encontrado: {ex.Message}");
                Console.WriteLine("Mande este erro para o operador, o programa será fechado quando um botão for digitado.");
                Console.ReadKey();
            }
            AlterarPagina(0, "Bem vindo a fila de hospital digital!");
        }
        static void AlterarPagina(int numeropagina, string texto = null)
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
                case 5:
                    PaginaSaida();
                    break;
            }
        }
        static void PaginaMenu()
        {
            Console.WriteLine("-----    Menu   -----");
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
            
        }
        static void PaginaSaida()
        {
            Console.WriteLine("Obrigado por utilizar a fila de hospital virtual! Aperte qualquer botão para que o aplicativo se feche.\n\t\t\t- Ryan \"BlankH\" Goncalves");
            Console.ReadKey();
        }
    }
}
