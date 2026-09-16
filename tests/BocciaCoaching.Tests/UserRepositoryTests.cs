using BocciaCoaching.Data;
using BocciaCoaching.Models.DTO.User;
using BocciaCoaching.Models.Entities;
using BocciaCoaching.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace BocciaCoaching.Tests
{
    public class UserRepositoryTests
    {
        [Fact]
        public async Task UpdatePassword_ConEmailSinUserId_ActualizaLaContrasena()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            await using var context = new ApplicationDbContext(options);
            var user = new User
            {
                UserId = Guid.NewGuid(),
                Email = "usuario@example.com",
                Password = BCrypt.Net.BCrypt.HashPassword("contraseña-anterior")
            };
            context.Users.Add(user);
            await context.SaveChangesAsync();
            var repository = new UserRepository(context);

            var result = await repository.UpdatePassword(new UpdatePasswordDto
            {
                Email = user.Email,
                NewPassword = "contraseña-nueva",
                ConfirmPassword = "contraseña-nueva"
            });

            result.Success.Should().BeTrue();
            BCrypt.Net.BCrypt.Verify("contraseña-nueva", user.Password).Should().BeTrue();
        }
    }
}
