using Microsoft.Xna.Framework;
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
    public abstract class TileEntity : Entity, IPersist
    {
        protected Vector2 tilePosition;
        protected int tileWidth, tileHeight;

        public TileEntity(Vector2 tilePosition, int tileWidth, int tileHeight, DrawLayer drawLayer) : base(new Vector2(tilePosition.X*8, tilePosition.Y*8), drawLayer)
        {
            this.tilePosition = tilePosition;
            this.tileWidth = tileWidth;
            this.tileHeight = tileHeight;
        }

        public int GetTileWidth()
        {
            if(tileWidth == 0)
            {
                throw new Exception("forgot to give tilewidth in constructor?");
            }
            return tileWidth;
        }
        public int GetTileHeight()
        {
            if (tileHeight == 0)
            {
                throw new Exception("forgot to give tileheight in constructor?");
            }
            return tileHeight;
        }

        public Vector2 GetTilePosition()
        {
            return tilePosition;
        }

        public virtual SaveState GenerateSave()
        {
            SaveState save = new SaveState(SaveState.Identifier.PLACEABLE);
            save.AddData("positionX", position.X.ToString());
            save.AddData("positionY", position.Y.ToString());
            save.AddData("tileX", tilePosition.X.ToString());
            save.AddData("tileY", tilePosition.Y.ToString());
            return save;
        }

        public override RectangleF GetCollisionRectangle()
        {
            return new RectangleF(position.X, position.Y, tileWidth*8, tileHeight*8);
        }

        public virtual void LoadSave(SaveState state)
        {
            //do nothing...
        }

        public abstract bool ShouldBeSaved();
    }
}
