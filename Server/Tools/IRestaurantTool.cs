using Google.Protobuf.WellKnownTypes;

namespace Server.Tools
{
    public interface IRestaurantTool
    {
        bool CompleteOrder(int id);
        bool RemoveDish(int id);
        Dish EditDish(Dish dish);
        Dish CreateDish(Dish dish);
        Order CreateOrder(Order order);
        bool ChangeVisibility(int id);
    }
}
