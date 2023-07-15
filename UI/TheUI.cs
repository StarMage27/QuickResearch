using Microsoft.Xna.Framework.Graphics;
using QuickResearch.Config;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;

namespace QuickResearch.UI
{
    class TheUI : UIState
    {
        const string texturePath = "QuickResearch/UI/";

        public override void OnInitialize()
        {
            string buttonQRName = "Quick Research";
            string pathQR = texturePath + buttonQRName.Replace(" ", "") + "Button/" + buttonQRName.Replace(" ", "") + "Button";
            var textureQR = ModContent.Request<Texture2D>(pathQR);
            UIHoverImageButton buttonQR = new(textureQR, buttonQRName, pathQR);   
            buttonQR.Width.Set(34, 0);
            buttonQR.Height.Set(32, 0);
            buttonQR.Top.Set(267, 0);
            buttonQR.Left.Set(70, 0);
            buttonQR.OnLeftClick += OnButtonQRClick;
            Append(buttonQR);

            string buttonQCName = "Quick Clean";
            string pathQC = texturePath + buttonQCName.Replace(" ", "") + "Button/" + buttonQCName.Replace(" ", "") + "Button";
            var textureQC = ModContent.Request<Texture2D>(pathQC);
            UIHoverImageButton buttonQC = new(textureQC, buttonQCName, pathQC);
            buttonQC.Width.Set(34, 0);
            buttonQC.Height.Set(32, 0);
            buttonQC.Top.Set(267, 0);
            buttonQC.Left.Set(154, 0);
            buttonQC.OnLeftClick += OnButtonQCClick;
            Append(buttonQC);

            string buttonRCName = "Research Craftable";
            string pathRC = texturePath + buttonRCName.Replace(" ", "") + "Button/" + buttonRCName.Replace(" ", "") + "Button";
            var textureRC = ModContent.Request<Texture2D>(pathRC);
            UIHoverImageButton buttonRC = new(textureRC, buttonRCName, pathRC);
            buttonRC.Width.Set(34, 0);
            buttonRC.Height.Set(32, 0);
            buttonRC.Top.Set(267, 0);
            buttonRC.Left.Set(112, 0);
            buttonRC.OnLeftClick += OnButtonRCClick;
            Append(buttonRC);
        }

        private void OnButtonQRClick(UIMouseEvent evt, UIElement listeningElement) => QRPlayer.BeginQuickResearch();

        private void OnButtonQCClick(UIMouseEvent evt, UIElement listeningElement) => QRPlayer.BeginQuickClean();

        private void OnButtonRCClick(UIMouseEvent evt, UIElement listeningElement) => QRPlayer.ResearchCraftable();

        public override void OnDeactivate() => base.OnDeactivate();

        internal void Unload() { }
    }
}