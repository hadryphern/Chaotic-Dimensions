#!/usr/bin/env bash
set -euo pipefail

ROOT_DIR="$(cd -- "$(dirname -- "${BASH_SOURCE[0]}")/.." && pwd)"
SRC_DIR="${HOME}/.local/src/codex-plasma-glass"
WALLPAPER_DIR="${HOME}/.local/share/wallpapers/CodexGlassCacao"
WALLPAPER_SOURCE="${WALLPAPER_DIR}/cacao-macro-1920x1080.jpg"
WALLPAPER="${WALLPAPER_DIR}/cacao-macro-3840x2160.jpg"
WALLPAPER_URL="https://s2.best-wallpaper.net/wallpaper/1920x1080/1506/Still-life-fruits-seeds-macro-photography_1920x1080.jpg"

usage() {
    cat <<'EOF'
Usage:
  tools/plasma_glass_macos_setup.sh [--apply] [--install]

Options:
  --apply    Apply the Plasma glass/macOS layout and theme settings. Default.
  --install  Install Fedora packages and local theme assets, then apply.
EOF
}

have() {
    command -v "$1" >/dev/null 2>&1
}

require() {
    if ! have "$1"; then
        printf 'Missing required command: %s\n' "$1" >&2
        exit 1
    fi
}

clone_or_update() {
    local url="$1"
    local dir="$2"

    if [[ -d "${dir}/.git" ]]; then
        git -C "$dir" pull --ff-only
    else
        git clone --depth 1 "$url" "$dir"
    fi
}

install_packages_and_assets() {
    require git
    require curl

    if have dnf5 && have pkexec; then
        pkexec dnf5 -y copr enable deltacopy/darkly
        pkexec dnf5 -y copr enable matinlotfali/KDE-Rounded-Corners
        pkexec dnf5 -y install \
            darkly darkly-qt5 darkly-qt6 \
            kwin-effect-roundcorners \
            kvantum kvantum-qt5 papirus-icon-theme \
            rsms-inter-vf-fonts fish fastfetch
    fi

    mkdir -p "$SRC_DIR"
    clone_or_update "https://github.com/vinceliuice/Fluent-icon-theme.git" "${SRC_DIR}/Fluent-icon-theme"
    clone_or_update "https://github.com/vinceliuice/WhiteSur-icon-theme.git" "${SRC_DIR}/WhiteSur-icon-theme"
    clone_or_update "https://github.com/vinceliuice/McMojave-cursors.git" "${SRC_DIR}/McMojave-cursors"
    clone_or_update "https://github.com/vinceliuice/WhiteSur-kde.git" "${SRC_DIR}/WhiteSur-kde"

    (cd "${SRC_DIR}/Fluent-icon-theme" && ./install.sh standard)
    (cd "${SRC_DIR}/WhiteSur-icon-theme" && ./install.sh -t all)
    (cd "${SRC_DIR}/McMojave-cursors" && ./install.sh)
    (cd "${SRC_DIR}/WhiteSur-kde" && ./install.sh)
}

ensure_wallpaper() {
    mkdir -p "$WALLPAPER_DIR"
    if [[ ! -s "$WALLPAPER_SOURCE" ]] || ! file "$WALLPAPER_SOURCE" | grep -q 'JPEG image'; then
        require curl
        curl -L --fail --retry 3 --connect-timeout 20 -o "$WALLPAPER_SOURCE" "$WALLPAPER_URL"
    fi

    if [[ ! -s "$WALLPAPER" ]] || ! file "$WALLPAPER" | grep -q '3840x2160'; then
        if have magick; then
            magick "$WALLPAPER_SOURCE" -filter Lanczos -resize 3840x2160^ -gravity center -extent 3840x2160 -unsharp 0x0.6+0.7+0.02 -quality 94 "$WALLPAPER"
        else
            WALLPAPER="$WALLPAPER_SOURCE"
        fi
    fi
}

