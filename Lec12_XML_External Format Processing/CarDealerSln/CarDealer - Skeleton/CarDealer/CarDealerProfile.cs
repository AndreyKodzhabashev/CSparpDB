using System;
using System.Linq;
using AutoMapper;
using CarDealer.Dtos.Export;
using CarDealer.Models;

namespace CarDealer
{
    public class CarDealerProfile : Profile
    {
        public CarDealerProfile()
        {
            this.CreateMap<Car, ExportCarBmwDto>();

            this.CreateMap<Supplier, ExportSuppliersDto>()
                .ForMember(dest => dest.PartsCount, opt => opt.MapFrom(src => src.Parts.Count));


            this.CreateMap<Sale, ExportSalesWithDiscountDto>()
                .ForMember(x => x.Car,
                    y => y.MapFrom(s => Mapper.Map<ExportSalesWithDiscountDto>(s.Car)))
                .ForMember(x => x.CustomerName, y => y.MapFrom(s => s.Customer.Name))
                .ForMember(x => x.Price,
                    y => y.MapFrom(s => s.Car.PartCars.Sum(pc => pc.Part.Price)))
                .ForMember(x => x.PriceWithDiscount,
                    y => y.MapFrom(s =>
                        Math.Round(s.Car.PartCars.Sum(pc => pc.Part.Price) - s.Car.PartCars.Sum(pc => pc.Part.Price) * s.Discount / 100, 2)));
        }
    }
}