namespace CustomerTool.Models.Db
{
    public class Customer
    {
        public Guid Id { get; private set; }
        public string Name { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public char? Gender { get; set; } = null;
        public string Address { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public bool IsDeleted { get; set; }

        public Customer()
        {
            Id = Guid.NewGuid();
        }
    }
}
