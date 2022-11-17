using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using System;
using Plateau.Components;
using Plateau.Entities;
using Plateau.Items;
using Microsoft.Xna.Framework.Content;
using MonoGame.Extended.ViewportAdapters;
using Plateau.Particles;
using Plateau.Sound;

namespace Plateau
{
    //System.Diagnostics.Debug.WriteLine(text);
    public class PlateauMain : Game
    {
        public class ResolutionSettings
        {
            public int resolution_width, resolution_height;
            public int scale, font_line_spacing; 
            public float font_spacing, font_scale;

            public ResolutionSettings(int resolution_width, int resolution_height, int scale, float font_spacing, int font_line_spacing, float font_scale)
            {
                this.resolution_width = resolution_width;
                this.resolution_height = resolution_height;
                this.scale = scale;
                this.font_spacing = font_spacing;
                this.font_line_spacing = font_line_spacing;
                this.font_scale = font_scale;
            }

            public override string ToString()
            {
                return resolution_width + "x" + resolution_height;
            }
        }

        public static SpriteFont FONT;
        public static float FONT_SCALE;

        public enum PlateauGameState
        {
            NORMAL, MAINMENU, DEBUG, CUTSCENE
        }

        public PlateauGameState currentState;

        public static int SCREEN_RESOLUTION_WIDTH;
        public static int SCREEN_RESOLUTION_HEIGHT;
        public static int NATIVE_RESOLUTION_WIDTH = 320;
        public static int NATIVE_RESOLUTION_HEIGHT = 180;
        public static Vector2 SHIFT_VECTOR;
        public static int SCALE;
        private static ResolutionSettings[] RESOLUTIONS;
        public static ResolutionSettings CURRENT_RESOLUTION;

        public static GraphicsDeviceManager GRAPHICS;
        public static GameWindow WINDOW;
        public static ContentManager CONTENT;
        public static GameServiceContainer SERVICES;

        public static string CONTENT_ROOT_DIRECTORY = "Content";

        private static SpriteBatch spriteBatch;

        private static Camera camera;
        public static Controller controller;
        private EntityPlayer player;
        private GameplayInterface ui;
        public static SaveManager SAVE_MANAGER;
        private MainMenuInterface mmi;
        private World world;
        private DebugConsole debugConsole;
        private SoundSystem soundSystem;

        private static RenderTarget2D mainTarget, lightsTarget, uiTarget;
        private Effect lightsShader;
        private CutsceneManager.Cutscene currentCutscene;
        private bool cutsceneTransitionDone;

        private AnimatedSprite lightMaskSmall, lightMaskMedium, lightMaskLarge;

        private bool mainMenuTransitionStarted = false;
        private bool mainMenuTransitionDone = false;

        public PlateauMain()
        {
            GRAPHICS = new GraphicsDeviceManager(this);

            currentState = PlateauGameState.MAINMENU;
            currentCutscene = null;
            cutsceneTransitionDone = false;

            IsFixedTimeStep = true; //nonvariable time steps...
            TargetElapsedTime = TimeSpan.FromMilliseconds(16.66667f); //60fps

            RESOLUTIONS = new ResolutionSettings[5];
            /*RESOLUTIONS[0] = new ResolutionSettings(1280, 720, 4, 1, 33, 0.2425f);
            RESOLUTIONS[1] = new ResolutionSettings(1600, 900, 5, 0, 33, 0.2025f);
            RESOLUTIONS[2] = new ResolutionSettings(1920, 1080, 6, 0, 33, 0.22f);
            RESOLUTIONS[3] = new ResolutionSettings(2560, 1440, 8, 0, 41, 0.22f);
            RESOLUTIONS[4] = new ResolutionSettings(3840, 2160, 12, 0, 41, 0.22f);*/
            RESOLUTIONS[0] = new ResolutionSettings(1280, 720, 4, 0, 37, 0.2f);
            RESOLUTIONS[1] = new ResolutionSettings(1600, 900, 5, 0, 37, 0.2f);
            RESOLUTIONS[2] = new ResolutionSettings(1920, 1080, 6, 0, 37, 0.2f);
            RESOLUTIONS[3] = new ResolutionSettings(2560, 1440, 8, 0, 37, 0.2f);
            RESOLUTIONS[4] = new ResolutionSettings(3840, 2160, 12, 0, 37, 0.2f);
        }

