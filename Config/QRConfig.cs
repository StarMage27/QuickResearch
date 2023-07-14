using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace QuickResearch.Config
{
	public class QRConfig : ModConfig
	{
		public override ConfigScope Mode => ConfigScope.ClientSide;

        [DefaultValue(true)]
        public bool ShowButtonsToggle;

        [DefaultValue(false)]
		public bool CompleteResearchToggle;

        [DefaultValue(false)]
        public bool ShowResearchMessagesToggle;

        [DefaultValue(false)]
        public bool ResearchCoinsToggle;

        [DefaultValue(false)]
        public bool ShowCleanMessagesToggle;

        [DefaultValue(false)]
        public bool CleanCoinsToggle;

        [DefaultValue(false)]
        public bool ShowResearchCraftableMessagesToggle;

        [DefaultValue(false)]
        public bool ResearchCraftableAfterQuickResearchToggle;
    }
}