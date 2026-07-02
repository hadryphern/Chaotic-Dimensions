Add-Type -AssemblyName System.Drawing

function New-Color([string]$hex) {
	return [System.Drawing.ColorTranslator]::FromHtml($hex)
}

function Get-Palette([string]$type) {
	switch ($type) {
		'Common' {
			return @(
				(New-Color '#1a0823'),
				(New-Color '#3a1350'),
				(New-Color '#7f38c7'),
				(New-Color '#c777ff'),
				(New-Color '#f3d8ff')
			)
		}
		'Melee' {
			return @(
				(New-Color '#21071f'),
				(New-Color '#4d1246'),
				(New-Color '#8f35a2'),
				(New-Color '#e37bce'),
				(New-Color '#ffe1f4')
			)
		}
		'Magic' {
			return @(
				(New-Color '#100d26'),
				(New-Color '#253067'),
				(New-Color '#5f6fda'),
				(New-Color '#9de3ff'),
				(New-Color '#f1ffff')
			)
		}
		'Ranged' {
			return @(
				(New-Color '#081b1e'),
				(New-Color '#12404c'),
				(New-Color '#2d7f9a'),
				(New-Color '#79f0ff'),
				(New-Color '#e7ffff')
			)
		}
		'Summoner' {
			return @(
				(New-Color '#14091e'),
				(New-Color '#38205b'),
				(New-Color '#8a4ad8'),
				(New-Color '#d5a2ff'),
				(New-Color '#fff1ff')
			)
		}
		default {
			throw "Unknown palette type: $type"
		}
	}
}

function Load-Bitmap([string]$path) {
	return [System.Drawing.Bitmap]::FromFile($path)
}

function Save-Bitmap([System.Drawing.Bitmap]$bitmap, [string]$path) {
	$directory = Split-Path -Parent $path
	if (-not (Test-Path $directory)) {
		New-Item -ItemType Directory -Path $directory -Force | Out-Null
	}

	$bitmap.Save($path, [System.Drawing.Imaging.ImageFormat]::Png)
}

function Convert-ToDevourPalette {
	param(
		[System.Drawing.Bitmap]$Source,
		[System.Drawing.Color[]]$Palette
	)

	$result = New-Object System.Drawing.Bitmap($Source.Width, $Source.Height, [System.Drawing.Imaging.PixelFormat]::Format32bppArgb)
	for ($x = 0; $x -lt $Source.Width; $x++) {
		for ($y = 0; $y -lt $Source.Height; $y++) {
			$pixel = $Source.GetPixel($x, $y)
			if ($pixel.A -eq 0) {
				continue
			}

			$brightness = ((0.299 * $pixel.R) + (0.587 * $pixel.G) + (0.114 * $pixel.B)) / 255.0
			$index =
				if ($brightness -lt 0.18) { 0 }
				elseif ($brightness -lt 0.35) { 1 }
				elseif ($brightness -lt 0.58) { 2 }
				elseif ($brightness -lt 0.8) { 3 }
				else { 4 }

			$target = $Palette[$index]
			$result.SetPixel($x, $y, [System.Drawing.Color]::FromArgb($pixel.A, $target.R, $target.G, $target.B))
		}
	}

	return $result
}

function Add-HeadCrystals {
	param(
		[System.Drawing.Bitmap]$Bitmap,
		[System.Drawing.Color]$Highlight
	)

	$frameHeight = 56
	$frameCount = [int]($Bitmap.Height / $frameHeight)
	$points = @(
		[System.Drawing.Point]::new(18, 7),
		[System.Drawing.Point]::new(19, 6),
		[System.Drawing.Point]::new(20, 7),
		[System.Drawing.Point]::new(18, 14),
		[System.Drawing.Point]::new(21, 14)
	)

	for ($frame = 0; $frame -lt $frameCount; $frame++) {
		$offsetY = $frame * $frameHeight
		foreach ($point in $points) {
			$x = $point.X
			$y = $point.Y + $offsetY
			if ($x -lt $Bitmap.Width -and $y -lt $Bitmap.Height) {
				$existing = $Bitmap.GetPixel($x, $y)
				if ($existing.A -gt 0) {
					$Bitmap.SetPixel($x, $y, [System.Drawing.Color]::FromArgb($existing.A, $Highlight.R, $Highlight.G, $Highlight.B))
				}
			}
		}
	}
}

