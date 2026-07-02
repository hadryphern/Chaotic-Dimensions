using namespace System.Drawing
using namespace System.Drawing.Drawing2D
using namespace System.Drawing.Imaging

Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

Add-Type -AssemblyName System.Drawing

$repoRoot = Split-Path -Parent $PSScriptRoot
$uiDir = Join-Path $repoRoot "Assets\UI"
New-Item -ItemType Directory -Force -Path $uiDir | Out-Null

$fontName = "Palatino Linotype"
$fontFamily = [FontFamily]::new($fontName)

$random = [Random]::new(424242)

function New-ArgbColor([int]$a, [int]$r, [int]$g, [int]$b) {
	return [Color]::FromArgb($a, $r, $g, $b)
}

function New-Canvas([int]$width, [int]$height) {
	$bitmap = [Bitmap]::new($width, $height, [PixelFormat]::Format32bppArgb)
	$graphics = [Graphics]::FromImage($bitmap)
	$graphics.SmoothingMode = [SmoothingMode]::AntiAlias
	$graphics.InterpolationMode = [InterpolationMode]::HighQualityBicubic
	$graphics.PixelOffsetMode = [PixelOffsetMode]::HighQuality
	$graphics.CompositingQuality = [CompositingQuality]::HighQuality
	$graphics.TextRenderingHint = [System.Drawing.Text.TextRenderingHint]::AntiAliasGridFit
	return @{
		Bitmap = $bitmap
		Graphics = $graphics
	}
}

function Save-Canvas($canvas, [string]$relativePath) {
	$path = Join-Path $repoRoot $relativePath
	$canvas.Bitmap.Save($path, [ImageFormat]::Png)
	$canvas.Graphics.Dispose()
	$canvas.Bitmap.Dispose()
}

function New-PointF([double]$x, [double]$y) {
	return [PointF]::new([float]$x, [float]$y)
}

function Rotate-Point([double]$x, [double]$y, [double]$rotationRadians) {
	$cos = [Math]::Cos($rotationRadians)
	$sin = [Math]::Sin($rotationRadians)
	return New-PointF (($x * $cos) - ($y * $sin)) (($x * $sin) + ($y * $cos))
}

function Get-RotatedPolygon([double]$centerX, [double]$centerY, [double]$rotationRadians, [PointF[]]$localPoints) {
	$points = [PointF[]]::new($localPoints.Length)
	for ($i = 0; $i -lt $localPoints.Length; $i++) {
		$rotated = Rotate-Point $localPoints[$i].X $localPoints[$i].Y $rotationRadians
		$points[$i] = New-PointF ($centerX + $rotated.X) ($centerY + $rotated.Y)
	}
	return $points
}

function Draw-GlowEllipse([Graphics]$graphics, [RectangleF]$rect, [Color]$color, [int]$layers) {
	for ($layer = $layers; $layer -ge 1; $layer--) {
		$inflate = $layer * 12
		$alpha = [Math]::Max(4, [int]($color.A / ($layer + 2)))
		$glowRect = [RectangleF]::new(
			$rect.X - $inflate,
			$rect.Y - $inflate,
			$rect.Width + ($inflate * 2),
			$rect.Height + ($inflate * 2))
		$brush = [SolidBrush]::new((New-ArgbColor $alpha $color.R $color.G $color.B))
		$graphics.FillEllipse($brush, $glowRect)
		$brush.Dispose()
	}
}

function Draw-Star([Graphics]$graphics, [double]$x, [double]$y, [double]$radius, [Color]$coreColor, [double]$twinkleScale) {
	$glowRect = [RectangleF]::new([float]($x - ($radius * 1.8)), [float]($y - ($radius * 1.8)), [float]($radius * 3.6), [float]($radius * 3.6))
	Draw-GlowEllipse $graphics $glowRect $coreColor 2

	$coreBrush = [SolidBrush]::new($coreColor)
	$graphics.FillEllipse($coreBrush, [RectangleF]::new([float]($x - $radius), [float]($y - $radius), [float]($radius * 2), [float]($radius * 2)))
	$coreBrush.Dispose()

	$lineColor = New-ArgbColor ([Math]::Min(255, [int]($coreColor.A * 0.7))) $coreColor.R $coreColor.G $coreColor.B
	$pen = [Pen]::new($lineColor, [float][Math]::Max(1.0, $radius * 0.35))
	$graphics.DrawLine($pen, [float]($x - ($radius * 4.6 * $twinkleScale)), [float]$y, [float]($x + ($radius * 4.6 * $twinkleScale)), [float]$y)
	$graphics.DrawLine($pen, [float]$x, [float]($y - ($radius * 4.6 * $twinkleScale)), [float]$x, [float]($y + ($radius * 4.6 * $twinkleScale)))
	$graphics.DrawLine($pen, [float]($x - ($radius * 2.6 * $twinkleScale)), [float]($y - ($radius * 2.6 * $twinkleScale)), [float]($x + ($radius * 2.6 * $twinkleScale)), [float]($y + ($radius * 2.6 * $twinkleScale)))
	$graphics.DrawLine($pen, [float]($x - ($radius * 2.6 * $twinkleScale)), [float]($y + ($radius * 2.6 * $twinkleScale)), [float]($x + ($radius * 2.6 * $twinkleScale)), [float]($y - ($radius * 2.6 * $twinkleScale)))
	$pen.Dispose()
}

