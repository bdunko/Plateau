using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using Plateau.Components;
using Plateau.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plateau.Entities
{
    public class PEntityDecoration : PlacedEntity
    {
        protected PartialRecolorSprite sprite;

        public PEntityDecoration(PartialRecolorSprite sprite, Vector2 tilePosition, PlaceableItem sourceItem, DrawLayer drawLayer) : base(tilePosition, sourceItem, drawLayer, sprite.GetFrameWidth() / 8, sprite.GetFrameHeight() / 8)
        {
            this.sprite = sprite;
            this.itemForm = sourceItem;
        }

        public override void Draw(SpriteBatch sb)
        {
            sprite.Draw(sb, new Vector2(position.X, position.Y + ((GetDrawLayer() != DrawLayer.FOREGROUND_CARPET && GetDrawLayer() != DrawLayer.BACKGROUND_WALL && GetDrawLayer() != DrawLayer.BACKGROUND_WALLPAPER)? 1 : 0)), Color.White);
        }

        public override void LoadSave(SaveState state)
        {
            //nothing to load
        }

        public override void Update(float deltaTime, Area area)
        {
            sprite.Update(deltaTime);
            if (sprite.IsCurrentLoopFinished())
            {
                sprite.SetLoop("anim");
            }
        }

        public override void OnRemove(EntityPlayer player, Area area, World world)
        {
            if(area.GetCollisionTypeAt((int)this.tilePosition.X, (int)this.tilePosition.Y) == Area.CollisionTypeEnum.SOLID)
            {
                this.position.Y -= 8;
            }
            base.OnRemove(player, area, world);
        }

        public override RectangleF GetCollisionRectangle()
        {
            return new RectangleF(position, new Size2(sprite.GetFrameWidth(), sprite.GetFrameHeight()));
        }
    }
}
