using System.Collections.Generic;
using Assignment4.Core;

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
            //should this tage højde for that a user can be assigned to a task that is resolved? dunno
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
            else 
            {
                return Response.Conflict;
            }
        }

        public UserDTO Read(int userId)
        {
            throw new System.NotImplementedException();
        }

        public IReadOnlyCollection<UserDTO> ReadAll()
        {
            throw new System.NotImplementedException();
        }

        public Response Update(UserUpdateDTO user)
        {
            throw new System.NotImplementedException();
        }
    }
}