apply_theme_settings() {
    require kwriteconfig6

    kwriteconfig6 --file darklyrc --group Common --key ShadowStrength 210
    kwriteconfig6 --file darklyrc --group Common --key FloatingTitlebar true
    kwriteconfig6 --file darklyrc --group Common --key ShadowSize ShadowVeryLarge
    kwriteconfig6 --file darklyrc --group Common --key ShadowIntensity High
    kwriteconfig6 --file darklyrc --group Common --key ShadowColor "0,0,0"
    kwriteconfig6 --file darklyrc --group Common --key OutlineCloseButton false
    kwriteconfig6 --file darklyrc --group Common --key CornerRadius 8
    kwriteconfig6 --file darklyrc --group Common --key ButtonHeight 2
    kwriteconfig6 --file darklyrc --group Common --key ButtonWidth 1
    kwriteconfig6 --file darklyrc --group Common --key FancyMargins true
    kwriteconfig6 --file darklyrc --group Common --key SunkenEffect false
    kwriteconfig6 --file darklyrc --group Common --key UseNewCheckBox true

    kwriteconfig6 --file darklyrc --group Style --key AnimationsEnabled true
    kwriteconfig6 --file darklyrc --group Style --key AnimationSteps 38
    kwriteconfig6 --file darklyrc --group Style --key AnimationsDuration 230
    kwriteconfig6 --file darklyrc --group Style --key WidgetDrawShadow true
    kwriteconfig6 --file darklyrc --group Style --key WidgetToolBarShadow true
    kwriteconfig6 --file darklyrc --group Style --key ToolBarDrawItemSeparator false
    kwriteconfig6 --file darklyrc --group Style --key ToolBarDrawSeparator false
    kwriteconfig6 --file darklyrc --group Style --key DisableDolphinUrlNavigatorBackground true
    kwriteconfig6 --file darklyrc --group Style --key SidePanelDrawFrame false
    kwriteconfig6 --file darklyrc --group Style --key DockWidgetDrawFrame false
    kwriteconfig6 --file darklyrc --group Style --key AdjustToDarkThemes true
    kwriteconfig6 --file darklyrc --group Style --key MenuOpacity 76
    kwriteconfig6 --file darklyrc --group Style --key DolphinSidebarOpacity 58
    kwriteconfig6 --file darklyrc --group Style --key DolphinViewOpacity 68
    kwriteconfig6 --file darklyrc --group Style --key TransparentDolphinView false
    kwriteconfig6 --file darklyrc --group Style --key MenuBarOpacity 72
    kwriteconfig6 --file darklyrc --group Style --key ToolBarOpacity 70
    kwriteconfig6 --file darklyrc --group Style --key TabBarOpacity 76
    kwriteconfig6 --file darklyrc --group Style --key ForceOpaque "kscreenlocker,wine,vlc,kdevelop,smplayer,virtualbox,virtualboxvm,obs,kaffeine,kstars,digikam,kdenlive"

    kwriteconfig6 --file darklyrc --group Windeco --key BorderSize BorderNone
    kwriteconfig6 --file darklyrc --group Windeco --key OtherCornerRadius 8
    kwriteconfig6 --file darklyrc --group Windeco --key TitleAlignment AlignLeft
    kwriteconfig6 --file darklyrc --group Windeco --key ButtonSize ButtonSmall
    kwriteconfig6 --file darklyrc --group Windeco --key DrawBorderOnMaximizedWindows false
    kwriteconfig6 --file darklyrc --group Windeco --key DrawTitleBarSeparator false
    kwriteconfig6 --file darklyrc --group Windeco --key DrawBackgroundGradient false
    kwriteconfig6 --file darklyrc --group Windeco --key DrawHighlight true
    kwriteconfig6 --file darklyrc --group Windeco --key AnimationsEnabled true
    kwriteconfig6 --file darklyrc --group Windeco --key AnimationsDuration 180
    kwriteconfig6 --file darklyrc --group Windeco --key HideTitleBar false
    kwriteconfig6 --file darklyrc --group Windeco --key RoundedCorners true

    kwriteconfig6 --file kdeglobals --group KDE --key widgetStyle darkly
    kwriteconfig6 --file kdeglobals --group KDE --key LookAndFeelPackage com.github.vinceliuice.WhiteSur-dark
    kwriteconfig6 --file kdeglobals --group KDE --key AnimationDurationFactor 0.75
    kwriteconfig6 --file kdeglobals --group General --key ColorScheme Darkly
    kwriteconfig6 --file kdeglobals --group Icons --key Theme Fluent-dark
    kwriteconfig6 --file kdeglobals --group GTK --key theme-name WhiteSur-Dark
    kwriteconfig6 --file kdeglobals --group GTK --key icon-theme-name Fluent-dark
    kwriteconfig6 --file plasmarc --group Theme --key name darkly
    kwriteconfig6 --file kcminputrc --group Mouse --key cursorTheme McMojave-cursors

    kwriteconfig6 --file kwinrc --group Compositing --key Enabled true
    kwriteconfig6 --file kwinrc --group Compositing --key AnimationSpeed 3
    kwriteconfig6 --file kwinrc --group KDE --key AnimationDurationFactor 0.75
    kwriteconfig6 --file kwinrc --group Plugins --key blurEnabled true
    kwriteconfig6 --file kwinrc --group Effect-blur --key BlurStrength 12
    kwriteconfig6 --file kwinrc --group Effect-blur --key NoiseStrength 1
    kwriteconfig6 --file kwinrc --group Effect-translucency --key Inactive 88
    kwriteconfig6 --file kwinrc --group Effect-translucency --key MoveResize 82
    kwriteconfig6 --file kwinrc --group Effect-translucency --key Dialogs 90
    kwriteconfig6 --file kwinrc --group Plugins --key translucencyEnabled true
    kwriteconfig6 --file kwinrc --group Plugins --key glideEnabled true
    kwriteconfig6 --file kwinrc --group Plugins --key magiclampEnabled true
    kwriteconfig6 --file kwinrc --group Plugins --key scaleEnabled true
    kwriteconfig6 --file kwinrc --group Plugins --key maximizeEnabled true
    kwriteconfig6 --file kwinrc --group org.kde.kdecoration3 --key library org.kde.darkly
    kwriteconfig6 --file kwinrc --group org.kde.kdecoration3 --key theme Darkly
    kwriteconfig6 --file kwinrc --group org.kde.kdecoration3 --key ButtonsOnLeft XIA
    kwriteconfig6 --file kwinrc --group org.kde.kdecoration3 --key ButtonsOnRight ""
    kwriteconfig6 --file kwinrc --group org.kde.kdecoration3 --key BorderSize None
    kwriteconfig6 --file kwinrc --group org.kde.kdecoration3 --key BorderSizeAuto false
    kwriteconfig6 --file kwinrc --group org.kde.kdecoration2 --key library org.kde.darkly
    kwriteconfig6 --file kwinrc --group org.kde.kdecoration2 --key theme Darkly
    kwriteconfig6 --file kwinrc --group org.kde.kdecoration2 --key ButtonsOnLeft XIA
    kwriteconfig6 --file kwinrc --group org.kde.kdecoration2 --key ButtonsOnRight ""
    kwriteconfig6 --file kwinrc --group Windows --key BorderlessMaximizedWindows true

    kwriteconfig6 --file kscreenlockerrc --group Greeter --group Wallpaper --group org.kde.image --group General --key Image "file://${WALLPAPER}"
    kwriteconfig6 --file kscreenlockerrc --group Greeter --group Wallpaper --group org.kde.image --group General --key FillMode 2
}

