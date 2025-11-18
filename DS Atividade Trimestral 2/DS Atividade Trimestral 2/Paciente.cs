using System;

namespace DS_Atividade_Trimestral_2
{
    class Paciente
    {
        public int id;
        public int idade;
        public string nome;
        public string estado;
        public int preferencial;

        public Paciente()
        {

        }
        public void CalcularPreferencial()
        {
            this.preferencial = 0;
            this.preferencial += idade <= 8 ? 1 : idade >= 50 ? 2 : 0;
            this.preferencial += estado == "M" ? 20 : estado == "G" ? 50 : 0;
            // this.preferencial += nome.Contains("Ryan") ? 1000 : 0;
        }
        public Paciente(int id, int idade, string nome, string estado)
        {
            this.id = id;
            this.idade = idade;
            this.nome = nome;
            this.estado = estado;
        }
        public void SolicitarNome()
        {
            Console.WriteLine("Digite o nome do paciente:");
            this.nome = Console.ReadLine();
        }
        public void SolicitarEstado()
        {
            Console.WriteLine("Digite o estado físico do paciente(\"B\" para baixo, \"M\" para medio e \"G\" para severo):");
            string estado = Console.ReadLine().ToUpper();
            for (int tentativas = 1; !(estado == "B" || estado == "M" || estado == "G");tentativas++)
            {
                if (!(tentativas%3 == 0))
                {
                    Console.WriteLine("Não foi dado um valor solicitado. Os possivels valores são \"B\" para baixo, \"M\" para medio e \"G\" para baixo. Tente novamente:");
                    estado = Console.ReadLine().ToUpper();
                }
                else
                {
                    Console.WriteLine($"Você já tentou {tentativas} vezes, digite \"1\" se quiser continuar com a adição deste usuario. Caso o contrario, será redirecionado ao Menu.");
                    estado = Console.ReadLine();
                    if (estado == "1")
                    {
                        Console.WriteLine("Então digite um valor conforme solicitado. \"B\", \"M\" ou \"G\".");
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