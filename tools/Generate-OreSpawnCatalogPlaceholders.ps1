using namespace System.Drawing
using namespace System.Drawing.Drawing2D
using namespace System.Drawing.Imaging

Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

Add-Type -AssemblyName System.Drawing

$repoRoot = Split-Path -Parent $PSScriptRoot

function New-Canvas([int]$width, [int]$height) {
	$bitmap = [Bitmap]::new($width, $height, [PixelFormat]::Format32bppArgb)
	$graphics = [Graphics]::FromImage($bitmap)
	$graphics.Clear([Color]::Transparent)
	$graphics.SmoothingMode = [SmoothingMode]::None
	$graphics.InterpolationMode = [InterpolationMode]::NearestNeighbor
	$graphics.PixelOffsetMode = [PixelOffsetMode]::Half
	$graphics.CompositingQuality = [CompositingQuality]::HighSpeed

	return @{
		Bitmap = $bitmap
		Graphics = $graphics
	}
}

function Save-Canvas($canvas, [string]$relativePath) {
	$path = Join-Path $repoRoot $relativePath
	$directory = Split-Path -Parent $path
	if (-not (Test-Path $directory)) {
		New-Item -ItemType Directory -Force -Path $directory | Out-Null
	}

	$canvas.Bitmap.Save($path, [ImageFormat]::Png)
	$canvas.Graphics.Dispose()
	$canvas.Bitmap.Dispose()
}

function Get-Theme([string]$seed) {
	$hash = [Math]::Abs($seed.GetHashCode())
	$palette = @(
		@{ Main = [ColorTranslator]::FromHtml('#4EC9B0'); Dark = [ColorTranslator]::FromHtml('#0F3F3B'); Light = [ColorTranslator]::FromHtml('#C5FFF2') },
		@{ Main = [ColorTranslator]::FromHtml('#F4B942'); Dark = [ColorTranslator]::FromHtml('#5A3F0A'); Light = [ColorTranslator]::FromHtml('#FFF0BF') },
		@{ Main = [ColorTranslator]::FromHtml('#FF6F91'); Dark = [ColorTranslator]::FromHtml('#5A1F31'); Light = [ColorTranslator]::FromHtml('#FFD0DD') },
		@{ Main = [ColorTranslator]::FromHtml('#7AA2F7'); Dark = [ColorTranslator]::FromHtml('#1C2A4A'); Light = [ColorTranslator]::FromHtml('#D8E4FF') },
		@{ Main = [ColorTranslator]::FromHtml('#9CE05A'); Dark = [ColorTranslator]::FromHtml('#29480B'); Light = [ColorTranslator]::FromHtml('#E0FFC0') },
		@{ Main = [ColorTranslator]::FromHtml('#D38BFF'); Dark = [ColorTranslator]::FromHtml('#43205A'); Light = [ColorTranslator]::FromHtml('#F0D7FF') }
	)

	return $palette[$hash % $palette.Count]
}

function Fill-Rect([Graphics]$graphics, [Color]$color, [int]$x, [int]$y, [int]$width, [int]$height) {
	$brush = [SolidBrush]::new($color)
	$graphics.FillRectangle($brush, $x, $y, $width, $height)
	$brush.Dispose()
}

function Fill-Ellipse([Graphics]$graphics, [Color]$color, [int]$x, [int]$y, [int]$width, [int]$height) {
	$brush = [SolidBrush]::new($color)
	$graphics.FillEllipse($brush, $x, $y, $width, $height)
	$brush.Dispose()
}

function Fill-Polygon([Graphics]$graphics, [Color]$color, [Point[]]$points) {
	$brush = [SolidBrush]::new($color)
	$graphics.FillPolygon($brush, $points)
	$brush.Dispose()
}

function Draw-Border([Graphics]$graphics, [Color]$color, [int]$width, [int]$height) {
	$pen = [Pen]::new($color)
	$pen.Alignment = [PenAlignment]::Inset
	$graphics.DrawRectangle($pen, 0, 0, $width - 1, $height - 1)
	$pen.Dispose()
}

function New-PixelFont([float]$size) {
	return [Font]::new("Consolas", $size, [FontStyle]::Bold, [GraphicsUnit]::Pixel)
}

function Draw-CenteredLabel([Graphics]$graphics, [string]$text, [Color]$color, [RectangleF]$layout) {
	$font = New-PixelFont 12
	$brush = [SolidBrush]::new($color)
	$format = [StringFormat]::new()
	$format.Alignment = [StringAlignment]::Center
	$format.LineAlignment = [StringAlignment]::Center
	$graphics.DrawString($text, $font, $brush, $layout, $format)
	$format.Dispose()
	$brush.Dispose()
	$font.Dispose()
}

