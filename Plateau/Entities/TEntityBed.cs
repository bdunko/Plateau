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
    public class TEntityBed : TEntitySleepable
    {
        public static int HEIGHT = 2;
        private static DialogueNode RECOLOR_DIALOGUE;
        private Texture2D sheetsGreyscale;
        private Texture2D sheetsRecolored;
        public Item sheetsDye;

        public TEntityBed(Texture2D texture, Texture2D sheetsGreyscale, Vector2 tilePosition, int tileWidth, int tileHeight, Vector2 drawAdjustment) : base(texture, tilePosition, tileWidth, tileHeight, drawAdjustment)
        {
            this.sheetsGreyscale = sheetsGreyscale;
            sheetsRecolored = null;
            sheetsDye = ItemDict.NONE;

            if (RECOLOR_DIALOGUE == null)
            {
                RECOLOR_DIALOGUE = new DialogueNode("Should I dye my bedsheets?", DialogueNode.PORTRAIT_SYSTEM);
                RECOLOR_DIALOGUE.decisionUpText = "Yeah!";
                RECOLOR_DIALOGUE.decisionUpNode = new DialogueNode("Looking good!", DialogueNode.PORTRAIT_SYSTEM, (player, currentArea, world) =>
                {
                    TEntityBed bed = (TEntityBed)player.GetTargettedTileEntity();
                    bed.Dye(player.GetHeldItem().GetItem());
                    player.GetHeldItem().Subtract(1);

                });
                RECOLOR_DIALOGUE.decisionDownText = "Nevermind";
            }
        }

        public override void Draw(SpriteBatch sb, float layerDepth)
        {
            sb.Draw(texture, position + new Vector2(0, 1) + drawAdjustment, texture.Bounds, Color.White, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, layerDepth);
            if(sheetsRecolored != null)
            {
                sb.Draw(sheetsRecolored, position + new Vector2(0, 1) + drawAdjustment, texture.Bounds, Color.White, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, layerDepth);
            }
        }

        public void NotifyRecolor()
        {
            if (sheetsDye == ItemDict.UN_DYE || sheetsDye == ItemDict.NONE)
            {
                sheetsDye = ItemDict.NONE;
                sheetsRecolored = null;
            }
            else
            {
                sheetsRecolored = Util.GenerateRecolor(sheetsGreyscale, ((DyeItem)sheetsDye).GetRecolorMap());
            }
        }

        public override string GetLeftClickAction(EntityPlayer player)
        {
            if (player.GetHeldItem().GetItem().HasTag(Item.Tag.DYE))
            {
                return "Dye";
            }
            return "";
        }

        public override void InteractLeft(EntityPlayer player, Area area, World world)
        {
            if (player.GetHeldItem().GetItem().HasTag(Item.Tag.DYE))
            {
                player.SetCurrentDialogue(RECOLOR_DIALOGUE);
            }
        }

        public void Dye(Item dye)
        {
            sheetsDye = dye;
            NotifyRecolor();
        }

        public override SaveState GenerateSave()
        {
            SaveState save = base.GenerateSave();
            save.AddData("entitytype", EntityType.BED.ToString());
            save.AddData("sheetsDye", sheetsDye.GetName());
            return save;
        }

        public override void LoadSave(SaveState state)
        {
            sheetsDye = ItemDict.GetItemByName(state.TryGetData("sheetsDye", ItemDict.NONE.GetName()));
            NotifyRecolor();
        }

        public override bool ShouldBeSaved()
        {
            return true;
        }

        public override void Update(float deltaTime, Area area)
        {
            //no update
        }
    }
}