function Add-BodyCrystals {
	param(
		[System.Drawing.Bitmap]$Bitmap,
		[System.Drawing.Color]$Highlight
	)

	$points = @(
		[System.Drawing.Point]::new(17, 8),
		[System.Drawing.Point]::new(21, 8),
		[System.Drawing.Point]::new(17, 19),
		[System.Drawing.Point]::new(21, 19),
		[System.Drawing.Point]::new(95, 7),
		[System.Drawing.Point]::new(127, 7),
		[System.Drawing.Point]::new(303, 8),
		[System.Drawing.Point]::new(337, 8)
	)

	foreach ($point in $points) {
		if ($point.X -lt $Bitmap.Width -and $point.Y -lt $Bitmap.Height) {
			$existing = $Bitmap.GetPixel($point.X, $point.Y)
			if ($existing.A -gt 0) {
				$Bitmap.SetPixel($point.X, $point.Y, [System.Drawing.Color]::FromArgb($existing.A, $Highlight.R, $Highlight.G, $Highlight.B))
			}
		}
	}
}

function Add-LegGlow {
	param(
		[System.Drawing.Bitmap]$Bitmap,
		[System.Drawing.Color]$Highlight
	)

	$frameHeight = 56
	$frameCount = [int]($Bitmap.Height / $frameHeight)
	$points = @(
		[System.Drawing.Point]::new(15, 42),
		[System.Drawing.Point]::new(24, 42)
	)

	for ($frame = 0; $frame -lt $frameCount; $frame++) {
		$offsetY = $frame * $frameHeight
		foreach ($point in $points) {
			$x = $point.X
			$y = $point.Y + $offsetY
			if ($x -lt $Bitmap.Width -and $y -lt $Bitmap.Height) {
				$existing = $Bitmap.GetPixel($x, $y)
				if ($existing.A -gt 0) {
					$Bitmap.SetPixel($x, $y, [System.Drawing.Color]::FromArgb($existing.A, $Highlight.R, $Highlight.G, $Highlight.B))
				}
			}
		}
	}
}

function Get-BoundingBox([System.Drawing.Bitmap]$Bitmap, [System.Drawing.Rectangle]$Bounds) {
	$minX = $Bounds.Right
	$minY = $Bounds.Bottom
	$maxX = $Bounds.Left
	$maxY = $Bounds.Top

	for ($x = $Bounds.Left; $x -lt $Bounds.Right; $x++) {
		for ($y = $Bounds.Top; $y -lt $Bounds.Bottom; $y++) {
			$pixel = $Bitmap.GetPixel($x, $y)
			if ($pixel.A -eq 0) {
				continue
			}

			$minX = [Math]::Min($minX, $x)
			$minY = [Math]::Min($minY, $y)
			$maxX = [Math]::Max($maxX, $x)
			$maxY = [Math]::Max($maxY, $y)
		}
	}

	if ($minX -gt $maxX -or $minY -gt $maxY) {
		return [System.Drawing.Rectangle]::new($Bounds.Left, $Bounds.Top, 1, 1)
	}

	return [System.Drawing.Rectangle]::new($minX, $minY, ($maxX - $minX) + 1, ($maxY - $minY) + 1)
}

function Create-ItemIcon {
	param(
		[System.Drawing.Bitmap]$Source,
		[System.Drawing.Rectangle]$SearchBounds
	)

	$bounds = Get-BoundingBox -Bitmap $Source -Bounds $SearchBounds
	$icon = New-Object System.Drawing.Bitmap(32, 32, [System.Drawing.Imaging.PixelFormat]::Format32bppArgb)
	$graphics = [System.Drawing.Graphics]::FromImage($icon)
	$graphics.Clear([System.Drawing.Color]::Transparent)
	$graphics.InterpolationMode = [System.Drawing.Drawing2D.InterpolationMode]::NearestNeighbor
	$graphics.PixelOffsetMode = [System.Drawing.Drawing2D.PixelOffsetMode]::Half
	$graphics.SmoothingMode = [System.Drawing.Drawing2D.SmoothingMode]::None

	$scale = [Math]::Min(28.0 / $bounds.Width, 28.0 / $bounds.Height)
	$drawWidth = [Math]::Max(1, [int]([Math]::Round($bounds.Width * $scale)))
	$drawHeight = [Math]::Max(1, [int]([Math]::Round($bounds.Height * $scale)))
	$drawX = [int]([Math]::Floor((32 - $drawWidth) / 2))
	$drawY = [int]([Math]::Floor((32 - $drawHeight) / 2))

	$graphics.DrawImage(
		$Source,
		[System.Drawing.Rectangle]::new($drawX, $drawY, $drawWidth, $drawHeight),
		$bounds,
		[System.Drawing.GraphicsUnit]::Pixel)

	$graphics.Dispose()
	return $icon
}

