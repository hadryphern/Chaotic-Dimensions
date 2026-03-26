using namespace System.Drawing
using namespace System.Drawing.Drawing2D
using namespace System.Drawing.Imaging

Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

Add-Type -AssemblyName System.Drawing

$repoRoot = Split-Path -Parent $PSScriptRoot

function New-Color([string]$hex) {
	return [ColorTranslator]::FromHtml($hex)
}

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
	$dir = Split-Path -Parent $path
	if (-not (Test-Path $dir)) {
		New-Item -ItemType Directory -Force -Path $dir | Out-Null
	}

	$canvas.Bitmap.Save($path, [ImageFormat]::Png)
	$canvas.Graphics.Dispose()
	$canvas.Bitmap.Dispose()
}

function Fill-Ellipse([Graphics]$graphics, [Color]$color, [int]$x, [int]$y, [int]$width, [int]$height) {
	$brush = [SolidBrush]::new($color)
	$graphics.FillEllipse($brush, $x, $y, $width, $height)
	$brush.Dispose()
}

function Fill-Rect([Graphics]$graphics, [Color]$color, [int]$x, [int]$y, [int]$width, [int]$height) {
	$brush = [SolidBrush]::new($color)
	$graphics.FillRectangle($brush, $x, $y, $width, $height)
	$brush.Dispose()
}

function Fill-Poly([Graphics]$graphics, [Color]$color, [Point[]]$points) {
	$brush = [SolidBrush]::new($color)
	$graphics.FillPolygon($brush, $points)
	$brush.Dispose()
}

function Set-PixelSafe([Bitmap]$bitmap, [Color]$color, [int]$x, [int]$y) {
	if ($x -ge 0 -and $x -lt $bitmap.Width -and $y -ge 0 -and $y -lt $bitmap.Height) {
		$bitmap.SetPixel($x, $y, $color)
	}
}

function Draw-Eye([Bitmap]$bitmap, [int]$x, [int]$y, [Color]$iris) {
	Set-PixelSafe $bitmap ([Color]::White) $x $y
	Set-PixelSafe $bitmap $iris ($x + 1) $y
}

function Draw-WaterDragonSheet([string]$relativePath) {
	$canvas = New-Canvas 56 224
	$g = $canvas.Graphics
	$b = $canvas.Bitmap
	$deep = New-Color '#12356d'
	$mid = New-Color '#1f63b5'
	$light = New-Color '#74d8ff'
	$wing = New-Color '#8beaff'
	$shadow = New-Color '#0b1d3e'

	for ($frame = 0; $frame -lt 4; $frame++) {
		$oy = $frame * 56
		$tailLift = @(2, 0, 3, 1)[$frame]
		$wingTilt = @(-2, 1, -1, 2)[$frame]

		Fill-Ellipse $g $shadow 6 ($oy + 24) 10 8
		Fill-Ellipse $g $deep 8 ($oy + 22) 12 10
		Fill-Ellipse $g $deep 16 ($oy + 20 - $tailLift) 13 10
		Fill-Ellipse $g $mid 24 ($oy + 18) 14 11
		Fill-Ellipse $g $mid 33 ($oy + 16) 12 10
		Fill-Ellipse $g $light 40 ($oy + 14) 11 10

		Fill-Poly $g $wing ([Point[]]@(
			[Point]::new(26, $oy + 18 + $wingTilt),
			[Point]::new(16, $oy + 8 + $wingTilt),
			[Point]::new(30, $oy + 14)
		))
		Fill-Poly $g $wing ([Point[]]@(
			[Point]::new(30, $oy + 26),
			[Point]::new(18, $oy + 34 - $wingTilt),
			[Point]::new(32, $oy + 30)
		))
		Fill-Poly $g $light ([Point[]]@(
			[Point]::new(5, $oy + 25),
			[Point]::new(0, $oy + 21 - $tailLift),
			[Point]::new(4, $oy + 29)
		))
		Fill-Poly $g $light ([Point[]]@(
			[Point]::new(44, $oy + 20),
			[Point]::new(54, $oy + 18),
			[Point]::new(47, $oy + 25)
		))
		Fill-Rect $g $light 38 ($oy + 16) 6 2
	}

	for ($frame = 0; $frame -lt 4; $frame++) {
		$oy = $frame * 56
		Draw-Eye $b 45 ($oy + 17) (New-Color '#032145')
	}

	Save-Canvas $canvas $relativePath
}

