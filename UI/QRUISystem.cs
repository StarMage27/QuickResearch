using Terraria;
using Terraria.ModLoader;
using Terraria.UI;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using QuickResearch.Config;
using Terraria.ID;

namespace QuickResearch.UI;

public class QuickResearchButton : ModSystem
{
    internal UserInterface? ui = null;
    internal QRUIState? uiState = null;
    private GameTime gameTime = new();
    bool showQRToggle = false;
    bool showRCToggle = false;
    bool showQCToggle = false;

    public override void UpdateUI(GameTime gameTime)
    {
        if (Main.netMode == NetmodeID.Server) { return; }
        if (ui?.CurrentState == null) { return; }

        this.gameTime = gameTime;
        ui.Update(gameTime);

        if (Main.playerInventory)
        {
            ShowMyUI();
        }
        else
        {
            HideMyUI();
        }
    }

    public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
    {
        if (!Main.GameMode.Equals(GameModeID.Creative) || Main.CreativeMenu.Blocked || !Main.playerInventory)
        {
            HideMyUI();
            return;
        }

        int mouseTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));

        if (mouseTextIndex != -1)
        {
            layers.Insert(mouseTextIndex, new LegacyGameInterfaceLayer("MyMod: MyInterface", () =>
            {
                if (ui?.CurrentState != null)
                {
                    ui.Draw(Main.spriteBatch, gameTime);
                }

                return true;
            }, InterfaceScaleType.UI));
        }

        QRConfig config = ModContent.GetInstance<QRConfig>();
        bool changed = false;
        if (showQRToggle != config.ShowQRToggle) { changed = true; }
        if (showRCToggle != config.ShowRCToggle) { changed = true; }
        if (showQCToggle != config.ShowQCToggle) { changed = true; }

        if (changed)
        {
            Unload();
            Load();
        }

        if (showQRToggle || showRCToggle || showQCToggle)
        {
            ShowMyUI();
        }
        else
        {
            HideMyUI();
        }

    }

    private void ShowMyUI() => ui?.SetState(uiState);

    private void HideMyUI() => ui?.SetState(null);

    public override void OnLocalizationsLoaded()
    {
        if (Main.netMode == NetmodeID.Server) { return; }
        uiState?.UpdateTranslations();
    }

    public override void Load()
    {
        if (Main.netMode == NetmodeID.Server) { return; }

        QRConfig config = ModContent.GetInstance<QRConfig>();
        
        showQRToggle = config.ShowQRToggle;
        showRCToggle = config.ShowRCToggle;
        showQCToggle = config.ShowQCToggle;

        ui = new UserInterface();

        uiState = new QRUIState(showQRToggle, showQCToggle, showRCToggle);
        uiState.Activate();

        ShowMyUI();
    }

    public override void Unload()
    {
        if (Main.netMode == NetmodeID.Server) { return; }

        uiState?.Unload();
        uiState = null;

        HideMyUI();
    }
}
