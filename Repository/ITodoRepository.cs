namespace TodoApi.Repository
{
    public interface ITodoRepository

    {
        Task<List<Todo>> GetAllAsync();
        Task<List<Todo>> GetPagedAsync(int page, int pageSize);
        Task<int> CountAsync();
        Task<Todo?> GetByIdAsync(int id);
        Task AddAsync(Todo todo);
        Task UpdateAsync(Todo todo);
        Task DeleteAsync(Todo todo);
    }
}
