    public record UserDetailsDTO
    {
        public int Id { get; set; }
        [StringLength(50)]
        public string UserName { get; set; }
        public string UserType { get; set; }

        [EmailAddress]
        [DataType(DataType.EmailAddress)]
        [Required]
        public string Email { get; init; }
    }

    public record UserCreateDTO
    {
        [StringLength(50)]
        public string UserName { get; set; }
        [Required]
        public string UserType { get; set; }
        [EmailAddress]
        [DataType(DataType.EmailAddress)]
        [Required]
        public string Email { get; set; }
    }


