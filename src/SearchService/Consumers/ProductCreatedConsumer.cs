﻿using AutoMapper;
using MassTransit;
using MongoDB.Entities;
using SearchService.Entities;
using Shared.Contracts;

namespace SearchService.Consumers;

public sealed class ProductCreatedConsumer : IConsumer<ProductCreated>
{
    private readonly IMapper _mapper;

    public ProductCreatedConsumer(IMapper mapper)
    {
        _mapper = mapper;
    }

    public async Task Consume(ConsumeContext<ProductCreated> context)
    {
        Console.WriteLine("Product Create: Message processing...");
        var product = _mapper.Map<Product>(context.Message);

        await product.SaveAsync();
    }
}
