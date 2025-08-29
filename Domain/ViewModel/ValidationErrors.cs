namespace TrilhaApiDesafio.Domain.ViewModel;

public struct ValidationErrors
{
    public List<String> Messages { get; set; } = default!;
    public bool IsError
    {
        get => Messages.Any();
    }
    public ValidationErrors(List<String> messages)
    {
        this.Messages = messages;
    }
}