        public static bool CanResolutionIncrease()
        {
            int resIndex = 0;
            while(RESOLUTIONS[resIndex] != CURRENT_RESOLUTION)
            {
                resIndex++;
            }

            if(resIndex+1 == RESOLUTIONS.Length)
            {
                return false;
            }

            if(GameState.ContainsFlag(GameState.FLAG_SETTINGS_WINDOWED) && GameState.GetFlagValue(GameState.FLAG_SETTINGS_WINDOWED) == 1)
            {
                return true;
            }

            //if both the next resolution up's width & height are contained within the screen, we can increase.
            return RESOLUTIONS[resIndex + 1].resolution_height <= SCREEN_RESOLUTION_HEIGHT &&
                RESOLUTIONS[resIndex + 1].resolution_width <= SCREEN_RESOLUTION_WIDTH; 
        }

        public static bool CanResolutionDecrease()
        {
            return RESOLUTIONS[0] != CURRENT_RESOLUTION;
        }

        public static void IncreaseResolution()
        {
            int resIndex = 0;
            while (RESOLUTIONS[resIndex] != CURRENT_RESOLUTION)
            {
                resIndex++;
            }

            ChangeResolution(RESOLUTIONS[resIndex + 1]);
        }

        public static void DecreaseResolution()
        {
            int resIndex = 0;
            while (RESOLUTIONS[resIndex] != CURRENT_RESOLUTION)
            {
                resIndex++;
            }

            ChangeResolution(RESOLUTIONS[resIndex - 1]);
        }

        public static ContentManager createContentManager()
        {
            ContentManager newCM;
            newCM = new ContentManager(PlateauMain.SERVICES);
            newCM.RootDirectory = PlateauMain.CONTENT.RootDirectory;
            return newCM;
        }

        public static void UpdateWindowed()
        {
            if(GameState.GetFlagValue(GameState.FLAG_SETTINGS_WINDOWED) == 1)
            {
                WINDOW.Position = new Point(0, 10);
                WINDOW.IsBorderless = false;
            } else
            {
                WINDOW.Position = new Point(0, 0);
                WINDOW.IsBorderless = true;
                while(CURRENT_RESOLUTION.resolution_height > SCREEN_RESOLUTION_HEIGHT || CURRENT_RESOLUTION.resolution_width > SCREEN_RESOLUTION_WIDTH)
                {
                    DecreaseResolution();
                }
            }
            ChangeResolution(CURRENT_RESOLUTION);
        }

        private static void ChangeResolution(ResolutionSettings newResolution)
        {
            CURRENT_RESOLUTION = newResolution;

            SCALE = newResolution.scale;
            FONT_SCALE = newResolution.font_scale;
            FONT.Spacing = newResolution.font_spacing;
            FONT.LineSpacing = newResolution.font_line_spacing;

            if (GameState.ContainsFlag(GameState.FLAG_SETTINGS_WINDOWED) && GameState.GetFlagValue(GameState.FLAG_SETTINGS_WINDOWED) == 1)
            {
                GRAPHICS.PreferredBackBufferWidth = newResolution.resolution_width;
                GRAPHICS.PreferredBackBufferHeight = newResolution.resolution_height;
                SHIFT_VECTOR = new Vector2(0, 0);
                GRAPHICS.ApplyChanges();
            } else //fullscreen
            {
                GRAPHICS.PreferredBackBufferWidth = SCREEN_RESOLUTION_WIDTH;
                GRAPHICS.PreferredBackBufferHeight = SCREEN_RESOLUTION_HEIGHT;
                Vector2 screenCenter = new Vector2(SCREEN_RESOLUTION_WIDTH / 2, SCREEN_RESOLUTION_HEIGHT / 2);
                Vector2 gamespaceSize = new Vector2(NATIVE_RESOLUTION_WIDTH * SCALE, NATIVE_RESOLUTION_HEIGHT * SCALE);
                SHIFT_VECTOR = new Vector2(screenCenter.X - (gamespaceSize.X / 2), screenCenter.Y - (gamespaceSize.Y / 2));
                GRAPHICS.ApplyChanges();
            }

            camera = new Camera(new OrthographicCamera(new BoxingViewportAdapter(WINDOW, GRAPHICS.GraphicsDevice, NATIVE_RESOLUTION_WIDTH * SCALE, NATIVE_RESOLUTION_HEIGHT * SCALE)));
            camera.SetZoom(SCALE);
            lightsTarget = new RenderTarget2D(GRAPHICS.GraphicsDevice, NATIVE_RESOLUTION_WIDTH * SCALE, NATIVE_RESOLUTION_HEIGHT * SCALE);
            mainTarget = new RenderTarget2D(GRAPHICS.GraphicsDevice, NATIVE_RESOLUTION_WIDTH * SCALE, NATIVE_RESOLUTION_HEIGHT * SCALE);
            uiTarget = new RenderTarget2D(GRAPHICS.GraphicsDevice, NATIVE_RESOLUTION_WIDTH * SCALE, NATIVE_RESOLUTION_HEIGHT * SCALE);
        }

