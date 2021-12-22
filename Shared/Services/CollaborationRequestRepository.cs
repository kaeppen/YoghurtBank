


public class CollaborationRequestRepository : ICollaborationRequestRepository
{
    private readonly IYoghurtContext _context;
    public CollaborationRequestRepository(IYoghurtContext context)
    {
        _context = context;
    }

    public async Task<CollaborationRequestDetailsDTO> CreateAsync(CollaborationRequestCreateDTO request)
    {
        var student = (Student)await _context.Users.FindAsync(request.StudentId);
        if (student == null) return null;
        var super = (Supervisor)await _context.Users.FindAsync(request.SupervisorId);
        if (super == null) return null;

        var entity = new CollaborationRequest
        {
            Requester = student,
            Requestee = super,
            Application = request.Application,
            Idea = await _context.Ideas.FindAsync(request.IdeaId),
            Status = CollaborationRequestStatus.Waiting 
        };

        _context.CollaborationRequests.Add(entity);
        super.CollaborationRequests.Add(entity);
        student.CollaborationRequests.Add(entity);
        await _context.SaveChangesAsync();

        return new CollaborationRequestDetailsDTO
        {
            Id = entity.Id,
            StudentId = entity.Requester.Id,
            SupervisorId = entity.Requestee.Id,
            Status = entity.Status,
            Application = entity.Application,
        };
    }


    public async Task<CollaborationRequestDetailsDTO> FindById(int id)
    {
        var collabRequest = await _context.CollaborationRequests.FindAsync(id);

        if (collabRequest == null)
        {
            return null;
        }

        return new CollaborationRequestDetailsDTO
        {
            Id = collabRequest.Id,
            StudentId = _context.Users.FindAsync(collabRequest.Id).Result.Id,
            SupervisorId = _context.Users.FindAsync(collabRequest.Id).Result.Id,
            Status = collabRequest.Status,
            Application = collabRequest.Application
        };
    }

    public async Task<int?> DeleteAsync(int id)
    {
        var entity = await _context.CollaborationRequests.FindAsync(id);
        if (entity == null)
        {
            return null;
        }

        _context.CollaborationRequests.Remove(entity);
        await _context.SaveChangesAsync();
        return entity.Id;
    }


    public async Task<IReadOnlyCollection<CollaborationRequestDetailsDTO>> FindRequestsByIdeaAsync(int ideaId)
    {
        var requests = await _context.CollaborationRequests.Where(c => c.Idea.Id == ideaId).Select(c => new CollaborationRequestDetailsDTO
        {
            Id = c.Id,
            StudentId = c.Requester.Id,
            SupervisorId = c.Requestee.Id,
            Application = c.Application,
            Status = c.Status

        }).ToListAsync();

        return requests.AsReadOnly();

    }

    public async Task<CollaborationRequestDetailsDTO> UpdateAsync(int id, CollaborationRequestUpdateDTO updateRequest)
    {

        var entity = await _context.CollaborationRequests.Where(c => c.Id == id).Include(c => c.Requester).Include(c => c.Requestee).FirstOrDefaultAsync();

        if (entity == null)
        {
            return null; 
        }
        entity.Status = updateRequest.Status;
        await _context.SaveChangesAsync();

        return new CollaborationRequestDetailsDTO
        {
            Id = entity.Id,
            Status = entity.Status,
            Application = entity.Application,
            StudentId = entity.Requester.Id,
            SupervisorId = entity.Requestee.Id
        };
    }

    public async Task<bool> FindUserType(int id)
    {
        var user = await _context.Users.FindAsync(id);
        
        if (user.GetType() == typeof(Supervisor))
        {
            return true;
        }
        else
        {
            return false;
        }
    }


    public async Task<IReadOnlyCollection<CollaborationRequestDetailsDTO>> FindRequestsBySupervisorAsync(int supervisorId)
    {

        var listOfUsers = await _context.CollaborationRequests.Where(c => c.Requestee.Id == supervisorId).Select(c => new CollaborationRequestDetailsDTO
        {
            Id = c.Id,
            StudentId = c.Requester.Id,
            SupervisorId = c.Requestee.Id,
            Application = c.Application,
            Status = c.Status

        }).ToListAsync();
        return listOfUsers.AsReadOnly();
    }

    public async Task<IReadOnlyCollection<CollaborationRequestDetailsDTO>> FindRequestsByStudentAsync(int studentId)
    {

        var listOfUsers = await _context.CollaborationRequests.Where(c => c.Requester.Id == studentId).Select(c => new CollaborationRequestDetailsDTO
        {
            Id = c.Id,
            StudentId = c.Requester.Id,
            SupervisorId = c.Requestee.Id,
            Application = c.Application,
            Status = c.Status

        }).ToListAsync();
        return listOfUsers.AsReadOnly();
    }
}
