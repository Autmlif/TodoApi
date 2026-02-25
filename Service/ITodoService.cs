namespace TodoApi.Service
{
    public interface ITodoService
    {
        Task<List<TodoItemDTO>> GetAllAsync();
        Task<TodoItemDTO?> GetByIdAsync(int id);
        Task<TodoItemDTO> CreateAsync(TodoItemDTO dto);
        Task<bool> UpdateAsync(int id, TodoItemDTO dto);
        Task<bool> DeleteAsync(int id);
    }
}
