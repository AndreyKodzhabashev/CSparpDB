using System;
using System.Runtime.CompilerServices;
using FastFood.Models.Enums;
using FastFood.Web.ViewModels.Categories;
using FastFood.Web.ViewModels.Employees;
using FastFood.Web.ViewModels.Items;
using FastFood.Web.ViewModels.Orders;

namespace FastFood.Web.MappingConfiguration
{
    using AutoMapper;
    using Models;
    using ViewModels.Positions;

    public class FastFoodProfile : Profile
    {
        public FastFoodProfile()
        {
            //Positions
            this.CreateMap<CreatePositionInputModel, Position>()
                .ForMember(x => x.Name, y => y.MapFrom(s => s.PositionName));

            this.CreateMap<Position, PositionsAllViewModel>()
                .ForMember(x => x.Name, y => y.MapFrom(s => s.Name));

            this.CreateMap<CreateCategoryInputModel, Category>()
                .ForMember(x => x.Name, y => y.MapFrom(s => s.CategoryName)); //TODO done by Andrey

            this.CreateMap<RegisterEmployeeInputModel, Employee>()
                .ForMember(x => x.PositionId, y => y.Ignore());
                
            //TODO done by Andrey
            this.CreateMap<Employee, EmployeesAllViewModel>();

            this.CreateMap<Position, RegisterEmployeeViewModel>()
                .ForMember(x => x.PositionName, y => y.MapFrom(s => s.Name));

            this.CreateMap<RegisterEmployeeInputModel, Employee>()
                .ForMember(x=>x.Name, y=>y.MapFrom(s=>s.Name))
                .ForMember(x=>x.Age, y=>y.MapFrom(s=>s.Age));

            this.CreateMap<Employee, EmployeesAllViewModel>()
                .ForMember(x => x.Position, y => y.MapFrom(s => s.Position.Name));

            this.CreateMap<Category, CreateItemViewModel>()
                .ForMember(x => x.CategoryId, y => y.MapFrom(s => s.Id));

            this.CreateMap<CreateItemInputModel, Item>()
                .ForMember(x => x.Name, y => y.MapFrom(s => s.Name));

            this.CreateMap<Item, ItemsAllViewModels>()
                .ForMember(x => x.Category, y => y.MapFrom(s => s.Category.Name));

            this.CreateMap<Order, OrderAllViewModel>()
                .ForMember(x => x.OrderId, y => y.MapFrom(s => s.Id))
                .ForMember(x => x.Employee, y => y.MapFrom(s => s.Employee.Name))
                .ForMember(x => x.Customer, y => y.MapFrom(s => s.Customer))
                .ForMember(x => x.DateTime, y => y.MapFrom(s => s.DateTime.ToString("D")))
                .ForMember(x=>x.OrderType, y=>y.MapFrom(s=>s.Type));

           
        }
    }
}