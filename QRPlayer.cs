using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameInput;
using QuickResearch.Config;
using Terraria.Audio;
using Terraria.GameContent.Creative;
using System.Collections.Generic;
using static Terraria.GameContent.Creative.CreativeUI;

namespace QuickResearch;

public class QRPlayer : ModPlayer
{
    private static readonly CreativeItemSacrificesCatalog sacrificesCatalog = CreativeItemSacrificesCatalog.Instance;

    public override void ProcessTriggers(TriggersSet triggersSet)
    {
        if (!Main.GameMode.Equals(GameModeID.Creative)) { return; }
        if (QuickResearch.QRBind.JustPressed) { BeginQuickResearch(); }
        if (QuickResearch.QCBind.JustPressed) { BeginQuickClean(); }
        if (QuickResearch.RCBind.JustPressed) { ResearchCraftable(); }
    }

    public static void BeginQuickResearch()
    {
        QRConfig config = ModContent.GetInstance<QRConfig>();
        bool flagResearched = false;
        bool flagSacrificed = false;
        bool showResearchMessagesToggle = config.ShowResearchMessagesToggle;

        Item[] inventory = Main.LocalPlayer.inventory;
        string sacrificedItems = "";
        string researchedItems = "";

        for (int i = 0; i < inventory.Length; i++)
        {
            Item item = inventory[i];
            int itemID = item.type;

            if (item.favorited || item.IsAir) { continue; }
            if (item == Main.LocalPlayer.HeldItem) { continue; }
            if (!config.ResearchCoinsToggle && item.IsACoin) { continue; }
            if (!sacrificesCatalog.TryGetSacrificeCountCapToUnlockInfiniteItems(itemID, out int maxSacrificeCount)) { continue; }

            int currentSacrificeCount = GetSacrificeCount(itemID, out bool isResearched);

            if (isResearched) { continue; }
            bool canBeResearched = item.stack >= maxSacrificeCount - currentSacrificeCount;
            if (!canBeResearched && config.CompleteResearchToggle) { continue; }

            flagSacrificed = true;

            SacrificeItem(item, out int amountSacrificed);

            if (canBeResearched) { flagResearched = true; }
            if (!showResearchMessagesToggle) { continue; }

            if (canBeResearched)
            {
                researchedItems += $"[i/s1:{itemID}]";
            }
            else
            {
                sacrificedItems += $"[i/s{amountSacrificed}:{itemID}]";
            }
        }

        if (flagResearched)
        {
            SoundEngine.PlaySound(SoundID.Research);
            SoundEngine.PlaySound(SoundID.ResearchComplete);
            if (showResearchMessagesToggle) {
                Main.NewText($"{QRTranslation.SacrificedChatText.Value}: {sacrificedItems}");
                Main.NewText($"{QRTranslation.ResearchedChatText.Value}: {researchedItems}");
            }
        }
        else if (flagSacrificed)
        {
            SoundEngine.PlaySound(SoundID.Research);
            if (showResearchMessagesToggle) {
                Main.NewText($"{QRTranslation.SacrificedChatText.Value}: {sacrificedItems}");
            }
        }
        else
        {
            SoundEngine.PlaySound(SoundID.MenuTick);
        }

        if (config.ResearchCraftableAfterQuickResearchToggle)
        {
            ResearchCraftable();
        }
    }

    public static void BeginQuickClean()
    {
        QRConfig config = ModContent.GetInstance<QRConfig>();
        bool showCleanMessagesToggle = config.ShowCleanMessagesToggle;
        bool flagCleaned = false;
        Item[] inventory = Main.LocalPlayer.inventory;
        string cleanedItems = "";
        
        for (int i = 0; i < inventory.Length; i++)
        {
            Item item = inventory[i];
            int itemID = item.type;

            if (item.favorited || item.IsAir) { continue; }
            if (item == Main.LocalPlayer.HeldItem) { continue; }
            if (!config.CleanCoinsToggle && item.IsACoin) { continue; }
            if (!sacrificesCatalog.TryGetSacrificeCountCapToUnlockInfiniteItems(itemID, out _)) { continue; }

            GetSacrificeCount(itemID, out bool isResearched);
            if (!isResearched) { continue; }

            int itemAmount = item.stack;
            item.TurnToAir();
            flagCleaned = true;

            if (!showCleanMessagesToggle) { continue; }
            cleanedItems += $"[i/s{itemAmount}:{itemID}]";
        }

        if (flagCleaned)
        {
            SoundEngine.PlaySound(SoundID.Grab);
        }
        else
        {
            SoundEngine.PlaySound(SoundID.MenuTick);
        }

        if (showCleanMessagesToggle)
        {
            Main.NewText($"{QRTranslation.QCChatText.Value}: {cleanedItems}.");
        }
    }

