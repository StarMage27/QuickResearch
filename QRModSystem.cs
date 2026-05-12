
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QuickResearch;

public class QRModSystem : ModSystem
{
    public static List<int> allItemIDs = [];
    public static List<(Item item, int tileID)> placeableItemsWithTileIDs = [];
    public static HashSet<(Item item, int tileID)> itemAndTileIDsOfStations = [];
    public static Dictionary<int, List<int>> itemIDAndItsGroups = [];
    public static HashSet<int> craftingTileIDs = [];

    public static List<int> sinks = [];

    public override void PostAddRecipes()
    {
        GetItemIDs();
        GetTileItems();
        // GetItemsWithGroups();
        GetAllCraftingItemsAndTiles();
        sinks = tileItemIDsCraftedFromItemID(ItemID.WaterBucket);
    }

    public static void GetTileItems()
    {
        for (int i = 1; i < ItemID.Count; i++)
        {
            Item item = ContentSamples.ItemsByType[i];
            int tile = item.createTile;
            if (tile == -1) { continue; }
            placeableItemsWithTileIDs.Add((item, tile));
        }

        IEnumerable<ModItem> modItems = ModContent.GetContent<ModItem>();
        foreach (ModItem modItem in modItems) {
            Item item = modItem.Item;
            int tile = item.createTile;
            if (tile == -1) { continue; }
            placeableItemsWithTileIDs.Add((item, tile));
        }
    }

    public static void GetItemIDs()
    {
        for (int i = 1; i < ItemID.Count; i++)
        {
            allItemIDs.Add(i);
        }

        IEnumerable<ModItem> modItems = ModContent.GetContent<ModItem>();
        foreach (ModItem modItem in modItems) {
            Item item = modItem.Item;
            allItemIDs.Add(item.type);
        }
    }

    public static void GetItemsWithGroups()
    {
        foreach (var (groupID, group) in RecipeGroup.recipeGroups)
        {
            foreach (int itemID in group.ValidItems)
            {
                bool itemIsHere = itemIDAndItsGroups.TryGetValue(itemID, out List<int> groups);
                if (!itemIsHere)
                {
                    itemIDAndItsGroups.Add(itemID, new List<int>(groupID));
                }
                else
                {
                    groups.Add(groupID);
                }
            }
        }
    }


    public static void GetAllCraftingItemsAndTiles()
    {
        foreach (Recipe recipe in Main.recipe)
        {
            foreach (int requiredTile in recipe.requiredTile)
            {
                craftingTileIDs.Add(requiredTile);
            }

        }

        foreach (var (item, tileID) in placeableItemsWithTileIDs)
        {
            foreach (int craftingTileID in craftingTileIDs)
            {
                if (tileID == craftingTileID)
                {
                    itemAndTileIDsOfStations.Add((item, craftingTileID));
                }
            }
        }
    }

    public static List<int> itemIDsCraftedFromItemID(int ingridientID)
    {
        HashSet<int> output = [];
        foreach (Recipe recipe in Main.recipe)
        {
            foreach (Item item in recipe.requiredItem)
            {
                if (ingridientID != item.type) { continue; }
                output.Add(recipe.createItem.type);
            }
        }
        return output.ToList();
    }

    public static List<int> tileItemIDsCraftedFromItemID(int ingridientID)
    {
        HashSet<int> output = [];
        foreach (Recipe recipe in Main.recipe)
        {
            foreach (Item item in recipe.requiredItem)
            {
                if (ingridientID != item.type) { continue; }
                if (recipe.createItem.createTile == -1) { continue; }
                output.Add(recipe.createItem.type);
            }
        }
        return output.ToList();
    }

    public override void Unload()
    {
        placeableItemsWithTileIDs = null;
        itemAndTileIDsOfStations = null;
    }
}