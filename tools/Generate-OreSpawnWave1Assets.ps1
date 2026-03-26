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

function Draw-DiamondCore([Graphics]$graphics, [Color]$outer, [Color]$inner, [int]$centerX, [int]$centerY, [int]$radius) {
	Fill-Poly $graphics $outer ([Point[]]@(
		[Point]::new($centerX, $centerY - $radius),
		[Point]::new($centerX + $radius, $centerY),
		[Point]::new($centerX, $centerY + $radius),
		[Point]::new($centerX - $radius, $centerY)
	))

	$innerRadius = [Math]::Max(1, $radius - 2)
	Fill-Poly $graphics $inner ([Point[]]@(
		[Point]::new($centerX, $centerY - $innerRadius),
		[Point]::new($centerX + $innerRadius, $centerY),
		[Point]::new($centerX, $centerY + $innerRadius),
		[Point]::new($centerX - $innerRadius, $centerY)
	))
}

function Draw-Spark([Bitmap]$bitmap, [int]$x, [int]$y, [Color]$bright, [Color]$soft) {
	Set-PixelSafe $bitmap $bright $x $y
	Set-PixelSafe $bitmap $soft ($x - 1) $y
	Set-PixelSafe $bitmap $soft ($x + 1) $y
	Set-PixelSafe $bitmap $soft $x ($y - 1)
	Set-PixelSafe $bitmap $soft $x ($y + 1)
}

function Draw-PlaceholderWingedSheet([string]$relativePath, [Color]$accentOuter, [Color]$accentInner, [Color]$wingTint) {
	$canvas = New-Canvas 56 224
	$g = $canvas.Graphics
	$b = $canvas.Bitmap
	$shadow = New-Color '#1f232c'
	$body = New-Color '#343b49'
	$plate = New-Color '#4a5568'

	for ($frame = 0; $frame -lt 4; $frame++) {
		$oy = $frame * 56
		$wingLift = @(0, 2, 1, 3)[$frame]
		$tailShift = @(0, -1, 1, -1)[$frame]

		Fill-Poly $g $shadow ([Point[]]@(
			[Point]::new(12, $oy + 23),
			[Point]::new(2, $oy + 12 + $wingLift),
			[Point]::new(18, $oy + 19)
		))
		Fill-Poly $g $shadow ([Point[]]@(
			[Point]::new(14, $oy + 29),
			[Point]::new(3, $oy + 40 - $wingLift),
			[Point]::new(18, $oy + 33)
		))
		Fill-Poly $g $shadow ([Point[]]@(
			[Point]::new(44, $oy + 23),
			[Point]::new(54, $oy + 12 + $wingLift),
			[Point]::new(38, $oy + 19)
		))
		Fill-Poly $g $shadow ([Point[]]@(
			[Point]::new(42, $oy + 29),
			[Point]::new(53, $oy + 40 - $wingLift),
			[Point]::new(38, $oy + 33)
		))
		Fill-Poly $g $wingTint ([Point[]]@(
			[Point]::new(12, $oy + 23),
			[Point]::new(7, $oy + 16 + $wingLift),
			[Point]::new(18, $oy + 20)
		))
		Fill-Poly $g $wingTint ([Point[]]@(
			[Point]::new(44, $oy + 23),
			[Point]::new(49, $oy + 16 + $wingLift),
			[Point]::new(38, $oy + 20)
		))

		Fill-Ellipse $g $body 17 ($oy + 17) 22 18
		Fill-Ellipse $g $plate 24 ($oy + 14) 11 9
		Fill-Poly $g $plate ([Point[]]@(
			[Point]::new(33, $oy + 18),
			[Point]::new(45, $oy + 16),
			[Point]::new(38, $oy + 23)
		))
		Fill-Poly $g $shadow ([Point[]]@(
			[Point]::new(17, $oy + 25),
			[Point]::new(8, $oy + 24 + $tailShift),
			[Point]::new(15, $oy + 29)
		))
		Fill-Rect $g $shadow 20 ($oy + 31) 4 6
		Fill-Rect $g $shadow 32 ($oy + 31) 4 6
		Draw-DiamondCore $g $accentOuter $accentInner 28 ($oy + 25) 5
		Draw-Spark $b 31 ($oy + 21) $accentInner $accentOuter
	}

	Save-Canvas $canvas $relativePath
}

