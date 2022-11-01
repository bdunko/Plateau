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
    public class PEntityFence : PEntityDecoration
    {
        private enum FenceState
        {
            SINGLE, MIDDLE, LEFT, RIGHT
        }

        private FenceState fenceState;

        public PEntityFence(PartialRecolorSprite sprite, Vector2 tilePosition, PlaceableItem sourceItem, DrawLayer drawLayer) : base(sprite, tilePosition, sourceItem, drawLayer)
        {
            this.fenceState = FenceState.SINGLE;
        }

        public override void Draw(SpriteBatch sb, float layerDepth)
        {
            switch(fenceState)
            {
                case FenceState.SINGLE:
                    sprite.SetLoopIfNot("single");
                    break;
                case FenceState.MIDDLE:
                    sprite.SetLoopIfNot("middle");
                    break;
                case FenceState.LEFT:
                    sprite.SetLoopIfNot("left");
                    break;
                case FenceState.RIGHT:
                    sprite.SetLoopIfNot("right");
                    break;
            }
            base.Draw(sb, layerDepth);
        }

        public override void Update(float deltaTime, Area area)
        {
            bool fenceRight = area.GetTileEntity((int)this.tilePosition.X + 1, (int)this.tilePosition.Y) is PEntityFence && area.GetCollisionTypeAt((int)this.tilePosition.X, (int)this.tilePosition.Y + tileHeight) != Area.CollisionTypeEnum.AIR;
            bool fenceLeft = area.GetTileEntity((int)this.tilePosition.X - 1, (int)this.tilePosition.Y) is PEntityFence & area.GetCollisionTypeAt((int)this.tilePosition.X, (int)this.tilePosition.Y + tileHeight) != Area.CollisionTypeEnum.AIR;
            if(fenceRight && fenceLeft)
            {
                fenceState = FenceState.MIDDLE;
            } else if (fenceRight)
            {
                fenceState = FenceState.LEFT;
            } else if (fenceLeft)
            {
                fenceState = FenceState.RIGHT;
            } else
            {
                fenceState = FenceState.SINGLE;
            }
            base.Update(deltaTime, area);
        }
    }
}
