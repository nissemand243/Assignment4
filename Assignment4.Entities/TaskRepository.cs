using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using Assignment4.Core;
using System.Linq; 
using Microsoft.Extensions.Configuration;

namespace Assignment4.Entities
{
    public class TaskRepository : ITaskRepository
    {

        private readonly SqlConnection _connection; 
        private readonly IKanbanContext _context; 

         public TaskRepository(SqlConnection connection, IKanbanContext context)
        {
            _connection = connection; 
            _context = context; 
        }

        public (Response Response, int TaskId) Create(TaskCreateDTO task)
        {
            throw new System.NotImplementedException();
        }

        public Response Delete(int taskId)
        {
            var task = _context.Tasks.Find(taskId); 
            if(task == null)
            {
                return Response.NotFound; 
            }
            if(task.State == State.New) 
            {
                _context.Tasks.Remove(task); 
                _context.saveChanges(); 
                return Response.Deleted; 
            } 
            else if(task.State == State.Active)
            {
                task.State = State.Removed; 
                _context.saveChanges(); 
                return Response.Updated;
            }
            else 
            {   
                _context.saveChanges(); 
                return Response.Conflict; 
            }
            
        }

        public TaskDetailsDTO Read(int taskId)
        {
    
           /*  var task = from t in _context.Tasks
                       where t.Id == taskId 
                       select new TaskDetailsDTO(
                           t.Id, 
                           t.Title, 
                           t.Description,
                           new System.DateTime(), 
                           t.AssignedTo.Name, 
                           t.Tags.Select(t => t.name).ToList(), 
                           t.
                           
                       ); 
         
            return task.FirstOrDefault();  */
            throw new System.NotImplementedException(); 
        }

        public IReadOnlyCollection<TaskDTO> ReadAll()
        {
            throw new System.NotImplementedException();
        }

        public IReadOnlyCollection<TaskDTO> ReadAllByState(State state)
        {
            throw new System.NotImplementedException();
        }

        public IReadOnlyCollection<TaskDTO> ReadAllByTag(string tag)
        {
            throw new System.NotImplementedException();
        }

        public IReadOnlyCollection<TaskDTO> ReadAllByUser(int userId)
        {
            throw new System.NotImplementedException();
        }

        public IReadOnlyCollection<TaskDTO> ReadAllRemoved()
        {
            throw new System.NotImplementedException();
        }

        public Response Update(TaskUpdateDTO task)
        {
            throw new System.NotImplementedException();
        }
    }
}