        protected override void Initialize()
        {
            Content.RootDirectory = CONTENT_ROOT_DIRECTORY;
            spriteBatch = new SpriteBatch(GRAPHICS.GraphicsDevice);

            WINDOW = Window;
            WINDOW.Title = "Plateau";
            WINDOW.Position = new Point(0, 0);
            WINDOW.IsBorderless = true;

            SCREEN_RESOLUTION_WIDTH = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            SCREEN_RESOLUTION_HEIGHT = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            GRAPHICS.PreferredBackBufferWidth =  SCREEN_RESOLUTION_WIDTH; 
            GRAPHICS.PreferredBackBufferHeight =  SCREEN_RESOLUTION_HEIGHT; 
            IsMouseVisible = true;
            GRAPHICS.ApplyChanges();

            controller = new Controller();

            ui = new GameplayInterface(controller);
            mmi = new MainMenuInterface(controller, SaveManager.DoesSaveExist(1), SaveManager.DoesSaveExist(2), SaveManager.DoesSaveExist(3));

            Util.Initialize();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            CONTENT = Content;
            SERVICES = Services;

            FONT = Content.Load<SpriteFont>(Paths.FONT);

            ChangeResolution(RESOLUTIONS[0]);
            while(CanResolutionIncrease())
            {
                IncreaseResolution();
            }

            SoundSystem.Initialize(Content);
            DialogueNode.LoadPortraits();

            ParticleFactory.LoadContent(Content);
            AppliedEffects.Initialize(Content);
            ItemDict.LoadContent(Content);

            GameState.Initialize();
            
            ui.LoadContent(Content);
            world = new World(ui);
            player = new EntityPlayer(world, controller);
            LootTables.Initialize();
            mmi.LoadContent(Content, camera.GetBoundingBox());
            world.LoadContent(Content, GraphicsDevice, player, camera.GetBoundingBox());

            lightsShader = Content.Load<Effect>(Paths.SHADER_LIGHTS);

            //lightsTarget = new RenderTarget2D(GraphicsDevice, NATIVE_RESOLUTION_WIDTH * SCALE, NATIVE_RESOLUTION_HEIGHT * SCALE);
            float[] lengths = Util.CreateAndFillArray(4, 0.2f);
            lightMaskSmall = new AnimatedSprite(Content.Load<Texture2D>(Paths.TEXTURE_LIGHT_SMALL_SPRITESHEET), 4, 1, 4, lengths);
            lightMaskSmall.AddLoop("loop", 0, 3, true);
            lightMaskSmall.SetLoop("loop");
            lightMaskMedium = new AnimatedSprite(Content.Load<Texture2D>(Paths.TEXTURE_LIGHT_MEDIUM_SPRITESHEET), 4, 1, 4, lengths);
            lightMaskMedium.AddLoop("loop", 0, 3, true);
            lightMaskMedium.SetLoop("loop");
            lightMaskLarge = new AnimatedSprite(Content.Load<Texture2D>(Paths.TEXTURE_LIGHT_LARGE_SPRITESHEET), 4, 1, 4, lengths);
            lightMaskLarge.AddLoop("loop", 0, 3, true);
            lightMaskLarge.SetLoop("loop");

            EntityPlayer.FishlinePart.LoadContent(Content);
            CutsceneManager.Initialize(player, camera);
        }

