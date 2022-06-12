using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using Server.Models;
using Server.Tools;
using System;

namespace Server.Services;

public class RestaurantService : Restaurant.RestaurantBase
{
    private readonly Database _db;
    private readonly ITimeTool _timeTool;
    private readonly IRestaurantTool _restaurantTool;


    public RestaurantService(Database db, 
        ITimeTool timeTool, IRestaurantTool restaurantTool)
    {
        _db = db;
        _timeTool = timeTool;
        _restaurantTool = restaurantTool;
        
    }
    
    public override Task<Dish> CreateDish(Dish request, ServerCallContext context)
    {
        var dish = _restaurantTool.CreateDish(request);

        return Task.FromResult(dish);
    }

    public override Task<Order> CreateOrder(Order request, ServerCallContext context)
    {
        var order = _restaurantTool.CreateOrder(request);

        return Task.FromResult(order);    
    }

    public override async Task GetMenu(Menu request, IServerStreamWriter<Dish> responseStream, ServerCallContext context)
    {
        var dishes = _db.Dishes.Where(d=>d.IsVisible==true).ToList();
        foreach(var d in dishes)
        {
            await responseStream.WriteAsync(new Dish {Id= d.Id, Name = d.Name, Price=d.Price});
        }
    }

    public override Task<Dish> EditDish(Dish request, ServerCallContext context)
    {
        var dish = _restaurantTool.EditDish(request);

        return Task.FromResult(dish);
    }
  
    public override Task<Response> DeleteDish(DishId request, ServerCallContext context)
    {
        var result = _restaurantTool.RemoveDish(request.Id);

        if (!result)
            throw new RpcException(new Status(StatusCode.NotFound, "Not Found"));

        return Task.FromResult(new Response { Result = true });
    }

    public override Task<Response> CompleteOrder(OrderId request, ServerCallContext context)
    {
        var result = _restaurantTool.CompleteOrder(request.Id);

        if(result == false)
            throw new RpcException(new Status(StatusCode.NotFound, "Not Found"));

        return Task.FromResult(new Response { Result = result });
    }

    public override async Task<Dish> GetDish(DishId request, ServerCallContext context)
    {
        var dish = _db.Dishes.Find(request.Id);

        if (dish is null)
            throw new RpcException(new Status(StatusCode.NotFound, "Not Found"));

        return await Task.FromResult(new Dish { Id=dish.Id, Name = dish.Name, IsVisible= dish.IsVisible, Price = dish.Price });
    }

    public override async Task GetOrders(Orders request, IServerStreamWriter<Order> responseStream, ServerCallContext context)
    {
        var orders = _db.Orders.Where(o=> o.Completed==false);
        foreach (var o in orders)
        {

            var TimeStamp = _timeTool.ConvertDateTimeToTimeStamp(o.Time);

            var dishes = _db.OrdersDishes.Where(a => a.Order.Id == o.Id)
                .Include(a => a.Dish);

            List<int> tab = new List<int>();


            foreach (var d in dishes)
            {
                tab.Add(d.Dish.Id);
            }
            var order = new Order
            {
                Id = o.Id,
                TableNumber = o.TableNumber,
                Completed = o.Completed,
                TotalPrice = o.TotalPrice,
                Time = TimeStamp
            };
            order.DishesId.AddRange(tab);
            await responseStream.WriteAsync(order);
        }
    }

    public override Task<Response> ChangeVisibility(DishId request, ServerCallContext context)
    {
        var result = _restaurantTool.ChangeVisibility(request.Id);

        if(!result)
            throw new RpcException(new Status(StatusCode.NotFound, "Not Found"));

        return Task.FromResult(new Response { Result = result});
    }

    public override async Task GetDishes(Menu request, IServerStreamWriter<Dish> responseStream, ServerCallContext context)
    {
        var dishes = _db.Dishes.ToList();
        foreach (var d in dishes)
        {
            await responseStream.WriteAsync(new Dish { Id = d.Id, Name = d.Name, Price = d.Price, IsVisible =d.IsVisible });
        }
    }
}