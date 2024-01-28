

using Domain.Enum;
using System.ComponentModel.DataAnnotations;

namespace Domain.ViewModels.Task
{
    public class TaskViewModel
    {
        public long Id { get; set; }

        [Display(Name = "Наименование")]
        public string Name { get; set; }

        [Display(Name = "Описание")]
        public string Description { get; set; }

        [Display(Name = "Статус")]
        public string Status { get; set; }

        [Display(Name = "Время создания")]
        public string Created { get; set; }
    }
}
