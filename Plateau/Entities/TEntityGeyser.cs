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
    class TEntityGeyser : TileEntity, ITickDaily, IInteract
    {
        private AnimatedSprite sprite;
        private static DialogueNode examineDialogue = new DialogueNode("The erupting geyser fills the room with moisture, almost as though it were raining. It seems as though you won't need to worry about watering anything planted nearby.", DialogueNode.PORTRAIT_SYSTEM);

        public TEntityGeyser(Vector2 tilePosition, AnimatedSprite sprite) : base(tilePosition, sprite.GetFrameWidth() / 8, sprite.GetFrameHeight() / 8, DrawLayer.NORMAL)
        {
            this.sprite = sprite;
        }

        public override void Draw(SpriteBatch sb, float layerDepth)
        {
            sprite.Draw(sb, this.position, Color.White, layerDepth);
        }

        public override bool ShouldBeSaved()
        {
            return false;
        }

        public void TickDaily(World timeData, Area area, EntityPlayer player)
        {
            for(int x = -20; x <= 20; x++)
            {
                for(int y = -10; y <= 10; y++)
                {
                    TileEntity got = area.GetTileEntity((int)(this.position.X/8) + x, (int)(this.position.Y/8) + y);
                    
                    if (got is TEntityFarmable)
                    {
                        TEntityFarmable toWater = (TEntityFarmable)got;
                        toWater.WaterGeyser();
                    }
                }
            }
        }
        public override void Update(float deltaTime, Area area)
        {
            sprite.Update(deltaTime);
        }

        public string GetLeftShiftClickAction(EntityPlayer player)
        {
            return "";   
        }

        public string GetRightShiftClickAction(EntityPlayer player)
        {
            return "";
        }

        public string GetLeftClickAction(EntityPlayer player)
        {
            return "";
        }

        public string GetRightClickAction(EntityPlayer player)
        {
            return "Examine";
        }

        public void InteractRight(EntityPlayer player, Area area, World world)
        {
            player.SetCurrentDialogue(examineDialogue);
        }

        public void InteractLeft(EntityPlayer player, Area area, World world)
        {
            
        }

        public void InteractRightShift(EntityPlayer player, Area area, World world)
        {
            
        }

        public void InteractLeftShift(EntityPlayer player, Area area, World world)
        {
            
        }
    }
}
