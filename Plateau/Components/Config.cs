using Microsoft.Xna.Framework.Input;
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

            save.AddData("ResolutionScale", RESOLUTION_SCALE.ToString());
            save.AddData("Windowed", WINDOWED.ToString());

            save.AddData("SFXVolume", SFX_VOLUME.ToString());
            save.AddData("MusicVolume", MUSIC_VOLUME.ToString());

            save.AddData("HideControls", HIDE_CONTROLS.ToString());
            save.AddData("HideGrid", HIDE_GRID.ToString());
            save.AddData("HideReticle", HIDE_RETICLE.ToString());

            save.AddData("KeyBindLeft", KeyBinds.LEFT.ToString());
            save.AddData("KeyBindRight", KeyBinds.RIGHT.ToString());
            save.AddData("KeyBindUp", KeyBinds.UP.ToString());
            save.AddData("KeyBindDown", KeyBinds.DOWN.ToString());
            save.AddData("KeyBindInventory", KeyBinds.INVENTORY.ToString());
            save.AddData("KeyBindScrapbook", KeyBinds.SCRAPBOOK.ToString());
            save.AddData("KeyBindCrafting", KeyBinds.CRAFTING.ToString());
            save.AddData("KeyBindSettings", KeyBinds.SETTINGS.ToString());
            save.AddData("KeyBindEditmode", KeyBinds.EDITMODE.ToString());
            save.AddData("KeyBindCycleHotbar", KeyBinds.CYCLE_HOTBAR.ToString());
            save.AddData("KeyBindDiscardItem", KeyBinds.DISCARD_ITEM.ToString());

            return save;
        }

        public static void LoadSave(SaveState save)
        {
            RESOLUTION_SCALE = Int32.Parse(save.TryGetData("ResolutionScale", RESOLUTION_SCALE.ToString()));
            WINDOWED = Boolean.Parse(save.TryGetData("Windowed", WINDOWED.ToString()));

            SFX_VOLUME = Int32.Parse(save.TryGetData("SFXVolume", SFX_VOLUME.ToString()));
            MUSIC_VOLUME = Int32.Parse(save.TryGetData("MusicVolume", MUSIC_VOLUME.ToString()));

            HIDE_CONTROLS = Boolean.Parse(save.TryGetData("HideControls", HIDE_CONTROLS.ToString()));
            HIDE_GRID = Boolean.Parse(save.TryGetData("HideGrid", HIDE_GRID.ToString()));
            HIDE_RETICLE = Boolean.Parse(save.TryGetData("HideReticle", HIDE_RETICLE.ToString()));

            KeyBinds.LEFT = (Keys)Enum.Parse(typeof(Keys), save.TryGetData("KeyBindLeft", KeyBinds.LEFT.ToString()));
            KeyBinds.RIGHT = (Keys)Enum.Parse(typeof(Keys), save.TryGetData("KeyBindRight", KeyBinds.RIGHT.ToString()));
            KeyBinds.UP = (Keys)Enum.Parse(typeof(Keys), save.TryGetData("KeyBindUp", KeyBinds.UP.ToString()));
            KeyBinds.DOWN = (Keys)Enum.Parse(typeof(Keys), save.TryGetData("KeyBindDown", KeyBinds.DOWN.ToString()));
            KeyBinds.INVENTORY = (Keys)Enum.Parse(typeof(Keys), save.TryGetData("KeyBindInventory", KeyBinds.INVENTORY.ToString()));
            KeyBinds.SCRAPBOOK = (Keys)Enum.Parse(typeof(Keys), save.TryGetData("KeyBindScrapbook", KeyBinds.SCRAPBOOK.ToString()));
            KeyBinds.CRAFTING = (Keys)Enum.Parse(typeof(Keys), save.TryGetData("KeyBindCrafting", KeyBinds.CRAFTING.ToString()));
            KeyBinds.SETTINGS = (Keys)Enum.Parse(typeof(Keys), save.TryGetData("KeyBindSettings", KeyBinds.SETTINGS.ToString()));
            KeyBinds.EDITMODE = (Keys)Enum.Parse(typeof(Keys), save.TryGetData("KeyBindEditmode", KeyBinds.EDITMODE.ToString()));
            KeyBinds.CYCLE_HOTBAR = (Keys)Enum.Parse(typeof(Keys), save.TryGetData("KeyBindCycleHotbar", KeyBinds.CYCLE_HOTBAR.ToString()));
            KeyBinds.DISCARD_ITEM = (Keys)Enum.Parse(typeof(Keys), save.TryGetData("KeyBindDiscardItem", KeyBinds.DISCARD_ITEM.ToString()));

        }
    }
}