apply_window_rules() {
    kwriteconfig6 --file kwinrulesrc --group General --key count 3
    kwriteconfig6 --file kwinrulesrc --group General --key rules discord,code,zen

    kwriteconfig6 --file kwinrulesrc --group discord --key Description "Codex glass - Discord keeps client titlebar to avoid duplicate controls"
    kwriteconfig6 --file kwinrulesrc --group discord --key wmclass discord
    kwriteconfig6 --file kwinrulesrc --group discord --key wmclasscomplete false
    kwriteconfig6 --file kwinrulesrc --group discord --key wmclassmatch 1
    kwriteconfig6 --file kwinrulesrc --group discord --key noborder true
    kwriteconfig6 --file kwinrulesrc --group discord --key noborderrule 2

    kwriteconfig6 --file kwinrulesrc --group code --key Description "Codex glass - force KDE frame for VS Code"
    kwriteconfig6 --file kwinrulesrc --group code --key wmclass Code
    kwriteconfig6 --file kwinrulesrc --group code --key wmclasscomplete false
    kwriteconfig6 --file kwinrulesrc --group code --key wmclassmatch 1
    kwriteconfig6 --file kwinrulesrc --group code --key noborder false
    kwriteconfig6 --file kwinrulesrc --group code --key noborderrule 2

    kwriteconfig6 --file kwinrulesrc --group zen --key Description "Codex glass - force KDE frame for Zen Browser"
    kwriteconfig6 --file kwinrulesrc --group zen --key wmclass zen
    kwriteconfig6 --file kwinrulesrc --group zen --key wmclasscomplete false
    kwriteconfig6 --file kwinrulesrc --group zen --key wmclassmatch 1
    kwriteconfig6 --file kwinrulesrc --group zen --key noborder false
    kwriteconfig6 --file kwinrulesrc --group zen --key noborderrule 2
}

