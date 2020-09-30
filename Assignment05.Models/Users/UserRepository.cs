using System.Linq;
using System;
using System.Threading.Tasks;
using System.Data.Entity;
using Assignment05.Entities;

using System.Collections.Generic;

namespace Assignment05.Models
{
    public class UserRepository : IUserRepository
    {
        private KanbanContext _context;
        public UserRepository(KanbanContext context)
        {
            _context = context;
        }

        public async Task<(Response response, int userId)> CreateAsync(UserCreateDTO user)
        {

            if (user.EmailAddress == null || user.Name == null) 
            {
                return (Response.BadRequest, -1);
            }


            User newUser = new User {
                Name = user.Name,
                EmailAddress = user.EmailAddress
            };
            
            try
            {
                _context.Users.Add(newUser);
                await _context.SaveChangesAsync();
            }
            catch
            {
                return (Response.Conflict, -1);
            }

            return (Response.Created, newUser.Id);
        }

        public async Task<Response> DeleteAsync(int userId, bool force = false)
        {
            var userQuery = from e in _context.Users where e.Id == userId select e;
            if(!userQuery.Any()) return Response.NotFound;
            var user = userQuery.FirstOrDefault();

            var tasks = from task in _context.Tasks
                        where task.AssignedToId == userId
                        select task;

            if(force || !tasks.Any())
            {
                tasks.ToList().ForEach(t => t.AssignedTo = null);

                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
                return Response.Deleted;
            }
            else
            {
                return Response.Conflict;
            }        
        }

        public IQueryable<UserListDTO> ReadAsync()
        {
            throw new System.NotImplementedException();
        }

        public Task<UserDetailsDTO> ReadAsync(int userId)
        {
            throw new System.NotImplementedException();
        }

        public Task<Response> UpdateAsync(UserUpdateDTO user)
        {
            throw new System.NotImplementedException();
        }
    }



}