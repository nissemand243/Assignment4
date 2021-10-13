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

        private readonly IKanbanContext _context; 

         public TaskRepository(IKanbanContext context)
        {
            _context = context; 
        }

        public (Response Response, int TaskId) Create(TaskCreateDTO task)
        {
            //mangler business rule 2.5 = create/update task must allow for editing tags?
            //2.5 betyder: "når du opretter/opdaterer en task, skal der kunne tilføjes/fjernes tags?"
            var taskToBeAdded = new Task
            {
                Title = task.Title,
                //AssignedTo =  _context.Users.Find(task.AssignedToId), //det her virker ikke... 
                Description = task.Description, 
                State = State.New, 
                Created = System.DateTime.UtcNow, 
                StateUpdated = System.DateTime.UtcNow, 
                Tags = GetTags(task.Tags).ToList() 
            };

            _context.Tasks.Add(taskToBeAdded);
            _context.saveChanges(); 

            return (Response.Created, taskToBeAdded.Id);
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
            var task = from t in _context.Tasks
                       where t.Id == taskId
                       select new TaskDetailsDTO(
                           t.Id, 
                           t.Title, 
                           t.Description,
                           t.Created, 
                           t.AssignedTo.Name,
                           t.Tags.Select(t => t.name).ToList(), 
                           t.State,
                           t.StateUpdated
                       );

            return task.FirstOrDefault();
        }

        public IReadOnlyCollection<TaskDTO> ReadAll() // DENNE ER IKKE TESTET
        {
            return _context.Tasks.Select(t => new TaskDTO(t.Id, t.Title, t.AssignedTo.Name, t.Tags.Select(ta => ta.name).ToList(), t.State)).ToList().AsReadOnly();
           
            /*
            //there might be a more clever way to do this 
            var returnList = new List<TaskDTO>(); 
            foreach(var task in _context.Tasks)
            {
                var dto = new TaskDTO(task.Id, task.Title, task.AssignedTo.Name, task.Tags.Select(t => t.name).ToList(), task.State);
                returnList.Add(dto);
            }
            return returnList; */
        }

        public IReadOnlyCollection<TaskDTO> ReadAllByState(State state)
        {
            return _context.Tasks.Where(t => t.State == state).Select(t => new TaskDTO(t.Id, t.Title, t.AssignedTo.Name, t.Tags.Select(ta => ta.name).ToList(), t.State)).ToList().AsReadOnly();
        }

        public IReadOnlyCollection<TaskDTO> ReadAllByTag(string tag)
        {
            return _context.Tasks.Where(t => t.Tags.Select(ta => ta.name).ToList().Contains(tag)).Select(t => new TaskDTO(t.Id, t.Title, t.AssignedTo.Name, t.Tags.Select(ta => ta.name).ToList(), t.State)).ToList().AsReadOnly();
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
            var taskToBeUpdated = _context.Tasks.Find(task.Id); 
            if(taskToBeUpdated == null)
            {
                return Response.NotFound; 
            }
            taskToBeUpdated.Title = task.Title; 
            taskToBeUpdated.Description = task.Description; 
            //taskToBeUpdated.AssignedTo = _context.Users.Find(task.AssignedToId); 
            taskToBeUpdated.State = task.State; 
            taskToBeUpdated.StateUpdated = System.DateTime.UtcNow; 
            taskToBeUpdated.Tags = GetTags(task.Tags).ToList(); 

            _context.saveChanges();
            return Response.Updated;
            
        }


        public IEnumerable<Tag> GetTags(IEnumerable<string> tags)
        {
            var existing = _context.Tags.Where(t => tags.Contains(t.name)).ToDictionary(t => t.name);
            foreach(var tag in tags)
            {
                yield return existing.TryGetValue(tag, out var t) ? t : new Tag {name = tag}; 
            }
        }
    }
}