        protected override void UnloadContent()
        {
            Content.Unload();
            Util.UnloadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (this.IsActive)
            {
                if (ui.IsPaused())
                {
                    ui.Unpause();
                    player.IgnoreMouseInputThisFrame();
                }
                if (player.GetInterfaceState() == InterfaceState.EXIT_CONFIRMED)
                {
                    Exit();
                }

                if (currentState == PlateauGameState.NORMAL && (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || controller.IsKeyPressed(KeyBinds.ESCAPE) && player.GetCurrentDialogue() == null))
                {
                    if (!ui.IsItemHeld())
                    {
                        if (player.GetInterfaceState() == InterfaceState.NONE)
                        {
                            player.SetInterfaceState(InterfaceState.EXIT);
                            player.Pause();
                            world.Pause();
                        }
                        else
                        {
                            player.SetInterfaceState(InterfaceState.NONE);
                            player.Unpause();
                            world.Unpause();
                        }
                    }
                    else
                    {
                        player.AddNotification(new EntityPlayer.Notification("I can't close my inventory while holding an item. Place the item down then try again.", Color.Red, EntityPlayer.Notification.Length.LONG));
                    }
                }
                controller.Update(); //read in inputs from mouse/keyboard

                if (currentState == PlateauGameState.CUTSCENE)
                {
                    if (!cutsceneTransitionDone)
                    {
                        player.StopAllMovement();
                        player.SetToDefaultPose();
                        ui.Update(deltaTime, player, camera.GetBoundingBox(), world.GetCurrentArea(), world.GetTimeData(), world); //update the interface
                        world.Update(deltaTime, gameTime, player, ui, camera, true); //update current area
                        if (ui.IsTransitionReady())
                        {
                            if (!currentCutscene.IsComplete())
                            {
                                currentCutscene.OnActivation(player, world, camera);
                                ui.Hide();
                                ui.TransitionFadeIn();
                            }
                            else
                            {
                                currentCutscene.OnFinish(player, world, camera);
                                camera.Update(1f, player, world.GetCurrentArea().MapPixelWidth(), world.GetCurrentArea().MapPixelHeight());
                                ui.Unhide();
                                ui.TransitionFadeIn();
                                currentCutscene = null;
                                currentState = PlateauGameState.NORMAL;
                            }
                            cutsceneTransitionDone = true;
                        }
                    }
                    else
                    {
                        UpdateLightMasks(deltaTime);
                        currentCutscene.Update(deltaTime, player, world.GetCurrentArea(), world, camera);
                        ui.Update(deltaTime, player, camera.GetBoundingBox(), world.GetCurrentArea(), world.GetTimeData(), world); //update the interface
                        world.Update(deltaTime, gameTime, player, ui, camera, true); //update current area
                        if (currentCutscene.IsComplete())
                        {
                            if (currentCutscene.transitionOut == CutsceneManager.CutsceneTransition.FADE)
                            {
                                cutsceneTransitionDone = false;
                                ui.TransitionFadeToBlack();
                            } else
                            {
                                currentCutscene.OnFinish(player, world, camera);
                                camera.Update(1f, player, world.GetCurrentArea().MapPixelWidth(), world.GetCurrentArea().MapPixelHeight());
                                ui.Unhide();
                                currentCutscene = null;
                                currentState = PlateauGameState.NORMAL;
                            }
                        } 
                    }
                }
                else if (currentState == PlateauGameState.DEBUG)
                {
                    controller.ActivateStringInput();
                    if (controller.IsKeyPressed(KeyBinds.ESCAPE) || controller.IsKeyPressed(KeyBinds.CONSOLE))
                    {
                        currentState = PlateauGameState.NORMAL;
                    }
                    if (controller.IsKeyPressed(KeyBinds.ENTER))
                    {
                        string command = controller.GetStringInput();
                        debugConsole.RunCommand(command);
                        controller.ClearStringInput();
                        ui.Update(0, player, camera.GetBoundingBox(), world.GetCurrentArea(), world.GetTimeData(), world);
                        if (!camera.IsLocked())
                        {
                            camera.Update(0, player, world.GetCurrentArea().MapPixelWidth(), world.GetCurrentArea().MapPixelHeight());
                        }
                    }
                }
                else if (currentState == PlateauGameState.MAINMENU)
                {
                    mmi.Update(deltaTime, camera.GetBoundingBox());
                    if (mmi.GetState() != MainMenuInterface.MainMenuState.NONE)
                    {
                        ui.Update(deltaTime, player, camera.GetBoundingBox(), world.GetCurrentArea(), world.GetTimeData(), world); //update the interface
                        if (!mainMenuTransitionStarted)
                        {
                            //start the transition by fading to block
                            ui.Hide();
                            ui.TransitionFadeToBlack();
                            mainMenuTransitionStarted = true;
                        }
                        else if (!mainMenuTransitionDone)
                        {
                            //wait until transition is done (fading to black) to load file
                            if (ui.IsTransitionReady())
                                mainMenuTransitionDone = true;
                        }
                        else
                        {
                            if (mmi.GetState() == MainMenuInterface.MainMenuState.CLICKED_SAVE_1)
                            {
                                SAVE_MANAGER = new SaveManager(SaveManager.FILE_NAME_1);
                            }
                            else if (mmi.GetState() == MainMenuInterface.MainMenuState.CLICKED_SAVE_2)
                            {
                                SAVE_MANAGER = new SaveManager(SaveManager.FILE_NAME_2);
                            }
                            else if (mmi.GetState() == MainMenuInterface.MainMenuState.CLICKED_SAVE_3)
                            {
                                SAVE_MANAGER = new SaveManager(SaveManager.FILE_NAME_3);
                            }
                            SAVE_MANAGER.LoadFile(player, world);
                            ui.Update(0, player, camera.GetBoundingBox(), world.GetCurrentArea(), world.GetTimeData(), world); //update after loading file so hotbar shows correctly on fadein
                            currentState = PlateauGameState.NORMAL;
                            UpdateWindowed();
                            for (int i = 0; i < RESOLUTIONS.Length; i++)
                            {
                                if (RESOLUTIONS[i].scale == GameState.GetFlagValue(GameState.FLAG_SETTINGS_RESOLUTION_SCALE))
                                {
                                    ChangeResolution(RESOLUTIONS[i]);
                                }
                            }
                            debugConsole = new DebugConsole(world, SAVE_MANAGER, player, camera);
                            ui.Unhide();
                            ui.TransitionFadeIn();
                        }
                    }
                }
                else if (currentState == PlateauGameState.NORMAL)
                {
                    if (ui.NoMenusOpen())
                    {
                        UpdateLightMasks(deltaTime);
                    }

                    player.SetController(controller);
                    player.Update(deltaTime, world.GetCurrentArea());
                    if (!camera.IsLocked())
                    {
                        camera.Update(deltaTime, player, world.GetCurrentArea().MapPixelWidth(), world.GetCurrentArea().MapPixelHeight()); //move camera
                    }
                    ui.Update(deltaTime, player, camera.GetBoundingBox(), world.GetCurrentArea(), world.GetTimeData(), world); //update the interface
                    world.Update(deltaTime, gameTime, player, ui, camera, false); //update current area


                    if (!player.IsPaused())
                    {
                        world.GetCurrentArea().CheckEntityCollisions(player);
                        world.HandleTransitions(player);
                    }


                    if (controller.IsKeyPressed(KeyBinds.CONSOLE))
                    {
                        currentState = PlateauGameState.DEBUG;
                        controller.ClearStringInput();
                    }

                    if (world.IsDayOver())
                    {
                        currentCutscene = CutsceneManager.CUTSCENE_SLEEP;
                        SAVE_MANAGER.SaveFile(player, world);
                    } else
                    {
                        currentCutscene = world.GetCutsceneIfPossible(player);
                    }

                    if (currentCutscene != null)
                    {
                        currentState = PlateauGameState.CUTSCENE;
                        if (currentCutscene.transitionIn == CutsceneManager.CutsceneTransition.FADE)
                        {
                            cutsceneTransitionDone = false;
                            ui.TransitionFadeToBlack();
                        }
                        else
                        {
                            ui.Hide();
                            cutsceneTransitionDone = true;
                            currentCutscene.OnActivation(player, world, camera);
                        }
                    }
                }
                SoundSystem.Update(deltaTime, player, world);
            }
            else
            {
                ui.Pause();
            }

            if (currentCutscene != null && currentCutscene.background == CutsceneManager.CutsceneBackground.SPECTRUM)
            {
                Color targetColor = SPECTRUM_ROTATION[currentSpectrumColorTarget];
                if (spectrumR == targetColor.R && spectrumB == targetColor.B && spectrumG == targetColor.G)
                {
                    currentSpectrumColorTarget++;
                    if (currentSpectrumColorTarget == SPECTRUM_ROTATION.Length)
                    {
                        currentSpectrumColorTarget = 0;
                    }
                    startingR = spectrumR;
                    startingB = spectrumB;
                    startingG = spectrumG;
                }

                spectrumR = Util.AdjustTowards(spectrumR, targetColor.R, deltaTime * (Math.Abs(startingR - targetColor.R) / TIME_PER_CHANGE));
                spectrumB = Util.AdjustTowards(spectrumB, targetColor.B, deltaTime * (Math.Abs(startingB - targetColor.B) / TIME_PER_CHANGE));
                spectrumG = Util.AdjustTowards(spectrumG, targetColor.G, deltaTime * (Math.Abs(startingG - targetColor.G) / TIME_PER_CHANGE));
            } else
            {
                startingR = 255;
                startingB = 255;
                startingG = 255;
                spectrumR = 255;
                spectrumB = 255;
                spectrumG = 255;
            }

            base.Update(gameTime);
        }

