using Microsoft.AspNetCore.Mvc;
using DAL;
using SoftLine.ViewModels.Task;
using Service.Interfaces;
using Domain.Filters.Task;

namespace SoftLine.Controllers
{
    public class TaskController : Controller
    {
        private readonly ITaskService _taskService;

        public TaskController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Tasks()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateTaskViewModel model)
        {
            var response = await _taskService.Create(model);

            if (response.StatusCode == Domain.Enum.StatusCode.OK)
            {
                return Ok(new { description = response.Description });
            }
            return BadRequest(new { description = response.Description });
        }

        [HttpGet]
        public async Task<IActionResult> TaskHandler(TaskFIlter filter)
        {
            var response = await _taskService.GetTasks(filter);
            return Json(new {data = response.Data });
        }

        [HttpGet]
        public async Task<IActionResult> GetTask(string name)
        {
            var response = await _taskService.GetTask(name);
            if (response.StatusCode == Domain.Enum.StatusCode.OK)
            {
                return Json(new { data = response.Data });
            }
            return BadRequest(new { description = response.Description });
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteTask(string name)
        {
            var response = await _taskService.Delete(name);

            if (response.StatusCode == Domain.Enum.StatusCode.OK)
            {
                return Ok(new { description = response.Description });
            }
            return BadRequest(new { description = response.Description });
        }

        [HttpPut]
        public async Task<IActionResult> Update(CreateTaskViewModel model, string currentName)
        {
            var response = await _taskService.Update(model, currentName);

            if (response.StatusCode == Domain.Enum.StatusCode.OK)
            {
                return Ok(new { description = response.Description });
            }
            return BadRequest(new { description = response.Description });
        }
    }
}