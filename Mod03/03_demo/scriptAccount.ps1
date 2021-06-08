# Prompts for a region for the resources to be created
# and generates a random name for the resources using
# "az204" as the prefix for easy identification in the portal

$myLocation = Read-Host -Prompt "Enter the region (i.e. westus): "
$myResourceGroup = "newRSG"
$myStorageAcct = "newRSG" + $(get-random -minimum 10000 -maximum 100000)

# Create the resource group
# New-AzResourceGroup -Name $myResourceGroup -Location $myLocation

# Create the storage account
New-AzStorageAccount -ResourceGroupName $myResourceGroup -Name $myStorageAcct `
    -Location $myLocation -SkuName "Standard_LRS"


Write-Host  "`nNote the following resource group, and storage account names, you will use them in the code examples below.
    Resource group: $myResourceGroup
    Storage account: $myStorageAcct"