FROM microsoft/aspnetcore-build:2.0 AS build-env
WORKDIR /app

COPY . . 

WORKDIR /app/src/Sia.Gateway
RUN dotnet publish -c Release -o out

# Build runtime image
FROM microsoft/aspnetcore:2.0
WORKDIR /app
COPY --from=build-env /app/src/Sia.Gateway/out .
EXPOSE 80
ENTRYPOINT ["dotnet", "Sia.Gateway.dll"]
