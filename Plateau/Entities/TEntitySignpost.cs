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
using Plateau.Particles;

namespace Plateau.Entities
{
    class TEntitySignpost : TileEntity, IInteract
    {
        private Texture2D texture;
        private EntityType type;
        private DialogueNode dialogue;

        public TEntitySignpost(Texture2D texture, Vector2 tilePosition, EntityType type) : base(tilePosition, texture.Width / 8, texture.Height / 8, DrawLayer.NORMAL)
        {
            this.texture = texture;
            this.type = type;
            this.position.Y += 1;
        }

        public void SetDialogueNode(DialogueNode dialogue)
        {
            this.dialogue = dialogue;
        }

        public override void Draw(SpriteBatch sb, float layerDepth)
        {
            sb.Draw(texture, position, Color.White);
        }

        public override RectangleF GetCollisionRectangle()
        {
            return new RectangleF(position, new Size2(texture.Width, texture.Height));
        }

        public string GetLeftClickAction(EntityPlayer player)
        {
            return "";
        }

        public string GetLeftShiftClickAction(EntityPlayer player)
        {
            return "";
        }

        public string GetRightClickAction(EntityPlayer player)
        {
            return "Read";
        }

        public string GetRightShiftClickAction(EntityPlayer player)
        {
            return "";
        }

        public void InteractLeft(EntityPlayer player, Area area, World world)
        {
            //nothing
        }

        public void InteractLeftShift(EntityPlayer player, Area area, World world)
        {
            //nothing
        }

        public void InteractRight(EntityPlayer player, Area area, World world)
        {
            player.SetCurrentDialogue(dialogue);
        }

        public void InteractRightShift(EntityPlayer player, Area area, World world)
        {
            //nothing
        }

        public override bool ShouldBeSaved()
        {
            return false;
        }

        public override void Update(float deltaTime, Area area)
        {
            //do nothing
        }
    }
}
