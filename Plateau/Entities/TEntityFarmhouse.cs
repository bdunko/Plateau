using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using Plateau.Components;
using Plateau.Items;

namespace Plateau.Entities
{
    public class TEntityFarmhouse : TileEntity, IInteract, IPersist
    {
        public static TEntityFarmhouse FARMHOUSE;
        private Item wallsDye, roofDye, trimDye;
        public static int WIDTH = 5;
        public static int HEIGHT = 3;
        private static int DRAW_X_ADJUSTMENT = -32;
        private static int DRAW_Y_ADJUSTMENT = -63;
        private static string TENT_LOOP = "tent";
        private static string CABIN_LOOP = "cabin";
        private static string HOUSE_LOOP = "house";
        private static string MANSION_LOOP = "mansion";
        private DialogueNode recolorDialogue, recolorTentDialogue;

        private Vector2 drawPosition;
        private Texture2D wallsSS, roofSS, trimSS;
        private AnimatedSprite baseSprite, wallsSprite, roofSprite, trimSprite;

        public TEntityFarmhouse(Vector2 tilePosition, Texture2D baseSS, Texture2D wallsSS, Texture2D roofSS, Texture2D trimSS) : base(tilePosition, 5, 3, DrawLayer.NORMAL)
        {
            this.drawPosition = new Vector2(position.X + DRAW_X_ADJUSTMENT, position.Y + DRAW_Y_ADJUSTMENT);
            this.wallsSS = wallsSS;
            this.roofSS = roofSS;
            this.trimSS = trimSS;
            this.roofDye = ItemDict.NONE;
            this.wallsDye = ItemDict.NONE;
            this.trimDye = ItemDict.NONE;
            baseSprite = GenerateAnimatedSprite(baseSS);
            FARMHOUSE = this;
            recolorTentDialogue = new DialogueNode("Should I paint my tent?", DialogueNode.PORTRAIT_SYSTEM);
            recolorTentDialogue.decisionUpText = "Yeah!";
            recolorTentDialogue.decisionUpNode = new DialogueNode("Looking good!", DialogueNode.PORTRAIT_SYSTEM, (player, currentArea, world) =>
            {
                TEntityFarmhouse house = (TEntityFarmhouse)player.GetTargettedTileEntity();
                house.DyeWalls(player.GetHeldItem().GetItem());
                player.GetHeldItem().Subtract(1);
            });
            recolorTentDialogue.decisionDownText = "Nevermind";

            recolorDialogue = new DialogueNode("What part of the house should I paint?", DialogueNode.PORTRAIT_SYSTEM);
            recolorDialogue.decisionRightText = "Roof/Accent";
            recolorDialogue.decisionRightNode = new DialogueNode("Looking good!", DialogueNode.PORTRAIT_SYSTEM, (player, currentArea, world) =>
            {
                TEntityFarmhouse house = (TEntityFarmhouse)player.GetTargettedTileEntity();
                house.DyeRoof(player.GetHeldItem().GetItem());
                player.GetHeldItem().Subtract(1);
            });
            recolorDialogue.decisionUpText = "Walls";
            recolorDialogue.decisionUpNode = new DialogueNode("Looking good!", DialogueNode.PORTRAIT_SYSTEM, (player, currentArea, world) =>
            {
                TEntityFarmhouse house = (TEntityFarmhouse)player.GetTargettedTileEntity();
                house.DyeWalls(player.GetHeldItem().GetItem());
                player.GetHeldItem().Subtract(1);
            });
            recolorDialogue.decisionLeftText = "Trim";
            recolorDialogue.decisionLeftNode = new DialogueNode("Looking good!", DialogueNode.PORTRAIT_SYSTEM, (player, currentArea, world) =>
            {
                TEntityFarmhouse house = (TEntityFarmhouse)player.GetTargettedTileEntity();
                house.DyeTrim(player.GetHeldItem().GetItem());
                player.GetHeldItem().Subtract(1);
            });
            recolorDialogue.decisionDownText = "Nevermind";
        }

        public void DyeWalls(Item dye)
        {
            wallsDye = dye;
            NotifyRecolor();
        }

        public void DyeTrim(Item dye)
        {
            trimDye = dye;
            NotifyRecolor();
        }

        public void DyeRoof(Item dye)
        {
            roofDye = dye;
            NotifyRecolor();
        }

        private void UpdateSpriteLoop(AnimatedSprite sprite)
        {
            if (sprite != null)
            {
                switch (GameState.GetFlagValue(GameState.FLAG_HOUSE_UPGRADE_LEVEL))
                {
                    case 0:
                        sprite.SetLoopIfNot(TENT_LOOP);
                        break;
                    case 1:
                        sprite.SetLoopIfNot(CABIN_LOOP);
                        break;
                    case 2:
                        sprite.SetLoopIfNot(HOUSE_LOOP);
                        break;
                    case 3:
                    default:
                        sprite.SetLoopIfNot(MANSION_LOOP);
                        break;
                }
            }
        }

        private AnimatedSprite GenerateAnimatedSprite(Texture2D spritesheet)
        {
            AnimatedSprite sprite = new AnimatedSprite(spritesheet, 4, 1, 4, Util.CreateAndFillArray(4, 1000.0f));
            sprite.AddLoop(TENT_LOOP, 0, 0, true);
            sprite.AddLoop(CABIN_LOOP, 1, 1, true);
            sprite.AddLoop(HOUSE_LOOP, 2, 2, true);
            sprite.AddLoop(MANSION_LOOP, 3, 3, true);
            UpdateSpriteLoop(sprite);

            return sprite;
        }

        public string GetLeftClickAction(EntityPlayer player)
        {
            if (player.GetHeldItem().GetItem().HasTag(Item.Tag.DYE))
            {
                return "Paint";
            }
            return "";
        }

