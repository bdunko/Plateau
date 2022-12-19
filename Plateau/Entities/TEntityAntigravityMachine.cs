using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using Plateau.Components;

namespace Plateau.Entities
{
    public class TEntityAntigravityMachine : TileEntity, IInteract
    {
        AnimatedSprite sprite;
        private static bool currentlyReversed;

        private bool upsideDown;

        public TEntityAntigravityMachine(Vector2 tilePosition, AnimatedSprite sprite, bool upsideDown) : base(tilePosition, sprite.GetFrameWidth() / 8, sprite.GetFrameHeight() / 8, DrawLayer.NORMAL)
        {
            this.sprite = sprite;
            currentlyReversed = false;
            this.upsideDown = upsideDown;
        }

        public override void Draw(SpriteBatch sb)
        {
            if(currentlyReversed)
            {
                sprite.SetLoopIfNot("reversed");
            } else
            {
                sprite.SetLoopIfNot("normal");
            }
            sprite.Draw(sb, this.position, Color.White, upsideDown ? SpriteEffects.FlipVertically : SpriteEffects.None);
        }

        public override RectangleF GetCollisionRectangle()
        {
            return new RectangleF(this.position, new Vector2(sprite.GetFrameWidth(), sprite.GetFrameHeight()));
        }

        public override void Update(float deltaTime, Area area)
        {
            sprite.Update(deltaTime);
        }

        public string GetLeftClickAction(EntityPlayer player)
        {
            return "";
        }

        public string GetLeftShiftClickAction(EntityPlayer player)
        {
            return "";
        }

        public string GetRightClickAction(EntityPlayer player)
        {
            return "Activate";
        }

        public string GetRightShiftClickAction(EntityPlayer player)
        {
            return "";
        }

        public void InteractLeft(EntityPlayer player, Area area, World world)
        {
            
        }

        public void InteractLeftShift(EntityPlayer player, Area area, World world)
        {
           
        }

        public void InteractRight(EntityPlayer player, Area area, World world)
        {
            if (upsideDown)
            {
                player.SetGravityState(EntityPlayer.GravityState.NORMAL);
                currentlyReversed = false;
            }
            else
            {
                player.SetGravityState(EntityPlayer.GravityState.REVERSED);
                currentlyReversed = true;
            }
            
        }

        public void InteractRightShift(EntityPlayer player, Area area, World world)
        {

        }

        public override bool ShouldBeSaved()
        {
            return false;
        }
    }
}
