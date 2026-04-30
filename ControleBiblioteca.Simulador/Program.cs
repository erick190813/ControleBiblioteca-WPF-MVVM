using System.Net.Http.Json;

Console.Title = "Simulador de Biblioteca em Tempo Real";
Console.WriteLine("=========================================");
Console.WriteLine("Iniciando Simulador de Empréstimos...");
Console.WriteLine("=========================================\n");

string apiUrl = "https://localhost:7118";

using var client = new HttpClient { BaseAddress = new Uri(apiUrl) };
Random random = new Random();

while (true)
{
    try
    {
        // Define uma probabilidade: 60% de chance de Emprestar, 40% de Devolver
        int acao = random.Next(1, 101);

        if (acao <= 60)
        {
            // Sorteia IDs de 1 a 10 para simular os Livros e Usuários cadastrados
            var novoEmprestimo = new
            {
                LivroId = random.Next(1, 11), // Vai sortear de 1 a 10
                UsuarioId = random.Next(1, 11)
            };

            Console.WriteLine($"[EMPRÉSTIMO] Tentando registrar: Livro ID {novoEmprestimo.LivroId} para Usuário ID {novoEmprestimo.UsuarioId}...");

            var response = await client.PostAsJsonAsync("/api/Emprestimos", novoEmprestimo);

            if (response.IsSuccessStatusCode)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(" -> SUCESSO: Empréstimo registrado!");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                string erro = await response.Content.ReadAsStringAsync();
                Console.WriteLine($" -> RECUSADO: {erro} (Status {response.StatusCode})");
            }
            Console.ResetColor();
        }
        else
        {
            // Sorteia um ID de empréstimo (de 1 a 20) para tentar devolver
            int emprestimoId = random.Next(1, 21);

            Console.WriteLine($"[DEVOLUÇÃO] Tentando devolver o Empréstimo ID {emprestimoId}...");
            var response = await client.PutAsync($"/api/Emprestimos/{emprestimoId}/devolver", null);

            if (response.IsSuccessStatusCode)
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine(" -> SUCESSO: Livro devolvido ao estoque!");
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(" -> AVISO: Livro já estava devolvido anteriormente.");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine($" -> IGNORADO: Empréstimo não encontrado (Status {response.StatusCode})");
            }
            Console.ResetColor();
        }
    }
    catch (HttpRequestException)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("[ERRO CRÍTICO] Falha ao conectar. A API está rodando na mesma porta configurada?");
        Console.ResetColor();
    }
    catch (Exception ex)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"[ERRO] Ocorreu uma exceção inesperada: {ex.Message}");
        Console.ResetColor();
    }

    // Aguarda um tempo aleatório entre 3 e 8 segundos para simular fluxo humano real
    int tempoEspera = random.Next(3000, 8000);
    Console.WriteLine($"\n... aguardando {tempoEspera / 1000} segundos ...\n");
    await Task.Delay(tempoEspera);
}