function Draw-PlaceholderInsectSheet([string]$relativePath, [Color]$accentOuter, [Color]$accentInner, [Color]$limbTint) {
	$canvas = New-Canvas 48 192
	$g = $canvas.Graphics
	$b = $canvas.Bitmap
	$shadow = New-Color '#20252e'
	$body = New-Color '#38424d'
	$plate = New-Color '#525f73'

	for ($frame = 0; $frame -lt 4; $frame++) {
		$oy = $frame * 48
		$legShift = @(0, 1, 0, 1)[$frame]
		$armShift = @(0, 2, 1, 2)[$frame]

		Fill-Ellipse $g $shadow 20 ($oy + 10) 10 9
		Fill-Ellipse $g $body 15 ($oy + 16) 18 14
		Fill-Ellipse $g $plate 21 ($oy + 13) 9 7
		Fill-Poly $g $limbTint ([Point[]]@(
			[Point]::new(15, $oy + 19),
			[Point]::new(4, $oy + 10 + $armShift),
			[Point]::new(12, $oy + 24)
		))
		Fill-Poly $g $limbTint ([Point[]]@(
			[Point]::new(33, $oy + 19),
			[Point]::new(44, $oy + 10 + $armShift),
			[Point]::new(36, $oy + 24)
		))
		Fill-Rect $g $shadow 14 ($oy + 26) 4 10
		Fill-Rect $g $shadow 30 ($oy + 26 + $legShift) 4 10
		Fill-Rect $g $shadow 18 ($oy + 28 + (1 - $legShift)) 3 8
		Fill-Rect $g $shadow 27 ($oy + 28) 3 8
		Draw-DiamondCore $g $accentOuter $accentInner 24 ($oy + 23) 4
		Draw-Spark $b 26 ($oy + 19) $accentInner $accentOuter
	}

	Save-Canvas $canvas $relativePath
}

function Draw-PlaceholderScorpionSheet([string]$relativePath, [Color]$accentOuter, [Color]$accentInner, [Color]$clawTint) {
	$canvas = New-Canvas 64 256
	$g = $canvas.Graphics
	$b = $canvas.Bitmap
	$shadow = New-Color '#1f242c'
	$body = New-Color '#38404a'
	$plate = New-Color '#525d6c'

	for ($frame = 0; $frame -lt 4; $frame++) {
		$oy = $frame * 64
		$tailLift = @(0, -3, -1, -4)[$frame]
		$clawShift = @(0, 2, 1, 3)[$frame]

		Fill-Ellipse $g $body 19 ($oy + 25) 24 15
		Fill-Ellipse $g $plate 29 ($oy + 18) 11 10
		Fill-Ellipse $g $clawTint 7 ($oy + 20 - $clawShift) 12 8
		Fill-Ellipse $g $clawTint 7 ($oy + 32 + $clawShift) 12 8
		Fill-Ellipse $g $clawTint 45 ($oy + 20 - $clawShift) 12 8
		Fill-Ellipse $g $clawTint 45 ($oy + 32 + $clawShift) 12 8
		Fill-Rect $g $shadow 18 ($oy + 38) 3 11
		Fill-Rect $g $shadow 26 ($oy + 40) 3 9
		Fill-Rect $g $shadow 34 ($oy + 40) 3 9
		Fill-Rect $g $shadow 42 ($oy + 38) 3 11
		Fill-Ellipse $g $plate 41 ($oy + 11 + $tailLift) 8 8
		Fill-Ellipse $g $plate 46 ($oy + 5 + $tailLift) 7 7
		Fill-Poly $g $clawTint ([Point[]]@(
			[Point]::new(53, $oy + 4 + $tailLift),
			[Point]::new(61, $oy + 1 + $tailLift),
			[Point]::new(57, $oy + 10 + $tailLift)
		))
		Draw-DiamondCore $g $accentOuter $accentInner 31 ($oy + 31) 5
		Draw-Spark $b 34 ($oy + 27) $accentInner $accentOuter
	}

	Save-Canvas $canvas $relativePath
}

