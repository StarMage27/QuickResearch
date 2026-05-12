using Terraria.Localization;
using Terraria.ModLoader;

namespace QuickResearch;

public class QRTranslation : ModSystem
{
    public static LocalizedText QRButtonText = LocalizedText.Empty;
    public static LocalizedText QCButtonText = LocalizedText.Empty;
    public static LocalizedText RCButtonText = LocalizedText.Empty;
    public static LocalizedText DBButtonText = LocalizedText.Empty;
    public static LocalizedText ResearchedChatText = LocalizedText.Empty;
    public static LocalizedText SacrificedChatText = LocalizedText.Empty;
    public static LocalizedText CleanedChatText = LocalizedText.Empty;
    public static LocalizedText ResearchedCraftableItemsChatText = LocalizedText.Empty;

    public override void OnModLoad()
    {
        QRButtonText = Language.GetOrRegister(Mod.GetLocalizationKey("UI.QRButtonText"));
        QCButtonText = Language.GetOrRegister(Mod.GetLocalizationKey("UI.QCButtonText"));
        RCButtonText = Language.GetOrRegister(Mod.GetLocalizationKey("UI.RCButtonText"));
        DBButtonText = Language.GetOrRegister(Mod.GetLocalizationKey("UI.DBButtonText"));
        ResearchedChatText = Language.GetOrRegister(Mod.GetLocalizationKey("Chat.ResearchedChatText"));
        SacrificedChatText = Language.GetOrRegister(Mod.GetLocalizationKey("Chat.SacrificedChatText"));
        CleanedChatText = Language.GetOrRegister(Mod.GetLocalizationKey("Chat.CleanedChatText"));
        ResearchedCraftableItemsChatText = Language.GetOrRegister(Mod.GetLocalizationKey("Chat.ResearchedCraftableItemsChatText"));
    }
}