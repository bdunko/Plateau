using Microsoft.Xna.Framework.Audio;
using Plateau.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plateau.Sound
{
    public class SoundSet
    {
        public enum SoundType
        {
            FX, LOOP, SONG
        }

        private SoundType soundType;
        private SoundEffect[] sounds;

        public SoundSet(SoundType type, params SoundEffect[] sounds) {
            this.soundType = type;
            this.sounds = sounds;
        }

        public SoundType GetSoundType()
        {
            return this.soundType;
        }

        public SoundEffect GetSound()
        {
            return sounds[Util.RandInt(0, sounds.Count() - 1)];
        }
    }
}
