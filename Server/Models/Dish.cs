using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Server.Models
{

    public class Dish
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public float Price { get; set; }
        public bool IsVisible { get; set; }
    }
}
