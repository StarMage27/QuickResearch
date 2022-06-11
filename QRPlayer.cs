using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameInput;
using QuickResearch.Config;
using static Terraria.ID.ContentSamples;
using static Terraria.Audio.SoundEngine;
using static Terraria.GameContent.Creative.CreativeItemSacrificesCatalog;

namespace QuickResearch
{
    public class QRPlayer : ModPlayer
    {
        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            if (QuickResearch.QRBind.JustPressed && Main.GameMode.Equals(3))
            {
                BeginResearch();
            }
        }

        private static void SetSacrificeCount(Item item, int newSarcrificeCount)
        {
            Main.LocalPlayerCreativeTracker.ItemSacrifices.SetSacrificeCountDirectly(ItemPersistentIdsByNetIds[item.type], newSarcrificeCount);
        }

        public static void BeginResearch()
        {
            bool researched = false;
            bool researching = false;
            bool completeResearchEnabled = ModContent.GetInstance<QRConfig>().CompleteResearchToggle;

            foreach (Item item in Main.LocalPlayer.inventory)
            {
                if ((item != Main.LocalPlayer.HeldItem) && !item.favorited && !item.IsAir && Instance.TryGetSacrificeCountCapToUnlockInfiniteItems(item.type, out _))
                {
                    int count = Main.LocalPlayer.creativeTracker.ItemSacrifices.GetSacrificeCount(item.type);
                    int countNeeded = Instance.SacrificeCountNeededByItemId[item.type];

                    if ((completeResearchEnabled && (item.stack >= countNeeded)) || ((!completeResearchEnabled) && (count < countNeeded)))
                    {
                        researching = true;
                        int newSarcrificeCount = count + item.stack;

                        if (newSarcrificeCount > countNeeded)
                        {
                            researched = true;
                            newSarcrificeCount = countNeeded;
                            item.stack = count + item.stack - countNeeded;
                            SetSacrificeCount(item, newSarcrificeCount);
                        }
                        else if (newSarcrificeCount == countNeeded)
                        {
                            researched = true;
                            SetSacrificeCount(item, newSarcrificeCount);
                            item.TurnToAir();
                        }
                        else
                        {
                            SetSacrificeCount(item, newSarcrificeCount);
                            item.TurnToAir();
                        }
                    }
                }
            }

            if (researched)
            {
                PlaySound(SoundID.Research);
                PlaySound(SoundID.ResearchComplete);
            }
            else if (researching == true)
            {
                PlaySound(SoundID.Research);
            }
            else
            {
                PlaySound(SoundID.MenuTick);
            }
        }
    }
}