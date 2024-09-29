# Use the official .NET 8 SDK image to build the project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Set the working directory inside the container
WORKDIR /app

# Copy the project file and restore dependencies
COPY src/ESD.DataReaderService/ESD.DataReaderService.csproj ./ESD.DataReaderService/
WORKDIR /app/ESD.DataReaderService
RUN dotnet restore

# Copy the rest of the source code
COPY ./src .
COPY ./src/ESD.DataReaderService ./ESD.DataReaderService/


# Build the project
RUN dotnet publish -c Release -o /app/out

# Use the official .NET 8 runtime image to run the app
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime

# Set the working directory in the runtime image
WORKDIR /app

# Copy the output from the build stage
COPY --from=build /app/out .

# Expose the port your application is running on (optional)
EXPOSE 80

# Run the application
ENTRYPOINT ["dotnet", "ESD.DataReaderService.dll"]
