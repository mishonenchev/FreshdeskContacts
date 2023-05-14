using FreshdeskContacts.Models;
using FreshdeskContacts.Repositories;
using FreshdeskContacts.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;
using FreshdeskContacts;
using FreshdeskContacts.Database;

namespace UnitTests
{
    public class GithubUserServiceTests
    {
        
        public GithubUserServiceTests()
        {
        }
        
        [Fact]
        public void AddUser_ShouldAddUserToDatabase()
        {
            var _testsStartup = new TestsStartup();
            _testsStartup.Dispose();
            // Arrange
            var user = new GithubUser
            {
                Login = "testlogin",
                Name = "The Name",
                Email = "testlogin@github.com",
                Description = "A test user",
                HtmlUrl = "https://github.com/testlogin"
            };

            // Act
            var dbContext = _testsStartup.GetService<MyDbContext>();
            var repo = new GithubUserRepository(dbContext);
            var service = new GithubUserService(repo);
            service.AddUser(user);

            // Assert
            var addedUser = service.GetUserByLogin("testlogin");
            Assert.NotNull(addedUser);
            Assert.Equal("testlogin", addedUser.Login);
        }

        [Fact]
        public void AddUser_ShouldThrowException_WhenUserAlreadyExists()
        {
            var _testsStartup = new TestsStartup();
            _testsStartup.Dispose();
            // Arrange
            var existingUser = new GithubUser
            {
                Login = "testlogin",
                Name = "The Name",
                Email = "testlogin@github.com",
                Description = "A test user",
                HtmlUrl = "https://github.com/testlogin"
            };
            var dbContext = _testsStartup.GetService<MyDbContext>();
            var repo = new GithubUserRepository(dbContext);
            var service = new GithubUserService(repo);
            service.AddUser(existingUser);

            var newUser = new GithubUser
            {
                Login = "testlogin",
                Name = "Another Name",
                Email = "another-testlogin@github.com",
                Description = "Another test user",
                HtmlUrl = "https://github.com/another-testlogin"
            };

            // Act & Assert
            Assert.Throws<Exception>(() => service.AddUser(newUser));
        }

        [Fact]
        public void UpdateUser_ShouldUpdateUserInDatabase()
        {
            var _testsStartup = new TestsStartup();
            _testsStartup.Dispose();
            // Arrange
            var user = new GithubUser
            {
                Login = "testlogin",
                Name = "The Name",
                Email = "testlogin@github.com",
                Description = "A test user",
                HtmlUrl = "https://github.com/testlogin"
            };
            var dbContext = _testsStartup.GetService<MyDbContext>();
            var repo = new GithubUserRepository(dbContext);
            var service = new GithubUserService(repo);
            service.AddUser(user);

            var updatedUser = new GithubUser
            {
                Login = "testlogin",
                Name = "Updated Name",
                Email = "updated-testlogin@github.com",
                Description = "An updated test user",
                HtmlUrl = "https://github.com/updated-testlogin"
            };

            // Act
            service.UpdateUser(user.Id, updatedUser);

            // Assert
            var retrievedUser = service.GetUserByLogin("testlogin");
            Assert.NotNull(retrievedUser);
            Assert.Equal("Updated Name", retrievedUser.Name);
            Assert.Equal("updated-testlogin@github.com", retrievedUser.Email);
        }

        [Fact]
        public void UpdateUser_ShouldThrowException_WhenUserDoesNotExist()
        {
            var _testsStartup = new TestsStartup();
            _testsStartup.Dispose();
            // Arrange
            var user = new GithubUser
            {
                Login = "nonexistent_user",
                Name = "Nonexistent User",
                Email = "nonexistent_user@example.com",
                Description = "I do not exist",
                HtmlUrl = "https://github.com/nonexistent_user",
            };
            var dbContext = _testsStartup.GetService<MyDbContext>();
            var repo = new GithubUserRepository(dbContext);
            var service = new GithubUserService(repo);

            // Act & Assert
            Assert.Throws<Exception>(() => service.UpdateUser(1, user));
        }
    }
}