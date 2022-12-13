using System;
using System.Collections.Generic;
using System.Text;

namespace Plateau.Components
{
    public class Config 
    {
        //default values; overwritten when loading if other values exist
        public static bool HIDE_CONTROLS = false;
        public static bool HIDE_GRID = false;
        public static bool HIDE_RETICLE = false;
        public static bool WINDOWED = false;
        public static int SFX_VOLUME = 5;
        public static int MUSIC_VOLUME = 5;
        public static int RESOLUTION_SCALE = 0;

        public static SaveState GenerateSave()
        {
            SaveState save = new SaveState(SaveState.Identifier.CONFIG);

            save.AddData("HideControls", HIDE_CONTROLS.ToString());
            save.AddData("HideGrid", HIDE_GRID.ToString());
            save.AddData("HideReticle", HIDE_RETICLE.ToString());
            save.AddData("Windowed", WINDOWED.ToString());
            save.AddData("SFXVolume", SFX_VOLUME.ToString());
            save.AddData("MusicVolume", MUSIC_VOLUME.ToString());
            save.AddData("ResolutionScale", RESOLUTION_SCALE.ToString());

            return save;
        }

        public static void LoadSave(SaveState save)
        {
            HIDE_CONTROLS = Boolean.Parse(save.TryGetData("HideControls", HIDE_CONTROLS.ToString()));
            HIDE_GRID = Boolean.Parse(save.TryGetData("HideGrid", HIDE_GRID.ToString()));
            HIDE_RETICLE = Boolean.Parse(save.TryGetData("HideReticle", HIDE_RETICLE.ToString()));
            WINDOWED = Boolean.Parse(save.TryGetData("Windowed", WINDOWED.ToString()));
            SFX_VOLUME = Int32.Parse(save.TryGetData("SFXVolume", SFX_VOLUME.ToString()));
            MUSIC_VOLUME = Int32.Parse(save.TryGetData("MusicVolume", MUSIC_VOLUME.ToString()));
            RESOLUTION_SCALE = Int32.Parse(save.TryGetData("ResolutionScale", RESOLUTION_SCALE.ToString()));
        }
    }
}
