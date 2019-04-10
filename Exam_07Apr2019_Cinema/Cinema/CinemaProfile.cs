using System;
using System.Collections.Generic;
using System.Globalization;
using AutoMapper;
using Cinema.Data.Models;
using Cinema.DataProcessor.ImportDto;

namespace Cinema
{
    public class CinemaProfile : Profile
    {
        // Configure your AutoMapper here if you wish to use it. If not, DO NOT DELETE THIS CLASS
        public CinemaProfile()
        {
            this.CreateMap<ImportHallDto, Hall>()
                .ForMember(dest => dest.Seats, opt => opt.Ignore())
                .ForMember(dest => dest.Projections, opt => opt.Ignore());

            this.CreateMap<ImportProjectionDto, Projection>()
                .ForMember(dest => dest.Tickets, opt => opt.Ignore())
                .ForMember(dest => dest.Hall, opt => opt.Ignore())
                .ForMember(dest => dest.Movie, opt => opt.Ignore())
                .ForMember(dest => dest.DateTime,
                    opt => opt.MapFrom(src =>
                        DateTime.ParseExact(src.DateTime, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));

            this.CreateMap<ImportCustomerTicketsDto, Customer>()
                .ForMember(dest => dest.Tickets, opt => opt.Ignore());
        }
    }
}