using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Server.Models
{

    public class Order
    {
        [Key]
        public int Id { get; set; }
        public int TableNumber { get; set; }
        public bool Completed { get; set; }
        public float TotalPrice { get; set; }
        public DateTime Time { get; set; }
    }
}
