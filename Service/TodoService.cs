using TodoApi.Repository;

namespace TodoApi.Service
{
    public class TodoService : ITodoService
    {
        private readonly ITodoRepository _repository;
        private readonly ILogger<TodoService> _logger;

        public TodoService(ITodoRepository repository, ILogger<TodoService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<List<TodoItemDTO>> GetAllAsync()
        {
            var todos = await _repository.GetAllAsync();
            _logger.LogInformation("Getting items at {RequestTime}", DateTime.Now);
            return todos.Select(x => new TodoItemDTO(x)).ToList();
        }

        public async Task<TodoItemDTO?> GetByIdAsync(int id)
        {
            var todo = await _repository.GetByIdAsync(id);
            return todo is null ? null : new TodoItemDTO(todo);
        }

        public async Task<TodoItemDTO> CreateAsync(TodoItemDTO dto)
        {
            var entity = new Todo
            {
                Name = dto.Name,
                IsComplete = dto.IsComplete
            };

            await _repository.AddAsync(entity);

            return new TodoItemDTO(entity);
        }

        public async Task<bool> UpdateAsync(int id, TodoItemDTO dto)
        {
            var todo = await _repository.GetByIdAsync(id);
            if (todo is null) return false;

            todo.Name = dto.Name;
            todo.IsComplete = dto.IsComplete;

            await _repository.UpdateAsync(todo);
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var todo = await _repository.GetByIdAsync(id);
            if (todo is null) return false;

            await _repository.DeleteAsync(todo);
            return true;
        }
    }
}
