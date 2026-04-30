using ControleBiblioteca.Api.DTOs;
using ControleBiblioteca.Api.Models;
using ControleBiblioteca.Api.Repositories;
using ControleBiblioteca.Api.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ControleBiblioteca.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmprestimosController : ControllerBase
    {
        private readonly IEmprestimoRepository _repository;
        private readonly BibliotecaDbContext _context;
        private readonly ILogger<EmprestimosController> _logger;

        public EmprestimosController(
            IEmprestimoRepository repository,
            BibliotecaDbContext context,
            ILogger<EmprestimosController> logger)
        {
            _repository = repository;
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Obtém a lista de todos os empréstimos registrados.
        /// </summary>
        /// <remarks>
        /// Retorna uma lista completa contendo os dados do empréstimo, 
        /// incluindo o título do livro e o nome do usuário associado.
        /// Ideal para popular o dashboard de controle.
        /// </remarks>
        /// <returns>Uma lista de objetos EmprestimoResponseDTO.</returns>
        /// <response code="200">Retorna a lista de empréstimos com sucesso.</response>
        /// <response code="500">Ocorreu um erro interno no servidor ao acessar o banco de dados.</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<EmprestimoResponseDTO>>> GetEmprestimos()
        {
            try
            {
                var emprestimos = await _repository.ObterTodosAsync();
                var response = emprestimos.Select(e => MapToResponseDTO(e));
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao listar empréstimos.");
                return StatusCode(500, "Erro interno ao processar a solicitação.");
            }
        }

        /// <summary>
        /// Obtém os detalhes de um empréstimo específico pelo seu ID.
        /// </summary>
        /// <param name="id">O ID único do empréstimo.</param>
        /// <returns>Os dados detalhados do empréstimo solicitado.</returns>
        /// <response code="200">Empréstimo encontrado e retornado com sucesso.</response>
        /// <response code="404">Nenhum empréstimo foi encontrado com o ID fornecido.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<EmprestimoResponseDTO>> GetEmprestimo(int id)
        {
            var emprestimo = await _repository.ObterPorIdAsync(id);
            if (emprestimo == null) return NotFound($"Empréstimo com ID {id} não encontrado.");

            return Ok(MapToResponseDTO(emprestimo));
        }

        /// <summary>
        /// Registra um novo empréstimo de livro para um usuário.
        /// </summary>
        /// <remarks>
        /// Este endpoint processa regras críticas de negócio:
        /// 
        ///     1. Valida se o Livro e o Usuário informados existem na base.
        ///     2. Checa se a propriedade 'QuantidadeDisponivel' do livro é maior que zero.
        ///     3. Deduz automaticamente 1 unidade do estoque do livro.
        ///     4. Define a 'DataRetirada' como o momento atual e a 'DataVencimento' para 7 dias corridos.
        ///     
        /// Toda a operação é envolvida em uma transação de banco de dados para garantir que não haja perda de consistência no estoque em caso de falha no servidor.
        /// </remarks>
        /// <param name="dto">Objeto contendo o ID do Livro e o ID do Usuário.</param>
        /// <returns>O registro do empréstimo recém-criado.</returns>
        /// <response code="201">Empréstimo registrado com sucesso e estoque atualizado.</response>
        /// <response code="400">Dados inválidos enviados ou o livro está sem estoque.</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<EmprestimoResponseDTO>> RegistrarEmprestimo([FromBody] RegistrarEmprestimoDTO dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var livro = await _context.Livros.FindAsync(dto.LivroId);
                var usuario = await _context.Usuarios.FindAsync(dto.UsuarioId);

                if (livro == null) return BadRequest("Livro não cadastrado.");
                if (usuario == null) return BadRequest("Usuário não cadastrado.");
                if (livro.QuantidadeDisponivel <= 0) return BadRequest("Não há exemplares disponíveis deste livro.");

                var novoEmprestimo = new Emprestimo
                {
                    LivroId = dto.LivroId,
                    UsuarioId = dto.UsuarioId,
                    DataRetirada = DateTime.Now,
                    DataVencimento = DateTime.Now.AddDays(7),
                    StatusEmprestimo = "Ativo"
                };

                livro.QuantidadeDisponivel--;

                await _repository.AdicionarAsync(novoEmprestimo);
                await transaction.CommitAsync();

                return CreatedAtAction(nameof(GetEmprestimo), new { id = novoEmprestimo.Id }, MapToResponseDTO(novoEmprestimo));
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Erro ao registrar empréstimo.");
                return StatusCode(500, "Erro ao processar o empréstimo no banco de dados.");
            }
        }

        /// <summary>
        /// Processa a devolução de um livro emprestado.
        /// </summary>
        /// <remarks>
        /// Ao acionar este endpoint, o sistema executará as seguintes ações:
        /// 
        ///     - Verifica se o empréstimo já não foi encerrado anteriormente.
        ///     - Retorna 1 unidade para a 'QuantidadeDisponivel' do livro no estoque.
        ///     - Marca a 'DataDevolucaoReal' com a data e hora exatas da requisição.
        ///     - Atualiza o status para "Devolvido".
        ///     - Calcula automaticamente a 'MultaAtraso' cobrando R$ 2,00 por cada dia excedido após a data de vencimento.
        /// </remarks>
        /// <param name="id">O ID do empréstimo que está sendo finalizado.</param>
        /// <response code="204">Devolução processada com sucesso. (Sem conteúdo de retorno)</response>
        /// <response code="400">O livro já consta como devolvido no sistema.</response>
        /// <response code="404">O registro de empréstimo não foi encontrado.</response>
        /// <response code="409">Houve um conflito de concorrência ao tentar atualizar o banco de dados.</response>
        [HttpPut("{id}/devolver")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> DevolverLivro(int id)
        {
            try
            {
                var emprestimo = await _repository.ObterPorIdAsync(id);
                if (emprestimo == null) return NotFound();
                if (emprestimo.StatusEmprestimo == "Devolvido") return BadRequest("Este empréstimo já foi encerrado.");

                emprestimo.DataDevolucaoReal = DateTime.Now;
                emprestimo.StatusEmprestimo = "Devolvido";

                if (emprestimo.Livro != null)
                {
                    emprestimo.Livro.QuantidadeDisponivel++;
                }

                if (emprestimo.DataDevolucaoReal > emprestimo.DataVencimento)
                {
                    var diasAtraso = (emprestimo.DataDevolucaoReal.Value - emprestimo.DataVencimento).Days;
                    emprestimo.MultaAtraso = diasAtraso * 2.0m;
                }

                await _repository.AtualizarAsync(emprestimo);
                return NoContent();
            }
            catch (DbUpdateConcurrencyException)
            {
                return Conflict("O registro foi alterado por outro usuário. Tente novamente.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao devolver livro.");
                return StatusCode(500, "Erro interno na devolução.");
            }
        }

        /// <summary>
        /// Remove fisicamente um registro de empréstimo do banco de dados.
        /// </summary>
        /// <remarks>
        /// **Atenção:** Se o empréstimo excluído ainda estiver com o status "Ativo", 
        /// o sistema assumirá que a transação foi cancelada e devolverá automaticamente 
        /// a unidade do livro ao estoque para evitar inconsistências no inventário.
        /// </remarks>
        /// <param name="id">O ID do empréstimo a ser excluído.</param>
        /// <response code="204">Registro excluído com sucesso.</response>
        /// <response code="404">O registro informado não existe.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeletarEmprestimo(int id)
        {
            try
            {
                var emprestimo = await _repository.ObterPorIdAsync(id);
                if (emprestimo == null) return NotFound();

                if (emprestimo.StatusEmprestimo == "Ativo" && emprestimo.Livro != null)
                {
                    emprestimo.Livro.QuantidadeDisponivel++;
                }

                await _repository.DeletarAsync(emprestimo);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao excluir empréstimo.");
                return StatusCode(500, "Não foi possível excluir o registro.");
            }
        }

        private static EmprestimoResponseDTO MapToResponseDTO(Emprestimo e) => new()
        {
            Id = e.Id,
            TituloLivro = e.Livro?.Titulo ?? "N/A",
            NomeUsuario = e.Usuario?.NomeCompleto ?? "N/A",
            DataRetirada = e.DataRetirada,
            DataDevolucaoReal = e.DataDevolucaoReal,
            MultaAtraso = e.MultaAtraso,
            StatusEmprestimo = e.StatusEmprestimo
        };
    }
}