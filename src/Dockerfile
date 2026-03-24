# STAGE 1: Build & Publish
# We use the SDK image to compile the code
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# 1. Copy the project file and restore dependencies 
# (This layer is cached unless your .csproj changes)
COPY ["src/boxes.csproj", "src/"]
RUN dotnet restore "src/boxes.csproj"

# 2. Copy the rest of the source code
COPY . .

# 3. Move to the project folder and Publish
# This compiles the code and gathers all DLLs into /app/publish
WORKDIR "/src/src"
RUN dotnet publish "boxes.csproj" -c Release -o /app/publish /p:UseAppHost=false

# STAGE 2: Runtime
# We switch to a much smaller image that only contains the .NET runtime
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app

# 4. Copy ONLY the compiled output from the build stage
COPY --from=build /app/publish .

# 5. Set the entry point to run your DLL
ENTRYPOINT ["dotnet", "boxes.dll"]