function Create-TemplateIcon {
	param(
		[string[]]$Template,
		[hashtable]$Palette
	)

	$bitmap = New-Object System.Drawing.Bitmap(32, 32, [System.Drawing.Imaging.PixelFormat]::Format32bppArgb)
	for ($y = 0; $y -lt $Template.Length; $y++) {
		$row = $Template[$y]
		for ($x = 0; $x -lt $row.Length; $x++) {
			$key = [string]$row[$x]
			if (-not $Palette.ContainsKey($key)) {
				continue
			}

			$color = $Palette[$key]
			if ($color.A -eq 0) {
				continue
			}

			for ($py = 0; $py -lt 2; $py++) {
				for ($px = 0; $px -lt 2; $px++) {
					$bitmap.SetPixel(($x * 2) + $px, ($y * 2) + $py, $color)
				}
			}
		}
	}

	return $bitmap
}

$root = Resolve-Path (Join-Path $PSScriptRoot '..')
$vanillaDir = 'C:\Users\happi\Downloads\terraria_sprites'
$armorDir = Join-Path $root 'Content\Items\Armor\CrystalineDevour'
$previewPath = Join-Path $root 'Assets\UI\CrystalineDevourArmorPreview.png'

$bodySource = Load-Bitmap (Join-Path $vanillaDir 'Armor\Armor_170.png')
$legsSource = Load-Bitmap (Join-Path $vanillaDir 'Armor_Legs_141.png')
$headSources = @{
	CrystalineDevourMeleeHelm = Load-Bitmap (Join-Path $vanillaDir 'Armor_Head_171.png')
	CrystalineDevourMagicHelm = Load-Bitmap (Join-Path $vanillaDir 'Armor_Head_170.png')
	CrystalineDevourRangedHelm = Load-Bitmap (Join-Path $vanillaDir 'Armor_Head_169.png')
	CrystalineDevourSummonerHelm = Load-Bitmap (Join-Path $vanillaDir 'Armor_Head_172.png')
}

$commonPalette = Get-Palette 'Common'
$bodyConverted = Convert-ToDevourPalette -Source $bodySource -Palette $commonPalette
$legsConverted = Convert-ToDevourPalette -Source $legsSource -Palette $commonPalette
Add-BodyCrystals -Bitmap $bodyConverted -Highlight $commonPalette[4]
Add-LegGlow -Bitmap $legsConverted -Highlight $commonPalette[4]

Save-Bitmap -Bitmap $bodyConverted -Path (Join-Path $armorDir 'CrystalineDevourBreastplate_Body.png')
Save-Bitmap -Bitmap $bodyConverted -Path (Join-Path $armorDir 'CrystalineDevourBreastplate_FemaleBody.png')
Save-Bitmap -Bitmap $bodyConverted -Path (Join-Path $armorDir 'CrystalineDevourBreastplate_Arms.png')
Save-Bitmap -Bitmap $legsConverted -Path (Join-Path $armorDir 'CrystalineDevourGreaves_Legs.png')

