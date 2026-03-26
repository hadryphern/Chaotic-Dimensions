using namespace System.Drawing
using namespace System.Drawing.Drawing2D
using namespace System.Drawing.Imaging

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

Add-Type -AssemblyName System.Drawing

$repoRoot = Split-Path -Parent $PSScriptRoot

function Hex([string]$value) {
	return [ColorTranslator]::FromHtml($value)
}

function Clamp([double]$value) {
	return [Math]::Max(0, [Math]::Min(255, [int][Math]::Round($value)))
}

function Mix([Color]$a, [Color]$b, [double]$t) {
	$t = [Math]::Max(0.0, [Math]::Min(1.0, $t))
	return [Color]::FromArgb(255,
		(Clamp ($a.R + (($b.R - $a.R) * $t))),
		(Clamp ($a.G + (($b.G - $a.G) * $t))),
		(Clamp ($a.B + (($b.B - $a.B) * $t))))
}

function Light([Color]$color, [double]$amount) {
	return Mix $color ([Color]::White) $amount
}

function Dark([Color]$color, [double]$amount) {
	return Mix $color ([Color]::Black) $amount
}

function New-Canvas([int]$width, [int]$height) {
	$bitmap = [Bitmap]::new($width, $height, [PixelFormat]::Format32bppArgb)
	$graphics = [Graphics]::FromImage($bitmap)
	$graphics.Clear([Color]::Transparent)
	$graphics.SmoothingMode = [SmoothingMode]::None
	$graphics.InterpolationMode = [InterpolationMode]::NearestNeighbor
	$graphics.PixelOffsetMode = [PixelOffsetMode]::Half
	$graphics.CompositingQuality = [CompositingQuality]::HighSpeed
	return @{ Bitmap = $bitmap; Graphics = $graphics }
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

function Set-Px([Bitmap]$bitmap, [int]$x, [int]$y, [Color]$color) {
	if ($x -ge 0 -and $y -ge 0 -and $x -lt $bitmap.Width -and $y -lt $bitmap.Height) {
		$bitmap.SetPixel($x, $y, $color)
	}
}

function Paint-Noise([Bitmap]$bitmap, [Color]$c1, [Color]$c2, [Color]$c3, [int]$seed) {
	for ($x = 0; $x -lt $bitmap.Width; $x++) {
		for ($y = 0; $y -lt $bitmap.Height; $y++) {
			$mix = (($x * 19) + ($y * 23) + (($x + 3) * ($y + 5)) + $seed) % 13
			$color = if ($mix -lt 4) { $c1 } elseif ($mix -lt 9) { $c2 } else { $c3 }
			$bitmap.SetPixel($x, $y, $color)
		}
	}
}

function Stamp-Diamond([Bitmap]$bitmap, [int]$cx, [int]$cy, [int]$rx, [int]$ry, [Color]$main, [Color]$light, [Color]$dark) {
	for ($dx = -$rx; $dx -le $rx; $dx++) {
		$ratio = 1.0 - ([Math]::Abs($dx) / [Math]::Max(1.0, [double]$rx))
		$limit = [int][Math]::Floor($ry * $ratio)
		for ($dy = -$limit; $dy -le $limit; $dy++) {
			$edge = ([Math]::Abs($dx) -eq $rx) -or ([Math]::Abs($dy) -eq $limit)
			$color = if ($edge) { $dark } elseif ($dx -le 0 -and $dy -le 0) { $light } elseif ($dx -ge 0 -and $dy -ge 0) { Dark $main 0.1 } else { $main }
			Set-Px $bitmap ($cx + $dx) ($cy + $dy) $color
		}
	}
}

function Stamp-Hole([Bitmap]$bitmap, [int]$cx, [int]$cy, [int]$radius, [Color]$inner, [Color]$edge) {
	for ($dx = -$radius; $dx -le $radius; $dx++) {
		for ($dy = -$radius; $dy -le $radius; $dy++) {
			if (($dx * $dx) + ($dy * $dy) -gt ($radius * $radius)) {
				continue
			}

			$distance = [Math]::Sqrt(($dx * $dx) + ($dy * $dy))
			Set-Px $bitmap ($cx + $dx) ($cy + $dy) $(if ($distance -ge ($radius - 1)) { $edge } else { $inner })
		}
	}
}

function Draw-Bevel([Bitmap]$bitmap, [Color]$light, [Color]$dark) {
	for ($i = 0; $i -lt $bitmap.Width; $i++) {
		Set-Px $bitmap $i 0 $light
		Set-Px $bitmap 0 $i $light
		Set-Px $bitmap $i ($bitmap.Height - 1) $dark
		Set-Px $bitmap ($bitmap.Width - 1) $i $dark
	}
}

function Draw-Grid([Bitmap]$bitmap, [Color]$color) {
	for ($i = 0; $i -lt 16; $i++) {
		Set-Px $bitmap 7 $i $color
		Set-Px $bitmap 8 $i $color
		Set-Px $bitmap $i 7 $color
		Set-Px $bitmap $i 8 $color
	}
}

function Draw-Grain([Bitmap]$bitmap, [Color]$dark, [Color]$light) {
	for ($x = 0; $x -lt 16; $x++) {
		for ($y = 0; $y -lt 16; $y++) {
			if ((($x * 3) + $y) % 6 -eq 0) {
				Set-Px $bitmap $x $y $(if (($x % 5) -eq 0) { $dark } else { $light })
			}
		}
	}
}

function Poly([Graphics]$graphics, [Color]$color, [Point[]]$points) {
	$brush = [SolidBrush]::new($color)
	$graphics.FillPolygon($brush, $points)
	$brush.Dispose()
}

function Outline([Graphics]$graphics, [Color]$color, [Point[]]$points) {
	$pen = [Pen]::new($color)
	$graphics.DrawPolygon($pen, $points)
	$pen.Dispose()
}

function Oval([Graphics]$graphics, [Color]$color, [int]$x, [int]$y, [int]$w, [int]$h) {
	$brush = [SolidBrush]::new($color)
	$graphics.FillEllipse($brush, $x, $y, $w, $h)
	$brush.Dispose()
}

$paletteSeeds = @{
	'Amethyst' = @('#433B52', '#5D5270', '#76688E', '#5C338B', '#B986FF', '#E9D8FF', '#8F59D8')
	'Ruby' = @('#4A353E', '#65444F', '#815C68', '#7B1B34', '#D84B67', '#FFC7D1', '#AE3153')
	'PinkTourmaline' = @('#5A414E', '#7A5768', '#9A7488', '#A8427D', '#EF78B8', '#FFD5EA', '#D44D97')
	'TigersEye' = @('#4F412E', '#6B563B', '#8A6F4B', '#7F4E1F', '#D39A4C', '#F7D49B', '#AF6D2D')
	'Kyanite' = @('#34495B', '#496981', '#618CA9', '#2D6FC8', '#6EBEFF', '#D8F2FF', '#4B97EB')
	'Salt' = @('#CFD8E5', '#E0E7F0', '#F1F6FC', '#AAB3C4', '#F8FBFF', '#FFFFFF', '#D9E4F3')
	'Titanium' = @('#66738E', '#8491AD', '#AAB7CE', '#44506B', '#B6C4DC', '#F0F5FF', '#8F9DB8')
	'Uranium' = @('#314A2A', '#48663D', '#648A54', '#1F3418', '#96EE5B', '#E0FFB2', '#6DCB3C')
	'Molenoid' = @('#6B4C2F', '#7F5A35', '#936A42', '#4C331C', '#8A643F', '#B58D60', '#6E5136')
	'RedAntNest' = @('#7D3F32', '#985043', '#B8695B', '#52231D', '#C46A56', '#E7A089', '#9B4537')
	'TermiteNest' = @('#8C6940', '#A67D4D', '#C39861', '#5C4125', '#D9B17A', '#F1D3A4', '#B28351')
	'CrystalWood' = @('#5C7D8E', '#7BA5B8', '#98CDDD', '#46748D', '#9EE7FF', '#E5FAFF', '#76B7D8')
	'SkyWood' = @('#6788A2', '#7EA8C0', '#9CC6D8', '#4A6481', '#A3D6EF', '#EDF8FF', '#6F9EBF')
	'DuplicatorWood' = @('#8D7A4D', '#A8925D', '#C0AE7B', '#65522C', '#D6C586', '#F4E7B6', '#A18C54')
	'Teleport' = @('#2B2759', '#423E7B', '#5A54A1', '#1B1940', '#7D83FF', '#E0E2FF', '#A873FF')
	'EnderPearl' = @('#312446', '#483367', '#614885', '#1D152A', '#A98DE6', '#EADFFF', '#7A60BC')
	'EyeOfEnder' = @('#2D4C2B', '#406B3B', '#5B8D54', '#152912', '#8EE46A', '#E1FFCD', '#4FBA55')
	'Fairy' = @('#4E5F7A', '#6985AA', '#93B7DE', '#4A65A0', '#8CCBFF', '#F2FBFF', '#CBE6FF')
	'Rat' = @('#5D5E7A', '#787A98', '#9C9FBB', '#42445D', '#C4B5FF', '#F0EAFF', '#8B7BC5')
	'Lava' = @('#5C2D14', '#813E18', '#A7541E', '#381806', '#FF8C3A', '#FFD7A0', '#FFBD6A')
	'WaterDragon' = @('#22596C', '#2E768A', '#3B95A6', '#173F50', '#64D7F3', '#D5FBFF', '#99F1FF')
	'Scorpion' = @('#5B4D2A', '#6D5B33', '#836E3A', '#302615', '#C8B067', '#F5EAB8', '#9C813C')
	'Mantis' = @('#355A1B', '#487525', '#659D35', '#21380F', '#93D45A', '#E6FFBF', '#6BB53A')
	'Caterkiller' = @('#59512F', '#746A3C', '#988C4C', '#312A16', '#D4C86D', '#FBF5BA', '#A99341')
	'Cephadrome' = @('#623A44', '#7E4A58', '#A36375', '#3E2027', '#E3839C', '#FFD1DE', '#B95A77')
	'Mobzilla' = @('#474F39', '#5B6846', '#73895A', '#242A18', '#9FDD72', '#ECFFCB', '#C0E98D')
}

function Get-Palette([string]$name) {
	$seed = $paletteSeeds[$name]
	return @{
		Base = (Hex $seed[0]); Base2 = (Hex $seed[1]); Base3 = (Hex $seed[2]); Dark = (Hex $seed[3]);
		Main = (Hex $seed[4]); Light = (Hex $seed[5]); Accent = (Hex $seed[6])
	}
}

function New-TileBitmap([string]$name, [string]$kind, [string]$paletteName, [bool]$glow) {
	$bitmap = [Bitmap]::new(16, 16, [PixelFormat]::Format32bppArgb)
	$palette = Get-Palette $paletteName
	$seed = [Math]::Abs($name.GetHashCode())
	Paint-Noise $bitmap $palette.Base $palette.Base2 $palette.Base3 $seed

	switch ($kind) {
		'Ore' {
			$rng = [Random]::new($seed)
			$count = if ($glow) { 4 } else { 5 }
			for ($i = 0; $i -lt $count; $i++) {
				$cx = 3 + $rng.Next(10)
				$cy = 3 + $rng.Next(10)
				Stamp-Diamond $bitmap $cx $cy (1 + $rng.Next(2)) (2 + $rng.Next(2)) $palette.Main $palette.Light $palette.Dark
				if ($glow) {
					Set-Px $bitmap ($cx - 2) $cy (Mix $palette.Base2 $palette.Main 0.45)
					Set-Px $bitmap ($cx + 2) $cy (Mix $palette.Base2 $palette.Main 0.45)
					Set-Px $bitmap $cx ($cy - 2) (Mix $palette.Base2 $palette.Main 0.45)
					Set-Px $bitmap $cx ($cy + 2) (Mix $palette.Base2 $palette.Main 0.45)
				}
			}
		}
		'GemBlock' {
			Paint-Noise $bitmap (Dark $palette.Main 0.28) (Dark $palette.Main 0.12) (Light $palette.Main 0.1) $seed
			Draw-Grid $bitmap $palette.Dark
			Draw-Bevel $bitmap $palette.Light $palette.Dark
		}
		'MetalBlock' {
			Draw-Grid $bitmap $palette.Dark
			Draw-Bevel $bitmap $palette.Light $palette.Dark
			foreach ($p in @(@(3,3), @(12,3), @(3,12), @(12,12))) {
				Set-Px $bitmap $p[0] $p[1] $palette.Light
				Set-Px $bitmap ($p[0] + 1) ($p[1] + 1) $palette.Dark
			}
		}
		'SaltBlock' {
			Draw-Bevel $bitmap $palette.Light (Dark $palette.Base2 0.18)
			foreach ($p in @(@(1,12), @(3,10), @(5,9), @(7,7), @(9,6), @(12,4), @(14,2))) {
				Set-Px $bitmap $p[0] $p[1] (Dark $palette.Base2 0.32)
			}
		}
		'Dirt' {
			foreach ($x in 1, 5, 9, 13) {
				Set-Px $bitmap $x 3 $palette.Dark
				Set-Px $bitmap (($x + 1) % 16) 10 $palette.Light
			}
		}
		'Nest' {
			Stamp-Hole $bitmap 5 5 2 (Dark $palette.Dark 0.25) $palette.Dark
			Stamp-Hole $bitmap 11 6 2 (Dark $palette.Dark 0.25) $palette.Dark
			Stamp-Hole $bitmap 8 11 2 (Dark $palette.Dark 0.25) $palette.Dark
		}
		'Log' {
			Draw-Grain $bitmap $palette.Dark $palette.Light
			Stamp-Hole $bitmap 8 8 2 (Dark $palette.Base 0.18) $palette.Dark
			Draw-Bevel $bitmap $palette.Light $palette.Dark
		}
		'CrystalLog' {
			for ($x = 1; $x -lt 15; $x += 3) {
				Stamp-Diamond $bitmap $x 8 1 5 $palette.Main $palette.Light $palette.Dark
			}
			Draw-Bevel $bitmap $palette.Light $palette.Dark
		}
		'MagicBlock' {
			Draw-Grid $bitmap $palette.Dark
			Draw-Bevel $bitmap $palette.Light $palette.Dark
			foreach ($p in @(@(7,3), @(8,3), @(4,7), @(5,7), @(6,7), @(7,7), @(8,7), @(9,7), @(10,7), @(7,11), @(8,11), @(5,5), @(10,5), @(5,9), @(10,9))) {
				Set-Px $bitmap $p[0] $p[1] $palette.Main
			}
		}
		'EyeBlock' {
			Draw-Bevel $bitmap $palette.Light $palette.Dark
			for ($x = 3; $x -le 12; $x++) {
				Set-Px $bitmap $x 7 $palette.Dark
				Set-Px $bitmap $x 8 $palette.Dark
			}
			for ($x = 5; $x -le 10; $x++) {
				for ($y = 5; $y -le 10; $y++) {
					$inside = (($x - 7.5) * ($x - 7.5) / 10.0) + (($y - 7.5) * ($y - 7.5) / 6.0)
					if ($inside -le 1.1) {
						$pixel = if ($x -eq 7 -or $x -eq 8) { $palette.Dark } elseif ($x -le 6 -and $y -le 7) { $palette.Light } else { $palette.Main }
						Set-Px $bitmap $x $y $pixel
					}
				}
			}
		}
	}

	return $bitmap
}

function Draw-ItemSprite([string]$name, [string]$kind, [string]$paletteName) {
	$palette = Get-Palette $paletteName
	$canvas = New-Canvas 24 24
	$g = $canvas.Graphics

	switch ($kind) {
		'BlockCube' {
			Poly $g (Light $palette.Main 0.18) ([Point[]]@([Point]::new(12,4), [Point]::new(18,7), [Point]::new(12,10), [Point]::new(6,7)))
			Poly $g $palette.Main ([Point[]]@([Point]::new(6,7), [Point]::new(12,10), [Point]::new(12,18), [Point]::new(6,15)))
			Poly $g (Dark $palette.Main 0.18) ([Point[]]@([Point]::new(12,10), [Point]::new(18,7), [Point]::new(18,15), [Point]::new(12,18)))
		}
		'Gem' {
			Poly $g (Dark $palette.Main 0.1) ([Point[]]@([Point]::new(8,18), [Point]::new(11,8), [Point]::new(15,5), [Point]::new(18,10), [Point]::new(16,19)))
			Poly $g $palette.Main ([Point[]]@([Point]::new(5,19), [Point]::new(8,9), [Point]::new(11,6), [Point]::new(14,11), [Point]::new(12,20)))
			Poly $g $palette.Light ([Point[]]@([Point]::new(8,10), [Point]::new(10,8), [Point]::new(10,15), [Point]::new(8,16)))
		}
		'OreChunk' {
			Poly $g $palette.Base2 ([Point[]]@([Point]::new(4,18), [Point]::new(6,10), [Point]::new(11,6), [Point]::new(18,8), [Point]::new(20,15), [Point]::new(16,20), [Point]::new(9,21)))
			Poly $g $palette.Main ([Point[]]@([Point]::new(8,16), [Point]::new(10,11), [Point]::new(13,10), [Point]::new(14,15), [Point]::new(11,18)))
			Poly $g $palette.Light ([Point[]]@([Point]::new(13,13), [Point]::new(15,11), [Point]::new(17,13), [Point]::new(16,16), [Point]::new(14,16)))
		}
		'Ingot' {
			Poly $g $palette.Light ([Point[]]@([Point]::new(6,10), [Point]::new(16,10), [Point]::new(19,13), [Point]::new(9,13)))
			Poly $g $palette.Main ([Point[]]@([Point]::new(9,13), [Point]::new(19,13), [Point]::new(17,18), [Point]::new(7,18)))
			Poly $g (Dark $palette.Main 0.18) ([Point[]]@([Point]::new(16,10), [Point]::new(19,13), [Point]::new(17,18), [Point]::new(14,15)))
		}
		'Nugget' {
			Poly $g $palette.Main ([Point[]]@([Point]::new(5,16), [Point]::new(8,11), [Point]::new(12,9), [Point]::new(15,11), [Point]::new(16,15), [Point]::new(12,18), [Point]::new(7,18)))
			Poly $g (Light $palette.Main 0.22) ([Point[]]@([Point]::new(11,8), [Point]::new(14,6), [Point]::new(18,8), [Point]::new(17,12), [Point]::new(13,12)))
		}
		'Scale' {
			Poly $g $palette.Main ([Point[]]@([Point]::new(8,20), [Point]::new(5,14), [Point]::new(8,8), [Point]::new(12,6), [Point]::new(16,8), [Point]::new(19,14), [Point]::new(16,20)))
		}
		'Claw' {
			Poly $g $palette.Main ([Point[]]@([Point]::new(7,19), [Point]::new(10,8), [Point]::new(15,4), [Point]::new(18,7), [Point]::new(14,14), [Point]::new(11,20)))
		}
		'Horn' {
			Poly $g $palette.Main ([Point[]]@([Point]::new(6,18), [Point]::new(8,11), [Point]::new(11,7), [Point]::new(16,5), [Point]::new(19,7), [Point]::new(15,12), [Point]::new(10,20)))
		}
		'Jaw' {
			Poly $g $palette.Main ([Point[]]@([Point]::new(4,16), [Point]::new(7,10), [Point]::new(13,8), [Point]::new(19,11), [Point]::new(17,18), [Point]::new(10,20)))
			foreach ($tooth in 0..3) {
				Poly $g $palette.Light ([Point[]]@([Point]::new(7 + ($tooth * 3),16), [Point]::new(8 + ($tooth * 3),13), [Point]::new(9 + ($tooth * 3),16)))
			}
		}
		'Foam' {
			Oval $g $palette.Main 5 11 10 10
			Oval $g (Light $palette.Main 0.18) 11 8 9 9
			Oval $g $palette.Accent 8 5 7 7
			Oval $g $palette.Light 13 5 4 4
		}
	}

	Save-Canvas $canvas "Content/Items/Materials/OreSpawn/$name.png"
}

$tileDefs = @(
	'AmethystOreTile|Ore|Amethyst|false',
	'RubyOreTile|Ore|Ruby|false',
	'PinkTourmalineOreTile|Ore|PinkTourmaline|false',
	'TigersEyeOreTile|Ore|TigersEye|false',
	'KyaniteOreTile|Ore|Kyanite|false',
	'SaltOreTile|Ore|Salt|false',
	'TitaniumOreTile|Ore|Titanium|false',
	'UraniumOreTile|Ore|Uranium|true',
	'AmethystBlockTile|GemBlock|Amethyst|false',
	'RubyBlockTile|GemBlock|Ruby|false',
	'PinkTourmalineBlockTile|GemBlock|PinkTourmaline|false',
	'TigersEyeBlockTile|GemBlock|TigersEye|false',
	'KyaniteBlockTile|GemBlock|Kyanite|false',
	'SaltBlockTile|SaltBlock|Salt|false',
	'TitaniumBlockTile|MetalBlock|Titanium|false',
	'UraniumBlockTile|MetalBlock|Uranium|false',
	'MolenoidDirtTile|Dirt|Molenoid|false',
	'RedAntNestTile|Nest|RedAntNest|false',
	'TermiteNestTile|Nest|TermiteNest|false',
	'CrystalTreeLogTile|CrystalLog|CrystalWood|false',
	'SkyTreeLogTile|Log|SkyWood|false',
	'DuplicatorLogTile|Log|DuplicatorWood|false',
	'TeleportBlockTile|MagicBlock|Teleport|false',
	'EnderPearlBlockTile|MagicBlock|EnderPearl|false',
	'EyeOfEnderBlockTile|EyeBlock|EyeOfEnder|false'
)

$itemDefs = @(
	'Amethyst|Gem|Amethyst',
	'Ruby|Gem|Ruby',
	'PinkTourmaline|Gem|PinkTourmaline',
	'TigersEye|Gem|TigersEye',
	'Kyanite|Gem|Kyanite',
	'Salt|Gem|Salt',
	'TitaniumOre|OreChunk|Titanium',
	'UraniumOre|OreChunk|Uranium',
	'AmethystBlock|BlockCube|Amethyst',
	'RubyBlock|BlockCube|Ruby',
	'PinkTourmalineBlock|BlockCube|PinkTourmaline',
	'TigersEyeBlock|BlockCube|TigersEye',
	'KyaniteBlock|BlockCube|Kyanite',
	'SaltBlock|BlockCube|Salt',
	'TitaniumBlock|BlockCube|Titanium',
	'UraniumBlock|BlockCube|Uranium',
	'MolenoidDirt|BlockCube|Molenoid',
	'RedAntNest|BlockCube|RedAntNest',
	'TermiteNest|BlockCube|TermiteNest',
	'CrystalTreeLog|BlockCube|CrystalWood',
	'SkyTreeLog|BlockCube|SkyWood',
	'DuplicatorLog|BlockCube|DuplicatorWood',
	'TeleportBlock|BlockCube|Teleport',
	'EnderPearlBlock|BlockCube|EnderPearl',
	'EyeOfEnderBlock|BlockCube|EyeOfEnder',
	'FairyCrystal|Gem|Fairy',
	'RatCrystal|Gem|Rat',
	'LavaFoam|Foam|Lava',
	'MobzillaScale|Scale|Mobzilla',
	'TitaniumNugget|Nugget|Titanium',
	'TitaniumIngot|Ingot|Titanium',
	'UraniumNugget|Nugget|Uranium',
	'UraniumIngot|Ingot|Uranium',
	'WaterDragonScale|Scale|WaterDragon',
	'EmperorScorpionScale|Scale|Scorpion',
	'MantisClaw|Claw|Mantis',
	'CaterkillerJaw|Jaw|Caterkiller',
	'CephadromeHorn|Horn|Cephadrome'
)

foreach ($entry in $tileDefs) {
	$parts = $entry.Split('|')
	$bitmap = New-TileBitmap $parts[0] $parts[1] $parts[2] ([bool]::Parse($parts[3]))
	$canvas = New-Canvas 16 16
	$canvas.Graphics.DrawImage($bitmap, 0, 0, 16, 16)
	$bitmap.Dispose()
	Save-Canvas $canvas "Content/Tiles/OreSpawn/$($parts[0]).png"
}

foreach ($entry in $itemDefs) {
	$parts = $entry.Split('|')
	Draw-ItemSprite $parts[0] $parts[1] $parts[2]
}

Write-Output 'Generated OreSpawn resource sprites.'