        public void NotifyRecolor()
        {
            if(roofDye == ItemDict.UN_DYE || roofDye == ItemDict.NONE)
            {
                roofDye = ItemDict.NONE;
                roofSprite = null;
            } else
            {
                Texture2D recoloredRoof = Util.GenerateRecolor(roofSS, ((DyeItem)roofDye).GetHouseRecolorMap(), Util.RecolorAdjustment.SLIGHT_LIGHTEN);
                roofSprite = GenerateAnimatedSprite(recoloredRoof);
            }

            if (wallsDye == ItemDict.UN_DYE || wallsDye == ItemDict.NONE)
            {
                wallsDye = ItemDict.NONE;
                wallsSprite = null;
            }
            else
            {
                Texture2D recoloredWalls = Util.GenerateRecolor(wallsSS, ((DyeItem)wallsDye).GetHouseRecolorMap());
                wallsSprite = GenerateAnimatedSprite(recoloredWalls);
            }

            if (trimDye == ItemDict.UN_DYE || trimDye == ItemDict.NONE)
            {
                trimDye = ItemDict.NONE;
                trimSprite = null;
            }
            else
            {
                Texture2D recoloredTrim = Util.GenerateRecolor(trimSS, ((DyeItem)trimDye).GetHouseRecolorMap(), Util.RecolorAdjustment.EXTRA_DARKEN);
                trimSprite = GenerateAnimatedSprite(recoloredTrim);
            }
        }

        public string GetRightClickAction(EntityPlayer player)
        {
            if (GameState.GetFlagValue(GameState.FLAG_HOUSE_UPGRADE_LEVEL) == 0)
            {
                return "Sleep";
            } else {
                return "Enter";
            }
        }

        public override SaveState GenerateSave()
        {
            SaveState save = base.GenerateSave();
            save.AddData("entitytype", EntityType.FARMHOUSE.ToString());
            save.AddData("wallsDye", wallsDye.GetName());
            save.AddData("roofDye", roofDye.GetName());
            save.AddData("trimDye", trimDye.GetName());
            return save;
        }

        public override void LoadSave(SaveState state)
        {
            wallsDye = ItemDict.GetItemByName(state.TryGetData("wallsDye", ItemDict.NONE.GetName()));
            roofDye = ItemDict.GetItemByName(state.TryGetData("roofDye", ItemDict.NONE.GetName()));
            trimDye = ItemDict.GetItemByName(state.TryGetData("trimDye", ItemDict.NONE.GetName()));
            NotifyRecolor();
        }

        public void InteractRight(EntityPlayer player, Area area, World world)
        {
            int houseUpgradeLevel = GameState.GetFlagValue(GameState.FLAG_HOUSE_UPGRADE_LEVEL);
            if (houseUpgradeLevel == 0)
            {
                player.SetCurrentDialogue(TEntitySleepable.SLEEP_PROMPT_DIALOGUE);
                return;
            }

            switch (houseUpgradeLevel)
            {
                case 1:
                    player.TransitionTo("INTERIOR", "SPfarmhouseCabin", Area.TransitionZone.Animation.TO_LEFT);
                    break;
                case 2:
                    player.TransitionTo("INTERIOR", "SPfarmhouseHouse", Area.TransitionZone.Animation.TO_LEFT);
                    break;
                case 3:
                default:
                    player.TransitionTo("INTERIOR", "SPfarmhouseMansion", Area.TransitionZone.Animation.TO_LEFT);
                    break;
            }
            player.ToggleAttemptTransition();

        }

        public void InteractLeft(EntityPlayer player, Area area, World world)
        {
            if(player.GetHeldItem().GetItem().HasTag(Item.Tag.DYE))
            {
                player.SetCurrentDialogue(GameState.GetFlagValue(GameState.FLAG_HOUSE_UPGRADE_LEVEL) == 0 ? recolorTentDialogue : recolorDialogue);
            }
        }

        public string GetLeftShiftClickAction(EntityPlayer player)
        {
            return "";
        }

        public string GetRightShiftClickAction(EntityPlayer player)
        {
            return "DEBUG";
        }

        public void InteractRightShift(EntityPlayer player, Area area, World world)
        {
            //debug
            GameState.SetFlag(GameState.FLAG_HOUSE_UPGRADE_LEVEL, GameState.GetFlagValue(GameState.FLAG_HOUSE_UPGRADE_LEVEL) + 1);
            if(GameState.GetFlagValue(GameState.FLAG_HOUSE_UPGRADE_LEVEL) >= 4)
            {
                GameState.SetFlag(GameState.FLAG_HOUSE_UPGRADE_LEVEL, 0);
            }
        }

        public void InteractLeftShift(EntityPlayer player, Area area, World world)
        {
            
        }

        public override bool ShouldBeSaved()
        {
            return true;
        }

        public override void Draw(SpriteBatch sb, float layerDepth)
        {
            baseSprite.Draw(sb, drawPosition, Color.White, layerDepth);
            if(roofSprite != null)
            {
                roofSprite.Draw(sb, drawPosition, Color.White, layerDepth);
            }
            if(wallsSprite != null)
            {
                wallsSprite.Draw(sb, drawPosition, Color.White, layerDepth);
            }
            if(trimSprite != null)
            {
                trimSprite.Draw(sb, drawPosition, Color.White, layerDepth);
            }
        }

        public override void Update(float deltaTime, Area area)
        {
            baseSprite.Update(deltaTime);
            UpdateSpriteLoop(baseSprite);
            UpdateSpriteLoop(wallsSprite);
            UpdateSpriteLoop(trimSprite);
            UpdateSpriteLoop(roofSprite);
        }
    }
}
