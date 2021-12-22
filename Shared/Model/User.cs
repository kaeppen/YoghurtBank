
public abstract class User
    {
    public int Id { get; init; }

    [Required]
    [StringLength(50)]
    public string UserName { get; init; }

        [EmailAddress]
        [DataType(DataType.EmailAddress)]
        [Required]
        public string Email { get; init; }
        public ICollection<CollaborationRequest> CollaborationRequests { get; set; }

}
