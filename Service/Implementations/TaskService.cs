using DAL.Interfaces;
using Domain.Entities;
using Domain.Enum;
using Domain.Extensions;
using Domain.Filters.Task;
using Domain.ViewModels.Task;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Service.Interfaces;
using SoftLine.Response;
using SoftLine.ViewModels.Task;

namespace Service.Implementations
{
    public class TaskService : ITaskService
    {
        private readonly IBaseRepository<TaskEntity> _taskRepository;
        private readonly IBaseRepository<StatusEntity> _statusRepository;
        private ILogger<TaskService> _logger;

        public TaskService(IBaseRepository<TaskEntity> taskRepository, IBaseRepository<StatusEntity> statusRepository,
            ILogger<TaskService> logger)
        {
            _taskRepository = taskRepository;
            _statusRepository = statusRepository;
            _logger = logger;
        }

        public async Task<IBaseResponse<TaskEntity>> Create(CreateTaskViewModel model)
        {
            try
            {
                model.Validate();

                _logger.LogInformation(message: $"Запрос на создание задачи - {model.Name}");

                var task = await _taskRepository.GetAll()
                    .FirstOrDefaultAsync(x => x.Name == model.Name);

                if (task != null)
                {
                    return new BaseResponse<TaskEntity>()
                    {
                        Description = "Задача с таким названием уже есть",
                        StatusCode = StatusCode.TaskIsHasAlready,
                    };
                }

                task = new TaskEntity()
                {
                    Name = model.Name,
                    Description = model.Description,
                    StatusId = (long)model.StatusName,
                    Created = DateTime.Now,
                };

                await _taskRepository.Create(task);

                _logger.LogInformation($"Задача добавлена: {task.Name} {task.Created}");
                return new BaseResponse<TaskEntity>()
                {
                    Description = "Задача добавлена",
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[TaskService.Create]: {ex.Message}");
                return new BaseResponse<TaskEntity>()
                {
                    Description = $"{ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<IBaseResponse<bool>> Delete(string name)
        {
            try
            {
                _logger.LogInformation(message: $"Запрос на удаление задачи - {name}");

                var task = await _taskRepository.GetAll().FirstOrDefaultAsync(x => x.Name == name);
                if (task == null)
                {
                    return new BaseResponse<bool>()
                    {
                        Description = "Задача не найдена",
                        StatusCode = StatusCode.TaskNotFound
                    };
                }

                await _taskRepository.Delete(task);

                return new BaseResponse<bool>()
                {
                    Description = "Задача удалена",
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[TaskService.Delete]: {ex.Message}");
                return new BaseResponse<bool>()
                {
                    Description = $"{ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<IBaseResponse<IEnumerable<TaskViewModel>>> GetTasks(TaskFIlter filter)
        {
            try
            {
                _logger.LogInformation(message: "Запрос на получение данных");

                var tasks = await _taskRepository.GetAll()
                    .WhereIf(!string.IsNullOrWhiteSpace(filter.Name), x => x.Name == filter.Name)
                    .WhereIf(filter.StatusName.HasValue, x => x.Status.Name == filter.StatusName.GetDisplayName())
                    .Select(x => new TaskViewModel()
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Description = x.Description,
                        Status = x.Status.Name,
                    })
                    .ToListAsync();

                return new BaseResponse<IEnumerable<TaskViewModel>>()
                {
                    Data = tasks,
                    StatusCode = StatusCode.OK
                };
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, $"[TaskService.Create]: {ex.Message}");
                return new BaseResponse<IEnumerable<TaskViewModel>>()
                {
                    Description = $"{ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<IBaseResponse<TaskViewModel>> GetTask(string name)
        {
            try
            {             
                var task = await _taskRepository.GetAll().FirstOrDefaultAsync(x => x.Name == name);
                if (task == null)
                {
                    return new BaseResponse<TaskViewModel>()
                    {
                        Description = "Задача не найдена",
                        StatusCode = StatusCode.TaskNotFound
                    };
                }

                var taskModel = new TaskViewModel()
                {
                    Id = task.Id,
                    Name = task.Name,
                    Description = task.Description,
                    Status = task.Status.Name,
                };

                return new BaseResponse<TaskViewModel>()
                {
                    Data = taskModel,
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[TaskService.Get]: {ex.Message}");
                return new BaseResponse<TaskViewModel>()
                {
                    Description = $"{ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<IBaseResponse<bool>> Update(CreateTaskViewModel model, string name)
        {
            try
            {
                model.Validate();

                _logger.LogInformation(message: $"Запрос на редактирование задачи - {model.Name}");

                var task = await _taskRepository.GetAll()
                    .FirstOrDefaultAsync(x => x.Name == name);

                if (task == null)
                {
                    return new BaseResponse<bool>()
                    {
                        Description = "Задача не найдена",
                        StatusCode = StatusCode.TaskNotFound
                    };
                }

                task.Name = model.Name;
                task.Description = model.Description;
                task.Status = await _statusRepository.GetAll()
                    .FirstOrDefaultAsync(x => x.Name == model.StatusName.GetDisplayName());

                if (task.Status == null)
                {
                    return new BaseResponse<bool>()
                    {
                        Description = "Задача не найдена",
                        StatusCode = StatusCode.TaskNotFound
                    };
                }

                task.StatusId = task.Status.Id;
              
                await _taskRepository.Update(task);

                _logger.LogInformation($"Задача обновлена: {task.Name} {task.Created}");
                return new BaseResponse<bool>()
                {
                    Description = "Задача обновлена",
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[TaskService.Update]: {ex.Message}");
                return new BaseResponse<bool>()
                {
                    Description = $"{ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }
    }
}
