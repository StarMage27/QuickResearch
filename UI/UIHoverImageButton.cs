using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria.GameContent.UI.Elements;
using ReLogic.Content;

namespace QuickResearch.UI
{
    internal class UIHoverImageButton : UIImageButton
    {
        internal string HoverText;
        internal bool mouseover = false;
        internal bool mousedown = false;

        public UIHoverImageButton(Asset<Texture2D> texture, string hoverText) : base(texture) => HoverText = hoverText;

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            var texture = ModContent.Request<Texture2D>("QuickResearch/UI/QuickResearchButton");
            var textureMO = ModContent.Request<Texture2D>("QuickResearch/UI/QuickResearchButtonMouseOver");
            var textureD = ModContent.Request<Texture2D>("QuickResearch/UI/QuickResearchButtonPress");
            var textureMOD = ModContent.Request<Texture2D>("QuickResearch/UI/QuickResearchButtonPressMouseOver");

            base.DrawSelf(spriteBatch);

            SetVisibility(1, 1);

            if (mouseover == true)
            {
                Main.hoverItemName = HoverText;
                Main.LocalPlayer.mouseInterface = true;

                if (mousedown == false)
                {
                    SetImage(textureMO);
                }
                else
                {
                    SetImage(textureMOD);
                }
            }
            else
            {
                if (mousedown == false)
                {
                    SetImage(texture);
                }
                else
                {
                    SetImage(textureD);
                }
            }
        }
        public override void MouseOver(UIMouseEvent evt) => mouseover = true;

        public override void MouseOut(UIMouseEvent evt) => mouseover = false;

        public override void MouseDown(UIMouseEvent evt)
        {
            base.MouseDown(evt);
            mousedown = true;
        }

        public override void MouseUp(UIMouseEvent evt)
        {
            base.MiddleMouseUp(evt);
            mousedown = false;
        }
    }
}