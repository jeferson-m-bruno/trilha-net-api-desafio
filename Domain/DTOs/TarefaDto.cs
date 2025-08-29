using TrilhaApiDesafio.Domain.Models;

namespace TrilhaApiDesafio.Domain.DTOs;

public class TarefaDto
{
    public string Titulo { get; set; }
    public string Descricao { get; set; }
    public DateTime Data { get; set; }
    public EnumStatusTarefa Status { get; set; }
    
    public List<String> Validation()
    {
        List<String> messages = new List<String>();
        if (String.IsNullOrEmpty(Titulo))
        {
            messages.Add("Título não pode ser vazio");
        }

        if (Data == DateTime.MinValue)
        {
            messages.Add("A data da tarefa não pode ser vazia");
        }

        return messages;
    }
}