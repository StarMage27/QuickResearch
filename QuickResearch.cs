using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace QuickResearch
{
	public class QuickResearch : Mod
	{
        public static HashSet<Item> items; // All researchable items

        public static HashSet<(Item item, int tileID)> itemsAndTileIDsOfStations;

        public static ModKeybind QRBind { get; set; }
        public static ModKeybind QCBind { get; set; }
        public static ModKeybind RCBind { get; set; }

        public override void Load()
        {
            QRBind = KeybindLoader.RegisterKeybind(this, "Quick research", "J");
            QCBind = KeybindLoader.RegisterKeybind(this, "Quick clean", "K");
            RCBind = KeybindLoader.RegisterKeybind(this, "Research craftable", "L");

            GetResearchableItems();
            GetAllCraftingItemsAndTiles();
        }

        public static void GetResearchableItems()
        {
            items = new HashSet<Item>();

            List<int> itemsID = new();
            Main.LocalPlayerCreativeTracker.ItemSacrifices.FillListOfItemsThatCanBeObtainedInfinitely(itemsID);

            foreach (int itemID in itemsID)
            {
                Item item = new(itemID);
                items.Add(item);
            }
        }

        public static void GetAllCraftingItemsAndTiles()
        {
            itemsAndTileIDsOfStations = new HashSet<(Item item, int tileID)>();

            HashSet<int> craftingTilesIDs = new();

            foreach (Recipe recipe in Main.recipe)
            {
                foreach (int requiredTile in recipe.requiredTile)
                {
                    craftingTilesIDs.Add(requiredTile);
                }
            }

            foreach (Item item in items)
            {
                foreach (int craftingTileID in craftingTilesIDs)
                {
                    if (item.createTile.Equals(craftingTileID))
                    {
                        itemsAndTileIDsOfStations.Add((item, craftingTileID));
                    }
                }
            }
        }

        public override void Unload()
        {
            items = null;
            itemsAndTileIDsOfStations = null;

            QRBind = null;
            QCBind = null;
            RCBind = null;
        }
    }
}