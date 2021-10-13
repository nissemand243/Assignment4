using System.Collections.Generic;
using Assignment4.Core;
using System.Linq;


//OBS DENNE KLASSE ER IKKE TESTET! 
namespace Assignment4.Entities
{
    public class UserRepository : IUserRepository
    {
        private readonly IKanbanContext _context;

        public UserRepository(IKanbanContext context)
        {
            _context = context;
        }
        public (Response Response, int UserId) Create(UserCreateDTO user)
        {
            var userToBeAdded = new User
            {
                Name = user.Name, 
                Email = user.Email
            };
            if(_context.Users.Find(user.Email) != null)
            {
                return (Response.Conflict, userToBeAdded.Id); 
            }
            _context.Users.Add(userToBeAdded);
            _context.saveChanges(); 

            return (Response.Created, userToBeAdded.Id);
        }

        public Response Delete(int userId, bool force = false)
        {
            var user = _context.Users.Find(userId); 
            if(user.Tasks.Count == 0)
            {
                _context.Users.Remove(user); 
                return Response.Deleted;
            }
            else if(user.Tasks.Count > 0 && force == true)
            {
                _context.Users.Remove(user);
                return Response.Deleted;
            }
            else if(user.Tasks.Count > 0 && force == false)
            {
                foreach(var task in user.Tasks)
                {
                    if(task.State != State.Closed || task.State != State.Removed)
                    {
                        return Response.Conflict;
                    }
                }
                _context.Users.Remove(user);
                return Response.Deleted;
            }
            else 
            {
                return Response.Conflict;
            }
        }

        public UserDTO Read(int userId)
        {
           var user = from u in _context.Users
                       where u.Id == userId
                       select new UserDTO(
                           u.Id,
                           u.Email,
                           u.Name
                       );

            return user.FirstOrDefault();
        }

        public IReadOnlyCollection<UserDTO> ReadAll()
        {
            return _context.Users.Select(u => new UserDTO(u.Id, u.Email, u.Name)).ToList().AsReadOnly();
        }

        public Response Update(UserUpdateDTO user)
        {
            var userToBeUpdated = _context.Users.Find(user.Id); 
            if(userToBeUpdated == null)
            {
                return Response.NotFound; 
            }
            userToBeUpdated.Id = user.Id;
            userToBeUpdated.Email = user.Email;
            userToBeUpdated.Name = user.Name;

            _context.saveChanges();
            return Response.Updated;
        }
    }
}