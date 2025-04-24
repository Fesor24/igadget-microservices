﻿namespace Shared.Contracts;
public class ProductCreated
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public int YearOfRelease { get; set; }
    public string ImageUrl { get; set; }
    public string Category { get; set; }
    public string Brand { get; set; }
}