function Get-ShortLabel([string]$key) {
	$letters = ($key -replace '[^A-Z]', '')
	if ([string]::IsNullOrWhiteSpace($letters)) {
		$letters = $key.Substring(0, [Math]::Min(2, $key.Length)).ToUpperInvariant()
	}
	elseif ($letters.Length -gt 2) {
		$letters = $letters.Substring(0, 2)
	}
	return $letters.ToUpperInvariant()
}

function Draw-NpcSheet([string]$relativePath, [string]$key, [int]$frameWidth, [int]$frameHeight, [string]$category) {
	$theme = Get-Theme $key
	$totalHeight = $frameHeight * 4
	$canvas = New-Canvas $frameWidth $totalHeight
	$g = $canvas.Graphics
	$shadow = [ColorTranslator]::FromHtml('#10131A')
	$label = Get-ShortLabel $key

	for ($frame = 0; $frame -lt 4; $frame++) {
		$top = $frame * $frameHeight
		$step = $frame % 2
		Fill-Rect $g ([Color]::FromArgb(55, $theme.Dark)) 1 ($top + 1) ($frameWidth - 2) ($frameHeight - 2)
		Draw-Border $g $theme.Dark $frameWidth $frameHeight

		switch ($category) {
			'Boss' {
				Fill-Ellipse $g $shadow ([Math]::Max(2, [int]($frameWidth * 0.15))) ($top + [int]($frameHeight * 0.15)) ([int]($frameWidth * 0.7)) ([int]($frameHeight * 0.55))
				Fill-Ellipse $g $theme.Main ([Math]::Max(4, [int]($frameWidth * 0.2))) ($top + [int]($frameHeight * 0.2) + $step) ([int]($frameWidth * 0.6)) ([int]($frameHeight * 0.45))
				Fill-Polygon $g $theme.Light ([Point[]]@(
					[Point]::new([int]($frameWidth * 0.5), $top + [int]($frameHeight * 0.12)),
					[Point]::new([int]($frameWidth * 0.65), $top + [int]($frameHeight * 0.33)),
					[Point]::new([int]($frameWidth * 0.5), $top + [int]($frameHeight * 0.5)),
					[Point]::new([int]($frameWidth * 0.35), $top + [int]($frameHeight * 0.33))
				))
			}
			'Flyer' {
				Fill-Polygon $g $theme.Dark ([Point[]]@(
					[Point]::new([int]($frameWidth * 0.2), $top + [int]($frameHeight * 0.5)),
					[Point]::new([int]($frameWidth * 0.02), $top + [int]($frameHeight * 0.25) + ($step * 2)),
					[Point]::new([int]($frameWidth * 0.3), $top + [int]($frameHeight * 0.38))
				))
				Fill-Polygon $g $theme.Dark ([Point[]]@(
					[Point]::new([int]($frameWidth * 0.8), $top + [int]($frameHeight * 0.5)),
					[Point]::new([int]($frameWidth * 0.98), $top + [int]($frameHeight * 0.25) + ($step * 2)),
					[Point]::new([int]($frameWidth * 0.7), $top + [int]($frameHeight * 0.38))
				))
				Fill-Ellipse $g $theme.Main ([int]($frameWidth * 0.26)) ($top + [int]($frameHeight * 0.28)) ([int]($frameWidth * 0.48)) ([int]($frameHeight * 0.34))
				Fill-Ellipse $g $theme.Light ([int]($frameWidth * 0.38)) ($top + [int]($frameHeight * 0.18)) ([int]($frameWidth * 0.22)) ([int]($frameHeight * 0.18))
			}
			'Companion' {
				Fill-Ellipse $g $theme.Light ([int]($frameWidth * 0.28)) ($top + [int]($frameHeight * 0.08)) ([int]($frameWidth * 0.42)) ([int]($frameHeight * 0.26))
				Fill-Rect $g $theme.Main ([int]($frameWidth * 0.24)) ($top + [int]($frameHeight * 0.32)) ([int]($frameWidth * 0.52)) ([int]($frameHeight * 0.34))
				Fill-Rect $g $shadow ([int]($frameWidth * 0.3)) ($top + [int]($frameHeight * 0.66)) ([int]($frameWidth * 0.12)) ([int]($frameHeight * 0.18))
				Fill-Rect $g $shadow ([int]($frameWidth * 0.58)) ($top + [int]($frameHeight * 0.66) + $step) ([int]($frameWidth * 0.12)) ([int]($frameHeight * 0.18))
			}
			default {
				Fill-Ellipse $g $theme.Light ([int]($frameWidth * 0.34)) ($top + [int]($frameHeight * 0.12)) ([int]($frameWidth * 0.28)) ([int]($frameHeight * 0.22))
				Fill-Rect $g $theme.Main ([int]($frameWidth * 0.24)) ($top + [int]($frameHeight * 0.34)) ([int]($frameWidth * 0.52)) ([int]($frameHeight * 0.24))
				Fill-Rect $g $shadow ([int]($frameWidth * 0.28)) ($top + [int]($frameHeight * 0.58)) ([int]($frameWidth * 0.12)) ([int]($frameHeight * 0.2))
				Fill-Rect $g $shadow ([int]($frameWidth * 0.6)) ($top + [int]($frameHeight * 0.58) + $step) ([int]($frameWidth * 0.12)) ([int]($frameHeight * 0.2))
			}
		}

		Draw-CenteredLabel $g $label $theme.Dark ([RectangleF]::new(0, $top + ($frameHeight * 0.74), $frameWidth, [Math]::Max(12, $frameHeight * 0.18)))
	}

	Save-Canvas $canvas $relativePath
}

