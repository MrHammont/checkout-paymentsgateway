	#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Checkout.Identity.Api/Checkout.Identity.Api.csproj", "Checkout.Identity.Api/"]
COPY ["Checkout.Identity.Auth/Checkout.Identity.Auth.csproj", "Checkout.Identity.Auth.csproj/"]
COPY ["Checkout.Identity.Contracts/Checkout.Identity.Contracts.csproj", "Checkout.Identity.Contracts\Checkout.Identity.Contracts.csproj/"]
RUN dotnet restore "Checkout.Identity.Api/Checkout.Identity.Api.csproj"
COPY . .
WORKDIR "/src/Checkout.Identity.Api"
RUN dotnet build "Checkout.Identity.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Checkout.Identity.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Checkout.Identity.Api.dll"]
