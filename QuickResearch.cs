using QuickResearch.UI;
using Terraria.ModLoader;

namespace QuickResearch
{
	public class QuickResearch : Mod
	{
        public static ModKeybind QRBind { get; set; }
        public static ModKeybind QCBind { get; set; }

        public override void Load()
        {
            QRBind = KeybindLoader.RegisterKeybind(this, "Quick research", "J");
            QCBind = KeybindLoader.RegisterKeybind(this, "Quick clean", "K");
        }

        public override void Unload()
        {
            QRBind = null;
            QCBind = null;
        }
    }
}