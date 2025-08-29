using Microsoft.AspNetCore.Mvc;
using TrilhaApiDesafio.Context;
using TrilhaApiDesafio.Domain.DTOs;
using TrilhaApiDesafio.Domain.Models;
using TrilhaApiDesafio.Domain.ViewModel;

namespace TrilhaApiDesafio.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TarefaController : ControllerBase
    {
        private readonly OrganizadorContext _context;

        public TarefaController(OrganizadorContext context)
        {
            _context = context;
        }

        [HttpGet("{id}")]
        public IActionResult ObterPorId(int id)
        {
            // TODO: Buscar o Id no banco utilizando o EF
            Tarefa tarefa = _context.Tarefas.Find(id);
            // TODO: Validar o tipo de retorno. Se não encontrar a tarefa, retornar NotFound,
            if (tarefa == null)
                return NoContent();
            // caso contrário retornar OK com a tarefa encontrada
            return Ok(tarefa);
        }

        [HttpGet("ObterTodos")]
        public IActionResult ObterTodos(int? page, int? pageSize)
        {
            // TODO: Buscar todas as tarefas no banco utilizando o EF
            if (page < 1 || page == null) { page = 1;}
            if (pageSize < 1 || pageSize == null) { pageSize = 10 ;}

            var query = _context.Tarefas.AsQueryable();
            query = query.Skip((int)((page - 1) * pageSize));
            query = query.Take((int)pageSize);
            
            var tarefas = query.ToList();
            return Ok(tarefas);
        }

        [HttpGet("ObterPorTitulo")]
        public IActionResult ObterPorTitulo(string titulo)
        {
            if (String.IsNullOrEmpty(titulo))
            {
                return BadRequest("Título inválido, campo vazio.");
            }
            
            // TODO: Buscar  as tarefas no banco utilizando o EF, que contenha o titulo recebido por parâmetro
            List<Tarefa> tarefas = _context.Tarefas.Where(tarefa => tarefa.Titulo.Contains(titulo)).ToList();

            if (!tarefas.Any())
            {
                return NoContent();
            }
            // Dica: Usar como exemplo o endpoint ObterPorData
            return Ok(tarefas);
        }

        [HttpGet("ObterPorData")]
        public IActionResult ObterPorData(DateTime data)
        {
            var tarefa = _context.Tarefas.Where(x => x.Data.Date == data.Date).ToList();
            if (!tarefa.Any())
            {
                return NoContent();
            }
            return Ok(tarefa);
        }

        [HttpGet("ObterPorStatus")]
        public IActionResult ObterPorStatus(EnumStatusTarefa status)
        {
            // TODO: Buscar  as tarefas no banco utilizando o EF, que contenha o status recebido por parâmetro
            // Dica: Usar como exemplo o endpoint ObterPorData
            var tarefa = _context.Tarefas.Where(x => x.Status == status).ToList();
            if (!tarefa.Any())
            {
                return NoContent();
            }
            return Ok(tarefa);
        }

        [HttpPost]
        public IActionResult Criar([FromBody]TarefaDto tarefaDto)
        {
            var validation = new ValidationErrors(
                messages: tarefaDto.Validation());
            if (validation.IsError)
            {
                return BadRequest(validation);
            }

            var tarefa = new Tarefa()
            {
                Data = tarefaDto.Data,
                Descricao = tarefaDto.Descricao,
                Status = tarefaDto.Status,
                Titulo = tarefaDto.Titulo,
            };
            
            // TODO: Adicionar a tarefa recebida no EF e salvar as mudanças (save changes)
            _context.Tarefas.Add(tarefa);
            var result = _context.SaveChanges();
            if (result <= 0)
            {
                return BadRequest("Dados não foram persistido!");
            }
            return CreatedAtAction(nameof(ObterPorId), new { id = tarefa.Id }, tarefa);
        }

        [HttpPut("{id}")]
        public IActionResult Atualizar(int id, [FromBody]TarefaDto tarefaDto)
        {
            var validation = new ValidationErrors(
                messages: tarefaDto.Validation());
            if (validation.IsError)
            {
                return BadRequest(validation);
            }
            
            var tarefaBanco = _context.Tarefas.Find(id);
            if (tarefaBanco == null)
                return NotFound();
            
            // TODO: Atualizar as informações da variável tarefaBanco com a tarefa recebida via parâmetro
            tarefaBanco.Titulo = tarefaDto.Titulo;
            tarefaBanco.Descricao = tarefaDto.Descricao;
            tarefaBanco.Data = tarefaDto.Data.Date;
            tarefaBanco.Status = tarefaDto.Status;
            
            // TODO: Atualizar a variável tarefaBanco no EF e salvar as mudanças (save changes)
            _context.Tarefas.Update(tarefaBanco);
            var result = _context.SaveChanges();
            if (result <= 0)
            {
                return BadRequest("Dados não foram persistido!");
            }
            return Ok(tarefaBanco);
        }

        [HttpDelete("{id}")]
        public IActionResult Deletar(int id)
        {
            var tarefaBanco = _context.Tarefas.Find(id);

            if (tarefaBanco == null)
                return NotFound();

            // TODO: Remover a tarefa encontrada através do EF e salvar as mudanças (save changes)
            _context.Tarefas.Remove(tarefaBanco);
            var result = _context.SaveChanges();
            if (result <= 0)
            {
                return BadRequest("Dados não foram persistido!");
            }
            return Ok();
        }
    }
}
