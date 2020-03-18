# Build Agent to use for execution of any build steps  
FROM mcr.microsoft.com/dotnet/core/sdk:3.1

RUN apt-get update && apt-get upgrade -y
RUN apt-get install -y make

WORKDIR /code
COPY src/ ./src/
COPY tests/ ./tests/
COPY Checkout.PaymentsGateway.sln Checkout.PaymentsGateway.sln
COPY deployment/Makefile Makefile
COPY docker-compose.dcproj docker-compose.dcproj
COPY Nuget.config Nuget.config

ENV DOTNET_SKIP_FIRST_TIME_EXPERIENCE true
ENV NUGET_XMLDOC_MODE skip

ENTRYPOINT ["make"]