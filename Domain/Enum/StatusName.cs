using System.ComponentModel.DataAnnotations;

namespace Domain.Enum
{
    public enum StatusName
    {
        [Display(Name = "Добавлена")]
        Added = 1,
        [Display(Name = "В работе")]
        InProgress = 2,
        [Display(Name = "Завершена")]
        Complete = 3
    }
}
