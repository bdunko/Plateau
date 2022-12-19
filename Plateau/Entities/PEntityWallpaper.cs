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
    public class PEntityWallpaper : PlacedEntity
    {
        private PartialRecolorSprite sprite;

        public PEntityWallpaper(PartialRecolorSprite sprite, Vector2 tilePosition, PlaceableItem sourceItem, DrawLayer drawLayer) : base(tilePosition, sourceItem, drawLayer, sprite.GetFrameWidth() / 8, sprite.GetFrameHeight() / 8)
        {
            this.sprite = sprite;
        }

        public override void Draw(SpriteBatch sb)
        {
            sprite.Draw(sb, position, Color.White);
        }

        public override RectangleF GetCollisionRectangle()
        {
            return new RectangleF(position, new Vector2(sprite.GetFrameWidth(), sprite.GetFrameHeight()));
        }

        public override void Update(float deltaTime, Area area)
        {
            sprite.Update(deltaTime);
        }
    }
}
