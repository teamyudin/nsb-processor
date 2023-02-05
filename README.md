# nsb-processor
NServiceBus Processor

https://learn.microsoft.com/en-us/aspnet/core/security/app-secrets?view=aspnetcore-7.0&tabs=windows

``` shell
dotnet user-secrets init --project ".\src\server\Processor"
dotnet user-secrets set "NServiceBusConfig:SharedAccessKey" "shared-key-here" --project ".\src\server\Processor"

dotnet user-secrets init --project ".\src\server\ClientUI"
dotnet user-secrets set "NServiceBusConfig:SharedAccessKey" "shared-key-here" --project ".\src\server\ClientUI"

dotnet user-secrets list
```