

using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class TaskEntity
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime Created { get; set; }       

        public long StatusId { get; set; }
        public StatusEntity Status { get; set; }
    }
}
