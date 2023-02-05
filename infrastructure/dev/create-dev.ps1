$resourceGroup = "rg-tasklocker-dev-centralus-001"
$location="centralus"
$serviceBusName = "sb-tasklocker-dev-centralus-001"
$tags = "Environment=Dev"

az group create --location $location --name $resourceGroup --tags $tags

az servicebus namespace create --resource-group $resourceGroup --name $serviceBusName --location $location --tags $tags

