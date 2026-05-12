using Terraria;
using Terraria.ModLoader;
using Terraria.UI;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using QuickResearch.Config;

namespace QuickResearch.UI
{
    public class QuickResearchButton : ModSystem
    {
        internal UserInterface QRInterface;
        internal QRUI QRUI;
        private GameTime lastUpdateUiGameTime;
        bool showQRToggle = false;
        bool showRCToggle = false;
        bool showQCToggle = false;
        bool showDBToggle = false;
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

            bool changed = false;
            if (showQRToggle != ModContent.GetInstance<QRConfig>().ShowQRToggle)
            {
                changed = true;
            }
            if (showRCToggle != ModContent.GetInstance<QRConfig>().ShowRCToggle)
            {
                changed = true;
            }
            if (showQCToggle != ModContent.GetInstance<QRConfig>().ShowQCToggle)
            {
                changed = true;
            }
            // if (showDBToggle != ModContent.GetInstance<QRConfig>().ShowDBToggle)
            // {
            //     changed = true;
            // }
            
            if (changed)
            {
                Unload();
                Load();
            }

            if ((showQRToggle || showRCToggle || showQCToggle || showDBToggle) && Main.GameMode.Equals(3) && Main.CreativeMenu.Blocked == false && Main.playerInventory == true)
            {
                ShowMyUI();
            }
            else
            {
                HideMyUI();
            }

        }

        private void ShowMyUI()
        {
            QRInterface?.SetState(QRUI);
        }

        private void HideMyUI()
        {
            QRInterface?.SetState(null);
        }

        public override void OnLocalizationsLoaded()
        {
            Unload();
            showQRToggle = ModContent.GetInstance<QRConfig>().ShowQRToggle;
            showRCToggle = ModContent.GetInstance<QRConfig>().ShowRCToggle;
            showQCToggle = ModContent.GetInstance<QRConfig>().ShowQCToggle;
            // showDBToggle = ModContent.GetInstance<QRConfig>().ShowDBToggle;

            if (!Main.dedServ)
            {
                QRInterface = new UserInterface();

                QRUI = new QRUI(showQRToggle, showQCToggle, showRCToggle, showDBToggle);
                QRUI.Activate();
            }

            ShowMyUI();
        }
        // public override void Load()
        // {
        //     showQRToggle = ModContent.GetInstance<QRConfig>().ShowQRToggle;
        //     showRCToggle = ModContent.GetInstance<QRConfig>().ShowRCToggle;
        //     showQCToggle = ModContent.GetInstance<QRConfig>().ShowQCToggle;
        //     // showDBToggle = ModContent.GetInstance<QRConfig>().ShowDBToggle;

        //     if (!Main.dedServ)
        //     {
        //         QRInterface = new UserInterface();

        //         QRUI = new QRUI(showQRToggle, showQCToggle, showRCToggle, showDBToggle);
        //         QRUI.Activate();
        //     }

        //     ShowMyUI();
        // }

        public override void Unload()
        {
            QRUI?.Unload();
            QRUI = null;

            HideMyUI();
        }
    }
}