function Draw-PlaceholderBruteSheet([string]$relativePath, [Color]$accentOuter, [Color]$accentInner, [Color]$weaponTint) {
	$canvas = New-Canvas 48 192
	$g = $canvas.Graphics
	$b = $canvas.Bitmap
	$shadow = New-Color '#1e222b'
	$body = New-Color '#3b4452'
	$plate = New-Color '#566173'

	for ($frame = 0; $frame -lt 4; $frame++) {
		$oy = $frame * 48
		$lift = @(0, 1, 0, 1)[$frame]

		Fill-Ellipse $g $plate 15 ($oy + 6) 18 14
		Fill-Rect $g $body 12 ($oy + 18) 22 14
		Fill-Rect $g $shadow 14 ($oy + 30) 6 12
		Fill-Rect $g $shadow 26 ($oy + 30 + $lift) 6 12
		Fill-Rect $g $shadow 8 ($oy + 20) 5 12
		Fill-Rect $g $shadow 34 ($oy + 17) 4 16
		Fill-Rect $g $weaponTint 37 ($oy + 11) 6 19
		Fill-Rect $g $weaponTint 34 ($oy + 12) 10 4
		Draw-DiamondCore $g $accentOuter $accentInner 23 ($oy + 24) 4
		Draw-Spark $b 26 ($oy + 18) $accentInner $accentOuter
	}

	Save-Canvas $canvas $relativePath
}

function Draw-PlaceholderWormSheet([string]$relativePath, [Color]$accentOuter, [Color]$accentInner, [Color]$jawTint) {
	$canvas = New-Canvas 64 256
	$g = $canvas.Graphics
	$b = $canvas.Bitmap
	$shadow = New-Color '#1f222c'
	$body = New-Color '#3b4350'
	$plate = New-Color '#566275'

	for ($frame = 0; $frame -lt 4; $frame++) {
		$oy = $frame * 64
		$headLift = @(0, -1, 1, -1)[$frame]

		Fill-Ellipse $g $body 6 ($oy + 26) 12 11
		Fill-Ellipse $g $body 17 ($oy + 23) 12 12
		Fill-Ellipse $g $body 29 ($oy + 20) 12 13
		Fill-Ellipse $g $plate 41 ($oy + 18) 14 13
		Fill-Ellipse $g $shadow 50 ($oy + 15 + $headLift) 10 11
		Fill-Poly $g $jawTint ([Point[]]@(
			[Point]::new(58, $oy + 19 + $headLift),
			[Point]::new(63, $oy + 14 + $headLift),
			[Point]::new(60, $oy + 23 + $headLift)
		))
		Fill-Poly $g $jawTint ([Point[]]@(
			[Point]::new(58, $oy + 21 + $headLift),
			[Point]::new(63, $oy + 27 + $headLift),
			[Point]::new(60, $oy + 18 + $headLift)
		))
		Draw-DiamondCore $g $accentOuter $accentInner 41 ($oy + 26) 4
		Draw-Spark $b 44 ($oy + 22) $accentInner $accentOuter
	}

	Save-Canvas $canvas $relativePath
}

function Draw-WaterDragonScaleIcon([string]$relativePath) {
	$canvas = New-Canvas 32 32
	$g = $canvas.Graphics
	$b = $canvas.Bitmap
	$outline = New-Color '#0a2f59'
	$core = New-Color '#0f58a6'
	$glow = New-Color '#2f9cff'
	$shine = New-Color '#ccfbff'

	Fill-Poly $g $outline ([Point[]]@(
		[Point]::new(6, 25),
		[Point]::new(12, 6),
		[Point]::new(23, 8),
		[Point]::new(26, 15),
		[Point]::new(21, 27),
		[Point]::new(11, 28)
	))
	Fill-Poly $g $core ([Point[]]@(
		[Point]::new(8, 24),
		[Point]::new(13, 9),
		[Point]::new(22, 11),
		[Point]::new(23, 16),
		[Point]::new(19, 24),
		[Point]::new(12, 26)
	))
	Fill-Poly $g $glow ([Point[]]@(
		[Point]::new(12, 18),
		[Point]::new(18, 10),
		[Point]::new(21, 12),
		[Point]::new(17, 21)
	))
	Fill-Ellipse $g $shine 15 6 6 6
	Draw-Spark $b 20 9 $shine $glow
	Save-Canvas $canvas $relativePath
}

