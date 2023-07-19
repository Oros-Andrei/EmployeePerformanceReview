namespace EmployeePerformanceReview.Models
{
    public class UserInfoModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string LoginEmail { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public decimal PerformanceScore { get; set; }
    }
}