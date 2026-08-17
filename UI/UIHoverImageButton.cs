using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria.GameContent.UI.Elements;
using ReLogic.Content;

namespace QuickResearch.UI
{
    internal class UIHoverImageButton(Asset<Texture2D> texture, string hoverText, string nPath) : UIImageButton(texture)
    {
        internal string HoverText = hoverText;
        internal bool mouseover = false;
        internal bool mousedown = false;
        internal string path = nPath;

        public void SetHoverText(string newHoverText)
        {
            HoverText = newHoverText;
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            var texture = ModContent.Request<Texture2D>(path);
            var textureMO = ModContent.Request<Texture2D>(path + "MouseOver");
            var textureD = ModContent.Request<Texture2D>(path + "Press");
            var textureMOD = ModContent.Request<Texture2D>(path + "PressMouseOver");

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

        public override void LeftMouseDown(UIMouseEvent evt)
        {
            base.LeftMouseDown(evt);
            mousedown = true;
        }

        public override void LeftMouseUp(UIMouseEvent evt)
        {
            base.MiddleMouseUp(evt);
            mousedown = false;
        }
    }
}
