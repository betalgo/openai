param(
    [Parameter(Mandatory = $true)]
    [string]$ProjectFilePath,

    [string]$PackageName,

    [string]$VersionFilePath,

    [string]$VersionRegex = '^\s*<Version>(.*)</Version>\s*$',

    [string]$VersionStatic,

    [bool]$TagCommit = $true,

    [string]$TagFormat = 'v*',

    [string]$NugetKey,

    [string]$NugetSource = 'https://api.nuget.org',

    [bool]$IncludeSymbols = $false,

    [bool]$NoBuild = $false,

    [bool]$PublishToGitHubPackages = $false,

    [string]$GitHubPackagesOwner,

    [string]$GitHubPackagesApiKey,

    [string]$GitHubPackagesSource,

    [bool]$GitHubPackagesIncludeSymbols = $false,

    [System.Text.RegularExpressions.RegexOptions]$VersionRegexOptions = [System.Text.RegularExpressions.RegexOptions]::Multiline
)

$ErrorActionPreference = 'Stop'

function Set-GithubOutput {
    param(
        [string]$Name,
        [string]$Value
    )

    if (-not $env:GITHUB_OUTPUT) {
        return
    }

    "$Name=$Value" | Out-File -FilePath $env:GITHUB_OUTPUT -Append -Encoding utf8
}

function Invoke-CommandChecked {
    param(
        [string]$Command,
        [string[]]$Arguments
    )

    $logArgs = @()
    for ($i = 0; $i -lt $Arguments.Count; $i++) {
        $current = $Arguments[$i]
        if ($current -in @("--api-key", "-k")) {
            $logArgs += $current
            if ($i + 1 -lt $Arguments.Count) {
                $logArgs += "***"
                $i++
            }
            continue
        }
        $logArgs += $current
    }

    $commandDisplay = "$Command $($logArgs -join ' ')"
    Write-Host "Executing: $commandDisplay"
    $process = Start-Process -FilePath $Command -ArgumentList $Arguments -NoNewWindow -Wait -PassThru
    if ($process.ExitCode -ne 0) {
        throw "Command '$commandDisplay' failed with exit code $($process.ExitCode)."
    }
}

if (-not (Test-Path $ProjectFilePath)) {
    throw "Project file not found: $ProjectFilePath"
}

$resolvedProjectPath = (Resolve-Path $ProjectFilePath).ProviderPath
if (-not $PackageName) {
    $PackageName = [IO.Path]::GetFileNameWithoutExtension($resolvedProjectPath)
}

$versionFilePath = if ($VersionFilePath) { $VersionFilePath } else { $resolvedProjectPath }

if (-not $VersionStatic) {
    if (-not (Test-Path $versionFilePath)) {
        throw "Version file not found: $versionFilePath"
    }

    $fileContent = Get-Content $versionFilePath -Raw
    $match = [regex]::Match($fileContent, $VersionRegex, $VersionRegexOptions)
    if (-not $match.Success -or -not $match.Groups[1].Value) {
        throw "Unable to extract version information using regex '$VersionRegex' from '$versionFilePath'."
    }
    $Version = $match.Groups[1].Value.Trim()
} else {
    $Version = $VersionStatic
}

Write-Host "Project: $ProjectFilePath"
Write-Host "Package: $PackageName"
Write-Host "Version: $Version"

Set-GithubOutput -Name "package_name" -Value $PackageName
Set-GithubOutput -Name "version" -Value $Version

$packageIdLower = $PackageName.ToLowerInvariant()
$versionsUri = "$NugetSource/v3-flatcontainer/$packageIdLower/index.json"

$existingVersions = @()
try {
    $response = Invoke-RestMethod -Method Get -Uri $versionsUri -ErrorAction Stop
    if ($response -and $response.versions) {
        $existingVersions = $response.versions
    }
} catch [System.Net.WebException] {
    $response = $_.Exception.Response
    if ($response -and $response.StatusCode -eq [System.Net.HttpStatusCode]::NotFound) {
        Write-Host "Package not found on feed. Treating as first release."
    } else {
        throw
    }
}

$shouldPublish = -not ($existingVersions -contains $Version)
Set-GithubOutput -Name "should_publish" -Value ($shouldPublish.ToString().ToLowerInvariant())

if (-not $shouldPublish) {
    Write-Host "Version $Version already exists on $NugetSource. Skipping publish."
    Set-GithubOutput -Name "published" -Value "false"
    return
}

if (-not $NugetKey) {
    Write-Warning "NUGET_KEY was not provided; skipping publish."
    Set-GithubOutput -Name "published" -Value "false"
    return
}

$outputDir = Join-Path ([IO.Path]::GetDirectoryName($resolvedProjectPath)) "nupkg"
if (-not (Test-Path $outputDir)) {
    New-Item -ItemType Directory -Path $outputDir | Out-Null
}

