using System.Collections.Generic;
using System.Linq;
using Assignment05.Entities;
using Microsoft.EntityFrameworkCore;
using static Assignment05.Entities.State;
using static Assignment05.Models.Response;
using System.Threading.Tasks;

namespace Assignment05.Models
{
    public class TaskRepository : ITaskRepository
    {
        private IKanbanContext _context;

        public TaskRepository(IKanbanContext context)
        {
            _context = context;
        }

        public async System.Threading.Tasks.Task<(Response response, int createdId)> Create(TaskCreateDTO task)
        {
            if (task.AssignedToId.HasValue && !UserExists(task.AssignedToId.Value))
            {
                return (Conflict, 0);
            }

            var entity = new Entities.Task
            {
                Title = task.Title,
                Description = task.Description,
                AssignedToId = task.AssignedToId,
                Tags = MapTags(0, task.Tags).ToList()
            };

            _context.Tasks.Add(entity);
            await _context.SaveChangesAsync();

            return (Created, entity.Id);
        }

        public async System.Threading.Tasks.Task<TaskDetailsDTO> Read(int taskId)
        {
            var task = from t in _context.Tasks
                       where t.Id == taskId
                       select new TaskDetailsDTO
                       {
                           Id = t.Id,
                           Title = t.Title,
                           Description = t.Description,
                           AssignedToId = t.AssignedToId,
                           AssignedToName = t.AssignedTo.Name,
                           State = t.State,
                           Tags = t.Tags.ToDictionary(a => a.TagId, a => a.Tag.Name)
                       };

            return await task.FirstOrDefaultAsync();
        }

        public IQueryable<TaskListDTO> Read(bool includeRemoved = false)
        {
            return from t in _context.Tasks
                   where includeRemoved || t.State != Removed
                   select new TaskListDTO
                   {
                       Id = t.Id,
                       Title = t.Title,
                       AssignedToId = t.AssignedToId,
                       AssignedToName = t.AssignedTo.Name,
                       State = t.State,
                       Tags = t.Tags.ToDictionary(a => a.TagId, a => a.Tag.Name)
                   };
        }

        public async System.Threading.Tasks.Task<Response> Update(TaskUpdateDTO task)
        {
            var entity = _context.Tasks.Include(t => t.Tags).FirstOrDefault(t => t.Id == task.Id);

            if (entity == null)
            {
                return NotFound;
            }

            if (task.AssignedToId.HasValue && !UserExists(task.AssignedToId.Value))
            {
                return (Conflict);
            }

            entity.Title = task.Title;
            entity.Description = task.Description;
            entity.AssignedToId = task.AssignedToId;
            entity.State = task.State;
            entity.Tags = MapTags(task.Id, task.Tags).ToList();

            await _context.SaveChangesAsync();
            return Updated;
        }

        public async System.Threading.Tasks.Task<Response> Delete(int taskId)
        {
            var entity = _context.Tasks.Find(taskId);

            if (entity == null)
            {
                return NotFound;
            }

            Response response;

            switch (entity.State)
            {
                case New:
                    _context.Tasks.Remove(entity);
                    response = Deleted;
                    break;
                case Active:
                    entity.State = Removed;
                    response = Deleted;
                    break;
                default:
                    response = Conflict;
                    break;
            }

            await _context.SaveChangesAsync();

            return response;
        }

        private bool UserExists(int userId) => _context.Users.Any(u => u.Id == userId);

        private IEnumerable<TaskTag> MapTags(int taskId, IEnumerable<string> tags)
        {
            foreach (var tag in tags)
            {
                var entity = _context.Tags.FirstOrDefault(t => t.Name == tag) ?? new Tag { Name = tag };

                yield return new TaskTag { TaskId = taskId, Tag = entity };
            }
        }
    }
}
