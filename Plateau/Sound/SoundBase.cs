using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Plateau.Components;
using Plateau.Entities;

namespace Plateau.Sound
{
    public class SoundBase : ISound
    {
        protected static float FADE_SPEED = 1.0f;
        protected float baseVolume;
        private FadeState fadeState;
        protected SoundEffectInstance soundEffectInstance;

        public SoundBase(SoundEffect soundEffect, bool loops)
        {
            this.soundEffectInstance = soundEffect.CreateInstance();
            this.fadeState = FadeState.NONE;
            this.baseVolume = 1.0f;
            this.soundEffectInstance.IsLooped = loops;
        }

        public bool IsFinished()
        {
            return soundEffectInstance.State == SoundState.Stopped || GetVolume() == 0;
        }

        public bool IsPlaying()
        {
            return soundEffectInstance.State == SoundState.Playing && GetVolume() > 0;
        }

        public void Play()
        {
            soundEffectInstance.Play();
        }

        public void StartFadeIn()
        {
            baseVolume = 0.0f;
            fadeState = FadeState.FADE_IN;
        }

        public void StartFadeOut()
        {
            fadeState = FadeState.FADE_OUT;
        }

        public virtual void Update(float deltaTime, EntityPlayer player, World world)
        {
            switch(fadeState)
            {
                case FadeState.FADE_IN:
                    baseVolume = Math.Min(1, baseVolume + (FADE_SPEED * deltaTime));
                    if(baseVolume == 1)
                    {
                        fadeState = FadeState.NONE;
                    }
                    break;
                case FadeState.FADE_OUT:
                    baseVolume = Math.Max(0, baseVolume - (FADE_SPEED * deltaTime));
                    if(baseVolume == 0)
                    {
                        fadeState = FadeState.NONE;
                    }
                    break;
            }
            soundEffectInstance.Volume = GetVolume();
        }

        public virtual float GetVolume()
        {
            return baseVolume;
        }
    }
}
