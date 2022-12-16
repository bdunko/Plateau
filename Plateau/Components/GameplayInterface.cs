using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using Plateau.Entities;
using Plateau.Items;
using Plateau.Sound;
using System;
using System.Collections.Generic;

namespace Plateau.Components
{
    public enum InterfaceState
    {
        NONE, INVENTORY, CHEST, SCRAPBOOK, EXIT, EXIT_CONFIRMED, SETTINGS, CRAFTING,
        TRANSITION_TO_UP, TRANSITION_TO_DOWN, TRANSITION_TO_LEFT, TRANSITION_TO_RIGHT, TRANSITION_FADE_TO_BLACK, TRANSITION_FADE_IN
    }

    public class QueuedString
    {
        public Vector2 position;
        public string text;
        public Color color;
        public QueuedString(string text, Vector2 position, Color color)
        {
            this.text = text;
            this.position = position;
            this.color = color;
        }
    }

    public class NineSlice
    {
        private Texture2D slicetopleft, slicetopright, slicetopcenter, slicebottomleft, slicebottomright, slicebottomcenter, slicemiddleleft, slicemiddleright, slicemiddlecenter;
        private bool tight;

        public NineSlice(Texture2D topleft, Texture2D topright, Texture2D topcenter,
            Texture2D middleleft, Texture2D middleright, Texture2D middlecenter,
            Texture2D bottomleft, Texture2D bottomright, Texture2D bottomcenter, bool tight)
        {
            slicetopleft = topleft;
            slicetopright = topright;
            slicetopcenter = topcenter;
            slicebottomright = bottomright;
            slicebottomleft = bottomleft;
            slicebottomcenter = bottomcenter;
            slicemiddleleft = middleleft;
            slicemiddleright = middleright;
            slicemiddlecenter = middlecenter;
            this.tight = tight;
        }

        public void Draw(SpriteBatch sb, RectangleF rectangle, Color sliceColor)
        {
            int sliceWidth = slicemiddlecenter.Width;
            int sliceHeight = slicemiddlecenter.Height;

            if (!tight)
            {
                rectangle.X -= sliceWidth + 2;
                rectangle.Y -= sliceHeight - 1;
                rectangle.Width += sliceWidth + 4;
                rectangle.Height += sliceHeight; //-2
            } else if (rectangle.Width != 0)
            {
                
                rectangle.X -= 1;
                rectangle.Y -= 1;
                rectangle.Height += 0.5f;
                rectangle.Width += 0.5f;
            }

            for (int x = 0; x < rectangle.Width; x += sliceWidth)
            {
                for (int y = 0; y < rectangle.Height; y += sliceHeight)
                {
                    Texture2D drawnPiece = slicemiddlecenter;
                    if (y == 0)
                    {
                        if (x == 0)
                        {
                            drawnPiece = slicetopleft;
                        }
                        else if (x + sliceWidth >= rectangle.Width)
                        {
                            drawnPiece = slicetopright;
                        }
                        else
                        {
                            drawnPiece = slicetopcenter;
                        }
                    }
                    else if (x == 0)
                    {
                        if (y + sliceHeight >= rectangle.Height)
                        {
                            drawnPiece = slicebottomleft;
                        }
                        else
                        {
                            drawnPiece = slicemiddleleft;
                        }
                    }
                    else if (y + sliceHeight >= rectangle.Height)
                    {
                        if (x + sliceWidth >= rectangle.Width)
                        {
                            drawnPiece = slicebottomright;
                        }
                        else
                        {
                            drawnPiece = slicebottomcenter;
                        }
                    }
                    else if (x + sliceWidth >= rectangle.Width)
                    {
                        drawnPiece = slicemiddleright;
                    }
                    sb.Draw(drawnPiece, new Vector2(x + rectangle.X, y + rectangle.Y), sliceColor);
                }
            }
        }

        //adjusts position a bit too..
        public void DrawString(SpriteBatch sb, string str, Vector2 position, RectangleF cameraBoundingBox, Color textColor, Color sliceColor, bool autoadjust = false)
        {
            Vector2 textSize = PlateauMain.FONT.MeasureString(str) * PlateauMain.FONT_SCALE;
            if (autoadjust)
            {
                bool runsOffRight = position.X + textSize.X + 5 > cameraBoundingBox.Right;
                bool runsOffBottom = position.Y + textSize.Y + 24 > cameraBoundingBox.Bottom;

                if (runsOffRight && runsOffBottom)
                {
                    position.X = cameraBoundingBox.Right - (textSize.X + 8);
                    position.Y -= textSize.Y + 7; 
                }
                else if (runsOffRight)
                {
                    position.X = cameraBoundingBox.Right - (textSize.X + 8);
                    position.Y -= textSize.Y;
                }
                else if (runsOffBottom)
                {
                    position.Y = cameraBoundingBox.Bottom - (textSize.Y + 20);
                }

                if (position.Y - 4 < cameraBoundingBox.Top)
                {
                    position.Y = cameraBoundingBox.Top + 4;
                }
            }

            Draw(sb, new RectangleF(position, textSize), sliceColor);
            sb.DrawString(PlateauMain.FONT, str, position, textColor, 0.0f, Vector2.Zero, PlateauMain.FONT_SCALE, SpriteEffects.None, 0.0f);
            //GameplayInterface.QUEUED_STRINGS.Add(new QueuedString(str, position, textColor));
        }

    }

    public class GameplayInterface
    {
        public static NineSlice TOOLTIP_9SLICE;
        public static NineSlice INTERFACE_9SLICE;
        public static NineSlice DIALOGUE_9SLICE;
        public static NineSlice THOUGHTS_9SLICE;
        public static NineSlice WHITE_9SLICE;

        private class CollectedTooltip
        {
            public static int FAST_THRESHOLD = 50;

            public static float LENGTH = 0.8f;
            private static float DELTA_Y = 47;
            private static float DELTA_Y_FAST = 47;
            private static float LENGTH_FAST = 0.2f;

            public float timeElapsed;
            public string name;
            public bool fast;

            public CollectedTooltip(string name, bool fast)
            {
                this.name = name;
                this.timeElapsed = 0;
                this.fast = fast;
            }

            public void Update(float deltaTime)
            {
                timeElapsed += deltaTime;
            }

            public float GetYAdjustment()
            {
                return (fast ? DELTA_Y_FAST : DELTA_Y) * (timeElapsed / (fast ? LENGTH_FAST : LENGTH));
            }

            public bool IsFinished()
            {
                return timeElapsed >= (fast ? LENGTH_FAST : LENGTH);
            }
        }

        public static int HOTBAR_LENGTH = 10;

        private static float BLACK_BACKGROUND_OPACITY = 0.7f;
        private static float PLACEMENT_OPACITY = 0.9f;
        private static float GRID_OPACITY = 0.15f;

        private static int NUM_SCRAPBOOK_TABS = 11;
        private static int NUM_SCRAPBOOK_TITLES_PER_TAB = 10;
        private static int NUM_SCRAPBOOK_PAGES = NUM_SCRAPBOOK_TABS * NUM_SCRAPBOOK_TITLES_PER_TAB;

        private Controller controller;
        private Texture2D hotbar, hotbar_selected, inventory, chest_inventory, chest_inventory_greyscale, inventory_selected;
        private Texture2D craftingButtonSolo;
        private string tooltipName, tooltipDescription;
        private ItemStack[] inventoryItems;
        public static Texture2D[] numbers, numbersNoBorder;
        public static Texture2D itemBox, hoveringItemBox;
        private Texture2D black_background, black_box, grid;
        private Texture2D placeableTexture;
        private Texture2D[] seasonText;
        private Texture2D[] dayText;
        private Texture2D dateTimePanel;
        private Texture2D mouseControl, keyControl, menuControl, shiftOnUnpressed, shiftOnPressed, shiftOffUnpressed, shiftOffPressed, escPressed, escUnpressed;
        private Texture2D keyControlWDown, keyControlADown, keyControlSDown, keyControlDDown;
        private bool isWDown, isADown, isSDown, isDDown;
        private bool isHoldingPlaceable = false;
        private bool isPlaceableLocationValid = false;
        private bool showPlaceableTexture = true;
        private Vector2 placeableLocation;
        private Vector2 gridLocation;
        private List<CollectedTooltip> collectedTooltips;
        private Vector2 lastPlacedTile;

        private ClothingItem hat, shirt, outerwear, pants, socks, shoes, gloves, earrings, scarf, glasses, back, sailcloth, hair;
        private Item accessory1, accessory2, accessory3;
        private ClothedSprite playerSprite;

        private ItemStack[] chestInventory;
        private Color chestColor;
        private int seasonIndex, dayIndex, hourTensIndex, hourOnesIndex, minuteTensIndex, minuteOnesIndex;
        private Vector2 inventorySelectedPosition;
        private bool paused;

        private InterfaceState interfaceState;

        private RectangleF[] itemRectangles;
        private RectangleF[] chestRectangles;
        private Vector2 TOOLTIP_OFFSET = new Vector2(18, -8);

        private static Vector2 INVENTORY_POSITION = new Vector2(57f, 10); //if modified, have to manually edit garbage can location & clothing rectangles...
        private static RectangleF GLASSES_INVENTORY_RECT = new RectangleF(130f, 12, 18, 18);
        private static RectangleF BACK_INVENTORY_RECT = new RectangleF(130f, 31, 18, 18);
        private static RectangleF SAILCLOTH_INVENTORY_RECT = new RectangleF(130f, 50, 18, 18);
        private static RectangleF SCARF_INVENTORY_RECT = new RectangleF(149f, 12, 18, 18);
        private static RectangleF OUTERWEAR_INVENTORY_RECT = new RectangleF(149f, 31, 18, 18);
        private static RectangleF SOCKS_INVENTORY_RECT = new RectangleF(149f, 50, 18, 18);
        private static RectangleF HAT_INVENTORY_RECT = new RectangleF(168f, 12, 18, 18);
        private static RectangleF SHIRT_INVENTORY_RECT = new RectangleF(168f, 31, 18, 18);
        private static RectangleF PANTS_INVENTORY_RECT = new RectangleF(168f, 50, 18, 18);
        private static RectangleF EARRINGS_INVENTORY_RECT = new RectangleF(187f, 12, 18, 18);
        private static RectangleF GLOVES_INVENTORY_RECT = new RectangleF(187f, 31, 18, 18);
        private static RectangleF SHOES_INVENTORY_RECT = new RectangleF(187f, 50, 18, 18);
        private static RectangleF ACCESSORY1_INVENTORY_RECT = new RectangleF(206f, 12, 18, 18);
        private static RectangleF ACCESSORY2_INVENTORY_RECT = new RectangleF(206f, 31, 18, 18);
        private static RectangleF ACCESSORY3_INVENTORY_RECT = new RectangleF(206f, 50, 18, 18);
        private static Color CLOTHING_INDICATOR_COLOR = Color.Green * 0.5f;

        private static Vector2 HOTBAR_POSITION = new Vector2(57f, 157);
        private static Vector2 HOTBAR_SELECTED_POSITION_0 = new Vector2(63f, 158);
        private static Vector2 CHEST_INVENTORY_POSITION = INVENTORY_POSITION - new Vector2(0, 9);
        private static Vector2 INVENTORY_PLAYER_PREVIEW = new Vector2(48.5f, 0);
        private static Vector2 DATETIME_PANEL_POSITION = new Vector2(271, 2);
        private static Vector2 DATETIME_PANEL_DAYTEXT_OFFSET = new Vector2(1, 15);
        private static Vector2 DATETIME_PANEL_SEASONTEXT_OFFSET = new Vector2(2, 1);
        private static Vector2 DATETIME_PANEL_TIME_OFFSET = new Vector2(26, 16.5f);
        private static Vector2 DATETIME_PANEL_GOLD_OFFSET = new Vector2(35, 29);

        private string mouseLeftAction, mouseRightAction, mouseLeftShiftAction, mouseRightShiftAction;
        private static Vector2 MOUSE_CONTROL_POSITION = new Vector2(272, 155); 
        private static Vector2 MOUSE_LEFT_TEXT_POSITION = new Vector2(273, 170); 
        private static Vector2 MOUSE_RIGHT_TEXT_POSITION = new Vector2(302, 170); 

        private string upAction, leftAction, rightAction, downAction;
        private static Vector2 KEY_CONTROL_POSITION = new Vector2(15, 152); 
        private static Vector2 KEY_RIGHT_TEXT_POSITION = new Vector2(50, 153); 
        private static Vector2 KEY_LEFT_TEXT_POSITION = new Vector2(12, 153); 
        private static Vector2 KEY_UP_TEXT_POSITION = new Vector2(30, 148); 
        private static Vector2 KEY_DOWN_TEXT_POSITION = new Vector2(30, 176); 

        private static Vector2 MENU_CONTROL_POSITION = new Vector2(1, 49); 
        private static Vector2 MENU_BAG_HOTKEY_POSITION = new Vector2(16, 53); 
        private static Vector2 MENU_SCRAPBOOK_HOTKEY_POSITION = new Vector2(16, 66); 
        private static Vector2 MENU_CRAFTING_HOTKEY_POSITION = new Vector2(16, 79); 
        private static Vector2 MENU_SETTINGS_HOTKEY_POSITION = new Vector2(16, 92); 
        private static Vector2 MENU_EDITMODE_HOTKEY_POSITION = new Vector2(16, 105);
        private static Vector2 MENU_CYCLE_INVENTORY_HOTKEY_POSITION = new Vector2(16, 118);
        private static Vector2 MENU_DROP_HOTKEY_POSITION = new Vector2(16, 131);
        private Texture2D menuControlsInventoryEnlarge, menuControlsInventoryDepressed;
        private Texture2D menuControlsScrapbookEnlarge, menuControlsScrapbookDepressed;
        private Texture2D menuControlsSettingsEnlarge, menuControlsSettingsDepressed;
        private Texture2D menuControlsCraftingEnlarge, menuControlsCraftingDepressed;
        private Texture2D menuControlsEditModeEnlarge, menuControlsEditModeDepressed;
        private bool isMouseOverInventoryMC, isMouseOverScrapbookMC, isMouseOverCraftingMC, isMouseOverSettingsMC, isMouseOverEditModeMC;
        private static Vector2 BACKGROUND_BLACK_OFFSET = new Vector2(-9, -9);

        private static Vector2 SHIFT_CONTROL_POSITION = new Vector2(295, 143);
        private static Vector2 SHIFT_TEXT_POSITION = new Vector2(258, 138);

        private static Vector2 ESC_CONTROL_POSITION = new Vector2(295, 127f);
        private static Vector2 ESC_TEXT_POSITION = new Vector2(281, 129f);

        private static Vector2 EDIT_MODE_NOTIFICATION_TEXT = new Vector2(33, 133);

        private static Vector2 MENU_BUTTON_SIZE = new Vector2(13, 13);
        private static int MENU_DELTA_Y = 13;
        private RectangleF[] menuButtons;

        private static int HOTBAR_SELECTED_DELTA_X = 19;
        private static int INVENTORY_INITIAL_DELTA_X = 8;
        private static int INVENTORY_INITIAL_DELTA_Y = 65;
        private static int INVENTORY_ITEM_DELTA_X = 19;
        private static int INVENTORY_ITEM_DELTA_Y = 19;
        private ItemStack inventoryHeldItem;

        private ItemStack heldItem;

        private Vector2 playerPosition;
        private int displayGold;
        private static int ITEM_COLLECTED_TOOLTIP_Y_ADDITION = 16;
        private static float ITEM_COLLECTED_TOOLTIP_DELAY = 0.1f;
        private static float ITEM_COLLECTED_TOOLTIP_DELAY_FAST = 0.02f;
        private float timeSinceItemCollectedTooltipAdded = 0.0f;
        private bool itemCollectedTooltipFast = false;

        private Texture2D scrapbookBase;
        private Texture2D scrapbookTab1Active, scrapbookTab2Active, scrapbookTab3Active, scrapbookTab4Active, scrapbookTab5Active, scrapbookTab6Active, scrapbookTab7Active, scrapbookTab8Active, scrapbookTab9Active, scrapbookTab10Active, scrapbookTab11Active;
        private Texture2D scrapbookTab1Hover, scrapbookTab2Hover, scrapbookTab3Hover, scrapbookTab4Hover, scrapbookTab5Hover, scrapbookTab6Hover, scrapbookTab7Hover, scrapbookTab8Hover, scrapbookTab9Hover, scrapbookTab10Hover, scrapbookTab11Hover;
        private Texture2D scrapbookTab1ActiveHover, scrapbookTab2ActiveHover, scrapbookTab3ActiveHover, scrapbookTab4ActiveHover, scrapbookTab5ActiveHover, scrapbookTab6ActiveHover, scrapbookTab7ActiveHover, scrapbookTab8ActiveHover, scrapbookTab9ActiveHover, scrapbookTab10ActiveHover, scrapbookTab11ActiveHover;
        private Texture2D scrapbookTitleActive, scrapbookTitleHover, scrapbookTitleActiveHover;
        public static Vector2 SCRAPBOOK_POSITION = new Vector2(17, 6);
        private static Vector2 SCRAPBOOK_TAB1_POSITION = SCRAPBOOK_POSITION + new Vector2(-1, 24);
        private static Vector2 SCRAPBOOK_TAB2_POSITION = SCRAPBOOK_TAB1_POSITION + new Vector2(0, 10);
        private static Vector2 SCRAPBOOK_TAB3_POSITION = SCRAPBOOK_TAB2_POSITION + new Vector2(0, 10);
        private static Vector2 SCRAPBOOK_TAB4_POSITION = SCRAPBOOK_TAB3_POSITION + new Vector2(0, 10);
        private static Vector2 SCRAPBOOK_TAB5_POSITION = SCRAPBOOK_TAB4_POSITION + new Vector2(0, 10);
        private static Vector2 SCRAPBOOK_TAB6_POSITION = SCRAPBOOK_TAB5_POSITION + new Vector2(0, 10);
        private static Vector2 SCRAPBOOK_TAB7_POSITION = SCRAPBOOK_TAB6_POSITION + new Vector2(0, 10);
        private static Vector2 SCRAPBOOK_TAB8_POSITION = SCRAPBOOK_TAB7_POSITION + new Vector2(0, 10);
        private static Vector2 SCRAPBOOK_TAB9_POSITION = SCRAPBOOK_TAB8_POSITION + new Vector2(0, 10);
        private static Vector2 SCRAPBOOK_TAB10_POSITION = SCRAPBOOK_TAB9_POSITION + new Vector2(0, 10);
        private static Vector2 SCRAPBOOK_TAB11_POSITION = SCRAPBOOK_TAB10_POSITION + new Vector2(0, 10);
        private static Vector2 SCRAPBOOK_TITLE1_POSITION = SCRAPBOOK_POSITION + new Vector2(20, 26);
        private static Vector2 SCRAPBOOK_TITLE2_POSITION = SCRAPBOOK_TITLE1_POSITION + new Vector2(0, 11);
        private static Vector2 SCRAPBOOK_TITLE3_POSITION = SCRAPBOOK_TITLE2_POSITION + new Vector2(0, 11);
        private static Vector2 SCRAPBOOK_TITLE4_POSITION = SCRAPBOOK_TITLE3_POSITION + new Vector2(0, 11);
        private static Vector2 SCRAPBOOK_TITLE5_POSITION = SCRAPBOOK_TITLE4_POSITION + new Vector2(0, 11);
        private static Vector2 SCRAPBOOK_TITLE6_POSITION = SCRAPBOOK_TITLE5_POSITION + new Vector2(0, 11);
        private static Vector2 SCRAPBOOK_TITLE7_POSITION = SCRAPBOOK_TITLE6_POSITION + new Vector2(0, 11);
        private static Vector2 SCRAPBOOK_TITLE8_POSITION = SCRAPBOOK_TITLE7_POSITION + new Vector2(0, 11);
        private static Vector2 SCRAPBOOK_TITLE9_POSITION = SCRAPBOOK_TITLE8_POSITION + new Vector2(0, 11);
        private static Vector2 SCRAPBOOK_TITLE10_POSITION = SCRAPBOOK_TITLE9_POSITION + new Vector2(0, 11);
        private int scrapbookCurrentTab = 0;
        private int scrapbookCurrentPage = 0;
        private int scrapbookHoverTab = 0;
        private int scrapbookHoverPage = 0;
        private Dictionary<string, ScrapbookPage.Component> scrapbookDynamicComponents;
        private ScrapbookPage[] scrapbookPages;
        private RectangleF[] scrapbookTabs;
        private RectangleF[] scrapbookTitles;

        private static string SCRAPBOOK_CALENDAR_CURRENT_DAY = "calendarCurrentDay";

        private int selectedHotbarPosition; 

        private DialogueNode currentDialogue = null;

        private Texture2D exitPrompt;
        private Texture2D exitButtonEnlarge;
        private static Vector2 EXIT_PROMPT_POSITION = new Vector2(130, 13);
        private RectangleF exitPromptButton;

        private Texture2D settings, checkmark, checkmark_hover, 
            resolutionup_enlarge, resolutionup_disabled, resolutiondown_enlarge, resolutiondown_disabled, 
            sound_segment_end, sound_segment, sound_segment_farleft, sound_segment_end_farright, sound_segment_end_farleft,
            hotkey_active, hotkey_hover, hotkey_overlap, hotkey_lrud_active, hotkey_lrud_hover, hotkey_lrud_overlap;
        private static Vector2 SETTINGS_POSITION = new Vector2(63, 16);
        private static Vector2 SETTINGS_RESOLUTION_TEXT_POSITION = SETTINGS_POSITION + new Vector2(40, 38);
        private static RectangleF SETTINGS_KEYBIND_LEFT_POSITION = new RectangleF(SETTINGS_POSITION + new Vector2(116, 16), new Vector2(18, 10));
        private static RectangleF SETTINGS_KEYBIND_RIGHT_POSITION = new RectangleF(SETTINGS_POSITION + new Vector2(116, 27), new Vector2(18, 10));
        private static RectangleF SETTINGS_KEYBIND_UP_POSITION = new RectangleF(SETTINGS_POSITION + new Vector2(165, 16), new Vector2(18, 10));
        private static RectangleF SETTINGS_KEYBIND_DOWN_POSITION = new RectangleF(SETTINGS_POSITION + new Vector2(165, 27), new Vector2(18, 10));
        private static RectangleF SETTINGS_KEYBIND_INVENTORY_POSITION = new RectangleF(SETTINGS_POSITION + new Vector2(153, 43), new Vector2(30, 10));
        private static RectangleF SETTINGS_KEYBIND_SCRAPBOOK_POSITION = new RectangleF(SETTINGS_POSITION + new Vector2(153, 54), new Vector2(30, 10));
        private static RectangleF SETTINGS_KEYBIND_CRAFTING_POSITION = new RectangleF(SETTINGS_POSITION + new Vector2(153, 65), new Vector2(30, 10));
        private static RectangleF SETTINGS_KEYBIND_SETTINGS_POSITION = new RectangleF(SETTINGS_POSITION + new Vector2(153, 76), new Vector2(30, 10));
        private static RectangleF SETTINGS_KEYBIND_EDITMODE_POSITION = new RectangleF(SETTINGS_POSITION + new Vector2(153, 87), new Vector2(30, 10));
        private static RectangleF SETTINGS_KEYBIND_CYCLE_HOTBAR_POSITION = new RectangleF(SETTINGS_POSITION + new Vector2(153, 101), new Vector2(30, 10));
        private static RectangleF SETTINGS_KEYBIND_DISCARD_ITEM_POSITION = new RectangleF(SETTINGS_POSITION + new Vector2(153, 112), new Vector2(30, 10));
        private enum Rebinds
        {
            LEFT, RIGHT, UP, DOWN, INVENTORY, SCRAPBOOK, CRAFTING, SETTINGS, EDITMODE, CYCLE_HOTBAR, DISCARD_ITEM, NONE
        }
        private Rebinds currentRebind = Rebinds.NONE;


        private RectangleF[] settingsOtherRectangles;
        private RectangleF[] settingsResolutionRectangles;
        private RectangleF[] settingsSFXRectangles;
        private RectangleF[] settingsMusicRectangles;

        private Vector2 targetTile;
        private Entity targetEntity;
        private Entity targetEntityLastFrame;
        private Texture2D reticle;
        private bool drawReticle;

        private Texture2D garbageCanOpen, garbageCanClosed;
        private static Vector2 GARBAGE_CAN_POSITION = new Vector2(44, 124);
        private RectangleF garbageCanRectangle;
        private RectangleF[] dropRectangles;

        private static string selectedHotbarItemName;
        private static Vector2 SELECTED_HOTBAR_ITEM_NAME_POSITION = new Vector2(160, 156);

        private AnimatedSprite dialogueBox, bounceArrow;
        private static float DIALOGUE_BOX_ANIMATION_LENGTH = 0.033f; //0.33 per 4 frames
        private bool inDialogue;
        private float currentDialogueNumChars;
        private static float DIALOGUE_SPEED_CHARS_PER_FRAME = 0.6f; //speed of dialogue
        private static Vector2 DIALOGUE_BOX_POSITION = new Vector2(57f, 8);
        private static Vector2 DIALOGUE_PORTRAIT_POSITION = DIALOGUE_BOX_POSITION + new Vector2(7, 8);
        private static Vector2 DIALOGUE_TEXT_POSITION = DIALOGUE_BOX_POSITION + new Vector2(50, 8);
        private static Vector2 DIALOGUE_BOUNCE_ARROW_POSITION = DIALOGUE_BOX_POSITION + new Vector2(188, 37);
        private static Vector2 KEY_CONTROL_POSITION_DIALOGUE = new Vector2(DIALOGUE_BOX_POSITION.X + 91, DIALOGUE_BOX_POSITION.Y + 62);
        private static Vector2 KEY_RIGHT_TEXT_POSITION_DIALOGUE = new Vector2(KEY_CONTROL_POSITION_DIALOGUE.X + 52, KEY_CONTROL_POSITION_DIALOGUE.Y + 9);
        private static Vector2 KEY_LEFT_TEXT_POSITION_DIALOGUE = new Vector2(KEY_CONTROL_POSITION_DIALOGUE.X - 25, KEY_CONTROL_POSITION_DIALOGUE.Y + 9);
        private static Vector2 KEY_UP_TEXT_POSITION_DIALOGUE = new Vector2(KEY_CONTROL_POSITION_DIALOGUE.X + 15, KEY_CONTROL_POSITION_DIALOGUE.Y - 8);
        private static Vector2 KEY_DOWN_TEXT_POSITION_DIALOGUE = new Vector2(KEY_CONTROL_POSITION_DIALOGUE.X + 15, KEY_CONTROL_POSITION_DIALOGUE.Y + 28);

        private static int TRANSITION_DELTA_Y = 500;
        private static int TRANSITION_DELTA_X = 725;
        private static float TRANSITION_ALPHA_SPEED = 2.7f;
        private Vector2 transitionPosition;
        private float transitionAlpha;

        private bool isMouseRightDown, isMouseLeftDown;
        private Texture2D mouseLeftDown, mouseRightDown;

        private static Texture2D workbench;
        private static Texture2D workbenchClothingTab, workbenchFloorWallTab, workbenchScaffoldingTab, workbenchFurnitureTab, workbenchMachineTab;
        private static Texture2D workbenchClothingTabHover, workbenchFloorWallTabHover, workbenchScaffoldingTabHover, workbenchFurnitureTabHover, workbenchMachineTabHover;
        private static Texture2D workbenchArrowLeft, workbenchArrowRight, workbenchCraftButton, workbenchCraftButtonEnlarged, workbenchQuestionMark, workbenchBlueprintDepression;

        private static Vector2 WORKBENCH_POSITION = new Vector2(50, 14);
        private static Vector2 WORKBENCH_MACHINE_TAB_POSITION = WORKBENCH_POSITION + new Vector2(-1, 27);
        private static Vector2 WORKBENCH_SCAFFOLDING_TAB_POSITION = WORKBENCH_MACHINE_TAB_POSITION + new Vector2(0, 12);
        private static Vector2 WORKBENCH_FURNITURE_TAB_POSITION = WORKBENCH_SCAFFOLDING_TAB_POSITION + new Vector2(0, 12);
        private static Vector2 WORKBENCH_HOUSE_TAB_POSITION = WORKBENCH_FURNITURE_TAB_POSITION + new Vector2(0, 12);
        private static Vector2 WORKBENCH_CLOTHING_TAB_POSITION = WORKBENCH_HOUSE_TAB_POSITION + new Vector2(0, 12);
        private static Vector2 WORKBENCH_LEFT_ARROW_POSITION = WORKBENCH_POSITION + new Vector2(16, 92);
        private static Vector2 WORKBENCH_RIGHT_ARROW_POSITION = WORKBENCH_LEFT_ARROW_POSITION + new Vector2(91, 0);
        private static Vector2 WORKBENCH_CRAFT_BUTTON = WORKBENCH_POSITION + new Vector2(156, 98);
        private static Vector2 WORKBENCH_BLUEPRINT_POSITION = WORKBENCH_POSITION + new Vector2(19, 26);
        private static Vector2 WORKBENCH_PAGE_NAME_POSITION = WORKBENCH_POSITION + new Vector2(67, 103);
        private static Vector2 WORKBENCH_SELECTED_RECIPE_POSITION = WORKBENCH_POSITION + new Vector2(162, 18);
        private static Vector2 WORKBENCH_SELECTED_RECIPE_COMPONENT_1 = WORKBENCH_POSITION + new Vector2(132, 48);
        private static Vector2 WORKBENCH_SELECTED_RECIPE_COMPONENT_2 = WORKBENCH_SELECTED_RECIPE_COMPONENT_1 + new Vector2(20, 0);
        private static Vector2 WORKBENCH_SELECTED_RECIPE_COMPONENT_3 = WORKBENCH_SELECTED_RECIPE_COMPONENT_2 + new Vector2(20, 0);
        private static Vector2 WORKBENCH_SELECTED_RECIPE_COMPONENT_4 = WORKBENCH_SELECTED_RECIPE_COMPONENT_3 + new Vector2(20, 0);
        private static int blueprintDeltaX = 20, blueprintDeltaY = 20, haveBoxesDeltaY = 28;
        private bool hoveringLeftArrow, hoveringRightArrow, hoveringCraftButton;
        private int workbenchCurrentTab, workbenchCurrentPage, workbenchHoverTab;
        private RectangleF[] workbenchTabRectangles;
        private RectangleF[] workbenchBlueprintRectangles;
        private RectangleF workbenchLeftArrowRectangle, workbenchRightArrowRectangle, workbenchCraftButtonRectangle;
        private Vector2 workbenchInventorySelectedPosition;
        private List<Vector2> workbenchCraftablePosition;
        private GameState.CraftingRecipe[] currentRecipes;
        private int selectedRecipeSlot;
        private GameState.CraftingRecipe selectedRecipe;
        private int[] numMaterialsOfRecipe;
        private List<HealthBar> healthBars;

        private static Vector2 NOTIFICATION_POSITION = new Vector2(160, 12);
        private EntityPlayer.Notification currentNotification;

        private static Vector2 APPLIED_EFFECT_ANCHOR = new Vector2(3, 3); //used to be 2, 16 with AREA_NAME
        private static float APPLIED_EFFECT_DELTA_X = 14.0f;
        private List<EntityPlayer.TimedEffect> appliedEffects;

        private static Vector2 AREA_NAME_POSITION = new Vector2(9, 5);
        private string areaName, zoneName;

        private bool isHidden;

        private float HOVERING_INTERFACE_MAX_OPACITY = 0.9f;
        private float HOVERING_INTERFACE_OPACITY_SPEED = 4.8f;
        private float hoveringInterfaceOpacity = 0.0f;
        private HoveringInterface currentHoveringInterface;
        private Vector2 hoveringInterfacePosition = new Vector2(-100, -100);

        private bool editMode = false;
        private enum RemovalMode
        {
            ANY, PLACEABLE, SCAFFOLDING_AND_WALLPAPER
        }
        private RemovalMode removalMode = RemovalMode.ANY;

        public static List<QueuedString> QUEUED_STRINGS;

        private int dialogueNodePage = 0;

        private EntityPlayer player;

        public GameplayInterface(Controller controller)
        {
            this.healthBars = new List<HealthBar>();
            this.dialogueNodePage = 0;
            this.currentHoveringInterface = null;
            QUEUED_STRINGS = new List<QueuedString>();
            this.areaName = "";
            this.zoneName = "";
            this.isHidden = false;
            this.currentNotification = null;
            this.selectedRecipeSlot = -1;
            this.numMaterialsOfRecipe = new int[4];
            for (int i = 0; i < 4; i++)
            {
                this.numMaterialsOfRecipe[i] = 0;
            }
            this.selectedRecipe = null;
            this.currentRecipes = new GameState.CraftingRecipe[15];
            this.workbenchInventorySelectedPosition = new Vector2(-1000, -1000);
            this.hoveringLeftArrow = false;
            this.hoveringRightArrow = false;
            this.workbenchHoverTab = -1;
            this.workbenchCurrentPage = 0;
            this.workbenchCurrentTab = 0;
            this.transitionPosition = new Vector2(0, 0);
            this.transitionAlpha = 1.0f;
            this.displayGold = -1;
            this.workbenchCraftablePosition = new List<Vector2>();
            this.currentDialogueNumChars = 0;
            this.paused = false;
            this.controller = controller;
            this.selectedHotbarPosition = 0;
            this.inventorySelectedPosition = new Vector2(-100, -100);
            this.inventoryItems = new ItemStack[EntityPlayer.INVENTORY_SIZE];
            this.chestInventory = new ItemStack[PEntityChest.INVENTORY_SIZE];
            numbers = new Texture2D[10];
            numbersNoBorder = new Texture2D[10];
            this.seasonText = new Texture2D[4];
            this.dayText = new Texture2D[7];
            this.tooltipName = "";
            this.tooltipDescription = "";
            this.collectedTooltips = new List<CollectedTooltip>();
            this.menuButtons = new RectangleF[5];
            this.targetTile = new Vector2(-100, -100);
            this.drawReticle = true;
            this.lastPlacedTile = new Vector2(-100, -100);
            this.isMouseLeftDown = false;
            this.isMouseRightDown = false;
            this.isWDown = false;
            this.isADown = false;
            this.isDDown = false;
            this.isSDown = false;
            this.isMouseOverCraftingMC = false;
            this.isMouseOverInventoryMC = false;
            this.isMouseOverScrapbookMC = false;
            this.isMouseOverSettingsMC = false;
            this.isMouseOverEditModeMC = false;
            mouseLeftAction = "";
            mouseRightAction = "";
            mouseLeftShiftAction = "";
            mouseRightShiftAction = "";
            leftAction = "";
            rightAction = "";
            downAction = "";
            upAction = "";
        }

        public void LoadContent(ContentManager content)
        {
            craftingButtonSolo = content.Load<Texture2D>(Paths.INTERFACE_CRAFTING_BUTTON_SOLO);
            inventoryHeldItem = new ItemStack(ItemDict.NONE, 0);
            inventory_selected = content.Load<Texture2D>(Paths.INTERFACE_INVENTORY_SELECTED);

            TOOLTIP_9SLICE = new NineSlice(content.Load<Texture2D>(Paths.INTERFACE_9SLICE_TOPLEFT_TOOLTIP), content.Load<Texture2D>(Paths.INTERFACE_9SLICE_TOPRIGHT_TOOLTIP), content.Load<Texture2D>(Paths.INTERFACE_9SLICE_TOPCENTER_TOOLTIP),
                content.Load<Texture2D>(Paths.INTERFACE_9SLICE_MIDDLELEFT_TOOLTIP), content.Load<Texture2D>(Paths.INTERFACE_9SLICE_MIDDLERIGHT_TOOLTIP), content.Load<Texture2D>(Paths.INTERFACE_9SLICE_MIDDLECENTER_TOOLTIP),
                content.Load<Texture2D>(Paths.INTERFACE_9SLICE_BOTTOMLEFT_TOOLTIP), content.Load<Texture2D>(Paths.INTERFACE_9SLICE_BOTTOMRIGHT_TOOLTIP), content.Load<Texture2D>(Paths.INTERFACE_9SLICE_BOTTOMCENTER_TOOLTIP), false);
            DIALOGUE_9SLICE = new NineSlice(content.Load<Texture2D>(Paths.INTERFACE_9SLICE_TOPLEFT_DIALOGUE), content.Load<Texture2D>(Paths.INTERFACE_9SLICE_TOPRIGHT_DIALOGUE), content.Load<Texture2D>(Paths.INTERFACE_9SLICE_TOPCENTER_DIALOGUE),
                content.Load<Texture2D>(Paths.INTERFACE_9SLICE_MIDDLELEFT_DIALOGUE), content.Load<Texture2D>(Paths.INTERFACE_9SLICE_MIDDLERIGHT_DIALOGUE), content.Load<Texture2D>(Paths.INTERFACE_9SLICE_MIDDLECENTER_DIALOGUE),
                content.Load<Texture2D>(Paths.INTERFACE_9SLICE_BOTTOMLEFT_DIALOGUE), content.Load<Texture2D>(Paths.INTERFACE_9SLICE_BOTTOMRIGHT_DIALOGUE), content.Load<Texture2D>(Paths.INTERFACE_9SLICE_BOTTOMCENTER_DIALOGUE), false);
            THOUGHTS_9SLICE = new NineSlice(content.Load<Texture2D>(Paths.INTERFACE_9SLICE_TOPLEFT_THOUGHTS), content.Load<Texture2D>(Paths.INTERFACE_9SLICE_TOPRIGHT_THOUGHTS), content.Load<Texture2D>(Paths.INTERFACE_9SLICE_TOPCENTER_THOUGHTS),
                            content.Load<Texture2D>(Paths.INTERFACE_9SLICE_MIDDLELEFT_THOUGHTS), content.Load<Texture2D>(Paths.INTERFACE_9SLICE_MIDDLERIGHT_THOUGHTS), content.Load<Texture2D>(Paths.INTERFACE_9SLICE_MIDDLECENTER_THOUGHTS),
                            content.Load<Texture2D>(Paths.INTERFACE_9SLICE_BOTTOMLEFT_THOUGHTS), content.Load<Texture2D>(Paths.INTERFACE_9SLICE_BOTTOMRIGHT_THOUGHTS), content.Load<Texture2D>(Paths.INTERFACE_9SLICE_BOTTOMCENTER_THOUGHTS), false);
            INTERFACE_9SLICE = new NineSlice(content.Load<Texture2D>(Paths.INTERFACE_9SLICE_TOPLEFT_INTERFACE), content.Load<Texture2D>(Paths.INTERFACE_9SLICE_TOPRIGHT_INTERFACE), content.Load<Texture2D>(Paths.INTERFACE_9SLICE_TOPCENTER_INTERFACE),
                            content.Load<Texture2D>(Paths.INTERFACE_9SLICE_MIDDLELEFT_INTERFACE), content.Load<Texture2D>(Paths.INTERFACE_9SLICE_MIDDLERIGHT_INTERFACE), content.Load<Texture2D>(Paths.INTERFACE_9SLICE_MIDDLECENTER_INTERFACE),
                            content.Load<Texture2D>(Paths.INTERFACE_9SLICE_BOTTOMLEFT_INTERFACE), content.Load<Texture2D>(Paths.INTERFACE_9SLICE_BOTTOMRIGHT_INTERFACE), content.Load<Texture2D>(Paths.INTERFACE_9SLICE_BOTTOMCENTER_INTERFACE), false);
            WHITE_9SLICE = new NineSlice(content.Load<Texture2D>(Paths.INTERFACE_9SLICE_TOPLEFT_WHITE), content.Load<Texture2D>(Paths.INTERFACE_9SLICE_TOPRIGHT_WHITE), content.Load<Texture2D>(Paths.INTERFACE_9SLICE_TOPCENTER_WHITE),
                            content.Load<Texture2D>(Paths.INTERFACE_9SLICE_MIDDLELEFT_WHITE), content.Load<Texture2D>(Paths.INTERFACE_9SLICE_MIDDLERIGHT_WHITE), content.Load<Texture2D>(Paths.INTERFACE_9SLICE_MIDDLECENTER_WHITE),
                            content.Load<Texture2D>(Paths.INTERFACE_9SLICE_BOTTOMLEFT_WHITE), content.Load<Texture2D>(Paths.INTERFACE_9SLICE_BOTTOMRIGHT_WHITE), content.Load<Texture2D>(Paths.INTERFACE_9SLICE_BOTTOMCENTER_WHITE), true);

            hotbar = content.Load<Texture2D>(Paths.INTERFACE_HOTBAR);
            hotbar_selected = content.Load<Texture2D>(Paths.INTERFACE_HOTBAR_SELECTED);
            numbers[0] = content.Load<Texture2D>(Paths.INTERFACE_NUMBER_ZERO);
            numbers[1] = content.Load<Texture2D>(Paths.INTERFACE_NUMBER_ONE);
            numbers[2] = content.Load<Texture2D>(Paths.INTERFACE_NUMBER_TWO);
            numbers[3] = content.Load<Texture2D>(Paths.INTERFACE_NUMBER_THREE);
            numbers[4] = content.Load<Texture2D>(Paths.INTERFACE_NUMBER_FOUR);
            numbers[5] = content.Load<Texture2D>(Paths.INTERFACE_NUMBER_FIVE);
            numbers[6] = content.Load<Texture2D>(Paths.INTERFACE_NUMBER_SIX);
            numbers[7] = content.Load<Texture2D>(Paths.INTERFACE_NUMBER_SEVEN);
            numbers[8] = content.Load<Texture2D>(Paths.INTERFACE_NUMBER_EIGHT);
            numbers[9] = content.Load<Texture2D>(Paths.INTERFACE_NUMBER_NINE);
            numbersNoBorder[0] = content.Load<Texture2D>(Paths.INTERFACE_NUMBER_ZERO_NO_BORDER);
            numbersNoBorder[1] = content.Load<Texture2D>(Paths.INTERFACE_NUMBER_ONE_NO_BORDER);
            numbersNoBorder[2] = content.Load<Texture2D>(Paths.INTERFACE_NUMBER_TWO_NO_BORDER);
            numbersNoBorder[3] = content.Load<Texture2D>(Paths.INTERFACE_NUMBER_THREE_NO_BORDER);
            numbersNoBorder[4] = content.Load<Texture2D>(Paths.INTERFACE_NUMBER_FOUR_NO_BORDER);
            numbersNoBorder[5] = content.Load<Texture2D>(Paths.INTERFACE_NUMBER_FIVE_NO_BORDER);
            numbersNoBorder[6] = content.Load<Texture2D>(Paths.INTERFACE_NUMBER_SIX_NO_BORDER);
            numbersNoBorder[7] = content.Load<Texture2D>(Paths.INTERFACE_NUMBER_SEVEN_NO_BORDER);
            numbersNoBorder[8] = content.Load<Texture2D>(Paths.INTERFACE_NUMBER_EIGHT_NO_BORDER);
            numbersNoBorder[9] = content.Load<Texture2D>(Paths.INTERFACE_NUMBER_NINE_NO_BORDER);
            itemBox = content.Load<Texture2D>(Paths.INTERFACE_ITEM_BOX);
            hoveringItemBox = content.Load<Texture2D>(Paths.INTERFACE_HOVERING_ITEM_BOX);
            inventory = content.Load<Texture2D>(Paths.INTERFACE_INVENTORY);
            black_background = content.Load<Texture2D>(Paths.INTERFACE_BACKGROUND_BLACK);
            black_box = content.Load<Texture2D>(Paths.INTERFACE_BLACK_BOX);
            grid = content.Load<Texture2D>(Paths.INTERFACE_GRID);
            editMode = false;

            dateTimePanel = content.Load<Texture2D>(Paths.INTERFACE_DATETIME_PANEL);
            seasonText[0] = content.Load<Texture2D>(Paths.INTERFACE_TEXT_SPRING);
            seasonText[1] = content.Load<Texture2D>(Paths.INTERFACE_TEXT_SUMMER);
            seasonText[2] = content.Load<Texture2D>(Paths.INTERFACE_TEXT_AUTUMN);
            seasonText[3] = content.Load<Texture2D>(Paths.INTERFACE_TEXT_WINTER);
            dayText[0] = content.Load<Texture2D>(Paths.INTERFACE_TEXT_MON);
            dayText[1] = content.Load<Texture2D>(Paths.INTERFACE_TEXT_TUE);
            dayText[2] = content.Load<Texture2D>(Paths.INTERFACE_TEXT_WED);
            dayText[3] = content.Load<Texture2D>(Paths.INTERFACE_TEXT_THU);
            dayText[4] = content.Load<Texture2D>(Paths.INTERFACE_TEXT_FRI);
            dayText[5] = content.Load<Texture2D>(Paths.INTERFACE_TEXT_SAT);
            dayText[6] = content.Load<Texture2D>(Paths.INTERFACE_TEXT_SUN);

            mouseControl = content.Load<Texture2D>(Paths.INTERFACE_MOUSE_CONTROLS);
            keyControl = content.Load<Texture2D>(Paths.INTERFACE_KEY_CONTROLS);
            keyControlWDown = content.Load<Texture2D>(Paths.INTERFACE_KEY_CONTROLS_W_DOWN);
            keyControlADown = content.Load<Texture2D>(Paths.INTERFACE_KEY_CONTROLS_A_DOWN);
            keyControlSDown = content.Load<Texture2D>(Paths.INTERFACE_KEY_CONTROLS_S_DOWN);
            keyControlDDown = content.Load<Texture2D>(Paths.INTERFACE_KEY_CONTROLS_D_DOWN);
            menuControl = content.Load<Texture2D>(Paths.INTERFACE_MENU_CONTROLS);

            shiftOnPressed = content.Load<Texture2D>(Paths.INTERFACE_SHIFT_ON_PRESSED);
            shiftOnUnpressed = content.Load<Texture2D>(Paths.INTERFACE_SHIFT_ON_UNPRESSED);
            shiftOffPressed = content.Load<Texture2D>(Paths.INTERFACE_SHIFT_OFF_PRESSED);
            shiftOffUnpressed = content.Load<Texture2D>(Paths.INTERFACE_SHIFT_OFF_UNPRESSED);

            escPressed = content.Load<Texture2D>(Paths.INTERFACE_ESC_PRESSED);
            escUnpressed = content.Load<Texture2D>(Paths.INTERFACE_ESC_UNPRESSED);

            chest_inventory = content.Load<Texture2D>(Paths.INTERFACE_CHEST);
            chest_inventory_greyscale = content.Load<Texture2D>(Paths.INTERFACE_CHEST_GREYSCALE);

            //math out itemRectangles...
            itemRectangles = new RectangleF[EntityPlayer.INVENTORY_SIZE];

            for (int i = 0; i < GameplayInterface.HOTBAR_LENGTH; i++)
            {
                Vector2 itemPosition = new Vector2(HOTBAR_POSITION.X + 7 + (i * 19), HOTBAR_POSITION.Y + 2);
                itemRectangles[i] = new RectangleF(itemPosition.X, itemPosition.Y, 18, 18);
            }

            float startingX = INVENTORY_POSITION.X + INVENTORY_INITIAL_DELTA_X - 1;
            Vector2 location = new Vector2(startingX, INVENTORY_POSITION.Y + INVENTORY_INITIAL_DELTA_Y - 1);

            for (int i = 10; i < EntityPlayer.INVENTORY_SIZE; i++)
            {
                itemRectangles[i] = new RectangleF(location.X, location.Y, 18, 18);

                location.X += INVENTORY_ITEM_DELTA_X;
                if (i != 10 && (i + 1) % 10 == 0)
                {
                    location.Y += INVENTORY_ITEM_DELTA_Y;
                    location.X = startingX;
                }
            }

            //math out chest rectangles
            chestRectangles = new RectangleF[PEntityChest.INVENTORY_SIZE];
            startingX = CHEST_INVENTORY_POSITION.X + 7;
            location = new Vector2(startingX, CHEST_INVENTORY_POSITION.Y + 11);
            for(int i = 0; i < PEntityChest.INVENTORY_SIZE; i++)
            {
                chestRectangles[i] = new RectangleF(location.X, location.Y, 18, 18);
                location.X += INVENTORY_ITEM_DELTA_X;
                if(i != 0 && (i+1) % 10 == 0)
                {
                    location.Y += INVENTORY_ITEM_DELTA_Y;
                    location.X = startingX;
                }
            }

            InitializeScrapbook(content);

            exitPrompt = content.Load<Texture2D>(Paths.INTERFACE_EXIT_PROMPT);
            exitPromptButton = new RectangleF(EXIT_PROMPT_POSITION + new Vector2(31, 21), new Vector2(22, 10));
            exitButtonEnlarge = content.Load<Texture2D>(Paths.INTERFACE_EXIT_BUTTON_ENLARGE);

            settings = content.Load<Texture2D>(Paths.INTERFACE_SETTINGS);
            checkmark = content.Load<Texture2D>(Paths.INTERFACE_CHECKMARK);
            checkmark_hover = content.Load<Texture2D>(Paths.INTERFACE_CHECKMARK_HOVER);
            resolutionup_enlarge = content.Load<Texture2D>(Paths.INTERFACE_SETTINGS_RESOLUTIONUP_ENLARGE);
            resolutionup_disabled = content.Load<Texture2D>(Paths.INTERFACE_SETTINGS_RESOLUTIONUP_DISABLED);
            resolutiondown_enlarge = content.Load<Texture2D>(Paths.INTERFACE_SETTINGS_RESOLUTIONDOWN_ENLARGE);
            resolutiondown_disabled = content.Load<Texture2D>(Paths.INTERFACE_SETTINGS_RESOLUTIONDOWN_DISABLED);
            sound_segment = content.Load<Texture2D>(Paths.INTERFACE_SETTINGS_SOUND_SEGMENT);
            sound_segment_end = content.Load<Texture2D>(Paths.INTERFACE_SETTINGS_SOUND_SEGMENT_END);
            sound_segment_end_farleft = content.Load<Texture2D>(Paths.INTERFACE_SETTINGS_SOUND_SEGMENT_END_FARLEFT);
            sound_segment_end_farright = content.Load<Texture2D>(Paths.INTERFACE_SETTINGS_SOUND_SEGMENT_END_FARRIGHT);
            sound_segment_farleft = content.Load<Texture2D>(Paths.INTERFACE_SETTINGS_SOUND_SEGMENT_FARLEFT);
            hotkey_active = content.Load<Texture2D>(Paths.INTERFACE_SETTINGS_HOTKEY_ACTIVE);
            hotkey_hover = content.Load<Texture2D>(Paths.INTERFACE_SETTINGS_HOTKEY_HOVER);
            hotkey_overlap = content.Load<Texture2D>(Paths.INTERFACE_SETTINGS_HOTKEY_OVERLAP);
            hotkey_lrud_active = content.Load<Texture2D>(Paths.INTERFACE_SETTINGS_HOTKEY_LRUD_ACTIVE);
            hotkey_lrud_hover = content.Load<Texture2D>(Paths.INTERFACE_SETTINGS_HOTKEY_LRUD_HOVER);
            hotkey_lrud_overlap = content.Load<Texture2D>(Paths.INTERFACE_SETTINGS_HOTKEY_LRUD_OVERLAP);


            settingsOtherRectangles = new RectangleF[4];
            settingsOtherRectangles[0] = new RectangleF(SETTINGS_POSITION + new Vector2(6, 84), new Vector2(8, 8)); //HIDE CONTROLS
            settingsOtherRectangles[1] = new RectangleF(new Vector2(settingsOtherRectangles[0].X, settingsOtherRectangles[0].Y + 10), new Vector2(8, 8)); //HIDE EDIT GRID
            settingsOtherRectangles[2] = new RectangleF(new Vector2(settingsOtherRectangles[1].X, settingsOtherRectangles[1].Y + 10), new Vector2(8, 8)); //HIDE RETICLE
            settingsOtherRectangles[3] = new RectangleF(new Vector2(settingsOtherRectangles[2].X, settingsOtherRectangles[2].Y + 10), new Vector2(8, 8)); //WINDOWED

            settingsResolutionRectangles = new RectangleF[2];
            settingsResolutionRectangles[0] = new RectangleF(SETTINGS_POSITION + new Vector2(7, 28), new Vector2(9, 9)); //increase
            settingsResolutionRectangles[1] = new RectangleF(new Vector2(settingsResolutionRectangles[0].X + 55  , settingsResolutionRectangles[0].Y), new Vector2(9, 9)); //decreases

            settingsSFXRectangles = new RectangleF[10];
            settingsSFXRectangles[0] = new RectangleF(SETTINGS_POSITION + new Vector2(21, 51), new Vector2(5, 7));
            settingsSFXRectangles[1] = new RectangleF(new Vector2(settingsSFXRectangles[0].X + 5, settingsSFXRectangles[0].Y + 1), new Vector2(5, 5));
            settingsSFXRectangles[2] = new RectangleF(new Vector2(settingsSFXRectangles[1].X + 5, settingsSFXRectangles[1].Y), new Vector2(5, 5));
            settingsSFXRectangles[3] = new RectangleF(new Vector2(settingsSFXRectangles[2].X + 5, settingsSFXRectangles[2].Y), new Vector2(5, 5));
            settingsSFXRectangles[4] = new RectangleF(new Vector2(settingsSFXRectangles[3].X + 5, settingsSFXRectangles[3].Y), new Vector2(5, 5));
            settingsSFXRectangles[5] = new RectangleF(new Vector2(settingsSFXRectangles[4].X + 5, settingsSFXRectangles[4].Y), new Vector2(5, 5));
            settingsSFXRectangles[6] = new RectangleF(new Vector2(settingsSFXRectangles[5].X + 5, settingsSFXRectangles[5].Y), new Vector2(5, 5));
            settingsSFXRectangles[7] = new RectangleF(new Vector2(settingsSFXRectangles[6].X + 5, settingsSFXRectangles[6].Y), new Vector2(5, 5));
            settingsSFXRectangles[8] = new RectangleF(new Vector2(settingsSFXRectangles[7].X + 5, settingsSFXRectangles[7].Y), new Vector2(5, 5));
            settingsSFXRectangles[9] = new RectangleF(new Vector2(settingsSFXRectangles[8].X + 5, settingsSFXRectangles[8].Y-1), new Vector2(5, 7));

            settingsMusicRectangles = new RectangleF[10];
            settingsMusicRectangles[0] = new RectangleF(SETTINGS_POSITION + new Vector2(21, 61), new Vector2(5, 7));
            settingsMusicRectangles[1] = new RectangleF(new Vector2(settingsMusicRectangles[0].X + 5, settingsMusicRectangles[0].Y + 1), new Vector2(5, 5));
            settingsMusicRectangles[2] = new RectangleF(new Vector2(settingsMusicRectangles[1].X + 5, settingsMusicRectangles[1].Y), new Vector2(5, 5));
            settingsMusicRectangles[3] = new RectangleF(new Vector2(settingsMusicRectangles[2].X + 5, settingsMusicRectangles[2].Y), new Vector2(5, 5));
            settingsMusicRectangles[4] = new RectangleF(new Vector2(settingsMusicRectangles[3].X + 5, settingsMusicRectangles[3].Y), new Vector2(5, 5));
            settingsMusicRectangles[5] = new RectangleF(new Vector2(settingsMusicRectangles[4].X + 5, settingsMusicRectangles[4].Y), new Vector2(5, 5));
            settingsMusicRectangles[6] = new RectangleF(new Vector2(settingsMusicRectangles[5].X + 5, settingsMusicRectangles[5].Y), new Vector2(5, 5));
            settingsMusicRectangles[7] = new RectangleF(new Vector2(settingsMusicRectangles[6].X + 5, settingsMusicRectangles[6].Y), new Vector2(5, 5));
            settingsMusicRectangles[8] = new RectangleF(new Vector2(settingsMusicRectangles[7].X + 5, settingsMusicRectangles[7].Y), new Vector2(5, 5));
            settingsMusicRectangles[9] = new RectangleF(new Vector2(settingsMusicRectangles[8].X + 5, settingsMusicRectangles [8].Y - 1), new Vector2(5, 7));

            reticle = content.Load<Texture2D>(Paths.INTERFACE_RETICLE);

            garbageCanClosed = content.Load<Texture2D>(Paths.INTERFACE_GARBAGE_CAN_CLOSED);
            garbageCanOpen = content.Load<Texture2D>(Paths.INTERFACE_GARBAGE_CAN_OPEN);
            garbageCanRectangle = new RectangleF(GARBAGE_CAN_POSITION, new Vector2(garbageCanClosed.Width, garbageCanClosed.Height));
            dropRectangles = new RectangleF[] {
                new RectangleF(0, 0, 61, 200), //left side
                new RectangleF(61, 0, 28, 73), //left upper
                new RectangleF(256, 0, 65, 200), //right side
                new RectangleF(227, 0, 29, 73), //right upper
                new RectangleF(89, 0, 138, 11), //top
                new RectangleF(61, 152, 195, 6) //bottom between inv & hotbar
            };

            float[] frameLengths = Util.CreateAndFillArray(4, DIALOGUE_BOX_ANIMATION_LENGTH);
            dialogueBox = new AnimatedSprite(content.Load<Texture2D>(Paths.INTERFACE_DIALOGUE_BOX), 4, 1, 4, frameLengths);
            dialogueBox.AddLoop("anim", 0, 3, false);
            dialogueBox.AddLoop("close", 0, 3, false, true);
            dialogueBox.SetLoop("anim");

            frameLengths = Util.CreateAndFillArray(5, 0.1f);
            frameLengths[0] = 0.5f;
            bounceArrow = new AnimatedSprite(content.Load<Texture2D>(Paths.INTERFACE_BOUNCE_ARROW), 5, 1, 5, frameLengths);
            bounceArrow.AddLoop("anim", 0, 4);
            bounceArrow.SetLoop("anim");

            mouseLeftDown = content.Load<Texture2D>(Paths.INTERFACE_MOUSE_LEFT_DOWN);
            mouseRightDown = content.Load<Texture2D>(Paths.INTERFACE_MOUSE_RIGHT_DOWN);

            menuControlsCraftingDepressed = content.Load<Texture2D>(Paths.INTERFACE_MENU_CONTROLS_CRAFTING_DEPRESSED);
            menuControlsCraftingEnlarge = content.Load<Texture2D>(Paths.INTERFACE_MENU_CONTROLS_CRAFTING_ENLARGE);
            menuControlsSettingsDepressed = content.Load<Texture2D>(Paths.INTERFACE_MENU_CONTROLS_SETTINGS_DEPRESSED);
            menuControlsSettingsEnlarge = content.Load<Texture2D>(Paths.INTERFACE_MENU_CONTROLS_SETTINGS_ENLARGE);
            menuControlsInventoryDepressed = content.Load<Texture2D>(Paths.INTERFACE_MENU_CONTROLS_INVENTORY_DEPRESSED);
            menuControlsInventoryEnlarge = content.Load<Texture2D>(Paths.INTERFACE_MENU_CONTROLS_INVENTORY_ENLARGE);
            menuControlsScrapbookDepressed = content.Load<Texture2D>(Paths.INTERFACE_MENU_CONTROLS_SCRAPBOOK_DEPRESSED);
            menuControlsScrapbookEnlarge = content.Load<Texture2D>(Paths.INTERFACE_MENU_CONTROLS_SCRAPBOOK_ENLARGE);
            menuControlsEditModeEnlarge = content.Load<Texture2D>(Paths.INTERFACE_MENU_CONTROLS_EDITMODE_ENLARGE);
            menuControlsEditModeDepressed = content.Load<Texture2D>(Paths.INTERFACE_MENU_CONTROLS_EDITMODE_DEPRESSED);

            /*
             * 
             *         private static Texture2D workbench;
        private static Texture2D workbenchClothingTab, workbenchClothingTab2, workbenchHouseTab, workbenchScaffoldingTab, workbenchFurnitureTab;
        private static Texture2D workbenchArrowLeft, workbenchArrowRight, workbenchCraftButton;
             */
            workbench = content.Load<Texture2D>(Paths.INTERFACE_CRAFTING);
            workbenchClothingTab = content.Load<Texture2D>(Paths.INTERFACE_CRAFTING_TAB_CLOTHING);
            workbenchFloorWallTab = content.Load<Texture2D>(Paths.INTERFACE_CRAFTING_TAB_FLOOR_WALL);
            workbenchScaffoldingTab = content.Load<Texture2D>(Paths.INTERFACE_CRAFTING_TAB_SCAFFOLDING);
            workbenchFurnitureTab = content.Load<Texture2D>(Paths.INTERFACE_CRAFTING_TAB_FURNITURE);
            workbenchMachineTab = content.Load<Texture2D>(Paths.INTERFACE_CRAFTING_TAB_MACHINE);
            workbenchClothingTabHover = content.Load<Texture2D>(Paths.INTERFACE_CRAFTING_TAB_CLOTHING_HOVER);
            workbenchFloorWallTabHover = content.Load<Texture2D>(Paths.INTERFACE_CRAFTING_TAB_FLOOR_WALL_HOVER);
            workbenchScaffoldingTabHover = content.Load<Texture2D>(Paths.INTERFACE_CRAFTING_TAB_SCAFFOLDING_HOVER);
            workbenchFurnitureTabHover = content.Load<Texture2D>(Paths.INTERFACE_CRAFTING_TAB_FURNITURE_HOVER);
            workbenchMachineTabHover = content.Load<Texture2D>(Paths.INTERFACE_CRAFTING_TAB_MACHINE_HOVER);
            workbenchQuestionMark = content.Load<Texture2D>(Paths.INTERFACE_CRAFTING_QUESTION_MARK);
            workbenchBlueprintDepression = content.Load<Texture2D>(Paths.INTERFACE_CRAFTING_BLUEPRINT_DEPRESSION);

            workbenchArrowLeft = content.Load<Texture2D>(Paths.INTERFACE_CRAFTING_ARROW_LEFT);
            workbenchArrowRight = content.Load<Texture2D>(Paths.INTERFACE_CRAFTING_ARROW_RIGHT);
            workbenchCraftButton = content.Load<Texture2D>(Paths.INTERFACE_CRAFTING_CRAFT_BUTTON);
            workbenchCraftButtonEnlarged = content.Load<Texture2D>(Paths.INTERFACE_CRAFTING_CRAFT_BUTTON_ENLARGED);

            workbenchTabRectangles = new RectangleF[5];
            workbenchTabRectangles[0] = new RectangleF(WORKBENCH_MACHINE_TAB_POSITION, new Size2(11, 11));
            workbenchTabRectangles[1] = new RectangleF(WORKBENCH_SCAFFOLDING_TAB_POSITION, new Size2(11, 11));
            workbenchTabRectangles[2] = new RectangleF(WORKBENCH_FURNITURE_TAB_POSITION, new Size2(11, 11));
            workbenchTabRectangles[3] = new RectangleF(WORKBENCH_HOUSE_TAB_POSITION, new Size2(11, 11));
            workbenchTabRectangles[4] = new RectangleF(WORKBENCH_CLOTHING_TAB_POSITION, new Size2(11, 11));

            workbenchBlueprintRectangles = new RectangleF[15];
            workbenchBlueprintRectangles[0] = new RectangleF(WORKBENCH_BLUEPRINT_POSITION, new Size2(18, 18));
            workbenchBlueprintRectangles[1] = new RectangleF(WORKBENCH_BLUEPRINT_POSITION + new Vector2(blueprintDeltaX, 0), new Size2(18, 18));
            workbenchBlueprintRectangles[2] = new RectangleF(WORKBENCH_BLUEPRINT_POSITION + new Vector2(blueprintDeltaX*2, 0), new Size2(18, 18));
            workbenchBlueprintRectangles[3] = new RectangleF(WORKBENCH_BLUEPRINT_POSITION + new Vector2(blueprintDeltaX*3, 0), new Size2(18, 18));
            workbenchBlueprintRectangles[4] = new RectangleF(WORKBENCH_BLUEPRINT_POSITION + new Vector2(blueprintDeltaX*4, 0), new Size2(18, 18));
            workbenchBlueprintRectangles[5] = new RectangleF(WORKBENCH_BLUEPRINT_POSITION + new Vector2(0, blueprintDeltaY), new Size2(18, 18));
            workbenchBlueprintRectangles[6] = new RectangleF(WORKBENCH_BLUEPRINT_POSITION + new Vector2(blueprintDeltaX, blueprintDeltaY), new Size2(18, 18));
            workbenchBlueprintRectangles[7] = new RectangleF(WORKBENCH_BLUEPRINT_POSITION + new Vector2(blueprintDeltaX * 2, blueprintDeltaY), new Size2(18, 18));
            workbenchBlueprintRectangles[8] = new RectangleF(WORKBENCH_BLUEPRINT_POSITION + new Vector2(blueprintDeltaX * 3, blueprintDeltaY), new Size2(18, 18));
            workbenchBlueprintRectangles[9] = new RectangleF(WORKBENCH_BLUEPRINT_POSITION + new Vector2(blueprintDeltaX * 4, blueprintDeltaY), new Size2(18, 18));
            workbenchBlueprintRectangles[10] = new RectangleF(WORKBENCH_BLUEPRINT_POSITION + new Vector2(0, blueprintDeltaY * 2), new Size2(18, 18));
            workbenchBlueprintRectangles[11] = new RectangleF(WORKBENCH_BLUEPRINT_POSITION + new Vector2(blueprintDeltaX, blueprintDeltaY * 2), new Size2(18, 18));
            workbenchBlueprintRectangles[12] = new RectangleF(WORKBENCH_BLUEPRINT_POSITION + new Vector2(blueprintDeltaX * 2, blueprintDeltaY * 2), new Size2(18, 18));
            workbenchBlueprintRectangles[13] = new RectangleF(WORKBENCH_BLUEPRINT_POSITION + new Vector2(blueprintDeltaX * 3, blueprintDeltaY * 2), new Size2(18, 18));
            workbenchBlueprintRectangles[14] = new RectangleF(WORKBENCH_BLUEPRINT_POSITION + new Vector2(blueprintDeltaX * 4, blueprintDeltaY * 2), new Size2(18, 18));

            workbenchLeftArrowRectangle = new RectangleF(WORKBENCH_LEFT_ARROW_POSITION, new Size2(13, 11));
            workbenchRightArrowRectangle = new RectangleF(WORKBENCH_RIGHT_ARROW_POSITION, new Size2(13, 11));

            workbenchCraftButtonRectangle = new RectangleF(WORKBENCH_CRAFT_BUTTON, new Size2(28, 12));

        }

        public bool IsTransitionReady()
        {
            if(interfaceState == InterfaceState.TRANSITION_TO_DOWN)
            {
                return transitionPosition.Y >= 0;
            }
            else if (interfaceState == InterfaceState.TRANSITION_TO_UP)
            {
                return transitionPosition.Y <= 0;
            }
            else if (interfaceState == InterfaceState.TRANSITION_TO_LEFT)
            {
                return transitionPosition.X <= 0;
            }
            else if (interfaceState == InterfaceState.TRANSITION_TO_RIGHT)
            {
                return transitionPosition.X >= 0;
            } else if (interfaceState == InterfaceState.TRANSITION_FADE_TO_BLACK)
            {
                return transitionAlpha >= 1.0f;
            } else if (interfaceState == InterfaceState.TRANSITION_FADE_IN)
            {
                return transitionAlpha <= 0.0f;
            }
            return true;
        }
        
        private static ScrapbookPage[] BuildScrapbook(ContentManager content, List<ScrapbookPage> pages)
        {
            ScrapbookPage[] book = new ScrapbookPage[NUM_SCRAPBOOK_PAGES];
            
            int pageNum = 0;
            foreach(ScrapbookPage page in pages) {
                book[pageNum] = page;
                pageNum++;
            }

            for(int i = pageNum; i < NUM_SCRAPBOOK_PAGES; i++)
            {
                book[i] = new ScrapbookPage("");
            }

            return book;
        }

        private static Vector2 GetBlueprintPosition(int n)
        {
            Vector2 position = new Vector2(7, 34);
            int blueprintDeltaX = 25;
            int blueprintDeltaY = 23;

            //0 1 2 3 4 5
            //6 7 8 9 10 11
            //12 13 14 15 16 17
            //18 19 20 21 22 23

            int xMod = n % 6;
            int yMod = n / 6;

            position += new Vector2(blueprintDeltaX * xMod, blueprintDeltaY * yMod);

            return position;
        }
        private void InitializeScrapbook(ContentManager content)
        {
            //scrapbook stuff
            scrapbookBase = content.Load<Texture2D>(Paths.INTERFACE_SCRAPBOOK_BASE);
            scrapbookTitleActive = content.Load<Texture2D>(Paths.INTERFACE_SCRAPBOOK_PAGE_ACTIVE);
            scrapbookTitleHover = content.Load<Texture2D>(Paths.INTERFACE_SCRAPBOOK_PAGE_HOVER);
            scrapbookTitleActiveHover = content.Load<Texture2D>(Paths.INTERFACE_SCRAPBOOK_PAGE_ACTIVE_HOVER);
            scrapbookTab1Active = content.Load<Texture2D>(Paths.INTERFACE_SCRAPBOOK_TAB1_ACTIVE);
            scrapbookTab2Active = content.Load<Texture2D>(Paths.INTERFACE_SCRAPBOOK_TAB2_ACTIVE);
            scrapbookTab3Active = content.Load<Texture2D>(Paths.INTERFACE_SCRAPBOOK_TAB3_ACTIVE);
            scrapbookTab4Active = content.Load<Texture2D>(Paths.INTERFACE_SCRAPBOOK_TAB4_ACTIVE);
            scrapbookTab5Active = content.Load<Texture2D>(Paths.INTERFACE_SCRAPBOOK_TAB5_ACTIVE);
            scrapbookTab6Active = content.Load<Texture2D>(Paths.INTERFACE_SCRAPBOOK_TAB6_ACTIVE);
            scrapbookTab7Active = content.Load<Texture2D>(Paths.INTERFACE_SCRAPBOOK_TAB7_ACTIVE);
            scrapbookTab8Active = content.Load<Texture2D>(Paths.INTERFACE_SCRAPBOOK_TAB8_ACTIVE);
            scrapbookTab9Active = content.Load<Texture2D>(Paths.INTERFACE_SCRAPBOOK_TAB9_ACTIVE);
            scrapbookTab10Active = content.Load<Texture2D>(Paths.INTERFACE_SCRAPBOOK_TAB10_ACTIVE);
            scrapbookTab11Active = content.Load<Texture2D>(Paths.INTERFACE_SCRAPBOOK_TAB11_ACTIVE);
            scrapbookTab1Hover = content.Load<Texture2D>(Paths.INTERFACE_SCRAPBOOK_TAB1_HOVER);
            scrapbookTab2Hover = content.Load<Texture2D>(Paths.INTERFACE_SCRAPBOOK_TAB2_HOVER);
            scrapbookTab3Hover = content.Load<Texture2D>(Paths.INTERFACE_SCRAPBOOK_TAB3_HOVER);
            scrapbookTab4Hover = content.Load<Texture2D>(Paths.INTERFACE_SCRAPBOOK_TAB4_HOVER);
            scrapbookTab5Hover = content.Load<Texture2D>(Paths.INTERFACE_SCRAPBOOK_TAB5_HOVER);
            scrapbookTab6Hover = content.Load<Texture2D>(Paths.INTERFACE_SCRAPBOOK_TAB6_HOVER);
            scrapbookTab7Hover = content.Load<Texture2D>(Paths.INTERFACE_SCRAPBOOK_TAB7_HOVER);
            scrapbookTab8Hover = content.Load<Texture2D>(Paths.INTERFACE_SCRAPBOOK_TAB8_HOVER);
            scrapbookTab9Hover = content.Load<Texture2D>(Paths.INTERFACE_SCRAPBOOK_TAB9_HOVER);
            scrapbookTab10Hover = content.Load<Texture2D>(Paths.INTERFACE_SCRAPBOOK_TAB10_HOVER);
            scrapbookTab11Hover = content.Load<Texture2D>(Paths.INTERFACE_SCRAPBOOK_TAB11_HOVER);
            scrapbookTab1ActiveHover = content.Load<Texture2D>(Paths.INTERFACE_SCRAPBOOK_TAB1_HOVERACTIVE);
            scrapbookTab2ActiveHover = content.Load<Texture2D>(Paths.INTERFACE_SCRAPBOOK_TAB2_HOVERACTIVE);
            scrapbookTab3ActiveHover = content.Load<Texture2D>(Paths.INTERFACE_SCRAPBOOK_TAB3_HOVERACTIVE);
            scrapbookTab4ActiveHover = content.Load<Texture2D>(Paths.INTERFACE_SCRAPBOOK_TAB4_HOVERACTIVE);
            scrapbookTab5ActiveHover = content.Load<Texture2D>(Paths.INTERFACE_SCRAPBOOK_TAB5_HOVERACTIVE);
            scrapbookTab6ActiveHover = content.Load<Texture2D>(Paths.INTERFACE_SCRAPBOOK_TAB6_HOVERACTIVE);
            scrapbookTab7ActiveHover = content.Load<Texture2D>(Paths.INTERFACE_SCRAPBOOK_TAB7_HOVERACTIVE);
            scrapbookTab8ActiveHover = content.Load<Texture2D>(Paths.INTERFACE_SCRAPBOOK_TAB8_HOVERACTIVE);
            scrapbookTab9ActiveHover = content.Load<Texture2D>(Paths.INTERFACE_SCRAPBOOK_TAB9_HOVERACTIVE);
            scrapbookTab10ActiveHover = content.Load<Texture2D>(Paths.INTERFACE_SCRAPBOOK_TAB10_HOVERACTIVE);
            scrapbookTab11ActiveHover = content.Load<Texture2D>(Paths.INTERFACE_SCRAPBOOK_TAB11_HOVERACTIVE);

            //scrapbook tab rectangles
            scrapbookTabs = new RectangleF[NUM_SCRAPBOOK_TABS];
            scrapbookTabs[0] = new RectangleF(SCRAPBOOK_TAB1_POSITION, new Vector2(scrapbookTab1Active.Width, scrapbookTab1Active.Height));
            scrapbookTabs[1] = new RectangleF(SCRAPBOOK_TAB2_POSITION, new Vector2(scrapbookTab1Active.Width, scrapbookTab1Active.Height));
            scrapbookTabs[2] = new RectangleF(SCRAPBOOK_TAB3_POSITION, new Vector2(scrapbookTab1Active.Width, scrapbookTab1Active.Height));
            scrapbookTabs[3] = new RectangleF(SCRAPBOOK_TAB4_POSITION, new Vector2(scrapbookTab1Active.Width, scrapbookTab1Active.Height));
            scrapbookTabs[4] = new RectangleF(SCRAPBOOK_TAB5_POSITION, new Vector2(scrapbookTab1Active.Width, scrapbookTab1Active.Height));
            scrapbookTabs[5] = new RectangleF(SCRAPBOOK_TAB6_POSITION, new Vector2(scrapbookTab1Active.Width, scrapbookTab1Active.Height));
            scrapbookTabs[6] = new RectangleF(SCRAPBOOK_TAB7_POSITION, new Vector2(scrapbookTab1Active.Width, scrapbookTab1Active.Height));
            scrapbookTabs[7] = new RectangleF(SCRAPBOOK_TAB8_POSITION, new Vector2(scrapbookTab1Active.Width, scrapbookTab1Active.Height));
            scrapbookTabs[8] = new RectangleF(SCRAPBOOK_TAB9_POSITION, new Vector2(scrapbookTab1Active.Width, scrapbookTab1Active.Height));
            scrapbookTabs[9] = new RectangleF(SCRAPBOOK_TAB10_POSITION, new Vector2(scrapbookTab1Active.Width, scrapbookTab1Active.Height));
            scrapbookTabs[10] = new RectangleF(SCRAPBOOK_TAB11_POSITION, new Vector2(scrapbookTab1Active.Width, scrapbookTab1Active.Height));

            //scrapbook content rectangles
            scrapbookTitles = new RectangleF[NUM_SCRAPBOOK_PAGES];
            scrapbookTitles[0] = new RectangleF(SCRAPBOOK_TITLE1_POSITION, new Vector2(scrapbookTitleActive.Width, scrapbookTitleActive.Height));
            scrapbookTitles[1] = new RectangleF(SCRAPBOOK_TITLE2_POSITION, new Vector2(scrapbookTitleActive.Width, scrapbookTitleActive.Height));
            scrapbookTitles[2] = new RectangleF(SCRAPBOOK_TITLE3_POSITION, new Vector2(scrapbookTitleActive.Width, scrapbookTitleActive.Height));
            scrapbookTitles[3] = new RectangleF(SCRAPBOOK_TITLE4_POSITION, new Vector2(scrapbookTitleActive.Width, scrapbookTitleActive.Height));
            scrapbookTitles[4] = new RectangleF(SCRAPBOOK_TITLE5_POSITION, new Vector2(scrapbookTitleActive.Width, scrapbookTitleActive.Height));
            scrapbookTitles[5] = new RectangleF(SCRAPBOOK_TITLE6_POSITION, new Vector2(scrapbookTitleActive.Width, scrapbookTitleActive.Height));
            scrapbookTitles[6] = new RectangleF(SCRAPBOOK_TITLE7_POSITION, new Vector2(scrapbookTitleActive.Width, scrapbookTitleActive.Height));
            scrapbookTitles[7] = new RectangleF(SCRAPBOOK_TITLE8_POSITION, new Vector2(scrapbookTitleActive.Width, scrapbookTitleActive.Height));
            scrapbookTitles[8] = new RectangleF(SCRAPBOOK_TITLE9_POSITION, new Vector2(scrapbookTitleActive.Width, scrapbookTitleActive.Height));
            scrapbookTitles[9] = new RectangleF(SCRAPBOOK_TITLE10_POSITION, new Vector2(scrapbookTitleActive.Width, scrapbookTitleActive.Height));

            scrapbookDynamicComponents = new Dictionary<string, ScrapbookPage.Component>();


            List<ScrapbookPage> pages = new List<ScrapbookPage>();

            Texture2D blueprintTexture = content.Load<Texture2D>(Paths.INTERFACE_SCRAPBOOK_BLUEPRINT);
            Texture2D patternTexture = content.Load<Texture2D>(Paths.INTERFACE_SCRAPBOOK_PATTERN);

            Vector2 cookingRecipe1Pos = new Vector2(14, 17);
            Vector2 cookingRecipe2Pos = cookingRecipe1Pos + new Vector2(0, 19);
            Vector2 cookingRecipe3Pos = cookingRecipe2Pos + new Vector2(0, 19);
            Vector2 cookingRecipe4Pos = cookingRecipe3Pos + new Vector2(0, 19);
            Vector2 cookingRecipe5Pos = cookingRecipe4Pos + new Vector2(0, 19);
            Vector2 cookingRecipe6Pos = cookingRecipe5Pos + new Vector2(0, 19);

            ScrapbookPage.ImageComponent currentDay = new ScrapbookPage.ImageComponent(new Vector2(100, 100), content.Load<Texture2D>(Paths.INTERFACE_SCRAPBOOK_PAGE_CALENDAR_CURRENT_DAY), Color.White);
            scrapbookDynamicComponents[SCRAPBOOK_CALENDAR_CURRENT_DAY] = currentDay;

            pages.Add(new ScrapbookPage("Calendar", new ScrapbookPage.ImageComponent(new Vector2(0, 0), content.Load<Texture2D>(Paths.INTERFACE_SCRAPBOOK_PAGE_CALENDAR), Color.White), currentDay));
            pages.Add(new ScrapbookPage("Controls"));
            pages.Add(new ScrapbookPage("Relationships 1", new ScrapbookPage.ImageComponent(new Vector2(0, 0), content.Load<Texture2D>(Paths.INTERFACE_SCRAPBOOK_PAGE_RELATIONSHIPS1), Color.White)));
            pages.Add(new ScrapbookPage("Relationships 2", new ScrapbookPage.ImageComponent(new Vector2(0, 0), content.Load<Texture2D>(Paths.INTERFACE_SCRAPBOOK_PAGE_RELATIONSHIPS2), Color.White)));
            pages.Add(new ScrapbookPage("Shrines 1"));
            pages.Add(new ScrapbookPage("Shrines 2"));
            pages.Add(new ScrapbookPage("Shrines 2"));
            pages.Add(new ScrapbookPage("Edit Mode", new ScrapbookPage.ImageComponent(new Vector2(0, 0), content.Load<Texture2D>(Paths.INTERFACE_SCRAPBOOK_PAGE_EDIT_MODE), Color.White)));
            pages.Add(new ScrapbookPage("Construction"));
            pages.Add(new ScrapbookPage("Appearance", new ScrapbookPage.ImageComponent(new Vector2(0, 0), content.Load<Texture2D>(Paths.INTERFACE_SCRAPBOOK_PAGE_BARBER_POLE_ENCHANTED_VANITY), Color.White)));
            pages.Add(new ScrapbookPage("Farming", new ScrapbookPage.ImageComponent(new Vector2(0, 0), content.Load<Texture2D>(Paths.INTERFACE_SCRAPBOOK_PAGE_FARMING_INTRO), Color.White)));
            pages.Add(new ScrapbookPage("Animals", new ScrapbookPage.ImageComponent(new Vector2(0, 0), content.Load<Texture2D>(Paths.INTERFACE_SCRAPBOOK_PAGE_FARMING_ANIMALS), Color.White)));
            pages.Add(new ScrapbookPage("Compost Bin", new ScrapbookPage.ImageComponent(new Vector2(0, 0), content.Load<Texture2D>(Paths.INTERFACE_SCRAPBOOK_PAGE_FARMING_COMPOST), Color.White)));
            pages.Add(new ScrapbookPage("Seed Maker", new ScrapbookPage.ImageComponent(new Vector2(0, 0), content.Load<Texture2D>(Paths.INTERFACE_SCRAPBOOK_PAGE_FARMING_SEEDS), Color.White)));
            pages.Add(new ScrapbookPage("Cooking", new ScrapbookPage.ImageComponent(new Vector2(0, 0), content.Load<Texture2D>(Paths.INTERFACE_SCRAPBOOK_PAGE_COOKING), Color.White)));
            pages.Add(new ScrapbookPage("Workbench", new ScrapbookPage.ImageComponent(new Vector2(0, 0), content.Load<Texture2D>(Paths.INTERFACE_SCRAPBOOK_PAGE_WORKBENCH), Color.White)));
            pages.Add(new ScrapbookPage("Dairy Churn", new ScrapbookPage.ImageComponent(new Vector2(0, 0), content.Load<Texture2D>(Paths.INTERFACE_SCRAPBOOK_PAGE_DAIRY_CHURN), Color.White)));
            pages.Add(new ScrapbookPage("Mayonaise Maker", new ScrapbookPage.ImageComponent(new Vector2(0, 0), content.Load<Texture2D>(Paths.INTERFACE_SCRAPBOOK_PAGE_MAYONAISE_MAKER), Color.White)));
            pages.Add(new ScrapbookPage("Loom", new ScrapbookPage.ImageComponent(new Vector2(0, 0), content.Load<Texture2D>(Paths.INTERFACE_SCRAPBOOK_PAGE_LOOM), Color.White)));
            pages.Add(new ScrapbookPage("Furnace", new ScrapbookPage.ImageComponent(new Vector2(0, 0), content.Load<Texture2D>(Paths.INTERFACE_SCRAPBOOK_PAGE_FURNACE), Color.White)));
            pages.Add(new ScrapbookPage("Anvil", new ScrapbookPage.ImageComponent(new Vector2(0, 0), content.Load<Texture2D>(Paths.INTERFACE_SCRAPBOOK_PAGE_ANVIL), Color.White)));
            pages.Add(new ScrapbookPage("Compressor", new ScrapbookPage.ImageComponent(new Vector2(0, 0), content.Load<Texture2D>(Paths.INTERFACE_SCRAPBOOK_PAGE_COMPRESSOR), Color.White)));
            pages.Add(new ScrapbookPage("Pottery Wheel", new ScrapbookPage.ImageComponent(new Vector2(0, 0), content.Load<Texture2D>(Paths.INTERFACE_SCRAPBOOK_PAGE_POTTERY_WHEEL), Color.White)));
            pages.Add(new ScrapbookPage("Painter's Press", new ScrapbookPage.ImageComponent(new Vector2(0, 0), content.Load<Texture2D>(Paths.INTERFACE_SCRAPBOOK_PAGE_PAINTERS_PRESS), Color.White)));
            pages.Add(new ScrapbookPage("Beehive", new ScrapbookPage.ImageComponent(new Vector2(0, 0), content.Load<Texture2D>(Paths.INTERFACE_SCRAPBOOK_PAGE_BEEHIVE), Color.White)));
            pages.Add(new ScrapbookPage("Birdhouse", new ScrapbookPage.ImageComponent(new Vector2(0, 0), content.Load<Texture2D>(Paths.INTERFACE_SCRAPBOOK_PAGE_BIRDHOUSE), Color.White)));
            pages.Add(new ScrapbookPage("Aquarium", new ScrapbookPage.ImageComponent(new Vector2(0, 0), content.Load<Texture2D>(Paths.INTERFACE_SCRAPBOOK_PAGE_AQUARIUM), Color.White)));
            pages.Add(new ScrapbookPage("Perfumery", new ScrapbookPage.ImageComponent(new Vector2(0, 0), content.Load<Texture2D>(Paths.INTERFACE_SCRAPBOOK_PAGE_PERFUMERY), Color.White)));
            pages.Add(new ScrapbookPage("Origami", new ScrapbookPage.ImageComponent(new Vector2(0, 0), content.Load<Texture2D>(Paths.INTERFACE_SCRAPBOOK_PAGE_ORIGAMI_STATION), Color.White)));
            pages.Add(new ScrapbookPage("Jeweler's Bench", new ScrapbookPage.ImageComponent(new Vector2(0, 0), content.Load<Texture2D>(Paths.INTERFACE_SCRAPBOOK_PAGE_JEWELERS_BENCH), Color.White)));
            pages.Add(new ScrapbookPage("Drying Rack", new ScrapbookPage.ImageComponent(new Vector2(0, 0), content.Load<Texture2D>(Paths.INTERFACE_SCRAPBOOK_PAGE_DRYING_RACK), Color.White)));
            pages.Add(new ScrapbookPage("Keg", new ScrapbookPage.ImageComponent(new Vector2(0, 0), content.Load<Texture2D>(Paths.INTERFACE_SCRAPBOOK_PAGE_KEG), Color.White)));
            pages.Add(new ScrapbookPage("Alchemy", new ScrapbookPage.ImageComponent(new Vector2(0, 0), content.Load<Texture2D>(Paths.INTERFACE_SCRAPBOOK_PAGE_ALCHEMY), Color.White)));
            pages.Add(new ScrapbookPage("Extractor", new ScrapbookPage.ImageComponent(new Vector2(0, 0), content.Load<Texture2D>(Paths.INTERFACE_SCRAPBOOK_PAGE_EXTRACTOR), Color.White)));
            pages.Add(new ScrapbookPage("Gem Replicator", new ScrapbookPage.ImageComponent(new Vector2(0, 0), content.Load<Texture2D>(Paths.INTERFACE_SCRAPBOOK_PAGE_GEMSTONE_REPLICATOR), Color.White)));
            pages.Add(new ScrapbookPage("Glassblower", new ScrapbookPage.ImageComponent(new Vector2(0, 0), content.Load<Texture2D>(Paths.INTERFACE_SCRAPBOOK_PAGE_GLASSBLOWER), Color.White)));
            pages.Add(new ScrapbookPage("Mushbox", new ScrapbookPage.ImageComponent(new Vector2(0, 0), content.Load<Texture2D>(Paths.INTERFACE_SCRAPBOOK_PAGE_MUSHBOX), Color.White)));
            pages.Add(new ScrapbookPage("Flowerbed", new ScrapbookPage.ImageComponent(new Vector2(0, 0), content.Load<Texture2D>(Paths.INTERFACE_SCRAPBOOK_PAGE_FLOWERBED), Color.White)));
            pages.Add(new ScrapbookPage("Terrarium", new ScrapbookPage.ImageComponent(new Vector2(0, 0), content.Load<Texture2D>(Paths.INTERFACE_SCRAPBOOK_PAGE_TERRARIUM), Color.White)));
            pages.Add(new ScrapbookPage("Vivarium", new ScrapbookPage.ImageComponent(new Vector2(0, 0), content.Load<Texture2D>(Paths.INTERFACE_SCRAPBOOK_PAGE_VIVARIUM), Color.White)));
            pages.Add(new ScrapbookPage("Recycler", new ScrapbookPage.ImageComponent(new Vector2(0, 0), content.Load<Texture2D>(Paths.INTERFACE_SCRAPBOOK_PAGE_RECYCLER), Color.White)));
            pages.Add(new ScrapbookPage("Alchemizer", new ScrapbookPage.ImageComponent(new Vector2(0, 0), content.Load<Texture2D>(Paths.INTERFACE_SCRAPBOOK_PAGE_ALCHEMIZER), Color.White)));
            pages.Add(new ScrapbookPage("Cloning Machine", new ScrapbookPage.ImageComponent(new Vector2(0, 0), content.Load<Texture2D>(Paths.INTERFACE_SCRAPBOOK_PAGE_CLONING_MACHINE), Color.White)));
            pages.Add(new ScrapbookPage("Sky Statue", new ScrapbookPage.ImageComponent(new Vector2(0, 0), content.Load<Texture2D>(Paths.INTERFACE_SCRAPBOOK_PAGE_SKY_STATUE), Color.White)));
            pages.Add(new ScrapbookPage("Draconic Pillar", new ScrapbookPage.ImageComponent(new Vector2(0, 0), content.Load<Texture2D>(Paths.INTERFACE_SCRAPBOOK_PAGE_DRACONIC_PILLAR), Color.White)));
            pages.Add(new ScrapbookPage("Soulchest", new ScrapbookPage.ImageComponent(new Vector2(0, 0), content.Load<Texture2D>(Paths.INTERFACE_SCRAPBOOK_PAGE_SOULCHEST), Color.White)));
            pages.Add(new ScrapbookPage("Cookbook Forage 1", new ScrapbookPage.ImageComponent(new Vector2(0, 0), content.Load<Texture2D>(Paths.INTERFACE_SCRAPBOOK_PAGE_COOKBOOK_FORAGE1), Color.White),
                new ScrapbookPage.CookingRecipeComponent(cookingRecipe1Pos, GameState.GetCookingRecipeForResult(ItemDict.MOUNTAIN_BREAD), numbers, ScrapbookPage.CookingRecipeComponent.RecipeType.COOKING),
                new ScrapbookPage.CookingRecipeComponent(cookingRecipe2Pos, GameState.GetCookingRecipeForResult(ItemDict.CRISPY_GRASSHOPPER), numbers, ScrapbookPage.CookingRecipeComponent.RecipeType.COOKING),
                new ScrapbookPage.CookingRecipeComponent(cookingRecipe3Pos, GameState.GetCookingRecipeForResult(ItemDict.REJUVENATION_TEA), numbers, ScrapbookPage.CookingRecipeComponent.RecipeType.COOKING),
                new ScrapbookPage.CookingRecipeComponent(cookingRecipe4Pos, GameState.GetCookingRecipeForResult(ItemDict.CHICKWEED_BLEND), numbers, ScrapbookPage.CookingRecipeComponent.RecipeType.COOKING),
                new ScrapbookPage.CookingRecipeComponent(cookingRecipe5Pos, GameState.GetCookingRecipeForResult(ItemDict.HOMESTYLE_JELLY), numbers, ScrapbookPage.CookingRecipeComponent.RecipeType.COOKING),
                new ScrapbookPage.CookingRecipeComponent(cookingRecipe6Pos, GameState.GetCookingRecipeForResult(ItemDict.SEAFOOD_BASKET), numbers, ScrapbookPage.CookingRecipeComponent.RecipeType.COOKING)));
            pages.Add(new ScrapbookPage("Cookbook Forage 2", new ScrapbookPage.ImageComponent(new Vector2(0, 0), content.Load<Texture2D>(Paths.INTERFACE_SCRAPBOOK_PAGE_COOKBOOK_FORAGE2), Color.White),
                new ScrapbookPage.CookingRecipeComponent(cookingRecipe1Pos, GameState.GetCookingRecipeForResult(ItemDict.ELDERBERRY_TART), numbers, ScrapbookPage.CookingRecipeComponent.RecipeType.COOKING),
                new ScrapbookPage.CookingRecipeComponent(cookingRecipe2Pos, GameState.GetCookingRecipeForResult(ItemDict.AUTUMN_MASH), numbers, ScrapbookPage.CookingRecipeComponent.RecipeType.COOKING),
                new ScrapbookPage.CookingRecipeComponent(cookingRecipe3Pos, GameState.GetCookingRecipeForResult(ItemDict.WILD_POPCORN), numbers, ScrapbookPage.CookingRecipeComponent.RecipeType.COOKING),
                new ScrapbookPage.CookingRecipeComponent(cookingRecipe4Pos, GameState.GetCookingRecipeForResult(ItemDict.CAMPFIRE_COFFEE), numbers, ScrapbookPage.CookingRecipeComponent.RecipeType.COOKING),
                new ScrapbookPage.CookingRecipeComponent(cookingRecipe5Pos, GameState.GetCookingRecipeForResult(ItemDict.DARK_TEA), numbers, ScrapbookPage.CookingRecipeComponent.RecipeType.COOKING),
                new ScrapbookPage.CookingRecipeComponent(cookingRecipe6Pos, GameState.GetCookingRecipeForResult(ItemDict.LICHEN_JUICE), numbers, ScrapbookPage.CookingRecipeComponent.RecipeType.COOKING)));
            pages.Add(new ScrapbookPage("Cookbook Forage 3", new ScrapbookPage.ImageComponent(new Vector2(0, 0), content.Load<Texture2D>(Paths.INTERFACE_SCRAPBOOK_PAGE_COOKBOOK_FORAGE3), Color.White),
                new ScrapbookPage.CookingRecipeComponent(cookingRecipe1Pos, GameState.GetCookingRecipeForResult(ItemDict.FRIED_FISH), numbers, ScrapbookPage.CookingRecipeComponent.RecipeType.COOKING),
                new ScrapbookPage.CookingRecipeComponent(cookingRecipe2Pos, GameState.GetCookingRecipeForResult(ItemDict.FRIED_OYSTERS), numbers, ScrapbookPage.CookingRecipeComponent.RecipeType.COOKING),
                new ScrapbookPage.CookingRecipeComponent(cookingRecipe3Pos, GameState.GetCookingRecipeForResult(ItemDict.BLIND_DINNER), numbers, ScrapbookPage.CookingRecipeComponent.RecipeType.COOKING),
                new ScrapbookPage.CookingRecipeComponent(cookingRecipe4Pos, GameState.GetCookingRecipeForResult(ItemDict.SWEET_COCO_TREAT), numbers, ScrapbookPage.CookingRecipeComponent.RecipeType.COOKING),
                new ScrapbookPage.CookingRecipeComponent(cookingRecipe5Pos, GameState.GetCookingRecipeForResult(ItemDict.SARDINE_SNACK), numbers, ScrapbookPage.CookingRecipeComponent.RecipeType.COOKING),
                new ScrapbookPage.CookingRecipeComponent(cookingRecipe6Pos, GameState.GetCookingRecipeForResult(ItemDict.SURVIVORS_SURPRISE), numbers, ScrapbookPage.CookingRecipeComponent.RecipeType.COOKING)));
            pages.Add(new ScrapbookPage("Cookbook Spring", new ScrapbookPage.ImageComponent(new Vector2(0, 0), content.Load<Texture2D>(Paths.INTERFACE_SCRAPBOOK_PAGE_COOKBOOK_SPRING), Color.White),
                new ScrapbookPage.CookingRecipeComponent(cookingRecipe1Pos, GameState.GetCookingRecipeForResult(ItemDict.SPRING_PIZZA), numbers, ScrapbookPage.CookingRecipeComponent.RecipeType.COOKING),
                new ScrapbookPage.CookingRecipeComponent(cookingRecipe2Pos, GameState.GetCookingRecipeForResult(ItemDict.BOAR_STEW), numbers, ScrapbookPage.CookingRecipeComponent.RecipeType.COOKING),
                new ScrapbookPage.CookingRecipeComponent(cookingRecipe3Pos, GameState.GetCookingRecipeForResult(ItemDict.BAKED_POTATO), numbers, ScrapbookPage.CookingRecipeComponent.RecipeType.COOKING),
                new ScrapbookPage.CookingRecipeComponent(cookingRecipe4Pos, GameState.GetCookingRecipeForResult(ItemDict.STRAWBERRY_SALAD), numbers, ScrapbookPage.CookingRecipeComponent.RecipeType.COOKING)));
            pages.Add(new ScrapbookPage("Cookbook Summer", new ScrapbookPage.ImageComponent(new Vector2(0, 0), content.Load<Texture2D>(Paths.INTERFACE_SCRAPBOOK_PAGE_COOKBOOK_SUMMER), Color.White),
                new ScrapbookPage.CookingRecipeComponent(cookingRecipe1Pos, GameState.GetCookingRecipeForResult(ItemDict.FRESH_SALAD), numbers, ScrapbookPage.CookingRecipeComponent.RecipeType.COOKING),
                new ScrapbookPage.CookingRecipeComponent(cookingRecipe2Pos, GameState.GetCookingRecipeForResult(ItemDict.MEATY_PIZZA), numbers, ScrapbookPage.CookingRecipeComponent.RecipeType.COOKING),
                new ScrapbookPage.CookingRecipeComponent(cookingRecipe3Pos, GameState.GetCookingRecipeForResult(ItemDict.STEWED_VEGGIES), numbers, ScrapbookPage.CookingRecipeComponent.RecipeType.COOKING),
                new ScrapbookPage.CookingRecipeComponent(cookingRecipe4Pos, GameState.GetCookingRecipeForResult(ItemDict.EGGPLANT_PARMESAN), numbers, ScrapbookPage.CookingRecipeComponent.RecipeType.COOKING),
                new ScrapbookPage.CookingRecipeComponent(cookingRecipe5Pos, GameState.GetCookingRecipeForResult(ItemDict.WATERMELON_ICE), numbers, ScrapbookPage.CookingRecipeComponent.RecipeType.COOKING)));
            pages.Add(new ScrapbookPage("Cookbook Autumn", new ScrapbookPage.ImageComponent(new Vector2(0, 0), content.Load<Texture2D>(Paths.INTERFACE_SCRAPBOOK_PAGE_COOKBOOK_AUTUMN), Color.White),
                new ScrapbookPage.CookingRecipeComponent(cookingRecipe1Pos, GameState.GetCookingRecipeForResult(ItemDict.SEAFOOD_PAELLA), numbers, ScrapbookPage.CookingRecipeComponent.RecipeType.COOKING),
                new ScrapbookPage.CookingRecipeComponent(cookingRecipe2Pos, GameState.GetCookingRecipeForResult(ItemDict.DWARVEN_STEW), numbers, ScrapbookPage.CookingRecipeComponent.RecipeType.COOKING),
                new ScrapbookPage.CookingRecipeComponent(cookingRecipe3Pos, GameState.GetCookingRecipeForResult(ItemDict.ROASTED_PUMPKIN), numbers, ScrapbookPage.CookingRecipeComponent.RecipeType.COOKING),
                new ScrapbookPage.CookingRecipeComponent(cookingRecipe4Pos, GameState.GetCookingRecipeForResult(ItemDict.WRAPPED_CABBAGE), numbers, ScrapbookPage.CookingRecipeComponent.RecipeType.COOKING),
                new ScrapbookPage.CookingRecipeComponent(cookingRecipe5Pos, GameState.GetCookingRecipeForResult(ItemDict.VEGGIE_SIDE_ROAST), numbers, ScrapbookPage.CookingRecipeComponent.RecipeType.COOKING)));
            pages.Add(new ScrapbookPage("Cookbook Four", new ScrapbookPage.ImageComponent(new Vector2(0, 0), content.Load<Texture2D>(Paths.INTERFACE_SCRAPBOOK_PAGE_COOKBOOK_FOUR), Color.White),
                new ScrapbookPage.CookingRecipeComponent(cookingRecipe1Pos, GameState.GetCookingRecipeForResult(ItemDict.SEASONAL_PIPERADE), numbers, ScrapbookPage.CookingRecipeComponent.RecipeType.COOKING),
                new ScrapbookPage.CookingRecipeComponent(cookingRecipe2Pos, GameState.GetCookingRecipeForResult(ItemDict.COCONUT_BOAR), numbers, ScrapbookPage.CookingRecipeComponent.RecipeType.COOKING),
                new ScrapbookPage.CookingRecipeComponent(cookingRecipe3Pos, GameState.GetCookingRecipeForResult(ItemDict.POTATO_AND_BEET_FRIES), numbers, ScrapbookPage.CookingRecipeComponent.RecipeType.COOKING),
                new ScrapbookPage.CookingRecipeComponent(cookingRecipe4Pos, GameState.GetCookingRecipeForResult(ItemDict.PICKLED_BEET_EGG), numbers, ScrapbookPage.CookingRecipeComponent.RecipeType.COOKING),
                new ScrapbookPage.CookingRecipeComponent(cookingRecipe5Pos, GameState.GetCookingRecipeForResult(ItemDict.SUPER_JUICE), numbers, ScrapbookPage.CookingRecipeComponent.RecipeType.COOKING),
                new ScrapbookPage.CookingRecipeComponent(cookingRecipe6Pos, GameState.GetCookingRecipeForResult(ItemDict.RATATOUILLE), numbers, ScrapbookPage.CookingRecipeComponent.RecipeType.COOKING)));
            pages.Add(new ScrapbookPage("Cookbook Ice", new ScrapbookPage.ImageComponent(new Vector2(0, 0), content.Load<Texture2D>(Paths.INTERFACE_SCRAPBOOK_PAGE_COOKBOOK_ICE), Color.White),
                new ScrapbookPage.CookingRecipeComponent(cookingRecipe1Pos, GameState.GetCookingRecipeForResult(ItemDict.MINTY_MELT), numbers, ScrapbookPage.CookingRecipeComponent.RecipeType.COOKING),
                new ScrapbookPage.CookingRecipeComponent(cookingRecipe2Pos, GameState.GetCookingRecipeForResult(ItemDict.VANILLA_ICE_CREAM), numbers, ScrapbookPage.CookingRecipeComponent.RecipeType.COOKING),
                new ScrapbookPage.CookingRecipeComponent(cookingRecipe3Pos, GameState.GetCookingRecipeForResult(ItemDict.BERRY_MILKSHAKE), numbers, ScrapbookPage.CookingRecipeComponent.RecipeType.COOKING),
                new ScrapbookPage.CookingRecipeComponent(cookingRecipe4Pos, GameState.GetCookingRecipeForResult(ItemDict.BANANA_SUNDAE), numbers, ScrapbookPage.CookingRecipeComponent.RecipeType.COOKING),
                new ScrapbookPage.CookingRecipeComponent(cookingRecipe5Pos, GameState.GetCookingRecipeForResult(ItemDict.MINT_CHOCO_BAR), numbers, ScrapbookPage.CookingRecipeComponent.RecipeType.COOKING)));
            pages.Add(new ScrapbookPage("Cookbook Morning", new ScrapbookPage.ImageComponent(new Vector2(0, 0), content.Load<Texture2D>(Paths.INTERFACE_SCRAPBOOK_PAGE_COOKBOOK_BREAKFAST), Color.White),
                new ScrapbookPage.CookingRecipeComponent(cookingRecipe1Pos, GameState.GetCookingRecipeForResult(ItemDict.FRIED_EGG), numbers, ScrapbookPage.CookingRecipeComponent.RecipeType.COOKING),
                new ScrapbookPage.CookingRecipeComponent(cookingRecipe2Pos, GameState.GetCookingRecipeForResult(ItemDict.EGG_SCRAMBLE), numbers, ScrapbookPage.CookingRecipeComponent.RecipeType.COOKING),
                new ScrapbookPage.CookingRecipeComponent(cookingRecipe3Pos, GameState.GetCookingRecipeForResult(ItemDict.SPICY_BACON), numbers, ScrapbookPage.CookingRecipeComponent.RecipeType.COOKING),
                new ScrapbookPage.CookingRecipeComponent(cookingRecipe4Pos, GameState.GetCookingRecipeForResult(ItemDict.BLUEBERRY_PANCAKES), numbers, ScrapbookPage.CookingRecipeComponent.RecipeType.COOKING),
                new ScrapbookPage.CookingRecipeComponent(cookingRecipe5Pos, GameState.GetCookingRecipeForResult(ItemDict.APPLE_MUFFIN), numbers, ScrapbookPage.CookingRecipeComponent.RecipeType.COOKING),
                new ScrapbookPage.CookingRecipeComponent(cookingRecipe6Pos, GameState.GetCookingRecipeForResult(ItemDict.BREAKFAST_POTATOES), numbers, ScrapbookPage.CookingRecipeComponent.RecipeType.COOKING)));
            pages.Add(new ScrapbookPage("Cookbook Supper 1", new ScrapbookPage.ImageComponent(new Vector2(0, 0), content.Load<Texture2D>(Paths.INTERFACE_SCRAPBOOK_PAGE_COOKBOOK_SUPPER1), Color.White),
                new ScrapbookPage.CookingRecipeComponent(cookingRecipe1Pos, GameState.GetCookingRecipeForResult(ItemDict.STUFFED_FLOUNDER), numbers, ScrapbookPage.CookingRecipeComponent.RecipeType.COOKING),
                new ScrapbookPage.CookingRecipeComponent(cookingRecipe2Pos, GameState.GetCookingRecipeForResult(ItemDict.FRIED_CATFISH), numbers, ScrapbookPage.CookingRecipeComponent.RecipeType.COOKING),
                new ScrapbookPage.CookingRecipeComponent(cookingRecipe3Pos, GameState.GetCookingRecipeForResult(ItemDict.GRILLED_SALMON), numbers, ScrapbookPage.CookingRecipeComponent.RecipeType.COOKING),
                new ScrapbookPage.CookingRecipeComponent(cookingRecipe4Pos, GameState.GetCookingRecipeForResult(ItemDict.BAKED_SNAPPER), numbers, ScrapbookPage.CookingRecipeComponent.RecipeType.COOKING),
                new ScrapbookPage.CookingRecipeComponent(cookingRecipe5Pos, GameState.GetCookingRecipeForResult(ItemDict.SWORDFISH_POT), numbers, ScrapbookPage.CookingRecipeComponent.RecipeType.COOKING),
                new ScrapbookPage.CookingRecipeComponent(cookingRecipe6Pos, GameState.GetCookingRecipeForResult(ItemDict.CLAM_LINGUINI), numbers, ScrapbookPage.CookingRecipeComponent.RecipeType.COOKING)));
            pages.Add(new ScrapbookPage("Cookbook Supper 2", new ScrapbookPage.ImageComponent(new Vector2(0, 0), content.Load<Texture2D>(Paths.INTERFACE_SCRAPBOOK_PAGE_COOKBOOK_SUPPER2), Color.White),
                new ScrapbookPage.CookingRecipeComponent(cookingRecipe1Pos, GameState.GetCookingRecipeForResult(ItemDict.HONEY_STIR_FRY), numbers, ScrapbookPage.CookingRecipeComponent.RecipeType.COOKING),
                new ScrapbookPage.CookingRecipeComponent(cookingRecipe2Pos, GameState.GetCookingRecipeForResult(ItemDict.BUTTERED_ROLLS), numbers, ScrapbookPage.CookingRecipeComponent.RecipeType.COOKING),
                new ScrapbookPage.CookingRecipeComponent(cookingRecipe3Pos, GameState.GetCookingRecipeForResult(ItemDict.COLESLAW), numbers, ScrapbookPage.CookingRecipeComponent.RecipeType.COOKING),
                new ScrapbookPage.CookingRecipeComponent(cookingRecipe4Pos, GameState.GetCookingRecipeForResult(ItemDict.SAVORY_ROAST), numbers, ScrapbookPage.CookingRecipeComponent.RecipeType.COOKING),
                new ScrapbookPage.CookingRecipeComponent(cookingRecipe5Pos, GameState.GetCookingRecipeForResult(ItemDict.CHERRY_CHEESECAKE), numbers, ScrapbookPage.CookingRecipeComponent.RecipeType.COOKING),
                new ScrapbookPage.CookingRecipeComponent(cookingRecipe6Pos, GameState.GetCookingRecipeForResult(ItemDict.LEMON_SHORTCAKE), numbers, ScrapbookPage.CookingRecipeComponent.RecipeType.COOKING)));
            pages.Add(new ScrapbookPage("Cookbook Eastern", new ScrapbookPage.ImageComponent(new Vector2(0, 0), content.Load<Texture2D>(Paths.INTERFACE_SCRAPBOOK_PAGE_COOKBOOK_EASTERN), Color.White),
                new ScrapbookPage.CookingRecipeComponent(cookingRecipe1Pos, GameState.GetCookingRecipeForResult(ItemDict.MOUNTAIN_TERIYAKI), numbers, ScrapbookPage.CookingRecipeComponent.RecipeType.COOKING),
                new ScrapbookPage.CookingRecipeComponent(cookingRecipe2Pos, GameState.GetCookingRecipeForResult(ItemDict.SEARED_TUNA), numbers, ScrapbookPage.CookingRecipeComponent.RecipeType.COOKING),
                new ScrapbookPage.CookingRecipeComponent(cookingRecipe3Pos, GameState.GetCookingRecipeForResult(ItemDict.SUSHI_ROLL), numbers, ScrapbookPage.CookingRecipeComponent.RecipeType.COOKING),
                new ScrapbookPage.CookingRecipeComponent(cookingRecipe4Pos, GameState.GetCookingRecipeForResult(ItemDict.MUSHROOM_STIR_FRY), numbers, ScrapbookPage.CookingRecipeComponent.RecipeType.COOKING),
                new ScrapbookPage.CookingRecipeComponent(cookingRecipe5Pos, GameState.GetCookingRecipeForResult(ItemDict.LETHAL_SASHIMI), numbers, ScrapbookPage.CookingRecipeComponent.RecipeType.COOKING)));
            pages.Add(new ScrapbookPage("Cookbook Soups", new ScrapbookPage.ImageComponent(new Vector2(0, 0), content.Load<Texture2D>(Paths.INTERFACE_SCRAPBOOK_PAGE_COOKBOOK_SOUP), Color.White),
                new ScrapbookPage.CookingRecipeComponent(cookingRecipe1Pos, GameState.GetCookingRecipeForResult(ItemDict.FRENCH_ONION_SOUP), numbers, ScrapbookPage.CookingRecipeComponent.RecipeType.COOKING),
                new ScrapbookPage.CookingRecipeComponent(cookingRecipe2Pos, GameState.GetCookingRecipeForResult(ItemDict.CREAM_OF_MUSHROOM), numbers, ScrapbookPage.CookingRecipeComponent.RecipeType.COOKING),
                new ScrapbookPage.CookingRecipeComponent(cookingRecipe3Pos, GameState.GetCookingRecipeForResult(ItemDict.CREAMY_SPINACH), numbers, ScrapbookPage.CookingRecipeComponent.RecipeType.COOKING),
                new ScrapbookPage.CookingRecipeComponent(cookingRecipe4Pos, GameState.GetCookingRecipeForResult(ItemDict.SHRIMP_GUMBO), numbers, ScrapbookPage.CookingRecipeComponent.RecipeType.COOKING),
                new ScrapbookPage.CookingRecipeComponent(cookingRecipe5Pos, GameState.GetCookingRecipeForResult(ItemDict.FARMERS_STEW), numbers, ScrapbookPage.CookingRecipeComponent.RecipeType.COOKING),
                new ScrapbookPage.CookingRecipeComponent(cookingRecipe6Pos, GameState.GetCookingRecipeForResult(ItemDict.TOMATO_SOUP), numbers, ScrapbookPage.CookingRecipeComponent.RecipeType.COOKING)));
            pages.Add(new ScrapbookPage("Cookbook Unique", new ScrapbookPage.ImageComponent(new Vector2(0, 0), content.Load<Texture2D>(Paths.INTERFACE_SCRAPBOOK_PAGE_COOKBOOK_UNIQUE), Color.White),
                new ScrapbookPage.CookingRecipeComponent(cookingRecipe1Pos, GameState.GetCookingRecipeForResult(ItemDict.STORMFISH), numbers, ScrapbookPage.CookingRecipeComponent.RecipeType.COOKING),
                new ScrapbookPage.CookingRecipeComponent(cookingRecipe2Pos, GameState.GetCookingRecipeForResult(ItemDict.LUMINOUS_STEW), numbers, ScrapbookPage.CookingRecipeComponent.RecipeType.COOKING),
                new ScrapbookPage.CookingRecipeComponent(cookingRecipe3Pos, GameState.GetCookingRecipeForResult(ItemDict.ESCARGOT), numbers, ScrapbookPage.CookingRecipeComponent.RecipeType.COOKING),
                new ScrapbookPage.CookingRecipeComponent(cookingRecipe4Pos, GameState.GetCookingRecipeForResult(ItemDict.CREAMY_SQUID), numbers, ScrapbookPage.CookingRecipeComponent.RecipeType.COOKING),
                new ScrapbookPage.CookingRecipeComponent(cookingRecipe5Pos, GameState.GetCookingRecipeForResult(ItemDict.RAW_CALAMARI), numbers, ScrapbookPage.CookingRecipeComponent.RecipeType.COOKING),
                new ScrapbookPage.CookingRecipeComponent(cookingRecipe6Pos, GameState.GetCookingRecipeForResult(ItemDict.EEL_ROLL), numbers, ScrapbookPage.CookingRecipeComponent.RecipeType.COOKING)));
            pages.Add(new ScrapbookPage("Acces. Home 1", new ScrapbookPage.ImageComponent(new Vector2(0, 0), content.Load<Texture2D>(Paths.INTERFACE_SCRAPBOOK_PAGE_ACCESSORIES_HOMEMADE1), Color.White),
                new ScrapbookPage.CookingRecipeComponent(cookingRecipe1Pos, GameState.GetAccessoryRecipeForResult(ItemDict.BUTTERFLY_CHARM), numbers, ScrapbookPage.CookingRecipeComponent.RecipeType.ACCESSORY),
                new ScrapbookPage.CookingRecipeComponent(cookingRecipe2Pos, GameState.GetAccessoryRecipeForResult(ItemDict.DROPLET_CHARM), numbers, ScrapbookPage.CookingRecipeComponent.RecipeType.ACCESSORY),
                new ScrapbookPage.CookingRecipeComponent(cookingRecipe3Pos, GameState.GetAccessoryRecipeForResult(ItemDict.SNOUT_CHARM), numbers, ScrapbookPage.CookingRecipeComponent.RecipeType.ACCESSORY),
                new ScrapbookPage.CookingRecipeComponent(cookingRecipe4Pos, GameState.GetAccessoryRecipeForResult(ItemDict.SUNFLOWER_CHARM), numbers, ScrapbookPage.CookingRecipeComponent.RecipeType.ACCESSORY),
                new ScrapbookPage.CookingRecipeComponent(cookingRecipe5Pos, GameState.GetAccessoryRecipeForResult(ItemDict.SALTY_CHARM), numbers, ScrapbookPage.CookingRecipeComponent.RecipeType.ACCESSORY),
                new ScrapbookPage.CookingRecipeComponent(cookingRecipe6Pos, GameState.GetAccessoryRecipeForResult(ItemDict.SPINED_CHARM), numbers, ScrapbookPage.CookingRecipeComponent.RecipeType.ACCESSORY)));
            pages.Add(new ScrapbookPage("Acces. Home 2", new ScrapbookPage.ImageComponent(new Vector2(0, 0), content.Load<Texture2D>(Paths.INTERFACE_SCRAPBOOK_PAGE_ACCESSORIES_HOMEMADE2), Color.White),
                new ScrapbookPage.CookingRecipeComponent(cookingRecipe1Pos, GameState.GetAccessoryRecipeForResult(ItemDict.MANTLE_CHARM), numbers, ScrapbookPage.CookingRecipeComponent.RecipeType.ACCESSORY),
                new ScrapbookPage.CookingRecipeComponent(cookingRecipe2Pos, GameState.GetAccessoryRecipeForResult(ItemDict.DANDYLION_CHARM), numbers, ScrapbookPage.CookingRecipeComponent.RecipeType.ACCESSORY),
                new ScrapbookPage.CookingRecipeComponent(cookingRecipe3Pos, GameState.GetAccessoryRecipeForResult(ItemDict.ACID_BRACER), numbers, ScrapbookPage.CookingRecipeComponent.RecipeType.ACCESSORY),
                new ScrapbookPage.CookingRecipeComponent(cookingRecipe4Pos, GameState.GetAccessoryRecipeForResult(ItemDict.URCHIN_BRACER), numbers, ScrapbookPage.CookingRecipeComponent.RecipeType.ACCESSORY),
                new ScrapbookPage.CookingRecipeComponent(cookingRecipe5Pos, GameState.GetAccessoryRecipeForResult(ItemDict.FLUFFY_BRACER), numbers, ScrapbookPage.CookingRecipeComponent.RecipeType.ACCESSORY),
                new ScrapbookPage.CookingRecipeComponent(cookingRecipe6Pos, GameState.GetAccessoryRecipeForResult(ItemDict.DRUID_BRACER), numbers, ScrapbookPage.CookingRecipeComponent.RecipeType.ACCESSORY)));
            pages.Add(new ScrapbookPage("Acces. Home 3", new ScrapbookPage.ImageComponent(new Vector2(0, 0), content.Load<Texture2D>(Paths.INTERFACE_SCRAPBOOK_PAGE_ACCESSORIES_HOMEMADE3), Color.White),
                new ScrapbookPage.CookingRecipeComponent(cookingRecipe1Pos, GameState.GetAccessoryRecipeForResult(ItemDict.TRADITION_BRACER), numbers, ScrapbookPage.CookingRecipeComponent.RecipeType.ACCESSORY),
                new ScrapbookPage.CookingRecipeComponent(cookingRecipe2Pos, GameState.GetAccessoryRecipeForResult(ItemDict.SANDSTORM_BRACER), numbers, ScrapbookPage.CookingRecipeComponent.RecipeType.ACCESSORY),
                new ScrapbookPage.CookingRecipeComponent(cookingRecipe3Pos, GameState.GetAccessoryRecipeForResult(ItemDict.DWARVEN_CHILDS_BRACER), numbers, ScrapbookPage.CookingRecipeComponent.RecipeType.ACCESSORY),
                new ScrapbookPage.CookingRecipeComponent(cookingRecipe4Pos, GameState.GetAccessoryRecipeForResult(ItemDict.CARNIVORE_BRACER), numbers, ScrapbookPage.CookingRecipeComponent.RecipeType.ACCESSORY),
                new ScrapbookPage.CookingRecipeComponent(cookingRecipe5Pos, GameState.GetAccessoryRecipeForResult(ItemDict.PURIFICATION_BRACER), numbers, ScrapbookPage.CookingRecipeComponent.RecipeType.ACCESSORY),
                new ScrapbookPage.CookingRecipeComponent(cookingRecipe6Pos, GameState.GetAccessoryRecipeForResult(ItemDict.SCRAP_BRACER), numbers, ScrapbookPage.CookingRecipeComponent.RecipeType.ACCESSORY)));
            pages.Add(new ScrapbookPage("Acces. Home 4", new ScrapbookPage.ImageComponent(new Vector2(0, 0), content.Load<Texture2D>(Paths.INTERFACE_SCRAPBOOK_PAGE_ACCESSORIES_HOMEMADE4), Color.White),
                new ScrapbookPage.CookingRecipeComponent(cookingRecipe1Pos, GameState.GetAccessoryRecipeForResult(ItemDict.ESSENCE_BRACER), numbers, ScrapbookPage.CookingRecipeComponent.RecipeType.ACCESSORY)));
            pages.Add(new ScrapbookPage("Acces. Nature 1", new ScrapbookPage.ImageComponent(new Vector2(0, 0), content.Load<Texture2D>(Paths.INTERFACE_SCRAPBOOK_PAGE_ACCESSORIES_NATURAL1), Color.White),
                new ScrapbookPage.CookingRecipeComponent(cookingRecipe1Pos, GameState.GetAccessoryRecipeForResult(ItemDict.CHURN_CHARM), numbers, ScrapbookPage.CookingRecipeComponent.RecipeType.ACCESSORY),
                new ScrapbookPage.CookingRecipeComponent(cookingRecipe2Pos, GameState.GetAccessoryRecipeForResult(ItemDict.PRIMAL_CHARM), numbers, ScrapbookPage.CookingRecipeComponent.RecipeType.ACCESSORY),
                new ScrapbookPage.CookingRecipeComponent(cookingRecipe3Pos, GameState.GetAccessoryRecipeForResult(ItemDict.SUNRISE_CHARM), numbers, ScrapbookPage.CookingRecipeComponent.RecipeType.ACCESSORY),
                new ScrapbookPage.CookingRecipeComponent(cookingRecipe4Pos, GameState.GetAccessoryRecipeForResult(ItemDict.VOLCANIC_CHARM), numbers, ScrapbookPage.CookingRecipeComponent.RecipeType.ACCESSORY),
                new ScrapbookPage.CookingRecipeComponent(cookingRecipe5Pos, GameState.GetAccessoryRecipeForResult(ItemDict.MUSHY_CHARM), numbers, ScrapbookPage.CookingRecipeComponent.RecipeType.ACCESSORY),
                new ScrapbookPage.CookingRecipeComponent(cookingRecipe6Pos, GameState.GetAccessoryRecipeForResult(ItemDict.LUMINOUS_RING), numbers, ScrapbookPage.CookingRecipeComponent.RecipeType.ACCESSORY)));
            pages.Add(new ScrapbookPage("Acces. Nature 2", new ScrapbookPage.ImageComponent(new Vector2(0, 0), content.Load<Texture2D>(Paths.INTERFACE_SCRAPBOOK_PAGE_ACCESSORIES_NATURAL2), Color.White),
                new ScrapbookPage.CookingRecipeComponent(cookingRecipe1Pos, GameState.GetAccessoryRecipeForResult(ItemDict.FLORAL_RING), numbers, ScrapbookPage.CookingRecipeComponent.RecipeType.ACCESSORY),
                new ScrapbookPage.CookingRecipeComponent(cookingRecipe2Pos, GameState.GetAccessoryRecipeForResult(ItemDict.MUSICBOX_RING), numbers, ScrapbookPage.CookingRecipeComponent.RecipeType.ACCESSORY),
                new ScrapbookPage.CookingRecipeComponent(cookingRecipe3Pos, GameState.GetAccessoryRecipeForResult(ItemDict.STRIPED_BRACER), numbers, ScrapbookPage.CookingRecipeComponent.RecipeType.ACCESSORY),
                new ScrapbookPage.CookingRecipeComponent(cookingRecipe4Pos, GameState.GetAccessoryRecipeForResult(ItemDict.PIN_BRACER), numbers, ScrapbookPage.CookingRecipeComponent.RecipeType.ACCESSORY),
                new ScrapbookPage.CookingRecipeComponent(cookingRecipe5Pos, GameState.GetAccessoryRecipeForResult(ItemDict.DISSECTION_PENDANT), numbers, ScrapbookPage.CookingRecipeComponent.RecipeType.ACCESSORY),
                new ScrapbookPage.CookingRecipeComponent(cookingRecipe6Pos, GameState.GetAccessoryRecipeForResult(ItemDict.GAIA_PENDANT), numbers, ScrapbookPage.CookingRecipeComponent.RecipeType.ACCESSORY)));
            pages.Add(new ScrapbookPage("Acces. Nature 3", new ScrapbookPage.ImageComponent(new Vector2(0, 0), content.Load<Texture2D>(Paths.INTERFACE_SCRAPBOOK_PAGE_ACCESSORIES_NATURAL3), Color.White),
                new ScrapbookPage.CookingRecipeComponent(cookingRecipe1Pos, GameState.GetAccessoryRecipeForResult(ItemDict.CONTRACT_PENDANT), numbers, ScrapbookPage.CookingRecipeComponent.RecipeType.ACCESSORY),
                new ScrapbookPage.CookingRecipeComponent(cookingRecipe2Pos, GameState.GetAccessoryRecipeForResult(ItemDict.DYNAMITE_PENDANT), numbers, ScrapbookPage.CookingRecipeComponent.RecipeType.ACCESSORY),
                new ScrapbookPage.CookingRecipeComponent(cookingRecipe3Pos, GameState.GetAccessoryRecipeForResult(ItemDict.OILY_PENDANT), numbers, ScrapbookPage.CookingRecipeComponent.RecipeType.ACCESSORY),
                new ScrapbookPage.CookingRecipeComponent(cookingRecipe4Pos, GameState.GetAccessoryRecipeForResult(ItemDict.NEUTRALIZED_PENDANT), numbers, ScrapbookPage.CookingRecipeComponent.RecipeType.ACCESSORY)));
            pages.Add(new ScrapbookPage("Acces. Jewelry 1", new ScrapbookPage.ImageComponent(new Vector2(0, 0), content.Load<Texture2D>(Paths.INTERFACE_SCRAPBOOK_PAGE_ACCESSORIES_JEWELERY1), Color.White),
                new ScrapbookPage.CookingRecipeComponent(cookingRecipe1Pos, GameState.GetAccessoryRecipeForResult(ItemDict.PHILOSOPHERS_CREST), numbers, ScrapbookPage.CookingRecipeComponent.RecipeType.ACCESSORY),
                new ScrapbookPage.CookingRecipeComponent(cookingRecipe2Pos, GameState.GetAccessoryRecipeForResult(ItemDict.BLIND_RING), numbers, ScrapbookPage.CookingRecipeComponent.RecipeType.ACCESSORY),
                new ScrapbookPage.CookingRecipeComponent(cookingRecipe3Pos, GameState.GetAccessoryRecipeForResult(ItemDict.FLIGHT_RING), numbers, ScrapbookPage.CookingRecipeComponent.RecipeType.ACCESSORY),
                new ScrapbookPage.CookingRecipeComponent(cookingRecipe4Pos, GameState.GetAccessoryRecipeForResult(ItemDict.GLIMMER_RING), numbers, ScrapbookPage.CookingRecipeComponent.RecipeType.ACCESSORY),
                new ScrapbookPage.CookingRecipeComponent(cookingRecipe5Pos, GameState.GetAccessoryRecipeForResult(ItemDict.MONOCULTURE_RING), numbers, ScrapbookPage.CookingRecipeComponent.RecipeType.ACCESSORY),
                new ScrapbookPage.CookingRecipeComponent(cookingRecipe6Pos, GameState.GetAccessoryRecipeForResult(ItemDict.LUMBER_RING), numbers, ScrapbookPage.CookingRecipeComponent.RecipeType.ACCESSORY)));
            pages.Add(new ScrapbookPage("Acces. Jewelry 2", new ScrapbookPage.ImageComponent(new Vector2(0, 0), content.Load<Texture2D>(Paths.INTERFACE_SCRAPBOOK_PAGE_ACCESSORIES_JEWELERY2), Color.White),
                new ScrapbookPage.CookingRecipeComponent(cookingRecipe1Pos, GameState.GetAccessoryRecipeForResult(ItemDict.BAKERY_RING), numbers, ScrapbookPage.CookingRecipeComponent.RecipeType.ACCESSORY),
                new ScrapbookPage.CookingRecipeComponent(cookingRecipe2Pos, GameState.GetAccessoryRecipeForResult(ItemDict.ROSE_RING), numbers, ScrapbookPage.CookingRecipeComponent.RecipeType.ACCESSORY),
                new ScrapbookPage.CookingRecipeComponent(cookingRecipe3Pos, GameState.GetAccessoryRecipeForResult(ItemDict.SHELL_RING), numbers, ScrapbookPage.CookingRecipeComponent.RecipeType.ACCESSORY),
                new ScrapbookPage.CookingRecipeComponent(cookingRecipe4Pos, GameState.GetAccessoryRecipeForResult(ItemDict.FURNACE_RING), numbers, ScrapbookPage.CookingRecipeComponent.RecipeType.ACCESSORY),
                new ScrapbookPage.CookingRecipeComponent(cookingRecipe5Pos, GameState.GetAccessoryRecipeForResult(ItemDict.SOUND_PENDANT), numbers, ScrapbookPage.CookingRecipeComponent.RecipeType.ACCESSORY),
                new ScrapbookPage.CookingRecipeComponent(cookingRecipe6Pos, GameState.GetAccessoryRecipeForResult(ItemDict.EROSION_PENDANT), numbers, ScrapbookPage.CookingRecipeComponent.RecipeType.ACCESSORY)));
            pages.Add(new ScrapbookPage("Acces. Jewelry 3", new ScrapbookPage.ImageComponent(new Vector2(0, 0), content.Load<Texture2D>(Paths.INTERFACE_SCRAPBOOK_PAGE_ACCESSORIES_JEWELERY3), Color.White),
                new ScrapbookPage.CookingRecipeComponent(cookingRecipe1Pos, GameState.GetAccessoryRecipeForResult(ItemDict.POLYCULTURE_PENDANT), numbers, ScrapbookPage.CookingRecipeComponent.RecipeType.ACCESSORY),
                new ScrapbookPage.CookingRecipeComponent(cookingRecipe2Pos, GameState.GetAccessoryRecipeForResult(ItemDict.LADYBUG_PENDANT), numbers, ScrapbookPage.CookingRecipeComponent.RecipeType.ACCESSORY),
                new ScrapbookPage.CookingRecipeComponent(cookingRecipe3Pos, GameState.GetAccessoryRecipeForResult(ItemDict.STREAMLINE_PENDANT), numbers, ScrapbookPage.CookingRecipeComponent.RecipeType.ACCESSORY),
                new ScrapbookPage.CookingRecipeComponent(cookingRecipe4Pos, GameState.GetAccessoryRecipeForResult(ItemDict.TORNADO_PENDANT), numbers, ScrapbookPage.CookingRecipeComponent.RecipeType.ACCESSORY)));
            pages.Add(new ScrapbookPage("Acces. Shaman 1", new ScrapbookPage.ImageComponent(new Vector2(0, 0), content.Load<Texture2D>(Paths.INTERFACE_SCRAPBOOK_PAGE_ACCESSORIES_SHAMAN1), Color.White),
                new ScrapbookPage.CookingRecipeComponent(cookingRecipe1Pos, GameState.GetAccessoryRecipeForResult(ItemDict.ROYAL_CREST), numbers, ScrapbookPage.CookingRecipeComponent.RecipeType.ACCESSORY),
                new ScrapbookPage.CookingRecipeComponent(cookingRecipe2Pos, GameState.GetAccessoryRecipeForResult(ItemDict.MIDIAN_SYMBOL), numbers, ScrapbookPage.CookingRecipeComponent.RecipeType.ACCESSORY),
                new ScrapbookPage.CookingRecipeComponent(cookingRecipe3Pos, GameState.GetAccessoryRecipeForResult(ItemDict.UNITY_CREST), numbers, ScrapbookPage.CookingRecipeComponent.RecipeType.ACCESSORY),
                new ScrapbookPage.CookingRecipeComponent(cookingRecipe4Pos, GameState.GetAccessoryRecipeForResult(ItemDict.COMPRESSION_CREST), numbers, ScrapbookPage.CookingRecipeComponent.RecipeType.ACCESSORY),
                new ScrapbookPage.CookingRecipeComponent(cookingRecipe5Pos, GameState.GetAccessoryRecipeForResult(ItemDict.POLYMORPH_CREST), numbers, ScrapbookPage.CookingRecipeComponent.RecipeType.ACCESSORY),
                new ScrapbookPage.CookingRecipeComponent(cookingRecipe6Pos, GameState.GetAccessoryRecipeForResult(ItemDict.DASHING_CREST), numbers, ScrapbookPage.CookingRecipeComponent.RecipeType.ACCESSORY)));
            pages.Add(new ScrapbookPage("Acces. Shaman 2", new ScrapbookPage.ImageComponent(new Vector2(0, 0), content.Load<Texture2D>(Paths.INTERFACE_SCRAPBOOK_PAGE_ACCESSORIES_SHAMAN2), Color.White),
                new ScrapbookPage.CookingRecipeComponent(cookingRecipe1Pos, GameState.GetAccessoryRecipeForResult(ItemDict.FROZEN_CREST), numbers, ScrapbookPage.CookingRecipeComponent.RecipeType.ACCESSORY),
                new ScrapbookPage.CookingRecipeComponent(cookingRecipe2Pos, GameState.GetAccessoryRecipeForResult(ItemDict.MUTATING_CREST), numbers, ScrapbookPage.CookingRecipeComponent.RecipeType.ACCESSORY),
                new ScrapbookPage.CookingRecipeComponent(cookingRecipe3Pos, GameState.GetAccessoryRecipeForResult(ItemDict.MYTHICAL_CREST), numbers, ScrapbookPage.CookingRecipeComponent.RecipeType.ACCESSORY),
                new ScrapbookPage.CookingRecipeComponent(cookingRecipe4Pos, GameState.GetAccessoryRecipeForResult(ItemDict.VAMPYRIC_CREST), numbers, ScrapbookPage.CookingRecipeComponent.RecipeType.ACCESSORY),
                new ScrapbookPage.CookingRecipeComponent(cookingRecipe5Pos, GameState.GetAccessoryRecipeForResult(ItemDict.BREWERY_CREST), numbers, ScrapbookPage.CookingRecipeComponent.RecipeType.ACCESSORY),
                new ScrapbookPage.CookingRecipeComponent(cookingRecipe6Pos, GameState.GetAccessoryRecipeForResult(ItemDict.CLOUD_CREST), numbers, ScrapbookPage.CookingRecipeComponent.RecipeType.ACCESSORY)));
            pages.Add(new ScrapbookPage("Acces. Shaman 3", new ScrapbookPage.ImageComponent(new Vector2(0, 0), content.Load<Texture2D>(Paths.INTERFACE_SCRAPBOOK_PAGE_ACCESSORIES_SHAMAN3), Color.White),
                new ScrapbookPage.CookingRecipeComponent(cookingRecipe1Pos, GameState.GetAccessoryRecipeForResult(ItemDict.OCEANIC_RING), numbers, ScrapbookPage.CookingRecipeComponent.RecipeType.ACCESSORY),
                new ScrapbookPage.CookingRecipeComponent(cookingRecipe2Pos, GameState.GetAccessoryRecipeForResult(ItemDict.CYCLE_PENDANT), numbers, ScrapbookPage.CookingRecipeComponent.RecipeType.ACCESSORY)));
            pages.Add(new ScrapbookPage("Alchemy Dusty", new ScrapbookPage.ImageComponent(new Vector2(0, 0), content.Load<Texture2D>(Paths.INTERFACE_SCRAPBOOK_PAGE_ALCHEMY_DUSTY), Color.White),
                new ScrapbookPage.CookingRecipeComponent(cookingRecipe1Pos, GameState.GetAlchemyRecipeForResult(ItemDict.BLACK_CANDLE), numbers, ScrapbookPage.CookingRecipeComponent.RecipeType.ALCHEMY),
                new ScrapbookPage.CookingRecipeComponent(cookingRecipe2Pos, GameState.GetAlchemyRecipeForResult(ItemDict.SALTED_CANDLE), numbers, ScrapbookPage.CookingRecipeComponent.RecipeType.ALCHEMY),
                new ScrapbookPage.CookingRecipeComponent(cookingRecipe3Pos, GameState.GetAlchemyRecipeForResult(ItemDict.SPICED_CANDLE), numbers, ScrapbookPage.CookingRecipeComponent.RecipeType.ALCHEMY),
                new ScrapbookPage.CookingRecipeComponent(cookingRecipe4Pos, GameState.GetAlchemyRecipeForResult(ItemDict.MOSS_BOTTLE), numbers, ScrapbookPage.CookingRecipeComponent.RecipeType.ALCHEMY),
                new ScrapbookPage.CookingRecipeComponent(cookingRecipe5Pos, GameState.GetAlchemyRecipeForResult(ItemDict.SHIMMERING_SALVE), numbers, ScrapbookPage.CookingRecipeComponent.RecipeType.ALCHEMY),
                new ScrapbookPage.CookingRecipeComponent(cookingRecipe6Pos, GameState.GetAlchemyRecipeForResult(ItemDict.VOODOO_STEW), numbers, ScrapbookPage.CookingRecipeComponent.RecipeType.ALCHEMY)));
            pages.Add(new ScrapbookPage("Alchemy Musty", new ScrapbookPage.ImageComponent(new Vector2(0, 0), content.Load<Texture2D>(Paths.INTERFACE_SCRAPBOOK_PAGE_ALCHEMY_MUSTY), Color.White),
                new ScrapbookPage.CookingRecipeComponent(cookingRecipe1Pos, GameState.GetAlchemyRecipeForResult(ItemDict.SUGAR_CANDLE), numbers, ScrapbookPage.CookingRecipeComponent.RecipeType.ALCHEMY),
                new ScrapbookPage.CookingRecipeComponent(cookingRecipe2Pos, GameState.GetAlchemyRecipeForResult(ItemDict.SKY_BOTTLE), numbers, ScrapbookPage.CookingRecipeComponent.RecipeType.ALCHEMY),
                new ScrapbookPage.CookingRecipeComponent(cookingRecipe3Pos, GameState.GetAlchemyRecipeForResult(ItemDict.HEART_VESSEL), numbers, ScrapbookPage.CookingRecipeComponent.RecipeType.ALCHEMY),
                new ScrapbookPage.CookingRecipeComponent(cookingRecipe4Pos, GameState.GetAlchemyRecipeForResult(ItemDict.INVINCIROID), numbers, ScrapbookPage.CookingRecipeComponent.RecipeType.ALCHEMY),
                new ScrapbookPage.CookingRecipeComponent(cookingRecipe5Pos, GameState.GetAlchemyRecipeForResult(ItemDict.ADAMANTITE_BAR), numbers, ScrapbookPage.CookingRecipeComponent.RecipeType.ALCHEMY)));
            pages.Add(new ScrapbookPage("Alchemy Mystica 1", new ScrapbookPage.ImageComponent(new Vector2(0, 0), content.Load<Texture2D>(Paths.INTERFACE_SCRAPBOOK_PAGE_ALCHEMY_MYSTICA1), Color.White),
                new ScrapbookPage.CookingRecipeComponent(cookingRecipe1Pos, GameState.GetAlchemyRecipeForResult(ItemDict.SOOTHE_CANDLE), numbers, ScrapbookPage.CookingRecipeComponent.RecipeType.ALCHEMY),
                new ScrapbookPage.CookingRecipeComponent(cookingRecipe2Pos, GameState.GetAlchemyRecipeForResult(ItemDict.BURST_STONE), numbers, ScrapbookPage.CookingRecipeComponent.RecipeType.ALCHEMY),
                new ScrapbookPage.CookingRecipeComponent(cookingRecipe3Pos, GameState.GetAlchemyRecipeForResult(ItemDict.UNSTABLE_LIQUID), numbers, ScrapbookPage.CookingRecipeComponent.RecipeType.ALCHEMY),
                new ScrapbookPage.CookingRecipeComponent(cookingRecipe4Pos, GameState.GetAlchemyRecipeForResult(ItemDict.PHILOSOPHERS_STONE), numbers, ScrapbookPage.CookingRecipeComponent.RecipeType.ALCHEMY),
                new ScrapbookPage.CookingRecipeComponent(cookingRecipe5Pos, GameState.GetAlchemyRecipeForResult(ItemDict.GOLD_BAR), numbers, ScrapbookPage.CookingRecipeComponent.RecipeType.ALCHEMY),
                new ScrapbookPage.CookingRecipeComponent(cookingRecipe6Pos, GameState.GetAlchemyRecipeForResult(ItemDict.MYTHRIL_BAR), numbers, ScrapbookPage.CookingRecipeComponent.RecipeType.ALCHEMY)));
            pages.Add(new ScrapbookPage("Alchemy Mystica 2", new ScrapbookPage.ImageComponent(new Vector2(0, 0), content.Load<Texture2D>(Paths.INTERFACE_SCRAPBOOK_PAGE_ALCHEMY_MYSTICA2), Color.White),
                new ScrapbookPage.CookingRecipeComponent(cookingRecipe1Pos, GameState.GetAlchemyRecipeForResult(ItemDict.TROPICAL_BOTTLE), numbers, ScrapbookPage.CookingRecipeComponent.RecipeType.ALCHEMY)));
            pages.Add(new ScrapbookPage("Alchemy Incense", new ScrapbookPage.ImageComponent(new Vector2(0, 0), content.Load<Texture2D>(Paths.INTERFACE_SCRAPBOOK_PAGE_ALCHEMY_INCENSE), Color.White),
                new ScrapbookPage.CookingRecipeComponent(cookingRecipe1Pos, GameState.GetAlchemyRecipeForResult(ItemDict.IMPERIAL_INCENSE), numbers, ScrapbookPage.CookingRecipeComponent.RecipeType.ALCHEMY),
                new ScrapbookPage.CookingRecipeComponent(cookingRecipe2Pos, GameState.GetAlchemyRecipeForResult(ItemDict.SWEET_INCENSE), numbers, ScrapbookPage.CookingRecipeComponent.RecipeType.ALCHEMY),
                new ScrapbookPage.CookingRecipeComponent(cookingRecipe3Pos, GameState.GetAlchemyRecipeForResult(ItemDict.LAVENDER_INCENSE), numbers, ScrapbookPage.CookingRecipeComponent.RecipeType.ALCHEMY),
                new ScrapbookPage.CookingRecipeComponent(cookingRecipe4Pos, GameState.GetAlchemyRecipeForResult(ItemDict.COLD_INCENSE), numbers, ScrapbookPage.CookingRecipeComponent.RecipeType.ALCHEMY),
                new ScrapbookPage.CookingRecipeComponent(cookingRecipe5Pos, GameState.GetAlchemyRecipeForResult(ItemDict.FRESH_INCENSE), numbers, ScrapbookPage.CookingRecipeComponent.RecipeType.ALCHEMY)));
            pages.Add(new ScrapbookPage("Alchemy Elements", new ScrapbookPage.ImageComponent(new Vector2(0, 0), content.Load<Texture2D>(Paths.INTERFACE_SCRAPBOOK_PAGE_ALCHEMY_ELEMENTS), Color.White),
                new ScrapbookPage.CookingRecipeComponent(cookingRecipe1Pos, GameState.GetAlchemyRecipeForResult(ItemDict.SKY_ELEMENT), numbers, ScrapbookPage.CookingRecipeComponent.RecipeType.ALCHEMY),
                new ScrapbookPage.CookingRecipeComponent(cookingRecipe2Pos, GameState.GetAlchemyRecipeForResult(ItemDict.SEA_ELEMENT), numbers, ScrapbookPage.CookingRecipeComponent.RecipeType.ALCHEMY),
                new ScrapbookPage.CookingRecipeComponent(cookingRecipe3Pos, GameState.GetAlchemyRecipeForResult(ItemDict.LAND_ELEMENT), numbers, ScrapbookPage.CookingRecipeComponent.RecipeType.ALCHEMY),
                new ScrapbookPage.CookingRecipeComponent(cookingRecipe4Pos, GameState.GetAlchemyRecipeForResult(ItemDict.PRIMEVAL_ELEMENT), numbers, ScrapbookPage.CookingRecipeComponent.RecipeType.ALCHEMY)));
            pages.Add(new ScrapbookPage("Blueprint Wood", new ScrapbookPage.ImageComponent(new Vector2(0, 0), content.Load<Texture2D>(Paths.INTERFACE_SCRAPBOOK_PAGE_BLUEPRINTS_WOODWORKING), Color.White),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(0), blueprintTexture, GameState.SIMPLY_WOODWORKING_UNLOCKS[0]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(1), blueprintTexture, GameState.SIMPLY_WOODWORKING_UNLOCKS[1]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(2), blueprintTexture, GameState.SIMPLY_WOODWORKING_UNLOCKS[2]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(3), blueprintTexture, GameState.SIMPLY_WOODWORKING_UNLOCKS[3]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(4), blueprintTexture, GameState.SIMPLY_WOODWORKING_UNLOCKS[4]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(5), blueprintTexture, GameState.SIMPLY_WOODWORKING_UNLOCKS[5]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(6), blueprintTexture, GameState.SIMPLY_WOODWORKING_UNLOCKS[6]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(7), blueprintTexture, GameState.SIMPLY_WOODWORKING_UNLOCKS[7]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(8), blueprintTexture, GameState.SIMPLY_WOODWORKING_UNLOCKS[8]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(9), blueprintTexture, GameState.SIMPLY_WOODWORKING_UNLOCKS[9]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(10), blueprintTexture, GameState.SIMPLY_WOODWORKING_UNLOCKS[10]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(11), blueprintTexture, GameState.SIMPLY_WOODWORKING_UNLOCKS[11])));
            pages.Add(new ScrapbookPage("Blueprint Farm", new ScrapbookPage.ImageComponent(new Vector2(0, 0), content.Load<Texture2D>(Paths.INTERFACE_SCRAPBOOK_PAGE_BLUEPRINTS_FARMSTEADS), Color.White),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(0), blueprintTexture, GameState.FABULOUS_FARMSTEADS_UNLOCKS[0]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(1), blueprintTexture, GameState.FABULOUS_FARMSTEADS_UNLOCKS[1]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(2), blueprintTexture, GameState.FABULOUS_FARMSTEADS_UNLOCKS[2]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(3), blueprintTexture, GameState.FABULOUS_FARMSTEADS_UNLOCKS[3]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(4), blueprintTexture, GameState.FABULOUS_FARMSTEADS_UNLOCKS[4]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(5), blueprintTexture, GameState.FABULOUS_FARMSTEADS_UNLOCKS[5]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(6), blueprintTexture, GameState.FABULOUS_FARMSTEADS_UNLOCKS[6]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(7), blueprintTexture, GameState.FABULOUS_FARMSTEADS_UNLOCKS[7]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(8), blueprintTexture, GameState.FABULOUS_FARMSTEADS_UNLOCKS[8]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(9), blueprintTexture, GameState.FABULOUS_FARMSTEADS_UNLOCKS[9]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(10), blueprintTexture, GameState.FABULOUS_FARMSTEADS_UNLOCKS[10]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(11), blueprintTexture, GameState.FABULOUS_FARMSTEADS_UNLOCKS[11]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(12), blueprintTexture, GameState.FABULOUS_FARMSTEADS_UNLOCKS[12]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(13), blueprintTexture, GameState.FABULOUS_FARMSTEADS_UNLOCKS[13]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(14), blueprintTexture, GameState.FABULOUS_FARMSTEADS_UNLOCKS[14]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(15), blueprintTexture, GameState.FABULOUS_FARMSTEADS_UNLOCKS[15]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(16), blueprintTexture, GameState.FABULOUS_FARMSTEADS_UNLOCKS[16]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(17), blueprintTexture, GameState.FABULOUS_FARMSTEADS_UNLOCKS[17]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(18), blueprintTexture, GameState.FABULOUS_FARMSTEADS_UNLOCKS[18]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(19), blueprintTexture, GameState.FABULOUS_FARMSTEADS_UNLOCKS[19]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(20), blueprintTexture, GameState.FABULOUS_FARMSTEADS_UNLOCKS[20]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(21), blueprintTexture, GameState.FABULOUS_FARMSTEADS_UNLOCKS[21]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(22), blueprintTexture, GameState.FABULOUS_FARMSTEADS_UNLOCKS[22]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(23), blueprintTexture, GameState.FABULOUS_FARMSTEADS_UNLOCKS[23])));
            pages.Add(new ScrapbookPage("Blueprint Stone", new ScrapbookPage.ImageComponent(new Vector2(0, 0), content.Load<Texture2D>(Paths.INTERFACE_SCRAPBOOK_PAGE_BLUEPRINTS_STONECARVING), Color.White),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(0), blueprintTexture, GameState.CRAVING_STONECARVING_UNLOCKS[0]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(1), blueprintTexture, GameState.CRAVING_STONECARVING_UNLOCKS[1]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(2), blueprintTexture, GameState.CRAVING_STONECARVING_UNLOCKS[2]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(3), blueprintTexture, GameState.CRAVING_STONECARVING_UNLOCKS[3]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(4), blueprintTexture, GameState.CRAVING_STONECARVING_UNLOCKS[4]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(5), blueprintTexture, GameState.CRAVING_STONECARVING_UNLOCKS[5]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(6), blueprintTexture, GameState.CRAVING_STONECARVING_UNLOCKS[6]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(7), blueprintTexture, GameState.CRAVING_STONECARVING_UNLOCKS[7]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(8), blueprintTexture, GameState.CRAVING_STONECARVING_UNLOCKS[8]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(9), blueprintTexture, GameState.CRAVING_STONECARVING_UNLOCKS[9]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(10), blueprintTexture, GameState.CRAVING_STONECARVING_UNLOCKS[10]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(11), blueprintTexture, GameState.CRAVING_STONECARVING_UNLOCKS[11]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(12), blueprintTexture, GameState.CRAVING_STONECARVING_UNLOCKS[12]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(13), blueprintTexture, GameState.CRAVING_STONECARVING_UNLOCKS[13])));
            pages.Add(new ScrapbookPage("Blueprint Nature", new ScrapbookPage.ImageComponent(new Vector2(0, 0), content.Load<Texture2D>(Paths.INTERFACE_SCRAPBOOK_PAGE_BLUEPRINTS_NATURE), Color.White),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(0), blueprintTexture, GameState.A_TOUCH_OF_NATURE_UNLOCKS[0]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(1), blueprintTexture, GameState.A_TOUCH_OF_NATURE_UNLOCKS[1]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(2), blueprintTexture, GameState.A_TOUCH_OF_NATURE_UNLOCKS[2]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(3), blueprintTexture, GameState.A_TOUCH_OF_NATURE_UNLOCKS[3]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(4), blueprintTexture, GameState.A_TOUCH_OF_NATURE_UNLOCKS[4]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(5), blueprintTexture, GameState.A_TOUCH_OF_NATURE_UNLOCKS[5]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(6), blueprintTexture, GameState.A_TOUCH_OF_NATURE_UNLOCKS[6]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(7), blueprintTexture, GameState.A_TOUCH_OF_NATURE_UNLOCKS[7]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(8), blueprintTexture, GameState.A_TOUCH_OF_NATURE_UNLOCKS[8]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(9), blueprintTexture, GameState.A_TOUCH_OF_NATURE_UNLOCKS[9]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(10), blueprintTexture, GameState.A_TOUCH_OF_NATURE_UNLOCKS[10]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(11), blueprintTexture, GameState.A_TOUCH_OF_NATURE_UNLOCKS[11]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(12), blueprintTexture, GameState.A_TOUCH_OF_NATURE_UNLOCKS[12]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(13), blueprintTexture, GameState.A_TOUCH_OF_NATURE_UNLOCKS[13]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(14), blueprintTexture, GameState.A_TOUCH_OF_NATURE_UNLOCKS[14]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(15), blueprintTexture, GameState.A_TOUCH_OF_NATURE_UNLOCKS[15]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(16), blueprintTexture, GameState.A_TOUCH_OF_NATURE_UNLOCKS[16]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(17), blueprintTexture, GameState.A_TOUCH_OF_NATURE_UNLOCKS[17]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(18), blueprintTexture, GameState.A_TOUCH_OF_NATURE_UNLOCKS[18]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(19), blueprintTexture, GameState.A_TOUCH_OF_NATURE_UNLOCKS[19]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(20), blueprintTexture, GameState.A_TOUCH_OF_NATURE_UNLOCKS[20]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(21), blueprintTexture, GameState.A_TOUCH_OF_NATURE_UNLOCKS[21])));
            pages.Add(new ScrapbookPage("Blueprint Reflect", new ScrapbookPage.ImageComponent(new Vector2(0, 0), content.Load<Texture2D>(Paths.INTERFACE_SCRAPBOOK_PAGE_BLUEPRINTS_REFLECTION), Color.White),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(0), blueprintTexture, GameState.AN_ARTISTS_REFLECTION_UNLOCKS[0]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(1), blueprintTexture, GameState.AN_ARTISTS_REFLECTION_UNLOCKS[1]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(2), blueprintTexture, GameState.AN_ARTISTS_REFLECTION_UNLOCKS[2]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(3), blueprintTexture, GameState.AN_ARTISTS_REFLECTION_UNLOCKS[3]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(4), blueprintTexture, GameState.AN_ARTISTS_REFLECTION_UNLOCKS[4]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(5), blueprintTexture, GameState.AN_ARTISTS_REFLECTION_UNLOCKS[5]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(6), blueprintTexture, GameState.AN_ARTISTS_REFLECTION_UNLOCKS[6]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(7), blueprintTexture, GameState.AN_ARTISTS_REFLECTION_UNLOCKS[7]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(8), blueprintTexture, GameState.AN_ARTISTS_REFLECTION_UNLOCKS[8]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(9), blueprintTexture, GameState.AN_ARTISTS_REFLECTION_UNLOCKS[9]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(10), blueprintTexture, GameState.AN_ARTISTS_REFLECTION_UNLOCKS[10]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(11), blueprintTexture, GameState.AN_ARTISTS_REFLECTION_UNLOCKS[11]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(12), blueprintTexture, GameState.AN_ARTISTS_REFLECTION_UNLOCKS[12])));
            pages.Add(new ScrapbookPage("Blueprint Play", new ScrapbookPage.ImageComponent(new Vector2(0, 0), content.Load<Texture2D>(Paths.INTERFACE_SCRAPBOOK_PAGE_BLUEPRINTS_PLAYGROUND), Color.White),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(0), blueprintTexture, GameState.PLAYGROUND_PREP_UNLOCKS[0]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(1), blueprintTexture, GameState.PLAYGROUND_PREP_UNLOCKS[1]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(2), blueprintTexture, GameState.PLAYGROUND_PREP_UNLOCKS[2]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(3), blueprintTexture, GameState.PLAYGROUND_PREP_UNLOCKS[3]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(4), blueprintTexture, GameState.PLAYGROUND_PREP_UNLOCKS[4]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(5), blueprintTexture, GameState.PLAYGROUND_PREP_UNLOCKS[5]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(6), blueprintTexture, GameState.PLAYGROUND_PREP_UNLOCKS[6]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(7), blueprintTexture, GameState.PLAYGROUND_PREP_UNLOCKS[7]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(8), blueprintTexture, GameState.PLAYGROUND_PREP_UNLOCKS[8]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(9), blueprintTexture, GameState.PLAYGROUND_PREP_UNLOCKS[9]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(10), blueprintTexture, GameState.PLAYGROUND_PREP_UNLOCKS[10]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(11), blueprintTexture, GameState.PLAYGROUND_PREP_UNLOCKS[11]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(12), blueprintTexture, GameState.PLAYGROUND_PREP_UNLOCKS[12]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(13), blueprintTexture, GameState.PLAYGROUND_PREP_UNLOCKS[13]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(14), blueprintTexture, GameState.PLAYGROUND_PREP_UNLOCKS[14])));
            pages.Add(new ScrapbookPage("Blueprint Musical", new ScrapbookPage.ImageComponent(new Vector2(0, 0), content.Load<Texture2D>(Paths.INTERFACE_SCRAPBOOK_PAGE_BLUEPRINTS_MUSIC), Color.White),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(0), blueprintTexture, GameState.MUSIC_AT_HOME_UNLOCKS[0]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(1), blueprintTexture, GameState.MUSIC_AT_HOME_UNLOCKS[1]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(2), blueprintTexture, GameState.MUSIC_AT_HOME_UNLOCKS[2]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(3), blueprintTexture, GameState.MUSIC_AT_HOME_UNLOCKS[3]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(4), blueprintTexture, GameState.MUSIC_AT_HOME_UNLOCKS[4]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(5), blueprintTexture, GameState.MUSIC_AT_HOME_UNLOCKS[5])));
            pages.Add(new ScrapbookPage("Blueprint Tech", new ScrapbookPage.ImageComponent(new Vector2(0, 0), content.Load<Texture2D>(Paths.INTERFACE_SCRAPBOOK_PAGE_BLUEPRINTS_ENGINEERING), Color.White),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(0), blueprintTexture, GameState.ESSENTIAL_ENGINEERING_UNLOCKS[0]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(1), blueprintTexture, GameState.ESSENTIAL_ENGINEERING_UNLOCKS[1]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(2), blueprintTexture, GameState.ESSENTIAL_ENGINEERING_UNLOCKS[2]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(3), blueprintTexture, GameState.ESSENTIAL_ENGINEERING_UNLOCKS[3]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(4), blueprintTexture, GameState.ESSENTIAL_ENGINEERING_UNLOCKS[4]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(5), blueprintTexture, GameState.ESSENTIAL_ENGINEERING_UNLOCKS[5]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(6), blueprintTexture, GameState.ESSENTIAL_ENGINEERING_UNLOCKS[6]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(7), blueprintTexture, GameState.ESSENTIAL_ENGINEERING_UNLOCKS[7]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(8), blueprintTexture, GameState.ESSENTIAL_ENGINEERING_UNLOCKS[8]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(9), blueprintTexture, GameState.ESSENTIAL_ENGINEERING_UNLOCKS[9]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(10), blueprintTexture, GameState.ESSENTIAL_ENGINEERING_UNLOCKS[10]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(11), blueprintTexture, GameState.ESSENTIAL_ENGINEERING_UNLOCKS[11]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(12), blueprintTexture, GameState.ESSENTIAL_ENGINEERING_UNLOCKS[12]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(13), blueprintTexture, GameState.ESSENTIAL_ENGINEERING_UNLOCKS[13]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(14), blueprintTexture, GameState.ESSENTIAL_ENGINEERING_UNLOCKS[14])));
            pages.Add(new ScrapbookPage("Blueprint Urban", new ScrapbookPage.ImageComponent(new Vector2(0, 0), content.Load<Texture2D>(Paths.INTERFACE_SCRAPBOOK_PAGE_BLUEPRINTS_URBAN), Color.White),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(0), blueprintTexture, GameState.URBAN_DESIGN_BIBLE_UNLOCKS[0]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(1), blueprintTexture, GameState.URBAN_DESIGN_BIBLE_UNLOCKS[1]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(2), blueprintTexture, GameState.URBAN_DESIGN_BIBLE_UNLOCKS[2]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(3), blueprintTexture, GameState.URBAN_DESIGN_BIBLE_UNLOCKS[3]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(4), blueprintTexture, GameState.URBAN_DESIGN_BIBLE_UNLOCKS[4]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(5), blueprintTexture, GameState.URBAN_DESIGN_BIBLE_UNLOCKS[5]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(6), blueprintTexture, GameState.URBAN_DESIGN_BIBLE_UNLOCKS[6]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(7), blueprintTexture, GameState.URBAN_DESIGN_BIBLE_UNLOCKS[7]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(8), blueprintTexture, GameState.URBAN_DESIGN_BIBLE_UNLOCKS[8]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(9), blueprintTexture, GameState.URBAN_DESIGN_BIBLE_UNLOCKS[9]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(10), blueprintTexture, GameState.URBAN_DESIGN_BIBLE_UNLOCKS[10]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(11), blueprintTexture, GameState.URBAN_DESIGN_BIBLE_UNLOCKS[11]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(12), blueprintTexture, GameState.URBAN_DESIGN_BIBLE_UNLOCKS[12]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(13), blueprintTexture, GameState.URBAN_DESIGN_BIBLE_UNLOCKS[13]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(14), blueprintTexture, GameState.URBAN_DESIGN_BIBLE_UNLOCKS[14]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(15), blueprintTexture, GameState.URBAN_DESIGN_BIBLE_UNLOCKS[15]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(16), blueprintTexture, GameState.URBAN_DESIGN_BIBLE_UNLOCKS[16]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(17), blueprintTexture, GameState.URBAN_DESIGN_BIBLE_UNLOCKS[17]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(18), blueprintTexture, GameState.URBAN_DESIGN_BIBLE_UNLOCKS[18]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(19), blueprintTexture, GameState.URBAN_DESIGN_BIBLE_UNLOCKS[19]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(20), blueprintTexture, GameState.URBAN_DESIGN_BIBLE_UNLOCKS[20]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(21), blueprintTexture, GameState.URBAN_DESIGN_BIBLE_UNLOCKS[21]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(22), blueprintTexture, GameState.URBAN_DESIGN_BIBLE_UNLOCKS[22]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(23), blueprintTexture, GameState.URBAN_DESIGN_BIBLE_UNLOCKS[23])));
            pages.Add(new ScrapbookPage("Blueprint Ice", new ScrapbookPage.ImageComponent(new Vector2(0, 0), content.Load<Texture2D>(Paths.INTERFACE_SCRAPBOOK_PAGE_BLUEPRINTS_ICE), Color.White),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(0), blueprintTexture, GameState.ICE_A_TREATISE_UNLOCKS[0]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(1), blueprintTexture, GameState.ICE_A_TREATISE_UNLOCKS[1]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(2), blueprintTexture, GameState.ICE_A_TREATISE_UNLOCKS[2]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(3), blueprintTexture, GameState.ICE_A_TREATISE_UNLOCKS[3]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(4), blueprintTexture, GameState.ICE_A_TREATISE_UNLOCKS[4]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(5), blueprintTexture, GameState.ICE_A_TREATISE_UNLOCKS[5]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(6), blueprintTexture, GameState.ICE_A_TREATISE_UNLOCKS[6]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(7), blueprintTexture, GameState.ICE_A_TREATISE_UNLOCKS[7]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(7), blueprintTexture, GameState.ICE_A_TREATISE_UNLOCKS[8])));
            pages.Add(new ScrapbookPage("Patterns S/S", new ScrapbookPage.ImageComponent(new Vector2(0, 0), content.Load<Texture2D>(Paths.INTERFACE_SCRAPBOOK_PAGE_PATTERNS_SS), Color.White),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(0), patternTexture, GameState.BASIC_PATTERNS_SS_UNLOCKS[0]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(1), patternTexture, GameState.BASIC_PATTERNS_SS_UNLOCKS[1]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(2), patternTexture, GameState.BASIC_PATTERNS_SS_UNLOCKS[2]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(3), patternTexture, GameState.BASIC_PATTERNS_SS_UNLOCKS[3]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(4), patternTexture, GameState.BASIC_PATTERNS_SS_UNLOCKS[4]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(5), patternTexture, GameState.BASIC_PATTERNS_SS_UNLOCKS[5]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(6), patternTexture, GameState.BASIC_PATTERNS_SS_UNLOCKS[6]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(7), patternTexture, GameState.BASIC_PATTERNS_SS_UNLOCKS[7]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(8), patternTexture, GameState.BASIC_PATTERNS_SS_UNLOCKS[8]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(9), patternTexture, GameState.BASIC_PATTERNS_SS_UNLOCKS[9]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(10), patternTexture, GameState.BASIC_PATTERNS_SS_UNLOCKS[10]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(11), patternTexture, GameState.BASIC_PATTERNS_SS_UNLOCKS[11]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(12), patternTexture, GameState.BASIC_PATTERNS_SS_UNLOCKS[12]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(13), patternTexture, GameState.BASIC_PATTERNS_SS_UNLOCKS[13]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(14), patternTexture, GameState.BASIC_PATTERNS_SS_UNLOCKS[14]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(15), patternTexture, GameState.BASIC_PATTERNS_SS_UNLOCKS[15]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(16), patternTexture, GameState.BASIC_PATTERNS_SS_UNLOCKS[16]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(17), patternTexture, GameState.BASIC_PATTERNS_SS_UNLOCKS[17])));
            pages.Add(new ScrapbookPage("Patterns F/W", new ScrapbookPage.ImageComponent(new Vector2(0, 0), content.Load<Texture2D>(Paths.INTERFACE_SCRAPBOOK_PAGE_PATTERNS_FW), Color.White),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(0), patternTexture, GameState.BASIC_PATTERNS_FW_UNLOCKS[0]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(1), patternTexture, GameState.BASIC_PATTERNS_FW_UNLOCKS[1]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(2), patternTexture, GameState.BASIC_PATTERNS_FW_UNLOCKS[2]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(3), patternTexture, GameState.BASIC_PATTERNS_FW_UNLOCKS[3]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(4), patternTexture, GameState.BASIC_PATTERNS_FW_UNLOCKS[4]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(5), patternTexture, GameState.BASIC_PATTERNS_FW_UNLOCKS[5]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(6), patternTexture, GameState.BASIC_PATTERNS_FW_UNLOCKS[6]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(7), patternTexture, GameState.BASIC_PATTERNS_FW_UNLOCKS[7]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(8), patternTexture, GameState.BASIC_PATTERNS_FW_UNLOCKS[8]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(9), patternTexture, GameState.BASIC_PATTERNS_FW_UNLOCKS[9]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(10), patternTexture, GameState.BASIC_PATTERNS_FW_UNLOCKS[10]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(11), patternTexture, GameState.BASIC_PATTERNS_FW_UNLOCKS[11]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(12), patternTexture, GameState.BASIC_PATTERNS_FW_UNLOCKS[12]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(13), patternTexture, GameState.BASIC_PATTERNS_FW_UNLOCKS[13]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(14), patternTexture, GameState.BASIC_PATTERNS_FW_UNLOCKS[14]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(15), patternTexture, GameState.BASIC_PATTERNS_FW_UNLOCKS[15]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(16), patternTexture, GameState.BASIC_PATTERNS_FW_UNLOCKS[16]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(17), patternTexture, GameState.BASIC_PATTERNS_FW_UNLOCKS[17]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(18), patternTexture, GameState.BASIC_PATTERNS_FW_UNLOCKS[18]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(19), patternTexture, GameState.BASIC_PATTERNS_FW_UNLOCKS[19])));
            pages.Add(new ScrapbookPage("Patterns Country", new ScrapbookPage.ImageComponent(new Vector2(0, 0), content.Load<Texture2D>(Paths.INTERFACE_SCRAPBOOK_PAGE_PATTERNS_COUNTRY), Color.White),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(0), patternTexture, GameState.COUNTRY_PATTERNS_UNLOCKS[0]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(1), patternTexture, GameState.COUNTRY_PATTERNS_UNLOCKS[1]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(2), patternTexture, GameState.COUNTRY_PATTERNS_UNLOCKS[2]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(3), patternTexture, GameState.COUNTRY_PATTERNS_UNLOCKS[3]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(4), patternTexture, GameState.COUNTRY_PATTERNS_UNLOCKS[4]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(5), patternTexture, GameState.COUNTRY_PATTERNS_UNLOCKS[5]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(6), patternTexture, GameState.COUNTRY_PATTERNS_UNLOCKS[6]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(7), patternTexture, GameState.COUNTRY_PATTERNS_UNLOCKS[7]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(8), patternTexture, GameState.COUNTRY_PATTERNS_UNLOCKS[8]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(9), patternTexture, GameState.COUNTRY_PATTERNS_UNLOCKS[9]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(10), patternTexture, GameState.COUNTRY_PATTERNS_UNLOCKS[10]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(11), patternTexture, GameState.COUNTRY_PATTERNS_UNLOCKS[11])));
            pages.Add(new ScrapbookPage("Patterns Tropical", new ScrapbookPage.ImageComponent(new Vector2(0, 0), content.Load<Texture2D>(Paths.INTERFACE_SCRAPBOOK_PAGE_PATTERNS_TROPICAL), Color.White),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(0), patternTexture, GameState.TROPICAL_PATTERNS_UNLOCKS[0]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(1), patternTexture, GameState.TROPICAL_PATTERNS_UNLOCKS[1]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(2), patternTexture, GameState.TROPICAL_PATTERNS_UNLOCKS[2]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(3), patternTexture, GameState.TROPICAL_PATTERNS_UNLOCKS[3]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(4), patternTexture, GameState.TROPICAL_PATTERNS_UNLOCKS[4]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(5), patternTexture, GameState.TROPICAL_PATTERNS_UNLOCKS[5]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(6), patternTexture, GameState.TROPICAL_PATTERNS_UNLOCKS[6]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(7), patternTexture, GameState.TROPICAL_PATTERNS_UNLOCKS[7]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(8), patternTexture, GameState.TROPICAL_PATTERNS_UNLOCKS[8]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(9), patternTexture, GameState.TROPICAL_PATTERNS_UNLOCKS[9]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(10), patternTexture, GameState.TROPICAL_PATTERNS_UNLOCKS[10]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(11), patternTexture, GameState.TROPICAL_PATTERNS_UNLOCKS[11]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(12), patternTexture, GameState.TROPICAL_PATTERNS_UNLOCKS[12]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(13), patternTexture, GameState.TROPICAL_PATTERNS_UNLOCKS[13])));
            pages.Add(new ScrapbookPage("Patterns Costume", new ScrapbookPage.ImageComponent(new Vector2(0, 0), content.Load<Texture2D>(Paths.INTERFACE_SCRAPBOOK_PAGE_PATTERNS_COSTUME), Color.White),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(0), patternTexture, GameState.COSTUME_PATTERNS_UNLOCKS[0]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(1), patternTexture, GameState.COSTUME_PATTERNS_UNLOCKS[1]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(2), patternTexture, GameState.COSTUME_PATTERNS_UNLOCKS[2]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(3), patternTexture, GameState.COSTUME_PATTERNS_UNLOCKS[3]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(4), patternTexture, GameState.COSTUME_PATTERNS_UNLOCKS[4]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(5), patternTexture, GameState.COSTUME_PATTERNS_UNLOCKS[5]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(6), patternTexture, GameState.COSTUME_PATTERNS_UNLOCKS[6]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(7), patternTexture, GameState.COSTUME_PATTERNS_UNLOCKS[7]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(8), patternTexture, GameState.COSTUME_PATTERNS_UNLOCKS[8]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(9), patternTexture, GameState.COSTUME_PATTERNS_UNLOCKS[9]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(10), patternTexture, GameState.COSTUME_PATTERNS_UNLOCKS[10]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(11), patternTexture, GameState.COSTUME_PATTERNS_UNLOCKS[11]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(12), patternTexture, GameState.COSTUME_PATTERNS_UNLOCKS[12]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(13), patternTexture, GameState.COSTUME_PATTERNS_UNLOCKS[13]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(14), patternTexture, GameState.COSTUME_PATTERNS_UNLOCKS[14]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(15), patternTexture, GameState.COSTUME_PATTERNS_UNLOCKS[15])));
            pages.Add(new ScrapbookPage("Patterns Urban", new ScrapbookPage.ImageComponent(new Vector2(0, 0), content.Load<Texture2D>(Paths.INTERFACE_SCRAPBOOK_PAGE_PATTERNS_URBAN), Color.White),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(0), patternTexture, GameState.URBAN_PATTERNS_UNLOCKS[0]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(1), patternTexture, GameState.URBAN_PATTERNS_UNLOCKS[1]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(2), patternTexture, GameState.URBAN_PATTERNS_UNLOCKS[2]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(3), patternTexture, GameState.URBAN_PATTERNS_UNLOCKS[3]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(4), patternTexture, GameState.URBAN_PATTERNS_UNLOCKS[4]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(5), patternTexture, GameState.URBAN_PATTERNS_UNLOCKS[5]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(6), patternTexture, GameState.URBAN_PATTERNS_UNLOCKS[6]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(7), patternTexture, GameState.URBAN_PATTERNS_UNLOCKS[7]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(8), patternTexture, GameState.URBAN_PATTERNS_UNLOCKS[8]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(9), patternTexture, GameState.URBAN_PATTERNS_UNLOCKS[9]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(10), patternTexture, GameState.URBAN_PATTERNS_UNLOCKS[10]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(11), patternTexture, GameState.URBAN_PATTERNS_UNLOCKS[11]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(12), patternTexture, GameState.URBAN_PATTERNS_UNLOCKS[12]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(13), patternTexture, GameState.URBAN_PATTERNS_UNLOCKS[13]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(14), patternTexture, GameState.URBAN_PATTERNS_UNLOCKS[14]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(15), patternTexture, GameState.URBAN_PATTERNS_UNLOCKS[15]),
                new ScrapbookPage.BlueprintComponent(GetBlueprintPosition(16), patternTexture, GameState.URBAN_PATTERNS_UNLOCKS[16])));
            
            scrapbookPages = BuildScrapbook(content, pages);
        }

        private void SetKeyActionTexts(EntityPlayer player)
        {
            if (currentDialogue != null && dialogueNodePage + 1 == currentDialogue.NumPages())
            {
                rightAction = currentDialogue.decisionRightText;
                leftAction = currentDialogue.decisionLeftText;
                upAction = currentDialogue.decisionUpText;
                downAction = currentDialogue.decisionDownText;
            }
            else
            {
                if (player.GetInterfaceState() != InterfaceState.NONE)
                {
                    rightAction = "";
                    leftAction = "";
                    upAction = "";
                    downAction = "";
                }
                else
                {
                    rightAction = "Right";
                    leftAction = "Left";
                    if (player.IsGroundPound() || player.IsGroundPoundLock())
                    {
                        rightAction = "";
                        leftAction = "";
                    }
                    else if (player.IsWallGrab() || player.IsWallCling())
                    {
                        rightAction = player.GetDirection() == DirectionEnum.LEFT ? "Release" : "Hold";
                        leftAction = player.GetDirection() == DirectionEnum.LEFT ? "Hold" : "Release";
                    }
                    if (player.IsRolling() && player.IsGrounded())
                    {
                        upAction = "Stand";
                    }
                    else if (player.IsGrounded() || (!player.IsGrounded() && player.IsRolling()) || player.IsWallGrab() || player.IsWallCling())
                    {
                        upAction = "Jump";
                    }
                    else if (player.IsGliding() || player.IsGroundPound() || player.IsGroundPoundLock())
                    {
                        upAction = "";
                    }
                    else
                    {
                        if (player.GetSailcloth().GetItem() != ItemDict.CLOTHING_NONE)
                        {
                            upAction = "Glide";
                        } else
                        {
                            upAction = "";
                        }
                    }
                    if (player.IsGroundPoundLock())
                    {
                        downAction = "Quickroll";
                    }
                    else if (player.IsRolling() || player.IsWallGrab() || player.IsWallCling())
                    {
                        downAction = "";
                    }
                    else if (player.IsGrounded())
                    {
                        downAction = "Roll";
                    }
                    else if (!player.IsGrounded() && !player.IsGliding() && !player.IsSwimming())
                    {
                        downAction = "Groundpound";
                    }
                    else if (player.IsGliding() && player.GetSailcloth().GetItem() != ItemDict.CLOTHING_NONE)
                    {
                        downAction = "Exit Glide";
                    }
                    else
                    {
                        downAction = "";
                    }
                }
            }
        }

        private void SetMouseActionTexts(EntityPlayer player, bool hoveringPlaceable)
        {
            if (currentDialogue != null)
            {
                if (!currentDialogue.Splits())
                {
                    mouseLeftAction = "Next";
                    mouseRightAction = "";
                    if(currentDialogueNumChars < currentDialogue.dialogueTexts[dialogueNodePage].Length)
                    {
                        mouseRightAction = "Speed-Up";
                    }
                    mouseRightShiftAction = "";
                    mouseLeftShiftAction = "";
                } else
                {
                    mouseLeftAction = "";
                    mouseLeftShiftAction = "";
                }
            }
            else
            {
                mouseLeftAction = player.GetLeftClickAction();
                mouseRightAction = player.GetRightClickAction();
                mouseLeftShiftAction = player.GetLeftShiftClickAction();
                mouseRightShiftAction = player.GetRightShiftClickAction();
                
                if (player.GetInterfaceState() == InterfaceState.INVENTORY || player.GetInterfaceState() == InterfaceState.CHEST)
                {
                    mouseLeftShiftAction = "Hot-swap";
                    mouseRightShiftAction = "";
                    for (int i = 0; i < itemRectangles.Length; i++)
                    {
                        if(itemRectangles[i].Contains(controller.GetMousePos()) && (player.GetInventoryItemStack(i).GetItem() is ClothingItem || player.GetInventoryItemStack(i).GetItem().HasTag(Item.Tag.ACCESSORY)))
                        {
                            mouseRightShiftAction = "Change";
                        }
                    }
                    if (inventoryHeldItem.GetItem() != ItemDict.NONE)
                    {
                        mouseLeftAction = "Place All";
                        mouseRightAction = "Place One";
                        if (inventoryHeldItem.GetItem() is DyeItem)
                        {
                            mouseRightAction = "Apply Dye";
                            mouseRightShiftAction = "10x Dye";
                        }
                    }
                    else
                    {
                        mouseLeftAction = "Grab All";
                        mouseRightAction = "Grab Half";
                    }
                }
                else if (player.IsEditMode())
                {
                    if (player.GetHeldItem().GetItem() is PlaceableItem)
                    {
                        mouseLeftAction = "Place";
                    }
                    mouseRightAction = "Remove";
                    mouseRightShiftAction = "";
                    mouseLeftShiftAction = "";
                }
                else if (player.GetInterfaceState() == InterfaceState.SCRAPBOOK || player.GetInterfaceState() == InterfaceState.SETTINGS)
                {
                    mouseLeftAction = "Select";
                    mouseRightAction = "Select";
                    mouseRightShiftAction = "";
                    mouseLeftShiftAction = "";
                } else if (player.GetInterfaceState() == InterfaceState.EXIT)
                {
                    mouseLeftAction = "Confirm";
                    mouseRightAction = "";
                    mouseRightShiftAction = "";
                    mouseLeftShiftAction = "";
                } else if (player.GetInterfaceState() == InterfaceState.CRAFTING)
                {
                    mouseLeftAction = "Select";
                    mouseRightAction = "";
                    mouseRightShiftAction = "";
                    mouseLeftShiftAction = "";
                }

                if(player.IsRolling() || !player.IsGrounded())
                {
                    mouseLeftAction = "";
                    mouseRightAction = "";
                    mouseRightShiftAction = "";
                    mouseLeftShiftAction = "";
                }
            }
        }

        public void Pause()
        {
            this.paused = true;
        }

        public void Unpause()
        {
            this.paused = false;
        }

        private bool CheckClothingTooltips(EntityPlayer player, Vector2 mouse)
        {
            if (HAT_INVENTORY_RECT.Contains(mouse))
            {
                inventorySelectedPosition = HAT_INVENTORY_RECT.TopLeft;
                if (hat == ItemDict.CLOTHING_NONE)
                {
                    tooltipName = "Hat";
                    tooltipDescription = "You can place a hat here to put it on.";
                }
                else
                {
                    tooltipName = player.GetHat().GetItem().GetName();
                    tooltipDescription = player.GetHat().GetItem().GetDescription();
                }
                return true;
            }
            else if (SHIRT_INVENTORY_RECT.Contains(mouse))
            {
                inventorySelectedPosition = SHIRT_INVENTORY_RECT.TopLeft;
                if (shirt == ItemDict.CLOTHING_NONE)
                {
                    tooltipName = "Shirt";
                    tooltipDescription = "You can place a shirt here to put it on.";
                }
                else
                {
                    tooltipName = player.GetShirt().GetItem().GetName();
                    tooltipDescription = player.GetShirt().GetItem().GetDescription();
                }
                return true;
            }
            else if (OUTERWEAR_INVENTORY_RECT.Contains(mouse))
            {
                inventorySelectedPosition = OUTERWEAR_INVENTORY_RECT.TopLeft;
                if (outerwear == ItemDict.CLOTHING_NONE)
                {
                    tooltipName = "Outerwear";
                    tooltipDescription = "You can place outerwear here to put it on.";
                }
                else
                {
                    tooltipName = player.GetOuterwear().GetItem().GetName();
                    tooltipDescription = player.GetOuterwear().GetItem().GetDescription();
                }
                return true;
            }
            else if (PANTS_INVENTORY_RECT.Contains(mouse))
            {
                inventorySelectedPosition = PANTS_INVENTORY_RECT.TopLeft;
                if (pants == ItemDict.CLOTHING_NONE)
                {
                    tooltipName = "Pants";
                    tooltipDescription = "You can place pants here to put them on.";
                }
                else
                {
                    tooltipName = player.GetPants().GetItem().GetName();
                    tooltipDescription = player.GetPants().GetItem().GetDescription();
                }
                return true;
            }
            else if (SOCKS_INVENTORY_RECT.Contains(mouse))
            {
                inventorySelectedPosition = SOCKS_INVENTORY_RECT.TopLeft;
                if (socks == ItemDict.CLOTHING_NONE)
                {
                    tooltipName = "Socks";
                    tooltipDescription = "You can place socks here to put them on.";
                }
                else
                {
                    tooltipName = player.GetSocks().GetItem().GetName();
                    tooltipDescription = player.GetSocks().GetItem().GetDescription();
                }
                return true;
            }
            else if (SHOES_INVENTORY_RECT.Contains(mouse))
            {
                inventorySelectedPosition = SHOES_INVENTORY_RECT.TopLeft;
                if (shoes == ItemDict.CLOTHING_NONE)
                {
                    tooltipName = "Shoes";
                    tooltipDescription = "You can place shoes here to put them on.";
                }
                else
                {
                    tooltipName = player.GetShoes().GetItem().GetName();
                    tooltipDescription = player.GetShoes().GetItem().GetDescription();
                }
                return true;
            }
            else if (GLOVES_INVENTORY_RECT.Contains(mouse))
            {
                inventorySelectedPosition = GLOVES_INVENTORY_RECT.TopLeft;
                if (gloves == ItemDict.CLOTHING_NONE)
                {
                    tooltipName = "Gloves";
                    tooltipDescription = "You can place gloves here to put them on.";
                }
                else
                {
                    tooltipName = player.GetGloves().GetItem().GetName();
                    tooltipDescription = player.GetGloves().GetItem().GetDescription();
                }
                return true;
            }
            else if (EARRINGS_INVENTORY_RECT.Contains(mouse))
            {
                inventorySelectedPosition = EARRINGS_INVENTORY_RECT.TopLeft;
                if (earrings == ItemDict.CLOTHING_NONE)
                {
                    tooltipName = "Earrings";
                    tooltipDescription = "You can place earrings here to put them on.";
                }
                else
                {
                    tooltipName = player.GetEarrings().GetItem().GetName();
                    tooltipDescription = player.GetEarrings().GetItem().GetDescription();
                }
                return true;
            }
            else if (SCARF_INVENTORY_RECT.Contains(mouse))
            {
                inventorySelectedPosition = SCARF_INVENTORY_RECT.TopLeft;
                if (scarf == ItemDict.CLOTHING_NONE)
                {
                    tooltipName = "Scarf";
                    tooltipDescription = "You can place a scarf here to put it on.";
                }
                else
                {
                    tooltipName = player.GetScarf().GetItem().GetName();
                    tooltipDescription = player.GetScarf().GetItem().GetDescription();
                }
                return true;
            }
            else if (GLASSES_INVENTORY_RECT.Contains(mouse))
            {
                inventorySelectedPosition = GLASSES_INVENTORY_RECT.TopLeft;
                if (glasses == ItemDict.CLOTHING_NONE)
                {
                    tooltipName = "Glasses";
                    tooltipDescription = "You can place glasses here to put them on.";
                }
                else
                {
                    tooltipName = player.GetGlasses().GetItem().GetName();
                    tooltipDescription = player.GetGlasses().GetItem().GetDescription();
                }
                return true;
            }
            else if (BACK_INVENTORY_RECT.Contains(mouse))
            {
                inventorySelectedPosition = BACK_INVENTORY_RECT.TopLeft;
                if (back == ItemDict.CLOTHING_NONE)
                {
                    tooltipName = "Back";
                    tooltipDescription = "You can place a back item here to put it on.";
                }
                else
                {
                    tooltipName = player.GetBack().GetItem().GetName();
                    tooltipDescription = player.GetBack().GetItem().GetDescription();
                }
                return true;
            }
            else if (SAILCLOTH_INVENTORY_RECT.Contains(mouse))
            {
                inventorySelectedPosition = SAILCLOTH_INVENTORY_RECT.TopLeft;
                if (sailcloth == ItemDict.CLOTHING_NONE)
                {
                    tooltipName = "Sailcloth";
                    tooltipDescription = "You can place a sailcloth here to put it on.";
                }
                else
                {
                    tooltipName = player.GetSailcloth().GetItem().GetName();
                    tooltipDescription = player.GetSailcloth().GetItem().GetDescription();
                }
                return true;
            }
            else if (ACCESSORY1_INVENTORY_RECT.Contains(mouse))
            {
                inventorySelectedPosition = ACCESSORY1_INVENTORY_RECT.TopLeft;
                if (accessory1 == ItemDict.CLOTHING_NONE)
                {
                    tooltipName = "Accessory 1";
                    tooltipDescription = "You can place an accessory here to equip it.";
                }
                else
                {
                    tooltipName = player.GetAccessory1().GetItem().GetName();
                    tooltipDescription = player.GetAccessory1().GetItem().GetDescription();
                }
                return true;
            }
            else if (ACCESSORY2_INVENTORY_RECT.Contains(mouse))
            {
                inventorySelectedPosition = ACCESSORY2_INVENTORY_RECT.TopLeft;
                if (accessory2 == ItemDict.CLOTHING_NONE)
                {
                    tooltipName = "Accessory 2";
                    tooltipDescription = "You can place an accessory here to equip it.";
                }
                else
                {
                    tooltipName = player.GetAccessory2().GetItem().GetName();
                    tooltipDescription = player.GetAccessory2().GetItem().GetDescription();
                }
                return true;
            }
            else if (ACCESSORY3_INVENTORY_RECT.Contains(mouse))
            {
                inventorySelectedPosition = ACCESSORY3_INVENTORY_RECT.TopLeft;
                if (accessory3 == ItemDict.CLOTHING_NONE)
                {
                    tooltipName = "Accessory 3";
                    tooltipDescription = "You can place an accessory here to equip it.";
                }
                else
                {
                    tooltipName = player.GetAccessory3().GetItem().GetName();
                    tooltipDescription = player.GetAccessory3().GetItem().GetDescription();
                }
                return true;
            }
            return false;
        }

        private void CheckClothingClick(EntityPlayer player, Vector2 mousePos, bool shift)
        {
            if (BACK_INVENTORY_RECT.Contains(mousePos))
            {
                if (inventoryHeldItem.GetItem() is DyeItem && player.GetBack().GetItem() != ItemDict.NONE)
                {
                    TryApplyDye(player.GetBack(), player);
                }
                else if (inventoryHeldItem.GetItem() == ItemDict.NONE)
                {
                    if (player.GetBack().GetItem() != ItemDict.CLOTHING_NONE)
                    {
                        if (shift)
                        {
                            if (player.AddItemToInventory(player.GetBack().GetItem(), false, false))
                            {
                                player.SetBack(new ItemStack(ItemDict.CLOTHING_NONE, 0));
                            }
                        }
                        else
                        {
                            inventoryHeldItem = player.GetBack();
                            player.SetBack(new ItemStack(ItemDict.CLOTHING_NONE, 0));
                        }
                    }
                }
                else if (inventoryHeldItem.GetItem().HasTag(Item.Tag.BACK))
                {
                    ItemStack oldItem = new ItemStack(ItemDict.NONE, 0);
                    if (player.GetBack().GetItem() != ItemDict.CLOTHING_NONE)
                    {
                        oldItem = player.GetBack();
                    }
                    player.SetBack(inventoryHeldItem);
                    inventoryHeldItem = oldItem;
                }
            }
            else if (GLASSES_INVENTORY_RECT.Contains(mousePos))
            {
                if (inventoryHeldItem.GetItem() is DyeItem && player.GetGlasses().GetItem() != ItemDict.NONE)
                {
                    TryApplyDye(player.GetGlasses(), player);
                }
                else if (inventoryHeldItem.GetItem() == ItemDict.NONE)
                {
                    if (player.GetGlasses().GetItem() != ItemDict.CLOTHING_NONE)
                    {
                        if (shift)
                        {
                            if (player.AddItemToInventory(player.GetGlasses().GetItem(), false, false))
                            {
                                player.SetGlasses(new ItemStack(ItemDict.CLOTHING_NONE, 0));
                            }
                        }
                        else
                        {
                            inventoryHeldItem = player.GetGlasses();
                            player.SetGlasses(new ItemStack(ItemDict.CLOTHING_NONE, 0));
                        }
                    }
                }
                else if (inventoryHeldItem.GetItem().HasTag(Item.Tag.GLASSES))
                {
                    ItemStack oldItem = new ItemStack(ItemDict.NONE, 0);
                    if (player.GetGlasses().GetItem() != ItemDict.CLOTHING_NONE)
                    {
                        oldItem = player.GetGlasses();
                    }
                    player.SetGlasses(inventoryHeldItem);
                    inventoryHeldItem = oldItem;
                }
            }
            else if (SAILCLOTH_INVENTORY_RECT.Contains(mousePos))
            {
                if (inventoryHeldItem.GetItem() is DyeItem && player.GetSailcloth().GetItem() != ItemDict.NONE)
                {
                    TryApplyDye(player.GetSailcloth(), player);
                }
                else if (inventoryHeldItem.GetItem() == ItemDict.NONE)
                {
                    if (player.GetSailcloth().GetItem() != ItemDict.CLOTHING_NONE)
                    {
                        if (shift)
                        {
                            if (player.AddItemToInventory(player.GetSailcloth().GetItem(), false, false))
                            {
                                player.SetSailcloth(new ItemStack(ItemDict.CLOTHING_NONE, 0));
                            }
                        }
                        else
                        {
                            inventoryHeldItem = player.GetSailcloth();
                            player.SetSailcloth(new ItemStack(ItemDict.CLOTHING_NONE, 0));
                        }
                    }
                }
                else if (inventoryHeldItem.GetItem().HasTag(Item.Tag.SAILCLOTH))
                {
                    ItemStack oldItem = new ItemStack(ItemDict.NONE, 0);
                    if (player.GetSailcloth().GetItem() != ItemDict.CLOTHING_NONE)
                    {
                        oldItem = player.GetSailcloth();
                    }
                    player.SetSailcloth(inventoryHeldItem);
                    inventoryHeldItem = oldItem;
                }
            }
            else if (SCARF_INVENTORY_RECT.Contains(mousePos))
            {
                if (inventoryHeldItem.GetItem() is DyeItem && player.GetScarf().GetItem() != ItemDict.NONE)
                {
                    TryApplyDye(player.GetScarf(), player);
                }
                else if (inventoryHeldItem.GetItem() == ItemDict.NONE)
                {
                    if (player.GetScarf().GetItem() != ItemDict.CLOTHING_NONE)
                    {
                        if (shift)
                        {
                            if (player.AddItemToInventory(player.GetScarf().GetItem(), false, false))
                            {
                                player.SetScarf(new ItemStack(ItemDict.CLOTHING_NONE, 0));
                            }
                        }
                        else
                        {
                            inventoryHeldItem = player.GetScarf();
                            player.SetScarf(new ItemStack(ItemDict.CLOTHING_NONE, 0));
                        }
                    }
                }
                else if (inventoryHeldItem.GetItem().HasTag(Item.Tag.SCARF))
                {
                    ItemStack oldItem = new ItemStack(ItemDict.NONE, 0);
                    if (player.GetScarf().GetItem() != ItemDict.CLOTHING_NONE)
                    {
                        oldItem = player.GetScarf();
                    }
                    player.SetScarf(inventoryHeldItem);
                    inventoryHeldItem = oldItem;
                }
            }
            else if (OUTERWEAR_INVENTORY_RECT.Contains(mousePos))
            {
                if (inventoryHeldItem.GetItem() is DyeItem && player.GetOuterwear().GetItem() != ItemDict.NONE)
                {
                    TryApplyDye(player.GetOuterwear(), player);
                }
                else if (inventoryHeldItem.GetItem() == ItemDict.NONE)
                {
                    if (player.GetOuterwear().GetItem() != ItemDict.CLOTHING_NONE)
                    {
                        if (shift)
                        {
                            if (player.AddItemToInventory(player.GetOuterwear().GetItem(), false, false))
                            {
                                player.SetOuterwear(new ItemStack(ItemDict.CLOTHING_NONE, 0));
                            }
                        }
                        else
                        {
                            inventoryHeldItem = player.GetOuterwear();
                            player.SetOuterwear(new ItemStack(ItemDict.CLOTHING_NONE, 0));
                        }
                    }
                }
                else if (inventoryHeldItem.GetItem().HasTag(Item.Tag.OUTERWEAR))
                {
                    ItemStack oldItem = new ItemStack(ItemDict.NONE, 0);
                    if (player.GetOuterwear().GetItem() != ItemDict.CLOTHING_NONE)
                    {
                        oldItem = player.GetOuterwear();
                    }
                    player.SetOuterwear(inventoryHeldItem);
                    inventoryHeldItem = oldItem;
                }
            }
            else if (SOCKS_INVENTORY_RECT.Contains(mousePos))
            {
                if (inventoryHeldItem.GetItem() is DyeItem && player.GetSocks().GetItem() != ItemDict.NONE)
                {
                    TryApplyDye(player.GetSocks(), player);
                }
                else if (inventoryHeldItem.GetItem() == ItemDict.NONE)
                {
                    if (player.GetSocks().GetItem() != ItemDict.CLOTHING_NONE)
                    {
                        if (shift)
                        {
                            if (player.AddItemToInventory(player.GetSocks().GetItem(), false, false))
                            {
                                player.SetSocks(new ItemStack(ItemDict.CLOTHING_NONE, 0));
                            }
                        }
                        else
                        {
                            inventoryHeldItem = player.GetSocks();
                            player.SetSocks(new ItemStack(ItemDict.CLOTHING_NONE, 0));
                        }
                    }
                }
                else if (inventoryHeldItem.GetItem().HasTag(Item.Tag.SOCKS))
                {
                    ItemStack oldItem = new ItemStack(ItemDict.NONE, 0);
                    if (player.GetSocks().GetItem() != ItemDict.CLOTHING_NONE)
                    {
                        oldItem = player.GetSocks();
                    }
                    player.SetSocks(inventoryHeldItem);
                    inventoryHeldItem = oldItem;
                }
            }
            else if (HAT_INVENTORY_RECT.Contains(mousePos))
            {
                if (inventoryHeldItem.GetItem() is DyeItem && player.GetHat().GetItem() != ItemDict.NONE)
                {
                    TryApplyDye(player.GetHat(), player);
                }
                else if (inventoryHeldItem.GetItem() == ItemDict.NONE)
                {
                    if (player.GetHat().GetItem() != ItemDict.CLOTHING_NONE)
                    {
                        if (shift)
                        {
                            if (player.AddItemToInventory(player.GetHat().GetItem(), false, false))
                            {
                                player.SetHat(new ItemStack(ItemDict.CLOTHING_NONE, 0));
                            }
                        }
                        else
                        {
                            inventoryHeldItem = player.GetHat();
                            player.SetHat(new ItemStack(ItemDict.CLOTHING_NONE, 0));
                        }
                    }
                }
                else if (inventoryHeldItem.GetItem().HasTag(Item.Tag.HAT))
                {
                    ItemStack oldItem = new ItemStack(ItemDict.NONE, 0);
                    if (player.GetHat().GetItem() != ItemDict.CLOTHING_NONE)
                    {
                        oldItem = player.GetHat();
                    }
                    player.SetHat(inventoryHeldItem);
                    inventoryHeldItem = oldItem;
                }
            }
            else if (SHIRT_INVENTORY_RECT.Contains(mousePos))
            {
                if (inventoryHeldItem.GetItem() is DyeItem && player.GetShirt().GetItem() != ItemDict.NONE)
                {
                    TryApplyDye(player.GetShirt(), player);
                }
                else if (inventoryHeldItem.GetItem() == ItemDict.NONE)
                {
                    if (player.GetShirt().GetItem() != ItemDict.CLOTHING_NONE)
                    {
                        if (shift)
                        {
                            if (player.AddItemToInventory(player.GetShirt().GetItem(), false, false))
                            {
                                player.SetShirt(new ItemStack(ItemDict.CLOTHING_NONE, 0));
                            }
                        }
                        else
                        {
                            inventoryHeldItem = player.GetShirt();
                            player.SetShirt(new ItemStack(ItemDict.CLOTHING_NONE, 0));
                        }
                    }
                }
                else if (inventoryHeldItem.GetItem().HasTag(Item.Tag.SHIRT))
                {
                    ItemStack oldItem = new ItemStack(ItemDict.NONE, 0);
                    if (player.GetShirt().GetItem() != ItemDict.CLOTHING_NONE)
                    {
                        oldItem = player.GetShirt();
                    }
                    player.SetShirt(inventoryHeldItem);
                    inventoryHeldItem = oldItem;
                }
            }
            else if (PANTS_INVENTORY_RECT.Contains(mousePos))
            {
                if (inventoryHeldItem.GetItem() is DyeItem && player.GetPants().GetItem() != ItemDict.NONE)
                {
                    TryApplyDye(player.GetPants(), player);
                }
                else if (inventoryHeldItem.GetItem() == ItemDict.NONE)
                {
                    if (player.GetPants().GetItem() != ItemDict.CLOTHING_NONE)
                    {
                        if (shift)
                        {
                            if (player.AddItemToInventory(player.GetPants().GetItem(), false, false))
                            {
                                player.SetPants(new ItemStack(ItemDict.CLOTHING_NONE, 0));
                            }
                        }
                        else
                        {
                            inventoryHeldItem = player.GetPants();
                            player.SetPants(new ItemStack(ItemDict.CLOTHING_NONE, 0));
                        }
                    }
                }
                else if (inventoryHeldItem.GetItem().HasTag(Item.Tag.PANTS))
                {
                    ItemStack oldItem = new ItemStack(ItemDict.NONE, 0);
                    if (player.GetPants().GetItem() != ItemDict.CLOTHING_NONE)
                    {
                        oldItem = player.GetPants();
                    }
                    player.SetPants(inventoryHeldItem);
                    inventoryHeldItem = oldItem;
                }
            }
            else if (EARRINGS_INVENTORY_RECT.Contains(mousePos))
            {
                if (inventoryHeldItem.GetItem() is DyeItem && player.GetEarrings().GetItem() != ItemDict.NONE)
                {
                    TryApplyDye(player.GetEarrings(), player);
                }
                else if (inventoryHeldItem.GetItem() == ItemDict.NONE)
                {
                    if (player.GetEarrings().GetItem() != ItemDict.CLOTHING_NONE)
                    {
                        if (shift)
                        {
                            if (player.AddItemToInventory(player.GetEarrings().GetItem(), false, false))
                            {
                                player.SetEarrings(new ItemStack(ItemDict.CLOTHING_NONE, 0));
                            }
                        }
                        else
                        {
                            inventoryHeldItem = player.GetEarrings();
                            player.SetEarrings(new ItemStack(ItemDict.CLOTHING_NONE, 0));
                        }
                    }
                }
                else if (inventoryHeldItem.GetItem().HasTag(Item.Tag.EARRINGS))
                {
                    ItemStack oldItem = new ItemStack(ItemDict.NONE, 0);
                    if (player.GetEarrings().GetItem() != ItemDict.CLOTHING_NONE)
                    {
                        oldItem = player.GetEarrings();
                    }
                    player.SetEarrings(inventoryHeldItem);
                    inventoryHeldItem = oldItem;
                }
            }
            else if (GLOVES_INVENTORY_RECT.Contains(mousePos))
            {
                if (inventoryHeldItem.GetItem() is DyeItem && player.GetGloves().GetItem() != ItemDict.NONE)
                {
                    TryApplyDye(player.GetGloves(), player);
                }
                else if (inventoryHeldItem.GetItem() == ItemDict.NONE)
                {
                    if (player.GetGloves().GetItem() != ItemDict.CLOTHING_NONE)
                    {
                        if (shift)
                        {
                            if (player.AddItemToInventory(player.GetGloves().GetItem(), false, false))
                            {
                                player.SetGloves(new ItemStack(ItemDict.CLOTHING_NONE, 0));
                            }
                        }
                        else
                        {
                            inventoryHeldItem = player.GetGloves();
                            player.SetGloves(new ItemStack(ItemDict.CLOTHING_NONE, 0));
                        }
                    }
                }
                else if (inventoryHeldItem.GetItem().HasTag(Item.Tag.GLOVES))
                {
                    ItemStack oldItem = new ItemStack(ItemDict.NONE, 0);
                    if (player.GetGloves().GetItem() != ItemDict.CLOTHING_NONE)
                    {
                        oldItem = player.GetGloves();
                    }
                    player.SetGloves(inventoryHeldItem);
                    inventoryHeldItem = oldItem;
                }
            }
            else if (SHOES_INVENTORY_RECT.Contains(mousePos))
            {
                if (inventoryHeldItem.GetItem() is DyeItem && player.GetShoes().GetItem() != ItemDict.NONE)
                {
                    TryApplyDye(player.GetShoes(), player);
                }
                else if (inventoryHeldItem.GetItem() == ItemDict.NONE)
                {
                    if (player.GetShoes().GetItem() != ItemDict.CLOTHING_NONE)
                    {
                        if (shift)
                        {
                            if (player.AddItemToInventory(player.GetShoes().GetItem(), false, false))
                            {
                                player.SetShoes(new ItemStack(ItemDict.CLOTHING_NONE, 0));
                            }
                        }
                        else
                        {
                            inventoryHeldItem = player.GetShoes();
                            player.SetShoes(new ItemStack(ItemDict.CLOTHING_NONE, 0));
                        }
                    }
                }
                else if (inventoryHeldItem.GetItem().HasTag(Item.Tag.SHOES))
                {
                    ItemStack oldItem = new ItemStack(ItemDict.NONE, 0);
                    if (player.GetShoes().GetItem() != ItemDict.CLOTHING_NONE)
                    {
                        oldItem = player.GetShoes();
                    }
                    player.SetShoes(inventoryHeldItem);
                    inventoryHeldItem = oldItem;
                }
            }
            else if (ACCESSORY1_INVENTORY_RECT.Contains(mousePos))
            {
                if (inventoryHeldItem.GetItem() == ItemDict.NONE)
                {
                    if (player.GetAccessory1().GetItem() != ItemDict.CLOTHING_NONE)
                    {
                        if (shift)
                        {
                            if (player.AddItemToInventory(player.GetAccessory1().GetItem(), false, false))
                            {
                                player.SetAccessory1(new ItemStack(ItemDict.CLOTHING_NONE, 0));
                            }
                        }
                        else
                        {
                            inventoryHeldItem = player.GetAccessory1();
                            player.SetAccessory1(new ItemStack(ItemDict.CLOTHING_NONE, 0));
                        }
                    }
                }
                else if (inventoryHeldItem.GetItem().HasTag(Item.Tag.ACCESSORY))
                {
                    ItemStack oldItem = new ItemStack(ItemDict.NONE, 0);
                    if (player.GetAccessory1().GetItem() != ItemDict.CLOTHING_NONE)
                    {
                        oldItem = player.GetAccessory1();
                    }
                    player.SetAccessory1(inventoryHeldItem);
                    inventoryHeldItem = oldItem;
                }
            }
            else if (ACCESSORY2_INVENTORY_RECT.Contains(mousePos))
            {
                if (inventoryHeldItem.GetItem() == ItemDict.NONE)
                {
                    if (player.GetAccessory2().GetItem() != ItemDict.CLOTHING_NONE)
                    {
                        if (shift)
                        {
                            if (player.AddItemToInventory(player.GetAccessory2().GetItem(), false, false))
                            {
                                player.SetAccessory2(new ItemStack(ItemDict.CLOTHING_NONE, 0));
                            }
                        }
                        else
                        {
                            inventoryHeldItem = player.GetAccessory2();
                            player.SetAccessory2(new ItemStack(ItemDict.CLOTHING_NONE, 0));
                        }
                    }
                }
                else if (inventoryHeldItem.GetItem().HasTag(Item.Tag.ACCESSORY))
                {
                    ItemStack oldItem = new ItemStack(ItemDict.NONE, 0);
                    if (player.GetAccessory2().GetItem() != ItemDict.CLOTHING_NONE)
                    {
                        oldItem = player.GetAccessory2();
                    }
                    player.SetAccessory2(inventoryHeldItem);
                    inventoryHeldItem = oldItem;
                }
            }
            else if (ACCESSORY3_INVENTORY_RECT.Contains(mousePos))
            {
                if (inventoryHeldItem.GetItem() == ItemDict.NONE)
                {
                    if (player.GetAccessory3().GetItem() != ItemDict.CLOTHING_NONE)
                    {
                        if (shift)
                        {
                            if (player.AddItemToInventory(player.GetAccessory3().GetItem(), false, false))
                            {
                                player.SetAccessory3(new ItemStack(ItemDict.CLOTHING_NONE, 0));
                            }
                        }
                        else
                        {
                            inventoryHeldItem = player.GetAccessory3();
                            player.SetAccessory3(new ItemStack(ItemDict.CLOTHING_NONE, 0));
                        }
                    }
                }
                else if (inventoryHeldItem.GetItem().HasTag(Item.Tag.ACCESSORY))
                {
                    ItemStack oldItem = new ItemStack(ItemDict.NONE, 0);
                    if (player.GetAccessory3().GetItem() != ItemDict.CLOTHING_NONE)
                    {
                        oldItem = player.GetAccessory3();
                    }
                    player.SetAccessory3(inventoryHeldItem);
                    inventoryHeldItem = oldItem;
                }
            }
        }

        private void DrawClothingHeldIndicator(SpriteBatch sb, RectangleF cameraBoundingBox)
        {
            if (inventoryHeldItem.GetItem().HasTag(Item.Tag.ACCESSORY))
            {
                sb.DrawRectangle(Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, ACCESSORY1_INVENTORY_RECT), CLOTHING_INDICATOR_COLOR);
                sb.DrawRectangle(Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, ACCESSORY2_INVENTORY_RECT), CLOTHING_INDICATOR_COLOR);
                sb.DrawRectangle(Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, ACCESSORY3_INVENTORY_RECT), CLOTHING_INDICATOR_COLOR);
            }
            if(inventoryHeldItem.GetItem().HasTag(Item.Tag.BACK))
            {
                sb.DrawRectangle(Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, BACK_INVENTORY_RECT), CLOTHING_INDICATOR_COLOR);
            }
            if(inventoryHeldItem.GetItem().HasTag(Item.Tag.EARRINGS))
            {
                sb.DrawRectangle(Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, EARRINGS_INVENTORY_RECT), CLOTHING_INDICATOR_COLOR);
            }
            if (inventoryHeldItem.GetItem().HasTag(Item.Tag.GLASSES))
            {
                sb.DrawRectangle(Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, GLASSES_INVENTORY_RECT), CLOTHING_INDICATOR_COLOR);
            }
            if (inventoryHeldItem.GetItem().HasTag(Item.Tag.GLOVES))
            {
                sb.DrawRectangle(Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, GLOVES_INVENTORY_RECT), CLOTHING_INDICATOR_COLOR);
            }
            if (inventoryHeldItem.GetItem().HasTag(Item.Tag.HAT))
            {
                sb.DrawRectangle(Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, HAT_INVENTORY_RECT), CLOTHING_INDICATOR_COLOR);
            }
            if (inventoryHeldItem.GetItem().HasTag(Item.Tag.OUTERWEAR))
            {
                sb.DrawRectangle(Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, OUTERWEAR_INVENTORY_RECT), CLOTHING_INDICATOR_COLOR);
            }
            if (inventoryHeldItem.GetItem().HasTag(Item.Tag.PANTS))
            {
                sb.DrawRectangle(Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, PANTS_INVENTORY_RECT), CLOTHING_INDICATOR_COLOR);
            }
            if (inventoryHeldItem.GetItem().HasTag(Item.Tag.SAILCLOTH))
            {
                sb.DrawRectangle(Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, SAILCLOTH_INVENTORY_RECT), CLOTHING_INDICATOR_COLOR);
            }
            if (inventoryHeldItem.GetItem().HasTag(Item.Tag.SCARF))
            {
                sb.DrawRectangle(Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, SCARF_INVENTORY_RECT), CLOTHING_INDICATOR_COLOR);
            }
            if (inventoryHeldItem.GetItem().HasTag(Item.Tag.SHIRT))
            {
                sb.DrawRectangle(Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, SHIRT_INVENTORY_RECT), CLOTHING_INDICATOR_COLOR);
            }
            if (inventoryHeldItem.GetItem().HasTag(Item.Tag.SHOES))
            {
                sb.DrawRectangle(Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, SHOES_INVENTORY_RECT), CLOTHING_INDICATOR_COLOR);
            }
            if (inventoryHeldItem.GetItem().HasTag(Item.Tag.SOCKS))
            {
                sb.DrawRectangle(Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, SOCKS_INVENTORY_RECT), CLOTHING_INDICATOR_COLOR);
            }
        }

        private void ShiftSwapClothingItem(EntityPlayer player, int i)
        {
            ItemStack item = player.GetInventoryItemStack(i);
            if(item.GetItem().HasTag(Item.Tag.ACCESSORY))
            {
                if(player.GetAccessory1().GetItem() == ItemDict.CLOTHING_NONE)
                {
                    player.SetAccessory1(item);
                    player.RemoveItemStackAt(i);
                } else if (player.GetAccessory2().GetItem() == ItemDict.CLOTHING_NONE)
                {
                    player.SetAccessory2(item);
                    player.RemoveItemStackAt(i);
                } else if (player.GetAccessory3().GetItem() == ItemDict.CLOTHING_NONE)
                {
                    player.SetAccessory3(item);
                    player.RemoveItemStackAt(i);
                } else
                {
                    ItemStack oldAcc = player.GetAccessory1();
                    player.SetAccessory1(item);
                    player.AddItemStackAt(oldAcc, i);
                }
            } else if (item.GetItem().HasTag(Item.Tag.BACK))
            {
                if(player.GetBack().GetItem() == ItemDict.CLOTHING_NONE)
                {
                    player.SetBack(item);
                    player.RemoveItemStackAt(i);
                } else
                {
                    ItemStack old = player.GetBack();
                    player.SetBack(item);
                    player.AddItemStackAt(old, i);
                }
            }
            else if (item.GetItem().HasTag(Item.Tag.EARRINGS))
            {
                if (player.GetEarrings().GetItem() == ItemDict.CLOTHING_NONE)
                {
                    player.SetEarrings(item);
                    player.RemoveItemStackAt(i);
                }
                else
                {
                    ItemStack old = player.GetEarrings();
                    player.SetEarrings(item);
                    player.AddItemStackAt(old, i);
                }
            }
            else if (item.GetItem().HasTag(Item.Tag.GLASSES))
            {
                if (player.GetGlasses().GetItem() == ItemDict.CLOTHING_NONE)
                {
                    player.SetGlasses(item);
                    player.RemoveItemStackAt(i);
                }
                else
                {
                    ItemStack old = player.GetGlasses();
                    player.SetGlasses(item);
                    player.AddItemStackAt(old, i);
                }
            }
            else if (item.GetItem().HasTag(Item.Tag.GLOVES))
            {
                if (player.GetGloves().GetItem() == ItemDict.CLOTHING_NONE)
                {
                    player.SetGloves(item);
                    player.RemoveItemStackAt(i);
                }
                else
                {
                    ItemStack old = player.GetGloves();
                    player.SetGloves(item);
                    player.AddItemStackAt(old, i);
                }
            }
            else if (item.GetItem().HasTag(Item.Tag.HAT))
            {
                if (player.GetHat().GetItem() == ItemDict.CLOTHING_NONE)
                {
                    player.SetHat(item);
                    player.RemoveItemStackAt(i);
                }
                else
                {
                    ItemStack old = player.GetHat();
                    player.SetHat(item);
                    player.AddItemStackAt(old, i);
                }
            }
            else if (item.GetItem().HasTag(Item.Tag.OUTERWEAR))
            {
                if (player.GetOuterwear().GetItem() == ItemDict.CLOTHING_NONE)
                {
                    player.SetOuterwear(item);
                    player.RemoveItemStackAt(i);
                }
                else
                {
                    ItemStack old = player.GetOuterwear();
                    player.SetOuterwear(item);
                    player.AddItemStackAt(old, i);
                }
            }
            else if (item.GetItem().HasTag(Item.Tag.PANTS))
            {
                if (player.GetPants().GetItem() == ItemDict.CLOTHING_NONE)
                {
                    player.SetPants(item);
                    player.RemoveItemStackAt(i);
                }
                else
                {
                    ItemStack old = player.GetPants();
                    player.SetPants(item);
                    player.AddItemStackAt(old, i);
                }
            }
            else if (item.GetItem().HasTag(Item.Tag.SAILCLOTH))
            {
                if (player.GetSailcloth().GetItem() == ItemDict.CLOTHING_NONE)
                {
                    player.SetSailcloth(item);
                    player.RemoveItemStackAt(i);
                }
                else
                {
                    ItemStack old = player.GetSailcloth();
                    player.SetSailcloth(item);
                    player.AddItemStackAt(old, i);
                }
            }
            else if (item.GetItem().HasTag(Item.Tag.SCARF))
            {
                if (player.GetScarf().GetItem() == ItemDict.CLOTHING_NONE)
                {
                    player.SetScarf(item);
                    player.RemoveItemStackAt(i);
                }
                else
                {
                    ItemStack old = player.GetScarf();
                    player.SetScarf(item);
                    player.AddItemStackAt(old, i);
                }
            }
            else if (item.GetItem().HasTag(Item.Tag.SHIRT))
            {
                if (player.GetShirt().GetItem() == ItemDict.CLOTHING_NONE)
                {
                    player.SetShirt(item);
                    player.RemoveItemStackAt(i);
                }
                else
                {
                    ItemStack old = player.GetShirt();
                    player.SetShirt(item);
                    player.AddItemStackAt(old, i);
                }
            }
            else if (item.GetItem().HasTag(Item.Tag.SHOES))
            {
                if (player.GetShoes().GetItem() == ItemDict.CLOTHING_NONE)
                {
                    player.SetShoes(item);
                    player.RemoveItemStackAt(i);
                }
                else
                {
                    ItemStack old = player.GetShoes();
                    player.SetShoes(item);
                    player.AddItemStackAt(old, i);
                }
            }
            else if (item.GetItem().HasTag(Item.Tag.SOCKS))
            {
                if (player.GetSocks().GetItem() == ItemDict.CLOTHING_NONE)
                {
                    player.SetSocks(item);
                    player.RemoveItemStackAt(i);
                }
                else
                {
                    ItemStack old = player.GetSocks();
                    player.SetSocks(item);
                    player.AddItemStackAt(old, i);
                }
            }
        }

        private void OpenScrapbook(EntityPlayer player, World.TimeData timeData, World world)
        {
            int y = 0;
            int x = 0;
            switch(timeData.season)
            {
                case World.Season.SPRING:
                    y = 45;
                    break;
                case World.Season.SUMMER:
                    y = 45 + 18;
                    break;
                case World.Season.AUTUMN:
                    y = 45 + 18 + 18;
                    break;
                case World.Season.WINTER:
                    y = 45 + 18 + 18 + 18;
                    break;
            }
            switch(timeData.day)
            {
                case 0:
                    x = 33;
                    break;
                case 1:
                    x = 48;
                    break;
                case 2:
                    x = 62;
                    break;
                case 3:
                    x = 77;
                    break;
                case 4:
                    x = 89;
                    break;
                case 5:
                    x = 105;
                    break;
                case 6:
                    x = 123;
                    break;
            }
            scrapbookDynamicComponents[SCRAPBOOK_CALENDAR_CURRENT_DAY].SetPosition(new Vector2(x, y));

            for(int i = 0; i < scrapbookPages.Length; i++)
            {
                scrapbookPages[i].IsUnlocked(true);
            }
            scrapbookPages[0].IsUnlocked(true); //calendar
            scrapbookPages[1].IsUnlocked(true); //maps
            scrapbookPages[2].IsUnlocked(true); //relationships
            scrapbookPages[3].IsUnlocked(true); //relationships ii
            scrapbookPages[4].IsUnlocked(true); //farming intro
            /*scrapbookPages[5].IsUnlocked(GameState.CheckFlag(GameState.FLAG_BOOK_THE_FARMERS_HANDBOOK_ANIMALS));
            scrapbookPages[6].IsUnlocked(GameState.CheckFlag(GameState.FLAG_BOOK_THE_FARMERS_HANDBOOK_COMPOST));
            scrapbookPages[7].IsUnlocked(GameState.CheckFlag(GameState.FLAG_BOOK_THE_FARMERS_HANDBOOK_SEED_MAKERS));
            scrapbookPages[8].IsUnlocked(GameState.CheckFlag(GameState.FLAG_BOOK_THE_FARMERS_HANDBOOK_ADVANCED));
            scrapbookPages[9].IsUnlocked(GameState.CheckFlag(GameState.FLAG_BOOK_THE_FARMERS_HANDBOOK_MASTERY));
            scrapbookPages[10].IsUnlocked(GameState.CheckFlag(GameState.FLAG_BOOK_THE_FARMERS_HANDBOOK_MYTHS_AND_LEGENDS));
            scrapbookPages[11].IsUnlocked(GameState.CheckFlag(GameState.FLAG_BOOK_WORKING_WITH_YOUR_WORKBENCH));
            scrapbookPages[12].IsUnlocked(GameState.CheckFlag(GameState.FLAG_BOOK_THE_TRADERS_ATLAS));
            scrapbookPages[13].IsUnlocked(GameState.CheckFlag(GameState.FLAG_BOOK_DATING_FOR_DUMMIES));
            scrapbookPages[14].IsUnlocked(GameState.CheckFlag(GameState.FLAG_BOOK_STRAIGHTFORWARD_SMELTING));
            scrapbookPages[15].IsUnlocked(GameState.CheckFlag(GameState.FLAG_BOOK_MASTERFUL_METALWORKING));
            scrapbookPages[16].IsUnlocked(GameState.CheckFlag(GameState.FLAG_BOOK_COMPRESSOR_USER_MANUAL));
            scrapbookPages[17].IsUnlocked(GameState.CheckFlag(GameState.FLAG_BOOK_A_STUDY_OF_COLOR_IN_NATURE));
            scrapbookPages[18].IsUnlocked(GameState.CheckFlag(GameState.FLAG_BOOK_GREAT_GLASSBLOWING));
            scrapbookPages[19].IsUnlocked(GameState.CheckFlag(GameState.FLAG_BOOK_POTTERY_FOR_FUN_AND_PROFIT));
            scrapbookPages[20].IsUnlocked(GameState.CheckFlag(GameState.FLAG_BOOK_ARTISTIC_SCENT));
            scrapbookPages[21].IsUnlocked(GameState.CheckFlag(GameState.FLAG_BOOK_JUICY_JAMS_PRECIOUS_PRESERVES_AND_WORTHWHILE_WINES));
            scrapbookPages[22].IsUnlocked(GameState.CheckFlag(GameState.FLAG_BOOK_BEEKEEPERS_MANUAL));
            scrapbookPages[23].IsUnlocked(GameState.CheckFlag(GameState.FLAG_BOOK_BEEMASTERS_MANUAL));
            scrapbookPages[24].IsUnlocked(GameState.CheckFlag(GameState.FLAG_BOOK_BIRDWATCHERS_ISSUE_I));
            scrapbookPages[25].IsUnlocked(GameState.CheckFlag(GameState.FLAG_BOOK_BIRDWATCHERS_ISSUE_II));
            scrapbookPages[26].IsUnlocked(GameState.CheckFlag(GameState.FLAG_BOOK_FISHING_THROUGH_THE_SEASONS));
            scrapbookPages[27].IsUnlocked(GameState.CheckFlag(GameState.FLAG_BOOK_ANCIENT_MARINERS_SCROLL));
            scrapbookPages[28].IsUnlocked(GameState.CheckFlag(GameState.FLAG_BOOK_FISH_BEYOND));
            scrapbookPages[29].IsUnlocked(GameState.CheckFlag(GameState.FLAG_BOOK_FASHION_PRIMER));
            scrapbookPages[30].IsUnlocked(GameState.CheckFlag(GameState.FLAG_BOOK_DECOR_PRIMER));
            scrapbookPages[31].IsUnlocked(GameState.CheckFlag(GameState.FLAG_BOOK_THE_FORAGERS_COOKBOOK_VOL_1));
            scrapbookPages[32].IsUnlocked(GameState.CheckFlag(GameState.FLAG_BOOK_THE_FORAGERS_COOKBOOK_VOL_2));
            scrapbookPages[33].IsUnlocked(GameState.CheckFlag(GameState.FLAG_BOOK_THE_FORAGERS_COOKBOOK_VOL_3));
            scrapbookPages[34].IsUnlocked(GameState.CheckFlag(GameState.FLAG_BOOK_COOKBOOK_SPRINGS_GIFT));
            scrapbookPages[35].IsUnlocked(GameState.CheckFlag(GameState.FLAG_BOOK_COOKBOOK_SUMMERS_BOUNTY));
            scrapbookPages[36].IsUnlocked(GameState.CheckFlag(GameState.FLAG_BOOK_COOKBOOK_AUTUMNS_HARVEST));
            scrapbookPages[37].IsUnlocked(GameState.CheckFlag(GameState.FLAG_BOOK_COOKBOOK_FOUR_SEASONS));
            scrapbookPages[38].IsUnlocked(GameState.CheckFlag(GameState.FLAG_BOOK_CHILLING_CONFECTIONS));
            scrapbookPages[39].IsUnlocked(GameState.CheckFlag(GameState.FLAG_BOOK_BREAKFAST_WITH_GRANDMA_NINE));
            scrapbookPages[40].IsUnlocked(GameState.CheckFlag(GameState.FLAG_BOOK_SUPPER_WITH_GRANDMA_NINE));
            scrapbookPages[41].IsUnlocked(GameState.CheckFlag(GameState.FLAG_BOOK_SUPPER_WITH_GRANDMA_NINE));
            scrapbookPages[42].IsUnlocked(GameState.CheckFlag(GameState.FLAG_BOOK_SOUPER_SOUPS));
            scrapbookPages[43].IsUnlocked(GameState.CheckFlag(GameState.FLAG_BOOK_EASTERN_CUISINE));
            scrapbookPages[44].IsUnlocked(GameState.CheckFlag(GameState.FLAG_BOOK_UNIQUE_FLAVORS)); //GOOD
            scrapbookPages[45].IsUnlocked(GameState.CheckFlag(GameState.FLAG_BOOK_SIMPLY_WOODWORKING));
            scrapbookPages[46].IsUnlocked(GameState.CheckFlag(GameState.FLAG_BOOK_SIMPLY_WOODWORKING));
            scrapbookPages[47].IsUnlocked(GameState.CheckFlag(GameState.FLAG_BOOK_SIMPLY_WOODWORKING));
            scrapbookPages[48].IsUnlocked(GameState.CheckFlag(GameState.FLAG_BOOK_FABULOUS_FARMSTEADS));
            scrapbookPages[49].IsUnlocked(GameState.CheckFlag(GameState.FLAG_BOOK_FABULOUS_FARMSTEADS));
            scrapbookPages[50].IsUnlocked(GameState.CheckFlag(GameState.FLAG_BOOK_FABULOUS_FARMSTEADS));
            scrapbookPages[51].IsUnlocked(GameState.CheckFlag(GameState.FLAG_BOOK_FABULOUS_FARMSTEADS));
            scrapbookPages[52].IsUnlocked(GameState.CheckFlag(GameState.FLAG_BOOK_CRAVING_STONECARVING));
            scrapbookPages[53].IsUnlocked(GameState.CheckFlag(GameState.FLAG_BOOK_CRAVING_STONECARVING));
            scrapbookPages[54].IsUnlocked(GameState.CheckFlag(GameState.FLAG_BOOK_A_TOUCH_OF_NATURE));
            scrapbookPages[55].IsUnlocked(GameState.CheckFlag(GameState.FLAG_BOOK_A_TOUCH_OF_NATURE));
            scrapbookPages[56].IsUnlocked(GameState.CheckFlag(GameState.FLAG_BOOK_A_TOUCH_OF_NATURE));
            scrapbookPages[57].IsUnlocked(GameState.CheckFlag(GameState.FLAG_BOOK_AN_ARTISTS_REFLECTION));
            scrapbookPages[58].IsUnlocked(GameState.CheckFlag(GameState.FLAG_BOOK_AN_ARTISTS_REFLECTION));
            scrapbookPages[59].IsUnlocked(GameState.CheckFlag(GameState.FLAG_BOOK_PLAYGROUND_PREP));
            scrapbookPages[60].IsUnlocked(GameState.CheckFlag(GameState.FLAG_BOOK_PLAYGROUND_PREP));
            scrapbookPages[61].IsUnlocked(GameState.CheckFlag(GameState.FLAG_BOOK_MUSIC_AT_HOME));
            scrapbookPages[62].IsUnlocked(GameState.CheckFlag(GameState.FLAG_BOOK_ESSENTIAL_ENGINEERING));
            scrapbookPages[63].IsUnlocked(GameState.CheckFlag(GameState.FLAG_BOOK_ESSENTIAL_ENGINEERING));
            scrapbookPages[64].IsUnlocked(GameState.CheckFlag(GameState.FLAG_BOOK_URBAN_DESIGN_BIBLE));
            scrapbookPages[65].IsUnlocked(GameState.CheckFlag(GameState.FLAG_BOOK_URBAN_DESIGN_BIBLE));
            scrapbookPages[66].IsUnlocked(GameState.CheckFlag(GameState.FLAG_BOOK_URBAN_DESIGN_BIBLE));
            scrapbookPages[67].IsUnlocked(GameState.CheckFlag(GameState.FLAG_BOOK_URBAN_DESIGN_BIBLE));
            scrapbookPages[68].IsUnlocked(GameState.CheckFlag(GameState.FLAG_BOOK_URBAN_DESIGN_BIBLE));
            scrapbookPages[69].IsUnlocked(GameState.CheckFlag(GameState.FLAG_BOOK_ICE_A_TREATISE));
            scrapbookPages[70].IsUnlocked(GameState.CheckFlag(GameState.FLAG_BOOK_BASIC_PATTERNS_SS));
            scrapbookPages[71].IsUnlocked(GameState.CheckFlag(GameState.FLAG_BOOK_BASIC_PATTERNS_SS));
            scrapbookPages[72].IsUnlocked(GameState.CheckFlag(GameState.FLAG_BOOK_BASIC_PATTERNS_SS));
            scrapbookPages[73].IsUnlocked(GameState.CheckFlag(GameState.FLAG_BOOK_BASIC_PATTERNS_FW));
            scrapbookPages[74].IsUnlocked(GameState.CheckFlag(GameState.FLAG_BOOK_BASIC_PATTERNS_FW));
            scrapbookPages[75].IsUnlocked(GameState.CheckFlag(GameState.FLAG_BOOK_BASIC_PATTERNS_FW));
            scrapbookPages[76].IsUnlocked(GameState.CheckFlag(GameState.FLAG_BOOK_BASIC_PATTERNS_FW));
            scrapbookPages[77].IsUnlocked(GameState.CheckFlag(GameState.FLAG_BOOK_COUNTRY_PATTERNS));
            scrapbookPages[78].IsUnlocked(GameState.CheckFlag(GameState.FLAG_BOOK_COUNTRY_PATTERNS));
            scrapbookPages[79].IsUnlocked(GameState.CheckFlag(GameState.FLAG_BOOK_TROPICAL_PATTERNS));
            scrapbookPages[80].IsUnlocked(GameState.CheckFlag(GameState.FLAG_BOOK_TROPICAL_PATTERNS));
            scrapbookPages[81].IsUnlocked(GameState.CheckFlag(GameState.FLAG_BOOK_TROPICAL_PATTERNS));
            scrapbookPages[82].IsUnlocked(GameState.CheckFlag(GameState.FLAG_BOOK_COSTUME_PATTERNS));
            scrapbookPages[83].IsUnlocked(GameState.CheckFlag(GameState.FLAG_BOOK_COSTUME_PATTERNS));
            scrapbookPages[84].IsUnlocked(GameState.CheckFlag(GameState.FLAG_BOOK_COSTUME_PATTERNS));
            scrapbookPages[85].IsUnlocked(GameState.CheckFlag(GameState.FLAG_BOOK_URBAN_PATTERNS));
            scrapbookPages[86].IsUnlocked(GameState.CheckFlag(GameState.FLAG_BOOK_URBAN_PATTERNS));
            scrapbookPages[87].IsUnlocked(GameState.CheckFlag(GameState.FLAG_BOOK_URBAN_PATTERNS));
            scrapbookPages[88].IsUnlocked(GameState.CheckFlag(GameState.FLAG_BOOK_HOMEMADE_ACCESSORIES));
            scrapbookPages[89].IsUnlocked(GameState.CheckFlag(GameState.FLAG_BOOK_HOMEMADE_ACCESSORIES));
            scrapbookPages[90].IsUnlocked(GameState.CheckFlag(GameState.FLAG_BOOK_HOMEMADE_ACCESSORIES));
            scrapbookPages[91].IsUnlocked(GameState.CheckFlag(GameState.FLAG_BOOK_HOMEMADE_ACCESSORIES));
            scrapbookPages[92].IsUnlocked(GameState.CheckFlag(GameState.FLAG_BOOK_NATURAL_CRAFTS));
            scrapbookPages[93].IsUnlocked(GameState.CheckFlag(GameState.FLAG_BOOK_NATURAL_CRAFTS));
            scrapbookPages[94].IsUnlocked(GameState.CheckFlag(GameState.FLAG_BOOK_NATURAL_CRAFTS));
            scrapbookPages[95].IsUnlocked(GameState.CheckFlag(GameState.FLAG_BOOK_JEWELERS_HANDBOOK));
            scrapbookPages[96].IsUnlocked(GameState.CheckFlag(GameState.FLAG_BOOK_JEWELERS_HANDBOOK));
            scrapbookPages[97].IsUnlocked(GameState.CheckFlag(GameState.FLAG_BOOK_JEWELERS_HANDBOOK));
            scrapbookPages[98].IsUnlocked(GameState.CheckFlag(GameState.FLAG_BOOK_FOCI_OF_THE_SHAMAN));
            scrapbookPages[99].IsUnlocked(GameState.CheckFlag(GameState.FLAG_BOOK_FOCI_OF_THE_SHAMAN));
            scrapbookPages[100].IsUnlocked(GameState.CheckFlag(GameState.FLAG_BOOK_FOCI_OF_THE_SHAMAN));
            scrapbookPages[101].IsUnlocked(GameState.CheckFlag(GameState.FLAG_BOOK_DUSTY_TOME));
            scrapbookPages[102].IsUnlocked(GameState.CheckFlag(GameState.FLAG_BOOK_MUSTY_TOME));
            scrapbookPages[103].IsUnlocked(GameState.CheckFlag(GameState.FLAG_BOOK_ELEMENTAL_MYSTICA));
            scrapbookPages[104].IsUnlocked(GameState.CheckFlag(GameState.FLAG_BOOK_ELEMENTAL_MYSTICA));
            scrapbookPages[105].IsUnlocked(GameState.CheckFlag(GameState.FLAG_BOOK_INTENSE_INCENSE));
            scrapbookPages[106].IsUnlocked(GameState.CheckFlag(GameState.FLAG_BOOK_CHANNELING_THE_ELEMENTS));*/
            scrapbookPages[107].IsUnlocked(true);
            scrapbookPages[108].IsUnlocked(true);
            scrapbookPages[109].IsUnlocked(true);

            player.SetInterfaceState(InterfaceState.SCRAPBOOK);
            player.Pause();
            world.Pause();
        }

        private void TryApplyDye(ItemStack toDye, EntityPlayer player)
        {
            if (inventoryHeldItem.GetItem() is DyeItem)
            {
                if (toDye.GetItem().HasTag(Item.Tag.DYEABLE))
                {
                    string name = ItemDict.GetColoredItemBaseForm(toDye.GetItem());
                    bool multidye = toDye.GetItem().HasTag(Item.Tag.MULTIDYE);
                    if (inventoryHeldItem.GetItem() != ItemDict.UN_DYE)
                    {
                        name += ((DyeItem)inventoryHeldItem.GetItem()).GetDyedNameAdjustment();
                    }
                    if (!name.Equals(toDye.GetItem().GetName())) //prevent dying object to color it already is
                    {
                        if (toDye.GetQuantity() == 1) //dye the 1 item in place
                        {
                            toDye.SetItem(ItemDict.GetItemByName(name));
                            inventoryHeldItem.Subtract(1); //use one dye
                        }
                        else
                        {
                            bool dyeUsed = false;
                            for (int i = 0; i < (multidye ? 10 : 1); i++)
                            {
                                if (player.AddItemToInventory(ItemDict.GetItemByName(name), false, false)) //otherwise create a new item
                                {
                                    toDye.Subtract(1); //remove one of the dyed item
                                    dyeUsed = true;
                                }
                                if (toDye.GetQuantity() == 0) //ran out of items to dye
                                    break;
                            }
                            if (dyeUsed) //if at least one was successfully dyed
                                inventoryHeldItem.Subtract(1); //use one dye
                        }
                    }
                }
            }
        }

        public void TransitionUp()
        {
            interfaceState = InterfaceState.TRANSITION_TO_UP;
            transitionPosition = new Vector2(0, PlateauMain.NATIVE_RESOLUTION_HEIGHT+12);
            transitionAlpha = 1.0f;
        }

        public void TransitionDown()
        {
            interfaceState = InterfaceState.TRANSITION_TO_DOWN;
            transitionPosition = new Vector2(0, -PlateauMain.NATIVE_RESOLUTION_HEIGHT);
            transitionAlpha = 1.0f;
        }

        public void TransitionLeft()
        {
            interfaceState = InterfaceState.TRANSITION_TO_LEFT;
            transitionPosition = new Vector2(PlateauMain.NATIVE_RESOLUTION_WIDTH, 0);
            transitionAlpha = 1.0f;
        }

        public void TransitionRight()
        {
            interfaceState = InterfaceState.TRANSITION_TO_RIGHT;
            transitionPosition = new Vector2(-PlateauMain.NATIVE_RESOLUTION_WIDTH, 0);
            transitionAlpha = 1.0f;
        }

        public void TransitionFadeToBlack()
        {
            interfaceState = InterfaceState.TRANSITION_FADE_TO_BLACK;
            transitionAlpha = 0.0f;
            transitionPosition = new Vector2(0, 0);
        }

        public void TransitionFadeIn()
        {
            interfaceState = InterfaceState.TRANSITION_FADE_IN;
            transitionAlpha = 3.0f;
            transitionPosition = new Vector2(0, 0);
        }

        public void Update(float deltaTime, EntityPlayer player, RectangleF cameraBoundingBox, Area currentArea, World.TimeData timeData, World world)
        {
            this.player = player;
            selectedHotbarPosition = player.GetSelectedHotbarPosition();
            selectedHotbarItemName = player.GetHeldItem().GetItem().GetName();
            for (int i = 0; i < EntityPlayer.INVENTORY_SIZE; i++)
            {
                inventoryItems[i] = player.GetInventoryItemStack(i);
            }
            heldItem = player.GetHeldItem();
            workbenchCraftablePosition.Clear();
            tooltipName = "";
            tooltipDescription = "";
            areaName = currentArea.GetName();
            zoneName = currentArea.GetZoneName(player.GetCenteredPosition());
            if(displayGold == -1)
            {
                displayGold = player.GetGold();
            }
            int actualGold = player.GetGold();
            if (Math.Abs(actualGold - displayGold) <= 10)
            {
                displayGold = actualGold;
            }
            else
            {
                displayGold = Util.AdjustTowards(displayGold, actualGold, (Math.Abs(actualGold - displayGold) / 25) + 1);
            }

            appliedEffects = player.GetEffects();

            healthBars.Clear();
            foreach (IHaveHealthBar hbEntity in currentArea.GetHealthBarEntities())
            {
                healthBars.Add(hbEntity.GetHealthBar());
            }

            isMouseLeftDown = controller.GetMouseLeftDown();
            isMouseRightDown = controller.GetMouseRightDown();
            isADown = controller.IsKeyDown(KeyBinds.LEFT);
            isDDown = controller.IsKeyDown(KeyBinds.RIGHT);
            isWDown = controller.IsKeyDown(KeyBinds.UP);
            isSDown = controller.IsKeyDown(KeyBinds.DOWN);
            currentNotification = player.GetCurrentNotification();
            isMouseOverCraftingMC = false;
            isMouseOverInventoryMC = false;
            isMouseOverScrapbookMC = false;
            isMouseOverSettingsMC = false;
            isMouseOverEditModeMC = false;

            if (!isHidden)
            {
                if (menuButtons[0].Contains(controller.GetMousePos()))
                {
                    isMouseOverInventoryMC = true;
                    tooltipName = "Inventory";
                    player.IgnoreMouseInputThisFrame();
                }
                else if (menuButtons[1].Contains(controller.GetMousePos()))
                {
                    isMouseOverScrapbookMC = true;
                    tooltipName = "Scrapbook";
                    player.IgnoreMouseInputThisFrame();
                }
                else if (menuButtons[2].Contains(controller.GetMousePos()))
                {
                    isMouseOverCraftingMC = true;
                    tooltipName = "Crafting";
                    player.IgnoreMouseInputThisFrame();
                }
                else if (menuButtons[3].Contains(controller.GetMousePos()))
                {
                    isMouseOverSettingsMC = true;
                    tooltipName = "Settings";
                    player.IgnoreMouseInputThisFrame();
                }
                else if (menuButtons[4].Contains(controller.GetMousePos()))
                {
                    isMouseOverEditModeMC = true;
                    tooltipName = "Toggle Edit Mode";
                    player.IgnoreMouseInputThisFrame();
                }
            }

            dialogueBox.Update(deltaTime);

            switch (timeData.season)
            {
                case (World.Season.SPRING):
                    seasonIndex = 0;
                    break;
                case (World.Season.SUMMER):
                    seasonIndex = 1;
                    break;
                case (World.Season.AUTUMN):
                    seasonIndex = 2;
                    break;
                case (World.Season.WINTER):
                    seasonIndex = 3;
                    break;
            }
            dayIndex = timeData.day;
            hourOnesIndex = timeData.hour % 10;
            hourTensIndex = timeData.hour / 10;
            minuteOnesIndex = timeData.minute % 10;
            minuteTensIndex = timeData.minute / 10;

            if (interfaceState == InterfaceState.TRANSITION_TO_DOWN)
            {
                transitionPosition += new Vector2(0, TRANSITION_DELTA_Y * deltaTime);
                if (transitionPosition.Y > PlateauMain.NATIVE_RESOLUTION_HEIGHT)
                {
                    player.Unpause();
                    interfaceState = InterfaceState.NONE;
                }
            } else if (interfaceState == InterfaceState.TRANSITION_TO_UP)
            {
                transitionPosition += new Vector2(0, -TRANSITION_DELTA_Y * deltaTime);
                if (transitionPosition.Y < -PlateauMain.NATIVE_RESOLUTION_HEIGHT)
                {
                    player.Unpause();
                    interfaceState = InterfaceState.NONE;
                }
            } else if (interfaceState == InterfaceState.TRANSITION_TO_LEFT)
            {
                transitionPosition += new Vector2(-TRANSITION_DELTA_X * deltaTime, 0);
                if (transitionPosition.X < -PlateauMain.NATIVE_RESOLUTION_WIDTH)
                {
                    player.Unpause();
                    interfaceState = InterfaceState.NONE;
                }
            } else if (interfaceState == InterfaceState.TRANSITION_TO_RIGHT)
            {
                transitionPosition += new Vector2(TRANSITION_DELTA_X * deltaTime, 0);
                if(transitionPosition.X > PlateauMain.NATIVE_RESOLUTION_WIDTH)
                {
                    player.Unpause();
                    interfaceState = InterfaceState.NONE;
                }
            } else if (interfaceState == InterfaceState.TRANSITION_FADE_TO_BLACK)
            {
                transitionAlpha += TRANSITION_ALPHA_SPEED * deltaTime;
            } else if (interfaceState == InterfaceState.TRANSITION_FADE_IN)
            {
                transitionAlpha -= TRANSITION_ALPHA_SPEED * deltaTime;
                if (transitionAlpha <= 0.0f)
                {
                    player.Unpause();
                    interfaceState = InterfaceState.NONE;
                }
            }
            else
            {
                if (currentDialogue != null && currentDialogueNumChars >= currentDialogue.dialogueTexts[dialogueNodePage].Length)
                {
                    bounceArrow.Update(deltaTime);
                }

                interfaceState = player.GetInterfaceState();

                if (interfaceState == InterfaceState.CHEST)
                {
                    world.Pause();
                }
                targetTile = player.GetTargettedTile();
                targetEntityLastFrame = targetEntity;
                targetEntity = player.GetTargettedEntity();
                if(targetEntity == null)
                {
                    targetEntity = player.GetTargettedTileEntity();
                }

                if (targetEntity is IHaveHoveringInterface && targetEntity == targetEntityLastFrame)
                {
                    hoveringInterfaceOpacity += HOVERING_INTERFACE_OPACITY_SPEED * deltaTime;
                    hoveringInterfaceOpacity = Math.Min(HOVERING_INTERFACE_MAX_OPACITY, hoveringInterfaceOpacity);
                } else if (targetEntity is IHaveHoveringInterface && targetEntity != null && targetEntity != targetEntityLastFrame)
                {
                    hoveringInterfaceOpacity = 0;
                } else
                {
                    hoveringInterfaceOpacity -= HOVERING_INTERFACE_OPACITY_SPEED * deltaTime;
                    hoveringInterfaceOpacity = Math.Max(0, hoveringInterfaceOpacity);
                }

                inventorySelectedPosition = new Vector2(-100, -100);
                editMode = player.IsEditMode();

                if (currentDialogue == null)
                {
                    currentDialogue = player.GetCurrentDialogue();
                    if (currentDialogue != null)
                    {
                        while (currentDialogue != null && currentDialogue.GetText(Int32.MaxValue, 0).Equals(" \n"))
                        {
                            currentDialogue.OnActivation(player, currentArea, world);
                            currentDialogue = currentDialogue.GetNext(currentArea, world, player);
                        }
                        dialogueNodePage = 0;
                        currentDialogueNumChars = 0;
                        player.SetToDefaultPose();
                        dialogueBox.SetLoop("anim");
                        bounceArrow.SetLoop("anim");
                        inDialogue = true;
                        currentDialogue.OnActivation(player, currentArea, world);
                        //currentDialogue = player.GetCurrentDialogue(); //enables "empty" initial dialogues that get dynamically changed in OnActivation - edit: don't think this is needed or does anything
                    } 
                }
                else
                {
                    currentDialogueNumChars+= DIALOGUE_SPEED_CHARS_PER_FRAME;
                    if (controller.GetMouseLeftPress())
                    {
                        if (currentDialogueNumChars < currentDialogue.dialogueTexts[dialogueNodePage].Length)
                        {
                            currentDialogueNumChars = currentDialogue.dialogueTexts[dialogueNodePage].Length;
                        }
                        else 
                        {
                            if (dialogueNodePage + 1 != currentDialogue.NumPages())
                            {
                                currentDialogueNumChars = 0;
                                bounceArrow.SetLoop("anim");
                                dialogueNodePage++;
                            }
                            else if (!currentDialogue.Splits())
                            {
                                currentDialogueNumChars = 0;
                                dialogueNodePage = 0;
                                bounceArrow.SetLoop("anim");
                                currentDialogue = currentDialogue.GetNext(currentArea, world, player);
                                while (currentDialogue != null && currentDialogue.GetText(Int32.MaxValue, 0).Equals(" \n"))
                                {
                                    currentDialogue.OnActivation(player, currentArea, world);
                                    currentDialogue = currentDialogue.GetNext(currentArea, world, player);
                                }
                                if (currentDialogue == null)
                                {
                                    player.ClearDialogueNode();
                                    player.IgnoreMouseInputThisFrame();
                                } else
                                {
                                    currentDialogue.OnActivation(player, currentArea, world);
                                }
                            }
                        }
                    }
                    else if (controller.GetMouseRightPress())
                    {
                        if (currentDialogueNumChars < currentDialogue.dialogueTexts[dialogueNodePage].Length)
                        {
                            currentDialogueNumChars = currentDialogue.dialogueTexts[dialogueNodePage].Length;
                        }
                    }
                    else if (controller.IsKeyPressed(KeyBinds.LEFT) && dialogueNodePage + 1 == currentDialogue.NumPages())
                    {
                        if (!currentDialogue.decisionLeftText.Equals(""))
                        {
                            currentDialogue = currentDialogue.decisionLeftNode;
                            currentDialogueNumChars = 0;
                            dialogueNodePage = 0;
                            while (currentDialogue != null && currentDialogue.GetText(Int32.MaxValue, 0).Equals(" \n"))
                            {
                                currentDialogue.OnActivation(player, currentArea, world);
                                currentDialogue = currentDialogue.GetNext(currentArea, world, player);
                            }
                            if (currentDialogue == null)
                            {
                                player.ClearDialogueNode();
                            }
                            else
                            {
                                currentDialogue.OnActivation(player, currentArea, world);
                            }
                            bounceArrow.SetLoop("anim");
                        } 
                    }
                    else if (controller.IsKeyPressed(KeyBinds.RIGHT) && dialogueNodePage + 1 == currentDialogue.NumPages())
                    {
                        if (!currentDialogue.decisionRightText.Equals(""))
                        {
                            currentDialogue = currentDialogue.decisionRightNode;
                            currentDialogueNumChars = 0;
                            dialogueNodePage = 0;
                            while (currentDialogue != null && currentDialogue.GetText(Int32.MaxValue, 0).Equals(" \n"))
                            {
                                currentDialogue.OnActivation(player, currentArea, world);
                                currentDialogue = currentDialogue.GetNext(currentArea, world, player);
                            }
                            if (currentDialogue == null)
                            {
                                player.ClearDialogueNode();
                            }
                            else
                            {
                                currentDialogue.OnActivation(player, currentArea, world);
                            }
                            bounceArrow.SetLoop("anim");
                        }

                    }
                    else if (controller.IsKeyPressed(KeyBinds.UP) && dialogueNodePage + 1 == currentDialogue.NumPages())
                    {
                        if (!currentDialogue.decisionUpText.Equals(""))
                        {
                            currentDialogue = currentDialogue.decisionUpNode;
                            currentDialogueNumChars = 0;
                            dialogueNodePage = 0;
                            while (currentDialogue != null && currentDialogue.GetText(Int32.MaxValue, 0).Equals(" \n"))
                            {
                                currentDialogue.OnActivation(player, currentArea, world);
                                currentDialogue = currentDialogue.GetNext(currentArea, world, player);
                            }
                            if (currentDialogue == null)
                            {
                                player.ClearDialogueNode();
                            }
                            else
                            {
                                currentDialogue.OnActivation(player, currentArea, world);
                            }
                            bounceArrow.SetLoop("anim");
                        }
                    }
                    else if (controller.IsKeyPressed(KeyBinds.DOWN) && dialogueNodePage + 1 == currentDialogue.NumPages())
                    {
                        if (!currentDialogue.decisionDownText.Equals(""))
                        {
                            currentDialogue = currentDialogue.decisionDownNode;
                            currentDialogueNumChars = 0;
                            dialogueNodePage = 0;
                            while (currentDialogue != null && currentDialogue.GetText(Int32.MaxValue, 0).Equals(" \n"))
                            {
                                currentDialogue.OnActivation(player, currentArea, world);
                                currentDialogue = currentDialogue.GetNext(currentArea, world, player);
                            }
                            if (currentDialogue == null)
                            {
                                player.ClearDialogueNode();
                            }
                            else
                            {
                                currentDialogue.OnActivation(player, currentArea, world);
                            }
                            bounceArrow.SetLoop("anim");
                        }
                    }
                }

                for (int i = 0; i < menuButtons.Length; i++)
                {
                    menuButtons[i] = new RectangleF(MENU_CONTROL_POSITION + new Vector2(0, i * MENU_DELTA_Y), MENU_BUTTON_SIZE);
                }

                Vector2 mousePosition = controller.GetMousePos();
               
                //if not hidden, accept key presses to open interfaces
                if (!isHidden)
                {
                    //inventory manipulations - mouse wheel, num keys, and tab to cycle
                    if (currentDialogue == null && !player.GetUseTool() && (interfaceState == InterfaceState.INVENTORY || interfaceState == InterfaceState.CHEST || interfaceState == InterfaceState.CRAFTING || interfaceState == InterfaceState.SCRAPBOOK || interfaceState == InterfaceState.NONE || interfaceState == InterfaceState.SETTINGS))
                    {
                        //cycling inventory
                        if (controller.IsKeyPressed(KeyBinds.CYCLE_HOTBAR))
                            player.CycleInventory();

                        //if specifically in normal, chest, or inv mode, toss 1 of held item
                        if ((interfaceState == InterfaceState.NONE || interfaceState == InterfaceState.CHEST || interfaceState == InterfaceState.INVENTORY) && controller.IsKeyPressed(KeyBinds.DISCARD_ITEM))
                        {
                            if(inventoryHeldItem.GetItem () != ItemDict.NONE)
                            {
                                if (controller.IsShiftDown())
                                    DropInventoryHeldItemAll(world);
                                else
                                    DropInventoryHeldItem(world);
                            }
                            else
                            {
                                if (controller.IsShiftDown())
                                    DropHeldItemAll(world);
                                else
                                    DropHeldItem(world);
                            }

                        }

                        //adjust selectedhotbarposition according to mouse wheel movement
                        int newHotbarPosition = player.GetSelectedHotbarPosition() + controller.GetChangeInMouseWheel();
                        if (newHotbarPosition >= HOTBAR_LENGTH)
                            newHotbarPosition = 0;
                        else if (newHotbarPosition < 0)
                            newHotbarPosition = HOTBAR_LENGTH - 1;
                        player.SetSelectedHotbarPosition(newHotbarPosition);

                        //adjust selectedhotbarposition if any of the 1-9 keys are pressed down
                        for (int i = 0; i < HOTBAR_LENGTH; i++)
                            if (controller.IsKeyPressed(KeyBinds.HOTBAR_SELECT[i]))
                                player.SetSelectedHotbarPosition(i);


                    }

                    //clicking on a button in left sidebar
                    if ((controller.GetMouseLeftPress() || controller.GetMouseRightPress()) && currentDialogue == null)
                    {
                        if (menuButtons[0].Contains(mousePosition))
                        {
                            DropInventoryHeldItemAll(world); //throw currently held item out into world, if any
                            if (player.GetInterfaceState() == InterfaceState.INVENTORY)
                            {
                                player.SetInterfaceState(InterfaceState.NONE);
                                player.Unpause();
                                world.Unpause();
                            }
                            else
                            {
                                player.SetInterfaceState(InterfaceState.INVENTORY);
                                player.Pause();
                                world.Pause();
                            }
                            if (!player.IsRolling())
                            {
                                player.SetToDefaultPose();
                            }
                            player.IgnoreMouseInputThisFrame();
                            SoundSystem.PlayFX(SoundSystem.Sound.FX_TEST); //TODO: remove
                        }
                        else if (menuButtons[1].Contains(mousePosition))
                        {
                            DropInventoryHeldItemAll(world); //throw currently held item out into world, if any
                            if (interfaceState == InterfaceState.SCRAPBOOK)
                            {
                                player.SetInterfaceState(InterfaceState.NONE);
                                player.Unpause();
                                world.Unpause();
                            }
                            else
                            {
                                OpenScrapbook(player, timeData, world);
                            }
                            player.IgnoreMouseInputThisFrame();
                        }
                        else if (menuButtons[2].Contains(mousePosition))
                        {
                            DropInventoryHeldItemAll(world); //throw currently held item out into world, if any
                            if (interfaceState == InterfaceState.CRAFTING)
                            {
                                player.SetInterfaceState(InterfaceState.NONE);
                                player.Unpause();
                                world.Unpause();
                            }
                            else
                            {
                                player.SetInterfaceState(InterfaceState.CRAFTING);
                                player.Pause();
                                world.Pause();
                            }
                            player.IgnoreMouseInputThisFrame();
                        }
                        else if (menuButtons[3].Contains(mousePosition))
                        {
                            DropInventoryHeldItemAll(world); //throw currently held item out into world, if any
                            if (interfaceState == InterfaceState.SETTINGS)
                            {
                                player.SetInterfaceState(InterfaceState.NONE);
                                player.Unpause();
                                world.Unpause();
                            }
                            else
                            {
                                player.SetInterfaceState(InterfaceState.SETTINGS);
                                player.Pause();
                                world.Pause();
                            }
                            player.IgnoreMouseInputThisFrame();
                        }
                        else if (menuButtons[4].Contains(mousePosition))
                        {
                            player.ToggleEditMode();
                            player.IgnoreMouseInputThisFrame();
                        }
                    }

                    //place down a placeable
                    if (player.IsEditMode() && interfaceState == InterfaceState.NONE)
                    {
                        if (controller.GetMouseLeftDown() && player.GetHeldItem().GetItem() is PlaceableItem)
                        {
                            PlaceableItem item = (PlaceableItem)player.GetHeldItem().GetItem();
                            Vector2 mouseLocation = Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, controller.GetMousePos());
                            Vector2 tile = new Vector2((int)(mouseLocation.X / 8), (int)(mouseLocation.Y / 8) - (item.GetPlaceableHeight() - 1));

                            if (item.GetPlacementType() == PlaceableItem.PlacementType.NORMAL)
                            {
                                bool isPlaceableLocationValid = currentArea.IsTileEntityPlacementValid((int)tile.X, (int)tile.Y, item.GetPlaceableWidth(), item.GetPlaceableHeight());

                                if (isPlaceableLocationValid)
                                {
                                    Vector2 placementLocation = new Vector2(tile.X * 8, tile.Y * 8);
                                    TileEntity toPlace = (TileEntity)EntityFactory.GetEntity(EntityType.USE_ITEM, item, tile, currentArea);
                                    currentArea.AddTileEntity(toPlace);
                                    player.GetHeldItem().Subtract(1);
                                    player.IgnoreMouseInputThisFrame();
                                    showPlaceableTexture = false;
                                    lastPlacedTile = tile;
                                }
                                else
                                {
                                    if (showPlaceableTexture)
                                    {
                                        player.AddNotification(new EntityPlayer.Notification("This can\'t be placed here.", Color.Red));
                                    }
                                }
                            }
                            else if (item.GetPlacementType() == PlaceableItem.PlacementType.WALL)
                            {
                                bool isWallLocationValid = currentArea.IsWallEntityPlacementValid((int)tile.X, (int)tile.Y, item.GetPlaceableWidth(), item.GetPlaceableHeight());

                                if (isWallLocationValid)
                                {
                                    Vector2 placementLocation = new Vector2(tile.X * 8, tile.Y * 8);
                                    TileEntity toPlace = (TileEntity)EntityFactory.GetEntity(EntityType.USE_ITEM, item, tile, currentArea);
                                    currentArea.AddWallEntity(toPlace);
                                    player.GetHeldItem().Subtract(1);
                                    player.IgnoreMouseInputThisFrame();
                                    showPlaceableTexture = false;
                                    lastPlacedTile = tile;
                                }
                                else
                                {
                                    if (showPlaceableTexture)
                                    {
                                        player.AddNotification(new EntityPlayer.Notification("This can\'t be placed here.", Color.Red));
                                    }
                                }
                            }
                            else if (item.GetPlacementType() == PlaceableItem.PlacementType.CEILING)
                            {

                            }
                            else if (item.GetPlacementType() == PlaceableItem.PlacementType.WALLPAPER)
                            {
                                bool isWallpaperLocationValid = currentArea.IsWallpaperPlacementValid((int)tile.X, (int)tile.Y, item.GetPlaceableWidth(), item.GetPlaceableHeight());
                                if (isWallpaperLocationValid)
                                {
                                    Vector2 placementLocation = new Vector2(tile.X * 8, tile.Y * 8);
                                    PEntityWallpaper toPlace = (PEntityWallpaper)EntityFactory.GetEntity(EntityType.USE_ITEM, item, tile, currentArea);
                                    currentArea.AddWallpaperEntity(toPlace);
                                    player.GetHeldItem().Subtract(1);
                                    player.IgnoreMouseInputThisFrame();
                                    showPlaceableTexture = false;
                                    lastPlacedTile = tile;
                                }
                                else
                                {
                                    if (showPlaceableTexture)
                                    {
                                        Area.Subarea.NameEnum subarea = currentArea.GetSubareaAt(player.GetCollisionRectangle());
                                        if (subarea != Area.Subarea.NameEnum.FARMHOUSECABIN && subarea != Area.Subarea.NameEnum.FARMHOUSEHOUSE &&
                                            subarea != Area.Subarea.NameEnum.FARMHOUSEMANSIONLOWER && subarea != Area.Subarea.NameEnum.FARMHOUSEMANSIONUPPER &&
                                            subarea != Area.Subarea.NameEnum.FARMHOUSECELLAR)
                                            player.AddNotification(new EntityPlayer.Notification("I can only place Wallpaper in my house.", Color.Red));
                                        else
                                            player.AddNotification(new EntityPlayer.Notification("This can\'t be placed here.", Color.Red));
                                    }
                                }
                            }
                            else if (item.GetPlacementType() == PlaceableItem.PlacementType.FLOOR)
                            {
                                bool isFloorPlacementValid = currentArea.IsFloorEntityPlacementValid((int)tile.X, (int)tile.Y, item.GetPlaceableWidth());
                                if (isFloorPlacementValid)
                                {
                                    Vector2 placementLocation = new Vector2(tile.X * 8, tile.Y * 8);
                                    TileEntity toPlace = (TileEntity)EntityFactory.GetEntity(EntityType.USE_ITEM, item, tile, currentArea);
                                    currentArea.AddTileEntity(toPlace);
                                    player.GetHeldItem().Subtract(1);
                                    player.IgnoreMouseInputThisFrame();
                                    showPlaceableTexture = false;
                                    lastPlacedTile = tile;
                                }
                                else
                                {
                                    if (showPlaceableTexture)
                                    {
                                        player.AddNotification(new EntityPlayer.Notification("This can\'t be placed here.", Color.Red));
                                    }
                                }
                            }
                        }
                        else if (controller.GetMouseLeftDown() && player.GetHeldItem().GetItem() is BuildingBlockItem)
                        {
                            if (currentArea.GetAreaEnum() != Area.AreaEnum.FARM)
                            {
                                player.AddNotification(new EntityPlayer.Notification("I can only place scaffolding on my farm.", Color.Red));
                            }
                            else
                            {
                                BuildingBlockItem item = (BuildingBlockItem)player.GetHeldItem().GetItem();
                                Vector2 mouseLocation = Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, controller.GetMousePos());
                                Vector2 tile = new Vector2((int)(mouseLocation.X / 8), (int)(mouseLocation.Y / 8));
                                bool isBBLocationValid = currentArea.IsBuildingBlockPlacementValid((int)tile.X, (int)tile.Y, item.GetBlockType() == BlockType.BLOCK);
                                if (item.GetBlockType() == BlockType.BLOCK && player.GetCollisionRectangle().Intersects(new RectangleF(tile * new Vector2(8, 8), new Vector2(8, 4))))
                                {
                                    isBBLocationValid = false;
                                }

                                if (isBBLocationValid)
                                {
                                    BuildingBlock toPlace = new BuildingBlock(item, tile, item.GetPlacedTexture(), item.GetBlockType());
                                    currentArea.AddBuildingBlock(toPlace);
                                    player.GetHeldItem().Subtract(1);
                                    player.IgnoreMouseInputThisFrame();
                                    showPlaceableTexture = false;
                                    lastPlacedTile = tile;
                                }
                                else
                                {
                                    //also doesn't give message when attempting to place scaffolding over scaffolding
                                    if (showPlaceableTexture && currentArea.GetBuildingBlockAt((int)tile.X, (int)tile.Y) == null)
                                    {
                                        player.AddNotification(new EntityPlayer.Notification("This can\'t be placed here.", Color.Red));
                                    }
                                }
                            }
                        }
                        else if (controller.GetMouseRightDown()) //remove placeable
                        {

                            //TODODOREMOVE
                            Vector2 location = Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, controller.GetMousePos());
                            Vector2 tile = new Vector2((int)(location.X / 8), (int)(location.Y / 8));

                            //first, attempt to remove placeable (floor or wall).
                            //if successful, ONLY remove placeables (floor or wall) until mouse lifted
                            if (removalMode == RemovalMode.ANY || removalMode == RemovalMode.PLACEABLE)
                            {
                                //try ground placeable
                                Item itemForm = currentArea.GetTileEntityItemForm((int)tile.X, (int)tile.Y);
                                if (itemForm != ItemDict.NONE)
                                {
                                    currentArea.RemoveTileEntity(player, (int)tile.X, (int)tile.Y, world);
                                    player.IgnoreMouseInputThisFrame();
                                    removalMode = RemovalMode.PLACEABLE;
                                }

                                //try wall placeable
                                itemForm = currentArea.GetWallEntityItemForm((int)tile.X, (int)tile.Y);
                                if (itemForm != ItemDict.NONE)
                                {
                                    currentArea.RemoveWallEntity(player, (int)tile.X, (int)tile.Y, world);
                                    player.IgnoreMouseInputThisFrame();
                                    removalMode = RemovalMode.PLACEABLE;
                                }
                            }

                            //if we haven't removed a placeable, try to removal wallpaper or scaffolding
                            //if successful, ONLY remove wallpaper or scaffolidng until mouse lifted
                            if (removalMode == RemovalMode.ANY || removalMode == RemovalMode.SCAFFOLDING_AND_WALLPAPER)
                            {
                                Item itemForm = currentArea.GetBuildingBlockItemForm((int)tile.X, (int)tile.Y);
                                if (itemForm != ItemDict.NONE)
                                {
                                    currentArea.RemoveBuildingBlock((int)tile.X, (int)tile.Y, player, world);
                                    player.IgnoreMouseInputThisFrame();
                                    removalMode = RemovalMode.SCAFFOLDING_AND_WALLPAPER;
                                }

                                itemForm = currentArea.GetWallpaperItemForm((int)tile.X, (int)tile.Y);
                                if (itemForm != ItemDict.NONE)
                                {
                                    currentArea.RemoveWallpaperEntity(player, (int)tile.X, (int)tile.Y, world);
                                    player.IgnoreMouseInputThisFrame();
                                    removalMode = RemovalMode.SCAFFOLDING_AND_WALLPAPER;
                                }
                            }
                        }
                        else
                        {
                            removalMode = RemovalMode.ANY;
                        }
                    }


                    if (controller.IsKeyPressed(KeyBinds.CRAFTING) && currentDialogue == null)
                    {
                        DropInventoryHeldItemAll(world); //throw currently held item out into world, if any
                        if (interfaceState == InterfaceState.CRAFTING)
                        {
                            player.SetInterfaceState(InterfaceState.NONE);
                            player.Unpause();
                            world.Unpause();
                        }
                        else
                        {
                            player.SetInterfaceState(InterfaceState.CRAFTING);
                            player.Pause();
                            world.Pause();
                        }
                    }

                    if (controller.IsKeyPressed(KeyBinds.EDITMODE) && currentDialogue == null && interfaceState == InterfaceState.NONE)
                    {
                        player.ToggleEditMode();
                    }

                    if (controller.IsKeyPressed(KeyBinds.SCRAPBOOK) && currentDialogue == null)
                    {
                        DropInventoryHeldItemAll(world); //throw currently held item out into world, if any
                        if (interfaceState == InterfaceState.SCRAPBOOK)
                        {
                            player.SetInterfaceState(InterfaceState.NONE);
                            player.Unpause();
                            world.Unpause();
                        }
                        else
                        {
                            OpenScrapbook(player, timeData, world);
                        }
                    }

                    if (controller.IsKeyPressed(KeyBinds.INVENTORY) && currentDialogue == null)
                    {
                        DropInventoryHeldItemAll(world); //throw currently held item out into world, if any
                        if (player.GetInterfaceState() == InterfaceState.INVENTORY || player.GetInterfaceState() == InterfaceState.CHEST)
                        {
                            player.SetInterfaceState(InterfaceState.NONE);
                            player.Unpause();
                            world.Unpause();
                        }
                        else
                        {
                            player.SetInterfaceState(InterfaceState.INVENTORY);
                            player.Pause();
                            world.Pause();
                        }
                        if (!player.IsRolling())
                        {
                            player.SetToDefaultPose();
                        }
                    }

                    if (controller.IsKeyPressed(KeyBinds.SETTINGS) && currentDialogue == null)
                    {
                        DropInventoryHeldItemAll(world); //throw currently held item out into world, if any
                        if (interfaceState == InterfaceState.SETTINGS)
                        {
                            player.SetInterfaceState(InterfaceState.NONE);
                            player.Unpause();
                            world.Unpause();
                        }
                        else
                        {
                            player.SetInterfaceState(InterfaceState.SETTINGS);
                            player.Pause();
                            world.Pause();
                        }
                    }
                }

                Vector2 locationT = Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, controller.GetMousePos());
                Vector2 tileT = new Vector2((int)(locationT.X / 8), (int)(locationT.Y / 8));
                Item itemFormT = currentArea.GetTileEntityItemForm((int)tileT.X, (int)tileT.Y);

                SetKeyActionTexts(player);
                SetMouseActionTexts(player, itemFormT != ItemDict.NONE);

                timeSinceItemCollectedTooltipAdded += deltaTime;
                playerPosition = player.GetCenteredPosition();

                isHoldingPlaceable = false;
                gridLocation = currentArea.GetPositionOfTile((int)(cameraBoundingBox.Left / 8), (int)(cameraBoundingBox.Top / 8));
                gridLocation.X -= 0.5f;
                gridLocation.Y -= 0.5f;

                if (player.GetInterfaceState() == InterfaceState.INVENTORY || player.GetInterfaceState() == InterfaceState.CHEST)
                {
                    player.UpdateSprite(0);
                    playerSprite = player.GetSprite();
                    hair = (ClothingItem)player.GetHair().GetItem();
                    hat = (ClothingItem)player.GetHat().GetItem();
                    shirt = (ClothingItem)player.GetShirt().GetItem();
                    outerwear = (ClothingItem)player.GetOuterwear().GetItem();
                    pants = (ClothingItem)player.GetPants().GetItem();
                    socks = (ClothingItem)player.GetSocks().GetItem();
                    shoes = (ClothingItem)player.GetShoes().GetItem();
                    gloves = (ClothingItem)player.GetGloves().GetItem();
                    earrings = (ClothingItem)player.GetEarrings().GetItem();
                    scarf = (ClothingItem)player.GetScarf().GetItem();
                    glasses = (ClothingItem)player.GetGlasses().GetItem();
                    back = (ClothingItem)player.GetBack().GetItem();
                    sailcloth = (ClothingItem)player.GetSailcloth().GetItem();
                    accessory1 = player.GetAccessory1().GetItem();
                    accessory2 = player.GetAccessory2().GetItem();
                    accessory3 = player.GetAccessory3().GetItem();

                    bool hovering = false;
                    Vector2 mouse = controller.GetMousePos();
                    for (int i = 0; i < itemRectangles.Length; i++)
                    {
                        if (itemRectangles[i].Contains(mouse))
                        {
                            inventorySelectedPosition = itemRectangles[i].TopLeft;
                            ItemStack hovered = player.GetInventoryItemStack(i);
                            if (hovered.GetItem() == ItemDict.NONE)
                            {
                                break;
                            }
                            hovering = true;
                            tooltipDescription = hovered.GetItem().GetDescription();
                            tooltipName = hovered.GetItem().GetName() + (hovered.GetMaxQuantity() == 1 ? "" : " x" + hovered.GetQuantity());
                        }
                    }

                    if (interfaceState == InterfaceState.INVENTORY)
                    {
                        if (!hovering)
                        {
                            hovering = CheckClothingTooltips(player, mouse);
                        }
                    }
                    else if (interfaceState == InterfaceState.CHEST)
                    {
                        PEntityChest chest = (PEntityChest)player.GetTargettedTileEntity();
                        for (int i = 0; i < chestRectangles.Length; i++)
                        {
                            if (chestRectangles[i].Contains(mouse))
                            {
                                inventorySelectedPosition = chestRectangles[i].TopLeft;
                                ItemStack hovered = chest.GetInventoryItemStack(i);
                                if (hovered.GetItem() == ItemDict.NONE)
                                {
                                    break;
                                }
                                hovering = true;
                                tooltipDescription = hovered.GetItem().GetDescription();

                                tooltipName = hovered.GetItem().GetName() + (hovered.GetMaxQuantity() == 1 ? "" : " x" + hovered.GetQuantity());
                            }
                        }
                    }

                    if (garbageCanRectangle.Contains(mousePosition))
                    {
                        hovering = true;
                        tooltipName = "Garbage Can";
                        tooltipDescription = "WARNING: disposal is permanent.";
                    }

                    if (controller.GetMouseLeftPress())
                    {
                        Vector2 mousePos = controller.GetMousePos();
                        if (garbageCanRectangle.Contains(mousePos))
                        {
                            if (!inventoryHeldItem.GetItem().HasTag(Item.Tag.NO_TRASH))
                            {
                                inventoryHeldItem = new ItemStack(ItemDict.NONE, 0);
                            }
                        } else
                        {
                            foreach(RectangleF dropRectangle in dropRectangles)
                            {
                                if(dropRectangle.Contains(mousePos) && inventoryHeldItem.GetItem() != ItemDict.NONE)
                                {
                                    DropInventoryHeldItemAll(world);
                                }
                            }
                        }

                        for (int i = 0; i < itemRectangles.Length; i++)
                        {
                            if (itemRectangles[i].Contains(mousePos))
                            {
                                ItemStack selected = player.GetInventoryItemStack(i);
                                if (controller.IsShiftDown())
                                {
                                    if (interfaceState == InterfaceState.INVENTORY)
                                    {
                                        if (i >= 10)
                                        {
                                            for (int j = 0; j < GameplayInterface.HOTBAR_LENGTH; j++)
                                            {
                                                ItemStack hotbarStack = player.GetInventoryItemStack(j);
                                                if (hotbarStack.GetItem() == selected.GetItem() && !hotbarStack.IsFull())
                                                {
                                                    int overflow = hotbarStack.Add(selected.GetQuantity());
                                                    selected.SetQuantity(overflow);
                                                    if (selected.GetQuantity() == 0)
                                                    {
                                                        player.RemoveItemStackAt(i);
                                                    }
                                                }
                                            }
                                            if (selected.GetQuantity() > 0)
                                            {
                                                for (int j = 0; j < GameplayInterface.HOTBAR_LENGTH; j++)
                                                {
                                                    ItemStack hotbarStack = player.GetInventoryItemStack(j);
                                                    if (hotbarStack.GetItem() == ItemDict.NONE)
                                                    {
                                                        player.AddItemStackAt(selected, j);
                                                        player.RemoveItemStackAt(i);
                                                        break;
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            for (int j = 10; j < EntityPlayer.INVENTORY_SIZE; j++)
                                            {
                                                ItemStack inventoryStack = player.GetInventoryItemStack(j);
                                                if (inventoryStack.GetItem() == selected.GetItem() && !inventoryStack.IsFull())
                                                {
                                                    int overflow = inventoryStack.Add(selected.GetQuantity());
                                                    selected.SetQuantity(overflow);
                                                    if (selected.GetQuantity() == 0)
                                                    {
                                                        player.RemoveItemStackAt(i);
                                                    }
                                                }
                                            }
                                            if (selected.GetQuantity() > 0)
                                            {
                                                for (int j = 10; j < EntityPlayer.INVENTORY_SIZE; j++)
                                                {
                                                    ItemStack inventoryStack = player.GetInventoryItemStack(j);
                                                    if (inventoryStack.GetItem() == ItemDict.NONE)
                                                    {
                                                        player.AddItemStackAt(selected, j);
                                                        player.RemoveItemStackAt(i);
                                                        break;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    else if (interfaceState == InterfaceState.CHEST)
                                    {
                                        PEntityChest chest = (PEntityChest)player.GetTargettedTileEntity();
                                        for (int j = 0; j < PEntityChest.INVENTORY_SIZE; j++)
                                        {
                                            ItemStack inventoryStack = chest.GetInventoryItemStack(j);
                                            if (inventoryStack.GetItem() == selected.GetItem() && !inventoryStack.IsFull())
                                            {
                                                int overflow = inventoryStack.Add(selected.GetQuantity());
                                                selected.SetQuantity(overflow);
                                                if (selected.GetQuantity() == 0)
                                                {
                                                    player.RemoveItemStackAt(i);
                                                }
                                            }
                                        }
                                        if (selected.GetQuantity() > 0)
                                        {
                                            for (int j = 0; j < PEntityChest.INVENTORY_SIZE; j++)
                                            {
                                                ItemStack inventoryStack = chest.GetInventoryItemStack(j);
                                                if (inventoryStack.GetItem() == ItemDict.NONE)
                                                {
                                                    chest.AddItemStackAt(selected, j);
                                                    player.RemoveItemStackAt(i);
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                }
                                else if (inventoryHeldItem.GetItem() == ItemDict.NONE)
                                {
                                    inventoryHeldItem = selected;
                                    player.RemoveItemStackAt(i);
                                }
                                else if (selected.GetItem() == inventoryHeldItem.GetItem())
                                {
                                    inventoryHeldItem.SetQuantity(selected.Add(inventoryHeldItem.GetQuantity()));
                                    if (inventoryHeldItem.GetQuantity() == 0)
                                    {
                                        inventoryHeldItem = new ItemStack(ItemDict.NONE, 0);
                                    }
                                }
                                else
                                {
                                    player.AddItemStackAt(inventoryHeldItem, i);
                                    inventoryHeldItem = selected;
                                }
                            }
                        }

                        if (player.GetInterfaceState() == InterfaceState.INVENTORY)
                        {
                            CheckClothingClick(player, mousePos, controller.IsShiftDown());
                        }
                        else if (player.GetInterfaceState() == InterfaceState.CHEST) //chest left click
                        {
                            PEntityChest chest = (PEntityChest)player.GetTargettedTileEntity();
                            for (int i = 0; i < chestRectangles.Length; i++)
                            {
                                if (chestRectangles[i].Contains(mousePos))
                                {
                                    ItemStack selected = chest.GetInventoryItemStack(i);
                                    if (controller.IsShiftDown()) //shift click item from chest to inventory
                                    {
                                        bool placedInInventory = false;
                                        //attempt to add to inventory
                                        for (int j = 10; j < EntityPlayer.INVENTORY_SIZE; j++)
                                        {
                                            ItemStack inventoryStack = player.GetInventoryItemStack(j);
                                            if (inventoryStack.GetItem() == selected.GetItem() && !inventoryStack.IsFull())
                                            {
                                                int overflow = inventoryStack.Add(selected.GetQuantity());
                                                selected.SetQuantity(overflow);
                                                if (selected.GetQuantity() == 0)
                                                {
                                                    chest.RemoveItemStackAt(i);
                                                    placedInInventory = true;
                                                }
                                            }
                                        }
                                        if (selected.GetQuantity() > 0) //if stack not found to combine; or extra after combining, place in a NONE slot
                                        {
                                            for (int j = 10; j < EntityPlayer.INVENTORY_SIZE; j++)
                                            {
                                                ItemStack inventoryStack = player.GetInventoryItemStack(j);
                                                if (inventoryStack.GetItem() == ItemDict.NONE)
                                                {
                                                    player.AddItemStackAt(selected, j);
                                                    chest.RemoveItemStackAt(i);
                                                    placedInInventory = true;
                                                    break;
                                                }
                                            }
                                        }

                                        if (!placedInInventory)
                                        {
                                            //attempt to add to hotbar
                                            for (int j = 0; j < HOTBAR_LENGTH; j++)
                                            {
                                                ItemStack inventoryStack = player.GetInventoryItemStack(j);
                                                if (inventoryStack.GetItem() == selected.GetItem() && !inventoryStack.IsFull())
                                                {
                                                    int overflow = inventoryStack.Add(selected.GetQuantity());
                                                    selected.SetQuantity(overflow);
                                                    if (selected.GetQuantity() == 0)
                                                    {
                                                        chest.RemoveItemStackAt(i);
                                                    }
                                                }
                                            }
                                            if (selected.GetQuantity() > 0) //if stack not found to combine; or extra after combining, place in a NONE slot
                                            {
                                                for (int j = 0; j < HOTBAR_LENGTH; j++)
                                                {
                                                    ItemStack inventoryStack = player.GetInventoryItemStack(j);
                                                    if (inventoryStack.GetItem() == ItemDict.NONE)
                                                    {
                                                        player.AddItemStackAt(selected, j);
                                                        chest.RemoveItemStackAt(i);
                                                        break;
                                                    }
                                                }
                                            }
                                        }

                                    }
                                    else if (inventoryHeldItem.GetItem() == ItemDict.NONE) //if not shifting, picking item up
                                    {
                                        inventoryHeldItem = selected;
                                        chest.RemoveItemStackAt(i);
                                    }
                                    else if (selected.GetItem() == inventoryHeldItem.GetItem()) //adding held item into existing stack
                                    {
                                        inventoryHeldItem.SetQuantity(selected.Add(inventoryHeldItem.GetQuantity()));
                                        if (inventoryHeldItem.GetQuantity() == 0)
                                        {
                                            inventoryHeldItem = new ItemStack(ItemDict.NONE, 0);
                                        }
                                    }
                                    else //if not shifting, placing/swapping an item down
                                    {
                                        chest.AddItemStackAt(inventoryHeldItem, i);
                                        inventoryHeldItem = selected;
                                    }
                                }
                            }
                        }
                    }
                    else if (controller.GetMouseRightPress()) //right click...
                    {
                        Vector2 mousePos = controller.GetMousePos();
                        for (int i = 0; i < itemRectangles.Length; i++)
                        {
                            if (itemRectangles[i].Contains(mousePos))
                            {
                                ItemStack selected = player.GetInventoryItemStack(i);
                                if (inventoryHeldItem != null && inventoryHeldItem.GetItem() is DyeItem && selected.GetItem() != ItemDict.NONE)
                                {
                                    if (controller.IsShiftDown())
                                    {
                                        for (int j = 0; j < 9; j++)
                                        {
                                            TryApplyDye(selected, player);
                                        }
                                    }
                                    TryApplyDye(selected, player);
                                }
                                else if ((selected.GetItem() is ClothingItem || selected.GetItem().HasTag(Item.Tag.ACCESSORY)) && controller.IsShiftDown() && interfaceState == InterfaceState.INVENTORY)
                                {
                                    ShiftSwapClothingItem(player, i);
                                }
                                else if (inventoryHeldItem.GetItem() == ItemDict.NONE)
                                {
                                    if (selected.GetQuantity() > 1)
                                    {
                                        inventoryHeldItem = new ItemStack(selected.GetItem(), (int)Math.Floor((double)selected.GetQuantity() / 2));
                                        selected.SetQuantity((int)Math.Ceiling((double)selected.GetQuantity() / 2));
                                    }
                                }
                                else
                                {
                                    if (selected.GetItem() == ItemDict.NONE)
                                    {
                                        player.AddItemStackAt(new ItemStack(inventoryHeldItem.GetItem(), 1), i);
                                        inventoryHeldItem.Subtract(1);
                                        if (inventoryHeldItem.GetQuantity() == 0)
                                        {
                                            inventoryHeldItem = new ItemStack(ItemDict.NONE, 0);
                                        }
                                    }
                                    else if (selected.GetItem() == inventoryHeldItem.GetItem())
                                    {
                                        if (selected.Add(1) != 1)
                                        {
                                            inventoryHeldItem.Subtract(1);
                                            if (inventoryHeldItem.GetQuantity() == 0)
                                            {
                                                inventoryHeldItem = new ItemStack(ItemDict.NONE, 0);
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        if (player.GetInterfaceState() == InterfaceState.INVENTORY)
                        {
                            CheckClothingClick(player, mousePos, controller.IsShiftDown());
                        }
                        else if (player.GetInterfaceState() == InterfaceState.CHEST) //chest right click
                        {
                            PEntityChest chest = (PEntityChest)player.GetTargettedTileEntity();
                            for (int i = 0; i < chestRectangles.Length; i++)
                            {
                                if (chestRectangles[i].Contains(mousePos))
                                {
                                    ItemStack selected = chest.GetInventoryItemStack(i);
                                    if (inventoryHeldItem.GetItem() == ItemDict.NONE) //picking up half a stack
                                    {
                                        if (selected.GetQuantity() > 1)
                                        {
                                            inventoryHeldItem = new ItemStack(selected.GetItem(), (int)Math.Floor((double)selected.GetQuantity() / 2));
                                            selected.SetQuantity((int)Math.Ceiling((double)selected.GetQuantity() / 2));
                                        }
                                    }
                                    else //placing down a single of a held item into chest
                                    {
                                        if (selected.GetItem() == ItemDict.NONE)
                                        {
                                            chest.AddItemStackAt(new ItemStack(inventoryHeldItem.GetItem(), 1), i);
                                            inventoryHeldItem.Subtract(1);
                                            if (inventoryHeldItem.GetQuantity() == 0)
                                            {
                                                inventoryHeldItem = new ItemStack(ItemDict.NONE, 0);
                                            }
                                        }
                                        else if (selected.GetItem() == inventoryHeldItem.GetItem()) //combining stacks...
                                        {
                                            if (selected.Add(1) != 1)
                                            {
                                                inventoryHeldItem.Subtract(1);
                                                if (inventoryHeldItem.GetQuantity() == 0)
                                                {
                                                    inventoryHeldItem = new ItemStack(ItemDict.NONE, 0);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                else if (interfaceState == InterfaceState.SCRAPBOOK)
                { //if scrapbook is open 
                    scrapbookHoverTab = -1;
                    scrapbookHoverPage = -1;
                    for (int i = 0; i < scrapbookTabs.Length; i++)
                    {
                        if (scrapbookTabs[i].Contains(mousePosition))
                        {
                            scrapbookHoverTab = i;
                            if (controller.GetMouseLeftDown() || controller.GetMouseRightDown())
                            {
                                scrapbookCurrentTab = i;
                            }
                        }
                    }

                    for (int i = 0; i < scrapbookTitles.Length; i++)
                    {
                        if (scrapbookTitles[i].Contains(mousePosition))
                        {
                            scrapbookHoverPage = (scrapbookCurrentTab * 10) + i;
                            if (controller.GetMouseLeftDown() || controller.GetMouseRightDown())
                            {
                                scrapbookCurrentPage = (scrapbookCurrentTab * 10) + i;
                            }
                        }
                    }

                }
                else if (interfaceState == InterfaceState.EXIT)
                {
                    if (controller.GetMouseLeftPress() || controller.GetMouseRightPress())
                    {
                        if (exitPromptButton.Contains(mousePosition))
                        {
                            player.SetInterfaceState(InterfaceState.EXIT_CONFIRMED);
                        }
                    }
                }
                else if (interfaceState == InterfaceState.SETTINGS)
                {
                    //if key rebind is started, check for input from controller and process
                    if(currentRebind != Rebinds.NONE && controller.GetStringInput().Length != 0)
                    {
                        //preprocess input
                        String input = controller.GetStringInput();
                        System.Diagnostics.Debug.WriteLine(input);
                        if (input != Keys.Left.ToString() && input != Keys.Right.ToString() && input != Keys.Up.ToString() && input != Keys.Down.ToString() && input != Keys.Tab.ToString())
                            input = input.Substring(0, 1).ToUpper();

                        //attempt to parse into key
                        Keys newHotkey = Keys.None;
                        if (Enum.TryParse(input, out newHotkey) && Util.IsValidHotkey(newHotkey))
                        {
                            //if successful, overwrite the hotkey and deactivate input
                            switch(currentRebind)
                            {
                                case Rebinds.LEFT:
                                    KeyBinds.LEFT = newHotkey;
                                    break;
                                case Rebinds.RIGHT:
                                    KeyBinds.RIGHT = newHotkey;
                                    break;
                                case Rebinds.UP:
                                    KeyBinds.UP = newHotkey;
                                    break;
                                case Rebinds.DOWN:
                                    KeyBinds.DOWN = newHotkey;
                                    break;
                                case Rebinds.INVENTORY:
                                    KeyBinds.INVENTORY = newHotkey;
                                    break;
                                case Rebinds.SCRAPBOOK:
                                    KeyBinds.SCRAPBOOK = newHotkey;
                                    break;
                                case Rebinds.CRAFTING:
                                    KeyBinds.CRAFTING = newHotkey;
                                    break;
                                case Rebinds.SETTINGS:
                                    KeyBinds.SETTINGS = newHotkey;
                                    break;
                                case Rebinds.EDITMODE:
                                    KeyBinds.EDITMODE = newHotkey;
                                    break;
                                case Rebinds.CYCLE_HOTBAR:
                                    KeyBinds.CYCLE_HOTBAR = newHotkey;
                                    break;
                                case Rebinds.DISCARD_ITEM:
                                    KeyBinds.DISCARD_ITEM = newHotkey;
                                    break;
                                default:
                                    throw new Exception();
                            }
                            currentRebind = Rebinds.NONE;
                            controller.DeactivateStringInput();
                            SaveManager.SaveConfig();
                        }
                        else
                        {
                            //otherwise clear the input and wait for another key
                            controller.ClearStringInput();
                        }
                    }

                    if (controller.GetMouseLeftPress() || controller.GetMouseRightPress())
                    {
                        //RESO
                        if(settingsResolutionRectangles[0].Contains(mousePosition))
                        {
                            if(PlateauMain.CanResolutionIncrease())
                            {
                                PlateauMain.IncreaseResolution();
                                Config.RESOLUTION_SCALE = PlateauMain.CURRENT_RESOLUTION.scale;
                                SaveManager.SaveConfig();
                            }
                            //INCREASE RES
                        } else if (settingsResolutionRectangles[1].Contains(mousePosition))
                        {
                            //DECREASE RES
                            if (PlateauMain.CanResolutionDecrease())
                            {
                                PlateauMain.DecreaseResolution();
                                Config.RESOLUTION_SCALE = PlateauMain.CURRENT_RESOLUTION.scale;
                                SaveManager.SaveConfig();
                            }
                        }

                        //SOUND
                        for(int i = 0; i < settingsSFXRectangles.Length; i++)
                        {
                            if(settingsSFXRectangles[i].Contains(mousePosition))
                            {
                                if (Config.SFX_VOLUME == i + 1)
                                {
                                    Config.SFX_VOLUME = 0;
                                    SaveManager.SaveConfig();
                                } else {
                                    Config.SFX_VOLUME = i + 1;
                                    SaveManager.SaveConfig();
                                }
                            }
                        }

                        for (int i = 0; i < settingsMusicRectangles.Length; i++)
                        {
                            if (settingsMusicRectangles[i].Contains(mousePosition))
                            {
                                if (Config.MUSIC_VOLUME == i + 1)
                                {
                                    Config.MUSIC_VOLUME = 0;
                                    SaveManager.SaveConfig();
                                }
                                else
                                {
                                    Config.MUSIC_VOLUME = i + 1;
                                    SaveManager.SaveConfig();
                                }
                            }
                        }

                        //OTHER
                        if (settingsOtherRectangles[0].Contains(mousePosition))
                        {
                            Config.HIDE_CONTROLS = !Config.HIDE_CONTROLS;
                            SaveManager.SaveConfig();
                        }
                        else if (settingsOtherRectangles[1].Contains(mousePosition))
                        {
                            Config.HIDE_GRID = !Config.HIDE_GRID;
                            SaveManager.SaveConfig();
                        }
                        else if (settingsOtherRectangles[2].Contains(mousePosition))
                        {
                            Config.HIDE_RETICLE = !Config.HIDE_RETICLE;
                            SaveManager.SaveConfig();
                        }
                        else if (settingsOtherRectangles[3].Contains(mousePosition))
                        {
                            Config.WINDOWED = !Config.WINDOWED;
                            SaveManager.SaveConfig();
                            PlateauMain.UpdateWindowed();
                        } 

                        //initiate hotkey rebind
                        if(SETTINGS_KEYBIND_LEFT_POSITION.Contains(mousePosition))
                        {
                            KeyBinds.LEFT = Keys.None;
                            currentRebind = Rebinds.LEFT;
                            controller.ActivateStringInput(true);
                        }
                        else if(SETTINGS_KEYBIND_RIGHT_POSITION.Contains(mousePosition))
                        {
                            KeyBinds.RIGHT = Keys.None;
                            currentRebind = Rebinds.RIGHT;
                            controller.ActivateStringInput(true);
                        }
                        else if (SETTINGS_KEYBIND_UP_POSITION.Contains(mousePosition))
                        {
                            KeyBinds.UP = Keys.None;
                            currentRebind = Rebinds.UP;
                            controller.ActivateStringInput(true);
                        }
                        else if (SETTINGS_KEYBIND_DOWN_POSITION.Contains(mousePosition))
                        {
                            KeyBinds.DOWN = Keys.None;
                            currentRebind = Rebinds.DOWN;
                            controller.ActivateStringInput(true);
                        }
                        else if (SETTINGS_KEYBIND_INVENTORY_POSITION.Contains(mousePosition))
                        {
                            KeyBinds.INVENTORY = Keys.None;
                            currentRebind = Rebinds.INVENTORY;
                            controller.ActivateStringInput(true);
                        }
                        else if (SETTINGS_KEYBIND_SCRAPBOOK_POSITION.Contains(mousePosition))
                        {
                            KeyBinds.SCRAPBOOK = Keys.None;
                            currentRebind = Rebinds.SCRAPBOOK;
                            controller.ActivateStringInput(true);
                        }
                        else if (SETTINGS_KEYBIND_CRAFTING_POSITION.Contains(mousePosition))
                        {
                            KeyBinds.CRAFTING = Keys.None;
                            currentRebind = Rebinds.CRAFTING;
                            controller.ActivateStringInput(true);
                        }
                        else if (SETTINGS_KEYBIND_SETTINGS_POSITION.Contains(mousePosition))
                        {
                            KeyBinds.SETTINGS = Keys.None;
                            currentRebind = Rebinds.SETTINGS;
                            controller.ActivateStringInput(true);
                        }
                        else if (SETTINGS_KEYBIND_EDITMODE_POSITION.Contains(mousePosition))
                        {
                            KeyBinds.EDITMODE = Keys.None;
                            currentRebind = Rebinds.EDITMODE;
                            controller.ActivateStringInput(true);
                        }
                        else if (SETTINGS_KEYBIND_CYCLE_HOTBAR_POSITION.Contains(mousePosition))
                        {
                            KeyBinds.CYCLE_HOTBAR = Keys.None;
                            currentRebind = Rebinds.CYCLE_HOTBAR;
                            controller.ActivateStringInput(true);
                        }
                        else if (SETTINGS_KEYBIND_DISCARD_ITEM_POSITION.Contains(mousePosition))
                        {
                            KeyBinds.DISCARD_ITEM = Keys.None;
                            currentRebind = Rebinds.DISCARD_ITEM;
                            controller.ActivateStringInput(true);
                        }
                    }
                } 
                else if (interfaceState == InterfaceState.CRAFTING) //workbench
                {
                    if (selectedRecipe != null)
                    {
                        for (int j = 0; j < numMaterialsOfRecipe.Length; j++)
                        {
                            numMaterialsOfRecipe[j] = 0;
                        }
                        for (int k = 0; k < selectedRecipe.components.Length; k++)
                        {
                            numMaterialsOfRecipe[k] = player.GetNumberOfItemInInventory(selectedRecipe.components[k].GetItem());
                        }
                    }

                    //hover over tab
                    workbenchHoverTab = -1;
                    for (int i = 0; i < workbenchTabRectangles.Length; i++)
                    {
                        if (workbenchTabRectangles[i].Contains(mousePosition))
                        {
                            workbenchHoverTab = i;
                            break;
                        }
                    }

                    if (controller.GetMouseLeftDown()) //change tab
                    {
                        for (int i = 0; i < workbenchTabRectangles.Length; i++)
                        {
                            if (workbenchTabRectangles[i].Contains(mousePosition))
                            {
                                workbenchCurrentTab = i;
                                workbenchCurrentPage = 0;
                                selectedRecipeSlot = -1;
                                break;
                            }
                        }
                    }

                    if (workbenchLeftArrowRectangle.Contains(mousePosition)) //left arrow
                    {
                        hoveringLeftArrow = true;
                        if(controller.GetMouseLeftPress())
                        {
                            int numPagesInCurrentTab = 0;
                            switch (workbenchCurrentTab)
                            {
                                case 0:
                                    numPagesInCurrentTab = (GameState.NumMachineRecipes()-1) / 15;
                                    break;
                                case 1:
                                    numPagesInCurrentTab = (GameState.NumScaffoldingRecipes()-1) / 15;
                                    break;
                                case 2:
                                    numPagesInCurrentTab = (GameState.NumFurnitureRecipes()-1) / 15;
                                    break;
                                case 3:
                                    numPagesInCurrentTab = (GameState.NumWallFloorRecipes()-1) / 15;
                                    break;
                                case 4:
                                    numPagesInCurrentTab = (GameState.NumClothingRecipes()-1) / 15;
                                    break;
                            }

                            if(numPagesInCurrentTab != 0)
                            {
                                selectedRecipeSlot = -1;
                                workbenchCurrentPage--;
                                if(workbenchCurrentPage == -1)
                                {
                                    workbenchCurrentPage = numPagesInCurrentTab;
                                }
                            }
                        }
                    }
                    else if (workbenchRightArrowRectangle.Contains(mousePosition)) //right arrow
                    {
                        hoveringRightArrow = true;
                        if(controller.GetMouseLeftPress())
                        {
                            int numPagesInCurrentTab = 0;
                            switch(workbenchCurrentTab)
                            {
                                case 0:
                                    numPagesInCurrentTab = (GameState.NumMachineRecipes()-1) / 15;
                                    break;
                                case 1:
                                    numPagesInCurrentTab = (GameState.NumScaffoldingRecipes()-1) / 15;
                                    break;
                                case 2:
                                    numPagesInCurrentTab = (GameState.NumFurnitureRecipes()-1) / 15;
                                    break;
                                case 3:
                                    numPagesInCurrentTab = (GameState.NumWallFloorRecipes()-1) / 15;
                                    break;
                                case 4:
                                    numPagesInCurrentTab = (GameState.NumClothingRecipes()-1) / 15;
                                    break;
                            }

                            if (numPagesInCurrentTab > workbenchCurrentPage)
                            {
                                workbenchCurrentPage++;
                                selectedRecipeSlot = -1;
                            } else if (numPagesInCurrentTab == workbenchCurrentPage)
                            {
                                workbenchCurrentPage = 0;
                            }
                        }
                    }
                    else
                    {
                        hoveringLeftArrow = false;
                        hoveringRightArrow = false;
                    }

                    if (workbenchCraftButtonRectangle.Contains(mousePosition)) //craft button
                    {
                        hoveringCraftButton = true;
                        if (controller.GetMouseLeftPress())
                        {
                            for (int j = 0; j < (controller.IsShiftDown() ? 5 : 1); j++) { 
                                //check if player has all needed to craft...
                                bool possible = true;
                                foreach(ItemStack stack in selectedRecipe.components)
                                {
                                    if (!player.HasItemStack(stack))
                                        possible = false;
                                }

                                //if so, attempt to remove those items and add crafted stuff to inv
                                if (possible)
                                {
                                    //remove components
                                    foreach (ItemStack stack in selectedRecipe.components)
                                    {
                                        player.RemoveItemStackFromInventory(stack);
                                    }

                                    //add crafted stuff
                                    bool addedAll = true;
                                    for (int i = 0; i < selectedRecipe.result.GetQuantity(); i++)
                                    {
                                        if (!player.AddItemToInventory(selectedRecipe.result.GetItem())) //if it doesn't fit...
                                        {
                                            //take back added items so far and abort
                                            player.RemoveItemStackFromInventory(new ItemStack(selectedRecipe.result.GetItem(), i));
                                            addedAll = false;
                                            break;
                                        }
                                    }

                                    //if unsuccessful refund the items used
                                    if (!addedAll)
                                    {
                                        foreach (ItemStack stack in selectedRecipe.components)
                                        {
                                            for (int i = 0; i < stack.GetQuantity(); i++)
                                            {
                                                player.AddItemToInventory(stack.GetItem());
                                            }
                                        }
                                        player.AddNotification(new EntityPlayer.Notification("I don/'t have enough space in my bag to craft this.", Color.Red));
                                    } else
                                    {
                                        GameState.STATISTICS[GameState.STAT_ITEMS_CRAFTED] += 1;
                                    }
                                }
                            }
                        }
                    } else
                    {
                        hoveringCraftButton = false;
                    }

                    for(int i = 0; i < 15; i++)
                    {
                        switch (workbenchCurrentTab)
                        {
                            case 0:
                                currentRecipes[i] = GameState.GetMachineRecipe((workbenchCurrentPage * 15) + i);
                                break;
                            case 1:
                                currentRecipes[i] = GameState.GetScaffoldingRecipe((workbenchCurrentPage * 15) + i);
                                break;
                            case 2:
                                currentRecipes[i] = GameState.GetFurnitureRecipe((workbenchCurrentPage * 15) + i);
                                break;
                            case 3:
                                currentRecipes[i] = GameState.GetWallFloorRecipe((workbenchCurrentPage * 15) + i);
                                break;
                            case 4:
                                currentRecipes[i] = GameState.GetClothingRecipe((workbenchCurrentPage * 15) + i);
                                break;
                        } 
                    }

                    workbenchInventorySelectedPosition = new Vector2(-1000, -1000);
                    for (int i = 0; i < workbenchBlueprintRectangles.Length; i++) //blueprint rects
                    {
                        if (workbenchBlueprintRectangles[i].Contains(mousePosition))
                        {
                            workbenchInventorySelectedPosition = workbenchBlueprintRectangles[i].TopLeft;
                            if(currentRecipes[i] != null)
                            {
                                if (currentRecipes[i].haveBlueprint)
                                {
                                    Item result = currentRecipes[i].result.GetItem();
                                    tooltipName = result.GetName();
                                } else
                                {
                                    tooltipName = "???";
                                }
                            }
                            if (controller.GetMouseLeftDown() && currentRecipes[i] != null && currentRecipes[i].haveBlueprint)
                            {
                                selectedRecipeSlot = i;
                                selectedRecipe = currentRecipes[i];
                                for (int j = 0; j < numMaterialsOfRecipe.Length; j++)
                                {
                                    numMaterialsOfRecipe[j] = 0;
                                }
                                for (int k = 0; k < selectedRecipe.components.Length; k++)
                                {
                                    numMaterialsOfRecipe[k] = player.GetNumberOfItemInInventory(selectedRecipe.components[k].GetItem());
                                }
                            }
                        }
                        if (currentRecipes[i] != null && currentRecipes[i].haveBlueprint)
                        {
                            bool possible = true;
                            foreach (ItemStack stack in currentRecipes[i].components)
                            {
                                if (!player.HasItemStack(stack))
                                    possible = false;
                            }
                            if (possible)
                            {
                                workbenchCraftablePosition.Add(workbenchBlueprintRectangles[i].TopLeft);
                            }
                        }
                    }

                    //set tooltip
                    if(selectedRecipe != null)
                    {
                        if(new RectangleF(WORKBENCH_SELECTED_RECIPE_POSITION, new Vector2(16, 16)).Contains(mousePosition)) {
                            tooltipName = selectedRecipe.result.GetItem().GetName();
                        } else if (new RectangleF(WORKBENCH_SELECTED_RECIPE_COMPONENT_1, new Vector2(16, 16)).Contains(mousePosition) || new RectangleF(WORKBENCH_SELECTED_RECIPE_COMPONENT_1+new Vector2(0, haveBoxesDeltaY), new Vector2(16, 16)).Contains(mousePosition))
                        {
                            if (selectedRecipe.components.Length >= 1)
                            {
                                tooltipName = selectedRecipe.components[0].GetItem().GetName();
                            }
                        } else if (new RectangleF(WORKBENCH_SELECTED_RECIPE_COMPONENT_2, new Vector2(16, 16)).Contains(mousePosition) || new RectangleF(WORKBENCH_SELECTED_RECIPE_COMPONENT_2 + new Vector2(0, haveBoxesDeltaY), new Vector2(16, 16)).Contains(mousePosition))
                        {
                            if (selectedRecipe.components.Length >= 2)
                            {
                                tooltipName = selectedRecipe.components[1].GetItem().GetName();
                            }
                        } else if (new RectangleF(WORKBENCH_SELECTED_RECIPE_COMPONENT_3, new Vector2(16, 16)).Contains(mousePosition) || new RectangleF(WORKBENCH_SELECTED_RECIPE_COMPONENT_3 + new Vector2(0, haveBoxesDeltaY), new Vector2(16, 16)).Contains(mousePosition))
                        {
                            if (selectedRecipe.components.Length >= 3)
                            {
                                tooltipName = selectedRecipe.components[2].GetItem().GetName();
                            }
                        } else if (new RectangleF(WORKBENCH_SELECTED_RECIPE_COMPONENT_4, new Vector2(16, 16)).Contains(mousePosition) || new RectangleF(WORKBENCH_SELECTED_RECIPE_COMPONENT_4 + new Vector2(0, haveBoxesDeltaY), new Vector2(16, 16)).Contains(mousePosition))
                        {
                            if (selectedRecipe.components.Length >= 4)
                            {
                                tooltipName = selectedRecipe.components[3].GetItem().GetName();
                            }
                        }
                    }
                }
                else //if inventory/scrapbook is not open
                {
                    if (player.GetItemsCollectedRecently().Count == 0)
                        itemCollectedTooltipFast = false;

                    if (timeSinceItemCollectedTooltipAdded >= ITEM_COLLECTED_TOOLTIP_DELAY || (itemCollectedTooltipFast && timeSinceItemCollectedTooltipAdded >= ITEM_COLLECTED_TOOLTIP_DELAY_FAST))
                    {
                        if (player.GetItemsCollectedRecently().Count >= CollectedTooltip.FAST_THRESHOLD) //if above threshold, make all fast until cleared
                        {
                            itemCollectedTooltipFast = true;
                            //make older ones fast too
                            foreach(CollectedTooltip ct in collectedTooltips)
                                ct.fast = true;
                        }

                        List<Item> itemsCollectedRecently = player.GetItemsCollectedRecently();
                        if (itemsCollectedRecently.Count != 0)
                        {
                            Item newItem = itemsCollectedRecently[0];
                            collectedTooltips.Add(new CollectedTooltip(newItem.GetName(), itemCollectedTooltipFast));
                            itemsCollectedRecently.Remove(newItem);
                            timeSinceItemCollectedTooltipAdded = 0.0f;
                        }
                    }

                    List<CollectedTooltip> finished = new List<CollectedTooltip>();
                    foreach (CollectedTooltip ct in collectedTooltips)
                    {
                        ct.Update(deltaTime);
                        if (ct.IsFinished())
                        {
                            finished.Add(ct);
                        }
                    }
                    foreach (CollectedTooltip finishedCT in finished)
                    {
                        collectedTooltips.Remove(finishedCT);
                    }

                    if (player.GetHeldItem().GetItem() is PlaceableItem && interfaceState == InterfaceState.NONE && player.IsEditMode())
                    {
                        PlaceableItem item = (PlaceableItem)player.GetHeldItem().GetItem();
                        isHoldingPlaceable = true;
                        placeableTexture = item.GetPreviewTexture();
                        Vector2 location = Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, controller.GetMousePos());
                        Vector2 tile = new Vector2((int)(location.X / 8), (int)(location.Y / 8) - (item.GetPlaceableHeight() - 1));
                        placeableLocation = currentArea.GetPositionOfTile((int)tile.X, (int)tile.Y);
                        isPlaceableLocationValid = false;
                        if (item.GetPlacementType() == PlaceableItem.PlacementType.NORMAL)
                        {
                            placeableLocation.Y += 1;
                            isPlaceableLocationValid = currentArea.IsTileEntityPlacementValid((int)tile.X, (int)tile.Y, item.GetPlaceableWidth(), item.GetPlaceableHeight());
                        } else if (item.GetPlacementType() == PlaceableItem.PlacementType.WALL)
                        {
                            isPlaceableLocationValid = currentArea.IsWallEntityPlacementValid((int)tile.X, (int)tile.Y, item.GetPlaceableWidth(), item.GetPlaceableHeight());
                        } else if (item.GetPlacementType() == PlaceableItem.PlacementType.WALLPAPER)
                        {
                            isPlaceableLocationValid = currentArea.IsWallpaperPlacementValid((int)tile.X, (int)tile.Y, item.GetPlaceableWidth(), item.GetPlaceableHeight());
                        }
                        else if (item.GetPlacementType() == PlaceableItem.PlacementType.CEILING)
                        {
                            //ceiling todo
                        } else if (item.GetPlacementType() == PlaceableItem.PlacementType.FLOOR)
                        {
                            isPlaceableLocationValid = currentArea.IsFloorEntityPlacementValid((int)tile.X, (int)tile.Y, item.GetPlaceableWidth());
                        }

                        if (!showPlaceableTexture)
                        {
                            if (lastPlacedTile.X != tile.X || lastPlacedTile.Y != tile.Y)
                            {
                                showPlaceableTexture = true;
                                lastPlacedTile = new Vector2(-1000, -1000);
                            }
                        }
                    }
                    else if (player.GetHeldItem().GetItem() is BuildingBlockItem && player.IsEditMode() && interfaceState == InterfaceState.NONE)
                    {
                        BuildingBlockItem item = (BuildingBlockItem)player.GetHeldItem().GetItem();
                        isHoldingPlaceable = true;
                        placeableTexture = item.GetPlacedTexture();
                        Vector2 location = Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, controller.GetMousePos());
                        Vector2 tile = new Vector2((int)(location.X / 8), (int)(location.Y / 8));
                        placeableLocation = currentArea.GetPositionOfTile((int)tile.X, (int)tile.Y);
                        isPlaceableLocationValid = currentArea.IsBuildingBlockPlacementValid((int)tile.X, (int)tile.Y, item.GetBlockType() == BlockType.BLOCK);
                        if(currentArea.GetAreaEnum() != Area.AreaEnum.FARM)
                        {
                            isPlaceableLocationValid = false;
                        }
                        if (!showPlaceableTexture)
                        {
                            if (lastPlacedTile.X != tile.X || lastPlacedTile.Y != tile.Y)
                            {
                                showPlaceableTexture = true;
                                lastPlacedTile = new Vector2(-1000, -1000);
                            }
                        }
                    }
                }

                if (player.GetInterfaceState() == InterfaceState.CHEST)
                {
                    PEntityChest chest = (PEntityChest)player.GetTargettedTileEntity(); ;
                    for (int i = 0; i < PEntityChest.INVENTORY_SIZE; i++)
                    {
                        chestInventory[i] = chest.GetInventoryItemStack(i);
                    }
                    chestColor = ((PlaceableItem)chest.GetItemForm()).GetBaseColor();
                }

                drawReticle = player.IsGrounded() && !player.IsRolling();
            }
        }

        public void Draw(SpriteBatch sb, RectangleF cameraBoundingBox, float layerDepth)
        {
            //draw grid
            if(editMode && !Config.HIDE_GRID)
            {
                sb.Draw(grid, gridLocation, Color.White * GRID_OPACITY);
            }

            //draw reticle
            if (!Config.HIDE_RETICLE && drawReticle && !isHidden)
            {
                sb.Draw(reticle, targetTile * new Vector2(8, 8), Color.White);
            }

            //draw hovering interface
            if (targetEntity != null && targetEntity is IHaveHoveringInterface)
            {
                currentHoveringInterface = ((IHaveHoveringInterface)targetEntity).GetHoveringInterface(player);
                if (currentHoveringInterface != null)
                {
                    Vector2 hoveringSize = currentHoveringInterface.GetSize();
                    hoveringInterfacePosition = targetEntity.GetPosition();
                    hoveringInterfacePosition.Y -= 10;
                    RectangleF targetSize = targetEntity.GetCollisionRectangle();
                    hoveringInterfacePosition.X += targetSize.Width / 2;
                    hoveringInterfacePosition.X -= hoveringSize.X / 2;
                    hoveringInterfacePosition.Y -= hoveringSize.Y;
                }
                else
                {
                    hoveringInterfaceOpacity = 0;
                }
            }
            if (currentHoveringInterface != null && interfaceState == InterfaceState.NONE && !isHidden)
            {
                currentHoveringInterface.Draw(sb, hoveringInterfacePosition, hoveringInterfaceOpacity, cameraBoundingBox);
            }

            //draw healthbars
            foreach (HealthBar healthBar in healthBars)
            {
                healthBar.Draw(sb, layerDepth);
            }

            //draw collected tooltips
            if (interfaceState == InterfaceState.NONE)
            {
                if (!isHidden)
                {
                    foreach (CollectedTooltip ct in collectedTooltips)
                    {
                        string text = " + " + ct.name;
                        Vector2 nameSize = PlateauMain.FONT.MeasureString(text) * PlateauMain.FONT_SCALE;
                        Vector2 position = new Vector2(playerPosition.X - (0.5f * nameSize.X), playerPosition.Y - ITEM_COLLECTED_TOOLTIP_Y_ADDITION - ct.GetYAdjustment());
                        float opacity = 1.0f;
                        if (ct.timeElapsed >= CollectedTooltip.LENGTH - 0.2f)
                        {
                            opacity = 0.7f;
                            if (ct.timeElapsed >= CollectedTooltip.LENGTH - 0.1f)
                            {
                                opacity = 0.4f;
                            }
                        }
                        //collectedtooltip color
                        QUEUED_STRINGS.Add(new QueuedString(text, position, Color.DarkGreen * opacity));
                    }
                }
            }

            //draw scrapbook
            if (interfaceState == InterfaceState.SCRAPBOOK)
            {
                sb.Draw(black_background, Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, BACKGROUND_BLACK_OFFSET), Color.White * BLACK_BACKGROUND_OPACITY);
                sb.Draw(scrapbookBase, Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, SCRAPBOOK_POSITION), Color.White);
                switch (scrapbookCurrentTab)
                {
                    case 0:
                        sb.Draw(scrapbookHoverTab == 0 ? scrapbookTab1ActiveHover : scrapbookTab1Active,
                            Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, SCRAPBOOK_TAB1_POSITION + (scrapbookHoverTab == 0 ? new Vector2(-1, 0) : new Vector2(1, 1))), Color.White);
                        break;
                    case 1:
                        sb.Draw(scrapbookHoverTab == 1 ? scrapbookTab2ActiveHover : scrapbookTab2Active,
                            Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, SCRAPBOOK_TAB2_POSITION + (scrapbookHoverTab == 1 ? new Vector2(-1, 0) : new Vector2(1, 1))), Color.White);
                        break;
                    case 2:
                        sb.Draw(scrapbookHoverTab == 2 ? scrapbookTab3ActiveHover : scrapbookTab3Active,
                            Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, SCRAPBOOK_TAB3_POSITION + (scrapbookHoverTab == 2 ? new Vector2(-1, 0) : new Vector2(1, 1))), Color.White);
                        break;
                    case 3:
                        sb.Draw(scrapbookHoverTab == 3 ? scrapbookTab4ActiveHover : scrapbookTab4Active,
                            Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, SCRAPBOOK_TAB4_POSITION + (scrapbookHoverTab == 3 ? new Vector2(-1, 0) : new Vector2(1, 1))), Color.White);
                        break;
                    case 4:
                        sb.Draw(scrapbookHoverTab == 4 ? scrapbookTab5ActiveHover : scrapbookTab5Active,
                            Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, SCRAPBOOK_TAB5_POSITION + (scrapbookHoverTab == 4 ? new Vector2(-1, 0) : new Vector2(1, 1))), Color.White);
                        break;
                    case 5:
                        sb.Draw(scrapbookHoverTab == 5 ? scrapbookTab6ActiveHover : scrapbookTab6Active,
                            Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, SCRAPBOOK_TAB6_POSITION + (scrapbookHoverTab == 5 ? new Vector2(-1, 0) : new Vector2(1, 1))), Color.White);
                        break;
                    case 6:
                        sb.Draw(scrapbookHoverTab == 6 ? scrapbookTab7ActiveHover : scrapbookTab7Active,
                            Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, SCRAPBOOK_TAB7_POSITION + (scrapbookHoverTab == 6 ? new Vector2(-1, 0) : new Vector2(1, 1))), Color.White);
                        break;
                    case 7:
                        sb.Draw(scrapbookHoverTab == 7 ? scrapbookTab8ActiveHover : scrapbookTab8Active,
                            Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, SCRAPBOOK_TAB8_POSITION + (scrapbookHoverTab == 7 ? new Vector2(-1, 0) : new Vector2(1, 1))), Color.White);
                        break;
                    case 8:
                        sb.Draw(scrapbookHoverTab == 8 ? scrapbookTab9ActiveHover : scrapbookTab9Active,
                            Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, SCRAPBOOK_TAB9_POSITION + (scrapbookHoverTab == 8 ? new Vector2(-1, 0) : new Vector2(1, 1))), Color.White);
                        break;
                    case 9:
                        sb.Draw(scrapbookHoverTab == 9 ? scrapbookTab10ActiveHover : scrapbookTab10Active,
                            Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, SCRAPBOOK_TAB10_POSITION + (scrapbookHoverTab == 9 ? new Vector2(-1, 0) : new Vector2(1, 1))), Color.White);
                        break;
                    case 10:
                        sb.Draw(scrapbookHoverTab == 10 ? scrapbookTab11ActiveHover : scrapbookTab11Active,
                            Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, SCRAPBOOK_TAB11_POSITION + (scrapbookHoverTab == 10 ? new Vector2(-1, 0) : new Vector2(1, 1))), Color.White);
                        break;
                }
                if (scrapbookHoverTab != scrapbookCurrentTab)
                {
                    switch (scrapbookHoverTab)
                    {
                        case 0:
                            sb.Draw(scrapbookTab1Hover, Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, SCRAPBOOK_TAB1_POSITION + new Vector2(-1, 0)), Color.White);
                            break;
                        case 1:
                            sb.Draw(scrapbookTab2Hover, Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, SCRAPBOOK_TAB2_POSITION + new Vector2(-1, 0)), Color.White);
                            break;
                        case 2:
                            sb.Draw(scrapbookTab3Hover, Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, SCRAPBOOK_TAB3_POSITION + new Vector2(-1, 0)), Color.White);
                            break;
                        case 3:
                            sb.Draw(scrapbookTab4Hover, Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, SCRAPBOOK_TAB4_POSITION + new Vector2(-1, 0)), Color.White);
                            break;
                        case 4:
                            sb.Draw(scrapbookTab5Hover, Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, SCRAPBOOK_TAB5_POSITION + new Vector2(-1, 0)), Color.White);
                            break;
                        case 5:
                            sb.Draw(scrapbookTab6Hover, Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, SCRAPBOOK_TAB6_POSITION + new Vector2(-1, 0)), Color.White);
                            break;
                        case 6:
                            sb.Draw(scrapbookTab7Hover, Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, SCRAPBOOK_TAB7_POSITION + new Vector2(-1, 0)), Color.White);
                            break;
                        case 7:
                            sb.Draw(scrapbookTab8Hover, Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, SCRAPBOOK_TAB8_POSITION + new Vector2(-1, 0)), Color.White);
                            break;
                        case 8:
                            sb.Draw(scrapbookTab9Hover, Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, SCRAPBOOK_TAB9_POSITION + new Vector2(-1, 0)), Color.White);
                            break;
                        case 9:
                            sb.Draw(scrapbookTab10Hover, Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, SCRAPBOOK_TAB10_POSITION + new Vector2(-1, 0)), Color.White);
                            break;
                        case 10:
                            sb.Draw(scrapbookTab11Hover, Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, SCRAPBOOK_TAB11_POSITION + new Vector2(-1, 0)), Color.White);
                            break;
                    }
                }

                //draw selected title
                if (((int)scrapbookCurrentPage / 10) == scrapbookCurrentTab)
                {
                    sb.Draw(scrapbookHoverPage == scrapbookCurrentPage ? scrapbookTitleActiveHover : scrapbookTitleActive,
                        Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, scrapbookTitles[scrapbookCurrentPage % 10].Position) + (scrapbookHoverPage == scrapbookCurrentPage ? new Vector2(1, 0) : new Vector2(2, 1)), Color.White);
                }
                if (scrapbookHoverPage != -1 && scrapbookHoverPage != scrapbookCurrentPage && ((int)scrapbookHoverPage / 10) == scrapbookCurrentTab)
                {
                    sb.Draw(scrapbookTitleHover, Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, scrapbookTitles[scrapbookHoverPage % 10].Position) + new Vector2(1, 0), Color.White);
                }

                //draw titles...
                for (int i = scrapbookCurrentTab * 10; i < (scrapbookCurrentTab * 10) + 10; i++)
                {
                    QUEUED_STRINGS.Add(new QueuedString(scrapbookPages[i].GetName(), Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, scrapbookTitles[i % 10].Position) + new Vector2(4, 3), Color.Black));
                }

                scrapbookPages[scrapbookCurrentPage].Draw(sb, cameraBoundingBox, QUEUED_STRINGS);
            }
            else if (interfaceState == InterfaceState.INVENTORY || interfaceState == InterfaceState.CHEST)
            {
                sb.Draw(black_background, Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, BACKGROUND_BLACK_OFFSET), Color.White * BLACK_BACKGROUND_OPACITY);

                //draw inventory
                sb.Draw(inventory, Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, INVENTORY_POSITION), Color.White);
                //sb.Draw(playerSprite, Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, INVENTORY_PLAYER_PREVIEW), null, Color.White, 0.0f, Vector2.Zero, 2.0f, SpriteEffects.None, 0.0f);
                playerSprite.Draw(sb, Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, INVENTORY_PLAYER_PREVIEW), layerDepth, 2.0f);
                //draw clothing
                glasses.Draw(sb, Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, GLASSES_INVENTORY_RECT.TopLeft) + new Vector2(1, 1), Color.White, layerDepth);
                back.Draw(sb, Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, BACK_INVENTORY_RECT.TopLeft) + new Vector2(1, 1), Color.White, layerDepth);
                sailcloth.Draw(sb, Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, SAILCLOTH_INVENTORY_RECT.TopLeft) + new Vector2(1, 1), Color.White, layerDepth);
                scarf.Draw(sb, Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, SCARF_INVENTORY_RECT.TopLeft) + new Vector2(1, 1), Color.White, layerDepth);
                outerwear.Draw(sb, Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, OUTERWEAR_INVENTORY_RECT.TopLeft) + new Vector2(1, 1), Color.White, layerDepth);
                socks.Draw(sb, Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, SOCKS_INVENTORY_RECT.TopLeft) + new Vector2(1, 1), Color.White, layerDepth);
                hat.Draw(sb, Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, HAT_INVENTORY_RECT.TopLeft) + new Vector2(1, 1), Color.White, layerDepth);
                shirt.Draw(sb, Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, SHIRT_INVENTORY_RECT.TopLeft) + new Vector2(1, 1), Color.White, layerDepth);
                pants.Draw(sb, Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, PANTS_INVENTORY_RECT.TopLeft) + new Vector2(1, 1), Color.White, layerDepth);
                earrings.Draw(sb, Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, EARRINGS_INVENTORY_RECT.TopLeft) + new Vector2(1, 1), Color.White, layerDepth);
                gloves.Draw(sb, Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, GLOVES_INVENTORY_RECT.TopLeft) + new Vector2(1, 1), Color.White, layerDepth);
                shoes.Draw(sb, Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, SHOES_INVENTORY_RECT.TopLeft) + new Vector2(1, 1), Color.White, layerDepth);
                accessory1.Draw(sb, Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, ACCESSORY1_INVENTORY_RECT.TopLeft) + new Vector2(1, 1), Color.White, layerDepth);
                accessory2.Draw(sb, Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, ACCESSORY2_INVENTORY_RECT.TopLeft) + new Vector2(1, 1), Color.White, layerDepth);
                accessory3.Draw(sb, Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, ACCESSORY3_INVENTORY_RECT.TopLeft) + new Vector2(1, 1), Color.White, layerDepth);
                DrawClothingHeldIndicator(sb, cameraBoundingBox);

                //draw garbage can
                if (garbageCanRectangle.Contains(controller.GetMousePos()) && inventoryHeldItem.GetItem() != ItemDict.NONE && !inventoryHeldItem.GetItem().HasTag(Item.Tag.NO_TRASH))
                {
                    sb.Draw(garbageCanOpen, Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, GARBAGE_CAN_POSITION), Color.White);
                }
                else
                {
                    sb.Draw(garbageCanClosed, Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, GARBAGE_CAN_POSITION), Color.White);
                }


                for (int i = 10; i < EntityPlayer.INVENTORY_SIZE; i++)
                {
                    Item item = inventoryItems[i].GetItem();
                    Vector2 position = Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, new Vector2(itemRectangles[i].X, itemRectangles[i].Y)) + new Vector2(1, 1);
                    item.Draw(sb, position, Color.White, layerDepth);
                    if (item.GetStackCapacity() != 1 && inventoryItems[i].GetQuantity() != 0)
                    {
                        Vector2 itemQuantityPosition = new Vector2(itemRectangles[i].X + 12, itemRectangles[i].Y + 10);
                        sb.Draw(numbers[inventoryItems[i].GetQuantity() % 10], Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, itemQuantityPosition), Color.White);
                        if (inventoryItems[i].GetQuantity() >= 10)
                        {
                            itemQuantityPosition.X -= 4;
                            sb.Draw(numbers[inventoryItems[i].GetQuantity() / 10], Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, itemQuantityPosition), Color.White);
                        }
                    }
                }

                if (interfaceState == InterfaceState.CHEST)
                {
                    if (chestColor != Color.White)
                    {
                        sb.Draw(chest_inventory_greyscale, Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, CHEST_INVENTORY_POSITION), chestColor);
                    }
                    else
                    {
                        sb.Draw(chest_inventory, Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, CHEST_INVENTORY_POSITION), Util.DEFAULT_COLOR.color);
                    }

                    for (int i = 0; i < PEntityChest.INVENTORY_SIZE; i++)
                    {
                        Item item = chestInventory[i].GetItem();
                        Vector2 position = Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, new Vector2(chestRectangles[i].X, chestRectangles[i].Y)) + new Vector2(1, 1);
                        item.Draw(sb, position, Color.White, layerDepth);
                        if (item.GetStackCapacity() != 1 && chestInventory[i].GetQuantity() != 0)
                        {
                            Vector2 chestQuantityPosition = new Vector2(chestRectangles[i].X + 12, chestRectangles[i].Y + 10);
                            sb.Draw(numbers[chestInventory[i].GetQuantity() % 10], Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, chestQuantityPosition), Color.White);
                            if (chestInventory[i].GetQuantity() >= 10)
                            {
                                chestQuantityPosition.X -= 4;
                                sb.Draw(numbers[chestInventory[i].GetQuantity() / 10], Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, chestQuantityPosition), Color.White);
                            }
                        }
                    }
                }
            }
            else if (interfaceState == InterfaceState.EXIT)
            {
                sb.Draw(black_background, Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, BACKGROUND_BLACK_OFFSET), Color.White * BLACK_BACKGROUND_OPACITY);
                sb.Draw(exitPrompt, Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, EXIT_PROMPT_POSITION), Color.White);
                if (exitPromptButton.Contains(controller.GetMousePos()))
                {
                    sb.Draw(exitButtonEnlarge, Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, exitPromptButton.TopLeft) + new Vector2(-1, -1), Color.White);
                }
            }
            else if (interfaceState == InterfaceState.SETTINGS)
            {
                if(!isHidden) //when showing on main menu, don't black out background
                    sb.Draw(black_background, Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, BACKGROUND_BLACK_OFFSET), Color.White * BLACK_BACKGROUND_OPACITY);
                sb.Draw(settings, Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, SETTINGS_POSITION), Color.White);

                if (settingsOtherRectangles[0].Contains(controller.GetMousePos()))
                {
                    sb.Draw(checkmark_hover, Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, settingsOtherRectangles[0].TopLeft) + new Vector2(1, 1), Color.White);
                    tooltipName = "Hides the mouse and keyboard controls in the bottom left/right of the screen.";
                }
                else if (settingsOtherRectangles[1].Contains(controller.GetMousePos()))
                {
                    sb.Draw(checkmark_hover, Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, settingsOtherRectangles[1].TopLeft) + new Vector2(1, 1), Color.White);
                    tooltipName = "Hides the grid when in Edit Mode.";
                }
                else if (settingsOtherRectangles[2].Contains(controller.GetMousePos()))
                {
                    sb.Draw(checkmark_hover, Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, settingsOtherRectangles[2].TopLeft) + new Vector2(1, 1), Color.White);
                    tooltipName = "Hides the golden box showing the current tile being targetted.";
                }
                else if (settingsOtherRectangles[3].Contains(controller.GetMousePos()))
                {
                    sb.Draw(checkmark_hover, Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, settingsOtherRectangles[3].TopLeft) + new Vector2(1, 1), Color.White);
                    tooltipName = "Switches the game to windowed mode.";
                }

                if (Config.HIDE_CONTROLS)
                {
                    sb.Draw(checkmark, Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, settingsOtherRectangles[0].TopLeft) + new Vector2(1, 1), Color.White);
                }
                if (Config.HIDE_GRID)
                {
                    sb.Draw(checkmark, Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, settingsOtherRectangles[1].TopLeft) + new Vector2(1, 1), Color.White);
                }
                if (Config.HIDE_RETICLE)
                {
                    sb.Draw(checkmark, Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, settingsOtherRectangles[2].TopLeft) + new Vector2(1, 1), Color.White);
                }
                if (Config.WINDOWED)
                {
                    sb.Draw(checkmark, Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, settingsOtherRectangles[3].TopLeft) + new Vector2(1, 1), Color.White);
                }

                int sfxVolume = Config.SFX_VOLUME;
                for (int i = 1; i < settingsSFXRectangles.Length + 1; i++)
                {
                    if (sfxVolume >= i)
                    {
                        Texture2D tex = sound_segment;
                        Vector2 pos = settingsSFXRectangles[i - 1].TopLeft - new Vector2(2, 0);
                        if (sfxVolume == 1)
                        {
                            tex = sound_segment_end_farleft;
                            pos = pos + new Vector2(2, 0);
                        }
                        else if (i == 10)
                        {
                            tex = sound_segment_end_farright;
                        }
                        else if (i == 1)
                        {
                            tex = sound_segment_farleft;
                            pos = pos + new Vector2(2, 0);
                        }
                        else if (sfxVolume == i)
                        {
                            tex = sound_segment_end;
                        }
                        sb.Draw(tex, Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, pos), Color.White);
                    }
                }

                int musicVolume = Config.MUSIC_VOLUME;
                for (int i = 1; i < settingsMusicRectangles.Length + 1; i++)
                {
                    if (musicVolume >= i)
                    {
                        Texture2D tex = sound_segment;
                        Vector2 pos = settingsMusicRectangles[i - 1].TopLeft - new Vector2(2, 0);
                        if (musicVolume == 1)
                        {
                            tex = sound_segment_end_farleft;
                            pos = pos + new Vector2(2, 0);
                        }
                        else if (i == 10)
                        {
                            tex = sound_segment_end_farright;
                        }
                        else if (i == 1)
                        {
                            tex = sound_segment_farleft;
                            pos = pos + new Vector2(2, 0);
                        }
                        else if (musicVolume == i)
                        {
                            tex = sound_segment_end;
                        }
                        sb.Draw(tex, Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, pos), Color.White);
                    }
                }

                if (!PlateauMain.CanResolutionIncrease())
                {
                    sb.Draw(resolutionup_disabled, Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, settingsResolutionRectangles[0].TopLeft), Color.White);
                }
                if (!PlateauMain.CanResolutionDecrease())
                {
                    sb.Draw(resolutiondown_disabled, Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, settingsResolutionRectangles[1].TopLeft), Color.White);
                }
                
                if (settingsResolutionRectangles[0].Contains(controller.GetMousePos()) && PlateauMain.CanResolutionIncrease())
                {
                    sb.Draw(resolutionup_enlarge, Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, settingsResolutionRectangles[0].TopLeft) + new Vector2(-1, -1), Color.White);
                    tooltipName = "Increase the game resolution.";
                } else if (settingsResolutionRectangles[1].Contains(controller.GetMousePos()) && PlateauMain.CanResolutionDecrease())
                {
                    sb.Draw(resolutiondown_enlarge, Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, settingsResolutionRectangles[1].TopLeft) + new Vector2(-1, -1), Color.White);
                    tooltipName = "Decrease the game resolution.";
                }
                else if (settingsResolutionRectangles[0].Contains(controller.GetMousePos()) && !PlateauMain.CanResolutionIncrease())
                {
                    tooltipName = "The resolution cannot be increased more for your screen size.";
                }
                else if (settingsResolutionRectangles[1].Contains(controller.GetMousePos()) && !PlateauMain.CanResolutionDecrease())
                {
                    tooltipName = "The resolution cannot be decreased more.";
                }


                Vector2 resolutionTextLen = PlateauMain.FONT.MeasureString(PlateauMain.CURRENT_RESOLUTION.ToString()) * PlateauMain.FONT_SCALE;
                Vector2 resolutionTextPos = Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, SETTINGS_RESOLUTION_TEXT_POSITION - new Vector2(resolutionTextLen.X/2, resolutionTextLen.Y));
                QUEUED_STRINGS.Add(new QueuedString(PlateauMain.CURRENT_RESOLUTION.ToString(), resolutionTextPos, Color.Black));

                Vector2 leftKeybindTextLen = PlateauMain.FONT.MeasureString(Util.KeyToString(KeyBinds.LEFT)) * PlateauMain.FONT_SCALE;
                Vector2 leftKeybindTextPos = Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, SETTINGS_KEYBIND_LEFT_POSITION.Center - new Vector2(leftKeybindTextLen.X / 2 - 0.5f, leftKeybindTextLen.Y / 2 - 0.25f));
                QUEUED_STRINGS.Add(new QueuedString(Util.KeyToString(KeyBinds.LEFT), leftKeybindTextPos, Color.Black));
                if (currentRebind == Rebinds.LEFT)
                    sb.Draw(hotkey_lrud_active, Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, SETTINGS_KEYBIND_LEFT_POSITION.TopLeft) - new Vector2(0, 1), Color.White);
                else if(SETTINGS_KEYBIND_LEFT_POSITION.Contains(controller.GetMousePos()))
                    sb.Draw(hotkey_lrud_hover, Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, SETTINGS_KEYBIND_LEFT_POSITION.TopLeft) - new Vector2(0, 1), Color.White);
                if(KeyBinds.HasOverlap(KeyBinds.LEFT))
                    sb.Draw(hotkey_lrud_overlap, Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, SETTINGS_KEYBIND_LEFT_POSITION.TopLeft) - new Vector2(0, 1), Color.White);

                Vector2 rightKeybindTextLen = PlateauMain.FONT.MeasureString(Util.KeyToString(KeyBinds.RIGHT)) * PlateauMain.FONT_SCALE;
                Vector2 rightKeybindTextPos = Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, SETTINGS_KEYBIND_RIGHT_POSITION.Center - new Vector2(rightKeybindTextLen.X / 2 - 0.5f, rightKeybindTextLen.Y / 2 - 0.25f));
                QUEUED_STRINGS.Add(new QueuedString(Util.KeyToString(KeyBinds.RIGHT), rightKeybindTextPos, Color.Black));
                if (currentRebind == Rebinds.RIGHT)
                    sb.Draw(hotkey_lrud_active, Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, SETTINGS_KEYBIND_RIGHT_POSITION.TopLeft) - new Vector2(0, 1), Color.White);
                else if (SETTINGS_KEYBIND_RIGHT_POSITION.Contains(controller.GetMousePos()))
                    sb.Draw(hotkey_lrud_hover, Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, SETTINGS_KEYBIND_RIGHT_POSITION.TopLeft) - new Vector2(0, 1), Color.White);
                if (KeyBinds.HasOverlap(KeyBinds.RIGHT))
                    sb.Draw(hotkey_lrud_overlap, Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, SETTINGS_KEYBIND_RIGHT_POSITION.TopLeft) - new Vector2(0, 1), Color.White);

                Vector2 upKeybindTextLen = PlateauMain.FONT.MeasureString(Util.KeyToString(KeyBinds.UP)) * PlateauMain.FONT_SCALE;
                Vector2 upKeybindTextPos = Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, SETTINGS_KEYBIND_UP_POSITION.Center - new Vector2(upKeybindTextLen.X / 2 - 0.5f, upKeybindTextLen.Y / 2 - 0.25f));
                QUEUED_STRINGS.Add(new QueuedString(Util.KeyToString(KeyBinds.UP), upKeybindTextPos, Color.Black));
                if (currentRebind == Rebinds.UP)
                    sb.Draw(hotkey_lrud_active, Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, SETTINGS_KEYBIND_UP_POSITION.TopLeft) - new Vector2(0, 1), Color.White);
                else if (SETTINGS_KEYBIND_UP_POSITION.Contains(controller.GetMousePos()))
                    sb.Draw(hotkey_lrud_hover, Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, SETTINGS_KEYBIND_UP_POSITION.TopLeft) - new Vector2(0, 1), Color.White);
                if (KeyBinds.HasOverlap(KeyBinds.UP))
                    sb.Draw(hotkey_lrud_overlap, Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, SETTINGS_KEYBIND_UP_POSITION.TopLeft) - new Vector2(0, 1), Color.White);

                Vector2 downKeybindTextLen = PlateauMain.FONT.MeasureString(Util.KeyToString(KeyBinds.DOWN)) * PlateauMain.FONT_SCALE;
                Vector2 downKeybindTextPos = Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, SETTINGS_KEYBIND_DOWN_POSITION.Center - new Vector2(downKeybindTextLen.X / 2 - 0.5f, downKeybindTextLen.Y / 2 - 0.25f));
                QUEUED_STRINGS.Add(new QueuedString(Util.KeyToString(KeyBinds.DOWN), downKeybindTextPos, Color.Black));
                if (currentRebind == Rebinds.DOWN)
                    sb.Draw(hotkey_lrud_active, Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, SETTINGS_KEYBIND_DOWN_POSITION.TopLeft) - new Vector2(0, 1), Color.White);
                else if (SETTINGS_KEYBIND_DOWN_POSITION.Contains(controller.GetMousePos()))
                    sb.Draw(hotkey_lrud_hover, Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, SETTINGS_KEYBIND_DOWN_POSITION.TopLeft) - new Vector2(0, 1), Color.White);
                if (KeyBinds.HasOverlap(KeyBinds.DOWN))
                    sb.Draw(hotkey_lrud_overlap, Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, SETTINGS_KEYBIND_DOWN_POSITION.TopLeft) - new Vector2(0, 1), Color.White);

                Vector2 inventoryKeybindTextLen = PlateauMain.FONT.MeasureString(Util.KeyToString(KeyBinds.INVENTORY)) * PlateauMain.FONT_SCALE;
                Vector2 inventoryKeybindTextPos = Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, SETTINGS_KEYBIND_INVENTORY_POSITION.Center - new Vector2(inventoryKeybindTextLen.X / 2 - 0.5f, inventoryKeybindTextLen.Y / 2 - 0.25f));
                QUEUED_STRINGS.Add(new QueuedString(Util.KeyToString(KeyBinds.INVENTORY), inventoryKeybindTextPos, Color.Black));
                if (currentRebind == Rebinds.INVENTORY)
                    sb.Draw(hotkey_active, Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, SETTINGS_KEYBIND_INVENTORY_POSITION.TopLeft) - new Vector2(0, 1), Color.White);
                else if (SETTINGS_KEYBIND_INVENTORY_POSITION.Contains(controller.GetMousePos()))
                    sb.Draw(hotkey_hover, Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, SETTINGS_KEYBIND_INVENTORY_POSITION.TopLeft) - new Vector2(0, 1), Color.White);
                if (KeyBinds.HasOverlap(KeyBinds.INVENTORY))
                    sb.Draw(hotkey_overlap, Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, SETTINGS_KEYBIND_INVENTORY_POSITION.TopLeft) - new Vector2(0, 1), Color.White);

                Vector2 scrapbookKeybindTextLen = PlateauMain.FONT.MeasureString(Util.KeyToString(KeyBinds.SCRAPBOOK)) * PlateauMain.FONT_SCALE;
                Vector2 scrapbookKeybindTextPos = Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, SETTINGS_KEYBIND_SCRAPBOOK_POSITION.Center - new Vector2(scrapbookKeybindTextLen.X / 2 - 0.5f, scrapbookKeybindTextLen.Y / 2 - 0.25f));
                QUEUED_STRINGS.Add(new QueuedString(Util.KeyToString(KeyBinds.SCRAPBOOK), scrapbookKeybindTextPos, Color.Black));
                if (currentRebind == Rebinds.SCRAPBOOK)
                    sb.Draw(hotkey_active, Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, SETTINGS_KEYBIND_SCRAPBOOK_POSITION.TopLeft) - new Vector2(0, 1), Color.White);
                else if (SETTINGS_KEYBIND_SCRAPBOOK_POSITION.Contains(controller.GetMousePos()))
                    sb.Draw(hotkey_hover, Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, SETTINGS_KEYBIND_SCRAPBOOK_POSITION.TopLeft) - new Vector2(0, 1), Color.White);
                if (KeyBinds.HasOverlap(KeyBinds.SCRAPBOOK))
                    sb.Draw(hotkey_overlap, Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, SETTINGS_KEYBIND_SCRAPBOOK_POSITION.TopLeft) - new Vector2(0, 1), Color.White);

                Vector2 craftingKeybindTextLen = PlateauMain.FONT.MeasureString(Util.KeyToString(KeyBinds.CRAFTING)) * PlateauMain.FONT_SCALE;
                Vector2 craftingKeybindTextPos = Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, SETTINGS_KEYBIND_CRAFTING_POSITION.Center - new Vector2(craftingKeybindTextLen.X / 2 - 0.5f, craftingKeybindTextLen.Y / 2 - 0.25f));
                QUEUED_STRINGS.Add(new QueuedString(Util.KeyToString(KeyBinds.CRAFTING), craftingKeybindTextPos, Color.Black));
                if (currentRebind == Rebinds.CRAFTING)
                    sb.Draw(hotkey_active, Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, SETTINGS_KEYBIND_CRAFTING_POSITION.TopLeft) - new Vector2(0, 1), Color.White);
                else if (SETTINGS_KEYBIND_CRAFTING_POSITION.Contains(controller.GetMousePos()))
                    sb.Draw(hotkey_hover, Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, SETTINGS_KEYBIND_CRAFTING_POSITION.TopLeft) - new Vector2(0, 1), Color.White);
                if (KeyBinds.HasOverlap(KeyBinds.CRAFTING))
                    sb.Draw(hotkey_overlap, Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, SETTINGS_KEYBIND_CRAFTING_POSITION.TopLeft) - new Vector2(0, 1), Color.White);

                Vector2 settingsKeybindTextLen = PlateauMain.FONT.MeasureString(Util.KeyToString(KeyBinds.SETTINGS)) * PlateauMain.FONT_SCALE;
                Vector2 settingsKeybindTextPos = Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, SETTINGS_KEYBIND_SETTINGS_POSITION.Center - new Vector2(settingsKeybindTextLen.X / 2 - 0.5f, settingsKeybindTextLen.Y / 2 - 0.25f));
                QUEUED_STRINGS.Add(new QueuedString(Util.KeyToString(KeyBinds.SETTINGS), settingsKeybindTextPos, Color.Black));
                if (currentRebind == Rebinds.SETTINGS)
                    sb.Draw(hotkey_active, Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, SETTINGS_KEYBIND_SETTINGS_POSITION.TopLeft) - new Vector2(0, 1), Color.White);
                else if (SETTINGS_KEYBIND_SETTINGS_POSITION.Contains(controller.GetMousePos()))
                    sb.Draw(hotkey_hover, Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, SETTINGS_KEYBIND_SETTINGS_POSITION.TopLeft) - new Vector2(0, 1), Color.White);
                if (KeyBinds.HasOverlap(KeyBinds.SETTINGS))
                    sb.Draw(hotkey_overlap, Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, SETTINGS_KEYBIND_SETTINGS_POSITION.TopLeft) - new Vector2(0, 1), Color.White);

                Vector2 editmodeKeybindTextLen = PlateauMain.FONT.MeasureString(Util.KeyToString(KeyBinds.EDITMODE)) * PlateauMain.FONT_SCALE;
                Vector2 editmodeKeybindTextPos = Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, SETTINGS_KEYBIND_EDITMODE_POSITION.Center - new Vector2(editmodeKeybindTextLen.X / 2 - 0.5f, editmodeKeybindTextLen.Y / 2 - 0.25f));
                QUEUED_STRINGS.Add(new QueuedString(Util.KeyToString(KeyBinds.EDITMODE), editmodeKeybindTextPos, Color.Black));
                if (currentRebind == Rebinds.EDITMODE)
                    sb.Draw(hotkey_active, Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, SETTINGS_KEYBIND_EDITMODE_POSITION.TopLeft) - new Vector2(0, 1), Color.White);
                else if (SETTINGS_KEYBIND_EDITMODE_POSITION.Contains(controller.GetMousePos()))
                    sb.Draw(hotkey_hover, Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, SETTINGS_KEYBIND_EDITMODE_POSITION.TopLeft) - new Vector2(0, 1), Color.White);
                if (KeyBinds.HasOverlap(KeyBinds.EDITMODE))
                    sb.Draw(hotkey_overlap, Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, SETTINGS_KEYBIND_EDITMODE_POSITION.TopLeft) - new Vector2(0, 1), Color.White);

                Vector2 cycleKeybindTextLen = PlateauMain.FONT.MeasureString(Util.KeyToString(KeyBinds.CYCLE_HOTBAR)) * PlateauMain.FONT_SCALE;
                Vector2 cycleKeybindTextPos = Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, SETTINGS_KEYBIND_CYCLE_HOTBAR_POSITION.Center - new Vector2(cycleKeybindTextLen.X / 2 - 0.5f, cycleKeybindTextLen.Y / 2 - 0.25f));
                QUEUED_STRINGS.Add(new QueuedString(Util.KeyToString(KeyBinds.CYCLE_HOTBAR), cycleKeybindTextPos, Color.Black));
                if (currentRebind == Rebinds.CYCLE_HOTBAR)
                    sb.Draw(hotkey_active, Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, SETTINGS_KEYBIND_CYCLE_HOTBAR_POSITION.TopLeft) - new Vector2(0, 1), Color.White);
                else if (SETTINGS_KEYBIND_CYCLE_HOTBAR_POSITION.Contains(controller.GetMousePos()))
                    sb.Draw(hotkey_hover, Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, SETTINGS_KEYBIND_CYCLE_HOTBAR_POSITION.TopLeft) - new Vector2(0, 1), Color.White);
                if (KeyBinds.HasOverlap(KeyBinds.CYCLE_HOTBAR))
                    sb.Draw(hotkey_overlap, Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, SETTINGS_KEYBIND_CYCLE_HOTBAR_POSITION.TopLeft) - new Vector2(0, 1), Color.White);

                Vector2 discardKeybindTextLen = PlateauMain.FONT.MeasureString(Util.KeyToString(KeyBinds.DISCARD_ITEM)) * PlateauMain.FONT_SCALE;
                Vector2 discardKeybindTextPos = Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, SETTINGS_KEYBIND_DISCARD_ITEM_POSITION.Center - new Vector2(discardKeybindTextLen.X / 2 - 0.5f, discardKeybindTextLen.Y / 2 - 0.25f));
                QUEUED_STRINGS.Add(new QueuedString(Util.KeyToString(KeyBinds.DISCARD_ITEM), discardKeybindTextPos, Color.Black));
                if (currentRebind == Rebinds.DISCARD_ITEM)
                    sb.Draw(hotkey_active, Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, SETTINGS_KEYBIND_DISCARD_ITEM_POSITION.TopLeft) - new Vector2(0, 1), Color.White);
                else if (SETTINGS_KEYBIND_DISCARD_ITEM_POSITION.Contains(controller.GetMousePos()))
                    sb.Draw(hotkey_hover, Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, SETTINGS_KEYBIND_DISCARD_ITEM_POSITION.TopLeft) - new Vector2(0, 1), Color.White);
                if (KeyBinds.HasOverlap(KeyBinds.DISCARD_ITEM))
                    sb.Draw(hotkey_overlap, Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, SETTINGS_KEYBIND_DISCARD_ITEM_POSITION.TopLeft) - new Vector2(0, 1), Color.White);
            }
            else if (interfaceState == InterfaceState.CRAFTING)
            {
                sb.Draw(black_background, Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, BACKGROUND_BLACK_OFFSET), Color.White * BLACK_BACKGROUND_OPACITY);
                string pageName = "";
                sb.Draw(workbench, Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, WORKBENCH_POSITION), Color.White);

                switch (workbenchHoverTab)
                {
                    case -1:
                        break;
                    case 0:
                        sb.Draw(workbenchMachineTabHover, Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, WORKBENCH_MACHINE_TAB_POSITION), Color.White);
                        break;
                    case 1:
                        sb.Draw(workbenchScaffoldingTabHover, Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, WORKBENCH_SCAFFOLDING_TAB_POSITION), Color.White);
                        break;
                    case 2:
                        sb.Draw(workbenchFurnitureTabHover, Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, WORKBENCH_FURNITURE_TAB_POSITION), Color.White);
                        break;
                    case 3:
                        sb.Draw(workbenchFloorWallTabHover, Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, WORKBENCH_HOUSE_TAB_POSITION), Color.White);
                        break;
                    case 4:
                        sb.Draw(workbenchClothingTabHover, Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, WORKBENCH_CLOTHING_TAB_POSITION), Color.White);
                        break;
                    default:
                        throw new Exception();
                }

                switch (workbenchCurrentTab)
                {
                    case 0:
                        sb.Draw(workbenchMachineTab, Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, WORKBENCH_MACHINE_TAB_POSITION), Color.White);
                        pageName = "Machines " + (workbenchCurrentPage + 1);
                        break;
                    case 1:
                        sb.Draw(workbenchScaffoldingTab, Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, WORKBENCH_SCAFFOLDING_TAB_POSITION), Color.White);
                        pageName = "Scaffolding " + (workbenchCurrentPage + 1);
                        break;
                    case 2:
                        sb.Draw(workbenchFurnitureTab, Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, WORKBENCH_FURNITURE_TAB_POSITION), Color.White);
                        pageName = "Furniture " + (workbenchCurrentPage + 1);
                        break;
                    case 3:
                        sb.Draw(workbenchFloorWallTab, Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, WORKBENCH_HOUSE_TAB_POSITION), Color.White);
                        pageName = "Wallpaper & Flooring " + (workbenchCurrentPage + 1);
                        break;
                    case 4:
                        sb.Draw(workbenchClothingTab, Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, WORKBENCH_CLOTHING_TAB_POSITION), Color.White);
                        pageName = "Clothing " + (workbenchCurrentPage + 1);
                        break;
                }

                if (hoveringLeftArrow)
                {
                    sb.Draw(workbenchArrowLeft, Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, WORKBENCH_LEFT_ARROW_POSITION) - new Vector2(1, 1), Color.White);
                }
                else if (hoveringRightArrow)
                {
                    sb.Draw(workbenchArrowRight, Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, WORKBENCH_RIGHT_ARROW_POSITION) - new Vector2(1, 1), Color.White);
                }

                bool currentWorkbenchRecipePossible = true;
                if (selectedRecipe == null)
                {
                    currentWorkbenchRecipePossible = false;
                }
                else
                {
                    for (int i = 0; i < selectedRecipe.components.Length; i++)
                    {
                        if (numMaterialsOfRecipe[i] < selectedRecipe.components[i].GetQuantity())
                        {
                            currentWorkbenchRecipePossible = false;
                        }
                    }
                }

                if (hoveringCraftButton && currentWorkbenchRecipePossible)
                {
                    sb.Draw(workbenchCraftButtonEnlarged, Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, WORKBENCH_CRAFT_BUTTON) - new Vector2(1, 1), Color.White);
                }
                else if (currentWorkbenchRecipePossible)
                {
                    sb.Draw(workbenchCraftButton, Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, WORKBENCH_CRAFT_BUTTON), Color.White);
                }

                foreach (Vector2 possible in workbenchCraftablePosition)
                {
                    sb.Draw(inventory_selected, Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, possible), Color.LightGreen);
                }

                sb.Draw(inventory_selected, Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, workbenchInventorySelectedPosition), Color.Blue);

                for (int i = 0; i < workbenchBlueprintRectangles.Length; i++)
                {
                    if (currentRecipes[i] != null)
                    {
                        Vector2 recipePosition = Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, workbenchBlueprintRectangles[i].TopLeft + new Vector2(1, 1));
                        if (selectedRecipeSlot == i)
                        {
                            sb.Draw(workbenchBlueprintDepression, recipePosition, Color.White);
                        }

                        currentRecipes[i].result.GetItem().Draw(sb,
                            recipePosition,
                            currentRecipes[i].haveBlueprint ? Color.White : Color.Black, layerDepth);

                        if (!currentRecipes[i].haveBlueprint)
                        {
                            sb.Draw(workbenchQuestionMark,
                                recipePosition, Color.White);
                        }
                        else
                        {
                            if (currentRecipes[i].result.GetQuantity() != 1)
                            {
                                Vector2 itemQuantityPosition = new Vector2(recipePosition.X + 11, recipePosition.Y + 9);
                                sb.Draw(numbers[currentRecipes[i].result.GetQuantity() % 10], itemQuantityPosition, Color.White);
                                if (currentRecipes[i].result.GetQuantity() >= 10)
                                {
                                    itemQuantityPosition.X -= 4;
                                    sb.Draw(numbers[currentRecipes[i].result.GetQuantity() / 10], itemQuantityPosition, Color.White);
                                }
                            }
                        }

                    }
                }

                if (selectedRecipe != null)
                {
                    GameState.CraftingRecipe selected = selectedRecipe;
                    Vector2 selectedRecipePos = Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, WORKBENCH_SELECTED_RECIPE_POSITION);
                    selected.result.GetItem().Draw(sb, selectedRecipePos, Color.White, layerDepth);
                    if (selected.result.GetQuantity() != 1)
                    {
                        Vector2 itemQuantityPosition = new Vector2(selectedRecipePos.X + 11, selectedRecipePos.Y + 9);
                        sb.Draw(numbers[selected.result.GetQuantity() % 10], itemQuantityPosition, Color.White);
                        if (selected.result.GetQuantity() >= 10)
                        {
                            itemQuantityPosition.X -= 4;
                            sb.Draw(numbers[selected.result.GetQuantity() / 10], itemQuantityPosition, Color.White);
                        }
                    }
                    for (int i = 0; i < selected.components.Length; i++)
                    {
                        Vector2 pos = new Vector2(-100, -100);
                        switch (i)
                        {
                            case 0:
                                pos = Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, WORKBENCH_SELECTED_RECIPE_COMPONENT_1);
                                break;
                            case 1:
                                pos = Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, WORKBENCH_SELECTED_RECIPE_COMPONENT_2);
                                break;
                            case 2:
                                pos = Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, WORKBENCH_SELECTED_RECIPE_COMPONENT_3);
                                break;
                            case 3:
                                pos = Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, WORKBENCH_SELECTED_RECIPE_COMPONENT_4);
                                break;
                        }
                        //draw the "need" part
                        selected.components[i].GetItem().Draw(sb, pos, Color.White, layerDepth);
                        Vector2 itemQuantityPosition = new Vector2(pos.X + 11, pos.Y + 9);
                        sb.Draw(numbers[selected.components[i].GetQuantity() % 10], itemQuantityPosition, Color.White);
                        if (selected.components[i].GetQuantity() >= 10)
                        {
                            itemQuantityPosition.X -= 4;
                            sb.Draw(numbers[selected.components[i].GetQuantity() / 10], itemQuantityPosition, Color.White);
                        }

                        //draw the "have" part
                        pos.Y += haveBoxesDeltaY;
                        selected.components[i].GetItem().Draw(sb, pos, Color.White, layerDepth);
                        itemQuantityPosition = new Vector2(pos.X + 11, pos.Y + 9);
                        int quantity = numMaterialsOfRecipe[i];
                        bool haveEnough = quantity >= selectedRecipe.components[i].GetQuantity();
                        do
                        {
                            sb.Draw(numbers[quantity % 10], itemQuantityPosition, haveEnough ? Color.Green : Color.Red);
                            quantity /= 10;
                            itemQuantityPosition.X -= 4;
                        } while (quantity != 0);
                    }
                }
                Vector2 pageNameLen = PlateauMain.FONT.MeasureString(pageName) * PlateauMain.FONT_SCALE;
                Vector2 pageNamePos = Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, WORKBENCH_PAGE_NAME_POSITION - new Vector2(pageNameLen.X / 2, pageNameLen.Y));
                QUEUED_STRINGS.Add(new QueuedString(pageName, pageNamePos, Color.White));
            }

            //draw controls
            if (!Config.HIDE_CONTROLS)
            {
                if (!isHidden || currentDialogue != null)
                {
                    sb.Draw(mouseControl, Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, MOUSE_CONTROL_POSITION), Color.White);
                    if (isMouseRightDown)
                    {
                        sb.Draw(mouseRightDown, Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, MOUSE_CONTROL_POSITION), Color.White);
                    }
                    if (isMouseLeftDown)
                    {
                        sb.Draw(mouseLeftDown, Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, MOUSE_CONTROL_POSITION), Color.White);
                    }
                    Vector2 leftMouseSize = PlateauMain.FONT.MeasureString(mouseLeftAction) * PlateauMain.FONT_SCALE;
                    Vector2 leftShiftMouseSize = PlateauMain.FONT.MeasureString(mouseLeftShiftAction) * PlateauMain.FONT_SCALE;
                    Vector2 rightMouseSize = PlateauMain.FONT.MeasureString(mouseRightAction) * PlateauMain.FONT_SCALE;
                    Vector2 rightShiftMouseSize = PlateauMain.FONT.MeasureString(mouseRightShiftAction) * PlateauMain.FONT_SCALE;
                    if (controller.IsShiftDown())
                    {
                        WHITE_9SLICE.DrawString(sb, mouseLeftShiftAction, Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, MOUSE_LEFT_TEXT_POSITION) - (0.5f * leftShiftMouseSize), cameraBoundingBox, Color.LightGreen, Util.UI_BLACK_9SLICE.color);
                        WHITE_9SLICE.DrawString(sb, mouseRightShiftAction, Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, MOUSE_RIGHT_TEXT_POSITION) - (0.5f * rightShiftMouseSize), cameraBoundingBox, Color.LightGreen, Util.UI_BLACK_9SLICE.color);
                    }
                    else
                    {
                        WHITE_9SLICE.DrawString(sb, mouseLeftAction, Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, MOUSE_LEFT_TEXT_POSITION) - (0.5f * leftMouseSize), cameraBoundingBox, Color.White, Util.UI_BLACK_9SLICE.color);
                        WHITE_9SLICE.DrawString(sb, mouseRightAction, Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, MOUSE_RIGHT_TEXT_POSITION) - (0.5f * rightMouseSize), cameraBoundingBox, Color.White, Util.UI_BLACK_9SLICE.color);
                    }

                    if (mouseLeftShiftAction != "" || mouseRightShiftAction != "")
                    {
                        sb.Draw(controller.IsShiftDown() ? shiftOnPressed : shiftOnUnpressed, Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, SHIFT_CONTROL_POSITION), Color.White);
                        if (currentDialogue == null)
                        {
                            string shiftTooltip = controller.IsShiftDown() ? "" : "Shift for\nmore options";
                            if (shiftTooltip != "")
                            {
                                WHITE_9SLICE.DrawString(sb, shiftTooltip, Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, SHIFT_TEXT_POSITION), cameraBoundingBox, Color.LightGreen, Util.UI_BLACK_9SLICE.color);
                            }
                        }
                    }
                    else if ((!isHidden && currentDialogue == null))
                    {
                        sb.Draw(controller.IsShiftDown() ? shiftOffPressed : shiftOffUnpressed, Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, SHIFT_CONTROL_POSITION), Color.White);
                    }

                    if (!isHidden && currentDialogue == null)
                    {
                        sb.Draw(controller.IsKeyDown(KeyBinds.ESCAPE) ? escPressed : escUnpressed, Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, ESC_CONTROL_POSITION), Color.White);
                        WHITE_9SLICE.DrawString(sb, interfaceState == InterfaceState.NONE ? "Exit" : "Back", Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, ESC_TEXT_POSITION), cameraBoundingBox, Color.White, Util.UI_BLACK_9SLICE.color);
                    }
                }
            }


            Vector2 leftActionStrSize = PlateauMain.FONT.MeasureString(leftAction) * PlateauMain.FONT_SCALE;
            Vector2 rightActionStrSize = PlateauMain.FONT.MeasureString(rightAction) * PlateauMain.FONT_SCALE;
            Vector2 upActionStrSize = PlateauMain.FONT.MeasureString(upAction) * PlateauMain.FONT_SCALE;
            Vector2 downActionStrSize = PlateauMain.FONT.MeasureString(downAction) * PlateauMain.FONT_SCALE;
            if (currentDialogue == null)
            {
                if (!isHidden)
                {
                    if (!Config.HIDE_CONTROLS)
                    {
                        sb.Draw(keyControl, Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, KEY_CONTROL_POSITION), Color.White);
                        if (isSDown)
                        {
                            sb.Draw(keyControlSDown, Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, KEY_CONTROL_POSITION), Color.White);
                        }
                        if (isADown)
                        {
                            sb.Draw(keyControlADown, Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, KEY_CONTROL_POSITION), Color.White);
                        }
                        if (isWDown)
                        {
                            sb.Draw(keyControlWDown, Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, KEY_CONTROL_POSITION), Color.White);
                        }
                        if (isDDown)
                        {
                            sb.Draw(keyControlDDown, Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, KEY_CONTROL_POSITION), Color.White);
                        }
                        WHITE_9SLICE.DrawString(sb, leftAction, Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, KEY_LEFT_TEXT_POSITION) - (0.5f * leftActionStrSize), cameraBoundingBox, Color.White, Util.UI_BLACK_9SLICE.color);
                        WHITE_9SLICE.DrawString(sb, rightAction, Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, KEY_RIGHT_TEXT_POSITION) - (0.5f * rightActionStrSize), cameraBoundingBox, Color.White, Util.UI_BLACK_9SLICE.color);
                        WHITE_9SLICE.DrawString(sb, upAction, Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, KEY_UP_TEXT_POSITION) - (0.5f * upActionStrSize), cameraBoundingBox, Color.White, Util.UI_BLACK_9SLICE.color);
                        WHITE_9SLICE.DrawString(sb, downAction, Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, KEY_DOWN_TEXT_POSITION) - (0.5f * downActionStrSize), cameraBoundingBox, Color.White, Util.UI_BLACK_9SLICE.color);
                    }
                }
            }
            else if (currentDialogue.Splits() && dialogueNodePage + 1 == currentDialogue.NumPages() && currentDialogueNumChars >= currentDialogue.dialogueTexts[dialogueNodePage].Length)
            {
                Vector2 leftPosition = Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, KEY_LEFT_TEXT_POSITION_DIALOGUE) - (0.5f * leftActionStrSize);
                Vector2 rightPosition = Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, KEY_RIGHT_TEXT_POSITION_DIALOGUE) - (0.5f * rightActionStrSize);
                Vector2 upPosition = Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, KEY_UP_TEXT_POSITION_DIALOGUE) - (0.5f * upActionStrSize);
                Vector2 downPosition = Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, KEY_DOWN_TEXT_POSITION_DIALOGUE) - (0.5f * downActionStrSize);
                sb.Draw(keyControl, Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, KEY_CONTROL_POSITION_DIALOGUE), Color.White);
                WHITE_9SLICE.DrawString(sb, leftAction, leftPosition, cameraBoundingBox, Color.White, Color.Black);
                WHITE_9SLICE.DrawString(sb, rightAction, rightPosition, cameraBoundingBox, Color.White, Color.Black);
                WHITE_9SLICE.DrawString(sb, upAction, upPosition, cameraBoundingBox, Color.White, Color.Black);
                WHITE_9SLICE.DrawString(sb, downAction, downPosition, cameraBoundingBox, Color.White, Color.Black);
            }

            //draw all the general ui stuff
            if (!isHidden)
            {
                //draw name of hotbar item
                if (interfaceState != InterfaceState.INVENTORY && 
                    interfaceState != InterfaceState.SCRAPBOOK && 
                    interfaceState != InterfaceState.CHEST &&
                    interfaceState != InterfaceState.SCRAPBOOK &&
                    interfaceState != InterfaceState.SETTINGS &&
                    interfaceState != InterfaceState.EXIT)
                {
                    if (!selectedHotbarItemName.Equals(ItemDict.NONE.GetName()))
                    {
                        Vector2 nameLength = PlateauMain.FONT.MeasureString(selectedHotbarItemName) * PlateauMain.FONT_SCALE;
                        Vector2 shbinPos = Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, SELECTED_HOTBAR_ITEM_NAME_POSITION - new Vector2(nameLength.X / 2, nameLength.Y));
                        WHITE_9SLICE.DrawString(sb, selectedHotbarItemName, shbinPos, cameraBoundingBox, Color.White, Util.UI_BLACK_9SLICE.color);
                        //QUEUED_STRINGS.Add(new QueuedString(selectedHotbarItemName, shbinPos, Color.White));
                    }
                }


                //dont show areaname...
                /*if (interfaceState == InterfaceState.NONE)
                {
                    //draw the area/zone strings
                    string areaAndZone = areaName + " ";
                    if (!zoneName.Equals(""))
                    {
                        areaAndZone = areaAndZone + "- " + zoneName + " ";
                    }
                    INTERFACE_9SLICE.DrawString(sb, areaAndZone, Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, AREA_NAME_POSITION), cameraBoundingBox, Color.Black, Color.White);
                }*/
                //sb.DrawString(Game1.FONT, areaName, Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, AREA_NAME_POSITION), Color.LightGray, 0.0f, Vector2.Zero, Game1.FONT_SCALE, SpriteEffects.None, 0.0f);
                //sb.DrawString(Game1.FONT, zoneName, Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, ZONE_NAME_POSITION), Color.DarkGray, 0.0f, Vector2.Zero, Game1.FONT_SCALE, SpriteEffects.None, 0.0f);


                //draw the datetime panel and relevant text/numbers
                sb.Draw(dateTimePanel, Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, DATETIME_PANEL_POSITION), Color.White);
                sb.Draw(seasonText[seasonIndex], Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, Vector2.Add(DATETIME_PANEL_POSITION, DATETIME_PANEL_SEASONTEXT_OFFSET)), Color.White);
                sb.Draw(dayText[dayIndex], Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, Vector2.Add(DATETIME_PANEL_POSITION, DATETIME_PANEL_DAYTEXT_OFFSET)), Color.White);
                Vector2 timePos = Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, Vector2.Add(DATETIME_PANEL_POSITION, DATETIME_PANEL_TIME_OFFSET));
                sb.Draw(numbersNoBorder[hourTensIndex], timePos, Color.LightGray);
                timePos.X += 4;
                sb.Draw(numbersNoBorder[hourOnesIndex], timePos, Color.LightGray);
                timePos.X += 6;
                sb.Draw(numbersNoBorder[minuteTensIndex], timePos, Color.LightGray);
                timePos.X += 4;
                sb.Draw(numbersNoBorder[minuteOnesIndex], timePos, Color.LightGray);

                //draw the effect icons
                for (int i = 0; i < appliedEffects.Count; i++)
                {
                    float effectX = APPLIED_EFFECT_ANCHOR.X;
                    float effectY = APPLIED_EFFECT_ANCHOR.Y;
                    foreach (EntityPlayer.TimedEffect effect in appliedEffects)
                    {
                        RectangleF effectRect = new RectangleF(effectX, effectY, 12, 12);

                        if (effectRect.Contains(controller.GetMousePos()))
                        {
                            tooltipName = effect.effect.name;
                            string hoursLeft = ((int)effect.timeRemaining / 60).ToString();
                            if (hoursLeft.Length == 1)
                            {
                                hoursLeft = "0" + hoursLeft;
                            }
                            string minutesLeft = ((int)effect.timeRemaining % 60).ToString();
                            if (minutesLeft.Length == 1)
                            {
                                minutesLeft = "0" + minutesLeft;
                            }
                            tooltipDescription = effect.effect.description;
                            if (((int)effect.timeRemaining / 60) <= 24)
                            {
                                tooltipDescription += "\nActive for:  " + hoursLeft + ":" + minutesLeft;
                            }
                        }

                        effectX += APPLIED_EFFECT_DELTA_X;

                        effect.effect.DrawIcon(sb, Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, effectRect.TopLeft));
                    }
                }

                //draw gold amount
                Vector2 goldPos = Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, Vector2.Add(DATETIME_PANEL_POSITION, DATETIME_PANEL_GOLD_OFFSET));
                string numberStr = displayGold.ToString();
                while (numberStr.Length < 9)
                {
                    numberStr = "0" + numberStr;
                }
                char[] digits = numberStr.ToCharArray();
                for (int i = 8; i >= 0; i--)
                {
                    sb.Draw(numbersNoBorder[Int32.Parse(digits[i].ToString())], goldPos, Color.White);
                    goldPos.X -= 4;
                    if (i == 6 || i == 3)
                    {
                        goldPos.X--;
                    }
                }


                //draw hotbar
                sb.Draw(hotbar, Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, HOTBAR_POSITION), Color.White);
                Vector2 adjustedHotbarSelectedPos = new Vector2(HOTBAR_SELECTED_POSITION_0.X + HOTBAR_SELECTED_DELTA_X * selectedHotbarPosition, HOTBAR_SELECTED_POSITION_0.Y);
                sb.Draw(hotbar_selected, Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, adjustedHotbarSelectedPos), Color.White);
                sb.Draw(inventory_selected, Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, inventorySelectedPosition), Color.White);

                for (int i = 0; i < GameplayInterface.HOTBAR_LENGTH; i++)
                {
                    Vector2 position = Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, new Vector2(itemRectangles[i].X, itemRectangles[i].Y)) + new Vector2(1, 1);
                    Item item = inventoryItems[i].GetItem();
                    item.Draw(sb, position, Color.White, layerDepth);
                    if (item.GetStackCapacity() != 1 && inventoryItems[i].GetQuantity() != 0)
                    {
                        Vector2 itemQuantityPosition = new Vector2(itemRectangles[i].X + 12, itemRectangles[i].Y + 10);
                        sb.Draw(numbers[inventoryItems[i].GetQuantity() % 10], Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, itemQuantityPosition), Color.White);
                        if (inventoryItems[i].GetQuantity() >= 10)
                        {
                            itemQuantityPosition.X -= 4;
                            sb.Draw(numbers[inventoryItems[i].GetQuantity() / 10], Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, itemQuantityPosition), Color.White);
                        }
                    }
                }

                if (currentDialogue == null)
                {
                    sb.Draw(menuControl, Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, MENU_CONTROL_POSITION), Color.White);
                    if (isMouseOverCraftingMC)
                    {
                        sb.Draw(menuControlsCraftingEnlarge, Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, MENU_CONTROL_POSITION), Color.White);
                    }
                    else if (isMouseOverInventoryMC)
                    {
                        sb.Draw(menuControlsInventoryEnlarge, Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, MENU_CONTROL_POSITION), Color.White);
                    }
                    else if (isMouseOverScrapbookMC)
                    {
                        sb.Draw(menuControlsScrapbookEnlarge, Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, MENU_CONTROL_POSITION), Color.White);
                    }
                    else if (isMouseOverSettingsMC)
                    {
                        sb.Draw(menuControlsSettingsEnlarge, Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, MENU_CONTROL_POSITION), Color.White);
                    } else if (isMouseOverEditModeMC)
                    {
                        sb.Draw(menuControlsEditModeEnlarge, Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, MENU_CONTROL_POSITION), Color.White);
                    }

                    if (editMode)
                    {
                        sb.Draw(menuControlsEditModeDepressed, Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, MENU_CONTROL_POSITION), Color.White);
                    }

                    if (interfaceState == InterfaceState.CRAFTING)
                    {
                        sb.Draw(menuControlsCraftingDepressed, Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, MENU_CONTROL_POSITION), Color.White);
                    }
                    else if (interfaceState == InterfaceState.INVENTORY)
                    {
                        sb.Draw(menuControlsInventoryDepressed, Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, MENU_CONTROL_POSITION), Color.White);
                    }
                    else if (interfaceState == InterfaceState.SCRAPBOOK)
                    {
                        sb.Draw(menuControlsScrapbookDepressed, Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, MENU_CONTROL_POSITION), Color.White);
                    }
                    else if (interfaceState == InterfaceState.SETTINGS)
                    {
                        sb.Draw(menuControlsSettingsDepressed, Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, MENU_CONTROL_POSITION), Color.White);
                    } 
                }

                if (interfaceState == InterfaceState.INVENTORY)
                {
                    QUEUED_STRINGS.Add(new QueuedString("Inventory: " + Util.KeyToString(KeyBinds.INVENTORY), Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, MENU_BAG_HOTKEY_POSITION), Color.White));
                    QUEUED_STRINGS.Add(new QueuedString("Scrapbook: " + Util.KeyToString(KeyBinds.SCRAPBOOK), Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, MENU_SCRAPBOOK_HOTKEY_POSITION), Color.White));
                    QUEUED_STRINGS.Add(new QueuedString("Crafting: " + Util.KeyToString(KeyBinds.CRAFTING), Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, MENU_CRAFTING_HOTKEY_POSITION), Color.White));
                    QUEUED_STRINGS.Add(new QueuedString("Settings: " + Util.KeyToString(KeyBinds.SETTINGS), Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, MENU_SETTINGS_HOTKEY_POSITION), Color.White));
                    QUEUED_STRINGS.Add(new QueuedString("Editmode: " + Util.KeyToString(KeyBinds.EDITMODE), Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, MENU_EDITMODE_HOTKEY_POSITION), Color.White));
                    QUEUED_STRINGS.Add(new QueuedString("Cycle: " + Util.KeyToString(KeyBinds.CYCLE_HOTBAR), Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, MENU_CYCLE_INVENTORY_HOTKEY_POSITION), Color.White));
                    if(controller.IsShiftDown())
                        QUEUED_STRINGS.Add(new QueuedString("All: " + Util.KeyToString(KeyBinds.DISCARD_ITEM), Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, MENU_DROP_HOTKEY_POSITION), Color.White));
                    else
                        QUEUED_STRINGS.Add(new QueuedString("Drop: " + Util.KeyToString(KeyBinds.DISCARD_ITEM), Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, MENU_DROP_HOTKEY_POSITION), Color.White));
                }

                if(editMode && interfaceState == InterfaceState.NONE)
                {
                    string notification = "You are in Edit Mode.\nPress " + Util.KeyToString(KeyBinds.EDITMODE) + " to exit.";
                    Vector2 notificationSize = PlateauMain.FONT.MeasureString(notification) * PlateauMain.FONT_SCALE;
                    Vector2 notificationPos = Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, EDIT_MODE_NOTIFICATION_TEXT) - (0.5f * notificationSize);
                    WHITE_9SLICE.DrawString(sb, notification, notificationPos, cameraBoundingBox, Color.CornflowerBlue, Util.UI_BLACK_9SLICE.color);
                }

                if(interfaceState != InterfaceState.NONE || currentDialogue != null)
                {
                    player.ClearNotifications();
                    currentNotification = null;
                }

                if (currentNotification != null)
                    currentNotification.Draw(sb, Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, NOTIFICATION_POSITION), cameraBoundingBox);
            } 

            //draw the dialogue bubble
            if(inDialogue)
            { 
                dialogueBox.Draw(sb, Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, DIALOGUE_BOX_POSITION), Color.White, layerDepth);
                if (dialogueBox.IsCurrentLoopFinished() && dialogueBox.IsCurrentLoop("anim"))
                {
                    if (currentDialogue == null)
                    {
                        dialogueBox.SetLoopIfNot("close");
                    }
                    else
                    {
                        sb.Draw(currentDialogue.portrait, Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, DIALOGUE_PORTRAIT_POSITION), Color.White);
                        QUEUED_STRINGS.Add(new QueuedString(currentDialogue.GetText((int)currentDialogueNumChars, dialogueNodePage), Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, DIALOGUE_TEXT_POSITION), Color.Black));
                        if (currentDialogueNumChars >= currentDialogue.dialogueTexts[dialogueNodePage].Length)
                        {
                            bounceArrow.Draw(sb, Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, DIALOGUE_BOUNCE_ARROW_POSITION), Color.White, layerDepth);
                        }
                    }
                } else if (dialogueBox.IsCurrentLoopFinished() && dialogueBox.IsCurrentLoop("close"))
                {
                    inDialogue = false;
                }
            }

            //draw held item in hand in inventory, or draw item preview in editmode, or otherwise the mouse cursor
            if (inventoryHeldItem.GetItem() != ItemDict.NONE)
            {
                Vector2 mousePos = controller.GetMousePos();
                mousePos.X -= 8;
                mousePos.Y -= 8;
                inventoryHeldItem.GetItem().Draw(sb, Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, mousePos), Color.White, layerDepth);
                if (inventoryHeldItem.GetItem().GetStackCapacity() != 1 && inventoryHeldItem.GetQuantity() != 0)
                {
                    Vector2 itemQuantityPosition = new Vector2(mousePos.X + 11, mousePos.Y + 9);
                    sb.Draw(numbers[inventoryHeldItem.GetQuantity() % 10], Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, itemQuantityPosition), Color.White);
                    if (inventoryHeldItem.GetQuantity() >= 10)
                    {
                        itemQuantityPosition.X -= 4;
                        sb.Draw(numbers[inventoryHeldItem.GetQuantity() / 10], Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, itemQuantityPosition), Color.White);
                    }
                }
            } 
            else
            {
                if (isHoldingPlaceable)
                {
                    sb.Draw(placeableTexture, placeableLocation, isPlaceableLocationValid ? Color.White * PLACEMENT_OPACITY : Color.Red * (showPlaceableTexture ? 1.0f : 0.0f));
                }
                else
                {
                    if (!tooltipName.Equals(""))
                    {
                        //string tooltip = tooltipName + (!tooltipDescription.Equals("") ? ("\n" + tooltipDescription) : "");
                        //TOOLTIP_9SLICE.DrawString(sb, tooltip, Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, controller.GetMousePos() + (tooltipDescription.Equals("") ? new Vector2(-5, 5) : new Vector2(0, 0))) + TOOLTIP_OFFSET, cameraBoundingBox, Color.Black, Color.White, true);
                    }
                }
            }

            if(paused)
            {
                sb.Draw(black_background, Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, BACKGROUND_BLACK_OFFSET), Color.White * 0.8f);
            }

            if(interfaceState == InterfaceState.TRANSITION_TO_DOWN ||
                interfaceState == InterfaceState.TRANSITION_TO_LEFT ||
                interfaceState == InterfaceState.TRANSITION_TO_RIGHT ||
                interfaceState == InterfaceState.TRANSITION_TO_UP ||
                interfaceState == InterfaceState.TRANSITION_FADE_TO_BLACK ||
                interfaceState == InterfaceState.TRANSITION_FADE_IN)
            {
                sb.Draw(black_background, Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, transitionPosition + BACKGROUND_BLACK_OFFSET), Color.White * transitionAlpha);
            }
        }

        public bool IsPaused()
        {
            return paused;
        }

        public void Hide()
        {
            isHidden = true;
        }

        public void Unhide()
        {
            isHidden = false;
        }

        public void DrawStrings(SpriteBatch sb)
        {
            foreach(QueuedString toDraw in QUEUED_STRINGS)
            {
                sb.DrawString(PlateauMain.FONT, toDraw.text, toDraw.position, toDraw.color, 0.0f, Vector2.Zero, PlateauMain.FONT_SCALE, SpriteEffects.None, 0.0f);
            }

            QUEUED_STRINGS.Clear();
        }

        public void DrawTooltip(SpriteBatch sb, RectangleF cameraBoundingBox)
        {
            if(inventoryHeldItem.GetItem() == ItemDict.NONE)
            {
                if (!tooltipName.Equals(""))
                {
                    string tooltip = tooltipName + (!tooltipDescription.Equals("") ? ("\n" + tooltipDescription) : "");
                    TOOLTIP_9SLICE.DrawString(sb, Util.WrapString(tooltip, Util.TOOLTIP_WRAP_LENGTH), Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, controller.GetMousePos() + (tooltipDescription.Equals("") ? new Vector2(-5, 5) : new Vector2(0, 0))) + TOOLTIP_OFFSET, cameraBoundingBox, Color.Black, Color.White, true);
                }
            }

           /* if (inventoryHeldItem.GetItem() != ItemDict.NONE)
            {
                Vector2 mousePos = controller.GetMousePos();
                mousePos.X -= 8;
                mousePos.Y -= 8;
                inventoryHeldItem.GetItem().Draw(sb, Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, mousePos), Color.White, layerDepth);
                if (inventoryHeldItem.GetItem().GetStackCapacity() != 1 && inventoryHeldItem.GetQuantity() != 0)
                {
                    Vector2 itemQuantityPosition = new Vector2(mousePos.X + 11, mousePos.Y + 9);
                    sb.Draw(numbers[inventoryHeldItem.GetQuantity() % 10], Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, itemQuantityPosition), Color.White);
                    if (inventoryHeldItem.GetQuantity() >= 10)
                    {
                        itemQuantityPosition.X -= 4;
                        sb.Draw(numbers[inventoryHeldItem.GetQuantity() / 10], Util.ConvertFromAbsoluteToCameraVector(cameraBoundingBox, itemQuantityPosition), Color.White);
                    }
                }
            }
            else
            {
                if (isHoldingPlaceable)
                {
                    sb.Draw(placeableTexture, placeableLocation, isPlaceableLocationValid ? Color.White * PLACEMENT_OPACITY : Color.Red * (showPlaceableTexture ? 1.0f : 0.0f));
                }
                else
                {

                }
            }*/
        }

        public bool NoMenusOpen()
        {
            return interfaceState == InterfaceState.NONE;
        }

        //Drops ONE of inventory held item
        private void DropInventoryHeldItem(World world)
        {
            if (inventoryHeldItem.GetItem() != ItemDict.NONE)
            {
                Vector2 position = player.GetCenteredPosition() + new Vector2(0, 4);
                world.GetCurrentArea().AddEntity(new EntityItem(inventoryHeldItem.GetItem(), position, new Vector2((player.GetDirection() == DirectionEnum.LEFT ? -1 : 1) * Util.RandInt(55, 63) / 100.0f, -2.3f))); ;
                inventoryHeldItem.Subtract(1);
            }
        }

        //Drop ALL of inventory held item
        public void DropInventoryHeldItemAll(World world)
        {
            while (inventoryHeldItem.GetItem() != ItemDict.NONE)
                DropInventoryHeldItem(world);
        }

        private void DropHeldItemAll(World world)
        {
            while (heldItem.GetItem() != ItemDict.NONE)
                DropHeldItem(world);
        }
        private void DropHeldItem(World world)
        {
            if (heldItem.GetItem() != ItemDict.NONE)
            {
                Vector2 position = player.GetCenteredPosition() + new Vector2(0, 4);
                world.GetCurrentArea().AddEntity(new EntityItem(heldItem.GetItem(), position, new Vector2((player.GetDirection() == DirectionEnum.LEFT ? -1 : 1) * Util.RandInt(55, 63) / 100.0f, -2.3f))); ;
                heldItem.Subtract(1);
            }
        }

        public void SetTooltip(String tooltip, String description="")
        {
            tooltipName = tooltip;
            tooltipDescription = description;
        }
    }

}
