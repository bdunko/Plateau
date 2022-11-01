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
    public class PEntityBarberPole : PlacedEntity, IInteract
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

        public override void Draw(SpriteBatch sb, float layerDepth)
        {
            sprite.Draw(sb, new Vector2(position.X, position.Y + 1), Color.White, layerDepth);
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

        public void InteractRight(EntityPlayer player, Area area, World world)
        {
            //HAIRCUT
            //MAINTAIN CURRENT HAIR COLORING!
            string currentHairColor = player.GetHair().GetItem().GetName().Split('(')[1];
            currentHairColor = "(" + currentHairColor;
            ClothingItem currentHairstyle = (ClothingItem)ItemDict.GetItemByName(player.GetHair().GetItem().GetName().Split('(')[0].Trim());

            for(int i = 0; i < HAIRSTYLE_LIST.Length; i++)
            {
                if(currentHairstyle == HAIRSTYLE_LIST[i])
                {
                    i = i + 1;
                    if(i >= HAIRSTYLE_LIST.Length)
                    {
                        i = 0; //loop around
                    }
                    Item toGive = ItemDict.GetItemByName(HAIRSTYLE_LIST[i].GetName() + " " + currentHairColor);
                    player.SetHair(new ItemStack(toGive, 1));
                    break;
                }
            }
        }

        public void InteractLeft(EntityPlayer player, Area area, World world)
        {
            
        }

        public void InteractRightShift(EntityPlayer player, Area area, World world)
        {
            //FACIAL HAIR
            //MAINTAIN CURRENT HAIR COLORING!
            string currentHairColor = player.GetHair().GetItem().GetName().Split('(')[1];
            currentHairColor = "(" + currentHairColor;
            ClothingItem currentFacialHair = (ClothingItem)ItemDict.GetItemByName(player.GetFacialHair().GetItem().GetName().Split('(')[0].Trim());

            for (int i = 0; i < FACIALHAIR_LIST.Length; i++)
            {
                if (currentFacialHair == FACIALHAIR_LIST[i])
                {
                    i = i + 1;
                    if (i >= FACIALHAIR_LIST.Length)
                    {
                        i = 0; //loop around
                    }
                    if(FACIALHAIR_LIST[i] == ItemDict.CLOTHING_NONE)
                    {
                        player.SetFacialHair(new ItemStack(ItemDict.CLOTHING_NONE, 1));
                        break;
                    }
                    Item toGive = ItemDict.GetItemByName(FACIALHAIR_LIST[i].GetName() + " " + currentHairColor);
                    player.SetFacialHair(new ItemStack(toGive, 1));
                    break;
                }
            }
        }

        public void InteractLeftShift(EntityPlayer player, Area area, World world)
        {
            //CHANGE HAIR COLOR
            string currentHairColor = player.GetHair().GetItem().GetName().Split('(')[1];
            currentHairColor = currentHairColor.Substring(0, currentHairColor.Length - 1);
            string currentHairstyle = player.GetHair().GetItem().GetName().Split('(')[0].Trim();
            string currentFacialHair = player.GetFacialHair().GetItem().GetName().Split('(')[0].Trim();

            for (int i = 0; i < Util.HAIR_COLORS.Length; i++)
            {
                if (currentHairColor == Util.HAIR_COLORS[i].name)
                {
                    i = i + 1;
                    if (i >= Util.HAIR_COLORS.Length)
                    {
                        i = 0; //loop around
                    }
                    Item toGive = ItemDict.GetItemByName(currentHairstyle + " (" + Util.HAIR_COLORS[i].name + ")");
                    player.SetHair(new ItemStack(toGive, 1));
                    if (player.GetFacialHair().GetItem() != ItemDict.CLOTHING_NONE)
                    {
                        toGive = ItemDict.GetItemByName(currentFacialHair + " (" + Util.HAIR_COLORS[i].name + ")");
                        player.SetFacialHair(new ItemStack(toGive, 1));
                    }
                    break;
                }
            }
        }
    }
}
