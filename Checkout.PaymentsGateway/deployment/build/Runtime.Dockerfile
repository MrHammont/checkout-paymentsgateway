
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS Runtime
WORKDIR /app

COPY /artifacts/app /app

RUN groupadd -g 999 appuser && useradd -r -u 999 -g appuser appuser
RUN chown -R appuser:appuser .
USER appuser

ENTRYPOINT ["dotnet", "Checkout.PaymentsGateway.dll"]