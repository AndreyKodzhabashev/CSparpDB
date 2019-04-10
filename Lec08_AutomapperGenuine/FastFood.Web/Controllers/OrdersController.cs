using AutoMapper.QueryableExtensions;
using FastFood.Models;
using FastFood.Models.Enums;

namespace FastFood.Web.Controllers
{
    using AutoMapper;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Linq;
    using Data;
    using ViewModels.Orders;

    public class OrdersController : Controller
    {
        private readonly FastFoodContext context;
        private readonly IMapper mapper;

        public OrdersController(FastFoodContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public IActionResult Create()
        {
            var items = context.Items.Select(x => x.Name).ToList();
            var viewOrder = new CreateOrderViewModel
            {
                Items = items,

                Employees = this.context.Employees.ToList(),
            };

            return this.View(viewOrder);
        }

        [HttpPost]
        public IActionResult Create(CreateOrderInputModel model)
        {
            //customer, empId, ItemId, Qty
           Item item = context.Items.FirstOrDefault(x => x.Name == model.ItemName);
           var test = model.EmployeeName;
           var empId = model.EmployeeName.Split('-', StringSplitOptions.RemoveEmptyEntries).TakeLast(1)
               .Select(int.Parse).FirstOrDefault();
            
           var order = new Order()
            {
                Customer = model.Customer,
                DateTime = DateTime.Now,
                Type = Enum.Parse<OrderType>(model.OrderType),
                TotalPrice = model.Quantity * item.Price,
                EmployeeId = empId
            };
            
            var orderItem = new OrderItem()
            {
                ItemId = item.Id,
                Item = item,
                Order = order
            };

            context.OrderItems.Add(orderItem);
            context.Orders.Add(order);

            context.SaveChanges();
            return this.RedirectToAction("All", "Orders");
        }

        public IActionResult All()
        {
            var orders = context.Orders
                .ProjectTo<OrderAllViewModel>(mapper.ConfigurationProvider)
                .ToList();

            return View(orders);
        }
    }
}