    public static void ResearchCraftable()
    {
        QRConfig config = ModContent.GetInstance<QRConfig>();
        bool showResearchCraftableMessagesToggle = config.ShowResearchCraftableMessagesToggle;

        bool itemsAreResearched = true;
        bool itemsWereResearched = false;
        string researchedItems = "";

        bool nearShimmer = Condition.NearShimmer.IsMet();
        GetSacrificeCount(ItemID.BottomlessShimmerBucket, out bool shimmerBucketResearched);

        while (itemsAreResearched == true)
        {
            itemsAreResearched = false;

            foreach (Recipe recipe in Main.recipe)
            {
                int outputItemID = recipe.createItem.type;
                if (outputItemID == ItemID.None) { continue; }
                GetSacrificeCount(outputItemID, out bool resultingItemIsResearched);
                if (resultingItemIsResearched)
                {
                    if (outputItemID == ItemID.DirtBlock) { continue; } // This is necessary, trust

                    // Shimmer uncrafting recipes
                    int transformedItemID = ItemID.Sets.ShimmerTransformToItem[outputItemID];
                    if (transformedItemID > 0 || !(nearShimmer || shimmerBucketResearched)) { continue; }

                    foreach (var item in recipe.requiredItem)
                    {
                        ItemSacrificeResult result2 = ResearchItem(item.type);
                        if (result2 == ItemSacrificeResult.CannotSacrifice) { continue; }

                        itemsAreResearched = true;
                        itemsWereResearched = true;
                        if (!showResearchCraftableMessagesToggle) { continue; }
                        researchedItems += $"[i/s1:{item.type}]";
                    }

                    continue;
                }

                if (!ConditionsAreMet(recipe)) { continue; }


                bool stationAvailable = false;

                if (recipe.requiredTile.Count == 0)
                {
                    stationAvailable = true;
                }
                else if (PlayerMeetsTileRequirements(Main.LocalPlayer, recipe))
                {
                    stationAvailable = true;
                }
                else
                {
                    foreach (var tile in recipe.requiredTile)
                    {
                        foreach (var (item, tileID) in QRModSystem.itemAndTileIDsOfStations)
                        {
                            if (tile != tileID) { continue; }

                            GetSacrificeCount(item.type, out bool requiredStationIsResearched);
                            if (requiredStationIsResearched)
                            {
                                stationAvailable = true;
                                break;
                            }
                        }
                        if (stationAvailable) { break; }
                    }
                }
                if (!stationAvailable) { continue; }

                bool allIngredientsResearched = true;
                foreach (var requiredItem in recipe.requiredItem)
                {
                    GetSacrificeCount(requiredItem.type, out bool requiredItemIsResearched);
                    if (requiredItemIsResearched)
                    { continue; }

                    bool allGroupsResearched = false;
                    if (recipe.acceptedGroups.Count > 0)
                    {
                        allGroupsResearched = true;
                        foreach (var group in recipe.acceptedGroups)
                        {
                            if (IsItemInResearchedGroup(requiredItem.type, group))
                            {
                                allGroupsResearched = true;
                                break;
                            }
                            else
                            {
                                allGroupsResearched = false;
                            }
                        }
                    }

                    if (!allGroupsResearched)
                    {
                        allIngredientsResearched = false;
                        break;
                    }
                }

                if (!CreativeItemSacrificesCatalog.Instance.TryGetSacrificeCountCapToUnlockInfiniteItems(outputItemID, out _)) { continue; }
                if (!allIngredientsResearched) { continue; }

                ItemSacrificeResult result = ResearchItem(outputItemID);
                if (result == ItemSacrificeResult.CannotSacrifice) { continue; }
                itemsAreResearched = true;
                itemsWereResearched = true;
                if (!showResearchCraftableMessagesToggle) { continue; }
                researchedItems += $"[i/s1:{outputItemID}]";

            }

            // Shimmer transformation recipes
            foreach (int itemID in QRModSystem.allItemIDs)
            {

                int transformedItemID = ItemID.Sets.ShimmerTransformToItem[itemID];
                if (transformedItemID <= 0 || !(nearShimmer || shimmerBucketResearched)) { continue; }

                GetSacrificeCount(itemID, out bool itemIsResearched);
                GetSacrificeCount(transformedItemID, out bool transformedItemIsResearched);
                if (!itemIsResearched || transformedItemIsResearched) { continue; }

                ItemSacrificeResult result = ResearchItem(transformedItemID);

                if (result == ItemSacrificeResult.CannotSacrifice) { continue; }

                itemsAreResearched = true;
                itemsWereResearched = true;
                if (!showResearchCraftableMessagesToggle) { continue; }
                researchedItems += $"[i/s1:{transformedItemID}]";
            }
        }


        if (showResearchCraftableMessagesToggle && itemsWereResearched)
        {
            Main.NewText($"{QRTranslation.RCItemsChatText.Value}: " + researchedItems);
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

    private static bool IsItemInResearchedGroup(int itemID, int groupID)
    {
        RecipeGroup group = RecipeGroup.recipeGroups[groupID];
        bool itemInTheGroup = false;
        bool groupHasItemsResearched = false;
        foreach (var groupItem in group.ValidItems)
        {
            GetSacrificeCount(groupItem, out bool groupItemIsResearched);

            if (groupItem == itemID)
            {
                itemInTheGroup = true;
            }

            if (groupItemIsResearched)
            {
                groupHasItemsResearched = true;
            }
        }

        return itemInTheGroup && groupHasItemsResearched;
    }

    readonly static int[] graveIDs = [
        ItemID.Tombstone,
        ItemID.GraveMarker,
        ItemID.CrossGraveMarker,
        ItemID.Headstone,
        ItemID.Gravestone,
        ItemID.Obelisk,

        ItemID.RichGravestone1,
        ItemID.RichGravestone2,
        ItemID.RichGravestone3,
        ItemID.RichGravestone4,
        ItemID.RichGravestone5,
    ];
    readonly static int[] waterItemIDs = [
        ItemID.WaterBucket,
        ItemID.BottomlessBucket,
        ItemID.GoldenSink,

        ItemID.OasisFountain,
        ItemID.CavernFountain,
        ItemID.IcyWaterFountain,
        ItemID.PureWaterFountain,
        ItemID.BloodWaterFountain,
        ItemID.DesertWaterFountain,
        ItemID.JungleWaterFountain,
        ItemID.CorruptWaterFountain,
        ItemID.CrimsonWaterFountain,
        ItemID.HallowedWaterFountain,
    ];
    readonly static int[] honeyItemIDs = [
        ItemID.HoneyBucket,
        ItemID.BottomlessHoneyBucket,
    ];
    readonly static int[] lavaItemIDs = [
        ItemID.LavaBucket,
        ItemID.BottomlessLavaBucket,
    ];
    readonly static int[] snowItemIDs = [
        ItemID.SnowBlock,
        ItemID.IceBlock,
        ItemID.PurpleIceBlock,
        ItemID.RedIceBlock,
        ItemID.PinkIceBlock,
    ];

    private static bool ConditionsAreMet(Recipe recipe)
    {
        bool conditionsMet = true;
        foreach (Condition condition in recipe.Conditions)
        {
            if (condition == Condition.NearWater && (AnyItemResearched(waterItemIDs) || AnyItemResearched(QRModSystem.sinks)))
            {
                break;
            }
            if (condition == Condition.NearHoney && AnyItemResearched(honeyItemIDs))
            {
                break;
            }
            if (condition == Condition.NearLava && AnyItemResearched(lavaItemIDs))
            {
                break;
            }
            if (condition == Condition.InSnow && AnyItemResearched(snowItemIDs))
            {
                break;
            }
            if (condition == Condition.InGraveyard && AnyItemResearched(graveIDs))
            {
                break;
            }
            conditionsMet = conditionsMet && condition.IsMet();
        }
        return conditionsMet;
    }

    private static bool AnyItemResearched(int[] itemIDs)
    {
        bool anyItemResearched = false;
        foreach (int itemID in itemIDs)
        {
            GetSacrificeCount(itemID, out bool researched);
            if (researched)
            {
                anyItemResearched = true;
                break;
            }
        }
        return anyItemResearched;
    }

    private static bool AnyItemResearched(List<int> itemIDs)
    {
        bool anyItemResearched = false;
        foreach (int itemID in itemIDs)
        {
            GetSacrificeCount(itemID, out bool researched);
            if (researched)
            {
                anyItemResearched = true;
                break;
            }
        }
        return anyItemResearched;
    }

    private static bool PlayerMeetsTileRequirements(Player player, Recipe recipe)
    {
        for (int i = 0; i < recipe.requiredTile.Count && recipe.requiredTile[i] != -1; i++)
        {
            if (!player.adjTile[recipe.requiredTile[i]])
                return false;
        }

        return true;
    }
}