        private static int TIME_PER_CHANGE = 3;
        private float startingR = 0;
        private float startingG = 0;
        private float startingB = 0;
        private float spectrumR = 255;
        private float spectrumG = 255;
        private float spectrumB = 255;
        private static Color[] SPECTRUM_ROTATION = { 
            Color.LightPink,
            Color.IndianRed,
            Color.LightYellow,
            Color.SeaGreen,
            Color.CornflowerBlue,
            Color.BlueViolet,
            Color.PaleVioletRed
        };
        private int currentSpectrumColorTarget;

        protected override void Draw(GameTime gameTime)
        {
            if (currentState == PlateauGameState.MAINMENU)
            {
                ui.Hide();
                GRAPHICS.GraphicsDevice.SetRenderTarget(mainTarget);
                GRAPHICS.GraphicsDevice.Clear(Color.CadetBlue);
                spriteBatch.Begin(transformMatrix: camera.GetViewMatrix(), samplerState: SamplerState.PointClamp, blendState: BlendState.AlphaBlend);
                mmi.Draw(spriteBatch, camera.GetBoundingBox());
                ui.Draw(spriteBatch, camera.GetBoundingBox(), 0.99f);
                spriteBatch.End();

                GRAPHICS.GraphicsDevice.SetRenderTarget(null);
                GRAPHICS.GraphicsDevice.Clear(Color.Black);
                spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp);
                spriteBatch.Draw(mainTarget, SHIFT_VECTOR, Color.White);
                spriteBatch.End();
            }
            else if (currentState == PlateauGameState.NORMAL || currentState == PlateauGameState.DEBUG || currentState == PlateauGameState.CUTSCENE)
            {
                if (currentState != PlateauGameState.CUTSCENE || currentCutscene.background == CutsceneManager.CutsceneBackground.WORLD || 
                    (cutsceneTransitionDone == false && currentCutscene.IsComplete() == false) || (cutsceneTransitionDone == true && currentCutscene.IsComplete() == true))
                {
                    //lighting mask
                    GRAPHICS.GraphicsDevice.SetRenderTarget(lightsTarget);
                    GRAPHICS.GraphicsDevice.Clear(Color.WhiteSmoke);
                    spriteBatch.Begin(SpriteSortMode.Immediate, transformMatrix: camera.GetViewMatrix(), samplerState: SamplerState.PointClamp, blendState: BlendState.AlphaBlend);
                    foreach (Area.LightSource ls in world.GetCurrentArea().GetAreaLights())
                    {
                        AnimatedSprite lightMask = null;
                        switch (ls.lightStrength)
                        {
                            case Area.LightSource.Strength.SMALL:
                                lightMask = lightMaskSmall;
                                break;
                            case Area.LightSource.Strength.MEDIUM:
                                lightMask = lightMaskMedium;
                                break;
                            case Area.LightSource.Strength.LARGE:
                                lightMask = lightMaskLarge;
                                break;
                        }
                        Vector2 centeredPosition = Util.CenterTextureOnPoint(ls.position, lightMask.GetFrameWidth(), lightMask.GetFrameHeight());
                        if ((world.GetCurrentArea().IsPlayerInCave(player) && world.GetCurrentArea().IsPositionInsideCave(ls.position)) ||
                            !world.GetCurrentArea().IsPositionInsideCave(ls.position))
                        {
                            lightMask.Draw(spriteBatch, centeredPosition, ls.color, 0.0f);
                        }
                    }
                    /*Vector2 playerLightPos = Util.CenterTextureOnPoint(player.GetPosition(), lightMaskSmall);
                    playerLightPos.Y += 10;
                    playerLightPos.X += 2;
                    spriteBatch.Draw(lightMaskSmall, playerLightPos, Color.White);*/
                    spriteBatch.End();

                    GRAPHICS.GraphicsDevice.SetRenderTarget(mainTarget);
                    GRAPHICS.GraphicsDevice.Clear(Color.CadetBlue);
                    spriteBatch.Begin(transformMatrix: camera.GetViewMatrix(), samplerState: SamplerState.PointClamp, blendState: BlendState.AlphaBlend, sortMode: SpriteSortMode.Deferred);
                    world.DrawBackground(spriteBatch, camera.GetBoundingBox(), 0.0f); //depth 0.0f
                    world.DrawEntities(spriteBatch, DrawLayer.BACKGROUND_BEHIND_WALL, camera.GetBoundingBox(), 0.025f); //depth 0.10f
                    spriteBatch.End();
                    spriteBatch.Begin(transformMatrix: camera.GetViewMatrix(), samplerState: SamplerState.PointClamp, blendState: BlendState.AlphaBlend, sortMode: SpriteSortMode.Deferred);
                    world.DrawWaterBackground(spriteBatch, 0.025f);
                    world.DrawWalls(spriteBatch, 0.05f); //depth 0.05f
                    world.DrawBuildingBlocks(spriteBatch, 0.075f); //depth 0.075f
                    world.DrawEntities(spriteBatch, DrawLayer.BACKGROUND_WALLPAPER, camera.GetBoundingBox(), 0.10f); //depth 0.10f
                    world.DrawEntities(spriteBatch, DrawLayer.BACKGROUND_WALL, camera.GetBoundingBox(), 0.15f); //depth 0.15f
                    world.DrawEntities(spriteBatch, DrawLayer.NORMAL, camera.GetBoundingBox(), 0.20f); //depth 0.2f
                    spriteBatch.End();
                    spriteBatch.Begin(transformMatrix: camera.GetViewMatrix(), samplerState: SamplerState.PointClamp, blendState: BlendState.AlphaBlend, sortMode: SpriteSortMode.Deferred);
                    world.DrawBaseTerrain(spriteBatch, 0.25f); //depth 0.25f
                    world.DrawEntities(spriteBatch, DrawLayer.FOREGROUND_CARPET, camera.GetBoundingBox(), 0.30f); //depth 0.3f
                    world.DrawParticles(spriteBatch, 0.35f); //depth 0.35f
                    world.DrawEntities(spriteBatch, DrawLayer.PRIORITY, camera.GetBoundingBox(), 0.4f); //depth 0.4f
                    world.DrawItemEntities(spriteBatch, 0.45f); //depth 0.45f
                    player.Draw(spriteBatch, 0.5f); //depth 0.5f
                    spriteBatch.End();
                    spriteBatch.Begin(transformMatrix: camera.GetViewMatrix(), samplerState: SamplerState.PointClamp, blendState: BlendState.AlphaBlend, sortMode: SpriteSortMode.Deferred);
                    world.DrawEntities(spriteBatch, DrawLayer.FOREGROUND, camera.GetBoundingBox(), 0.55f); //depth 0.55f
                    world.DrawWater(spriteBatch, 0.60f); //depth 0.60f;
                    world.DrawForeground(spriteBatch, camera.GetBoundingBox(), 0.65f); //depth 0.65f
                    spriteBatch.End();
                } else if (currentCutscene.background == CutsceneManager.CutsceneBackground.SKY)
                {
                    GRAPHICS.GraphicsDevice.SetRenderTarget(mainTarget);
                    GRAPHICS.GraphicsDevice.Clear(Color.CadetBlue);
                    spriteBatch.Begin(transformMatrix: camera.GetViewMatrix(), samplerState: SamplerState.PointClamp, blendState: BlendState.AlphaBlend, sortMode: SpriteSortMode.Deferred);
                    world.DrawBackground(spriteBatch, camera.GetBoundingBox(), 0.0f); //depth 0.0f
                    spriteBatch.End();
                } else if (currentCutscene.background == CutsceneManager.CutsceneBackground.BLACK)
                {
                    GRAPHICS.GraphicsDevice.SetRenderTarget(mainTarget);
                    GRAPHICS.GraphicsDevice.Clear(Color.CadetBlue);
                    spriteBatch.Begin(transformMatrix: camera.GetViewMatrix(), samplerState: SamplerState.PointClamp, blendState: BlendState.AlphaBlend, sortMode: SpriteSortMode.Deferred);
                    spriteBatch.Draw(Content.Load<Texture2D>(Paths.INTERFACE_BACKGROUND_WHITE), camera.GetBoundingBox().TopLeft, Color.Black);
                    spriteBatch.End();
                }
                else if (currentCutscene.background == CutsceneManager.CutsceneBackground.BLACK)
                {
                    GRAPHICS.GraphicsDevice.SetRenderTarget(mainTarget);
                    GRAPHICS.GraphicsDevice.Clear(Color.CadetBlue);
                    spriteBatch.Begin(transformMatrix: camera.GetViewMatrix(), samplerState: SamplerState.PointClamp, blendState: BlendState.AlphaBlend, sortMode: SpriteSortMode.Deferred);
                    spriteBatch.Draw(Content.Load<Texture2D>(Paths.INTERFACE_BACKGROUND_WHITE), camera.GetBoundingBox().TopLeft, Color.White);
                    spriteBatch.End();
                }
                else if (currentCutscene.background == CutsceneManager.CutsceneBackground.SPECTRUM)
                { 
                    GRAPHICS.GraphicsDevice.SetRenderTarget(mainTarget);
                    GRAPHICS.GraphicsDevice.Clear(Color.CadetBlue);
                    spriteBatch.Begin(transformMatrix: camera.GetViewMatrix(), samplerState: SamplerState.PointClamp, blendState: BlendState.AlphaBlend, sortMode: SpriteSortMode.Deferred);
                    spriteBatch.Draw(Content.Load<Texture2D>(Paths.INTERFACE_BACKGROUND_WHITE), camera.GetBoundingBox().TopLeft, new Color((byte)spectrumR, (byte)spectrumG, (byte)spectrumB));
                    spriteBatch.End();
                }

                //GraphicsDevice.SetRenderTarget(null);
                //GraphicsDevice.Clear(Color.CadetBlue);
                /*spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp);
                lightsShader.Parameters["lightMask"].SetValue(lightsTarget);
                lightsShader.Parameters["darknessLevel"].SetValue(world.GetDarkLevel());
                lightsShader.CurrentTechnique.Passes[0].Apply();
                spriteBatch.End();*/

                if (currentState == PlateauGameState.DEBUG)
                {
                    spriteBatch.Begin(transformMatrix: camera.GetViewMatrix(), samplerState: SamplerState.PointClamp, blendState: BlendState.AlphaBlend, sortMode: SpriteSortMode.Deferred);
                    GameplayInterface.QUEUED_STRINGS.Add(new QueuedString("Command: " + controller.GetStringInput(), Util.ConvertFromAbsoluteToCameraVector(camera.GetBoundingBox(), new Vector2(4, 30)), debugConsole.DidLastSucceed() ? Color.LightGreen : Color.DarkRed));
                    spriteBatch.DrawString(PlateauMain.FONT, "X: " + player.GetCenteredPosition().X + "  Y: " + player.GetCenteredPosition().Y, Util.ConvertFromAbsoluteToCameraVector(camera.GetBoundingBox(), new Vector2(4, 38)), Color.Black, 0.0f, Vector2.Zero, PlateauMain.FONT_SCALE, SpriteEffects.None, 0.0f);
                    int column = 0;
                    int counter = 0;
                    int yPos = 45;
                    foreach (string command in DebugConsole.COMMAND_LIST)
                    {
                        GameplayInterface.QUEUED_STRINGS.Add(new QueuedString(command, Util.ConvertFromAbsoluteToCameraVector(camera.GetBoundingBox(), new Vector2(65 + (80 * column), yPos)), Color.Black));
                        yPos += 8;
                        counter++;
                        if (counter == 10)
                        {
                            column++;
                            counter = 0;
                            yPos = 45;
                        }
                    }
                    spriteBatch.End();
                }


                GRAPHICS.GraphicsDevice.SetRenderTarget(uiTarget);
                GRAPHICS.GraphicsDevice.Clear(Color.Transparent);
                spriteBatch.Begin(transformMatrix: camera.GetViewMatrix(), samplerState: SamplerState.PointClamp, blendState: BlendState.AlphaBlend, sortMode: SpriteSortMode.Deferred);
                ui.Draw(spriteBatch, camera.GetBoundingBox(), 0.99f);
                Util.Draw(spriteBatch, 1.0f);
                spriteBatch.End();

                spriteBatch.Begin(transformMatrix: camera.GetViewMatrix(), samplerState: SamplerState.LinearClamp, blendState: BlendState.AlphaBlend, sortMode: SpriteSortMode.Deferred);
                ui.DrawStrings(spriteBatch);
                spriteBatch.End();

                spriteBatch.Begin(transformMatrix: camera.GetViewMatrix(), samplerState: SamplerState.PointClamp, blendState: BlendState.AlphaBlend, sortMode: SpriteSortMode.Deferred);
                ui.DrawTooltip(spriteBatch, camera.GetBoundingBox());
                spriteBatch.End();

                GRAPHICS.GraphicsDevice.SetRenderTarget(null);
                GRAPHICS.GraphicsDevice.Clear(Color.Black); 
                spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp);
                lightsShader.Parameters["lightMask"].SetValue(lightsTarget);
                lightsShader.Parameters["darknessLevel"].SetValue(world.GetDarkLevel() * world.GetCurrentArea().GetDarkLevel(player.GetAdjustedPosition()));
                if (currentState != PlateauGameState.CUTSCENE || currentCutscene.background == CutsceneManager.CutsceneBackground.WORLD ||
                    (cutsceneTransitionDone == false && currentCutscene.IsComplete() == false) || (cutsceneTransitionDone == true && currentCutscene.IsComplete() == true))
                {
                    lightsShader.CurrentTechnique.Passes[0].Apply();
                }
                //spriteBatch.Draw(lightsTarget, SHIFT_VECTOR, Color.White);
                spriteBatch.Draw(mainTarget, SHIFT_VECTOR, Color.White);
                spriteBatch.End();

                spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp);
                spriteBatch.Draw(uiTarget, SHIFT_VECTOR, Color.White);
                spriteBatch.End();

            }

            base.Draw(gameTime);
        }

        private void UpdateLightMasks(float deltaTime)
        {
            lightMaskSmall.Update(deltaTime);
            lightMaskMedium.Update(deltaTime);
            lightMaskLarge.Update(deltaTime);
        }
    }
}
