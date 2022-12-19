using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using MonoGame.Extended;

namespace Plateau.Components
{
    public class AnimatedSprite
    {
        private class LoopData
        {
            public int startFrame, endFrame;
            public bool isLooping;
            public bool isReversed;

            public LoopData(int sF, int eF, bool lo, bool re)
            {
                startFrame = sF;
                endFrame = eF;
                isLooping = lo;
                isReversed = re;
            }
        }

        private Dictionary<string, LoopData> loops;
        private float[] frameLengths;

        private int columns, rows;
        private string currentLoop;
        private Texture2D spriteSheet;
        private int numFrames;
        private int currentFrame;
        private int frameWidth, frameHeight;
        private float currentFrameTime;
        private bool cycleFinished;
        private bool paused;

        public AnimatedSprite(Texture2D texture, int numFrames, int rows, int columns, float[] frameLengths)
        {
            this.spriteSheet = texture;
            this.numFrames = numFrames;
            this.columns = columns;
            this.rows = rows;
            this.frameWidth = spriteSheet.Width / columns;
            this.frameHeight = spriteSheet.Height / rows;
            currentFrame = 0;
            this.currentLoop = "";
            this.loops = new Dictionary<string, LoopData>();
            this.frameLengths = frameLengths;
            this.currentFrameTime = 0;
            this.cycleFinished = false;
            this.paused = true;
        }

        public void SetFrameLengths(float[] frameLengths)
        {
            this.frameLengths = frameLengths;
        }

        public int GetCurrentFrameOfLoop()
        {
            return currentFrame - loops[currentLoop].startFrame;
        }

        public void ChangeSpritesheet(Texture2D newSpritesheet)
        {
            this.spriteSheet = newSpritesheet;
        }

        public void Pause()
        {
            paused = true;
        }

        public void Unpause()
        {
            paused = false;
        }

        public void AddLoop(string loopName, int startFrame, int endFrame, bool isLooping = true, bool isReversed = false)
        {
            this.loops[loopName] = new LoopData(startFrame, endFrame, isLooping, isReversed);
        }

        public void SetFrame(int frame)
        {
            currentFrame = frame;
            if(currentFrame >= numFrames)
            {
                throw new Exception("CurrentFrame >= TotalFrames in an animated sprite");
            }
        }

        public void SetLoopIfNot(string loopName)
        {
            if(!currentLoop.Equals(loopName))
            {
                SetLoop(loopName);
            }
        }

        public bool HasLoop(string loopName)
        {
            return loops.ContainsKey(loopName);
        }

        public void SetLoop(string loopName)
        {
            this.currentLoop = loopName;
            currentFrame = loops[loopName].startFrame;
            currentFrameTime = 0;
            cycleFinished = false;
            paused = false;
        }

        public bool IsCurrentLoop(string loop)
        {
            return currentLoop.Equals(loop);
        }

        public bool IsCurrentLoopFinished()
        {
            return cycleFinished;
        }

        public void Update(float deltaTime)
        {
            if (frameLengths.Length != 1 && !paused)
            {
                currentFrameTime += deltaTime;
                if (currentFrameTime + deltaTime > frameLengths[currentFrame])
                {
                    currentFrameTime = 0;
                    LoopData curr = loops[currentLoop];
                    if (curr.isReversed ? currentFrame <= curr.startFrame : currentFrame >= curr.endFrame) //if this is the last frame...
                    {
                        if (curr.isLooping) //and the animation loops, move back to starting frame
                        {
                            currentFrame = curr.isReversed ? curr.endFrame : curr.startFrame;
                        }
                        else //if not, stay on current frame, but mark as finished
                        {
                            cycleFinished = true;
                        }
                    }
                    else
                    {
                        currentFrame += curr.isReversed ? -1 : 1;
                    }
                }
            }
        }

        public string GetCurrentLoop()
        {
            return currentLoop;
        }

        public int GetFrameHeight()
        {
            return frameHeight;
        }

        public int GetFrameWidth()
        {
            return frameWidth;
        }

        public bool IsPaused()
        {
            return paused;
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 location, Color color, SpriteEffects spriteEffect, float scale = 1.0f)
        {
            if (currentFrame > numFrames)
            {
                throw new Exception("CurrentFrame > numFrames in AnimatedSprite Draw (did you incorrectly input numFrames in constructor?)");
            }

            int row = (int)((float)currentFrame / (float)columns);
            int column = currentFrame % columns;

            Rectangle sourceRectangle = new Rectangle(frameWidth * column, frameHeight * row, frameWidth, frameHeight);

            /*spriteBatch.Draw(spriteSheet, new Vector2(location.X + 1f, location.Y), sourceRectangle, Color.Black);
            spriteBatch.Draw(spriteSheet, new Vector2(location.X - 1f, location.Y), sourceRectangle, Color.Black);
            spriteBatch.Draw(spriteSheet, new Vector2(location.X, location.Y + 1f), sourceRectangle, Color.Black);
            spriteBatch.Draw(spriteSheet, new Vector2(location.X, location.Y - 1f), sourceRectangle, Color.Black);*/

            //spritefects flip vertically for antigrav?
            spriteBatch.Draw(spriteSheet, location, sourceRectangle, color, 0.0f, Vector2.Zero, scale, spriteEffect, 1.0f);
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 location, Color color, float scale=1.0f)
        {
            Draw(spriteBatch, location, color, SpriteEffects.None, scale);
        }

        public Texture2D GetSpritesheet()
        {
            return this.spriteSheet;
        }
    }
}
