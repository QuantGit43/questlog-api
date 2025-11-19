FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY ["QuestLog.Api/QuestLog.Api.csproj", "QuestLog.Api/"]
COPY ["QuestLog.Application/QuestLog.Application.csproj", "QuestLog.Application/"]
COPY ["QuestLog.Domain/QuestLog.Domain.csproj", "QuestLog.Domain/"]
COPY ["QuestLog.Infrastructure/QuestLog.Infrastructure.csproj", "QuestLog.Infrastructure/"]

RUN dotnet restore "QuestLog.Api/QuestLog.Api.csproj"

COPY . .

WORKDIR "/src/QuestLog.Api"
RUN dotnet build "QuestLog.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "QuestLog.Api.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
EXPOSE 80
EXPOSE 443
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "QuestLog.Api.dll"]