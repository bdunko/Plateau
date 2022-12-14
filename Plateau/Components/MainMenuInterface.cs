using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using Plateau.Entities;
using System;

namespace Plateau.Components
{
    public class MainMenuInterface
    {
        public enum MainMenuState {
            NONE, CLICKED_SAVE_1, CLICKED_SAVE_2, CLICKED_SAVE_3, SETTINGS
        }
        private static Rectangle[] SAVE_BUTTONS = { new Rectangle(54, 138, 58, 39), new Rectangle(131, 138, 58, 39), new Rectangle(208, 138, 58, 39)};
        private static Rectangle SETTINGS_BUTTON = new Rectangle(298, 158, 18, 18);
        private static Rectangle QUIT_BUTTON = new Rectangle(298, 4, 18, 18);

        private static int NEW_SAVE_X_OFFSET = 5;
        private static int NEW_SAVE_Y_OFFSET = 13;
        bool file1Exists, file2Exists, file3Exists;
        private static Vector2 LOGO_POSITION = new Vector2(88, 10);

        private LayeredBackground background;
        private Texture2D logo, saveSlot, settingsIcon, newSaveIcon, saveSlotEnlarge;
        private Texture2D settingsIconEnlarge, settingsIconSelected;
        private Texture2D quitIcon, quitIconEnlarge;
        private Controller controller;
        private MainMenuState state;
        private World.TimeData fakeTimeData;
        private World.Weather fakeWeather;
        private bool hoveringSave1, hoveringSave2, hoveringSave3, hoveringSettings, hoveringQuit;
        private bool settingsOpen;

        public MainMenuInterface(Controller controller, bool file1Exists, bool file2Exists, bool file3Exists)
        {
            this.controller = controller;
            this.state = MainMenuState.NONE;
            this.file1Exists = file1Exists;
            this.file2Exists = file2Exists;
            this.file3Exists = file3Exists;
            this.settingsOpen = false;

            World.Season systemSeason = World.Season.NONE;
            switch (DateTime.Now.Month)
            {
                case 3:
                case 4:
                case 5:
                    systemSeason = World.Season.SPRING;
                    break;
                case 6:
                case 7:
                case 8:
                    systemSeason = World.Season.SUMMER;
                    break;
                case 9:
                case 10:
                case 11:
                    systemSeason = World.Season.AUTUMN;
                    break;
                case 12:
                case 1:
                case 2:
                    systemSeason = World.Season.WINTER;
                    break;
            }

            this.fakeTimeData = new World.TimeData(systemSeason, 1, DateTime.Now.Hour, 0, World.GetTimeOfDay(DateTime.Now.Hour));

            this.fakeWeather = World.Weather.SUNNY;
            if(Util.RandInt(1, 6) == 1)
            {
                this.fakeWeather = fakeTimeData.season == World.Season.WINTER ? World.Weather.SNOWY : World.Weather.RAINY;
            }
            else if(Util.RandInt(1, 2) == 1) {
                this.fakeWeather = World.Weather.CLOUDY;
            } 
        }

        public void Update(float deltaTime, RectangleF cameraBoundingBox, EntityPlayer player)
        {
            //state = MainMenuState.CLICKED_SAVE_1; //SKIP
            background.Update(deltaTime, cameraBoundingBox, fakeTimeData, fakeWeather, fakeTimeData.season);
            Vector2 mousePosition = controller.GetMousePos();
            hoveringSave1 = false;
            hoveringSave2 = false;
            hoveringSave3 = false;
            hoveringSettings = false;
            hoveringQuit = false;

            for (int i = 0; i < SAVE_BUTTONS.Length; i++)
            {
                if (SAVE_BUTTONS[i].Contains(mousePosition))
                {
                    switch (i)
                    {
                        case 0:
                            if (controller.GetMouseLeftPress())
                            {
                                state = MainMenuState.CLICKED_SAVE_1;
                            }
                            hoveringSave1 = true;
                            break;
                        case 1:
                            if (controller.GetMouseLeftPress())
                            {
                                state = MainMenuState.CLICKED_SAVE_2;
                            }
                            hoveringSave2 = true;
                            break;
                        case 2:
                            if (controller.GetMouseLeftPress())
                            {
                                state = MainMenuState.CLICKED_SAVE_3;
                            }
                            hoveringSave3 = true;
                            break;
                    }
                }

                if(SETTINGS_BUTTON.Contains(mousePosition))
                {
                    hoveringSettings = true;
                    if (controller.GetMouseLeftPress())
                    {
                        settingsOpen = !settingsOpen;
                        if (settingsOpen)
                        {
                            player.SetInterfaceState(InterfaceState.SETTINGS);
                            state = MainMenuState.SETTINGS;
                        }
                        else
                        {
                            player.SetInterfaceState(InterfaceState.NONE);
                            state = MainMenuState.NONE;
                        }
                    }
                }

                if(QUIT_BUTTON.Contains(mousePosition))
                {
                    hoveringQuit = true;
                    if (controller.GetMouseLeftPress())
                    {
                        player.SetInterfaceState(InterfaceState.EXIT_CONFIRMED);
                    }
                }
            }
        }