function Draw-TinyStar([Graphics]$graphics, [double]$x, [double]$y, [double]$radius, [Color]$color, [bool]$sparkle) {
	$brush = [SolidBrush]::new($color)
	$graphics.FillEllipse($brush, [RectangleF]::new([float]($x - $radius), [float]($y - $radius), [float]($radius * 2), [float]($radius * 2)))
	$brush.Dispose()
	if ($sparkle) {
		$pen = [Pen]::new((New-ArgbColor ([Math]::Min(255, $color.A)) $color.R $color.G $color.B), [float][Math]::Max(1.0, $radius * 0.6))
		$graphics.DrawLine($pen, [float]($x - ($radius * 3.0)), [float]$y, [float]($x + ($radius * 3.0)), [float]$y)
		$graphics.DrawLine($pen, [float]$x, [float]($y - ($radius * 3.0)), [float]$x, [float]($y + ($radius * 3.0)))
		$pen.Dispose()
	}
}

function Draw-Crystal([Graphics]$graphics, [double]$centerX, [double]$centerY, [double]$height, [double]$width, [double]$rotationRadians, [Color]$topColor, [Color]$bottomColor, [Color]$outlineColor, [Color]$glowColor, [double]$innerAlphaScale) {
	$localPoints = @(
		(New-PointF 0 (-$height * 0.58)),
		(New-PointF ($width * 0.34) (-$height * 0.1)),
		(New-PointF ($width * 0.2) ($height * 0.24)),
		(New-PointF 0 ($height * 0.58)),
		(New-PointF (-$width * 0.2) ($height * 0.24)),
		(New-PointF (-$width * 0.34) (-$height * 0.1))
	)

	$points = Get-RotatedPolygon $centerX $centerY $rotationRadians $localPoints
	$bounds = [RectangleF]::new([float]($centerX - ($width * 0.7)), [float]($centerY - ($height * 0.75)), [float]($width * 1.4), [float]($height * 1.5))
	Draw-GlowEllipse $graphics $bounds $glowColor 4

	$path = [GraphicsPath]::new()
	$path.AddPolygon($points)

	$brush = [LinearGradientBrush]::new($bounds, $topColor, $bottomColor, [LinearGradientMode]::Vertical)
	$graphics.FillPath($brush, $path)
	$brush.Dispose()

	$highlightLocal = @(
		(New-PointF (-$width * 0.06) (-$height * 0.44)),
		(New-PointF ($width * 0.1) (-$height * 0.08)),
		(New-PointF ($width * 0.02) ($height * 0.34)),
		(New-PointF (-$width * 0.12) ($height * 0.08))
	)
	$highlightPoints = Get-RotatedPolygon $centerX $centerY $rotationRadians $highlightLocal
	$highlightPath = [GraphicsPath]::new()
	$highlightPath.AddPolygon($highlightPoints)
	$highlightBrush = [SolidBrush]::new((New-ArgbColor ([int](150 * $innerAlphaScale)) 255 248 255))
	$graphics.FillPath($highlightBrush, $highlightPath)
	$highlightBrush.Dispose()
	$highlightPath.Dispose()

	$facetPen = [Pen]::new((New-ArgbColor ([int](120 * $innerAlphaScale)) 237 199 255), [float][Math]::Max(1.5, $width * 0.045))
	$facetStart = Rotate-Point (-$width * 0.05) (-$height * 0.42) $rotationRadians
	$facetEnd = Rotate-Point ($width * 0.06) ($height * 0.4) $rotationRadians
	$graphics.DrawLine($facetPen, [float]($centerX + $facetStart.X), [float]($centerY + $facetStart.Y), [float]($centerX + $facetEnd.X), [float]($centerY + $facetEnd.Y))
	$facetPen.Dispose()

	$outlinePen = [Pen]::new($outlineColor, [float][Math]::Max(1.8, $width * 0.05))
	$graphics.DrawPath($outlinePen, $path)
	$outlinePen.Dispose()
	$path.Dispose()
}