function Draw-ItemIcon([string]$relativePath, [string]$key, [int]$width, [int]$height, [string]$kind) {
	$theme = Get-Theme $key
	$canvas = New-Canvas $width $height
	$g = $canvas.Graphics
	$shadow = [ColorTranslator]::FromHtml('#11151D')

	Fill-Rect $g ([Color]::FromArgb(55, $theme.Dark)) 0 0 $width $height
	Draw-Border $g $theme.Dark $width $height

	switch ($kind) {
		'Magic' {
			Fill-Rect $g $shadow ([int]($width * 0.45)) ([int]($height * 0.24)) ([Math]::Max(2, [int]($width * 0.12))) ([int]($height * 0.5))
			Fill-Ellipse $g $theme.Main ([int]($width * 0.26)) ([int]($height * 0.08)) ([int]($width * 0.48)) ([int]($height * 0.34))
			Fill-Ellipse $g $theme.Light ([int]($width * 0.36)) ([int]($height * 0.16)) ([int]($width * 0.28)) ([int]($height * 0.18))
		}
		'Ranged' {
			Fill-Rect $g $theme.Main ([int]($width * 0.18)) ([int]($height * 0.4)) ([int]($width * 0.58)) ([int]($height * 0.2))
			Fill-Rect $g $theme.Light ([int]($width * 0.62)) ([int]($height * 0.34)) ([int]($width * 0.16)) ([int]($height * 0.16))
			Fill-Rect $g $shadow ([int]($width * 0.3)) ([int]($height * 0.58)) ([int]($width * 0.14)) ([int]($height * 0.2))
		}
		'Tool' {
			Fill-Rect $g $shadow ([int]($width * 0.46)) ([int]($height * 0.18)) ([Math]::Max(2, [int]($width * 0.1))) ([int]($height * 0.6))
			Fill-Rect $g $theme.Main ([int]($width * 0.2)) ([int]($height * 0.18)) ([int]($width * 0.52)) ([int]($height * 0.18))
			Fill-Rect $g $theme.Light ([int]($width * 0.16)) ([int]($height * 0.16)) ([int]($width * 0.16)) ([int]($height * 0.22))
		}
		default {
			Fill-Polygon $g $theme.Main ([Point[]]@(
				[Point]::new([int]($width * 0.22), [int]($height * 0.72)),
				[Point]::new([int]($width * 0.48), [int]($height * 0.2)),
				[Point]::new([int]($width * 0.7), [int]($height * 0.28)),
				[Point]::new([int]($width * 0.42), [int]($height * 0.82))
			))
			Fill-Rect $g $theme.Light ([int]($width * 0.56)) ([int]($height * 0.16)) ([int]($width * 0.16)) ([int]($height * 0.2))
		}
	}

	Draw-CenteredLabel $g (Get-ShortLabel $key) $theme.Dark ([RectangleF]::new(0, $height * 0.7, $width, [Math]::Max(10, $height * 0.2)))
	Save-Canvas $canvas $relativePath
}

