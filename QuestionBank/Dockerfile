FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy csproj and restore
COPY QuestionBank.csproj ./
RUN dotnet restore QuestionBank.csproj

# Copy everything else
COPY . .
RUN dotnet publish QuestionBank.csproj -c Release -o /app/publish


FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
#COPY questionbank.db ./questionbank.db
RUN ln -s /app/bin/Release/net9.0/publish/wwwroot/_content /app/wwwroot/_content
ENV DOTNET_ENVIRONMENT=Production
EXPOSE 8080
ENTRYPOINT ["dotnet", "QuestionBank.dll"]