function Draw-MantisSheet([string]$relativePath) {
	$canvas = New-Canvas 48 192
	$g = $canvas.Graphics
	$b = $canvas.Bitmap
	$dark = New-Color '#1a4a20'
	$mid = New-Color '#399c43'
	$light = New-Color '#88e85e'
	$claw = New-Color '#d7ff8f'

	for ($frame = 0; $frame -lt 4; $frame++) {
		$oy = $frame * 48
		$clawShift = @(0, 2, 1, 3)[$frame]
		$legShift = @(0, 1, 0, 1)[$frame]

		Fill-Ellipse $g $dark 22 ($oy + 14) 14 18
		Fill-Ellipse $g $mid 16 ($oy + 12) 12 14
		Fill-Ellipse $g $light 10 ($oy + 14) 10 9
		Fill-Rect $g $dark 22 ($oy + 30) 3 9
		Fill-Rect $g $dark 31 ($oy + 30) 3 9
		Fill-Rect $g $dark 14 ($oy + 23 + $legShift) 4 10
		Fill-Rect $g $dark 36 ($oy + 23 + (1 - $legShift)) 4 10
		Fill-Poly $g $claw ([Point[]]@(
			[Point]::new(16, $oy + 19),
			[Point]::new(4, $oy + 10 + $clawShift),
			[Point]::new(12, $oy + 24)
		))
		Fill-Poly $g $claw ([Point[]]@(
			[Point]::new(18, $oy + 25),
			[Point]::new(4, $oy + 31 - $clawShift),
			[Point]::new(14, $oy + 29)
		))
		Fill-Poly $g $claw ([Point[]]@(
			[Point]::new(30, $oy + 19),
			[Point]::new(44, $oy + 10 + $clawShift),
			[Point]::new(34, $oy + 24)
		))
		Fill-Poly $g $claw ([Point[]]@(
			[Point]::new(28, $oy + 25),
			[Point]::new(42, $oy + 31 - $clawShift),
			[Point]::new(32, $oy + 29)
		))
	}

	for ($frame = 0; $frame -lt 4; $frame++) {
		$oy = $frame * 48
		Draw-Eye $b 12 ($oy + 17) (New-Color '#0f2a0e')
	}

	Save-Canvas $canvas $relativePath
}

function Draw-EmperorScorpionSheet([string]$relativePath) {
	$canvas = New-Canvas 64 256
	$g = $canvas.Graphics
	$b = $canvas.Bitmap
	$dark = New-Color '#5f3410'
	$mid = New-Color '#a35b1b'
	$light = New-Color '#f0b459'
	$sting = New-Color '#ffd17f'

	for ($frame = 0; $frame -lt 4; $frame++) {
		$oy = $frame * 64
		$tailArc = @(0, -3, -1, -4)[$frame]
		$clawSpread = @(0, 2, 1, 3)[$frame]

		Fill-Ellipse $g $dark 20 ($oy + 24) 22 14
		Fill-Ellipse $g $mid 30 ($oy + 18) 12 10
		Fill-Ellipse $g $mid 14 ($oy + 24) 10 9
		Fill-Ellipse $g $light 8 ($oy + 20 - $clawSpread) 12 8
		Fill-Ellipse $g $light 8 ($oy + 31 + $clawSpread) 12 8
		Fill-Ellipse $g $light 42 ($oy + 20 - $clawSpread) 12 8
		Fill-Ellipse $g $light 42 ($oy + 31 + $clawSpread) 12 8
		Fill-Rect $g $dark 22 ($oy + 38) 3 11
		Fill-Rect $g $dark 30 ($oy + 38) 3 11
		Fill-Rect $g $dark 16 ($oy + 36) 3 11
		Fill-Rect $g $dark 38 ($oy + 36) 3 11
		Fill-Ellipse $g $mid 40 ($oy + 12 + $tailArc) 8 8
		Fill-Ellipse $g $mid 45 ($oy + 6 + $tailArc) 7 7
		Fill-Ellipse $g $light 49 ($oy + 1 + $tailArc) 6 6
		Fill-Poly $g $sting ([Point[]]@(
			[Point]::new(53, $oy + 4 + $tailArc),
			[Point]::new(60, $oy + 1 + $tailArc),
			[Point]::new(56, $oy + 9 + $tailArc)
		))
	}

	for ($frame = 0; $frame -lt 4; $frame++) {
		$oy = $frame * 64
		Draw-Eye $b 33 ($oy + 21) (New-Color '#402100')
	}

	Save-Canvas $canvas $relativePath
}