function Get-MobEntries {
	$content = Get-Content -Path (Join-Path $repoRoot 'Common/OreSpawn/OreSpawnMobCatalog.cs') -Raw
	$patterns = @(
		'public static readonly OreSpawnMobDefinition (?<Key>\w+) = Ambient\("(?<Id>[^"]+)", "(?<Display>[^"]+)", [^,]+, (?<Width>\d+), (?<Height>\d+),',
		'public static readonly OreSpawnMobDefinition (?<Key>\w+) = Companion\("(?<Id>[^"]+)", "(?<Display>[^"]+)", (?<Width>\d+), (?<Height>\d+),',
		'public static readonly OreSpawnMobDefinition (?<Key>\w+) = (?:Flyer|Walker|Hopper|Burrower)\("(?<Id>[^"]+)", "(?<Display>[^"]+)", [^,]+, [^,]+, (?<Width>\d+), (?<Height>\d+),'
	)

	$items = New-Object System.Collections.Generic.List[object]
	foreach ($pattern in $patterns) {
		foreach ($match in [regex]::Matches($content, $pattern)) {
			$category = if ($content.Substring($match.Index, [Math]::Min(120, $content.Length - $match.Index)) -like '*Companion(*') { 'Companion' }
			elseif ($content.Substring($match.Index, [Math]::Min(120, $content.Length - $match.Index)) -like '*Flyer(*') { 'Flyer' }
			else { 'Walker' }

			$items.Add([PSCustomObject]@{
				Key = $match.Groups['Key'].Value
				Width = [int]$match.Groups['Width'].Value
				Height = [int]$match.Groups['Height'].Value
				Category = $category
			})
		}
	}

	return $items | Sort-Object Key -Unique
}

function Get-BossEntries {
	$content = Get-Content -Path (Join-Path $repoRoot 'Common/OreSpawn/OreSpawnMobCatalog.cs') -Raw
	$matches = [regex]::Matches($content, 'Boss\("(?<Key>[^"]+)", "(?<Display>[^"]+)", [^,]+, (?<Width>\d+), (?<Height>\d+),')
	return $matches | ForEach-Object {
		[PSCustomObject]@{
			Key = $_.Groups['Key'].Value
			Width = [int]$_.Groups['Width'].Value
			Height = [int]$_.Groups['Height'].Value
			Category = 'Boss'
		}
	}
}

function Get-ItemEntries {
	$content = Get-Content -Path (Join-Path $repoRoot 'Common/OreSpawn/OreSpawnItemCatalog.cs') -Raw
	$matches = [regex]::Matches($content, 'public static readonly OreSpawnItemDefinition (?<Key>\w+) = (?<Kind>Melee|Magic|Ranged|Tool)\("(?<Id>[^"]+)", "(?<Display>[^"]+)", (?<Width>\d+), (?<Height>\d+),')
	return $matches | ForEach-Object {
		[PSCustomObject]@{
			Key = $_.Groups['Key'].Value
			Width = [int]$_.Groups['Width'].Value
			Height = [int]$_.Groups['Height'].Value
			Kind = $_.Groups['Kind'].Value
		}
	}
}

$existingNpcArt = @('WaterDragon', 'Mantis', 'EmperorScorpion', 'Hercules', 'Caterkiller', 'Cephadrome')
$existingItemArt = @('BigHammer', 'MantisClaw')
$bossSummons = @(
	@{ Key = 'KrakenBeacon'; Width = 32; Height = 32; Kind = 'Magic' },
	@{ Key = 'MobzillaSignal'; Width = 32; Height = 32; Kind = 'Ranged' },
	@{ Key = 'MothraTotem'; Width = 32; Height = 32; Kind = 'Magic' },
	@{ Key = 'KingsEmblem'; Width = 32; Height = 32; Kind = 'Tool' },
	@{ Key = 'QueensBloom'; Width = 32; Height = 32; Kind = 'Magic' },
	@{ Key = 'WtfSignal'; Width = 32; Height = 32; Kind = 'Ranged' }
)

foreach ($mob in Get-MobEntries) {
	if ($existingNpcArt -contains $mob.Key) {
		continue
	}

	Draw-NpcSheet "Content/NPCs/OreSpawn/$($mob.Key).png" $mob.Key $mob.Width $mob.Height $mob.Category
}

foreach ($boss in Get-BossEntries) {
	Draw-NpcSheet "Content/Bosses/OreSpawn/$($boss.Key).png" $boss.Key $boss.Width $boss.Height 'Boss'
}

foreach ($item in Get-ItemEntries) {
	if ($existingItemArt -contains $item.Key) {
		continue
	}

	Draw-ItemIcon "Content/Items/OreSpawn/$($item.Key).png" $item.Key $item.Width $item.Height $item.Kind
}

foreach ($summon in $bossSummons) {
	Draw-ItemIcon "Content/Items/Summons/OreSpawn/$($summon.Key).png" $summon.Key $summon.Width $summon.Height $summon.Kind
}