function Draw-MantisClawIcon([string]$relativePath) {
	$canvas = New-Canvas 32 32
	$g = $canvas.Graphics
	$b = $canvas.Bitmap
	$shadow = New-Color '#2a6518'
	$blade = New-Color '#eef7df'
	$edge = New-Color '#67d132'
	$tip = New-Color '#c8ff80'

	Fill-Poly $g $shadow ([Point[]]@(
		[Point]::new(6, 25),
		[Point]::new(11, 11),
		[Point]::new(20, 4),
		[Point]::new(26, 5),
		[Point]::new(21, 12),
		[Point]::new(15, 22),
		[Point]::new(23, 27),
		[Point]::new(16, 28),
		[Point]::new(10, 24)
	))
	Fill-Poly $g $blade ([Point[]]@(
		[Point]::new(9, 24),
		[Point]::new(13, 12),
		[Point]::new(20, 7),
		[Point]::new(22, 8),
		[Point]::new(16, 18),
		[Point]::new(18, 22),
		[Point]::new(13, 24)
	))
	Fill-Poly $g $edge ([Point[]]@(
		[Point]::new(18, 6),
		[Point]::new(26, 5),
		[Point]::new(22, 11),
		[Point]::new(17, 10)
	))
	Fill-Rect $g $tip 20 18 3 5
	Draw-Spark $b 23 7 $tip $edge
	Save-Canvas $canvas $relativePath
}

function Draw-EmperorScorpionScaleIcon([string]$relativePath) {
	$canvas = New-Canvas 32 32
	$g = $canvas.Graphics
	$b = $canvas.Bitmap
	$outline = New-Color '#121114'
	$core = New-Color '#26242c'
	$facet = New-Color '#4a4753'
	$ember = New-Color '#ffb24e'

	Fill-Poly $g $outline ([Point[]]@(
		[Point]::new(8, 27),
		[Point]::new(11, 10),
		[Point]::new(17, 4),
		[Point]::new(24, 9),
		[Point]::new(26, 19),
		[Point]::new(21, 28)
	))
	Fill-Poly $g $core ([Point[]]@(
		[Point]::new(10, 25),
		[Point]::new(12, 12),
		[Point]::new(17, 7),
		[Point]::new(22, 10),
		[Point]::new(23, 18),
		[Point]::new(19, 25)
	))
	Fill-Poly $g $facet ([Point[]]@(
		[Point]::new(13, 18),
		[Point]::new(18, 8),
		[Point]::new(21, 11),
		[Point]::new(18, 20)
	))
	Fill-Rect $g $ember 16 8 2 15
	Fill-Rect $g $ember 14 17 5 2
	Draw-Spark $b 17 7 $ember $facet
	Save-Canvas $canvas $relativePath
}

function Draw-CaterkillerJawIcon([string]$relativePath) {
	$canvas = New-Canvas 32 32
	$g = $canvas.Graphics
	$b = $canvas.Bitmap
	$ridge = New-Color '#9b1b14'
	$bone = New-Color '#6a6252'
	$shadow = New-Color '#222022'
	$fang = New-Color '#d8d1c3'

	Fill-Poly $g $bone ([Point[]]@(
		[Point]::new(7, 10),
		[Point]::new(14, 6),
		[Point]::new(24, 6),
		[Point]::new(28, 10),
		[Point]::new(24, 13),
		[Point]::new(11, 13)
	))
	Fill-Poly $g $ridge ([Point[]]@(
		[Point]::new(9, 10),
		[Point]::new(15, 8),
		[Point]::new(23, 8),
		[Point]::new(25, 10),
		[Point]::new(22, 12),
		[Point]::new(13, 12)
	))
	Fill-Poly $g $shadow ([Point[]]@(
		[Point]::new(8, 10),
		[Point]::new(11, 13),
		[Point]::new(9, 21),
		[Point]::new(5, 26),
		[Point]::new(3, 23),
		[Point]::new(5, 15)
	))
	Fill-Poly $g $shadow ([Point[]]@(
		[Point]::new(24, 10),
		[Point]::new(28, 14),
		[Point]::new(27, 23),
		[Point]::new(23, 27),
		[Point]::new(21, 21),
		[Point]::new(21, 13)
	))
	Fill-Rect $g $fang 12 13 2 4
	Fill-Rect $g $fang 16 13 2 5
	Fill-Rect $g $fang 20 13 2 4
	Draw-Spark $b 23 9 $fang $ridge
	Save-Canvas $canvas $relativePath
}