function Draw-PlaceholderBruteSheet([string]$relativePath, [Color]$accent) {
	$canvas = New-Canvas 48 192
	$g = $canvas.Graphics
	$dark = New-Color '#241c33'
	$mid = New-Color '#3d3158'

	for ($frame = 0; $frame -lt 4; $frame++) {
		$oy = $frame * 48
		$lift = @(0, 1, 0, 1)[$frame]
		Fill-Ellipse $g $dark 14 ($oy + 6) 18 14
		Fill-Rect $g $mid 12 ($oy + 18) 22 14
		Fill-Rect $g $dark 14 ($oy + 30) 6 12
		Fill-Rect $g $dark 26 ($oy + 30 + $lift) 6 12
		Fill-Rect $g $dark 6 ($oy + 20) 6 14
		Fill-Rect $g $dark 34 ($oy + 16) 4 16
		Fill-Rect $g $accent 36 ($oy + 12) 6 18
		Fill-Rect $g $accent 39 ($oy + 10) 3 4
		Fill-Rect $g $accent 34 ($oy + 12) 10 4
	}

	Save-Canvas $canvas $relativePath
}

function Draw-PlaceholderCaterkillerSheet([string]$relativePath, [Color]$accent) {
	$canvas = New-Canvas 64 256
	$g = $canvas.Graphics
	$dark = New-Color '#2b1d2b'
	$mid = New-Color '#533353'

	for ($frame = 0; $frame -lt 4; $frame++) {
		$oy = $frame * 64
		$headLift = @(0, -1, 1, -1)[$frame]
		Fill-Ellipse $g $mid 6 ($oy + 24) 12 12
		Fill-Ellipse $g $mid 16 ($oy + 22) 12 12
		Fill-Ellipse $g $mid 28 ($oy + 20) 12 12
		Fill-Ellipse $g $mid 40 ($oy + 18) 14 12
		Fill-Ellipse $g $dark 49 ($oy + 14 + $headLift) 11 11
		Fill-Poly $g $accent ([Point[]]@(
			[Point]::new(57, $oy + 18 + $headLift),
			[Point]::new(63, $oy + 14 + $headLift),
			[Point]::new(60, $oy + 22 + $headLift)
		))
		Fill-Poly $g $accent ([Point[]]@(
			[Point]::new(57, $oy + 20 + $headLift),
			[Point]::new(63, $oy + 26 + $headLift),
			[Point]::new(60, $oy + 18 + $headLift)
		))
	}

	Save-Canvas $canvas $relativePath
}

