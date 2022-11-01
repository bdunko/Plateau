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
    public class SoundSourced : SoundBase
    {
        private static float HEARING_DISTANCE = 40; //hearing distance in px

        private Vector2 source;
        private float distanceMult;

        public SoundSourced(SoundEffect soundEffect, bool loops, Vector2 source) : base(soundEffect, loops)
        {
            this.source = source;
        }

        public override float GetVolume()
        {
            return baseVolume * distanceMult;
        }

        public override void Update(float deltaTime, EntityPlayer player, World world)
        {
            float distanceFromSource = Util.DistanceBetweenVec2(player.GetCenteredPosition(), source); //TODO
            Console.WriteLine(distanceFromSource);
            distanceMult = Math.Max(0, (HEARING_DISTANCE - distanceFromSource) / HEARING_DISTANCE);
            //find horizontal distance and use that for pan
            float horizontalDistance = source.X - player.GetCenteredPosition().X;
            Console.WriteLine(horizontalDistance);
            // TORCH TO THE LEFT = 90 - 100 = -10
            // TORCH TO THE RIGHT = 120 - 100 = 20
            // TORCH ON PLAYER = 100-100 = 0
            //potentially use Util.AdjustTowards to slightly move value towards 0; prevent extreme panning...? need to test
            soundEffectInstance.Pan = horizontalDistance / HEARING_DISTANCE;
            base.Update(deltaTime, player, world);
        }
    }
}