function Draw-CephadromeHornIcon([string]$relativePath) {
	$canvas = New-Canvas 32 32
	$g = $canvas.Graphics
	$b = $canvas.Bitmap
	$shadow = New-Color '#30415a'
	$body = New-Color '#8aa0bf'
	$stripe = New-Color '#1478ff'
	$shine = New-Color '#e4f6ff'

	Fill-Poly $g $shadow ([Point[]]@(
		[Point]::new(8, 26),
		[Point]::new(12, 11),
		[Point]::new(18, 5),
		[Point]::new(24, 6),
		[Point]::new(20, 14),
		[Point]::new(26, 20),
		[Point]::new(20, 27)
	))
	Fill-Poly $g $body ([Point[]]@(
		[Point]::new(10, 24),
		[Point]::new(13, 12),
		[Point]::new(18, 8),
		[Point]::new(21, 9),
		[Point]::new(18, 15),
		[Point]::new(22, 20),
		[Point]::new(18, 24)
	))
	Fill-Poly $g $stripe ([Point[]]@(
		[Point]::new(16, 9),
		[Point]::new(22, 8),
		[Point]::new(18, 15),
		[Point]::new(20, 19),
		[Point]::new(16, 21),
		[Point]::new(15, 16)
	))
	Draw-Spark $b 21 9 $shine $stripe
	Fill-Rect $g $shine 14 11 2 10
	Save-Canvas $canvas $relativePath
}

function Draw-BigHammerIcon([string]$relativePath) {
	$canvas = New-Canvas 32 32
	$g = $canvas.Graphics
	$b = $canvas.Bitmap
	$headShadow = New-Color '#2a2626'
	$head = New-Color '#474343'
	$edge = New-Color '#686264'
	$slug = New-Color '#c5d13a'
	$handleDark = New-Color '#8a5b1b'
	$handleLight = New-Color '#c78d2e'

	Fill-Poly $g $handleDark ([Point[]]@(
		[Point]::new(7, 26),
		[Point]::new(10, 23),
		[Point]::new(18, 14),
		[Point]::new(21, 17),
		[Point]::new(12, 29),
		[Point]::new(8, 28)
	))
	Fill-Rect $g $handleLight 10 21 3 7
	Fill-Poly $g $headShadow ([Point[]]@(
		[Point]::new(15, 7),
		[Point]::new(24, 5),
		[Point]::new(28, 10),
		[Point]::new(25, 19),
		[Point]::new(16, 20),
		[Point]::new(12, 13)
	))
	Fill-Poly $g $head ([Point[]]@(
		[Point]::new(14, 8),
		[Point]::new(22, 7),
		[Point]::new(25, 10),
		[Point]::new(23, 18),
		[Point]::new(16, 18),
		[Point]::new(13, 13)
	))
	Fill-Rect $g $edge 12 10 3 8
	Fill-Rect $g $edge 21 8 2 9
	Fill-Rect $g $slug 18 13 4 2
	Draw-Spark $b 23 8 $edge $head
	Save-Canvas $canvas $relativePath
}

