
public interface ICollaborationRequestRepository
{
    Task<CollaborationRequestDetailsDTO> CreateAsync(CollaborationRequestCreateDTO request);
    Task<CollaborationRequestDetailsDTO> FindById(int id); //aka GET
    Task<int?> DeleteAsync(int id);

    Task<IReadOnlyCollection<CollaborationRequestDetailsDTO>> FindRequestsByIdeaAsync(int ideaId);

    Task<CollaborationRequestDetailsDTO> UpdateAsync(int id, CollaborationRequestUpdateDTO updateRequest); //return value? like delete

    Task<IReadOnlyCollection<CollaborationRequestDetailsDTO>> FindRequestsBySupervisorAsync(int supervisorId);
    Task<IReadOnlyCollection<CollaborationRequestDetailsDTO>> FindRequestsByStudentAsync(int studentId);
    Task<bool> FindUserType(int id);
}
