using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using Plateau.Components;
using Plateau.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plateau.Sound
{ 
    public class SoundSystem
    {
        public enum Sound
        {
            FX_TEST
        }

        private static Dictionary<Sound, SoundSet> SOUND_DICTIONARY;
        private static float HEARING_DISTANCE = 50.0f;
        private static List<ISound> activeSFX; //sfx
        private static Dictionary<Area.SoundZone, SoundZoned> activeZonedSounds; 
        private static SoundBase currentSong, queuedSong;

        public static void Initialize(ContentManager cm)
        {
            activeSFX = new List<ISound>();
            activeZonedSounds = new Dictionary<Area.SoundZone, SoundZoned>();
            SOUND_DICTIONARY = new Dictionary<Sound, SoundSet>();
            SOUND_DICTIONARY[Sound.FX_TEST] = new SoundSet(SoundSet.SoundType.FX, cm.Load<SoundEffect>(Paths.SOUND_FX_TEST));
            currentSong = null;
        }

        public static void PlayFX(Sound sound)
        {
            SoundBase fx = new SoundBase(SOUND_DICTIONARY[sound].GetSound(), false);
            activeSFX.Add(fx);
            fx.Play();
        }

        public static void PlaySourcedFX(Sound sound, bool loops, Vector2 source)
        {
            SoundSourced sourcedFx = new SoundSourced(SOUND_DICTIONARY[sound].GetSound(), loops, source);
            activeSFX.Add(sourcedFx);
            sourcedFx.Play();
        }

        public static void PlaySong(Sound sound)
        {
            SoundBase song = new SoundBase(SOUND_DICTIONARY[sound].GetSound(), false);
            if (currentSong == null)
            {
                currentSong = song;
                song.Play();
            } else
            {
                currentSong.StartFadeOut();
                queuedSong = song;
            }
        }

        public static void ClearSounds()
        {
            activeSFX.Clear();
        }

        public static void Update(float deltaTime, EntityPlayer player, World world)
        {
            //ADD SOUNDS FROM SOUNDZONES
            foreach(Area.SoundZone activeSz in world.GetCurrentArea().GetSoundZonesAtPosAndTimeAndSeason(player.GetCenteredPosition(), world.GetTimeOfDay(), world.GetCurrentArea().GetSeason()))
            {
                if(!activeZonedSounds.ContainsKey(activeSz))
                {
                    activeZonedSounds[activeSz] = new SoundZoned(SOUND_DICTIONARY[activeSz.sound].GetSound(), activeSz);
                }
            }

            //sfx and sourced sounds
            List<ISound> toRemove = new List<ISound>();
            for(int i = 0; i < activeSFX.Count; i++)
            {
                activeSFX[i].Update(deltaTime, player, world);
                if(activeSFX[i].IsFinished())
                {
                    toRemove.Add(activeSFX[i]);
                }
            }
            foreach(ISound soundToRemove in toRemove)
            {
                activeSFX.Remove(soundToRemove);
            }

            //zoned sounds
            List<Area.SoundZone> toRemoveZones = new List<Area.SoundZone>();
            foreach(Area.SoundZone szKey in activeZonedSounds.Keys)
            {
                activeZonedSounds[szKey].Update(deltaTime, player, world);
                if (activeZonedSounds[szKey].IsFinished())
                {
                    toRemoveZones.Add(szKey);
                }
            }
            foreach (Area.SoundZone szToRemove in toRemoveZones)
            {
                activeZonedSounds[szToRemove] = null;
            }

            //song
            if (currentSong != null)
            {
                currentSong.Update(deltaTime, player, world);
                if (currentSong.IsFinished())
                {
                    currentSong = null;
                    if (queuedSong != null)
                    {
                        currentSong = queuedSong;
                        currentSong.Play();
                        queuedSong = null;
                    }
                }
            }
        }
    }
}
