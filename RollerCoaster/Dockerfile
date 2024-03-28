FROM mcr.microsoft.com/dotnet/sdk:8.0 AS builder 

WORKDIR /Application
COPY . ./

RUN dotnet restore

RUN dotnet publish -c Development -o output

FROM mcr.microsoft.com/dotnet/aspnet:8.0

WORKDIR /Application
COPY --from=builder /Application/output .

ENV ASPNETCORE_ENVIRONMENT=DockerTesting

ENTRYPOINT ["dotnet", "RollerCoaster.dll"]