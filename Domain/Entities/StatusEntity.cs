using Domain.Enum;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class StatusEntity
    {
        public long Id { get; set; }
        public string Name { get; set; }

        public ICollection<TaskEntity> Tasks { get; set; }
    }
}
