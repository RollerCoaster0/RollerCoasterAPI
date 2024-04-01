FROM mcr.microsoft.com/dotnet/sdk:8.0 AS builder 

ARG BUILD_CONFIGURATION

WORKDIR /Application

copy *.csproj ./
RUN dotnet restore

COPY . ./
RUN dotnet publish -c ${BUILD_CONFIGURATION} -o output

FROM mcr.microsoft.com/dotnet/aspnet:8.0

WORKDIR /Application
COPY --from=builder /Application/output .

ENTRYPOINT ["dotnet", "RollerCoaster.dll"]