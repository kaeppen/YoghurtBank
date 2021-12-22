


[Authorize]
[ApiController]
[Route("api/collaborationRequest")]
public class CollaborationRequestController : ControllerBase
{

    private readonly ILogger<CollaborationRequestController> _logger;
    private readonly ICollaborationRequestRepository _repository;

    public CollaborationRequestController(ILogger<CollaborationRequestController> logger, ICollaborationRequestRepository repository)
    {
        _logger = logger;
        _repository = repository;
    }

    [HttpGet("{ideaid}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(IReadOnlyCollection<IdeaDetailsDTO>), StatusCodes.Status200OK)]
    public async Task<IReadOnlyCollection<CollaborationRequestDetailsDTO>> GetByIdeaId(int id)
    {
        var requests = await _repository.FindRequestsByIdeaAsync(id);
        return requests;
    }

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

    [HttpPost]
    public async Task<CollaborationRequestDetailsDTO> Post(CollaborationRequestCreateDTO request)
    {
        return await _repository.CreateAsync(request);
    }


    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<int?> Delete(int id)
    {
        return await _repository.DeleteAsync(id);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(CollaborationRequestDetailsDTO), StatusCodes.Status200OK)]
    public async Task<CollaborationRequestDetailsDTO> Put(int id, [FromBody] CollaborationRequestUpdateDTO request)
    {
        return (await _repository.UpdateAsync(id, request));
    }
}
