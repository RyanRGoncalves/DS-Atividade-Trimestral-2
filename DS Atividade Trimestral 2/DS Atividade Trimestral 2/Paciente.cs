using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DS_Atividade_Trimestral_2
{
    class Paciente
    {
        private int id;
        public int idade;
        public string nome;
        public string estado;

        public void SolicitarNome()
        {
            Console.WriteLine("Digite o nome do paciente:");
            this.nome = Console.ReadLine();
        }
        public void SolicitarEstado()
        {
            Console.WriteLine("Digite o estado físico do paciente(\"B\" para baixo, \"M\" para medio e \"S\" para severo):");
            string estado = Console.ReadLine().ToUpper();
            for (int tentativas = 1; !(estado == "B" || estado == "M" || estado == "S");tentativas++)
            {
                if (!(tentativas%3 == 0))
                {
                    Console.WriteLine("Não foi dado um valor solicitado. Os possivels valores são \"B\" para baixo, \"M\" para medio e \"S\" para baixo. Tente novamente:");
                    estado = Console.ReadLine().ToUpper();
                }
                else
                {
                    Console.WriteLine($"Você já tentou {tentativas} vezes, digite \"1\" se quiser continuar com a adição deste usuario. Caso o contrario, será redirecionado ao Menu.");
                    estado = Console.ReadLine();
                    if (estado == "1")
                    {
                        Console.WriteLine("Então digite um valor conforme solicitado. \"B\", \"M\" ou \"S\".");
                        estado = Console.ReadLine().ToUpper();
                    }
                    else
                    {
                        Program.AlterarPagina(0, "Nenhum usuario foi adicionado.");
                    }
                }
            }
            this.estado = estado;
        }
        public void SolicitarIdade()
        {
            int idade;
            Console.WriteLine("Digite a idade do paciente:");
            string resposta = Console.ReadLine();
            for (int tentativas = 1; !(int.TryParse(resposta, out idade) && idade >= 0); tentativas++)
            {
                if (!(tentativas % 3 == 0))
                {
                    Console.WriteLine("Não foi dado um valor solicitado. O valor tem que ser um número maior ou igual a 0. Tente novamente:");
                    resposta = Console.ReadLine();
                }
                else
                {
                    Console.WriteLine($"Você já tentou {tentativas} vezes, digite \"1\" se quiser continuar com a adição deste usuario. Caso o contrario, será redirecionado ao Menu.");
                    resposta = Console.ReadLine();
                    if (resposta == "1")
                    {
                        Console.WriteLine("Então digite um valor conforme solicitado. Um número maior ou igual a 0.");
                        resposta = Console.ReadLine();
                    }
                    else
                    {
                        Program.AlterarPagina(0, "Nenhum usuario foi adicionado.");
                    }
                }
            }
            this.idade = idade;
        }
    }
}
