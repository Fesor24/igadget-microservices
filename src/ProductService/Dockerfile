FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app
EXPOSE 80

COPY IGadget.sln IGadget.sln
COPY src/ShoppingCartService/ShoppingCartService.csproj src/ShoppingCartService/ShoppingCartService.csproj
COPY src/ProductService/ProductService.csproj src/ProductService/ProductService.csproj
COPY src/GatewayService/GatewayService.csproj src/GatewayService/GatewayService.csproj
COPY src/SearchService/SearchService.csproj src/SearchService/SearchService.csproj
COPY src/Shared/Shared.csproj src/Shared/Shared.csproj
COPY src/IdentityService/IdentityService.csproj src/IdentityService/IdentityService.csproj
COPY src/OrderService/OrderService.csproj src/OrderService/OrderService.csproj
COPY tests/ProductService.IntegrationTests/ProductService.IntegrationTests.csproj tests/ProductService.IntegrationTests/ProductService.IntegrationTests.csproj
COPY tests/ProductService.UnitTests/ProductService.UnitTests.csproj tests/ProductService.UnitTests/ProductService.UnitTests.csproj
COPY tests/SearchService.IntegrationTests/SearchService.IntegrationTests.csproj tests/SearchService.IntegrationTests/SearchService.IntegrationTests.csproj
COPY tests/OrderService.UnitTests/OrderService.UnitTests.csproj tests/OrderService.UnitTests/OrderService.UnitTests.csproj

RUN dotnet restore IGadget.sln

COPY src/ProductService src/ProductService
COPY src/Shared src/Shared

WORKDIR /app/src/ProductService
RUN dotnet publish -c Release -o /app/src/out

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/src/out .
ENTRYPOINT ["dotnet", "ProductService.dll"]