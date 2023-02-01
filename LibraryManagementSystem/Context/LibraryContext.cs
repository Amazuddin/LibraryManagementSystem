using System.Data.Entity;
using LibraryManagementSystem.Models;

namespace LibraryManagementSystem.Context
{
    public class LibraryContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<BookInfo> BookInfos { get; set; }
        public DbSet<BorrowInfo> BorrowInfos { get; set; }
    }
}