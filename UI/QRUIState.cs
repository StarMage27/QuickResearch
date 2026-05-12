using Microsoft.Xna.Framework.Graphics;
using QuickResearch.Config;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;

namespace QuickResearch.UI
{
    class QRUI : UIState
    {
        const string texturePath = "QuickResearch/UI/";

        bool qREnabled = false;
        bool qCEnabled = false;
        bool rCEnabled = false;
        bool dBEnabled = false;

        public QRUI()
        {
            qREnabled = false;
            qCEnabled = false;
            rCEnabled = false;
            dBEnabled = false;
        }

        public QRUI(bool newQREnabled, bool newQCEnabled, bool newRCEnabled, bool newDBEnabled)
        {
            qREnabled = newQREnabled;
            qCEnabled = newQCEnabled;
            rCEnabled = newRCEnabled;
            dBEnabled = newDBEnabled;
        }

        public override void OnInitialize()
        {
            int offset = 0;

            if (qREnabled)
            {
                string buttonQRName = "Quick Research";
                string pathQR = texturePath + buttonQRName.Replace(" ", "") + "Button/" + buttonQRName.Replace(" ", "") + "Button";
                var textureQR = ModContent.Request<Texture2D>(pathQR);
                UIHoverImageButton buttonQR = new(textureQR, QRTranslation.QRButtonText.Value, pathQR);   
                buttonQR.Width.Set(34, 0);
                buttonQR.Height.Set(32, 0);
                buttonQR.Top.Set(267, 0);
                buttonQR.Left.Set(70, 0);
                buttonQR.OnLeftClick += OnButtonQRClick;
                Append(buttonQR);
                offset += 42;
            }
            
            if (rCEnabled)
            {
                string buttonRCName = "Research Craftable";
                string pathRC = texturePath + buttonRCName.Replace(" ", "") + "Button/" + buttonRCName.Replace(" ", "") + "Button";
                var textureRC = ModContent.Request<Texture2D>(pathRC);
                UIHoverImageButton buttonRC = new(textureRC, QRTranslation.RCButtonText.Value, pathRC);
                buttonRC.Width.Set(34, 0);
                buttonRC.Height.Set(32, 0);
                buttonRC.Top.Set(267, 0);
                buttonRC.Left.Set(70 + offset, 0);
                buttonRC.OnLeftClick += OnButtonRCClick;
                Append(buttonRC);
                offset += 42;
            }

            if (qCEnabled)
            {
                string buttonQCName = "Quick Clean";
                string pathQC = texturePath + buttonQCName.Replace(" ", "") + "Button/" + buttonQCName.Replace(" ", "") + "Button";
                var textureQC = ModContent.Request<Texture2D>(pathQC);
                UIHoverImageButton buttonQC = new(textureQC, QRTranslation.QCButtonText.Value, pathQC);
                buttonQC.Width.Set(34, 0);
                buttonQC.Height.Set(32, 0);
                buttonQC.Top.Set(267, 0);
                buttonQC.Left.Set(70 + offset, 0);
                buttonQC.OnLeftClick += OnButtonQCClick;
                Append(buttonQC);
                offset += 42;
            }
            
            if (dBEnabled)
            {
                string buttonDBName = "Debug";
                string pathDB = texturePath + buttonDBName.Replace(" ", "") + "Button/" + buttonDBName.Replace(" ", "") + "Button";
                var textureDB = ModContent.Request<Texture2D>(pathDB);
                UIHoverImageButton buttonDB = new(textureDB, QRTranslation.DBButtonText.Value, pathDB);
                buttonDB.Width.Set(34, 0);
                buttonDB.Height.Set(32, 0);
                buttonDB.Top.Set(267, 0);
                buttonDB.Left.Set(70 + offset, 0);
                // buttonDB.OnLeftClick += OnButtonDBClick;
                // buttonDB.OnRightClick += OnButtonDBRClick;
                Append(buttonDB);
                offset += 42;
            }
        }

        private void OnButtonQRClick(UIMouseEvent evt, UIElement listeningElement) => QRPlayer.BeginQuickResearch();
        private void OnButtonQCClick(UIMouseEvent evt, UIElement listeningElement) => QRPlayer.BeginQuickClean();
        private void OnButtonRCClick(UIMouseEvent evt, UIElement listeningElement) => QRPlayer.ResearchCraftable();
        // private void OnButtonDBClick(UIMouseEvent evt, UIElement listeningElement) => QRPlayer.DisplayDebugInfo();
        // private void OnButtonDBRClick(UIMouseEvent evt, UIElement listeningElement) => QRPlayer.DisplayDebugInfo2();

        public override void OnDeactivate() => base.OnDeactivate();

        internal void Unload() { }
    }
}