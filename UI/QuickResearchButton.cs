using Terraria;
using Terraria.ModLoader;
using Terraria.UI;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace QuickResearch.UI
{
    public class QuickResearchButton : ModSystem
    {
        internal UserInterface QRInterface;
        internal TheUI QRUI;
        private GameTime lastUpdateUiGameTime;

        public override void UpdateUI(GameTime gameTime)
        {
            lastUpdateUiGameTime = gameTime;

            if (QRInterface?.CurrentState != null)
            {
                QRInterface.Update(gameTime);

                if (Main.playerInventory)
                {
                    ShowMyUI();
                }
                else
                {
                    HideMyUI();
                }
            }
        }

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            int mouseTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));

            if (mouseTextIndex != -1)
            {
                bool drawMethod()
                {
                    if (lastUpdateUiGameTime != null && QRInterface?.CurrentState != null)
                    {
                        QRInterface.Draw(Main.spriteBatch, lastUpdateUiGameTime);
                    }

                    return true;
                }

                layers.Insert(mouseTextIndex, new LegacyGameInterfaceLayer("MyMod: MyInterface", drawMethod, InterfaceScaleType.UI));
            }
            
            if (Main.GameMode.Equals(3) && Main.CreativeMenu.Blocked == false && Main.playerInventory == true)
            {
                ShowMyUI();
            }
            else
            {
                HideMyUI();
            }

        }

        internal void ShowMyUI() => QRInterface?.SetState(QRUI);

        internal void HideMyUI() => QRInterface?.SetState(null);

        public override void Load()
        {
            if (!Main.dedServ)
            {
                QRInterface = new UserInterface();

                QRUI = new TheUI();
                QRUI.Activate();
            }

            ShowMyUI();
        }

        public override void Unload()
        {
            QRUI?.Unload();
            QRUI = null;

            HideMyUI();
        }
    }
}