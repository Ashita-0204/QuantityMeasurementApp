FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy all project files
COPY ../QuantityMeasurementModel/QuantityMeasurementModel.csproj QuantityMeasurementModel/
COPY ../QuantityMeasurementApp/QuantityMeasurementApp.csproj QuantityMeasurementApp/
COPY ../QuantityMeasurementRepository/QuantityMeasurementRepository.csproj QuantityMeasurementRepository/
COPY ../QuantityMeasurementBusinessLayer/QuantityMeasurementBusinessLayer.csproj QuantityMeasurementBusinessLayer/
COPY QuantityMeasurementApi.csproj QuantityMeasurementApi/

# Restore
RUN dotnet restore QuantityMeasurementApi/QuantityMeasurementApi.csproj

# Copy all source
COPY ../QuantityMeasurementModel/ QuantityMeasurementModel/
COPY ../QuantityMeasurementApp/ QuantityMeasurementApp/
COPY ../QuantityMeasurementRepository/ QuantityMeasurementRepository/
COPY ../QuantityMeasurementBusinessLayer/ QuantityMeasurementBusinessLayer/
COPY . QuantityMeasurementApi/

# Build and publish
RUN dotnet publish QuantityMeasurementApi/QuantityMeasurementApi.csproj -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "QuantityMeasurementApi.dll"]