function Draw-PlaceholderCephadromeSheet([string]$relativePath, [Color]$accent) {
	$canvas = New-Canvas 56 224
	$g = $canvas.Graphics
	$dark = New-Color '#18233d'
	$mid = New-Color '#2a446c'

	for ($frame = 0; $frame -lt 4; $frame++) {
		$oy = $frame * 56
		$wing = @(0, 2, 1, 3)[$frame]
		Fill-Ellipse $g $mid 20 ($oy + 18) 18 11
		Fill-Ellipse $g $dark 34 ($oy + 16) 12 10
		Fill-Poly $g $accent ([Point[]]@(
			[Point]::new(43, $oy + 18),
			[Point]::new(54, $oy + 14),
			[Point]::new(46, $oy + 23)
		))
		Fill-Poly $g $mid ([Point[]]@(
			[Point]::new(24, $oy + 19),
			[Point]::new(8, $oy + 8 + $wing),
			[Point]::new(22, $oy + 23)
		))
		Fill-Poly $g $mid ([Point[]]@(
			[Point]::new(24, $oy + 25),
			[Point]::new(8, $oy + 38 - $wing),
			[Point]::new(23, $oy + 28)
		))
		Fill-Poly $g $mid ([Point[]]@(
			[Point]::new(14, $oy + 22),
			[Point]::new(2, $oy + 17),
			[Point]::new(5, $oy + 27)
		))
	}

	Save-Canvas $canvas $relativePath
}

function Draw-ScaleIcon([string]$relativePath, [Color]$dark, [Color]$mid, [Color]$light) {
	$canvas = New-Canvas 32 32
	$g = $canvas.Graphics
	Fill-Poly $g $dark ([Point[]]@(
		[Point]::new(6, 16),
		[Point]::new(16, 4),
		[Point]::new(25, 16),
		[Point]::new(16, 27)
	))
	Fill-Poly $g $mid ([Point[]]@(
		[Point]::new(9, 16),
		[Point]::new(16, 8),
		[Point]::new(22, 16),
		[Point]::new(16, 23)
	))
	Fill-Rect $g $light 14 10 3 8
	Save-Canvas $canvas $relativePath
}

function Draw-ClawIcon([string]$relativePath, [Color]$dark, [Color]$light) {
	$canvas = New-Canvas 32 32
	$g = $canvas.Graphics
	Fill-Poly $g $dark ([Point[]]@(
		[Point]::new(7, 24),
		[Point]::new(18, 6),
		[Point]::new(24, 12),
		[Point]::new(14, 27)
	))
	Fill-Poly $g $light ([Point[]]@(
		[Point]::new(13, 17),
		[Point]::new(20, 10),
		[Point]::new(21, 14),
		[Point]::new(15, 21)
	))
	Save-Canvas $canvas $relativePath
}

function Draw-HammerIcon([string]$relativePath) {
	$canvas = New-Canvas 32 32
	$g = $canvas.Graphics
	$head = New-Color '#8d8c96'
	$shadow = New-Color '#524e5f'
	$handle = New-Color '#8a5c2b'
	Fill-Rect $g $shadow 16 4 6 10
	Fill-Rect $g $head 10 7 14 8
	Fill-Rect $g $handle 13 12 5 16
	Fill-Rect $g $head 9 10 16 3
	Save-Canvas $canvas $relativePath
}

function Draw-SummonIcon([string]$relativePath, [Color]$ring, [Color]$core) {
	$canvas = New-Canvas 32 32
	$g = $canvas.Graphics
	Fill-Ellipse $g $ring 4 4 24 24
	Fill-Ellipse $g ([Color]::Transparent) 0 0 0 0
	Fill-Ellipse $g $core 8 8 16 16
	Fill-Rect $g ([Color]::White) 15 5 2 22
	Fill-Rect $g ([Color]::White) 5 15 22 2
	Save-Canvas $canvas $relativePath
}

