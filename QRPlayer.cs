using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameInput;
using QuickResearch.Config;
using Terraria.Audio;
using Terraria.GameContent.Creative;
using System.Collections.Generic;

namespace QuickResearch
{
    public class QRPlayer : ModPlayer
    {
        public List<Item> allItems;

        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            if (Main.GameMode.Equals(3))
            {
                if (QuickResearch.QRBind.JustPressed) { BeginQuickResearch(); }
                if (QuickResearch.QCBind.JustPressed) { BeginQuickClean(); }
                if (QuickResearch.RCBind.JustPressed) { ResearchCraftable(); }
            }
        }

        public static void BeginQuickResearch()
        {
            bool flagResearched = false;
            bool flagSacrificed = false;
            bool completeResearchToggle = ModContent.GetInstance<QRConfig>().CompleteResearchToggle;
            bool showResearchMessagesToggle = ModContent.GetInstance<QRConfig>().ShowResearchMessagesToggle;
            bool researchCoinsToggle = ModContent.GetInstance<QRConfig>().ResearchCoinsToggle;
            bool researchCraftableAfterQuickResearchToggle = ModContent.GetInstance<QRConfig>().ResearchCraftableAfterQuickResearchToggle;

            Item[] inventory = Main.LocalPlayer.inventory;

            for (int i = 0; i < inventory.Length; i++)
            {
                Item item = inventory[i];
                int itemID = item.type;

                if (item.favorited || item.IsAir) { continue; }
                if (item == Main.LocalPlayer.HeldItem) { continue; }
                if (!researchCoinsToggle && item.IsACoin) { continue; }
                if (!CreativeItemSacrificesCatalog.Instance.TryGetSacrificeCountCapToUnlockInfiniteItems(itemID, out _)) { continue; }

                string itemName = item.Name;
                int currentSacrificeCount = CreativeUI.GetSacrificeCount(itemID, out bool isResearched);
                int maxSacrificeCount = CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[itemID];

                if (isResearched) { continue; }
                if (item.stack >= maxSacrificeCount - currentSacrificeCount || !completeResearchToggle)
                {
                    flagSacrificed = true;

                    CreativeUI.SacrificeItem(item, out int amountSacrificed);

                    currentSacrificeCount = CreativeUI.GetSacrificeCount(itemID, out isResearched);

                    if (isResearched) { flagResearched = true; }

                    if (showResearchMessagesToggle)
                    {
                        if (isResearched)
                        {
                            Main.NewText(
                                $"[i/s{amountSacrificed}:{itemID}]" +
                                $" Researched {itemName}!"
                                );
                        }
                        else
                        {
                            Main.NewText(
                                $"[i/s{amountSacrificed}:{itemID}]" +
                                $" Sacrificed {itemName}" +
                                $" ({currentSacrificeCount}/{maxSacrificeCount})"
                                );
                        }
                    }
                }
            }

            if (flagResearched)
            {
                SoundEngine.PlaySound(SoundID.Research);
                SoundEngine.PlaySound(SoundID.ResearchComplete);
            }
            else if (flagSacrificed)
            {
                SoundEngine.PlaySound(SoundID.Research);
            }
            else
            {
                SoundEngine.PlaySound(SoundID.MenuTick);
            }

            if (researchCraftableAfterQuickResearchToggle)
            {
                ResearchCraftable();
            }
        }

        public static void BeginQuickClean()
        {
            bool showCleanMessagesToggle = ModContent.GetInstance<QRConfig>().ShowCleanMessagesToggle;
            bool cleanCoinsToggle = ModContent.GetInstance<QRConfig>().CleanCoinsToggle;

            bool flagCleaned = false;
            Item[] inventory = Main.LocalPlayer.inventory;

            for (int i = 0; i < inventory.Length; i++)
            {
                Item item = inventory[i];
                int itemID = item.type;

                if (item.favorited || item.IsAir) { continue; }
                if (item == Main.LocalPlayer.HeldItem) { continue; }
                if (!cleanCoinsToggle && item.IsACoin) { continue; }
                if (!CreativeItemSacrificesCatalog.Instance.TryGetSacrificeCountCapToUnlockInfiniteItems(itemID, out _)) { continue; }

                string itemName = item.Name;
                int itemAmount = item.stack;

                CreativeUI.GetSacrificeCount(itemID, out bool isResearched);

                if (isResearched)
                {
                    item.TurnToAir();
                    flagCleaned = true;
                }

                if (showCleanMessagesToggle)
                {
                    Main.NewText(
                        $"[i/s{itemAmount}:{itemID}]" +
                        $" Cleaned {itemName}."
                        );
                }
            }

            if (flagCleaned)
            {
                SoundEngine.PlaySound(SoundID.Grab);
            }
            else
            {
                SoundEngine.PlaySound(SoundID.MenuTick);
            }
        }

        public static void ResearchCraftable()
        {
            bool showResearchCraftableMessagesToggle = ModContent.GetInstance<QRConfig>().ShowResearchCraftableMessagesToggle;

            bool itemsAreResearched = true;
            bool itemsWereResearched = false;
            string researchedItems = "";

            while (itemsAreResearched == true)
            {
                itemsAreResearched = false;

                foreach (var recipe in Main.recipe)
                {
                    bool stationResearched = false;
                    
                    if (recipe.requiredTile.Count == 0)
                    {
                        stationResearched = true;
                    }

                    if (recipe.createItem.type == 0)
                    {
                        continue;
                    }

                    foreach (var tile in recipe.requiredTile)
                    {
                        foreach (var (item, tileID) in QuickResearch.itemsAndTileIDsOfStations)
                        {
                            if (tile == tileID)
                            {
                                CreativeUI.GetSacrificeCount(item.type, out bool requiredStationIsResearched);
                                if (requiredStationIsResearched)
                                {
                                    stationResearched = true;
                                    break;
                                }
                            }
                        }
                        if (stationResearched) { break; }
                    }

                    if (!stationResearched) { continue; }

                    bool allItemsResearched = true;

                    foreach (var requiredItem in recipe.requiredItem)
                    {
                        CreativeUI.GetSacrificeCount(requiredItem.type, out bool requiredItemIsResearched);
                        if (!requiredItemIsResearched)
                        {
                            allItemsResearched = false;
                            break;
                        }
                    }

                    if (allItemsResearched)
                    {
                        Item item = recipe.createItem;
                        CreativeUI.GetSacrificeCount(item.type, out bool resultingItemIsResearched);

                        if (resultingItemIsResearched)
                        {
                            continue;
                        }

                        CreativeUI.ResearchItem(item.type);
                        itemsAreResearched = true;
                        itemsWereResearched = true;
                        researchedItems += $"[i/s1:{item.type}]";
                    }
                }
            }

            if (showResearchCraftableMessagesToggle && itemsWereResearched)
            {
                Main.NewText("Researched craftable items: " + researchedItems);
            }

            if (itemsWereResearched)
            {
                SoundEngine.PlaySound(SoundID.ResearchComplete);
            }
            else
            {
                SoundEngine.PlaySound(SoundID.MenuTick);
            }
        }
    }
}