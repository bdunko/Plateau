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
    public class PEntitySynthesizer : PlacedEntity, IInteract, ITick
    {
        private enum ExtractorState
        {
            FINISHED, WORKING
        }

        private static Dictionary<Area.AreaEnum, List<List<Item>>> SYNTHESIZED_ITEMS = new Dictionary<Area.AreaEnum, List<List<Item>>>()
        {
            {Area.AreaEnum.INTERIOR, new List<List<Item>>() { 
                new List<Item>() { ItemDict.ORIGAMI_FISH, ItemDict.CLAM_LINGUINI, ItemDict.GOLDEN_BELLPEPPER, ItemDict.GOLDEN_WATERMELON_SLICE}, 
                new List<Item>() { ItemDict.ALBINO_WING, ItemDict.METAMORPHIC_KEY, ItemDict.STORMBRINGER_KOI, ItemDict.MINT_EXTRACT}, 
                new List<Item>() { ItemDict.ORIGAMI_SWAN, ItemDict.TRUFFLE, ItemDict.HARP, ItemDict.BASKET},
                new List<Item>() { ItemDict.LOAMY_COMPOST, ItemDict.LUMINOUS_STEW, ItemDict.GOLDEN_PUMPKIN, ItemDict.TRILOBITE} } },
            {Area.AreaEnum.FARM, new List<List<Item>>() { 
                new List<Item>() { ItemDict.WOOD, ItemDict.IMPERIAL_INCENSE, ItemDict.WOODEN_BENCH, ItemDict.ROASTED_PUMPKIN}, 
                new List<Item>() { ItemDict.GREAT_WHITE_SHARK, ItemDict.MYTHRIL_CHIP, ItemDict.SEDIMENTARY_KEY, ItemDict.RED_FEATHER}, 
                new List<Item>() { ItemDict.EARTH_CRYSTAL, ItemDict.FLAGPOLE, ItemDict.ONESIE, ItemDict.SHINING_COMPOST}, 
                new List<Item>() { ItemDict.SALT_SHARDS, ItemDict.CLAM, ItemDict.QUARTZ, ItemDict.WIND_CRYSTAL} } },
            {Area.AreaEnum.BEACH, new List<List<Item>>() { 
                new List<Item>() { ItemDict.EMPRESS_BUTTERFLY, ItemDict.PLATFORM_WOOD_FARM, ItemDict.SEAWEED, ItemDict.SLIDE}, 
                new List<Item>() { ItemDict.MILKING_PAIL, ItemDict.PIN_BRACER, ItemDict.WOOLEN_CLOTH, ItemDict.FOX_TAIL}, 
                new List<Item>() { ItemDict.PLANK, ItemDict.FIRE_CRYSTAL, ItemDict.IRON_BAR, ItemDict.HEART_VESSEL}, 
                new List<Item>() { ItemDict.BOX, ItemDict.OLIVE_SAPLING, ItemDict.PINK_DYE, ItemDict.LETHAL_SASHIMI} }},
            {Area.AreaEnum.TOWN, new List<List<Item>>() { 
                new List<Item>() { ItemDict.BLIND_DINNER, ItemDict.PILE_OF_BRICKS, ItemDict.WATER_CRYSTAL, ItemDict.HARDWOOD}, 
                new List<Item>() { ItemDict.TORNADO_PENDANT, ItemDict.SWORDFISH_POT, ItemDict.GOLDEN_FENCE, ItemDict.WEEDS}, 
                new List<Item>() { ItemDict.INFERNAL_SHARK, ItemDict.RATATOUILLE, ItemDict.RUBY, ItemDict.ROYAL_CREST}, 
                new List<Item>() { ItemDict.RED_ANGEL, ItemDict.SPORES, ItemDict.BLINDFOLD, ItemDict.BIRDS_NEST} }},
            {Area.AreaEnum.S0, new List<List<Item>>() { 
                new List<Item>() { ItemDict.HAYBALE, ItemDict.LINEN_CLOTH, ItemDict.HONEYCOMB, ItemDict.BUTTERED_ROLLS}, 
                new List<Item>() { ItemDict.SHRIMP, ItemDict.CART, ItemDict.TOPAZ, ItemDict.SHEARS}, 
                new List<Item>() { ItemDict.TORN_JEANS, ItemDict.DARK_ANGLER, ItemDict.BOAR_JERKY, ItemDict.IRON_CHIP}, 
                new List<Item>() { ItemDict.WATERMELON_WINE, ItemDict.MUSICBOX_RING, ItemDict.IGNEOUS_KEY, ItemDict.ROYAL_JELLY} }},
            {Area.AreaEnum.S1, new List<List<Item>>() { 
                new List<Item>() { ItemDict.SHIITAKE, ItemDict.SWORDFISH, ItemDict.ADAMANTITE_BAR, ItemDict.BOXING_MITTS}, 
                new List<Item>() { ItemDict.GLASS_SHEET, ItemDict.FRIED_EGG, ItemDict.TALL_FENCE, ItemDict.ADAMANTITE_ORE}, 
                new List<Item>() { ItemDict.DIAMOND, ItemDict.SOUR_WINE, ItemDict.OPAL, ItemDict.BLACKENED_OCTOPUS}, 
                new List<Item>() { ItemDict.ICE_NINE, ItemDict.SNEAKERS, ItemDict.GOLDEN_COCONUT, ItemDict.SHORT_SLEEVE_TEE} }},
            {Area.AreaEnum.S2, new List<List<Item>>() { 
                new List<Item>() { ItemDict.STAG_BEETLE, ItemDict.CHEESE, ItemDict.QUEENS_STINGER, ItemDict.LEMON_SAPLING}, 
                new List<Item>() { ItemDict.INVINCIROID, ItemDict.MILK, ItemDict.GUANO, ItemDict.THICK_COMPOST}, 
                new List<Item>() { ItemDict.SOLAR_PANEL, ItemDict.SALTY_CHARM, ItemDict.VAMPYRIC_CREST, ItemDict.AUTUMNS_KISS}, 
                new List<Item>() { ItemDict.WOOL, ItemDict.UNSTABLE_LIQUID, ItemDict.GOLDEN_EGG, ItemDict.FAIRY_DUST} }},
            {Area.AreaEnum.S3, new List<List<Item>>() { 
                new List<Item>() { ItemDict.COTTON_CLOTH, ItemDict.COAL, ItemDict.CRYSTAL_KEY, ItemDict.PEARL}, 
                new List<Item>() { ItemDict.BASEBALL_CAP, ItemDict.CRAB, ItemDict.SKELETON_KEY, ItemDict.GOLDEN_WOOL}, 
                new List<Item>() { ItemDict.RAINCOAT, ItemDict.MACKEREL, ItemDict.BLACK_DYE, ItemDict.QUERADE_MASK}, 
                new List<Item>() { ItemDict.SEARED_TUNA, ItemDict.LUMBER_RING, ItemDict.STONE, ItemDict.DARK_GREY_DYE} }},
            {Area.AreaEnum.S4, new List<List<Item>>() { 
                new List<Item>() { ItemDict.LUNAR_WHALE, ItemDict.WATERMELON_ICE, ItemDict.ELDERBERRY_JAM, ItemDict.BERRY_BUSH_PLANTER}, 
                new List<Item>() { ItemDict.JEWEL_SPIDER, ItemDict.LIGHT_GREY_DYE, ItemDict.SKY_BOTTLE, ItemDict.PRIMORDIAL_SHELL}, 
                new List<Item>() { ItemDict.UMBRELLA, ItemDict.BANANA_JAM, ItemDict.GAIA_PENDANT, ItemDict.DANDYLION_CHARM}, 
                new List<Item>() { ItemDict.POLLEN_PUFF, ItemDict.TOOLBOX, ItemDict.GOLDEN_LEAF, ItemDict.SEAFOOD_PAELLA} }},
            {Area.AreaEnum.APEX, new List<List<Item>>() { 
                new List<Item>() { ItemDict.SUIT_OF_ARMOR, ItemDict.FROST_SCULPTURE, ItemDict.SKY_ROSE, ItemDict.WATERLESS_MELON}, 
                new List<Item>() { ItemDict.PRISMATIC_FEATHER, ItemDict.TRIANGULATE_FLOOR, ItemDict.QUEEN_AROWANA, ItemDict.LEMONADE}, 
                new List<Item>() { ItemDict.BIZARRE_PERFUME, ItemDict.EARTHWORM, ItemDict.IGLOO, ItemDict.FIRE_HYDRANT}, 
                new List<Item>() { ItemDict.UN_DYE, ItemDict.TATAMI_FLOOR, ItemDict.CLAM, ItemDict.WILD_MEAT} }}
        };

        private static int PROCESSING_TIME = 23 * 60;

        private static int MAX_CAPACITY = 1;

        private PartialRecolorSprite sprite;
        private ItemStack heldItem;
        private int timeRemaining;
        private ExtractorState state;
        private ResultHoverBox resultHoverBox;

        public PEntitySynthesizer(PartialRecolorSprite sprite, Vector2 tilePosition, PlaceableItem sourceItem, DrawLayer drawLayer) : base(tilePosition, sourceItem, drawLayer, sprite.GetFrameWidth() / 8, sprite.GetFrameHeight() / 8)
        {
            this.heldItem = new ItemStack(ItemDict.NONE, 0);
            this.sprite = sprite;
            sprite.AddLoop("finished", 0, 0, true);
            sprite.AddLoop("working", 4, 5, true);
            sprite.AddLoop("placement", 0, 3, false);
            sprite.SetLoop("placement");
            this.state = ExtractorState.WORKING;
            this.timeRemaining = PROCESSING_TIME;
            this.resultHoverBox = new ResultHoverBox();
        }
        public override void Draw(SpriteBatch sb, float layerDepth)
        {
            sprite.Draw(sb, new Vector2(position.X, position.Y + 1), Color.White, layerDepth);
            resultHoverBox.Draw(sb, new Vector2(position.X + (sprite.GetFrameWidth() / 2), position.Y), layerDepth);
        }

        public override SaveState GenerateSave()
        {
            SaveState save = base.GenerateSave();
            save.AddData("state", state.ToString());
            save.AddData("item", heldItem.GetItem().GetName());
            save.AddData("quantity", heldItem.GetQuantity().ToString());
            save.AddData("timeRemaining", timeRemaining.ToString());
            return save;
        }

        public override void LoadSave(SaveState saveState)
        {
            heldItem = new ItemStack(ItemDict.GetItemByName(saveState.TryGetData("item", ItemDict.NONE.GetName())),
                Int32.Parse(saveState.TryGetData("quantity", "0")));
            timeRemaining = Int32.Parse(saveState.TryGetData("timeRemaining", "0"));
            string stateStr = saveState.TryGetData("state", ExtractorState.WORKING.ToString());
            if (stateStr.Equals(ExtractorState.WORKING.ToString()))
            {
                state = ExtractorState.WORKING;
            }
            else if (stateStr.Equals(ExtractorState.FINISHED.ToString()))
            {
                state = ExtractorState.FINISHED;
            }
        }

        public override void Update(float deltaTime, Area area)
        {
            sprite.Update(deltaTime);
            if(sprite.IsCurrentLoopFinished())
            {
                sprite.SetLoopIfNot("working");
            }
            if (!sprite.IsCurrentLoop("placement"))
            {
                if (state == ExtractorState.WORKING)
                {
                    sprite.SetLoopIfNot("working");
                }
                else
                {
                    sprite.SetLoopIfNot("finished");
                }
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
            if (heldItem.GetItem() != ItemDict.NONE)
            {
                return "Collect";
            }
            return "";
        }

        public void InteractRight(EntityPlayer player, Area area, World world)
        {
            if (heldItem.GetItem() != ItemDict.NONE)
            {
                for (int i = 0; i < heldItem.GetQuantity(); i++)
                {
                    area.AddEntity(new EntityItem(heldItem.GetItem(), new Vector2(position.X, position.Y - 10)));
                }

                sprite.SetLoop("placement");
                heldItem = new ItemStack(ItemDict.NONE, 0);
                state = ExtractorState.WORKING;
            } 
        }

        public void InteractLeft(EntityPlayer player, Area area, World world)
        {
            //do nothing
        }

        public void InteractRightShift(EntityPlayer player, Area area, World world)
        {
            //do nothing
        }

        public void InteractLeftShift(EntityPlayer player, Area area, World world)
        {
            //do nothing
        }

        private static Item GetSynthesizedItem(Area.AreaEnum areaEnum, Vector2 tilePosition)
        {
            return SYNTHESIZED_ITEMS[areaEnum][(int)tilePosition.X%4][(int)tilePosition.Y%4];
        }

        public void Tick(int time, EntityPlayer player, Area area, World world)
        {
            timeRemaining -= time;
            if (timeRemaining <= 0 && state == ExtractorState.WORKING)
            {
                heldItem = new ItemStack(GetSynthesizedItem(area.GetAreaEnum(), tilePosition), heldItem.GetQuantity()+1);
                sprite.SetLoop("placement");
                timeRemaining = PROCESSING_TIME;
                if(heldItem.GetQuantity() == MAX_CAPACITY)
                {
                    state = ExtractorState.FINISHED;
                }
            }
        }
    }
}
