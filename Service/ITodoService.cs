namespace TodoApi.Service
{
    public interface ITodoService
    {
        Task<PagedResult<TodoItemDTO>> GetAllAsync(int page, int pageSize);
        Task<TodoItemDTO?> GetByIdAsync(int id);
        Task<TodoItemDTO> CreateAsync(TodoItemDTO dto);
        Task<bool> UpdateAsync(int id, TodoItemDTO dto);
        Task<bool> DeleteAsync(int id);
    }
}
