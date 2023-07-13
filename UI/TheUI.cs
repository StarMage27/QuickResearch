using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;
using Terraria.UI;

namespace QuickResearch.UI
{
    class TheUI : UIState
    {
        public override void OnInitialize()
        {
            var texture = ModContent.Request<Texture2D>("QuickResearch/UI/QuickResearchButton");
            UIHoverImageButton button = new(texture, "Quick Research");
            button.Width.Set(34, 0);
            button.Height.Set(32, 0);
            button.Top.Set(267, 0);
            button.Left.Set(70, 0);
            button.OnLeftClick += OnButtonClick;
            
            Append(button);
        }

        private void OnButtonClick(UIMouseEvent evt, UIElement listeningElement)
        {
            QRPlayer.BeginQuickResearch();
        }

        internal void Unload()
        {

        }
    }
}