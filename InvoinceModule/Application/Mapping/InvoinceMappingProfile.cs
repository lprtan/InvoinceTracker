using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using InvoinceModule.Domain.Entities;
using InvoinceModule.Application.Dtos;

namespace InvoinceModule.Application.Mapping
{
    public class InvoinceMappingProfile : Profile
    {
        public InvoinceMappingProfile()
        {
            CreateMap<InvoiceHeader, InvoiceDto>().ReverseMap();
            CreateMap<InvoiceDetail, InvoiceDetailDto>().ReverseMap();
            CreateMap<InvoiceHeader, InvoiceHeaderDto>();
        }
    }
}
