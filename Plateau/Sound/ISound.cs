using Microsoft.Xna.Framework;
using Plateau.Components;
using Plateau.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plateau.Sound
{
    public interface ISound
    {
        void StartFadeIn();
        void StartFadeOut();
        void Play();
        bool IsPlaying();
        bool IsFinished();
        void Update(float deltaTime, EntityPlayer player, World world);
        float GetVolume();
    }
}