function Draw-EmperorScorpionIdolIcon([string]$relativePath) {
	$canvas = New-Canvas 32 32
	$g = $canvas.Graphics
	$b = $canvas.Bitmap
	$stone = New-Color '#6e4b22'
	$trim = New-Color '#cf8f3b'
	$gem = New-Color '#ffd47e'
	$shadow = New-Color '#2e1908'

	Fill-Poly $g $stone ([Point[]]@(
		[Point]::new(9, 26),
		[Point]::new(10, 10),
		[Point]::new(16, 6),
		[Point]::new(22, 10),
		[Point]::new(23, 26)
	))
	Fill-Rect $g $trim 12 9 8 3
	Fill-Rect $g $trim 12 19 8 3
	Fill-Ellipse $g $gem 13 13 6 6
	Fill-Poly $g $shadow ([Point[]]@(
		[Point]::new(8, 12),
		[Point]::new(4, 9),
		[Point]::new(9, 17)
	))
	Fill-Poly $g $shadow ([Point[]]@(
		[Point]::new(24, 12),
		[Point]::new(28, 9),
		[Point]::new(23, 17)
	))
	Draw-Spark $b 16 15 $gem $trim
	Save-Canvas $canvas $relativePath
}

function Draw-HerculesTotemIcon([string]$relativePath) {
	$canvas = New-Canvas 32 32
	$g = $canvas.Graphics
	$b = $canvas.Bitmap
	$goldDark = New-Color '#735622'
	$gold = New-Color '#cfa14a'
	$shine = New-Color '#fff0a5'
	$body = New-Color '#4a3920'

	Fill-Ellipse $g $goldDark 8 6 16 10
	Fill-Ellipse $g $gold 11 8 10 7
	Fill-Rect $g $body 12 14 8 10
	Fill-Rect $g $goldDark 8 16 4 8
	Fill-Rect $g $goldDark 20 16 4 8
	Fill-Rect $g $goldDark 13 24 3 4
	Fill-Rect $g $goldDark 17 24 3 4
	Draw-DiamondCore $g $gold $shine 16 19 4
	Draw-Spark $b 19 9 $shine $gold
	Save-Canvas $canvas $relativePath
}

function Draw-CaterkillerBaitIcon([string]$relativePath) {
	$canvas = New-Canvas 32 32
	$g = $canvas.Graphics
	$b = $canvas.Bitmap
	$hook = New-Color '#d7d9de'
	$fleshDark = New-Color '#6f2632'
	$flesh = New-Color '#c04b5e'
	$toxin = New-Color '#bc7bff'

	Fill-Poly $g $hook ([Point[]]@(
		[Point]::new(21, 4),
		[Point]::new(23, 4),
		[Point]::new(23, 18),
		[Point]::new(26, 21),
		[Point]::new(23, 28),
		[Point]::new(18, 25),
		[Point]::new(20, 23),
		[Point]::new(22, 24),
		[Point]::new(24, 21),
		[Point]::new(20, 17)
	))
	Fill-Ellipse $g $fleshDark 7 11 13 13
	Fill-Ellipse $g $flesh 9 13 9 9
	Fill-Poly $g $toxin ([Point[]]@(
		[Point]::new(9, 18),
		[Point]::new(4, 15),
		[Point]::new(6, 23)
	))
	Draw-Spark $b 15 15 (New-Color '#ffe0e5') $flesh
	Save-Canvas $canvas $relativePath
}

function Draw-CephadromeCallerIcon([string]$relativePath) {
	$canvas = New-Canvas 32 32
	$g = $canvas.Graphics
	$b = $canvas.Bitmap
	$shadow = New-Color '#1f3555'
	$body = New-Color '#4fa2ff'
	$ice = New-Color '#dff8ff'
	$core = New-Color '#7de8ff'

	Fill-Poly $g $shadow ([Point[]]@(
		[Point]::new(7, 22),
		[Point]::new(11, 11),
		[Point]::new(20, 8),
		[Point]::new(27, 13),
		[Point]::new(24, 19),
		[Point]::new(15, 25)
	))
	Fill-Poly $g $body ([Point[]]@(
		[Point]::new(9, 21),
		[Point]::new(12, 13),
		[Point]::new(19, 11),
		[Point]::new(24, 14),
		[Point]::new(22, 18),
		[Point]::new(15, 23)
	))
	Fill-Rect $g $ice 20 14 6 3
	Fill-Ellipse $g $core 12 15 7 7
	Draw-Spark $b 23 13 $ice $body
	Save-Canvas $canvas $relativePath
}

