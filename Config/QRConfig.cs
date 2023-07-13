using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace QuickResearch.Config
{
	public class QRConfig : ModConfig
	{
		public override ConfigScope Mode => ConfigScope.ClientSide;

		//[Header("QR config")]
		//[Label("Only complete research")]
		//[Tooltip("Only research items if there are enough of them to complete research.")]
		[DefaultValue(false)]

		public bool CompleteResearchToggle;
	}
}