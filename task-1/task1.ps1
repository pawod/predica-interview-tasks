param([string] $sid, [string] $rsGroup, [string]$rsTypes = "Microsoft.web/sites,Microsoft.sql/servers")
Import-Module Az.Accounts

if([string]::IsNullOrEmpty($sid))
{ 
    Write-Error("subscription id not specified.")
}
if([string]::IsNullOrEmpty($rsGroup))
{ 
    Write-Error("resource group not specified.")
}

try 
{
    Write-Host "Setting subscription context..."
    Get-AzSubscription -SubscriptionId $sid | Set-AzContext
}
catch [System.Management.Automation.PSInvalidOperationException]
{
    Write-Host "Please sign in."
    Connect-AzAccount
    Get-AzSubscription -SubscriptionId $sid | Set-AzContext
}
Write-Host "`nConnection established."


try 
{   
    $cond = ""
    foreach($type in $rsTypes.Split(","))
    {
        if ([string]::IsNullOrEmpty($type)) { continue }
        $cond += "`$_.ResourceType -ieq {$type} -or "
    }
    $exp = "Get-AzResource -ResourceGroupName $rsGroup | where {" + $cond.Substring(0, $cond.Length-5) + "}"
    
    Write-Host "Querying API..."
    $result = Invoke-Expression $exp

    if (!$result)
    {
        Write-Host "No Resources found."
    }
    else
    {
        Write-Host "Removing resources..."
        foreach($item in $result)
        {
            Write-Host "`t* Name: $($item.Name), ResourceGroupName: $($item.ResourceGroupName)"
            Write-Host "`t  Success: " $(Remove-AzResource -ResourceId $item.id -Force)
        }
    }   
}
finally
{
    Write-Host "`n"
    Disconnect-AzAccount
}