function Draw-Preview([string]$relativePath) {
	$preview = New-Canvas 1040 640
	$g = $preview.Graphics
	$bg = [SolidBrush]::new((New-Color '#0f0917'))
	$panel = [SolidBrush]::new((New-Color '#1d132b'))
	$panelAlt = [SolidBrush]::new((New-Color '#231737'))
	$textBrush = [SolidBrush]::new((New-Color '#f8efff'))
	$mutedBrush = [SolidBrush]::new((New-Color '#bdaed0'))
	$titleFont = [Font]::new('Consolas', 18, [FontStyle]::Bold, [GraphicsUnit]::Pixel)
	$labelFont = [Font]::new('Consolas', 11, [FontStyle]::Bold, [GraphicsUnit]::Pixel)
	$smallFont = [Font]::new('Consolas', 10, [FontStyle]::Regular, [GraphicsUnit]::Pixel)

	$g.FillRectangle($bg, 0, 0, 1040, 640)
	$g.FillRectangle($panel, 24, 72, 992, 248)
	$g.FillRectangle($panelAlt, 24, 348, 992, 268)
	$g.DrawString('OreSpawn Wave 1: clean mob placeholders + item pass', $titleFont, $textBrush, 24, 24)
	$g.DrawString('Top row stays temporary for gameplay testing. Bottom row is the new item direction inspired by the OreSpawn wiki.', $smallFont, $mutedBrush, 24, 50)

	$mobCards = @(
		@{ Name = 'Water Dragon'; Path = 'Content\NPCs\OreSpawn\WaterDragon.png'; X = 42; Y = 96; W = 136; H = 200 },
		@{ Name = 'Mantis'; Path = 'Content\NPCs\OreSpawn\Mantis.png'; X = 200; Y = 96; W = 136; H = 200 },
		@{ Name = 'Emperor Scorpion'; Path = 'Content\NPCs\OreSpawn\EmperorScorpion.png'; X = 358; Y = 96; W = 136; H = 200 },
		@{ Name = 'Hercules'; Path = 'Content\NPCs\OreSpawn\Hercules.png'; X = 516; Y = 96; W = 136; H = 200 },
		@{ Name = 'Caterkiller'; Path = 'Content\NPCs\OreSpawn\Caterkiller.png'; X = 674; Y = 96; W = 136; H = 200 },
		@{ Name = 'Cephadrome'; Path = 'Content\NPCs\OreSpawn\Cephadrome.png'; X = 832; Y = 96; W = 136; H = 200 }
	)

	foreach ($card in $mobCards) {
		$g.FillRectangle($panelAlt, $card.X, $card.Y, $card.W, $card.H)
		$image = [Image]::FromFile((Join-Path $repoRoot $card.Path))
		$g.DrawImage($image, $card.X + 34, $card.Y + 16, 68, 160)
		$image.Dispose()
		$g.DrawString($card.Name, $labelFont, $textBrush, $card.X + 8, $card.Y + 174)
		$g.DrawString('placeholder', $smallFont, $mutedBrush, $card.X + 8, $card.Y + 188)
	}

	$itemCards = @(
		@{ Name = 'Water Dragon Scale'; Path = 'Content\Items\Materials\OreSpawn\WaterDragonScale.png' },
		@{ Name = 'Mantis Claw'; Path = 'Content\Items\Materials\OreSpawn\MantisClaw.png' },
		@{ Name = 'Emperor Scorpion Scale'; Path = 'Content\Items\Materials\OreSpawn\EmperorScorpionScale.png' },
		@{ Name = 'Caterkiller Jaw'; Path = 'Content\Items\Materials\OreSpawn\CaterkillerJaw.png' },
		@{ Name = 'Cephadrome Horn'; Path = 'Content\Items\Materials\OreSpawn\CephadromeHorn.png' },
		@{ Name = 'Big Hammer'; Path = 'Content\Items\Weapons\Melee\BigHammer.png' },
		@{ Name = 'Scorpion Idol'; Path = 'Content\Items\Summons\OreSpawn\EmperorScorpionIdol.png' },
		@{ Name = 'Hercules Totem'; Path = 'Content\Items\Summons\OreSpawn\HerculesTotem.png' },
		@{ Name = 'Caterkiller Bait'; Path = 'Content\Items\Summons\OreSpawn\CaterkillerBait.png' },
		@{ Name = 'Cephadrome Caller'; Path = 'Content\Items\Summons\OreSpawn\CephadromeCaller.png' }
	)

	for ($i = 0; $i -lt $itemCards.Count; $i++) {
		$col = $i % 5
		$row = [Math]::Floor($i / 5)
		$x = 58 + ($col * 190)
		$y = 382 + ($row * 112)
		$g.FillRectangle($panel, $x, $y, 150, 84)
		$image = [Image]::FromFile((Join-Path $repoRoot $itemCards[$i].Path))
		$g.DrawImage($image, $x + 12, $y + 14, 56, 56)
		$image.Dispose()
		$g.DrawString($itemCards[$i].Name, $labelFont, $textBrush, $x + 74, $y + 18)
		$g.DrawString('original-inspired pass', $smallFont, $mutedBrush, $x + 74, $y + 42)
	}

	$bg.Dispose()
	$panel.Dispose()
	$panelAlt.Dispose()
	$textBrush.Dispose()
	$mutedBrush.Dispose()
	$titleFont.Dispose()
	$labelFont.Dispose()
	$smallFont.Dispose()

	Save-Canvas $preview $relativePath
}