function Draw-Preview([string]$relativePath) {
	$preview = New-Canvas 900 520
	$g = $preview.Graphics
	$bg = [SolidBrush]::new((New-Color '#100a1b'))
	$g.FillRectangle($bg, 0, 0, 900, 520)
	$bg.Dispose()

	$cards = @(
		@{ Name = 'Water Dragon'; Path = 'Content\NPCs\OreSpawn\WaterDragon.png'; X = 24; Y = 24 },
		@{ Name = 'Mantis'; Path = 'Content\NPCs\OreSpawn\Mantis.png'; X = 170; Y = 24 },
		@{ Name = 'Emperor Scorpion'; Path = 'Content\NPCs\OreSpawn\EmperorScorpion.png'; X = 316; Y = 24 },
		@{ Name = 'Hercules PH'; Path = 'Content\NPCs\OreSpawn\Hercules.png'; X = 462; Y = 24 },
		@{ Name = 'Caterkiller PH'; Path = 'Content\NPCs\OreSpawn\Caterkiller.png'; X = 608; Y = 24 },
		@{ Name = 'Cephadrome PH'; Path = 'Content\NPCs\OreSpawn\Cephadrome.png'; X = 754; Y = 24 }
	)

	$font = [Font]::new('Consolas', 12, [FontStyle]::Bold, [GraphicsUnit]::Pixel)
	$textBrush = [SolidBrush]::new((New-Color '#f6ecff'))
	$cardBrush = [SolidBrush]::new((New-Color '#211534'))

	foreach ($card in $cards) {
		$g.FillRectangle($cardBrush, $card.X, $card.Y, 122, 210)
		$image = [Image]::FromFile((Join-Path $repoRoot $card.Path))
		$g.DrawImage($image, $card.X + 39, $card.Y + 12, 42, 168)
		$image.Dispose()
		$g.DrawString($card.Name, $font, $textBrush, $card.X + 6, $card.Y + 180)
	}

	$g.DrawString('Wave 1 sprite split: Codex art on the left, placeholders on the right.', $font, $textBrush, 24, 460)

	$textBrush.Dispose()
	$cardBrush.Dispose()
	$font.Dispose()
	Save-Canvas $preview $relativePath
}

Draw-WaterDragonSheet 'Content\NPCs\OreSpawn\WaterDragon.png'
Draw-MantisSheet 'Content\NPCs\OreSpawn\Mantis.png'
Draw-EmperorScorpionSheet 'Content\NPCs\OreSpawn\EmperorScorpion.png'
Draw-PlaceholderBruteSheet 'Content\NPCs\OreSpawn\Hercules.png' (New-Color '#d8b06d')
Draw-PlaceholderCaterkillerSheet 'Content\NPCs\OreSpawn\Caterkiller.png' (New-Color '#ff8ed4')
Draw-PlaceholderCephadromeSheet 'Content\NPCs\OreSpawn\Cephadrome.png' (New-Color '#9fe7ff')

Draw-ScaleIcon 'Content\Items\Materials\OreSpawn\WaterDragonScale.png' (New-Color '#0b2a57') (New-Color '#2d8fe8') (New-Color '#b6f4ff')
Draw-ClawIcon 'Content\Items\Materials\OreSpawn\MantisClaw.png' (New-Color '#376f20') (New-Color '#e3ff8f')
Draw-ScaleIcon 'Content\Items\Materials\OreSpawn\EmperorScorpionScale.png' (New-Color '#6b3708') (New-Color '#d0822a') (New-Color '#ffd08f')
Draw-ClawIcon 'Content\Items\Materials\OreSpawn\CaterkillerJaw.png' (New-Color '#5c2551') (New-Color '#ffb4f1')
Draw-ClawIcon 'Content\Items\Materials\OreSpawn\CephadromeHorn.png' (New-Color '#24496b') (New-Color '#bcefff')
Draw-HammerIcon 'Content\Items\Weapons\Melee\BigHammer.png'

Draw-SummonIcon 'Content\Items\Summons\OreSpawn\EmperorScorpionIdol.png' (New-Color '#c5872e') (New-Color '#5a230a')
Draw-SummonIcon 'Content\Items\Summons\OreSpawn\HerculesTotem.png' (New-Color '#c8b070') (New-Color '#3a2242')
Draw-SummonIcon 'Content\Items\Summons\OreSpawn\CaterkillerBait.png' (New-Color '#ff8ed4') (New-Color '#4d2344')
Draw-SummonIcon 'Content\Items\Summons\OreSpawn\CephadromeCaller.png' (New-Color '#8be5ff') (New-Color '#1b3350')

Draw-Preview 'Assets\UI\OreSpawnWave1Preview.png'

Write-Output 'Generated OreSpawn Wave 1 NPC sprites, item icons, summon icons, and preview.'
