#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Checkout.PaymentsGateway.Api/Checkout.PaymentsGateway.Api.csproj", "Checkout.PaymentsGateway.Api/"]
COPY ["Checkout.PaymentsGateway.DataContext/Checkout.PaymentsGateway.DataContext.csproj", "Checkout.PaymentsGateway.DataContext/"]
COPY ["Checkout.PaymentsGateway.Contracts/Checkout.PaymentsGateway.Contracts.csproj", "Checkout.PaymentsGateway.Contracts/"]
COPY ["Checkout.PaymentsGateway.Domain/Checkout.PaymentsGateway.Domain.csproj", "Checkout.PaymentsGateway.Domain/"]
COPY ["Checkout.PaymentsGateway.Infrastructure/Checkout.PaymentsGateway.Infrastructure.csproj", "Checkout.PaymentsGateway.Infrastructure/"]
COPY ["Checkout.PaymentsGateway.BankIntegration.Contracts/Checkout.PaymentsGateway.BankIntegration.Contracts.csproj", "Checkout.PaymentsGateway.BankIntegration.Contracts.csproj/"]
COPY ["Checkout.Identity.Auth/Checkout.Identity.Auth.csproj", "Checkout.Identity.Auth.csproj/"]
COPY ["Checkout.Core.Logging/Checkout.Core.Logging.csproj", "Checkout.Core.Logging.csproj/"]
RUN dotnet restore "Checkout.PaymentsGateway.Api/Checkout.PaymentsGateway.Api.csproj"
COPY . .
WORKDIR "/src/Checkout.PaymentsGateway.Api"
RUN dotnet build "Checkout.PaymentsGateway.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Checkout.PaymentsGateway.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Checkout.PaymentsGateway.Api.dll"]
