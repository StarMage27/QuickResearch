using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameInput;
using QuickResearch.Config;
using Terraria.Audio;
using Terraria.GameContent.Creative;
using Microsoft.Xna.Framework;

namespace QuickResearch
{
    public class QRPlayer : ModPlayer
    {
        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            if (QuickResearch.QRBind.JustPressed && Main.GameMode.Equals(3))
            {
                BeginQuickResearch();
            }
        }

        public static void BeginQuickResearch()
        {
            bool flag = false;
            bool flag2 = false;
            bool completeResearchToggle = ModContent.GetInstance<QRConfig>().CompleteResearchToggle;
            Item[] inventory = Main.LocalPlayer.inventory;
            int num = default(int);

            for (int i = 0; i < inventory.Length; i++)
            {
                if (inventory[i] == Main.LocalPlayer.HeldItem || inventory[i].favorited || inventory[i].IsAir || !CreativeItemSacrificesCatalog.Instance.TryGetSacrificeCountCapToUnlockInfiniteItems(inventory[i].type, out num))
                {
                    continue;
                }

                bool isFullyResearched = false;
                int CurrentSacrificeCount = CreativeUI.GetSacrificeCount(inventory[i].type, out isFullyResearched);
                int MaxSacrificeCount = CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[inventory[i].type];

                if (!isFullyResearched && (completeResearchToggle && inventory[i].stack >= MaxSacrificeCount - CurrentSacrificeCount) || (!completeResearchToggle))
                {
                    Main.NewText(inventory[i]);

                    flag2 = true;

                    int SubtractingAmount = inventory[i].stack;
                    int ItemID = inventory[i].type;


                    int amountWeSacrificed;
                    CreativeUI.SacrificeItem(inventory[i], out amountWeSacrificed);

                    CurrentSacrificeCount = CreativeUI.GetSacrificeCount(ItemID, out isFullyResearched);
                    if (isFullyResearched)
                    {
                        flag = true;
                    }

                    inventory[i].TurnToAir();

                }
            }

            if (flag)
            {
                SoundEngine.PlaySound(SoundID.Research, (Vector2?)null);
                SoundEngine.PlaySound(SoundID.ResearchComplete, (Vector2?)null);
            }
            else if (flag2)
            {
                SoundEngine.PlaySound(SoundID.Research, (Vector2?)null);
            }
            else
            {
                SoundEngine.PlaySound(SoundID.MenuTick, (Vector2?)null);
            }
        }
    }
}