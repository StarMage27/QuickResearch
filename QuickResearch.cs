using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QuickResearch
{
	public class QuickResearch : Mod
	{
        public static HashSet<Item> items; // All items

        public static HashSet<(Item item, int tileID)> itemsAndTileIDsOfStations;

        public static ModKeybind QRBind { get; set; }
        public static ModKeybind QCBind { get; set; }
        public static ModKeybind RCBind { get; set; }

        public override void Load()
        {
            QRBind = KeybindLoader.RegisterKeybind(this, "Quick research", "J");
            RCBind = KeybindLoader.RegisterKeybind(this, "Research craftable", "K");
            QCBind = KeybindLoader.RegisterKeybind(this, "Quick clean", "L");

            GetItems();
            GetAllCraftingItemsAndTiles();
        }

        public static void GetItems()
        {
            items = new HashSet<Item>();

            List<int> itemsID = new();

            for (int i = 0; i < ItemLoader.ItemCount; i++)
            {
                try
                {
                    Item item = new(i);
                    items.Add(item);

                }
                catch { }
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