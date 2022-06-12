using Microsoft.EntityFrameworkCore;

namespace Server.Models;

public class Database : DbContext
{
    public Database(DbContextOptions<Database> options) : base(options)
    {

    }

    public DbSet<Dish> Dishes { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderDishes> OrdersDishes { get; set; }

}