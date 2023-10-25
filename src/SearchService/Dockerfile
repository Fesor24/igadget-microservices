FROM bitnami/dotnet-sdk:7 AS build
WORKDIR /app
EXPOSE 80

COPY IGadget.sln IGadget.sln
COPY src/SearchService/SearchService.csproj src/SearchService/SearchService.csproj
COPY src/ProductService/ProductService.csproj src/ProductService/ProductService.csproj
COPY src/IdentityService/IdentityService.csproj src/IdentityService/IdentityService.csproj
COPY src/GatewayService/GatewayService.csproj src/GatewayService/GatewayService.csproj
COPY src/Shared/Shared.csproj src/Shared/Shared.csproj

WORKDIR /app
RUN dotnet restore IGadget.sln

COPY src/SearchService src/SearchService
COPY src/Shared src/Shared

WORKDIR /app/src/SearchService
RUN dotnet publish -c Release -o /app/src/out

FROM bitnami/aspnet-core:7
WORKDIR /app
COPY --from=build /app/src/out .

ENTRYPOINT ["dotnet", "SearchService.dll"]
