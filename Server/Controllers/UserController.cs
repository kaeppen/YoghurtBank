


    [Authorize]
    [ApiController]
    //[Route("api/[controller]")]
    [Route("api/users")]
    //[RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUserRepository _repository;

        
        public UserController(ILogger<UserController> logger, IUserRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }

        [ProducesResponseType(typeof(UserDetailsDTO), StatusCodes.Status201Created)]
        //[ProducesResponseType(typeof(UserDetailsDTO), StatusCodes.)] - tilføj statuscode såfremt user ikke skabes
        [HttpPost]
        public async Task<UserDetailsDTO> Post(UserCreateDTO user)
        {
            return await _repository.CreateAsync(user);
            
        }

        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(UserDetailsDTO), StatusCodes.Status200OK)]
        [HttpGet("{id}")]
        [Route("/api/users/{id}")]
        public async Task<UserDetailsDTO> GetById(int id) 
        {
            return await _repository.FindUserByIdAsync(id);
        }

        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(UserDetailsDTO), StatusCodes.Status200OK)]
        [HttpGet("{email}")]
        [Route("/api/users/mail/{email}")]
        public async Task<UserDetailsDTO> GetByEmail(string email) 
        {
            return await _repository.FindUserByEmail(email);

        }

        
        [AllowAnonymous] //overrides the authorize annotation, used bcuz the other stuff doesn't work 
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpDelete("{id}")]
        public async Task<int> Delete(int id)
        {
            return await _repository.DeleteAsync(id);
        }

        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(IReadOnlyCollection<UserDetailsDTO>),StatusCodes.Status200OK)]
        [HttpGet]
        [Route("/api/users")]
        public async Task<IReadOnlyCollection<UserDetailsDTO>> GetSupervisors()
        {
            return await _repository.GetAllSupervisors();
        }
    }
