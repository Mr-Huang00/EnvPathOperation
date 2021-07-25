# EnvPathOperation
For easy to change Environment Path by cmdlet.
# Example
> ```powershell
Import-Module ./CmdletEnvPath.dll
$path="E:\mydir"
# add path,and return old paths.
Add-EnvPath -Path $path -Target User
# get paths of User
Get-EnvPath -Target User
# remove path and get the newest paths.
Remove-EnvPath -Path $path

```