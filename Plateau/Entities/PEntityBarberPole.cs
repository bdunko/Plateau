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
    public class PEntityBarberPole : PlacedEntity, IInteract, IHaveHoveringInterface
    {
        private PartialRecolorSprite sprite;
        private static ClothingItem[] HAIRSTYLE_LIST = { ItemDict.HAIR_AFRO_ALFONSO, ItemDict.HAIR_ALIENATED_ALICE, ItemDict.HAIR_BAREBONES_BRIAN, ItemDict.HAIR_BERTHA_BUN,
        ItemDict.HAIR_BENNY_BOWLCUT, ItemDict.HAIR_CLEANCUT_CHARLOTTE, ItemDict.HAIR_CARLOS_COOL, ItemDict.HAIR_EARNEST_EMMA, ItemDict.HAIR_CLEAN_CONOR, ItemDict.HAIR_FLASHY_FRIZZLE,
        ItemDict.HAIR_COMBED_CHRISTOPH, ItemDict.HAIR_FLUFFY_FELICIA, ItemDict.HAIR_COWLICK_COLTON, ItemDict.HAIR_GORGEOUS_GEORGEANN, ItemDict.HAIR_DIRTY_JACK, ItemDict.HAIR_HANGOVER_HANNA,
        ItemDict.HAIR_FREDDIE_FRINGE, ItemDict.HAIR_INNOCENT_ILIA, ItemDict.HAIR_GABRIEL_PART, ItemDict.HAIR_LUXURY_LARA, ItemDict.HAIR_LAZY_XAVIER, ItemDict.HAIR_MOUNTAIN_CLIMBER_MADELINE, 
        ItemDict.HAIR_MAXWELL_MOHAWK, ItemDict.HAIR_PADMA_PERFECTION, ItemDict.HAIR_MR_BALD, ItemDict.HAIR_PERSEPHONE_PUNK, ItemDict.HAIR_OVERHANG_OWEN, ItemDict.HAIR_SOPHIA_SWING,
        ItemDict.HAIR_PONYTAIL_TONYTALE, ItemDict.HAIR_STRICT_SUSIE, ItemDict.HAIR_SKULLCAP_STEVENS, ItemDict.HAIR_THE_ORIGINAL_OLIVIA, ItemDict.HAIR_LUCKY_LUKE, ItemDict.HAIR_ZAPPY_ZADIE};
        private static ClothingItem[] FACIALHAIR_LIST = { ItemDict.CLOTHING_NONE, ItemDict.FACIALHAIR_BEARD, ItemDict.FACIALHAIR_BARON_MUSTACHE, ItemDict.FACIALHAIR_CAVEMAN, ItemDict.FACIALHAIR_DROOPY, ItemDict.FACIALHAIR_GOATEE,
        ItemDict.FACIALHAIR_FLUFF, ItemDict.FACIALHAIR_FULLBEARD, ItemDict.FACIALHAIR_MONK, ItemDict.FACIALHAIR_GOATEEBACK, ItemDict.FACIALHAIR_SHORTBEARD, ItemDict.FACIALHAIR_SOULPATCH, ItemDict.FACIALHAIR_SIDEBURNS};

        public PEntityBarberPole(PartialRecolorSprite sprite, Vector2 tilePosition, PlaceableItem sourceItem, DrawLayer drawLayer) : base(tilePosition, sourceItem, drawLayer, sprite.GetFrameWidth() / 8, sprite.GetFrameHeight() / 8)
        {
            this.sprite = sprite;
            sprite.AddLoop("idle", 4, 11, true);
            sprite.AddLoop("placement", 0, 3, false);
            sprite.SetLoop("placement");
        }

        public override void Draw(SpriteBatch sb)
        {
            sprite.Draw(sb, new Vector2(position.X, position.Y + 1), Color.White);
        }

        public override void Update(float deltaTime, Area area)
        {
            sprite.Update(deltaTime);
            if (sprite.IsCurrentLoopFinished())
            {
                sprite.SetLoop("idle");
            }
        }

        public override RectangleF GetCollisionRectangle()
        {
            return new RectangleF(position, new Size2(sprite.GetFrameWidth(), sprite.GetFrameHeight()));
        }

        public string GetLeftShiftClickAction(EntityPlayer player)
        {
            return "Dye Hair";
        }

        public string GetRightShiftClickAction(EntityPlayer player)
        {
            return "Facial Hair";
        }

        public string GetLeftClickAction(EntityPlayer player)
        {
            return "";
        }

        public string GetRightClickAction(EntityPlayer player)
        {
            return "Haircut";
        }

        private static int getIndexOfHair(EntityPlayer player)
        {
            ClothingItem currentHairstyle = (ClothingItem)ItemDict.GetItemByName(player.GetHair().GetItem().GetName().Split('(')[0].Trim());

            for(int i = 0; i < HAIRSTYLE_LIST.Length; i++)
            {
                if (currentHairstyle == HAIRSTYLE_LIST[i])
                    return i;
            }

            throw new Exception("No matching hair found!");
        }

        private static int getIndexOfFacialHair(EntityPlayer player)
        {
            ClothingItem currentFacialHair = (ClothingItem)ItemDict.GetItemByName(player.GetFacialHair().GetItem().GetName().Split('(')[0].Trim());

            for (int i = 0; i < FACIALHAIR_LIST.Length; i++)
            {
                if (currentFacialHair == FACIALHAIR_LIST[i])
                    return i;
            }

            throw new Exception("No matching facial hair found!");
        }

        private static int getIndexOfHairColor(EntityPlayer player)
        {
            string currentHairColor = player.GetHair().GetItem().GetName().Split('(')[1];
            currentHairColor = currentHairColor.Substring(0, currentHairColor.Length - 1);

            for (int i = 0; i < Util.HAIR_COLORS.Length; i++)
            {
                if (currentHairColor == Util.HAIR_COLORS[i].name)
                {
                    return i;
                }
            }

            throw new Exception("No matching hair color found!");
        }

        //Haircut
        public void InteractRight(EntityPlayer player, Area area, World world)
        {
            //get new hairstyle
            int newHairstyleIndex = (getIndexOfHair(player) + 1) % HAIRSTYLE_LIST.Length;

            //get current hair color
            string currentHairColor = player.GetHair().GetItem().GetName().Split('(')[1];
            currentHairColor = "(" + currentHairColor;

            //set player's hair to new style, with same color
            Item toGive = ItemDict.GetItemByName(HAIRSTYLE_LIST[newHairstyleIndex].GetName() + " " + currentHairColor);
            player.SetHair(new ItemStack(toGive, 1));
        }

        public void InteractLeft(EntityPlayer player, Area area, World world)
        {
            
        }

        public void InteractRightShift(EntityPlayer player, Area area, World world)
        {
            string currentHairColor = player.GetHair().GetItem().GetName().Split('(')[1];
            currentHairColor = "(" + currentHairColor;

            int newFacialHairIndex = (getIndexOfFacialHair(player) + 1) % FACIALHAIR_LIST.Length;
            if (FACIALHAIR_LIST[newFacialHairIndex] == ItemDict.CLOTHING_NONE)
            {
                player.SetFacialHair(new ItemStack(ItemDict.CLOTHING_NONE, 1));
            }
            else
            {
                Item toGive = ItemDict.GetItemByName(FACIALHAIR_LIST[newFacialHairIndex].GetName() + " " + currentHairColor);
                player.SetFacialHair(new ItemStack(toGive, 1));
            }
        }

        public void InteractLeftShift(EntityPlayer player, Area area, World world)
        {
            //hair color
            string currentHairstyle = player.GetHair().GetItem().GetName().Split('(')[0].Trim();
            string currentFacialHair = player.GetFacialHair().GetItem().GetName().Split('(')[0].Trim();

            string newHairColor = Util.HAIR_COLORS[(getIndexOfHairColor(player) + 1) % Util.HAIR_COLORS.Length].name;

            Item toGive = ItemDict.GetItemByName(currentHairstyle + " (" + newHairColor + ")");
            player.SetHair(new ItemStack(toGive, 1));
            if (player.GetFacialHair().GetItem() != ItemDict.CLOTHING_NONE)
            {
                toGive = ItemDict.GetItemByName(currentFacialHair + " (" + newHairColor + ")");
                player.SetFacialHair(new ItemStack(toGive, 1));
            }
        }
        public virtual HoveringInterface GetHoveringInterface(EntityPlayer player)
        {
            return new HoveringInterface(
                new HoveringInterface.Row(
                    new HoveringInterface.TextElement("Hairstyle: " + (getIndexOfHair(player)+1) + "/" + (HAIRSTYLE_LIST.Length))),
                new HoveringInterface.Row(
                    new HoveringInterface.TextElement("Facial Hair: " + (getIndexOfFacialHair(player)+1) + "/" + (FACIALHAIR_LIST.Length))),
                new HoveringInterface.Row(
                    new HoveringInterface.TextElement("Color: " + (getIndexOfHairColor(player)+1) + "/" + (Util.HAIR_COLORS.Length)))
                );
        }
    }
}