function Draw-Nebula([Graphics]$graphics, [double]$centerX, [double]$centerY, [double]$width, [double]$height, [Color]$baseColor, [int]$passes) {
	for ($i = 0; $i -lt $passes; $i++) {
		$wobbleX = ($random.NextDouble() - 0.5) * $width * 0.45
		$wobbleY = ($random.NextDouble() - 0.5) * $height * 0.45
		$scale = 0.55 + ($random.NextDouble() * 0.95)
		$ellipseRect = [RectangleF]::new(
			[float]($centerX + $wobbleX - (($width * $scale) * 0.5)),
			[float]($centerY + $wobbleY - (($height * $scale) * 0.5)),
			[float]($width * $scale),
			[float]($height * $scale))
		$alpha = 10 + [int]($random.NextDouble() * 26)
		$brush = [SolidBrush]::new((New-ArgbColor $alpha $baseColor.R $baseColor.G $baseColor.B))
		$graphics.FillEllipse($brush, $ellipseRect)
		$brush.Dispose()
	}
}

function Draw-BackgroundImage {
	$canvas = New-Canvas 3200 1800
	$graphics = $canvas.Graphics
	$bitmap = $canvas.Bitmap
	$width = $bitmap.Width
	$height = $bitmap.Height

	$backgroundRect = [RectangleF]::new(0, 0, $width, $height)
	$gradient = [LinearGradientBrush]::new(
		$backgroundRect,
		(New-ArgbColor 255 13 4 26),
		(New-ArgbColor 255 42 12 64),
		[LinearGradientMode]::Vertical)
	$graphics.FillRectangle($gradient, $backgroundRect)
	$gradient.Dispose()

	$middleRect = [RectangleF]::new(0, $height * 0.18, $width, $height * 0.52)
	$middleBrush = [SolidBrush]::new((New-ArgbColor 150 72 20 98))
	$graphics.FillRectangle($middleBrush, $middleRect)
	$middleBrush.Dispose()

	Draw-Nebula $graphics ($width * 0.24) ($height * 0.27) 900 520 (New-ArgbColor 255 136 61 189) 34
	Draw-Nebula $graphics ($width * 0.56) ($height * 0.18) 1180 540 (New-ArgbColor 255 74 33 144) 40
	Draw-Nebula $graphics ($width * 0.74) ($height * 0.4) 950 560 (New-ArgbColor 255 176 92 226) 30
	Draw-Nebula $graphics ($width * 0.35) ($height * 0.55) 1240 580 (New-ArgbColor 255 53 17 99) 26

	for ($i = 0; $i -lt 220; $i++) {
		$x = $random.NextDouble() * $width
		$y = $random.NextDouble() * ($height * 0.86)
		$radius = 0.8 + ($random.NextDouble() * 1.8)
		$color = if ($i % 6 -eq 0) { New-ArgbColor 210 255 246 255 } elseif ($i % 4 -eq 0) { New-ArgbColor 170 221 166 255 } else { New-ArgbColor 150 250 220 255 }
		Draw-TinyStar $graphics $x $y $radius $color ($i % 7 -eq 0)
	}

	for ($i = 0; $i -lt 42; $i++) {
		$x = $random.NextDouble() * $width
		$y = $random.NextDouble() * ($height * 0.7)
		$radius = 1.8 + ($random.NextDouble() * 2.8)
		$twinkle = 0.8 + ($random.NextDouble() * 0.3)
		$color = if ($i % 2 -eq 0) { New-ArgbColor 215 255 244 255 } else { New-ArgbColor 205 235 180 255 }
		Draw-Star $graphics $x $y $radius $color $twinkle
	}

	for ($i = 0; $i -lt 70; $i++) {
		$x = $random.NextDouble() * $width
		$y = 180 + ($random.NextDouble() * ($height * 0.55))
		$size = 8 + ($random.NextDouble() * 28)
		$rotation = ($random.NextDouble() - 0.5) * 0.9
		$alphaScale = 0.4 + ($random.NextDouble() * 0.45)
		Draw-Crystal $graphics $x $y ($size * 2.3) ($size * 0.95) $rotation (New-ArgbColor 90 255 240 255) (New-ArgbColor 70 124 62 184) (New-ArgbColor 115 250 202 255) (New-ArgbColor 70 212 108 255) $alphaScale
	}

	for ($cluster = 0; $cluster -lt 11; $cluster++) {
		$baseX = -120 + ($cluster * ($width / 10.0))
		$baseY = $height * (0.72 + ($random.NextDouble() * 0.17))
		$crystalsInCluster = 7 + $random.Next(5)
		for ($i = 0; $i -lt $crystalsInCluster; $i++) {
			$x = $baseX + (($random.NextDouble() - 0.5) * 230)
			$y = $baseY + (($random.NextDouble() - 0.5) * 110)
			$crystalHeight = 180 + ($random.NextDouble() * 420)
			$crystalWidth = 80 + ($random.NextDouble() * 180)
			$rotation = (($random.NextDouble() - 0.5) * 0.55)
			Draw-Crystal $graphics $x $y $crystalHeight $crystalWidth $rotation (New-ArgbColor 228 255 234 255) (New-ArgbColor 205 102 44 180) (New-ArgbColor 220 255 230 255) (New-ArgbColor 82 150 76 225) 1.0
			if ($random.NextDouble() -lt 0.85) {
				Draw-Star $graphics ($x + (($random.NextDouble() - 0.5) * 90)) ($y - ($crystalHeight * (0.45 + ($random.NextDouble() * 0.18)))) (2.0 + ($random.NextDouble() * 3.4)) (New-ArgbColor 220 255 244 255) (0.8 + ($random.NextDouble() * 0.5))
			}
		}
	}

	for ($i = 0; $i -lt 24; $i++) {
		$linePen = [Pen]::new((New-ArgbColor 34 238 184 255), [float](1.6 + ($random.NextDouble() * 1.4)))
		$startX = $random.NextDouble() * $width
		$startY = $random.NextDouble() * ($height * 0.42)
		$endX = $startX + (($random.NextDouble() - 0.5) * 250)
		$endY = $startY + (($random.NextDouble() - 0.5) * 120)
		$graphics.DrawLine($linePen, [float]$startX, [float]$startY, [float]$endX, [float]$endY)
		$linePen.Dispose()
	}

	$hazeBrush = [SolidBrush]::new((New-ArgbColor 54 34 10 54))
	$graphics.FillRectangle($hazeBrush, [RectangleF]::new(0, $height * 0.58, $width, $height * 0.42))
	$hazeBrush.Dispose()

	Save-Canvas $canvas "Assets\UI\CrystalineCosmosBackground.png"
}

