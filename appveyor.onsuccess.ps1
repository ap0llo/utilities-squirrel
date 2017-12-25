$isAppVeyorBuild = [bool]$env:APPVEYOR 

if(!$isAppVeyorBuild)
{
    Write-Host "Not running on appveyor, aborting"
    exit
}

$branchName = $env:APPVEYOR_REPO_BRANCH
$version = $env:APPVEYOR_BUILD_VERSION
$commit = $env:APPVEYOR_REPO_COMMIT
$tagName = "v$($version)"

if($branchName -ne "master")
{
    Write-Host "Skipping tagging for branch $branchName"
    exit
}

Write-Host "Adding tag '$($tagName)' for commit $($commit)"
Invoke-Expression "git config --global credential.helper store"
Add-Content "$env:USERPROFILE\.git-credentials" "https://$($env:access_token):x-oauth-basic@github.com`n"  
Invoke-Expression "git tag $($tagName) $($commit)"
Invoke-Expression "git push origin $($tagName)" -ErrorAction SilentlyContinue 2> $null