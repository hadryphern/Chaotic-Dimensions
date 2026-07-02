var wallpaper = "file:///home/hadrykkxd/.local/share/wallpapers/CodexGlassCacao/cacao-macro-3840x2160.jpg";

function safeConfigure(widget, groups, values) {
    if (!widget) {
        return;
    }
    widget.currentConfigGroup = groups;
    for (var key in values) {
        widget.writeConfig(key, values[key]);
    }
}

function safeAdd(panel, plugin) {
    try {
        return panel.addWidget(plugin);
    } catch (e) {
        print("Could not add " + plugin + ": " + e);
        return null;
    }
}

var ds = desktops();
for (var i = 0; i < ds.length; i++) {
    ds[i].wallpaperPlugin = "org.kde.image";
    ds[i].currentConfigGroup = ["Wallpaper", "org.kde.image", "General"];
    ds[i].writeConfig("Image", wallpaper);
    ds[i].writeConfig("FillMode", 2);
}

var existingPanels = panels();
for (var p = existingPanels.length - 1; p >= 0; p--) {
    existingPanels[p].remove();
}

var top = new Panel();
top.location = "top";
top.height = 30;
top.lengthMode = "fill";
top.alignment = "center";
top.hiding = "none";
top.opacity = "adaptive";
top.floating = true;

var launcher = safeAdd(top, "org.kde.plasma.kickoff");
safeConfigure(launcher, ["General"], {
    icon: "application-menu",
    favoritesPortedToKAstats: "true"
});

safeAdd(top, "org.kde.plasma.appmenu");
safeAdd(top, "org.kde.plasma.marginsseparator");
safeAdd(top, "org.kde.plasma.systemtray");

var clock = safeAdd(top, "org.kde.plasma.digitalclock");
safeConfigure(clock, ["Appearance"], {
    showDate: "false",
    fontWeight: "500"
});

var dock = new Panel();
dock.location = "bottom";
dock.height = 70;
dock.lengthMode = "fit";
dock.minimumLength = 36;
dock.maximumLength = 62;
dock.alignment = "center";
dock.hiding = "windowscover";
dock.opacity = "adaptive";
dock.floating = true;

var tasks = safeAdd(dock, "org.kde.plasma.icontasks");
safeConfigure(tasks, ["General"], {
    launchers: "applications:app.zen_browser.zen.desktop,applications:code.desktop,applications:com.discordapp.Discord.desktop,applications:org.kde.dolphin.desktop,applications:org.kde.konsole.desktop,applications:systemsettings.desktop",
    showOnlyCurrentDesktop: "false",
    showOnlyCurrentActivity: "true",
    showOnlyCurrentScreen: "false",
    groupingStrategy: "1",
    sortingStrategy: "1",
    launchers59: "applications:app.zen_browser.zen.desktop,applications:code.desktop,applications:com.discordapp.Discord.desktop,applications:org.kde.dolphin.desktop,applications:org.kde.konsole.desktop,applications:systemsettings.desktop"
});

safeAdd(dock, "org.kde.plasma.minimizeall");
