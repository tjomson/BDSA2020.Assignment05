using Microsoft.EntityFrameworkCore;

namespace Assignment05.Entities
{
    public interface IKanbanContext
    {
        DbSet<Task> Tasks { get; set; }
        DbSet<Tag> Tags { get; set; }
        DbSet<TaskTag> TaskTags { get; set; }
        DbSet<User> Users { get; set; }

        int SaveChanges();
    }
}
