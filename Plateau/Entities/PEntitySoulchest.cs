using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using Plateau.Components;
using Plateau.Items;
using Plateau.Particles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plateau.Entities
{
    public class PEntitySoulchest : PEntityChest, IInteract, IHaveHoveringInterface
    {
        private static Dictionary<Item, ItemStack[]> soulchestContents = null;
        private static List<Item> chestList;

        public PEntitySoulchest(PartialRecolorSprite sprite, Vector2 tilePosition, PlaceableItem sourceItem, DrawLayer drawLayer) : base(sprite, tilePosition, sourceItem, drawLayer)
        {
            sprite.AddLoop("anim", 4, 14, true);
            sprite.AddLoop("placement", 0, 3, false);
            sprite.SetLoop("placement");
            PrepChestList();
            PrepDictionary();
        }

        private static void PrepChestList()
        {
            if(chestList == null)
            {
                chestList = new List<Item>();
                chestList.Add(ItemDict.SOULCHEST);
                foreach(Util.RecolorMap recolor in Util.DYE_COLORS)
                {
                    string name = ItemDict.SOULCHEST.GetName() + (" (" + recolor.name + ")");
                    chestList.Add(ItemDict.GetItemByName(name));
                }
            }
        }

        private static void PrepDictionary()
        {
            if (soulchestContents == null)
            {
                soulchestContents = new Dictionary<Item, ItemStack[]>();
                foreach(Item chest in chestList)
                {
                    soulchestContents[chest] = new ItemStack[INVENTORY_SIZE];
                    for(int i = 0; i < soulchestContents[chest].Length; i++)
                    {
                        soulchestContents[chest][i] = new ItemStack(ItemDict.NONE, 0);
                    }
                }
            }
        }

        public override ItemStack[] GetContents()
        {
            return soulchestContents[itemForm];
        }

        public new static SaveState GenerateSave()
        {
            if (soulchestContents == null)
            {
                PrepChestList();
                PrepDictionary();
            }

            SaveState save = new SaveState(SaveState.Identifier.SOULCHEST);
            foreach(Item chest in chestList) {
                for (int i = 0; i < INVENTORY_SIZE; i++)
                {
                    save.AddData(chest.GetName() + "inventory" + i, soulchestContents[chest][i].GetItem().GetName());
                    save.AddData(chest.GetName() + "inventory" + i + "quantity", soulchestContents[chest][i].GetQuantity().ToString());
                }
            }

            return save;
        }

        public new static void LoadSave(SaveState state)
        {
            if (soulchestContents == null)
            {
                PrepChestList();
                PrepDictionary();
            }
            foreach(Item chest in chestList)
            {
                for (int i = 0; i < INVENTORY_SIZE; i++)
                {
                    soulchestContents[chest][i] = new ItemStack(ItemDict.GetItemByName(state.TryGetData(chest.GetName() + "inventory" + i, ItemDict.NONE.GetName()))
                        , Int32.Parse(state.TryGetData(chest.GetName() + "inventory" + i + "quantity", "0")));
                }
            }

        }

        public override string GetRightClickAction(EntityPlayer player)
        {
            return "Open Chest";
        }

        public override void OnRemove(EntityPlayer player, Area area, World world)
        {
            base.OnRemove(player, area, world);
        }

        public override void RemoveItemStackAt(int inventorySlot)
        {
            soulchestContents[itemForm][inventorySlot] = new ItemStack(ItemDict.NONE, 0);
        }

        public override ItemStack GetInventoryItemStack(int inventorySlot)
        {
            return soulchestContents[itemForm][inventorySlot];
        }

        public override void AddItemStackAt(ItemStack itemStack, int inventorySlot)
        {
            soulchestContents[itemForm][inventorySlot] = itemStack;
        }

        public override Texture2D GetInventoryItemTexture(int i)
        {
            return soulchestContents[itemForm][i].GetItem().GetTexture();
        }

        public override bool GetInventoryItemStackable(int i)
        {
            return soulchestContents[itemForm][i].GetMaxQuantity() > 1;
        }

        public override int GetInventoryItemQuantity(int i)
        {
            return soulchestContents[itemForm][i].GetQuantity();
        }

        public override HoveringInterface GetHoveringInterface(EntityPlayer player)
        {
            if (soulchestContents[itemForm][0].GetItem() != ItemDict.NONE)
            {
                if (soulchestContents[itemForm][1].GetItem() != ItemDict.NONE)
                {
                    if (soulchestContents[itemForm][2].GetItem() != ItemDict.NONE)
                    {
                        return new HoveringInterface(
                            new HoveringInterface.Row(
                                new HoveringInterface.ItemStackElement(soulchestContents[itemForm][0]), new HoveringInterface.ItemStackElement(soulchestContents[itemForm][1]), new HoveringInterface.ItemStackElement(soulchestContents[itemForm][2])));
                    }
                    return new HoveringInterface(
                        new HoveringInterface.Row(
                            new HoveringInterface.ItemStackElement(soulchestContents[itemForm][0]), new HoveringInterface.ItemStackElement(soulchestContents[itemForm][1])));
                }
                return new HoveringInterface(
                    new HoveringInterface.Row(
                        new HoveringInterface.ItemStackElement(soulchestContents[itemForm][0])));
            }
            return null;
        }
    }
}
