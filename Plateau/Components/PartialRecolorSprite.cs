using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plateau.Components
{
    public class PartialRecolorSprite
    {
        private AnimatedSprite recolor, nonrecolor;

        public PartialRecolorSprite(AnimatedSprite recolor, AnimatedSprite nonrecolor)
        {
            this.recolor = recolor;
            this.nonrecolor = nonrecolor;
        }

        public void Pause()
        {
            recolor.Pause();
            nonrecolor.Pause();
        }

        public void Unpause()
        {
            recolor.Unpause();
            nonrecolor.Unpause();
        }

        public void AddLoop(string loopName, int startFrame, int endFrame, bool isLooping = true, bool isReversed = false)
        {
            recolor.AddLoop(loopName, startFrame, endFrame, isLooping, isReversed);
            nonrecolor.AddLoop(loopName, startFrame, endFrame, isLooping, isReversed);
        }

        public void SetFrame(int frame)
        {
            recolor.SetFrame(frame);
            nonrecolor.SetFrame(frame);
        }

        public void SetLoopIfNot(string loopName)
        {
            recolor.SetLoopIfNot(loopName);
            nonrecolor.SetLoopIfNot(loopName);
        }

        public void SetLoop(string loopName)
        {
            recolor.SetLoop(loopName);
            nonrecolor.SetLoop(loopName);
        }

        public bool IsCurrentLoop(string loop)
        {
            return recolor.IsCurrentLoop(loop);
        }

        public bool IsCurrentLoopFinished()
        {
            return recolor.IsCurrentLoopFinished();
        }

        public int GetFrameHeight()
        {
            return recolor.GetFrameHeight();
        }

        public int GetFrameWidth()
        {
            return recolor.GetFrameWidth();
        }

        public bool IsPaused()
        {
            return recolor.IsPaused();
        }

        public void Update(float deltaTime)
        {
            recolor.Update(deltaTime);
            nonrecolor.Update(deltaTime);
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 location, Color color, float layerDepth, float scale = 1.0f)
        {
            nonrecolor.Draw(spriteBatch, location, Color.White, layerDepth, scale);
            recolor.Draw(spriteBatch, location, Color.White, layerDepth, scale);
        }

    }
}
