using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using Plateau.Components;
using Plateau.Items;

namespace Plateau.Entities
{
    public class PEntityGlassblower : PlacedEntity, IInteract, ITick
    {
        private enum GlassblowerState
        {
            IDLE, WORKING, FINISHED
        }

        private static int MAX_CAPACITY = 1;
        private GlassblowerState state;
        private static int PROCESSING_TIME = 23 * 60;
        private PartialRecolorSprite sprite;
        private int timeRemaining;
        private ResultHoverBox resultHoverBox;
        private ItemStack heldItem;
        private bool checkedGround;

        public PEntityGlassblower(PartialRecolorSprite sprite, Vector2 tilePosition, PlaceableItem sourceItem, DrawLayer drawLayer) : base(tilePosition, sourceItem, drawLayer, sprite.GetFrameWidth() / 8, sprite.GetFrameHeight() / 8)
        {
            this.state = GlassblowerState.IDLE;
            this.heldItem = new ItemStack(ItemDict.NONE, 0);
            this.sprite = sprite;
            sprite.AddLoop("idle", 0, 0, true);
            sprite.AddLoop("placement", 0, 3, false);
            sprite.SetLoop("placement");
            sprite.AddLoop("anim", 4, 9, true);
            this.timeRemaining = PROCESSING_TIME;
            this.resultHoverBox = new ResultHoverBox();
            checkedGround = false;
        }

        public override void Draw(SpriteBatch sb, float layerDepth)
        {
            sprite.Draw(sb, new Vector2(position.X, position.Y + 1), Color.Wheat, layerDepth);
            resultHoverBox.Draw(sb, new Vector2(position.X + (sprite.GetFrameWidth() / 2), position.Y), layerDepth);
        }

        public override SaveState GenerateSave()
        {
            SaveState save = base.GenerateSave();
            save.AddData("item", heldItem.GetItem().GetName());
            save.AddData("quantity", heldItem.GetQuantity().ToString());
            save.AddData("timeRemaining", timeRemaining.ToString());
            save.AddData("state", state.ToString());
            return save;
        }

        public override void LoadSave(SaveState saveState)
        {
            heldItem = new ItemStack(ItemDict.GetItemByName(saveState.TryGetData("item", ItemDict.NONE.GetName())),
                Int32.Parse(saveState.TryGetData("quantity", "0")));
            timeRemaining = Int32.Parse(saveState.TryGetData("timeRemaining", "0"));
            string stateStr = saveState.TryGetData("state", GlassblowerState.IDLE.ToString());
            if (stateStr.Equals(GlassblowerState.IDLE.ToString()))
            {
                state = GlassblowerState.IDLE;
            }
            else if (stateStr.Equals(GlassblowerState.WORKING.ToString()))
            {
                state = GlassblowerState.WORKING;
            }
            else if (stateStr.Equals(GlassblowerState.FINISHED.ToString()))
            {
                state = GlassblowerState.FINISHED;
            }

            if(heldItem.GetItem() != ItemDict.NONE)
            {
                checkedGround = true;
            }
        }

        public override void Update(float deltaTime, Area area)
        {
            if(!checkedGround)
            {
                if (area.GetGroundTileType((int)tilePosition.X, (int)tilePosition.Y + 2) == Area.GroundTileType.SAND) 
                {
                    state = GlassblowerState.WORKING;
                    timeRemaining = PROCESSING_TIME;
                }
                checkedGround = true;
            }

            sprite.Update(deltaTime);
            if (sprite.IsCurrentLoopFinished())
            {
                sprite.SetLoop("idle");
            }
            if (state == GlassblowerState.WORKING)
            {
                sprite.SetLoopIfNot("anim");
            }

            if (heldItem.GetItem() != ItemDict.NONE)
            {
                resultHoverBox.AssignItemStack(heldItem);
            }
            else
            {
                resultHoverBox.RemoveItemStack();
            }
            resultHoverBox.Update(deltaTime);
        }

        public override void OnRemove(EntityPlayer player, Area area, World world)
        {
            if (heldItem.GetItem() != ItemDict.NONE)
            {
                for (int i = 0; i < heldItem.GetQuantity(); i++)
                {
                    area.AddEntity(new EntityItem(heldItem.GetItem(), new Vector2(position.X, position.Y - 10)));
                }
            }
            base.OnRemove(player, area, world);
        }

        public override RectangleF GetCollisionRectangle()
        {
            return new RectangleF(position, new Size2(sprite.GetFrameWidth(), sprite.GetFrameHeight()));
        }

        public string GetLeftShiftClickAction(EntityPlayer player)
        {
            return "";
        }

        public string GetRightShiftClickAction(EntityPlayer player)
        {
            return "";
        }

        public string GetLeftClickAction(EntityPlayer player)
        {
            return "";
        }

        public string GetRightClickAction(EntityPlayer player)
        {
            if (state == GlassblowerState.FINISHED)
            {
                return "Take";
            }
            return "";
        }

        public void InteractRight(EntityPlayer player, Area area, World world)
        {
            if (state == GlassblowerState.FINISHED)
            {
                for (int i = 0; i < heldItem.GetQuantity(); i++)
                {
                    area.AddEntity(new EntityItem(heldItem.GetItem(), new Vector2(position.X, position.Y - 10)));
                }
                sprite.SetLoop("placement");
                heldItem = new ItemStack(ItemDict.NONE, 0);
                state = GlassblowerState.IDLE;
                checkedGround = false;
            }
        }

        public void InteractLeft(EntityPlayer player, Area area, World world)
        {

        }

        public void InteractRightShift(EntityPlayer player, Area area, World world)
        {
            //nothing
        }

        public void InteractLeftShift(EntityPlayer player, Area area, World world)
        {
            //nothing
        }

        public void Tick(int minutesTicked, EntityPlayer player, Area area, World world)
        {
            if (state == GlassblowerState.WORKING)
            {
                timeRemaining -= minutesTicked;
                if (timeRemaining <= 0)
                {
                    //in S1, 1/10 chance to generate Wind and Water Crystals
                    //in S3, 1/10 chance to generate Earth and Fire Crystals
                    if (area.GetAreaEnum() == Area.AreaEnum.S3 || area.GetAreaEnum() == Area.AreaEnum.S1)
                    {
                        switch (Util.RandInt(1, 10))
                        {
                            case 1:
                                heldItem = new ItemStack(area.GetAreaEnum() == Area.AreaEnum.S3 ? ItemDict.FIRE_CRYSTAL : ItemDict.WIND_CRYSTAL, Math.Min(MAX_CAPACITY, heldItem.GetQuantity() + 1));
                                break;
                            case 2:
                                heldItem = new ItemStack(area.GetAreaEnum() == Area.AreaEnum.S3 ? ItemDict.EARTH_CRYSTAL : ItemDict.WATER_CRYSTAL, Math.Min(MAX_CAPACITY, heldItem.GetQuantity() + 1));
                                break;
                            default:
                                heldItem = new ItemStack(ItemDict.GLASS_SHEET, Math.Min(MAX_CAPACITY, heldItem.GetQuantity() + 1));
                                break;
                        }
                    }
                    else
                    {
                        heldItem = new ItemStack(ItemDict.GLASS_SHEET, Math.Min(MAX_CAPACITY, heldItem.GetQuantity() + 1));
                    }
                    sprite.SetLoop("placement");
                    timeRemaining = PROCESSING_TIME;
                    if (heldItem.GetQuantity() == MAX_CAPACITY)
                    {
                        sprite.SetLoop("idle");
                        state = GlassblowerState.FINISHED;
                    }
                }
            }
        }
    }
}