        public void LoadContent(ContentManager content, RectangleF cameraBoundingBox)
        {
            background = new LayeredBackground(content, cameraBoundingBox, new LayeredBackground.BackgroundParams(LayeredBackground.BackgroundParams.Type.BACKGROUND_SKY, true, true));
            newSaveIcon = content.Load<Texture2D>(Paths.INTERFACE_MAINMENU_NEWGAME);
            logo = content.Load<Texture2D>(Paths.INTERFACE_MAINMENU_LOGO);
            saveSlot = content.Load<Texture2D>(Paths.INTERFACE_MAINMENU_SAVESLOT);
            saveSlotEnlarge = content.Load<Texture2D>(Paths.INTERFACE_MAINMENU_SAVESLOT_ENLARGE);
            settingsIcon = content.Load<Texture2D>(Paths.INTERFACE_MAINMENU_SETTINGS);
            settingsIconEnlarge = content.Load<Texture2D>(Paths.INTERFACE_MAINMENU_SETTINGS_ENLARGE);
            settingsIconSelected = content.Load<Texture2D>(Paths.INTERFACE_MAINMENU_SETTINGS_SELECTED);
            quitIcon = content.Load<Texture2D>(Paths.INTERFACE_MAINMENU_QUIT);
            quitIconEnlarge = content.Load<Texture2D>(Paths.INTERFACE_MAINMENU_QUIT_ENLARGE);
        }

        public void Draw(SpriteBatch sb, RectangleF cameraBoundingBox)
        {
            background.Draw(sb, cameraBoundingBox, 1.0f, 1.0f);

            if (state != MainMenuState.SETTINGS)
            {
                sb.Draw(logo, Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, LOGO_POSITION), Color.White);
                sb.Draw(hoveringSave1 ? saveSlotEnlarge : saveSlot, Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, new Vector2(SAVE_BUTTONS[0].X, SAVE_BUTTONS[0].Y)), Color.White);
                sb.Draw(hoveringSave2 ? saveSlotEnlarge : saveSlot, Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, new Vector2(SAVE_BUTTONS[1].X, SAVE_BUTTONS[1].Y)), Color.White);
                sb.Draw(hoveringSave3 ? saveSlotEnlarge : saveSlot, Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, new Vector2(SAVE_BUTTONS[2].X, SAVE_BUTTONS[2].Y)), Color.White);
                if (!file1Exists)
                {
                    sb.Draw(newSaveIcon, Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, new Vector2(SAVE_BUTTONS[0].X + NEW_SAVE_X_OFFSET, SAVE_BUTTONS[0].Y + NEW_SAVE_Y_OFFSET)), Color.White);
                }
                if (!file2Exists)
                {
                    sb.Draw(newSaveIcon, Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, new Vector2(SAVE_BUTTONS[1].X + NEW_SAVE_X_OFFSET, SAVE_BUTTONS[1].Y + NEW_SAVE_Y_OFFSET)), Color.White);
                }
                if (!file3Exists)
                {
                    sb.Draw(newSaveIcon, Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, new Vector2(SAVE_BUTTONS[2].X + NEW_SAVE_X_OFFSET, SAVE_BUTTONS[2].Y + NEW_SAVE_Y_OFFSET)), Color.White);
                }
            }
            sb.Draw(state == MainMenuState.SETTINGS ? settingsIconSelected : hoveringSettings ? settingsIconEnlarge : settingsIcon, Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, new Vector2(SETTINGS_BUTTON.X, SETTINGS_BUTTON.Y)), Color.White);
            sb.Draw(hoveringQuit ? quitIconEnlarge : quitIcon, Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, new Vector2(QUIT_BUTTON.X, QUIT_BUTTON.Y)), Color.White);
            //sb.Draw(mouseCursor, Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, controller.GetMousePos()), Color.White);
        }

        public MainMenuState GetState()
        {
            return state;
        }

    }
}
