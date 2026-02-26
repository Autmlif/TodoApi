public class TodoItemDTO
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public bool IsComplete { get; set; }
    public int Priority { get; set; }

    public TodoItemDTO() { }
    public TodoItemDTO(Todo todoItem) =>
    (Id, Name, IsComplete, Priority) = (todoItem.Id, todoItem.Name, todoItem.IsComplete, todoItem.Priority);
}