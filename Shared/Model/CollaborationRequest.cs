
    public class CollaborationRequest
    {
        public int Id { get; set; }

        [Required]
        public Student Requester { get; init; }

        [Required]
        public Supervisor Requestee { get; init; }
        public Idea? Idea { get; init; }

        [Required]
        [StringLength(500)]
        public string Application { get; set; }
        public CollaborationRequestStatus Status { get; set; } = CollaborationRequestStatus.Waiting;


/**
return true if the status has been changed, and false if it has not been changed
**/
        public bool UpdateStatus(bool Approved, User Updater)
        {
            bool student = (this.Requester.Id == Updater.Id);

            if (Approved) {
                return ApproveRequest(student);
            } 
            else
            {
                return DenyRequest(student);
            }
        }

        private bool ApproveRequest(bool isStudent)
        {
            switch (Status)
            {
                case CollaborationRequestStatus.Waiting:
                    if (!isStudent) 
                    {
                        Status = CollaborationRequestStatus.ApprovedBySupervisor;
                        return true;
                    }
                    else 
                    {
                        return false;
                    }
                case CollaborationRequestStatus.ApprovedBySupervisor:
                    if (!isStudent) 
                    {
                        return false;
                    }
                    else 
                    {
                        Status = CollaborationRequestStatus.ApprovedByStudent;
                        return true;
                    }
                default:
                    return false;
            }
        }

        private bool DenyRequest(bool isStudent)
        {
            switch (Status)
            {
                case CollaborationRequestStatus.Declined:
                    return false;
                default:   
                    Status = CollaborationRequestStatus.Declined;
                    return true;
            }
        }
    }

