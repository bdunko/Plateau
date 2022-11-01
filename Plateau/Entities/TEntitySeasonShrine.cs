using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Plateau.Components;
using Plateau.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plateau.Entities
{
    public class TEntitySeasonShrine : TileEntity, IInteract, IHaveHoveringInterface, ITickDaily
    {
        TEntityShrine springShrine, summerShrine, autumnShrine, winterShrine, currentShrine;

        public TEntitySeasonShrine(Vector2 tilePosition, AnimatedSprite sprite, TEntityShrine springShrine, TEntityShrine summerShrine, TEntityShrine autumnShrine, TEntityShrine winterShrine, Area area) : base(tilePosition, sprite.GetFrameWidth() / 8, sprite.GetFrameHeight() / 8, DrawLayer.NORMAL)
        {
            this.springShrine = springShrine;
            this.summerShrine = summerShrine;
            this.autumnShrine = autumnShrine;
            this.winterShrine = winterShrine;
            UpdateForSeason(area);
        }
        public override void Draw(SpriteBatch sb, float layerDepth)
        {
            currentShrine.Draw(sb, layerDepth);
        }

        public override void Update(float deltaTime, Area area)
        {
            currentShrine.Update(deltaTime, area);
        }

        public override bool ShouldBeSaved()
        {
            return false;
        }

        public HoveringInterface GetHoveringInterface()
        {
            return currentShrine.GetHoveringInterface();
        }

        public string GetLeftClickAction(EntityPlayer player)
        {
            return currentShrine.GetLeftClickAction(player);
        }

        public string GetLeftShiftClickAction(EntityPlayer player)
        {
            return currentShrine.GetLeftShiftClickAction(player);
        }

        public string GetRightClickAction(EntityPlayer player)
        {
            return currentShrine.GetRightClickAction(player);
        }

        public string GetRightShiftClickAction(EntityPlayer player)
        {
            return currentShrine.GetRightShiftClickAction(player);
        }

        public void InteractLeft(EntityPlayer player, Area area, World world)
        {
            currentShrine.InteractLeft(player, area, world);
        }

        public void InteractLeftShift(EntityPlayer player, Area area, World world)
        {
            currentShrine.InteractLeftShift(player, area, world);
        }

        public void InteractRight(EntityPlayer player, Area area, World world)
        {
            currentShrine.InteractRight(player, area, world);
        }

        public void InteractRightShift(EntityPlayer player, Area area, World world)
        {
            currentShrine.InteractRightShift(player, area, world);
        }

        private void UpdateForSeason(Area area)
        {
            switch (area.GetSeason())
            {
                case World.Season.SPRING:
                    currentShrine = springShrine;
                    break;
                case World.Season.SUMMER:
                    currentShrine = summerShrine;
                    break;
                case World.Season.AUTUMN:
                    currentShrine = autumnShrine;
                    break;
                case World.Season.WINTER:
                    currentShrine = winterShrine;
                    break;
                default:
                    break;
            }
        }

        public void TickDaily(World timeData, Area area, EntityPlayer player)
        {
            UpdateForSeason(area);
            springShrine.TickDaily(timeData, area, player);
            summerShrine.TickDaily(timeData, area, player);
            autumnShrine.TickDaily(timeData, area, player);
            winterShrine.TickDaily(timeData, area, player);
        }
    }
}
