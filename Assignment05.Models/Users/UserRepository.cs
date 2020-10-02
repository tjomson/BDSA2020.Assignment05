using System.Linq;
using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
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
            var ids = from user in _context.Users
                        select user.Id;
            
            var asList = ids.ToList();

            var users = from user in _context.Users
                        where asList.Contains(user.Id)
                        select ReadAsync(user.Id).Result;

            return users;
        }
        public async Task<UserDetailsDTO> ReadAsync(int userId)
        {
            var userDetails = from h in _context.Users
                         where h.Id == userId
                         select new UserDetailsDTO
                         {
                             Id = h.Id,
                             Name = h.Name,
                             EmailAddress = h.EmailAddress,
                             Tasks = h.Tasks.Select(t => new UserTaskDTO{Id = t.Id, Title = t.Title, State = t.State}).ToList()
                         };

            return await userDetails.FirstOrDefaultAsync();
        }

        public async Task<Response> UpdateAsync(UserUpdateDTO user)
        {
            var userQuery = from users in _context.Users where users.Id == user.Id select users;
            if (!userQuery.Any()) return Response.NotFound;
            else
            {
                var thisUser = userQuery.FirstOrDefault();
                thisUser.Name = user.Name;
                thisUser.EmailAddress = user.EmailAddress;
                thisUser.Id = user.Id;
                await _context.SaveChangesAsync();
                return Response.Updated;
            }
        }
    }



}