

using Server.Models;

namespace Server.Tools
{
    public class RestaurantTool : IRestaurantTool
    {
        private readonly Database _db;
        private readonly ITimeTool _timeTool;

        public RestaurantTool(Database db, ITimeTool timeTool)
        {
            _db = db;
            _timeTool = timeTool;
        }

        public bool ChangeVisibility(int id)
        {
            var dish = _db.Dishes.Where(d => d.Id == id).FirstOrDefault();

            if (dish is null)
                return false;

           if (dish.IsVisible)
                dish.IsVisible = false;
           else 
                dish.IsVisible = true;

            _db.Dishes.Update(dish);
            _db.SaveChanges();

            return true;

        }

        public bool CompleteOrder(int Id)
        {
            var order = _db.Orders.Find(Id);

            if (order is null)
                return false;

            order.Completed = true;
            _db.SaveChanges();


            return true;
        }

        public Dish CreateDish(Dish dish)
        {
            Models.Dish temp = new Models.Dish
            {
                Name = dish.Name,
                Price = dish.Price,
                IsVisible = dish.IsVisible,

            };

            _db.Dishes.Add(temp);
            _db.SaveChanges();

            return new Dish { Id = temp.Id, Name = temp.Name, IsVisible = temp.IsVisible, Price = temp.Price };
        }

        public Order CreateOrder(Order order)
        {
            Models.Order newOrder = new Models.Order
            {
                TableNumber = order.TableNumber,
                Completed = order.Completed,
                TotalPrice = order.TotalPrice,
                Time = DateTime.Now,
            };

            _db.Orders.Add(newOrder);

            foreach (var i in order.DishesId)
            {
                var dish = _db.Dishes.Find(i);
                OrderDishes orderDish = new OrderDishes
                {
                    Dish = dish,
                    Order = newOrder,

                };
                _db.OrdersDishes.Add(orderDish);
            }
            _db.SaveChanges();

            Order retOrder = new Order
            {
                Id = newOrder.Id,
                Completed = newOrder.Completed,
                TotalPrice = newOrder.TotalPrice,
                TableNumber = newOrder.TableNumber,
                Time = _timeTool.ConvertDateTimeToTimeStamp(newOrder.Time.AddHours(2))
            };
            retOrder.DishesId.Add(order.DishesId);

            return retOrder;
        }

        public Dish EditDish(Dish dish)
        {
            var d = _db.Dishes.Find(dish.Id);
            d.Name = dish.Name;
            d.Price = dish.Price;
            d.IsVisible = dish.IsVisible;
            _db.Dishes.Update(d);
            _db.SaveChanges();

            return dish;
        }

        public bool RemoveDish(int Id)
        {
            var dish = _db.Dishes.Find(Id);

            if (dish is null)
                return false;

            _db.Remove(dish);
            _db.SaveChanges();

            return true;
        }
    }
}