Draw-PlaceholderWingedSheet 'Content\NPCs\OreSpawn\WaterDragon.png' (New-Color '#5dc1ff') (New-Color '#e3fbff') (New-Color '#456da5')
Draw-PlaceholderInsectSheet 'Content\NPCs\OreSpawn\Mantis.png' (New-Color '#8de867') (New-Color '#f0ffd8') (New-Color '#5a8b45')
Draw-PlaceholderScorpionSheet 'Content\NPCs\OreSpawn\EmperorScorpion.png' (New-Color '#ffb96d') (New-Color '#fff1d2') (New-Color '#80503b')
Draw-PlaceholderBruteSheet 'Content\NPCs\OreSpawn\Hercules.png' (New-Color '#e4cb76') (New-Color '#fff6ce') (New-Color '#8d6a2f')
Draw-PlaceholderWormSheet 'Content\NPCs\OreSpawn\Caterkiller.png' (New-Color '#ff7f94') (New-Color '#ffd9df') (New-Color '#8a5562')
Draw-PlaceholderWingedSheet 'Content\NPCs\OreSpawn\Cephadrome.png' (New-Color '#8ad8ff') (New-Color '#ecfcff') (New-Color '#5777b7')

Draw-WaterDragonScaleIcon 'Content\Items\Materials\OreSpawn\WaterDragonScale.png'
Draw-MantisClawIcon 'Content\Items\Materials\OreSpawn\MantisClaw.png'
Draw-EmperorScorpionScaleIcon 'Content\Items\Materials\OreSpawn\EmperorScorpionScale.png'
Draw-CaterkillerJawIcon 'Content\Items\Materials\OreSpawn\CaterkillerJaw.png'
Draw-CephadromeHornIcon 'Content\Items\Materials\OreSpawn\CephadromeHorn.png'
Draw-BigHammerIcon 'Content\Items\Weapons\Melee\BigHammer.png'

Draw-EmperorScorpionIdolIcon 'Content\Items\Summons\OreSpawn\EmperorScorpionIdol.png'
Draw-HerculesTotemIcon 'Content\Items\Summons\OreSpawn\HerculesTotem.png'
Draw-CaterkillerBaitIcon 'Content\Items\Summons\OreSpawn\CaterkillerBait.png'
Draw-CephadromeCallerIcon 'Content\Items\Summons\OreSpawn\CephadromeCaller.png'

Draw-Preview 'Assets\UI\OreSpawnWave1Preview.png'

Write-Output 'Generated OreSpawn Wave 1 placeholders, item icons, summon icons, and preview.'
