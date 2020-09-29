using System.Linq;

namespace Assignment05.Models
{
    public interface ITagRepository
    {
        (Response response, int taskId) Create(TagCreateDTO tag);
        IQueryable<TagDTO> Read();
        TagDTO Read(int tagId);
        Response Update(TagUpdateDTO tag);
        Response Delete(int tagId, bool force = false);
    }
}
