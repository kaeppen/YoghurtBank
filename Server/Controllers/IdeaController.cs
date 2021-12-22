[Authorize]
[ApiController]
[Route("api/[controller]")]
[RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
public class IdeaController : ControllerBase
{
    private readonly ILogger<IdeaController> _logger;
    private readonly IIdeaRepository _repository;

    public IdeaController(ILogger<IdeaController> logger, IIdeaRepository repository)
    {
        _logger = logger;
        _repository = repository;
    }


    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(IdeaDetailsDTO), StatusCodes.Status200OK)]
    [HttpGet("{id}")]
    public async Task<IdeaDetailsDTO> GetById(int id)
    {
        return await _repository.FindIdeaDetailsAsync(id);
    }

    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(IReadOnlyCollection<IdeaDetailsDTO>), StatusCodes.Status200OK)]
    [HttpGet("user/{superid:int}")]
    public async Task<IReadOnlyCollection<IdeaDetailsDTO>> GetBySuperId(int superid)
    {
        var ideas = await _repository.FindIdeasBySupervisorIdAsync(superid);
        return ideas.list;
    }

    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(IReadOnlyCollection<IdeaDetailsDTO>), StatusCodes.Status200OK)]
    [AllowAnonymous]
    [HttpGet]
    public async Task<IReadOnlyCollection<IdeaDetailsDTO>> GetAll()
    {
        return await _repository.ReadAllAsync();

    }

        [Authorize]
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<int?> Delete(int id)
        { 
            return await _repository.DeleteAsync(id);
        }

    [AllowAnonymous]
    [HttpPost]
    [ProducesResponseType(typeof(IdeaDetailsDTO), 201)]
    public async Task<IdeaDetailsDTO> Post(IdeaCreateDTO idea)
    {
        return await _repository.CreateAsync(idea);
    }


    [Authorize]
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType( StatusCodes.Status204NoContent)]
    public async Task<IdeaDetailsDTO> Put(int id, IdeaUpdateDTO update) 
    {
        return await _repository.UpdateAsync(id, update);
    }
    
}
