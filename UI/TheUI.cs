using Microsoft.Xna.Framework.Graphics;
using QuickResearch.Config;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;

namespace QuickResearch.UI
{
    class TheUI : UIState
    {
        public override void OnInitialize()
        {
            bool showButtonsToggle = ModContent.GetInstance<QRConfig>().ShowButtonsToggle;

            Main.NewText(showButtonsToggle);

            if (showButtonsToggle)
            {
                string pathQR = "QuickResearch/UI/QuickResearchButton";
                var textureQuickResearch = ModContent.Request<Texture2D>(pathQR);
                UIHoverImageButton buttonQuickResearch = new(textureQuickResearch, "Quick Research", pathQR);
                buttonQuickResearch.Width.Set(34, 0);
                buttonQuickResearch.Height.Set(32, 0);
                buttonQuickResearch.Top.Set(267, 0);
                buttonQuickResearch.Left.Set(70, 0);
                buttonQuickResearch.OnLeftClick += OnButtonResearchClick;
                Append(buttonQuickResearch);

                string pathQC = "QuickResearch/UI/QuickCleanButton";
                var textureQuickClean = ModContent.Request<Texture2D>(pathQC);
                UIHoverImageButton buttonQuickClean = new(textureQuickClean, "Quick Clean", pathQC);
                buttonQuickClean.Width.Set(34, 0);
                buttonQuickClean.Height.Set(32, 0);
                buttonQuickClean.Top.Set(267, 0);
                buttonQuickClean.Left.Set(112, 0);
                buttonQuickClean.OnLeftClick += OnButtonCleanClick;
                Append(buttonQuickClean);
            }
        }

        private void OnButtonResearchClick(UIMouseEvent evt, UIElement listeningElement) => QRPlayer.BeginQuickResearch();

        private void OnButtonCleanClick(UIMouseEvent evt, UIElement listeningElement) => QRPlayer.BeginQuickClean();

        public override void OnDeactivate()
        {
            base.OnDeactivate();
        }

        internal void Unload() { }
    }
}