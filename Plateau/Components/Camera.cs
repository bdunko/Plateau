using Microsoft.Xna.Framework;
using MonoGame.Extended;
using Plateau.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plateau.Components
{
    public class Camera
    {
        private OrthographicCamera camera;
        private bool isLocked;

        public Camera(OrthographicCamera camera)
        {
            this.camera = camera;
            this.isLocked = false;
            camera.LookAt(new Vector2(0,0));
        }

        public void Lock()
        {
            this.isLocked = true;
        }

        public void Unlock()
        {
            this.isLocked = false;
        }

        public void SetZoom(float zoomLevel)
        {
            camera.Zoom = zoomLevel;
        }

        public Matrix GetViewMatrix()
        {
            return camera.GetViewMatrix();
        }

        public RectangleF GetBoundingBox()
        {
            return camera.BoundingRectangle;
        }

        public bool IsLocked()
        {
            return isLocked;
        } 

        public void Update(float deltaTime, Vector2 targetPosition, int mapWidth, int mapHeight)
        {
            camera.LookAt(targetPosition); //point camera at player's location

            if (camera.BoundingRectangle.Right > mapWidth)
            {
                camera.Move(new Vector2(mapWidth - camera.BoundingRectangle.Right, 0));
            }
            if (camera.BoundingRectangle.Left < 0)
            {
                camera.Move(new Vector2(-camera.BoundingRectangle.Left, 0));
            }
            if (camera.BoundingRectangle.Top < 0)
            {
                camera.Move(new Vector2(0, -camera.BoundingRectangle.Top));
            }
            if (camera.BoundingRectangle.Bottom > mapHeight)
            {
                camera.Move(new Vector2(0, mapHeight - camera.BoundingRectangle.Bottom));
            }
        }

        public void Update(float deltaTime, EntityPlayer player, int mapWidth, int mapHeight)
        {
            Vector2 targetPosition = player.GetAdjustedPosition();
            if(player.GetGravityState() == EntityPlayer.GravityState.REVERSED)
            {
                targetPosition.Y += EntityPlayer.OFFSET_Y;
            }
            this.Update(deltaTime, targetPosition, mapWidth, mapHeight);
        }

    }
}
