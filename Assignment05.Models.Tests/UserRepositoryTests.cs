using System;
using System.Linq;
using Assignment05.Entities;
using Assignment05.Models;
using Assignment05.Models.Tests;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Xunit;
using static Assignment05.Entities.State;
using static Assignment05.Models.Response;

namespace Assignment05.Models.Tests
{


    public class UserRepositoryTests
    {
        private readonly SqliteConnection _connection;
        private readonly KanbanContext _context;
        private readonly UserRepository _repository;

        public UserRepositoryTests()
        {
            _connection = new SqliteConnection("Filename=:memory:");
            _connection.Open();
            var builder = new DbContextOptionsBuilder<KanbanContext>().UseSqlite(_connection);
            _context = new KanbanTestContext(builder.Options);
            _context.Database.EnsureCreated();
            _repository = new UserRepository(_context);
        }
        
        [Fact]
        public async void CreateAsync_given_proper_user()
        {
            var dto = new UserCreateDTO
            {
                Name = "user3",
                EmailAddress= "user3@kanban.com"
            };

            var (response, id) = await _repository.CreateAsync(dto);
            var createdUser = _context.Users.Find(id);

            Assert.Equal(4, id);
            Assert.Equal("user3", createdUser.Name);
            Assert.Equal("user3@kanban.com", createdUser.EmailAddress);
        }

        [Fact]
        public async void CreateAsync_given_duplicate_email()
        {
            var dto = new UserCreateDTO
            {
                Name = "user3",
                EmailAddress= "user2@kanban.com"
            };

            var (response, id) = await _repository.CreateAsync(dto);

            Assert.Equal(-1, id);
            Assert.Equal(Response.Conflict, response);
            Assert.Null(_context.Users.Find(id));

        }

        [Fact]
        public async void DeleteAsync_given_user_without_tasks_no_force()
        {
            var response = await _repository.DeleteAsync(3);

            Assert.Equal(response, Response.Deleted);
            Assert.Null(_context.Users.Find(3));
        }

        [Fact]
        public async void DeleteAsync_given_user_with_tasks_returns_conflict()
        {
            var response = await _repository.DeleteAsync(1);

            Assert.Equal(response, Response.Conflict);
            Assert.NotNull(_context.Users.Find(1));
        }

        [Fact]
        public async void DeleteAsync_given_user_with_tasks_deletes_with_force()
        {
            var response = await _repository.DeleteAsync(1, force:true);

            Assert.Equal(response, Response.Deleted);
            Assert.Null(_context.Users.Find(1));

            // Test that a task is unassigned
            Assert.Null(_context.Tasks.Find(2).AssignedTo);
        }
    }
}
