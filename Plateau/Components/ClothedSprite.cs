using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Plateau.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plateau.Components
{
    public class ClothedSprite
    {
        public static string STAND_CYCLE_L = "standL";
        public static string STAND_CYCLE_R = "standR";
        public static string WALK_CYCLE_L = "walkL";
        public static string WALK_CYCLE_R = "walkR";
        public static string IDLE_CYCLE_L = "idleL";
        public static string IDLE_CYCLE_R = "idleR";
        public static string JUMP_ANIM_L = "jumpL";
        public static string JUMP_ANIM_R = "jumpR";
        public static string FALLING_ANIM_L = "fallingL";
        public static string FALLING_ANIM_R = "fallingR";
        public static string GLIDE_START_ANIM_L = "glide_startL";
        public static string GLIDE_START_ANIM_R = "glide_startR";
        public static string GLIDE_CYCLE_L = "glideL";
        public static string GLIDE_CYCLE_R = "glideR";
        public static string WALL_GRAB_ANIM_L = "wall_grabL";
        public static string WALL_GRAB_ANIM_R = "wall_grabR";
        public static string LANDING_ANIM_L = "landingL";
        public static string LANDING_ANIM_R = "landingR";
        public static string ROLLING_CYCLE_L = "rollL";
        public static string ROLLING_CYCLE_R = "rollR";
        public static string GROUND_POUND_L = "gpL";
        public static string GROUND_POUND_R = "gpR";
        public static string HOE_L = "hoeL";
        public static string HOE_R = "hoeR";
        public static string WATER_L = "waterL";
        public static string WATER_R = "waterR";
        public static string AXE_L = "axeL";
        public static string AXE_R = "axeR";
        public static string PICKAXE_L = "pickL";
        public static string PICKAXE_R = "pickR";
        public static string FISH_L = "fishL";
        public static string FISH_R = "fishR";

        private static Texture2D NONE_SPRITESHEET;

        private AnimatedSprite hat, shirt, outerwear, pants, socks, shoes, gloves, earrings, scarf, glasses, back, sailcloth;
        private AnimatedSprite skin, hair, eyes, facialhair;
        private bool drawPantsOverShoes, hideHair, hideFacialHair;

        private AnimatedSprite[] sprites;

        public ClothedSprite(bool npc = false)
        {
            drawPantsOverShoes = false;
            hideHair = false;
            hideFacialHair = false;
            NONE_SPRITESHEET = PlateauMain.CONTENT.Load<Texture2D>(Paths.CLOTHING_NONE_SPRITESHEET);
            float[] frameLengths = Util.CreateAndFillArray(108, 0.1F);
            frameLengths[0] = 1.0f; //idle
            frameLengths[1] = 0.2f; //idle
            frameLengths[2] = 1.0f; //idle
            frameLengths[3] = 0.2f; //idle

            if(npc) //longer length between bends for NPC
            {
                frameLengths[0] = 3.0f; //idle
                frameLengths[1] = 0.2f; //idle
                frameLengths[2] = 3.0f; //idle
                frameLengths[3] = 0.2f; //idle
            }

            for (int i = 21; i <= 25; i++)
            {
                frameLengths[i] = 0.2f; //glide
            }
            for (int i = 27; i <= 31; i++)
            {
                frameLengths[i] = 0.2f; //glide
            }
            //hoer/hoel
            frameLengths[51] = 0.05f;
            frameLengths[52] = 0.05f;
            frameLengths[58] = 0.05f;
            frameLengths[59] = 0.05f;
            //axer/axel
            frameLengths[79] = 0.05f;
            frameLengths[80] = 0.05f;
            frameLengths[86] = 0.05f;
            frameLengths[87] = 0.05f;
            //pickr/pickl
            frameLengths[65] = 0.05f;
            frameLengths[66] = 0.05f;
            frameLengths[72] = 0.05f;
            frameLengths[73] = 0.05f;
            //fishing rod
            frameLengths[92] = 0.05f;
            frameLengths[93] = 0.05f;
            frameLengths[94] = 0.05f;
            frameLengths[95] = 0.0f;
            frameLengths[98] = 0.05f;
            frameLengths[99] = 0.05f;
            frameLengths[100] = 0.05f;
            frameLengths[101] = 0.0f;
            //watering cans
            frameLengths[102] = 0.15f;
            frameLengths[103] = 0.30f;
            frameLengths[105] = 0.15f;
            frameLengths[106] = 0.30f;
            hat = new AnimatedSprite(NONE_SPRITESHEET, 108, 11, 10, frameLengths);
            AddLoopsToSprite(hat);
            shirt = new AnimatedSprite(NONE_SPRITESHEET, 108, 11, 10, frameLengths);
            AddLoopsToSprite(shirt);
            outerwear = new AnimatedSprite(NONE_SPRITESHEET, 108, 11, 10, frameLengths);
            AddLoopsToSprite(outerwear);
            pants = new AnimatedSprite(NONE_SPRITESHEET, 108, 11, 10, frameLengths);
            AddLoopsToSprite(pants);
            socks = new AnimatedSprite(NONE_SPRITESHEET, 108, 11, 10, frameLengths);
            AddLoopsToSprite(socks);
            shoes = new AnimatedSprite(NONE_SPRITESHEET, 108, 11, 10, frameLengths);
            AddLoopsToSprite(shoes);
            gloves = new AnimatedSprite(NONE_SPRITESHEET, 108, 11, 10, frameLengths);
            AddLoopsToSprite(gloves);
            earrings = new AnimatedSprite(NONE_SPRITESHEET, 108, 11, 10, frameLengths);
            AddLoopsToSprite(earrings);
            scarf = new AnimatedSprite(NONE_SPRITESHEET, 108, 11, 10, frameLengths);
            AddLoopsToSprite(scarf);
            glasses = new AnimatedSprite(NONE_SPRITESHEET, 108, 11, 10, frameLengths);
            AddLoopsToSprite(glasses);
            back = new AnimatedSprite(NONE_SPRITESHEET, 108, 11, 10, frameLengths);
            AddLoopsToSprite(back);
            sailcloth = new AnimatedSprite(NONE_SPRITESHEET, 108, 11, 10, frameLengths);
            AddLoopsToSprite(sailcloth);

            hair = new AnimatedSprite(NONE_SPRITESHEET, 108, 11, 10, frameLengths);
            AddLoopsToSprite(hair);
            skin = new AnimatedSprite(NONE_SPRITESHEET, 108, 11, 10, frameLengths);
            AddLoopsToSprite(skin);
            eyes = new AnimatedSprite(NONE_SPRITESHEET, 108, 11, 10, frameLengths);
            AddLoopsToSprite(eyes);
            facialhair = new AnimatedSprite(NONE_SPRITESHEET, 108, 11, 10, frameLengths);
            AddLoopsToSprite(facialhair);
            
            sprites = new AnimatedSprite[16];
            sprites[0] = back;
            sprites[1] = skin;
            sprites[2] = eyes;
            sprites[3] = socks;
            sprites[4] = shoes;
            sprites[5] = pants;
            sprites[6] = shirt;
            sprites[7] = outerwear;
            sprites[8] = gloves;
            sprites[9] = earrings;
            sprites[10] = hair;
            sprites[11] = glasses;
            sprites[12] = hat;
            sprites[13] = scarf;
            sprites[14] = sailcloth;
            sprites[15] = facialhair;
        }

        private void AddLoopsToSprite(AnimatedSprite sprite)
        {
            sprite.AddLoop(STAND_CYCLE_R, 0, 0, true);
            sprite.AddLoop(STAND_CYCLE_L, 2, 2, true);
            sprite.AddLoop(IDLE_CYCLE_L, 2, 3, true);
            sprite.AddLoop(IDLE_CYCLE_R, 0, 1, true);
            sprite.AddLoop(WALK_CYCLE_R, 4, 11, true);
            sprite.AddLoop(WALK_CYCLE_L, 12, 19, true);
            sprite.AddLoop(GLIDE_START_ANIM_R, 20, 20, false);
            sprite.AddLoop(GLIDE_CYCLE_R, 21, 25, true);
            sprite.AddLoop(GLIDE_START_ANIM_L, 26, 26, false);
            sprite.AddLoop(GLIDE_CYCLE_L, 27, 31, true);
            sprite.AddLoop(WALL_GRAB_ANIM_R, 32, 32, false);
            sprite.AddLoop(WALL_GRAB_ANIM_L, 33, 33, false);
            sprite.AddLoop(JUMP_ANIM_R, 34, 35, false);
            sprite.AddLoop(FALLING_ANIM_R, 36, 36, false);
            sprite.AddLoop(LANDING_ANIM_R, 37, 37, false);
            sprite.AddLoop(JUMP_ANIM_L, 38, 39, false);
            sprite.AddLoop(FALLING_ANIM_L, 40, 40, false);
            sprite.AddLoop(LANDING_ANIM_L, 41, 41, false);
            sprite.AddLoop(ROLLING_CYCLE_R, 42, 45, true, false);
            sprite.AddLoop(ROLLING_CYCLE_L, 42, 45, true, true);
            sprite.AddLoop(GROUND_POUND_R, 46, 46, false);
            sprite.AddLoop(GROUND_POUND_L, 47, 47, false);
            sprite.AddLoop(HOE_R, 48, 54, false);
            sprite.AddLoop(HOE_L, 55, 61, false);
            sprite.AddLoop(PICKAXE_R, 62, 68, false);
            sprite.AddLoop(PICKAXE_L, 69, 75, false);
            sprite.AddLoop(AXE_R, 76, 82, false);
            sprite.AddLoop(AXE_L, 83, 89, false);
            sprite.AddLoop(FISH_R, 90, 95, false);
            sprite.AddLoop(FISH_L, 96, 101, false);
            sprite.AddLoop(WATER_R, 102, 104, false);
            sprite.AddLoop(WATER_L, 105, 107, false);

            sprite.SetLoop(IDLE_CYCLE_L);
        }

        public int GetCurrentFrameOfLoop()
        {
            return skin.GetCurrentFrameOfLoop();
        }

        public void SetLoopIfNot(string loopName)
        {
            foreach(AnimatedSprite sprite in sprites)
            {
                sprite.SetLoopIfNot(loopName);
            }
        }

        public void SetLoop(string loopName)
        {
            foreach(AnimatedSprite sprite in sprites)
            {
                sprite.SetLoop(loopName);
            }
        }

        public void Pause()
        {
            foreach(AnimatedSprite asprite in sprites)
            {
                asprite.Pause();
            }
        }

        public void Unpause()
        {
            foreach(AnimatedSprite asprite in sprites)
            {
                asprite.Unpause();
            }
        }

        public bool IsCurrentLoop(string loopName)
        {
            return skin.IsCurrentLoop(loopName);
        }

        public bool IsCurrentLoopFinished()
        {
            return skin.IsCurrentLoopFinished();
        }

        //pass in a bool: draw pants over shoes & and bool: draw hair.
        //base these in tags of items... the itemstacks are in player class so easily accessed
        //private AnimatedSprite hat, shirt, outerwear, pants, socks, shoes, gloves, earrings, scarf, glasses, back, sailcloth;
        public void Update(float deltaTime, Texture2D hatSS, Texture2D shirtSS, Texture2D outerwearSS, Texture2D pantsSS,
            Texture2D socksSS, Texture2D shoesSS, Texture2D glovesSS, Texture2D earringsSS, Texture2D scarfSS, Texture2D glassesSS, Texture2D backSS, Texture2D sailclothSS,
            Texture2D hairSS, Texture2D skinSS, Texture2D eyesSS, Texture2D facialhairSS, bool drawPantsOverShoes, bool hideHair, bool hideFacialHair)
        {
            this.drawPantsOverShoes = drawPantsOverShoes;
            this.hideHair = hideHair;
            this.hideFacialHair = hideFacialHair;
            hair.ChangeSpritesheet(hairSS);
            skin.ChangeSpritesheet(skinSS);
            eyes.ChangeSpritesheet(eyesSS);
            hat.ChangeSpritesheet(hatSS);
            shirt.ChangeSpritesheet(shirtSS);
            outerwear.ChangeSpritesheet(outerwearSS);
            pants.ChangeSpritesheet(pantsSS);
            socks.ChangeSpritesheet(socksSS);
            shoes.ChangeSpritesheet(shoesSS);
            gloves.ChangeSpritesheet(glovesSS);
            earrings.ChangeSpritesheet(earringsSS);
            scarf.ChangeSpritesheet(scarfSS);
            glasses.ChangeSpritesheet(glassesSS);
            back.ChangeSpritesheet(backSS);
            sailcloth.ChangeSpritesheet(sailclothSS);
            facialhair.ChangeSpritesheet(facialhairSS);
            foreach(AnimatedSprite asprite in sprites)
            {
                asprite.Update(deltaTime);
            }
        }

        public void Draw(SpriteBatch sb, Vector2 position, float layerDepth, SpriteEffects effect, float scale, float opacity)
        {
            back.Draw(sb, position, Color.White * opacity, layerDepth, effect, scale);
            skin.Draw(sb, position, Color.White * opacity, layerDepth, effect, scale);
            eyes.Draw(sb, position, Color.White * opacity, layerDepth, effect, scale);
            socks.Draw(sb, position, Color.White * opacity, layerDepth, effect, scale);
            if (!drawPantsOverShoes)
            {
                pants.Draw(sb, position, Color.White * opacity, layerDepth, effect, scale);
            }
            shoes.Draw(sb, position, Color.White * opacity, layerDepth, effect, scale);
            if (drawPantsOverShoes)
            {
                pants.Draw(sb, position, Color.White * opacity, layerDepth, effect, scale);
            }
            shirt.Draw(sb, position, Color.White * opacity, layerDepth, effect, scale);
            outerwear.Draw(sb, position, Color.White * opacity, layerDepth, effect, scale);
            gloves.Draw(sb, position, Color.White * opacity, layerDepth, effect, scale);
            earrings.Draw(sb, position, Color.White * opacity, layerDepth, effect, scale);

            if (!hideHair)
            {
                hair.Draw(sb, position, Color.White * opacity, layerDepth, effect, scale); //eventually edit this...
            }
            if (!hideFacialHair)
            {
                facialhair.Draw(sb, position, Color.White * opacity, layerDepth, effect, scale); //MUST CHANGE
            }
            glasses.Draw(sb, position, Color.White * opacity, layerDepth, effect, scale);
            hat.Draw(sb, position, Color.White * opacity, layerDepth, effect, scale);
            scarf.Draw(sb, position, Color.White * opacity, layerDepth, effect, scale);
            sailcloth.Draw(sb, position, Color.White * opacity, layerDepth, effect, scale);
        }

        public void Draw(SpriteBatch sb, Vector2 position, float layerDepth, float scale)
        {
            Draw(sb, position, layerDepth, SpriteEffects.None, scale, 1.0f);
        }

        public void Draw(SpriteBatch sb, Vector2 position, float layerDepth)
        {
            Draw(sb, position, layerDepth, SpriteEffects.None, 1.0f, 1.0f);
        }        
    }
}