apply_reference_glass_overrides() {
    kwriteconfig6 --file plasmarc --group Theme --key name WhiteSur-dark
    kwriteconfig6 --file kdeglobals --group Icons --key Theme WhiteSur-dark
    kwriteconfig6 --file kdeglobals --group GTK --key icon-theme-name WhiteSur-dark

    kwriteconfig6 --file kwinrc --group org.kde.kdecoration3 --key library org.kde.kwin.aurorae
    kwriteconfig6 --file kwinrc --group org.kde.kdecoration3 --key theme __aurorae__svg__WhiteSur-dark_x1.25
    kwriteconfig6 --file kwinrc --group org.kde.kdecoration3 --key ButtonsOnLeft XIA
    kwriteconfig6 --file kwinrc --group org.kde.kdecoration3 --key ButtonsOnRight ""
    kwriteconfig6 --file kwinrc --group org.kde.kdecoration3 --key BorderSize None
    kwriteconfig6 --file kwinrc --group org.kde.kdecoration3 --key BorderSizeAuto false
    kwriteconfig6 --file kwinrc --group org.kde.kdecoration2 --key library org.kde.kwin.aurorae
    kwriteconfig6 --file kwinrc --group org.kde.kdecoration2 --key theme __aurorae__svg__WhiteSur-dark_x1.25
    kwriteconfig6 --file kwinrc --group org.kde.kdecoration2 --key ButtonsOnLeft XIA
    kwriteconfig6 --file kwinrc --group org.kde.kdecoration2 --key ButtonsOnRight ""

    kwriteconfig6 --file darklyrc --group Common --key CornerRadius 12
    kwriteconfig6 --file darklyrc --group Common --key FloatingTitlebar true
    kwriteconfig6 --file darklyrc --group Common --key ShadowSize ShadowVeryLarge
    kwriteconfig6 --file darklyrc --group Common --key ShadowIntensity Maximum
    kwriteconfig6 --file darklyrc --group Common --key ShadowStrength 235
    kwriteconfig6 --file darklyrc --group Style --key MenuOpacity 56
    kwriteconfig6 --file darklyrc --group Style --key DolphinSidebarOpacity 42
    kwriteconfig6 --file darklyrc --group Style --key DolphinViewOpacity 54
    kwriteconfig6 --file darklyrc --group Style --key ToolBarOpacity 52
    kwriteconfig6 --file darklyrc --group Style --key MenuBarOpacity 50
    kwriteconfig6 --file darklyrc --group Style --key TabBarOpacity 56
    kwriteconfig6 --file darklyrc --group Style --key ForceOpaque "kscreenlocker,wine,vlc,virtualbox,virtualboxvm,obs"

    kwriteconfig6 --file kwinrc --group Plugins --key kwin4_effect_shapecornersEnabled true
    kwriteconfig6 --file kwinrc --group Plugins --key shapecornersEnabled true
    kwriteconfig6 --file kwinrc --group Round-Corners --key Size 24
    kwriteconfig6 --file kwinrc --group Round-Corners --key InactiveCornerRadius 24
    kwriteconfig6 --file kwinrc --group Round-Corners --key UseSquircleShape true
    kwriteconfig6 --file kwinrc --group Round-Corners --key Squircleness 0.62
    kwriteconfig6 --file kwinrc --group Round-Corners --key AnimationDuration 180
    kwriteconfig6 --file kwinrc --group Round-Corners --key UseNativeDecorationShadows false
    kwriteconfig6 --file kwinrc --group Round-Corners --key ShadowSize 58
    kwriteconfig6 --file kwinrc --group Round-Corners --key InactiveShadowSize 42
    kwriteconfig6 --file kwinrc --group Round-Corners --key ShadowColor black
    kwriteconfig6 --file kwinrc --group Round-Corners --key InactiveShadowColor black
    kwriteconfig6 --file kwinrc --group Round-Corners --key ActiveShadowAlpha 125
    kwriteconfig6 --file kwinrc --group Round-Corners --key InactiveShadowAlpha 80
    kwriteconfig6 --file kwinrc --group Round-Corners --key OutlineThickness 1
    kwriteconfig6 --file kwinrc --group Round-Corners --key InactiveOutlineThickness 1
    kwriteconfig6 --file kwinrc --group Round-Corners --key OutlineColor "255,255,255"
    kwriteconfig6 --file kwinrc --group Round-Corners --key InactiveOutlineColor "255,255,255"
    kwriteconfig6 --file kwinrc --group Round-Corners --key ActiveOutlineAlpha 48
    kwriteconfig6 --file kwinrc --group Round-Corners --key InactiveOutlineAlpha 28
    kwriteconfig6 --file kwinrc --group Round-Corners --key SecondOutlineThickness 0
    kwriteconfig6 --file kwinrc --group Round-Corners --key InactiveSecondOutlineThickness 0
    kwriteconfig6 --file kwinrc --group Round-Corners --key OuterOutlineThickness 0
    kwriteconfig6 --file kwinrc --group Round-Corners --key InactiveOuterOutlineThickness 0
    kwriteconfig6 --file kwinrc --group Round-Corners --key IncludeNormalWindows true
    kwriteconfig6 --file kwinrc --group Round-Corners --key IncludeDialogs true
    kwriteconfig6 --file kwinrc --group Round-Corners --key DisableRoundTile false
    kwriteconfig6 --file kwinrc --group Round-Corners --key DisableRoundMaximize true
    kwriteconfig6 --file kwinrc --group Round-Corners --key DisableRoundFullScreen true
    kwriteconfig6 --file kwinrc --group Round-Corners --key DisableOutlineTile false
    kwriteconfig6 --file kwinrc --group Round-Corners --key DisableOutlineMaximize true
    kwriteconfig6 --file kwinrc --group Round-Corners --key DisableOutlineFullScreen true

    kwriteconfig6 --file kwinrc --group Plugins --key blurEnabled true
    kwriteconfig6 --file kwinrc --group Effect-blur --key BlurStrength 16
    kwriteconfig6 --file kwinrc --group Effect-blur --key NoiseStrength 1
    kwriteconfig6 --file kwinrc --group Plugins --key translucencyEnabled true
    kwriteconfig6 --file kwinrc --group Effect-translucency --key Inactive 78
    kwriteconfig6 --file kwinrc --group Effect-translucency --key MoveResize 72
    kwriteconfig6 --file kwinrc --group Effect-translucency --key Dialogs 82

    dbus-send --session --dest=org.kde.KWin --print-reply /Effects org.kde.kwin.Effects.loadEffect string:kwin4_effect_shapecorners >/dev/null 2>&1 || true
}

