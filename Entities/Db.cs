using Microsoft.EntityFrameworkCore;

namespace EmployeeControlService.Entities

{
    public class Db : DbContext
    {
        public Db(DbContextOptions<Db> options) : base(options)
        {
        }

        public DbSet<UserInfo> UsersInfo { get; set; }
    }
}