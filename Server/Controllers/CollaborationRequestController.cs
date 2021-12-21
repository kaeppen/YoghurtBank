


[Authorize]
[ApiController]
[Route("api/collaborationRequest")]
//[RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
public class CollaborationRequestController : ControllerBase
{
    private readonly ILogger<CollaborationRequestController> _logger;

    private readonly ICollaborationRequestRepository _repository;

    public CollaborationRequestController(ILogger<CollaborationRequestController> logger,
        ICollaborationRequestRepository repository)
    {
        _logger = logger;
        _repository = repository;
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<IReadOnlyCollection<CollaborationRequestDetailsDTO>> Get()
    {

        //dummy data
        var cb1 = new CollaborationRequestDetailsDTO
        {
            StudentId = 1,
            SupervisorId = 2,
            Application = "Science",
            Status = CollaborationRequestStatus.Waiting
        };
        var cb2 = new CollaborationRequestDetailsDTO
        {
            StudentId = 3,
            SupervisorId = 4,
            Application = "Not Science",
            Status = CollaborationRequestStatus.Waiting
        };
        return new List<CollaborationRequestDetailsDTO> { cb1, cb2 };
        //return await _repository.DERSKALLAVESENGETMETODE();
    }

    [Authorize]
    [HttpGet("{ideaid}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(IReadOnlyCollection<IdeaDetailsDTO>), StatusCodes.Status200OK)]
    public async Task<IReadOnlyCollection<CollaborationRequestDetailsDTO>> GetByIdeaId(int id)
    {
        var requests = await _repository.FindRequestsByIdeaAsync(id);
        return requests;
    }

    [Authorize]
    [HttpGet("{userId}")]
    [ProducesResponseType(typeof(IReadOnlyCollection<IdeaDetailsDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Route("/api/collaborationRequest/id/{userId}")]
    public async Task<IReadOnlyCollection<CollaborationRequestDetailsDTO>> GetRequestsByUser(int userId)
    {
        var isSupervisor = await _repository.FindUserType(userId);

        if (isSupervisor)
        {
            return await _repository.FindRequestsBySupervisorAsync(userId);
        }
        else
        {
            return await _repository.FindRequestsByStudentAsync(userId);
        }

    }

    [Authorize]
    [HttpPost]
    //[ProducesResponseType()]
    public async Task<IActionResult> Post(CollaborationRequestCreateDTO request)
    {
        var created = await _repository.CreateAsync(request);
    }

    [Authorize]
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<int?> Delete(int id)
    {
        return await _repository.DeleteAsync(id);
    }

    [Authorize]
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(CollaborationRequestDetailsDTO), StatusCodes.Status200OK)]
    public async Task<CollaborationRequestDetailsDTO> Put(int id, [FromBody] CollaborationRequestUpdateDTO request)
    {
        return (await _repository.UpdateAsync(id, request));
    }
}
