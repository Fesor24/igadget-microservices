FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /app
EXPOSE 80

COPY IGadget.sln IGadget.sln
COPY src/OrderService/OrderService.csproj src/OrderService/OrderService.csproj
COPY src/ShoppingCartService/ShoppingCartService.csproj src/ShoppingCartService/ShoppingCartService.csproj
COPY src/ProductService/ProductService.csproj src/ProductService/ProductService.csproj
COPY src/SearchService/SearchService.csproj src/SearchService/SearchService.csproj
COPY src/GatewayService/GatewayService.csproj src/GatewayService/GatewayService.csproj
COPY src/IdentityService/IdentityService.csproj src/IdentityService/IdentityService.csproj
COPY src/DiscountService/DiscountService.csproj src/DiscountService/DiscountService.csproj
COPY src/Shared/Shared.csproj src/Shared/Shared.csproj

RUN dotnet restore IGadget.sln

COPY src/OrderService src/OrderService
COPY src/Shared src/Shared

WORKDIR /app/src/OrderService
RUN dotnet publish -c Release -o /app/src/out

FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app
COPY --from=build /app/src/out .
ENTRYPOINT ["dotnet", "OrderService.dll"]