Get-ChildItem -Path $outputDir -Filter '*.nupkg' -File -ErrorAction SilentlyContinue | Remove-Item -Force -ErrorAction SilentlyContinue
Get-ChildItem -Path $outputDir -Filter '*.snupkg' -File -ErrorAction SilentlyContinue | Remove-Item -Force -ErrorAction SilentlyContinue

if (-not $NoBuild) {
    Invoke-CommandChecked -Command "dotnet" -Arguments @("build", "-c", "Release", $resolvedProjectPath)
}

$packArguments = @("pack", $resolvedProjectPath, "-c", "Release", "-o", $outputDir)
if ($NoBuild) {
    # Assume restore was done earlier in the job; avoid re-building or re-restoring
    $packArguments += @("--no-build", "--no-restore")
}
if ($IncludeSymbols) {
    $packArguments += @("--include-symbols", "-p:SymbolPackageFormat=snupkg")
}
Invoke-CommandChecked -Command "dotnet" -Arguments $packArguments

$packagePattern = "$PackageName.$Version*.nupkg"
$packageFile = Get-ChildItem -Path $outputDir -Filter $packagePattern -File | Where-Object { $_.Extension -eq '.nupkg' } | Select-Object -First 1

if (-not $packageFile) {
    $packageFile = Get-ChildItem -Path $outputDir -Filter '*.nupkg' -File | Select-Object -First 1
}

if (-not $packageFile) {
    throw "No package file found in $outputDir."
}

$symbolFile = $null
if ($IncludeSymbols) {
    $symbolPattern = "$PackageName.$Version*.snupkg"
    $symbolFile = Get-ChildItem -Path $outputDir -Filter $symbolPattern -File | Select-Object -First 1
    if (-not $symbolFile) {
        $symbolFile = Get-ChildItem -Path $outputDir -Filter '*.snupkg' -File | Select-Object -First 1
    }
}

$sourcePushUri = "$NugetSource/v3/index.json"
Invoke-CommandChecked -Command "dotnet" -Arguments @("nuget", "push", $packageFile.FullName, "--source", $sourcePushUri, "--api-key", $NugetKey, "--skip-duplicate", "--no-symbols")

if ($IncludeSymbols -and $symbolFile) {
    Invoke-CommandChecked -Command "dotnet" -Arguments @("nuget", "push", $symbolFile.FullName, "--source", $sourcePushUri, "--api-key", $NugetKey, "--skip-duplicate")
}

if ($PublishToGitHubPackages) {
    if (-not $GitHubPackagesOwner) {
        $GitHubPackagesOwner = $env:GITHUB_REPOSITORY_OWNER
    }

    if (-not $GitHubPackagesSource -and $GitHubPackagesOwner) {
        $GitHubPackagesSource = "https://nuget.pkg.github.com/$GitHubPackagesOwner/index.json"
    }

    if (-not $GitHubPackagesApiKey) {
        Write-Warning "GitHub Packages API key/token not provided; skipping GitHub Packages publish."
    } elseif (-not $GitHubPackagesSource) {
        Write-Warning "GitHub Packages source could not be determined; skipping GitHub Packages publish."
    } else {
        Write-Host "Publishing to GitHub Packages source $GitHubPackagesSource"
        Invoke-CommandChecked -Command "dotnet" -Arguments @("nuget", "push", $packageFile.FullName, "--source", $GitHubPackagesSource, "--api-key", $GitHubPackagesApiKey, "--skip-duplicate", "--no-symbols")
        if ($GitHubPackagesIncludeSymbols -and $symbolFile) {
            Invoke-CommandChecked -Command "dotnet" -Arguments @("nuget", "push", $symbolFile.FullName, "--source", $GitHubPackagesSource, "--api-key", $GitHubPackagesApiKey, "--skip-duplicate")
        }
        Set-GithubOutput -Name "github_packages_source" -Value $GitHubPackagesSource
    }
}

Set-GithubOutput -Name "published" -Value "true"
Set-GithubOutput -Name "package_path" -Value $packageFile.FullName
if ($symbolFile) {
    Set-GithubOutput -Name "symbols_package_path" -Value $symbolFile.FullName
}

if ($TagCommit) {
    $tagName = $TagFormat.Replace("*", $Version)
    Invoke-CommandChecked -Command "git" -Arguments @("config", "user.name", "github-actions")
    Invoke-CommandChecked -Command "git" -Arguments @("config", "user.email", "actions@github.com")
    Invoke-CommandChecked -Command "git" -Arguments @("tag", $tagName)
    Invoke-CommandChecked -Command "git" -Arguments @("push", "origin", $tagName)
    Set-GithubOutput -Name "tag" -Value $tagName
}

