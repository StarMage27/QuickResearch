using Microsoft.Xna.Framework.Graphics;
using QuickResearch.Config;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;

namespace QuickResearch.UI;

class QRUIState(bool newQREnabled, bool newQCEnabled, bool newRCEnabled) : UIState
{
    const string texturePath = "QuickResearch/UI/";

    private readonly bool qREnabled = newQREnabled;
    private readonly bool qCEnabled = newQCEnabled;
    private readonly bool rCEnabled = newRCEnabled;

    UIHoverImageButton? buttonQR = null;
    UIHoverImageButton? buttonRC = null;
    UIHoverImageButton? buttonQC = null;

    private string GetTexturePathForButton(string name)
    {
        string nameNoSpaces = name.Replace(" ", "");
        return $"{texturePath}{nameNoSpaces}Button/{nameNoSpaces}Button";
    }

    public void UpdateTranslations()
    {
        buttonQR?.SetHoverText(QRTranslation.QRButtonText.Value);
        buttonRC?.SetHoverText(QRTranslation.RCButtonText.Value);
        buttonQC?.SetHoverText(QRTranslation.QCButtonText.Value);
    }

    public override void OnInitialize()
    {
        int offset = 0;

        if (qREnabled)
        {
            string pathQR = GetTexturePathForButton("Quick Research");
            var textureQR = ModContent.Request<Texture2D>(pathQR);
            buttonQR = new(textureQR, QRTranslation.QRButtonText.Value, pathQR);
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
            string pathRC = GetTexturePathForButton("Research Craftable");
            var textureRC = ModContent.Request<Texture2D>(pathRC);
            buttonRC = new(textureRC, QRTranslation.RCButtonText.Value, pathRC);
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
            string pathQC = GetTexturePathForButton("Quick Clean");
            var textureQC = ModContent.Request<Texture2D>(pathQC);
            buttonQC = new(textureQC, QRTranslation.QCButtonText.Value, pathQC);
            buttonQC.Width.Set(34, 0);
            buttonQC.Height.Set(32, 0);
            buttonQC.Top.Set(267, 0);
            buttonQC.Left.Set(70 + offset, 0);
            buttonQC.OnLeftClick += OnButtonQCClick;
            Append(buttonQC);
            offset += 42;
        }
    }

    private void OnButtonQRClick(UIMouseEvent evt, UIElement listeningElement) => QRPlayer.BeginQuickResearch();
    private void OnButtonQCClick(UIMouseEvent evt, UIElement listeningElement) => QRPlayer.BeginQuickClean();
    private void OnButtonRCClick(UIMouseEvent evt, UIElement listeningElement) => QRPlayer.ResearchCraftable();

    public override void OnDeactivate() => base.OnDeactivate();

    internal void Unload()
    {
        buttonQR = null;
        buttonRC = null;
        buttonQC = null;
    }
}