function Draw-TitleAsset([string]$relativePath, [int]$width, [int]$height, [string[]]$lines, [float[]]$fontSizes, [float[]]$lineCentersY, [bool]$addSubtitleGlow) {
	$canvas = New-Canvas $width $height
	$graphics = $canvas.Graphics

	$accentColor = New-ArgbColor 255 255 195 255
	$outerGlow = New-ArgbColor 80 168 84 255
	$smokeColor = New-ArgbColor 38 200 110 255

	for ($i = 0; $i -lt 12; $i++) {
		$smokeRect = [RectangleF]::new(
			[float](($random.NextDouble() * $width) - 110),
			[float](($height * 0.24) + ($random.NextDouble() * $height * 0.42)),
			[float](180 + ($random.NextDouble() * 320)),
			[float](56 + ($random.NextDouble() * 120)))
		$brush = [SolidBrush]::new((New-ArgbColor (8 + $random.Next(12)) $smokeColor.R $smokeColor.G $smokeColor.B))
		$graphics.FillEllipse($brush, $smokeRect)
		$brush.Dispose()
	}

	for ($lineIndex = 0; $lineIndex -lt $lines.Length; $lineIndex++) {
		$text = $lines[$lineIndex]
		$fontSize = $fontSizes[$lineIndex]
		$centerY = $lineCentersY[$lineIndex]
		$familyStyle = if ($lineIndex -eq 0 -and $lines.Length -gt 1) { [FontStyle]::Bold } else { [FontStyle]::Bold }
		$font = [Font]::new($fontFamily, $fontSize, $familyStyle, [GraphicsUnit]::Pixel)
		$measured = $graphics.MeasureString($text, $font)
		$left = ($width - $measured.Width) * 0.5
		$top = $centerY - ($measured.Height * 0.5)

		$path = [GraphicsPath]::new()
		$path.AddString($text, $fontFamily, [int]$familyStyle, $font.Size, (New-PointF $left $top), [StringFormat]::GenericDefault)
		$bounds = $path.GetBounds()

		$shardCount = if ($lineIndex -eq ($lines.Length - 1)) { 12 } else { 5 }
		for ($shard = 0; $shard -lt $shardCount; $shard++) {
			$anchorX = $bounds.Left + ($random.NextDouble() * $bounds.Width)
			$anchorY = if ($random.NextDouble() -lt 0.55) { $bounds.Top + 15 } else { $bounds.Bottom - 10 }
			$shardHeight = 36 + ($random.NextDouble() * 105)
			$shardWidth = 18 + ($random.NextDouble() * 44)
			$direction = if ($anchorY -lt ($bounds.Top + $bounds.Height * 0.5)) { -1 } else { 1 }
			$centerYCrystal = $anchorY + ($direction * $shardHeight * 0.35)
			$rotation = (($random.NextDouble() - 0.5) * 0.55)
			Draw-Crystal $graphics $anchorX $centerYCrystal $shardHeight $shardWidth $rotation (New-ArgbColor 180 255 239 255) (New-ArgbColor 140 130 50 206) (New-ArgbColor 190 255 215 255) (New-ArgbColor 80 178 96 255) 0.85
		}

		for ($glow = 4; $glow -ge 1; $glow--) {
			$alpha = 14 + ($glow * 8)
			$pen = [Pen]::new((New-ArgbColor $alpha $outerGlow.R $outerGlow.G $outerGlow.B), [float](12 + ($glow * 6)))
			$pen.LineJoin = [LineJoin]::Round
			$graphics.DrawPath($pen, $path)
			$pen.Dispose()
		}

		$outlinePen = [Pen]::new((New-ArgbColor 245 74 12 104), [float]12)
		$outlinePen.LineJoin = [LineJoin]::Round
		$graphics.DrawPath($outlinePen, $path)
		$outlinePen.Dispose()

		$fillBrush = [LinearGradientBrush]::new($bounds, (New-ArgbColor 255 255 236 255), (New-ArgbColor 255 223 132 255), [LinearGradientMode]::Vertical)
		$graphics.FillPath($fillBrush, $path)
		$fillBrush.Dispose()

		$graphics.SetClip($path)
		$innerBrush = [LinearGradientBrush]::new(
			[RectangleF]::new($bounds.Left, $bounds.Top, $bounds.Width, $bounds.Height * 0.34),
			(New-ArgbColor 130 255 255 255),
			(New-ArgbColor 0 255 255 255),
			[LinearGradientMode]::Vertical)
		$graphics.FillRectangle($innerBrush, [RectangleF]::new($bounds.Left, $bounds.Top, $bounds.Width, $bounds.Height * 0.38))
		$innerBrush.Dispose()
		$graphics.ResetClip()

		for ($spark = 0; $spark -lt 8; $spark++) {
			$sparkX = $bounds.Left + ($random.NextDouble() * $bounds.Width)
			$sparkY = $bounds.Top + ($random.NextDouble() * $bounds.Height)
			Draw-TinyStar $graphics $sparkX $sparkY (1.2 + ($random.NextDouble() * 1.2)) $accentColor $true
		}

		if ($addSubtitleGlow -and $lineIndex -eq 0) {
			$haloBrush = [SolidBrush]::new((New-ArgbColor 24 255 255 255))
			$graphics.FillEllipse($haloBrush, [RectangleF]::new($bounds.Left - 40, $bounds.Top - 20, $bounds.Width + 80, $bounds.Height + 40))
			$haloBrush.Dispose()
		}

		$path.Dispose()
		$font.Dispose()
	}

	Save-Canvas $canvas $relativePath
}

Draw-BackgroundImage
Draw-TitleAsset "Assets\UI\ChaoticDimensionsMenuTitle.png" 2600 900 @("CHAOTIC", "DIMENSIONS") @(180.0, 356.0) @(208.0, 520.0) $true

Write-Output "Generated Crystaline background and menu visuals in Assets\\UI"