$bodyIconTemplate = @(
	"................",
	"......ommo......",
	".....omllmo.....",
	"....omlgglmo....",
	"...ommbbbbmmo...",
	"...obbbbbbbbbo..",
	"..obbbloommbbbo.",
	"..obbbmggmmbbbo.",
	"..obbbbbbbbbbbo.",
	"...obbbddbbbbo..",
	"...obbddddbbo...",
	"...obbo..obbo...",
	"....oo....oo....",
	"...ooo....ooo...",
	"..oo........oo..",
	"................"
)
$legsIconTemplate = @(
	"................",
	"................",
	"...oo......oo...",
	"..ommo....ommo..",
	"..obbo....obbo..",
	"..obbo....obbo..",
	"..obbo....obbo..",
	"..obbo....obbo..",
	".omddmo..omddmo.",
	".obbbbo..obbbbo.",
	".obbbbo..obbbbo.",
	".obllbo..obllbo.",
	"..ommo....ommo..",
	"..ommo....ommo..",
	"...oo......oo...",
	"................"
)
$iconPalette = @{
	'.' = [System.Drawing.Color]::Transparent
	'o' = $commonPalette[0]
	'd' = $commonPalette[1]
	'b' = $commonPalette[2]
	'm' = $commonPalette[3]
	'l' = $commonPalette[4]
	'g' = [System.Drawing.Color]::White
}

$bodyIcon = Create-TemplateIcon -Template $bodyIconTemplate -Palette $iconPalette
Save-Bitmap -Bitmap $bodyIcon -Path (Join-Path $armorDir 'CrystalineDevourBreastplate.png')
$bodyIcon.Dispose()

$legsIcon = Create-TemplateIcon -Template $legsIconTemplate -Palette $iconPalette
Save-Bitmap -Bitmap $legsIcon -Path (Join-Path $armorDir 'CrystalineDevourGreaves.png')
$legsIcon.Dispose()

$helmetPaletteTypes = @{
	CrystalineDevourMeleeHelm = 'Melee'
	CrystalineDevourMagicHelm = 'Magic'
	CrystalineDevourRangedHelm = 'Ranged'
	CrystalineDevourSummonerHelm = 'Summoner'
}

foreach ($helmetName in $headSources.Keys) {
	$palette = Get-Palette $helmetPaletteTypes[$helmetName]
	$helmetBitmap = Convert-ToDevourPalette -Source $headSources[$helmetName] -Palette $palette
	Add-HeadCrystals -Bitmap $helmetBitmap -Highlight $palette[4]
	Save-Bitmap -Bitmap $helmetBitmap -Path (Join-Path $armorDir ($helmetName + '_Head.png'))

	$helmetIcon = Create-ItemIcon -Source $helmetBitmap -SearchBounds ([System.Drawing.Rectangle]::new(0, 0, 40, 56))
	Save-Bitmap -Bitmap $helmetIcon -Path (Join-Path $armorDir ($helmetName + '.png'))
	$helmetIcon.Dispose()
	$helmetBitmap.Dispose()
}

$preview = New-Object System.Drawing.Bitmap(360, 170, [System.Drawing.Imaging.PixelFormat]::Format32bppArgb)
$graphics = [System.Drawing.Graphics]::FromImage($preview)
$graphics.Clear((New-Color '#12061d'))
$graphics.InterpolationMode = [System.Drawing.Drawing2D.InterpolationMode]::NearestNeighbor
$graphics.PixelOffsetMode = [System.Drawing.Drawing2D.PixelOffsetMode]::Half
$graphics.SmoothingMode = [System.Drawing.Drawing2D.SmoothingMode]::None

$previewFiles = @(
	'CrystalineDevourMeleeHelm.png',
	'CrystalineDevourMagicHelm.png',
	'CrystalineDevourRangedHelm.png',
	'CrystalineDevourSummonerHelm.png',
	'CrystalineDevourBreastplate.png',
	'CrystalineDevourGreaves.png'
)

for ($i = 0; $i -lt $previewFiles.Count; $i++) {
	$image = [System.Drawing.Image]::FromFile((Join-Path $armorDir $previewFiles[$i]))
	$x = 18 + (($i % 3) * 110)
	$y = 22 + ([int]($i / 3) * 70)
	$graphics.DrawImage($image, $x, $y, 42, 42)
	$image.Dispose()
}

$preview.Save($previewPath, [System.Drawing.Imaging.ImageFormat]::Png)
$graphics.Dispose()
$preview.Dispose()

$bodyConverted.Dispose()
$legsConverted.Dispose()
$bodySource.Dispose()
$legsSource.Dispose()
foreach ($helmet in $headSources.Values) {
	$helmet.Dispose()
}
