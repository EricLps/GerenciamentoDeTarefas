using Microsoft.AspNetCore.Mvc;
using GerenciadorDeTarefas.Models;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace GerenciadorDeTarefas.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TarefaController : ControllerBase
    {
        private readonly TarefaContext _context;

        public TarefaController(TarefaContext context)
        {
            _context = context;
        }

        // Endpoint para Criar uma nova Tarefa (POST /tarefa)
        [HttpPost]
        public async Task<IActionResult> Criar(Tarefa tarefa)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Tarefas.Add(tarefa);
            await _context.SaveChangesAsync();


            return CreatedAtAction(nameof(ObterPorId), new { id = tarefa.Id }, tarefa);
        }

        // Endpoint para Obter todas as Tarefas (GET /tarefa)
        [HttpGet]
        public async Task<IActionResult> ObterTodos()
        {
            var tarefas = await _context.Tarefas.ToListAsync();
            return Ok(tarefas);
        }

        // Endpoint para Obter Tarefa por ID (GET /tarefa/{id})
        [HttpGet("{id}")]
        public async Task<IActionResult> ObterPorId(int id)
        {
            var tarefa = await _context.Tarefas.FirstOrDefaultAsync(x => x.Id == id);

            if (tarefa == null)
            {
                return NotFound();
            }

            return Ok(tarefa);
        }

        // Endpoint para Obter Tarefas por Status (GET /tarefa/status/{status})
        [HttpGet("status/{status}")]
        public async Task<IActionResult> ObterPorStatus(EnumStatusTarefa status)
        {
            // Filtra as tarefas pelo status fornecido
            var tarefas = await _context.Tarefas.Where(x => x.Status == status).ToListAsync();

            if (!tarefas.Any())
            {
                return NotFound($"Nenhuma tarefa encontrada com o status '{status}'.");
            }

            return Ok(tarefas);
        }

        // Endpoint para Atualizar uma Tarefa (PUT /tarefa/{id})
        [HttpPut("{id}")]
        public async Task<IActionResult> Atualizar(int id, Tarefa tarefa)
        {
            var tarefaBanco = await _context.Tarefas.FirstOrDefaultAsync(x => x.Id == id); // Busca a tarefa existente

            if (tarefaBanco == null)
            {
                return NotFound();
            }

            // Atualiza as propriedades da tarefa encontrada com os dados recebidos
            tarefaBanco.Titulo = tarefa.Titulo;
            tarefaBanco.Descricao = tarefa.Descricao;
            tarefaBanco.Data = tarefa.Data;
            tarefaBanco.Status = tarefa.Status;

            _context.Tarefas.Update(tarefaBanco);
            await _context.SaveChangesAsync();

            return Ok(tarefaBanco);
        }

        // Endpoint para Deletar uma Tarefa (DELETE /tarefa/{id})
        [HttpDelete("{id}")]
        public async Task<IActionResult> Deletar(int id)
        {
            var tarefaBanco = await _context.Tarefas.FirstOrDefaultAsync(x => x.Id == id);

            if (tarefaBanco == null)
            {
                return NotFound();
            }

            _context.Tarefas.Remove(tarefaBanco);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}