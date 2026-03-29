FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy project files
COPY ["software_API.csproj", "./"]

# Restore dependencies
RUN dotnet restore "software_API.csproj"

# Copy source code
COPY . .

# Build application
RUN dotnet build "software_API.csproj" -c Release -o /app/build

FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
WORKDIR /app

# Expose ports
EXPOSE 80
EXPOSE 443

FROM build AS publish
RUN dotnet publish "software_API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Set environment
ENV ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_URLS=http://+:80

# Run application
ENTRYPOINT ["dotnet", "software_API.dll"]
