using Domain.Enum;

namespace Domain.Filters.Task
{
    public class TaskFIlter
    {
        public string Name { get; set; }

        public StatusName? StatusName { get; set; }
    }
}
