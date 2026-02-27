using TodoApi.Common;
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

        public async Task<List<TodoItemDTO>> GetAllAsyncNoPages()
        {
            var todos = await _repository.GetAllAsync();
            LogHelper.loginfo.Info("获取所有Todo。");
            //_logger.LogInformation("Getting items at {RequestTime}", DateTime.Now);
            return todos.Select(x => new TodoItemDTO(x)).ToList();
        }

        public async Task<PagedResult<TodoItemDTO>> GetAllAsync(int page, int pageSize)
        {
            var safePage = page <= 0 ? 1 : page;    
            var safePageSize = pageSize <= 0 ? 10 : pageSize;
            var todos = await _repository.GetPagedAsync(safePage, safePageSize);
            var total = await _repository.CountAsync();

            return new PagedResult<TodoItemDTO>
            {
                Items = todos.Select(x => new TodoItemDTO(x)).ToList(),
                TotalCount = total,
                Page = safePage,
                PageSize = safePageSize
            };
        }

        public async Task<TodoItemDTO?> GetByIdAsync(int id)
        {
            var todo = await _repository.GetByIdAsync(id);
            LogHelper.loginfo.Info($"获取ID为{id}的Todo。");
            return todo is null ? null : new TodoItemDTO(todo);
        }


        public async Task<TodoItemDTO> CreateAsync(TodoItemDTO dto)
        {
            var entity = new Todo
            {
                Name = dto.Name,
                IsComplete = dto.IsComplete,
                Priority = dto.Priority
            };

            await _repository.AddAsync(entity);
            LogHelper.loginfo.Info($"添加名字为为{dto.Name}的Todo。");
            return new TodoItemDTO(entity);
        }

        public async Task<bool> UpdateAsync(int id, TodoItemDTO dto)
        {
            var todo = await _repository.GetByIdAsync(id);
            if (todo is null) return false;

            todo.Name = dto.Name;
            todo.IsComplete = dto.IsComplete;
            todo.Priority = dto.Priority;
            LogHelper.loginfo.Info($"修改名字为{dto.Name}的Todo。");
            await _repository.UpdateAsync(todo);
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var todo = await _repository.GetByIdAsync(id);
            if (todo is null) return false;
            LogHelper.loginfo.Info($"删除ID为{id}的Todo。");
            await _repository.DeleteAsync(todo);
            return true;
        }

        
    }
}
