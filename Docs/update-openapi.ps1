$Url = "https://app.stainless.com/api/spec/documented/openai/openapi.documented.yml"
$OutputFile = "openapi.documented.yml"
$TempFile = "$OutputFile.tmp"
$VersionFolder = "OpenapiDocumentedVersions"

Write-Host "Downloading $Url ..."
Invoke-WebRequest -Uri $Url -OutFile $TempFile -UseBasicParsing

# Ensure version folder exists
if (-not (Test-Path $VersionFolder)) {
    New-Item -ItemType Directory -Path $VersionFolder | Out-Null
}

# Check if existing file exists
if (Test-Path $OutputFile) {
    $oldContent = Get-Content $OutputFile -Raw
        $newContent = Get-Content $TempFile -Raw

    if ($oldContent -ne $newContent) {
# Files differ: version the new one with timestamp
        $timestamp = Get-Date -Format "yyyyMMdd_HHmmss"
            $versionedFile = Join-Path $VersionFolder ($OutputFile.Replace(".yml", "_$timestamp.yml"))
        
        Copy-Item -Path $TempFile -Destination $versionedFile -Force
        Write-Host "Change detected. New version also saved as $versionedFile"

        Move-Item -Path $TempFile -Destination $OutputFile -Force
        Write-Host "Updated $OutputFile with new version"
    }
    else {
# Same content
        Remove-Item $TempFile
        Write-Host "No changes detected. Existing file left untouched."
    }
}
else {
# No existing file: save it in place
    Move-Item -Path $TempFile -Destination $OutputFile -Force
    Write-Host "No existing file. Saved new file as $OutputFile"
}