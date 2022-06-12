namespace Server.Models
{
    public class OrderDishes
    {
        public int Id { get; set; }

        public Dish Dish { get; set; }

        public Order Order { get; set; }
    }
}
