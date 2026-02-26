using Microsoft.EntityFrameworkCore;

namespace TodoApi.Repository
{
    public class TodoRepository: ITodoRepository
    {
        private readonly TodoDb _db;
        public TodoRepository(TodoDb db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }

        public async Task<List<Todo>> GetAllAsync()
        {
            return await _db.Todos.ToListAsync();
        }

        public async Task<Todo?> GetByIdAsync(int id)
        {
            return await _db.Todos.FindAsync(id);
        }

        public async Task AddAsync(Todo todo)
        {
            _db.Todos.Add(todo);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateAsync(Todo todo)
        {
            _db.Todos.Update(todo);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(Todo todo)
        {
            _db.Todos.Remove(todo);
            await _db.SaveChangesAsync();
        }

        public async Task<List<Todo>> GetPagedAsync(int page, int pageSize)
        {
            return await _db.Todos.OrderByDescending(t => t.Priority).Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
        }

        public async Task<int> CountAsync()
        {
            return await _db.Todos.CountAsync();
        }
    }
}
