#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Checkout.PaymentsGateway.BankIntegration.Api/Checkout.PaymentsGateway.BankIntegration.Api.csproj", "Checkout.PaymentsGateway.BankIntegration.Api/"]
COPY ["Checkout.PaymentsGateway.BankIntegration.Contracts/Checkout.PaymentsGateway.BankIntegration.Contracts.csproj", "Checkout.PaymentsGateway.BankIntegration.Contracts.csproj/"]
RUN dotnet restore "Checkout.PaymentsGateway.BankIntegration.Api/Checkout.PaymentsGateway.BankIntegration.Api.csproj"
COPY . .
WORKDIR "/src/Checkout.PaymentsGateway.BankIntegration.Api"
RUN dotnet build "Checkout.PaymentsGateway.BankIntegration.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Checkout.PaymentsGateway.BankIntegration.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Checkout.PaymentsGateway.BankIntegration.Api.dll"]