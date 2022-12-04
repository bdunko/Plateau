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
    public class PEntityBirdhouse : PlacedEntity, IInteract, ITick
    {
        private Area.AreaEnum[] INCLUDED_AREAS = { Area.AreaEnum.APEX, Area.AreaEnum.BEACH, Area.AreaEnum.FARM, Area.AreaEnum.S0, Area.AreaEnum.S1, Area.AreaEnum.S2, Area.AreaEnum.S3, Area.AreaEnum.S4, Area.AreaEnum.TOWN};
        private static int MAX_CAPACITY = 3;
        private static int PROCESSING_TIME = 20 * 60;
        private PartialRecolorSprite sprite;
        private int timeRemaining;
        private ResultHoverBox resultHoverBox;
        private ItemStack producedItem;

        public PEntityBirdhouse(PartialRecolorSprite sprite, Vector2 tilePosition, PlaceableItem sourceItem, DrawLayer drawLayer) : base(tilePosition, sourceItem, drawLayer, sprite.GetFrameWidth() / 8, sprite.GetFrameHeight() / 8)
        {
            this.producedItem = new ItemStack(ItemDict.NONE, 0);
            this.sprite = sprite;
            sprite.AddLoop("idle", 0, 0, true);
            sprite.AddLoop("placement", 0, 3, false);
            sprite.SetLoop("placement");
            this.timeRemaining = PROCESSING_TIME;
            this.resultHoverBox = new ResultHoverBox();
        }

        public override void Draw(SpriteBatch sb, float layerDepth)
        {
            sprite.Draw(sb, new Vector2(position.X, position.Y + 1), Color.Wheat, layerDepth);
            resultHoverBox.Draw(sb, new Vector2(position.X + (sprite.GetFrameWidth() / 2), position.Y), layerDepth);
        }

        public override SaveState GenerateSave()
        {
            SaveState save = base.GenerateSave();
            save.AddData("item", producedItem.GetItem().GetName());
            save.AddData("quantity", producedItem.GetQuantity().ToString());
            save.AddData("timeRemaining", timeRemaining.ToString());
            return save;
        }

        public override void LoadSave(SaveState saveState)
        {
            producedItem = new ItemStack(ItemDict.GetItemByName(saveState.TryGetData("item", ItemDict.NONE.GetName())),
                Int32.Parse(saveState.TryGetData("quantity", "0")));
            timeRemaining = Int32.Parse(saveState.TryGetData("timeRemaining", "0"));
        }

        public override void Update(float deltaTime, Area area)
        {
            sprite.Update(deltaTime);
            if (sprite.IsCurrentLoopFinished())
            {
                sprite.SetLoop("idle");
            }

            if (producedItem.GetItem() != ItemDict.NONE)
            {
                resultHoverBox.AssignItemStack(producedItem);
            }
            else
            {
                resultHoverBox.RemoveItemStack();
            }
            resultHoverBox.Update(deltaTime);
        }

        public override void OnRemove(EntityPlayer player, Area area, World world)
        {
            if (producedItem.GetItem() != ItemDict.NONE)
            {
                InteractRight(player, area, world);
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
            if (producedItem.GetItem() != ItemDict.NONE)
            {
                return "Gather";
            }
            return "";
        }

        public void InteractRight(EntityPlayer player, Area area, World world)
        {
            if (producedItem.GetItem() != ItemDict.NONE)
            {
                List<Item> result = new List<Item>();
                for (int j = 0; j < producedItem.GetQuantity(); j++)
                {
                    result.Add(producedItem.GetItem());
                }

                bool isInsideCave = area.IsTilePositionInsideCave(tilePosition);

                switch (area.GetAreaEnum())
                {
                    case Area.AreaEnum.FARM:
                    case Area.AreaEnum.TOWN:
                    case Area.AreaEnum.S0:
                        if (isInsideCave)
                        {
                            if (Util.RandInt(1, 100) <= 8) { result.Add(ItemDict.LANTERN_MOTH); }
                            if (Util.RandInt(1, 100) <= 9) { result.Add(ItemDict.EARTHWORM); }
                            if (Util.RandInt(1, 100) <= 4) { result.Add(ItemDict.RICE_GRASSHOPPER); }
                            if (Util.RandInt(1, 100) <= 2) { result.Add(ItemDict.CAVEWORM); }
                            switch (area.GetSeason())
                            {
                                case World.Season.SPRING:
                                    if (Util.RandInt(1, 100) <= 14) { result.Add(ItemDict.RASPBERRY); }
                                    if (Util.RandInt(1, 250) <= 1) { result.Add(ItemDict.OLIVE); }
                                    if (Util.RandInt(1, 100) <= 2) { result.Add(ItemDict.CHERRY); }
                                    break;
                                case World.Season.SUMMER:
                                    if (Util.RandInt(1, 100) <= 14) { result.Add(ItemDict.BLUEBERRY); }
                                    if (Util.RandInt(1, 250) <= 1) { result.Add(ItemDict.ORANGE); }
                                    break;
                                case World.Season.AUTUMN:
                                    if (Util.RandInt(1, 100) <= 8) { result.Add(ItemDict.BLACKBERRY); }
                                    if (Util.RandInt(1, 100) <= 12) { result.Add(ItemDict.ELDERBERRY); }
                                    if (Util.RandInt(1, 100) <= 3) { result.Add(ItemDict.APPLE); }
                                    if (Util.RandInt(1, 250) <= 1) { result.Add(ItemDict.LEMON); }
                                    break;
                            }
                        }
                        else
                        {
                            for (int j = 0; j < producedItem.GetQuantity(); j++)
                            {
                                if (Util.RandInt(1, 100) <= 40) { result.Add(ItemDict.BIRDS_NEST); }
                                if (Util.RandInt(1, 100) <= 40) { result.Add(ItemDict.BLACK_FEATHER); }
                                if (Util.RandInt(1, 100) <= 15) { result.Add(ItemDict.WOOD); }
                                if (Util.RandInt(1, 100) <= 8) { result.Add(ItemDict.WEEDS); }
                            }
                        }
                        break;
                    case Area.AreaEnum.BEACH:

                        for (int j = 0; j < producedItem.GetQuantity(); j++)
                        {
                            if (isInsideCave)
                            {
                                if (Util.RandInt(1, 100) <= 28) { result.Add(ItemDict.EARTHWORM); }
                                if (Util.RandInt(1, 100) <= 16) { result.Add(ItemDict.CAVEWORM); }
                                if (Util.RandInt(1, 100) <= 3) { result.Add(ItemDict.COCONUT); }
                                if (Util.RandInt(1, 250) <= 1) { result.Add(ItemDict.BANANA); }
                            }
                            else
                            {
                                if (Util.RandInt(1, 100) <= 20) { result.Add(ItemDict.BIRDS_NEST); }
                                if (Util.RandInt(1, 100) <= 20) { result.Add(ItemDict.SEAWEED); }
                                if (Util.RandInt(1, 100) <= 5) { result.Add(ItemDict.INKY_SQUID); }
                                if (Util.RandInt(1, 100) <= 5) { result.Add(ItemDict.SEA_URCHIN); }
                                if (Util.RandInt(1, 100) <= 1) { result.Add(ItemDict.TUNA); }
                                if (Util.RandInt(1, 100) <= 1) { result.Add(ItemDict.CRIMSON_CORAL); }
                                if (Util.RandInt(1, 100) <= 12) { result.Add(ItemDict.RED_GINGER); }
                            }
                        }
                        break;
                    case Area.AreaEnum.S1:
                        for (int j = 0; j < producedItem.GetQuantity(); j++)
                        {
                            if (isInsideCave)
                            {
                                if (Util.RandInt(1, 100) <= 25) { result.Add(ItemDict.RASPBERRY); }
                                if (Util.RandInt(1, 100) <= 12) { result.Add(ItemDict.CHERRY); }
                                if (Util.RandInt(1, 100) <= 5) { result.Add(ItemDict.PINK_LADYBUG); }
                            }
                            else
                            {
                                if (Util.RandInt(1, 100) <= 40) { result.Add(ItemDict.BIRDS_NEST); }
                                if (Util.RandInt(1, 100) <= 40) { result.Add(ItemDict.MOSSY_BARK); }
                                if (Util.RandInt(1, 100) <= 6) { result.Add(ItemDict.CARP); }
                                if (Util.RandInt(1, 100) <= 2) { result.Add(ItemDict.SALMON); }
                                if (Util.RandInt(1, 100) <= 10) { result.Add(ItemDict.WOOD); }
                                if (Util.RandInt(1, 100) <= 2) { result.Add(ItemDict.GOLDEN_LEAF); }
                            }
                        }
                        break;
                    case Area.AreaEnum.S2:
                        for (int j = 0; j < producedItem.GetQuantity(); j++)
                        {
                            if (Util.RandInt(1, 100) <= 35) { result.Add(ItemDict.GUANO); }
                            if (Util.RandInt(1, 100) <= 40) { result.Add(ItemDict.BAT_WING); }
                            if (Util.RandInt(1, 100) <= 12) { result.Add(ItemDict.EMERALD_MOSS); }
                            if (Util.RandInt(1, 100) <= 14) { result.Add(ItemDict.CAVE_FUNGI); }
                            if (Util.RandInt(1, 100) <= 5) { result.Add(ItemDict.BLACKBERRY); }
                            if (Util.RandInt(1, 100) <= 5) { result.Add(ItemDict.ELDERBERRY); }
                            if (Util.RandInt(1, 100) <= 1) { result.Add(ItemDict.CHERRY); }
                            if (Util.RandInt(1, 100) <= 1) { result.Add(ItemDict.SHIITAKE); }
                            if (Util.RandInt(1, 100) <= 3) { result.Add(ItemDict.LEMON); }
                        }
                        break;
                    case Area.AreaEnum.S3:
                        for (int j = 0; j < producedItem.GetQuantity(); j++)
                        {
                            if (isInsideCave)
                            {
                                if (Util.RandInt(1, 100) <= 1) { result.Add(ItemDict.LEMON); }
                                if (Util.RandInt(1, 100) <= 1) { result.Add(ItemDict.OLIVE); }
                                if (Util.RandInt(1, 100) <= 3) { result.Add(ItemDict.ORANGE); }
                                if (Util.RandInt(1, 100) <= 11) { result.Add(ItemDict.BLUEBERRY); }
                                if (Util.RandInt(1, 100) <= 3) { result.Add(ItemDict.PINEAPPLE); }
                            }
                            else
                            {
                                if (Util.RandInt(1, 100) <= 40) { result.Add(ItemDict.BIRDS_NEST); }
                                if (Util.RandInt(1, 100) <= 13) { result.Add(ItemDict.WOOD); }
                                if (Util.RandInt(1, 100) <= 5) { result.Add(ItemDict.COAL); }
                                if (Util.RandInt(1, 100) <= 2) { result.Add(ItemDict.FAIRY_DUST); }
                            }
                            if (Util.RandInt(1, 100) <= 5) { result.Add(ItemDict.COCONUT); }
                        }
                        
                        break;
                    case Area.AreaEnum.S4:
                    case Area.AreaEnum.APEX:
                        if (isInsideCave)
                        {
                            if (Util.RandInt(1, 100) <= 1) { result.Add(ItemDict.ICE_NINE); }
                        }
                        for (int j = 0; j < producedItem.GetQuantity(); j++)
                        {
                            if (Util.RandInt(1, 100) <= 7) { result.Add(ItemDict.WHITE_BLOWFISH); }
                            if (Util.RandInt(1, 100) <= 5) { result.Add(ItemDict.SKY_PIKE); }
                            if (Util.RandInt(1, 100) <= 2) { result.Add(ItemDict.PRISMATIC_FEATHER); }
                        }
                        if (Util.RandInt(1, 100) <= 15) { result.Add(ItemDict.SNOW_CRYSTAL); }
                        break;

                }
                if (Util.RandInt(1, 100) == 1) {
                    if(isInsideCave)
                    {
                        result.Add(ItemDict.ALBINO_WING);
                    } else
                    {
                        result.Add(ItemDict.PRISMATIC_FEATHER);
                    }
                }

                foreach (Item resultItem in result)
                {
                    area.AddEntity(new EntityItem(resultItem, new Vector2(position.X, position.Y - 10)));
                }
                if(producedItem.GetQuantity() == MAX_CAPACITY)
                    timeRemaining = PROCESSING_TIME;
                sprite.SetLoop("placement");
                producedItem = new ItemStack(ItemDict.NONE, 0);
            }
        }

        public void InteractLeft(EntityPlayer player, Area area, World world)
        {

        }

        public void InteractRightShift(EntityPlayer player, Area area, World world)
        {

        }

        public void InteractLeftShift(EntityPlayer player, Area area, World world)
        {

        }

        public void Tick(int minutesTicked, EntityPlayer player, Area area, World world)
        {
            timeRemaining -= minutesTicked;
            if (timeRemaining <= 0)
            {
                if (INCLUDED_AREAS.Contains(area.GetAreaEnum()))
                {
                    Item toProduce = ItemDict.NONE;
                    bool isInsideCave = area.IsTilePositionInsideCave(tilePosition);
                    if (isInsideCave)
                    {
                        toProduce = ItemDict.BAT_WING;
                    }
                    else
                    {
                        switch (area.GetAreaEnum())
                        {
                            case Area.AreaEnum.FARM:
                            case Area.AreaEnum.TOWN:
                            case Area.AreaEnum.S0:
                                toProduce = ItemDict.BLACK_FEATHER;
                                break;
                            case Area.AreaEnum.BEACH:
                                toProduce = ItemDict.WHITE_FEATHER;
                                break;
                            case Area.AreaEnum.S1:
                                toProduce = ItemDict.BLUE_FEATHER;
                                break;
                            case Area.AreaEnum.S2:
                                toProduce = ItemDict.BAT_WING;
                                break;
                            case Area.AreaEnum.S3:
                                toProduce = ItemDict.RED_FEATHER;
                                break;
                            case Area.AreaEnum.S4:
                            case Area.AreaEnum.APEX:
                                toProduce = ItemDict.BIRDS_NEST;
                                break;
                        }
                    }

                    producedItem = (producedItem.GetItem() == ItemDict.NONE ? new ItemStack(toProduce, 1) :new ItemStack(toProduce, producedItem.GetQuantity() + 1));
                    if (producedItem.GetQuantity() > MAX_CAPACITY)
                    {
                        producedItem.SetQuantity(MAX_CAPACITY);
                    }
                }
                timeRemaining = PROCESSING_TIME;
            }
        }
    }
}
