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
    public class PEntityEnchantedVanity : PlacedEntity, IInteract
    {
        private PartialRecolorSprite sprite;
        private float timeSinceAnimation;
        private static float TIME_BETWEEN_ANIMATION = 5.0f;

        private static ClothingItem[] SKIN_COLORS = {ItemDict.SKIN_SNOW, ItemDict.SKIN_ECRU, ItemDict.SKIN_PEACH, ItemDict.SKIN_OLIVE, ItemDict.SKIN_RUSSET, ItemDict.SKIN_CHOCOLATE,
            ItemDict.SKIN_ALIEN, ItemDict.SKIN_DRIFTER, ItemDict.SKIN_EXEMPLAR, ItemDict.SKIN_MERIDIAN, ItemDict.SKIN_PHANTOM, ItemDict.SKIN_MIDNIGHT};
        private static ClothingItem[] EYE_COLORS = {ItemDict.EYES_AMBER, ItemDict.EYES_BROWN, ItemDict.EYES_DOT, ItemDict.EYES_EMERALD, ItemDict.EYES_OCEAN, ItemDict.EYES_FROST, 
            ItemDict.EYES_MINT, ItemDict.EYES_SILVER, ItemDict.EYES_SOLAR, ItemDict.EYES_TEAK, ItemDict.EYES_BLUSH, ItemDict.EYES_BLANK};

        public PEntityEnchantedVanity(PartialRecolorSprite sprite, Vector2 tilePosition, PlaceableItem sourceItem, DrawLayer drawLayer) : base(tilePosition, sourceItem, drawLayer, sprite.GetFrameWidth() / 8, sprite.GetFrameHeight() / 8)
        {
            this.sprite = sprite;
            sprite.AddLoop("idle", 0, 0, true);
            sprite.AddLoop("placement", 0, 3, false);
            sprite.AddLoop("anim", 4, 7, false);
            sprite.SetLoop("placement");
            this.timeSinceAnimation = 0;
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
            if (!sprite.IsCurrentLoop("placement"))
            {
                timeSinceAnimation += deltaTime;
                if (timeSinceAnimation >= TIME_BETWEEN_ANIMATION)
                {
                    sprite.SetLoop("anim");
                    timeSinceAnimation = 0;
                }
            }
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
            return "Eye Recolor";
        }

        public string GetLeftClickAction(EntityPlayer player)
        {
            return "";
        }

        public string GetRightClickAction(EntityPlayer player)
        {
            return "Skin Recolor";
        }

        public void InteractRight(EntityPlayer player, Area area, World world)
        {
            ClothingItem currentSkinColor = (ClothingItem)player.GetSkin().GetItem();
            for(int i = 0; i < SKIN_COLORS.Length; i++)
            {
                if(currentSkinColor == SKIN_COLORS[i])
                {
                    i = i + 1;
                    if (i >= SKIN_COLORS.Length)
                    {
                        i = 0;
                    }
                    player.SetSkin(new ItemStack(SKIN_COLORS[i], 1));
                    break;
                }
            }
        }

        public void InteractLeft(EntityPlayer player, Area area, World world)
        {

        }

        public void InteractRightShift(EntityPlayer player, Area area, World world)
        {
            ClothingItem currentEyes = (ClothingItem)player.GetEyes().GetItem();
            for (int i = 0; i < EYE_COLORS.Length; i++)
            {
                if (currentEyes == EYE_COLORS[i])
                {
                    i = i + 1;
                    if (i >= EYE_COLORS.Length)
                    {
                        i = 0;
                    }
                    player.SetEyes(new ItemStack(EYE_COLORS[i], 1));
                    break;
                }
            }
        }

        public void InteractLeftShift(EntityPlayer player, Area area, World world)
        {

        }
    }
}
