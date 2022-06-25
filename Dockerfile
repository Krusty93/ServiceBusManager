ARG environment

FROM mcr.microsoft.com/dotnet/sdk:6.0 as build

COPY ./src ./src/

RUN dotnet publish \
    --configuration Release \
    --self-contained false \
    --runtime linux-x64 \
    --output /app/publish \
    src/ServiceBusManager.Server/ServiceBusManager.Server.API/ServiceBusManager.Server.API.csproj

FROM mcr.microsoft.com/dotnet/aspnet:6.0 as final
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 80
ENV ASPNETCORE_ENVIRONMENT=$environment
ENTRYPOINT ["dotnet", "ServiceBusManager.Server.API.dll"]