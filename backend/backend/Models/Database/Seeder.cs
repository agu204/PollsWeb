using backend.Helpers;
using backend.Models.Database.Entities;


namespace backend.Models.Database;

public class Seeder
{
    private readonly PollsContext _context;

    public Seeder(PollsContext context)
    {
        _context = context;
    }

    public async Task SeedAsync()
    {
        await SeedUsersAsync();
        

        await _context.SaveChangesAsync();
    }

    private async Task SeedUsersAsync()
    {
        User[] users = [
                new User {
                    Name = "Manuel" ,
                    Email = "manuel@gmail.com",
                    Password = PasswordHelper.Hash("123456"),
                    Address = "Su casa",
                    Role = "Admin",
                    
                },
                new User {
                    Name = "Javier" ,
                    Email = "javier@gmail.com",
                    Password = PasswordHelper.Hash("123456"),
                    Address = "Su casa",
                    Role = "Admin",
                    
                },
                new User {
                    Name = "Juanma" ,
                    Email = "juanma@gmail.com",
                    Password = PasswordHelper.Hash("123456"),
                    Address = "Su casa",
                    Role = "User",
                    
                },
                new User {
                    Name = "Agustin" ,
                    Email = "agustin@gmail.com",
                    Password = PasswordHelper.Hash("123456"),
                    Address = "Su casa",
                    Role = "User",
                    
                },
                new User {
                    Name = "Pepe" ,
                    Email = "pepe@gmail.com",
                    Password = PasswordHelper.Hash("123456"),
                    Address = "Su casa",
                    Role = "User",
                    
                }
            ];

        await _context.Users.AddRangeAsync(users);
    }
            

}
