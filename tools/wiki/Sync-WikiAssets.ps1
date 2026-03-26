param(
  [string]$ProjectRoot = (Resolve-Path (Join-Path $PSScriptRoot "..\\..")).Path
)

$docsRoot = Join-Path $ProjectRoot "docs"
$imagesRoot = Join-Path $docsRoot "assets\\images"
$contentRoot = Join-Path $ProjectRoot "Content"
$contentTarget = Join-Path $imagesRoot "content"
$gallerySource = Join-Path $ProjectRoot ".github\\readme\\gallery"
$galleryTarget = Join-Path $imagesRoot "gallery"

New-Item -ItemType Directory -Force $imagesRoot, $contentTarget, $galleryTarget | Out-Null

Copy-Item (Join-Path $ProjectRoot "icon.png") (Join-Path $imagesRoot "icon.png") -Force
Copy-Item (Join-Path $ProjectRoot "icon_small.png") (Join-Path $imagesRoot "favicon.png") -Force
Copy-Item (Join-Path $ProjectRoot ".github\\readme\\banner.png") (Join-Path $imagesRoot "banner.png") -Force
Get-ChildItem $gallerySource -Filter *.png | Copy-Item -Destination $galleryTarget -Force

Get-ChildItem $contentRoot -Recurse -Filter *.png | ForEach-Object {
  $relative = $_.FullName.Substring($contentRoot.Length).TrimStart('\')
  $destination = Join-Path $contentTarget $relative
  $destinationDir = Split-Path $destination -Parent

  New-Item -ItemType Directory -Force $destinationDir | Out-Null
  Copy-Item $_.FullName $destination -Force
}

Write-Host "Wiki assets synced to $imagesRoot"
