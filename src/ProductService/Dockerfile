FROM bitnami/dotnet-sdk:7 AS build
WORKDIR /app
EXPOSE 80

COPY IGadget.sln IGadget.sln
COPY src/ProductService/ProductService.csproj src/ProductService/ProductService.csproj
COPY src/SearchService/SearchService.csproj src/SearchService/SearchService.csproj
COPY src/GatewayService/GatewayService.csproj src/GatewayService/GatewayService.csproj
COPY src/IdentityService/IdentityService.csproj src/IdentityService/IdentityService.csproj
COPY src/ShoppingCartService/ShoppingCartService.csproj src/ShoppingCartService/ShoppingCartService.csproj
COPY src/OrderService/OrderService.csproj src/OrderService/OrderService.csproj
COPY src/Shared/Shared.csproj src/Shared/Shared.csproj

RUN dotnet restore IGadget.sln

COPY src/ProductService src/ProductService
COPY src/Shared src/Shared

WORKDIR /app/src/ProductService
RUN dotnet publish -c Release -o /app/src/out

FROM bitnami/aspnet-core:7
WORKDIR /app
COPY --from=build /app/src/out .
ENTRYPOINT ["dotnet", "ProductService.dll"]