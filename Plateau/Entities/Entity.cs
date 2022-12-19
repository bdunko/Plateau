using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using Plateau.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plateau.Entities
{
    public enum DrawLayer
    {
        FOREGROUND_CARPET, FOREGROUND, PRIORITY, NORMAL, BACKGROUND_WALL, BACKGROUND_WALLPAPER, BACKGROUND_BEHIND_WALL
    }

    public abstract class Entity
    {
        protected Vector2 position;
        private DrawLayer drawLayer;

        public Entity(Vector2 position, DrawLayer drawLayer)
        {
            this.position = position;
            this.drawLayer = drawLayer;
        }

        public DrawLayer GetDrawLayer()
        {
            return drawLayer;
        }

        public virtual void SetPosition(Vector2 position)
        {
            this.position = position;
        }
        public Vector2 GetPosition()
        {
            return this.position;
        }

        public abstract RectangleF GetCollisionRectangle();
        public abstract void Draw(SpriteBatch sb);
        public abstract void Update(float deltaTime, Area area);
    }
}
