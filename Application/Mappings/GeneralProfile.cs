﻿using Application.Features.Products.Commands.CreateProduct;
using Application.Features.Products.Queries.GetAllProducts;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Mappings
{
    public class GeneralProfile : Profile
    {
        public GeneralProfile()
        {
            CreateMap<Produto, GetAllProductsViewModel>().ReverseMap();
            CreateMap<CreateProductCommand, Produto>();
            CreateMap<GetAllProductsQuery, GetAllProductsParameter>();
        }
    }
}
