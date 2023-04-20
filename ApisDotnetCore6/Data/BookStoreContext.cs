using System.Security.Principal;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ApisDotnetCore6.Data;

public class BookStoreContext : DbContext
{
    public BookStoreContext(DbContextOptions<BookStoreContext> opt) : base(opt)
    {
    }

    #region DbSet

    public DbSet<Book>? Books { get; set; }

    #endregion
}