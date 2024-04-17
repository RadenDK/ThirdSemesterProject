# Use the official ASP.NET Core SDK as a base image for building the project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app

# Copy everything and build the project
COPY . ./
RUN dotnet publish -c Release -o out

# Use the official ASP.NET Core runtime as a base image for running the application
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

# Copy the build output from the build environment
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "GameClientApi.dll"]