using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using CollectorApp.Api.Dtos;
using CollectorApp.Api.Models;

namespace CollectorApp.Api.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<BarcodeCreateDto, Barcode>();
            CreateMap<Barcode, BarcodeResponseDto>();
        }
    }
}