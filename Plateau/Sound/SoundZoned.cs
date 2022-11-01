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
    public class SoundZoned : SoundBase
    {
        public Area.SoundZone soundZone;

        public SoundZoned(SoundEffect soundEffect, Area.SoundZone soundZone) : base(soundEffect, true)
        {
            this.soundZone = soundZone;
            StartFadeIn();
        }

        public override void Update(float deltaTime, EntityPlayer player, World world)
        {
            if (!soundZone.rect.Contains(player.GetCenteredPosition()) ||
                (!(soundZone.time == World.TimeOfDay.ALL) && world.GetTimeOfDay() != soundZone.time) ||
                (!(soundZone.season == World.Season.NONE) && world.GetCurrentArea().GetSeason() != soundZone.season)) {
                StartFadeOut();
            }
            base.Update(deltaTime, player, world);
        }
    }
}
