using System.Linq;

namespace Assignment05.Models
{
    public interface ITaskRepository
    {
        (Response response, int createdId) Create(TaskCreateDTO task);
        IQueryable<TaskListDTO> Read(bool includeRemoved = false);
        TaskDetailsDTO Read(int taskId);
        Response Update(TaskUpdateDTO task);
        Response Delete(int taskId);
    }
}