apply_electron_titlebars() {
    local code_settings="${HOME}/.config/Code/User/settings.json"

    mkdir -p "${HOME}/.config/Code/User"
    if have jq; then
        if [[ -s "$code_settings" ]]; then
            tmp="$(mktemp)"
            jq '. + {
                "window.titleBarStyle": "native",
                "window.menuBarVisibility": "compact",
                "window.commandCenter": false
            }' "$code_settings" >"$tmp"
            mv "$tmp" "$code_settings"
        else
            printf '%s\n' '{"window.titleBarStyle":"native","window.menuBarVisibility":"compact","window.commandCenter":false}' >"$code_settings"
        fi
    fi

    mkdir -p "${HOME}/.config"
    cat >"${HOME}/.config/code-flags.conf" <<'EOF'
--ozone-platform-hint=auto
--enable-features=WaylandWindowDecorations
EOF
}

apply_wallpaper_and_layout() {
    local layout_script="${ROOT_DIR}/tools/plasma_macos_layout.js"

    if have plasma-apply-wallpaperimage; then
        plasma-apply-wallpaperimage "$WALLPAPER" --fill-mode preserveAspectCrop || true
    fi

    if have busctl && [[ -f "$layout_script" ]]; then
        local js
        js="$(sed -e "s|file:///home/hadrykkxd/.local/share/wallpapers/CodexGlassCacao/cacao-macro-1920x1080.jpg|file://${WALLPAPER}|g" -e "s|file:///home/hadrykkxd/.local/share/wallpapers/CodexGlassCacao/cacao-macro-3840x2160.jpg|file://${WALLPAPER}|g" "$layout_script")"
        busctl --user call org.kde.plasmashell /PlasmaShell org.kde.PlasmaShell evaluateScript s "$js" >/dev/null || true
    fi
}

