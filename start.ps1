dotnet build

dotnet run --project=1_Monolith/Monolith.Api/
dotnet run --project=2_Service/MyBackgroundCheckService.Api/
dotnet run --project=3_Provider/BobBackgroundCheckProvider.Api/

cd 2_Service/MyBackgroundCheckService.Processor
dotnet bin/Debug/netcoreapp2.0/MyBackgroundCheckService.Processor.dll
cd 1_Monolith/Monolith.BackgroundCheck
dotnet bin/Debug/netcoreapp2.0/Monolith.BackgroundCheck.dll
cd 3_Provider/BobBackgroundCheckProvider.Processor
dotnet bin/Debug/netcoreapp2.0/BobBackgroundCheckProvider.Processor.dll




