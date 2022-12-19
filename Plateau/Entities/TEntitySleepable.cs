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
    public class TEntitySleepable : TileEntity, IInteract
    {
        protected Vector2 drawAdjustment;
        protected Texture2D texture;
        public static DialogueNode SLEEP_PROMPT_DIALOGUE;

        private static Dictionary<Area.AreaEnum, GameState.SpawnEnum> FLAG_VALUES = new Dictionary<Area.AreaEnum, GameState.SpawnEnum>() 
        {
            {Area.AreaEnum.INTERIOR, GameState.SpawnEnum.HOME},
            {Area.AreaEnum.FARM, GameState.SpawnEnum.HOME},
            {Area.AreaEnum.S1, GameState.SpawnEnum.S1TENT},
            {Area.AreaEnum.S2, GameState.SpawnEnum.S2BUNK},
            {Area.AreaEnum.S3, GameState.SpawnEnum.S3TENT},
            {Area.AreaEnum.S4, GameState.SpawnEnum.S4TENT}
        };

        public TEntitySleepable(Texture2D texture, Vector2 tilePosition, int tileWidth, int tileHeight, Vector2 drawAdjustment) : base(tilePosition, tileWidth, tileHeight, DrawLayer.NORMAL)
        {
            this.drawAdjustment = drawAdjustment;
            this.texture = texture;
            SLEEP_PROMPT_DIALOGUE = new DialogueNode("Should I go to bed?", DialogueNode.PORTRAIT_SYSTEM);
            SLEEP_PROMPT_DIALOGUE.decisionDownText = "Nevermind";
            SLEEP_PROMPT_DIALOGUE.decisionUpText = "Yes";
            SLEEP_PROMPT_DIALOGUE.decisionUpNode = new DialogueNode("", DialogueNode.PORTRAIT_SYSTEM, (player, area, world) =>
            {
                GameState.SetFlag(GameState.FLAG_SPAWN_LOCATION, (int)FLAG_VALUES[area.GetAreaEnum()]);
                world.PlayCutscene(CutsceneManager.CUTSCENE_SLEEP);
            });
        }

        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(texture, position + new Vector2(0, 1) + drawAdjustment, texture.Bounds, Color.White);
        }

        public virtual string GetLeftClickAction(EntityPlayer player)
        {
            return "";
        }

        public virtual string GetLeftShiftClickAction(EntityPlayer player)
        {
            return "";
        }

        public string GetRightClickAction(EntityPlayer player)
        {
            return "Sleep";
        }

        public virtual string GetRightShiftClickAction(EntityPlayer player)
        {
            return "";
        }

        public virtual void InteractLeft(EntityPlayer player, Area area, World world)
        {

        }

        public virtual void InteractLeftShift(EntityPlayer player, Area area, World world)
        {

        }

        public virtual void InteractRight(EntityPlayer player, Area area, World world)
        {
            player.SetCurrentDialogue(SLEEP_PROMPT_DIALOGUE);
        }

        public virtual void InteractRightShift(EntityPlayer player, Area area, World world)
        {
            
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