apply_gtk_and_flatpak() {
    gsettings set org.gnome.desktop.interface icon-theme 'Fluent-dark' 2>/dev/null || true
    gsettings set org.gnome.desktop.interface gtk-theme 'WhiteSur-Dark' 2>/dev/null || true
    gsettings set org.gnome.desktop.interface cursor-theme 'McMojave-cursors' 2>/dev/null || true

    flatpak override --user --filesystem="${HOME}/.local/share/icons:ro" --env=ICON_THEME=Fluent-dark app.zen_browser.zen 2>/dev/null || true
    flatpak override --user --filesystem="${HOME}/.local/share/icons:ro" --env=ICON_THEME=Fluent-dark com.discordapp.Discord 2>/dev/null || true
}

apply_konsole_profile() {
    local scheme_dir="${HOME}/.local/share/konsole"
    local fish_dir="${HOME}/.config/fish/conf.d"
    local scheme="${scheme_dir}/CodexGlass.colorscheme"
    local profile="${scheme_dir}/CodexGlass.profile"

    mkdir -p "$scheme_dir" "$fish_dir"

    kwriteconfig6 --file "$scheme" --group General --key Description "Codex Glass"
    kwriteconfig6 --file "$scheme" --group General --key Opacity 0.74
    kwriteconfig6 --file "$scheme" --group General --key Blur true
    kwriteconfig6 --file "$scheme" --group General --key ColorRandomization false
    kwriteconfig6 --file "$scheme" --group General --key Wallpaper ""
    kwriteconfig6 --file "$scheme" --group General --key WallpaperOpacity 1
    kwriteconfig6 --file "$scheme" --group Background --key Color "15,17,20"
    kwriteconfig6 --file "$scheme" --group BackgroundFaint --key Color "15,17,20"
    kwriteconfig6 --file "$scheme" --group BackgroundIntense --key Color "25,28,32"
    kwriteconfig6 --file "$scheme" --group Foreground --key Color "232,236,244"
    kwriteconfig6 --file "$scheme" --group ForegroundFaint --key Color "145,153,166"
    kwriteconfig6 --file "$scheme" --group ForegroundIntense --key Color "255,255,255"

    local colors=(
        "Color0=15,17,20" "Color1=255,95,86" "Color2=39,201,63" "Color3=255,189,46"
        "Color4=59,142,234" "Color5=191,98,255" "Color6=55,214,205" "Color7=216,222,233"
        "Color0Intense=72,78,90" "Color1Intense=255,112,105" "Color2Intense=63,220,83" "Color3Intense=255,205,75"
        "Color4Intense=91,166,255" "Color5Intense=207,124,255" "Color6Intense=85,229,221" "Color7Intense=255,255,255"
        "Color0Faint=11,13,16" "Color1Faint=156,61,57" "Color2Faint=35,128,54" "Color3Faint=156,119,48"
        "Color4Faint=46,91,145" "Color5Faint=117,70,153" "Color6Faint=43,132,129" "Color7Faint=130,136,148"
    )
    local item group color
    for item in "${colors[@]}"; do
        group="${item%%=*}"
        color="${item#*=}"
        kwriteconfig6 --file "$scheme" --group "$group" --key Color "$color"
    done

    kwriteconfig6 --file "$profile" --group General --key Name "Codex Glass"
    kwriteconfig6 --file "$profile" --group General --key Parent "FALLBACK/"
    kwriteconfig6 --file "$profile" --group General --key Command "/usr/bin/fish"
    kwriteconfig6 --file "$profile" --group Appearance --key ColorScheme "CodexGlass"
    kwriteconfig6 --file "$profile" --group Appearance --key Font "Noto Sans Mono,11,-1,5,50,0,0,0,0,0"
    kwriteconfig6 --file "$profile" --group Scrolling --key HistoryMode 2
    kwriteconfig6 --file "$profile" --group TerminalFeatures --key BlinkingCursorEnabled true
    kwriteconfig6 --file konsolerc --group "Desktop Entry" --key DefaultProfile "CodexGlass.profile"
    kwriteconfig6 --file konsolerc --group MainWindow --key MenuBar Disabled

    cat >"${fish_dir}/codex_fastfetch.fish" <<'FISH'
if status is-interactive; and set -q KONSOLE_VERSION; and type -q fastfetch
    fastfetch
end
FISH
}

reload_plasma() {
    kbuildsycoca6 >/dev/null 2>&1 || true
    busctl --user call org.kde.KWin /KWin org.kde.KWin reconfigure >/dev/null 2>&1 || true
    busctl --user call org.kde.plasmashell /PlasmaShell org.kde.PlasmaShell refreshCurrentShell >/dev/null 2>&1 || true
}

mode="apply"
for arg in "$@"; do
    case "$arg" in
        --apply) mode="apply" ;;
        --install) mode="install" ;;
        -h|--help) usage; exit 0 ;;
        *) printf 'Unknown option: %s\n' "$arg" >&2; usage; exit 2 ;;
    esac
done

if [[ "$mode" == "install" ]]; then
    install_packages_and_assets
fi

ensure_wallpaper
apply_theme_settings
apply_reference_glass_overrides
apply_window_rules
apply_wallpaper_and_layout
apply_gtk_and_flatpak
apply_electron_titlebars
apply_konsole_profile
reload_plasma

printf 'Plasma glass/macOS configuration applied. Reopen running apps to pick up the new style.\n'
