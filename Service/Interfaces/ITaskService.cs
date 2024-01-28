using Domain.Entities;
using Domain.Filters.Task;
using Domain.ViewModels.Task;
using SoftLine.Response;
using SoftLine.ViewModels.Task;

namespace Service.Interfaces
{
	public interface ITaskService
	{
        Task<IBaseResponse<TaskEntity>> Create(CreateTaskViewModel model);

        Task<IBaseResponse<IEnumerable<TaskViewModel>>> GetTasks(TaskFIlter filter);

        Task<IBaseResponse<bool>> Update(CreateTaskViewModel model, string name);

        Task<IBaseResponse<TaskViewModel>> GetTask(string name);

        Task<IBaseResponse<bool>> Delete(string name);
    }
}