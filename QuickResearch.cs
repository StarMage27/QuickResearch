using Terraria.Localization;
using Terraria.ModLoader;

namespace QuickResearch;

public class QuickResearch : Mod
{
    public static ModKeybind QRBind { get; set; }
    public static ModKeybind QCBind { get; set; }
    public static ModKeybind RCBind { get; set; }

    public override void Load()
    {
        QRBind = KeybindLoader.RegisterKeybind(this, "Quick research", "J");
        RCBind = KeybindLoader.RegisterKeybind(this, "Research craftable", "K");
        QCBind = KeybindLoader.RegisterKeybind(this, "Quick clean", "L");
    }

    public override void Unload()
    {
        QRBind = null;
        QCBind = null;
        RCBind = null;
    }
}