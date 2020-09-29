using System.Linq;
using Assignment05.Entities;
using Microsoft.EntityFrameworkCore;
using static Assignment05.Entities.State;
using static Assignment05.Models.Response;

namespace Assignment05.Models
{
    public class TagRepository : ITagRepository
    {
        private readonly IKanbanContext _context;

        public TagRepository(IKanbanContext context)
        {
            _context = context;
        }

        public (Response response, int taskId) Create(TagCreateDTO tag)
        {
            var tagExists = _context.Tags.Any(t => t.Name == tag.Name);

            if (tagExists)
            {
                return (Conflict, 0);
            }

            var entity = new Tag
            {
                Name = tag.Name
            };

            _context.Tags.Add(entity);
            _context.SaveChanges();

            return (Created, entity.Id);
        }

        public IQueryable<TagDTO> Read()
        {
            return from t in _context.Tags
                   select new TagDTO
                   {
                       Id = t.Id,
                       Name = t.Name,
                       New = t.Tasks.Count(a => a.Task.State == New),
                       Active = t.Tasks.Count(a => a.Task.State == Active),
                       Resolved = t.Tasks.Count(a => a.Task.State == Resolved),
                       Closed = t.Tasks.Count(a => a.Task.State == Closed),
                       Removed = t.Tasks.Count(a => a.Task.State == New)
                   };
        }

        public TagDTO Read(int tagId)
        {
            var tags = from t in _context.Tags
                       where t.Id == tagId
                       select new TagDTO
                       {
                           Id = t.Id,
                           Name = t.Name,
                           New = t.Tasks.Count(a => a.Task.State == New),
                           Active = t.Tasks.Count(a => a.Task.State == Active),
                           Resolved = t.Tasks.Count(a => a.Task.State == Resolved),
                           Closed = t.Tasks.Count(a => a.Task.State == Closed),
                           Removed = t.Tasks.Count(a => a.Task.State == New)
                       };

            return tags.FirstOrDefault();
        }

        public Response Update(TagUpdateDTO tag)
        {
            var tagExists = _context.Tags.Any(t => t.Id != tag.Id && t.Name == tag.Name);

            if (tagExists)
            {
                return Conflict;
            }

            var entity = _context.Tags.Find(tag.Id);

            if (entity == null)
            {
                return NotFound;
            }

            entity.Name = tag.Name;

            _context.SaveChanges();

            return Updated;
        }

        public Response Delete(int tagId, bool force = false)
        {
            var entity = _context.Tags.Include(t => t.Tasks).FirstOrDefault(t => t.Id == tagId);

            if (entity == null)
            {
                return NotFound;
            }

            if (!force && entity.Tasks.Any())
            {
                return Conflict;
            }

            _context.Tags.Remove(entity);
            _context.SaveChanges();

            return Deleted;
        }
    }
}
