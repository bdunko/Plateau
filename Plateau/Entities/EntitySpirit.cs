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
    public class EntitySpirit : EntityCreature
    { 
        public enum Element
        {
            SUN, LEAF, WOOD, WATER
        }

        private static float SPEED = 0.3f;
        private static float JUMP_SPEED = 2.4f;
        protected DialogueNode[] dialogues;
        private bool moves;
        protected Element element;

        public EntitySpirit(AnimatedSprite sprite, Vector2 position, Element element, DialogueNode[] dialogues, bool moves) : base(sprite, position, moves ? SPEED : 0, moves ? JUMP_SPEED : 0)
        {
            this.sprite = sprite;
            this.position = position;
            this.dialogues = dialogues;
            this.moves = moves;
            this.element = element;
        }

        public override RectangleF GetCollisionRectangle()
        {
            return new RectangleF(position.X, position.Y, sprite.GetFrameWidth(), sprite.GetFrameHeight());
        }

        public override void Update(float deltaTime, Area area)
        {
            base.Update(deltaTime, area);
        }

        public override string GetLeftClickAction(EntityPlayer player)
        {
            return "";
        }

        public override string GetRightClickAction(EntityPlayer player)
        {
            return "Talk";
        }

        protected override void UpdateAnimation()
        {
            if (velocityX == 0)
            {
                sprite.SetLoopIfNot(direction == DirectionEnum.LEFT ? "idleL" : "idleR");
            } else
            {
                sprite.SetLoopIfNot(direction == DirectionEnum.LEFT ? "walkL" : "walkR");
            }
        }

        public override void InteractRight(EntityPlayer player, Area area, World world)
        {
            player.SetCurrentDialogue(dialogues[Util.RandInt(0, dialogues.Length - 1)]);
            stationary = true;
            velocityX = 0;
            TurnToFace(player);
        }

        public override void InteractLeft(EntityPlayer player, Area area, World world)
        {
            
        }

        public override void TickDaily(World world, Area area, EntityPlayer player)
        {

        }

        public override void Tick(int minutesTicked, EntityPlayer player, Area area, World world)
        {
            if(player.GetCurrentDialogue() == null)
            {
                stationary = false;
            }
        }
    }
}
