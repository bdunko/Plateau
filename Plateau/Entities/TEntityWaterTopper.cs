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
    public class TEntityWaterTopper : TileEntity
    {
        protected AnimatedSprite sprite;
        private float transparency;

        public TEntityWaterTopper(Vector2 tilePosition, AnimatedSprite sprite, float transparency, DrawLayer drawLayer) : base(tilePosition, sprite.GetFrameWidth() / 8, sprite.GetFrameHeight() / 8, drawLayer)
        {
            this.sprite = sprite;
            this.transparency = transparency;
        }

        public override void Draw(SpriteBatch sb, float layerDepth)
        {
            sprite.Draw(sb, this.position, Color.White * transparency, layerDepth);
        }

        public override bool ShouldBeSaved()
        {
            return false;
        }

        public override void Update(float deltaTime, Area area)
        {
            sprite.Update(deltaTime);
        }
    }
}
