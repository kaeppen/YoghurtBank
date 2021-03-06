
public class UserRepository : IUserRepository
{
    private readonly IYoghurtContext _context;

    public UserRepository(IYoghurtContext context)
    {
        _context = context;
    }

    public async Task<UserDetailsDTO> CreateAsync(UserCreateDTO user)
    {
        if (user.UserType == "Student")
        {
            return await CreateStudent(user);
        }
        else if (user.UserType == "Supervisor")
        {
            return await CreateSupervisor(user);
        }
        else
        {
            return null;
        }
    }

    public async Task<UserDetailsDTO>? FindUserByIdAsync(int userId)
    {
        var user = await _context.Users.FindAsync(userId);

        if (user == null)

        {
            return null;
        }
        else
        {
            UserDetailsDTO userDetailsDTO = null;

            if (user.GetType() == typeof(Student))
            {
                userDetailsDTO = new UserDetailsDTO
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    UserType = "Student",
                    Email = user.Email
                };
            }
            else
            {
                userDetailsDTO = new UserDetailsDTO
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    UserType = "Supervisor",
                    Email = user.Email
                };
            }

            return (userDetailsDTO);
        }
    }

    public async Task<int?> DeleteAsync(int id)
    {
        var entity = await _context.Users.FindAsync(id);
        if (entity == null)
        {
            return null;
        }
        _context.Users.Remove(entity);
        await _context.SaveChangesAsync();
        return entity.Id;
    }

    public async Task<UserDetailsDTO> FindUserByEmail(string email)
    {

        var user = await _context.Users.Where(u => u.Email == email).Select(u => new UserDetailsDTO
        {
            Id = u.Id,
            UserName = u.UserName,
            UserType = u.GetType().Name,
            Email = u.Email
        }).FirstOrDefaultAsync();

        return user;
    }
    public async Task<IReadOnlyCollection<UserDetailsDTO>> GetAllSupervisors()
    {

        var returnlist = new List<UserDetailsDTO>();
        foreach (var user in _context.Users)
        {
            if (user.GetType() == typeof(Supervisor))
            {
                returnlist.Add(new UserDetailsDTO { Id = user.Id, UserName = user.UserName, UserType = "Supervisor", Email = user.Email });
            }
        }

        return returnlist.AsReadOnly();
    }

    private async Task<UserDetailsDTO> CreateStudent(UserCreateDTO user)
    {
        var entity = new Student
        {
            UserName = user.UserName,
            CollaborationRequests = new List<CollaborationRequest>(),
            Email = user.Email
        };
        await _context.Users.AddAsync(entity);
        await _context.SaveChangesAsync();
        return new UserDetailsDTO
        {
            Id = entity.Id,
            UserName = entity.UserName,
            UserType = "Student",
            Email = entity.Email
        };
    }

    private async Task<UserDetailsDTO> CreateSupervisor(UserCreateDTO user)
    {
        var entity = new Supervisor
        {
            UserName = user.UserName,
            CollaborationRequests = new List<CollaborationRequest>(),
            Ideas = new List<Idea>(),
            Email = user.Email
        };
        await _context.Users.AddAsync(entity);
        await _context.SaveChangesAsync();
        return new UserDetailsDTO
        {
            Id = entity.Id,
            UserName = entity.UserName,
            UserType = "Supervisor",
            Email = entity.Email
        };
    }

}

