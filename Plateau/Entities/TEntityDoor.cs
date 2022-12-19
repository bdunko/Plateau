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
    public class TEntityDoor : TileEntity, IInteract
    {
        public enum DoorType
        {
            NORMAL, ELEVATOR, CELLAR, CELLARB1, CELLARB2, CELLARB3, CELLARB4, CELLARB5, CELLARB6, CABLECAR
        }

        private DoorType doorType;
        private static DialogueNode ELEVATOR_LOCKED;
        private static DialogueNode CELLAR_LOCKED;
        private static DialogueNode CABLECAR_LOCKED;

        public TEntityDoor(Vector2 tilePosition, int tileWidth, int tileHeight, DoorType doorType) : base(tilePosition, tileWidth, tileHeight, DrawLayer.NORMAL)
        {
            this.doorType = doorType;
            if(ELEVATOR_LOCKED == null)
            {
                ELEVATOR_LOCKED = new DialogueNode("The elevator doors refuse to open.\nIt seems that it is not functional at the moment.", DialogueNode.PORTRAIT_SYSTEM);
            } 
            if(CELLAR_LOCKED == null)
            {
                CELLAR_LOCKED = new DialogueNode("The old cellar doors are rusted over.\nIt doesn't seem like you'll be opening them anytime soon.", DialogueNode.PORTRAIT_SYSTEM);
            }
            if(CABLECAR_LOCKED == null)
            {
                CABLECAR_LOCKED = new DialogueNode("There's note pinned to the cablecar station's door:\n\"Out of Order\"", DialogueNode.PORTRAIT_SYSTEM);
            }
        }

        public override void Draw(SpriteBatch sb)
        {
            //doesn't draw
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
            return "Enter";
        }

        public string GetRightShiftClickAction(EntityPlayer player)
        {
            return "";
        }

        public void InteractLeft(EntityPlayer player, Area area, World world)
        {

        }

        public void InteractLeftShift(EntityPlayer player, Area area, World world)
        {

        }

        public void InteractRight(EntityPlayer player, Area area, World world)
        {
            bool open = false;
            DialogueNode dialogueIfClosed = null;
            switch(doorType)
            {
                case DoorType.ELEVATOR:
                    switch(area.GetAreaEnum())
                    {
                        case Area.AreaEnum.S1:
                            open = GameState.GetFlagValue(GameState.FLAG_MOUNTAIN_STRATUM_LEVEL) >= 2;
                            dialogueIfClosed = ELEVATOR_LOCKED;
                            break;
                        case Area.AreaEnum.S2:
                            open = GameState.GetFlagValue(GameState.FLAG_MOUNTAIN_STRATUM_LEVEL) >= 3;
                            dialogueIfClosed = ELEVATOR_LOCKED;
                            break;
                        case Area.AreaEnum.S3:
                            open = GameState.GetFlagValue(GameState.FLAG_MOUNTAIN_STRATUM_LEVEL) >= 4;
                            dialogueIfClosed = ELEVATOR_LOCKED;
                            break;
                        case Area.AreaEnum.S4:
                            open = GameState.GetFlagValue(GameState.FLAG_MOUNTAIN_STRATUM_LEVEL) >= 5;
                            dialogueIfClosed = ELEVATOR_LOCKED;
                            break;
                    }
                    break;
                case DoorType.CELLAR:
                    open = GameState.GetFlagValue(GameState.FLAG_CELLAR_UPGRADE_LEVEL) >= 1;
                    dialogueIfClosed = CELLAR_LOCKED;
                    break;
                case DoorType.CABLECAR:
                    open = GameState.GetFlagValue(GameState.FLAG_MOUNTAIN_STRATUM_LEVEL) >= 1;
                    dialogueIfClosed = CABLECAR_LOCKED;
                    break;
                case DoorType.CELLARB1:
                    open = GameState.GetFlagValue(GameState.FLAG_CELLAR_UPGRADE_LEVEL) >= 2;
                    dialogueIfClosed = ELEVATOR_LOCKED;
                    break;
                case DoorType.CELLARB2:
                    open = GameState.GetFlagValue(GameState.FLAG_CELLAR_UPGRADE_LEVEL) >= 3;
                    dialogueIfClosed = ELEVATOR_LOCKED;
                    break;
                case DoorType.CELLARB3:
                    open = GameState.GetFlagValue(GameState.FLAG_CELLAR_UPGRADE_LEVEL) >= 4;
                    dialogueIfClosed = ELEVATOR_LOCKED;
                    break;
                case DoorType.CELLARB4:
                    open = GameState.GetFlagValue(GameState.FLAG_CELLAR_UPGRADE_LEVEL) >= 5;
                    dialogueIfClosed = ELEVATOR_LOCKED;
                    break;
                case DoorType.CELLARB5:
                    open = GameState.GetFlagValue(GameState.FLAG_CELLAR_UPGRADE_LEVEL) >= 6;
                    dialogueIfClosed = ELEVATOR_LOCKED;
                    break;
                case DoorType.CELLARB6:
                    open = GameState.GetFlagValue(GameState.FLAG_CELLAR_UPGRADE_LEVEL) >= 7;
                    dialogueIfClosed = ELEVATOR_LOCKED;
                    break;
                case DoorType.NORMAL:
                default:
                    open = true;
                    break;
            }

            if(open)
            {
                player.ToggleAttemptTransition();
            } else
            {
                player.SetCurrentDialogue(dialogueIfClosed);
            }
        }

        public void InteractRightShift(EntityPlayer player, Area area, World world)
        {
            InteractRight(player, area, world);
        }

        public override bool ShouldBeSaved()
        {
            return false;
        }

        public override void Update(float deltaTime, Area area)
        {
            //no update
        }
    }
}