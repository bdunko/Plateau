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
    public class PEntityChest : PlacedEntity, IInteract, IHaveHoveringInterface
    {
        public static int INVENTORY_SIZE = 30;

        private ItemStack[] contents;
        protected PartialRecolorSprite sprite;

        public PEntityChest(PartialRecolorSprite sprite, Vector2 tilePosition, PlaceableItem sourceItem, DrawLayer drawLayer) : base(tilePosition, sourceItem, drawLayer, sprite.GetFrameWidth() / 8, sprite.GetFrameHeight() / 8)
        {
            this.sprite = sprite;
            sprite.AddLoop("anim", 0, 3, true);
            sprite.AddLoop("placement", 4, 6, false);
            sprite.SetLoop("placement");
            contents = new ItemStack[INVENTORY_SIZE];
            for(int i = 0; i < contents.Length; i++)
            {
                contents[i] = new ItemStack(ItemDict.NONE, 0);
            }
        }

        public virtual ItemStack[] GetContents()
        {
            return contents;
        }

        public override void Update(float deltaTime, Area area)
        {
            sprite.Update(deltaTime);
            if(sprite.IsCurrentLoopFinished())
            {
                sprite.SetLoop("anim");
            }
        }

        public override void Draw(SpriteBatch sb, float layerDepth)
        {
            sprite.Draw(sb, new Vector2(position.X, position.Y + 1), Color.White, layerDepth);
        }

        public override SaveState GenerateSave()
        {
            SaveState save = base.GenerateSave();
            for(int i = 0; i < INVENTORY_SIZE; i++)
            {
                save.AddData("inventory" + i, contents[i].GetItem().GetName());
                save.AddData("inventory" + i + "quantity", contents[i].GetQuantity().ToString());
            }

            return save;
        }

        public override void LoadSave(SaveState state)
        {
            for(int i = 0; i < INVENTORY_SIZE; i++)
            {
                contents[i] = new ItemStack(ItemDict.GetItemByName(state.TryGetData("inventory" + i, ItemDict.NONE.GetName()))
                    , Int32.Parse(state.TryGetData("inventory" + i + "quantity", "0")));
            }
        }

        public string GetLeftClickAction(EntityPlayer player)
        {
            return "";
        }

        public virtual string GetRightClickAction(EntityPlayer player)
        {
            return "Open Chest";
        }

        public override RectangleF GetCollisionRectangle()
        {
            return new RectangleF(position, new Size2(sprite.GetFrameWidth(), sprite.GetFrameHeight()));
        }

        public void InteractRight(EntityPlayer player, Area area, World world)
        {
            player.SetInterfaceState(InterfaceState.CHEST);
            player.Pause();
        }

        public void InteractLeft(EntityPlayer player, Area area, World world)
        {
            //do nothing
        }

        public string GetLeftShiftClickAction(EntityPlayer player)
        {
            return "";
        }

        public string GetRightShiftClickAction(EntityPlayer player)
        {
            return "";
        }

        public override void OnRemove(EntityPlayer player, Area area, World world)
        {
            foreach(ItemStack stack in contents)
            {
                for(int i = 0; i < stack.GetQuantity(); i++)
                {
                    area.AddEntity(new EntityItem(stack.GetItem(), this.position - new Vector2(0, 8)));
                }
            }
            base.OnRemove(player, area, world);
        }

        public void InteractRightShift(EntityPlayer player, Area area, World world)
        {
            //do nothing
        }

        public void InteractLeftShift(EntityPlayer player, Area area, World world)
        {
            //do nothing
        }

        public virtual void RemoveItemStackAt(int inventorySlot)
        {
            contents[inventorySlot] = new ItemStack(ItemDict.NONE, 0);
        }

        public virtual ItemStack GetInventoryItemStack(int inventorySlot)
        {
            return contents[inventorySlot];
        }

        public virtual void AddItemStackAt(ItemStack itemStack, int inventorySlot)
        {
            contents[inventorySlot] = itemStack;
        }

        public virtual Texture2D GetInventoryItemTexture(int i)
        {
            return contents[i].GetItem().GetTexture();
        }

        public virtual bool GetInventoryItemStackable(int i)
        {
            return contents[i].GetMaxQuantity() > 1;
        }

        public virtual int GetInventoryItemQuantity(int i)
        {
            return contents[i].GetQuantity();
        }

        public virtual HoveringInterface GetHoveringInterface(EntityPlayer player)
        {
            if (contents[0].GetItem() != ItemDict.NONE)
            {
                if(contents[1].GetItem() != ItemDict.NONE)
                {
                    if(contents[2].GetItem() != ItemDict.NONE)
                    {
                        return new HoveringInterface(
                            new HoveringInterface.Row(
                                new HoveringInterface.ItemStackElement(contents[0]), new HoveringInterface.ItemStackElement(contents[1]), new HoveringInterface.ItemStackElement(contents[2])));
                    }
                    return new HoveringInterface(
                        new HoveringInterface.Row(
                            new HoveringInterface.ItemStackElement(contents[0]), new HoveringInterface.ItemStackElement(contents[1])));
                }
                return new HoveringInterface(
                    new HoveringInterface.Row(
                        new HoveringInterface.ItemStackElement(contents[0])));
            }
            return new HoveringInterface();
        }
    }
}
