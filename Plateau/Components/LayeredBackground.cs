using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using Plateau.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plateau.Components
{
    public class LayeredBackground
    {
        public class BackgroundParams
        {
            public enum Type
            {
                BACKGROUND_SKY, BACKGROUND_SPACE, FOREGROUND_SKY, FOREGROUND_SPACE
            }

            public bool hasParticles;
            public Type type;
            public bool hasClouds;

            public BackgroundParams(Type type, bool hasParticles, bool hasClouds)
            {
                this.type = type;
                this.hasParticles = hasParticles;
                this.hasClouds = hasClouds;
            }
        }

        public class Element
        {
            public Texture2D texture;
            public Vector2 position, speed;
            protected bool isFixedPosition;
            protected bool stretchToFit;

            public Element(Texture2D texture, Vector2 position, Vector2 speed, bool stretchToFit = false)
            {
                this.texture = texture;
                this.speed = speed;
                this.position = position;
                this.isFixedPosition = true;
                this.stretchToFit = stretchToFit;
            }

            public Element(Texture2D texture, RectangleF inflatedBounds, Vector2 speed)
            {
                this.texture = texture;
                this.position = new Vector2(0, 0);
                RandomizePosition(inflatedBounds);
                this.speed = speed;
                this.isFixedPosition = false;
            }

            public void RandomizePosition(RectangleF bounds)
            {
                if (!isFixedPosition)
                {
                    position.X = Util.RandInt((int)(bounds.Left - texture.Width), (int)(bounds.Right + texture.Width));
                    position.Y = Util.RandInt((int)(bounds.Top - texture.Height), (int)(bounds.Bottom + texture.Height));
                }
            }

            public virtual void Update(float deltaTime, RectangleF boundingRect, RectangleF cameraBoundingBox, Vector2 cameraMovementSinceLastFrame, float layerMultiplier)
            {
                if (!isFixedPosition)
                {
                    RectangleF elementRect = new RectangleF(position, new Size2(texture.Bounds.X, texture.Bounds.Y));
                    if (!elementRect.Intersects(boundingRect.ToRectangle()))
                    {
                        do
                        {
                            RandomizePosition(boundingRect);
                            elementRect = new RectangleF(position, new Size2(texture.Width, texture.Height));
                        } while (elementRect.Intersects(cameraBoundingBox.ToRectangle()));
                    }
                    this.position += new Vector2(speed.X * deltaTime, speed.Y * deltaTime);
                    this.position += new Vector2(cameraMovementSinceLastFrame.X * layerMultiplier, cameraMovementSinceLastFrame.Y * layerMultiplier);
                }
            }

            public virtual void Draw(SpriteBatch sb, RectangleF cameraBoundingBox, Color colorTint)
            {
                if (isFixedPosition)
                {
                    if (stretchToFit)
                    {
                        //System.Diagnostics.Debug.WriteLine(cameraBoundingBox);
                        sb.Draw(texture, cameraBoundingBox.TopLeft + position, texture.Bounds, colorTint);
                    }
                    else
                    {
                        sb.Draw(texture, cameraBoundingBox.TopLeft + position, texture.Bounds, colorTint);
                    }
                } else
                {
                    if (new RectangleF(position.X, position.Y, texture.Width, texture.Height).Intersects(cameraBoundingBox))
                    {
                        sb.Draw(texture, position, texture.Bounds, colorTint);
                    }
                }
            }
        }

        public class RotatingElement : Element
        {
            private float rotationSpeed = 0;
            private float angle;

            public RotatingElement(Texture2D texture, RectangleF inflatedBounds, Vector2 speed, float rotationSpeed) : base(texture, inflatedBounds, speed)
            {
                this.rotationSpeed = rotationSpeed;
                this.angle = 3.14f / 2;
            }

            public override void Update(float deltaTime, RectangleF boundingRect, RectangleF cameraBoundingBox, Vector2 cameraMovementSinceLastFrame, float layerMultiplier)
            {
                base.Update(deltaTime, boundingRect, cameraBoundingBox, cameraMovementSinceLastFrame, layerMultiplier);
                angle += rotationSpeed;
            }

            public override void Draw(SpriteBatch sb, RectangleF cameraBoundingBox, Color colorTint)
            {
                if (isFixedPosition)
                {
                    if (stretchToFit)
                    {
                        Console.WriteLine("LAYEREDBACKGROUND:STRETCHTOFIT IS WEIRD");
                    }
                    sb.Draw(texture, cameraBoundingBox.TopLeft + position, texture.Bounds, colorTint, angle, new Vector2(texture.Width / 2, texture.Height / 2), 1.0f, SpriteEffects.None, 1.0f);
                }
                else
                {
                    if (new RectangleF(position.X, position.Y, texture.Width, texture.Height).Intersects(cameraBoundingBox))
                    {
                        sb.Draw(texture, position, texture.Bounds, colorTint, angle, new Vector2(texture.Width / 2, texture.Height / 2), 1.0f, SpriteEffects.None, 1.0f);
                    }
                }
            }
        }

        public class Layer
        {
            private string name;
            private List<Element> elements;
            private Color colorTint;
            //0.0 means it completely ignores the camera movement - position indepedent of camera
            //positive numbers means it overreacts to camera movment
            //negative numbers means it holds position somewhat vs camera movement - sun would have a -1 multiplier - no matter what camera does, moves global position to maintain same spot in viewport
            private float layerMultipier;
            //private bool absolutePosition;
            private bool disable;
            private float transparency;

            public Layer(string name, Color colorTint, float layerMultiplier)
            {
                this.name = name;
                this.colorTint = colorTint;
                //this.absolutePosition = absolutePosition;
                elements = new List<Element>();
                this.layerMultipier = -layerMultiplier;
                this.disable = false;
                this.transparency = 1.0f;
            }

            public void Randomize(RectangleF bounds)
            {
                foreach(Element e in elements)
                {
                    e.RandomizePosition(bounds);
                }
            }

            public void Disable()
            {
                this.disable = true;
            }

            public void Enable()
            {
                this.disable = false;
            }

            public Color GetColorTint()
            {
                return colorTint;
            }

            public bool IsDisabled()
            {
                return disable;
            }

            public string GetName()
            {
                return name;
            }

            public void AddElement(Element element)
            {
                elements.Add(element);
            }

            public void ChangeColorTint(Color newTint)
            {
                this.colorTint = newTint;
            }

            public void Update(float deltaTime, RectangleF inflatedBounds, RectangleF cameraBoundingBox, Vector2 cameraMovementSinceLastFrame)
            {
                foreach(Element element in elements)
                {
                    element.Update(deltaTime, inflatedBounds, cameraBoundingBox, cameraMovementSinceLastFrame, layerMultipier);
                }
            }

            public void SetTransparency(float transparency)
            {
                this.transparency = transparency;
            }

            public void Draw(SpriteBatch sb, RectangleF cameraBoundingBox, float layerTransparency) {
                foreach(Element element in elements)
                {
                    element.Draw(sb, cameraBoundingBox, colorTint * transparency * layerTransparency);
                }
            }
        }

        private static string BACKGROUND_LAYER_SKY_DAY = "skyDay";
        private static string BACKGROUND_LAYER_SKY_MORNING = "skyMorning";
        private static string BACKGROUND_LAYER_SKY_EVENING = "skyEvening";
        private static string BACKGROUND_LAYER_SKY_NIGHT = "skyNight";

        private static string BACKGROUND_LAYER_SUN = "sun";
        private static string BACKGROUND_LAYER_MOON = "moon";
        private static string BACKGROUND_LAYER_STARS = "stars";
        private static string BACKGROUND_LAYER_PLANETS = "planets";

        private static string BACKGROUND_LAYER_CLOUDS_FRONT = "cloudsFront";
        private static string BACKGROUND_LAYER_CLOUDS_MIDDLE = "cloudsMiddle";
        private static string BACKGROUND_LAYER_CLOUDS_BACK = "cloudsBack";

        private static string BACKGROUND_LAYER_RAIN_FRONT = "rainFront";
        private static string BACKGROUND_LAYER_RAIN_BACK = "rainBack";
        private static string FOREGROUND_LAYER_RAIN = "fgRain";

        private static string BACKGROUND_LAYER_SNOW_FRONT = "snowFront";
        private static string BACKGROUND_LAYER_SNOW_BACK = "snowBack";
        private static string FOREGROUND_LAYER_SNOW = "fgSnow";

        private static string FOREGROUND_LAYER_CLOUDS = "cloudsFG";
        private static string FOREGROUND_LAYER_PARTICLES = "particleFG";
        private static string BACKGROUND_LAYER_PARTICLES = "particleBG";

        private static string FILTER_LAYER = "filter";

        private static int FOREGROUND_PX_PER_RAIN = 1600;
        private static int FOREGROUND_PX_PER_SNOW = 1800;
        private static int FOREGROUND_PX_PER_PARTICLE = 4100;
        private static int BACKGROUND_PX_PER_CLOUD_FRONT = 30000;
        private static int BACKGROUND_PX_PER_CLOUD = 16000;
        //private static int FOREGROUND_PX_PER_CLOUD = 125000;
        private static int BACKGROUND_PX_PER_RAIN = 1300;
        private static int BACKGROUND_PX_PER_SNOW = 1500;
        private static int BACKGROUND_PX_PER_PARTICLE = 2500;
        private static int BACKGROUND_PX_PER_STAR = 2500;
        private static float BLEND_MINUTES = 60.0f;

        private Dictionary<string, Layer> layers;
        private RectangleF lastFrameCameraBox;
        private bool firstFrame;
        private BackgroundParams parameters;
        
        public LayeredBackground(ContentManager Content, RectangleF cameraBoundingBox, BackgroundParams parameters)
        {
            this.firstFrame = true;
            this.parameters = parameters;

            layers = new Dictionary<string, Layer>();
            RectangleF inflatedBounds = LayeredBackground.CalculateInflatedBoundsRect(cameraBoundingBox);
            int area = (int)Util.CalculateArea(inflatedBounds);

            switch (parameters.type)
            {
                case BackgroundParams.Type.BACKGROUND_SKY:
                case BackgroundParams.Type.BACKGROUND_SPACE:
                    LayeredBackground.Layer morningSkyLayer = new LayeredBackground.Layer(BACKGROUND_LAYER_SKY_MORNING, Color.White, -1.0f);
                    LayeredBackground.Layer daySkyLayer = new LayeredBackground.Layer(BACKGROUND_LAYER_SKY_DAY, Color.White, -1.0f);
                    LayeredBackground.Layer eveningSkyLayer = new LayeredBackground.Layer(BACKGROUND_LAYER_SKY_EVENING, Color.White, -1.0f);
                    LayeredBackground.Layer nightSkyLayer = new LayeredBackground.Layer(BACKGROUND_LAYER_SKY_NIGHT, Color.White, -1.0f);
                    LayeredBackground.Layer starLayer = new LayeredBackground.Layer(BACKGROUND_LAYER_STARS, Color.White, -0.9f);
                    LayeredBackground.Layer sunLayer = new LayeredBackground.Layer(BACKGROUND_LAYER_SUN, Color.White, -1.0f);
                    LayeredBackground.Layer moonLayer = new LayeredBackground.Layer(BACKGROUND_LAYER_MOON, Color.White, -1.0f);
                    LayeredBackground.Layer planetLayer = new LayeredBackground.Layer(BACKGROUND_LAYER_PLANETS, Color.White, -1.0f);

                    if (parameters.type == BackgroundParams.Type.BACKGROUND_SKY)
                    {
                        morningSkyLayer.AddElement(new LayeredBackground.Element(Content.Load<Texture2D>(Paths.BACKGROUND_SKY_MORNING), new Vector2(0, 0), new Vector2(0, 0), true));
                        daySkyLayer.AddElement(new LayeredBackground.Element(Content.Load<Texture2D>(Paths.BACKGROUND_SKY_DAY), new Vector2(0, 0), new Vector2(0, 0), true));
                        eveningSkyLayer.AddElement(new LayeredBackground.Element(Content.Load<Texture2D>(Paths.BACKGROUND_SKY_EVENING), new Vector2(0, 0), new Vector2(0, 0), true));
                        nightSkyLayer.AddElement(new LayeredBackground.Element(Content.Load<Texture2D>(Paths.BACKGROUND_SKY_NIGHT), new Vector2(0, 0), new Vector2(0, 0), true));
                        sunLayer.AddElement(new LayeredBackground.Element(Content.Load<Texture2D>(Paths.BACKGROUND_SUN), new Vector2(30.0f / 320.0f, 8.0f / 200.0f), new Vector2(0, 0)));
                        moonLayer.AddElement(new LayeredBackground.Element(Content.Load<Texture2D>(Paths.BACKGROUND_MOON), new Vector2(30.0f / 320.0f, 8.0f / 200.0f), new Vector2(0, 0)));
                    }
                    else if (parameters.type == BackgroundParams.Type.BACKGROUND_SPACE)
                    {
                        morningSkyLayer.AddElement(new LayeredBackground.Element(Content.Load<Texture2D>(Paths.BACKGROUND_SKY_SPACE), new Vector2(0, 0), new Vector2(0, 0), true));
                        daySkyLayer.AddElement(new LayeredBackground.Element(Content.Load<Texture2D>(Paths.BACKGROUND_SKY_SPACE), new Vector2(0, 0), new Vector2(0, 0), true));
                        eveningSkyLayer.AddElement(new LayeredBackground.Element(Content.Load<Texture2D>(Paths.BACKGROUND_SKY_SPACE), new Vector2(0, 0), new Vector2(0, 0), true));
                        nightSkyLayer.AddElement(new LayeredBackground.Element(Content.Load<Texture2D>(Paths.BACKGROUND_SKY_SPACE), new Vector2(0, 0), new Vector2(0, 0), true));

                        planetLayer.AddElement(new LayeredBackground.Element(Content.Load<Texture2D>(Paths.BACKGROUND_PLANETS), new Vector2(30.0f / 320.0f, 8.0f / 200.0f), new Vector2(0, 0)));

                    }

                    for (int i = 0; i < area / BACKGROUND_PX_PER_STAR; i++)
                    {
                        starLayer.AddElement(new LayeredBackground.Element(Content.Load<Texture2D>(Paths.BACKGROUND_STAR), inflatedBounds, new Vector2(Util.RandInt(0, 0), 0)));
                    }

                    LayeredBackground.Layer cloudLayerBack = new LayeredBackground.Layer(BACKGROUND_LAYER_CLOUDS_BACK, Util.CLOUD_BACK_DAY.color, -0.6f);
                    LayeredBackground.Layer cloudLayerMiddle = new LayeredBackground.Layer(BACKGROUND_LAYER_CLOUDS_MIDDLE, Util.CLOUD_MIDDLE_DAY.color, -0.3f);
                    LayeredBackground.Layer cloudLayerFront = new LayeredBackground.Layer(BACKGROUND_LAYER_CLOUDS_FRONT, Util.CLOUD_FRONT_DAY.color, 0.0f);


                    if (parameters.hasClouds)
                    {
                        for (int i = 0; i < area / BACKGROUND_PX_PER_CLOUD_FRONT; i++)
                        {
                            switch (Util.RandInt(1, 6))
                            {
                                case 1:
                                    cloudLayerFront.AddElement(new LayeredBackground.Element(Content.Load<Texture2D>(Paths.BACKGROUND_CLOUD_LG1), inflatedBounds, new Vector2(Util.RandInt(17, 22), 0)));
                                    break;
                                case 2:
                                    cloudLayerFront.AddElement(new LayeredBackground.Element(Content.Load<Texture2D>(Paths.BACKGROUND_CLOUD_LG2), inflatedBounds, new Vector2(Util.RandInt(17, 22), 0)));
                                    break;
                                case 3:
                                    cloudLayerFront.AddElement(new LayeredBackground.Element(Content.Load<Texture2D>(Paths.BACKGROUND_CLOUD_LG3), inflatedBounds, new Vector2(Util.RandInt(17, 22), 0)));
                                    break;
                                case 4:
                                    cloudLayerFront.AddElement(new LayeredBackground.Element(Content.Load<Texture2D>(Paths.BACKGROUND_CLOUD_LG4), inflatedBounds, new Vector2(Util.RandInt(17, 22), 0)));
                                    break;
                                case 5:
                                    cloudLayerFront.AddElement(new LayeredBackground.Element(Content.Load<Texture2D>(Paths.BACKGROUND_CLOUD_LG5), inflatedBounds, new Vector2(Util.RandInt(17, 22), 0)));
                                    break;
                                case 6:
                                    cloudLayerFront.AddElement(new LayeredBackground.Element(Content.Load<Texture2D>(Paths.BACKGROUND_CLOUD_LG6), inflatedBounds, new Vector2(Util.RandInt(17, 22), 0)));
                                    break;
                            }
                        }
                        for (int i = 0; i < area / BACKGROUND_PX_PER_CLOUD; i++)
                        {
                            switch (Util.RandInt(1, 6))
                            {
                                case 1:
                                    cloudLayerMiddle.AddElement(new LayeredBackground.Element(Content.Load<Texture2D>(Paths.BACKGROUND_CLOUD_MD1), inflatedBounds, new Vector2(Util.RandInt(10, 15), 0)));
                                    break;
                                case 2:
                                    cloudLayerMiddle.AddElement(new LayeredBackground.Element(Content.Load<Texture2D>(Paths.BACKGROUND_CLOUD_MD2), inflatedBounds, new Vector2(Util.RandInt(10, 15), 0)));
                                    break;
                                case 3:
                                    cloudLayerMiddle.AddElement(new LayeredBackground.Element(Content.Load<Texture2D>(Paths.BACKGROUND_CLOUD_MD3), inflatedBounds, new Vector2(Util.RandInt(10, 15), 0)));
                                    break;
                                case 4:
                                    cloudLayerMiddle.AddElement(new LayeredBackground.Element(Content.Load<Texture2D>(Paths.BACKGROUND_CLOUD_MD4), inflatedBounds, new Vector2(Util.RandInt(10, 15), 0)));
                                    break;
                                case 5:
                                    cloudLayerMiddle.AddElement(new LayeredBackground.Element(Content.Load<Texture2D>(Paths.BACKGROUND_CLOUD_MD5), inflatedBounds, new Vector2(Util.RandInt(10, 15), 0)));
                                    break;
                                case 6:
                                    cloudLayerMiddle.AddElement(new LayeredBackground.Element(Content.Load<Texture2D>(Paths.BACKGROUND_CLOUD_MD6), inflatedBounds, new Vector2(Util.RandInt(10, 15), 0)));
                                    break;
                                case 7:
                                    cloudLayerMiddle.AddElement(new LayeredBackground.Element(Content.Load<Texture2D>(Paths.BACKGROUND_CLOUD_MD7), inflatedBounds, new Vector2(Util.RandInt(10, 15), 0)));
                                    break;
                                case 8:
                                    cloudLayerMiddle.AddElement(new LayeredBackground.Element(Content.Load<Texture2D>(Paths.BACKGROUND_CLOUD_MD8), inflatedBounds, new Vector2(Util.RandInt(10, 15), 0)));
                                    break;
                            }
                        }
                        for (int i = 0; i < area / BACKGROUND_PX_PER_CLOUD; i++)
                        {
                            switch (Util.RandInt(1, 6))
                            {
                                case 1:
                                    cloudLayerBack.AddElement(new LayeredBackground.Element(Content.Load<Texture2D>(Paths.BACKGROUND_CLOUD_SM1), inflatedBounds, new Vector2(Util.RandInt(3, 8), 0)));
                                    break;
                                case 2:
                                    cloudLayerBack.AddElement(new LayeredBackground.Element(Content.Load<Texture2D>(Paths.BACKGROUND_CLOUD_SM2), inflatedBounds, new Vector2(Util.RandInt(3, 8), 0)));
                                    break;
                                case 3:
                                    cloudLayerBack.AddElement(new LayeredBackground.Element(Content.Load<Texture2D>(Paths.BACKGROUND_CLOUD_SM3), inflatedBounds, new Vector2(Util.RandInt(3, 8), 0)));
                                    break;
                                case 4:
                                    cloudLayerBack.AddElement(new LayeredBackground.Element(Content.Load<Texture2D>(Paths.BACKGROUND_CLOUD_SM4), inflatedBounds, new Vector2(Util.RandInt(3, 8), 0)));
                                    break;
                                case 5:
                                    cloudLayerBack.AddElement(new LayeredBackground.Element(Content.Load<Texture2D>(Paths.BACKGROUND_CLOUD_SM5), inflatedBounds, new Vector2(Util.RandInt(3, 8), 0)));
                                    break;
                                case 6:
                                    cloudLayerBack.AddElement(new LayeredBackground.Element(Content.Load<Texture2D>(Paths.BACKGROUND_CLOUD_SM6), inflatedBounds, new Vector2(Util.RandInt(3, 8), 0)));
                                    break;
                            }
                        }
                    }

                    LayeredBackground.Layer rainLayerFront = new LayeredBackground.Layer(BACKGROUND_LAYER_RAIN_FRONT, Color.White, -0.5f);
                    for (int i = 0; i < area / BACKGROUND_PX_PER_RAIN; i++)
                    {
                        rainLayerFront.AddElement(new LayeredBackground.Element(Content.Load<Texture2D>(Paths.BACKGROUND_RAIN_FRONT), inflatedBounds, new Vector2(Util.RandInt(-10, 10), 450)));
                    }

                    LayeredBackground.Layer rainLayerBack = new LayeredBackground.Layer(BACKGROUND_LAYER_RAIN_BACK, Color.White, -0.7f);
                    for (int i = 0; i < area / BACKGROUND_PX_PER_RAIN; i++)
                    {
                        rainLayerBack.AddElement(new LayeredBackground.Element(Content.Load<Texture2D>(Paths.BACKGROUND_RAIN_BACK), inflatedBounds, new Vector2(Util.RandInt(-10, 10), 350)));
                    }

                    LayeredBackground.Layer snowLayerFront = new LayeredBackground.Layer(BACKGROUND_LAYER_SNOW_FRONT, Color.White, -0.5f);
                    for (int i = 0; i < area / BACKGROUND_PX_PER_SNOW; i++)
                    {
                        snowLayerFront.AddElement(new LayeredBackground.RotatingElement(Content.Load<Texture2D>(Paths.BACKGROUND_SNOW_FRONT), inflatedBounds, new Vector2(Util.RandInt(-15, 15), 35),
                            Util.RandInt(-6, 6) / 100.0f));
                    }

                    LayeredBackground.Layer snowLayerBack = new LayeredBackground.Layer(BACKGROUND_LAYER_SNOW_BACK, Color.White, -0.5f);
                    for (int i = 0; i < area / BACKGROUND_PX_PER_SNOW; i++)
                    {
                        snowLayerBack.AddElement(new LayeredBackground.RotatingElement(Content.Load<Texture2D>(Paths.BACKGROUND_SNOW_BACK), inflatedBounds, new Vector2(Util.RandInt(-7, 7), 25),
                            Util.RandInt(-4, 4) / 100.0f));
                    }

                    LayeredBackground.Layer backgroundParticleLayer = new LayeredBackground.Layer(BACKGROUND_LAYER_PARTICLES, Color.White, -0.5f);
                    if (parameters.hasParticles)
                    {
                        for (int i = 0; i < area / BACKGROUND_PX_PER_PARTICLE; i++)
                        {
                            backgroundParticleLayer.AddElement(new LayeredBackground.RotatingElement(Content.Load<Texture2D>(Paths.SPRITE_PARTICLE_1x2),
                                inflatedBounds,
                                new Vector2(Util.RandInt(60, 90), Util.RandInt(-10, 10)), Util.RandInt(-8, 8) / 100.0f));
                        }
                    }

                    this.AddLayer(morningSkyLayer);
                    this.AddLayer(daySkyLayer);
                    this.AddLayer(eveningSkyLayer);
                    this.AddLayer(nightSkyLayer);
                    this.AddLayer(starLayer);
                    this.AddLayer(sunLayer);
                    this.AddLayer(moonLayer);
                    this.AddLayer(planetLayer);
                    this.AddLayer(cloudLayerBack);
                    this.AddLayer(rainLayerBack);
                    this.AddLayer(snowLayerBack);
                    this.AddLayer(cloudLayerMiddle);
                    this.AddLayer(backgroundParticleLayer);
                    this.AddLayer(cloudLayerFront);
                    this.AddLayer(rainLayerFront);
                    this.AddLayer(snowLayerFront);
                    break;
                case BackgroundParams.Type.FOREGROUND_SKY:
                case BackgroundParams.Type.FOREGROUND_SPACE:
                    LayeredBackground.Layer foregroundRainLayer = new LayeredBackground.Layer(FOREGROUND_LAYER_RAIN, Color.White * Util.FOREGROUND_TRANSPARENCY, 0.3f);
                    for (int i = 0; i < area / FOREGROUND_PX_PER_RAIN; i++)
                    {
                        foregroundRainLayer.AddElement(new LayeredBackground.Element(Content.Load<Texture2D>(Paths.FOREGROUND_RAIN), inflatedBounds, new Vector2(Util.RandInt(-10, 10), 600)));
                    }

                    LayeredBackground.Layer foregroundSnowLayer = new LayeredBackground.Layer(FOREGROUND_LAYER_SNOW, Color.White * Util.FOREGROUND_TRANSPARENCY, 0.3f);
                    for (int i = 0; i < area / FOREGROUND_PX_PER_SNOW; i++)
                    {
                        foregroundSnowLayer.AddElement(new LayeredBackground.RotatingElement(Content.Load<Texture2D>(Paths.FOREGROUND_SNOW), inflatedBounds, new Vector2(Util.RandInt(-23, 23), 45),
                            Util.RandInt(-3, 3) / 100.0f));
                    }

                    LayeredBackground.Layer foregroundParticleLayer = new LayeredBackground.Layer(FOREGROUND_LAYER_PARTICLES, Color.White * Util.FOREGROUND_TRANSPARENCY, 0.3f);
                    if (parameters.hasParticles)
                    {
                        for (int i = 0; i < area / FOREGROUND_PX_PER_PARTICLE; i++)
                        {
                            foregroundParticleLayer.AddElement(new LayeredBackground.RotatingElement(Content.Load<Texture2D>(Paths.SPRITE_PARTICLE_2x2),
                                inflatedBounds,
                                new Vector2(Util.RandInt(100, 140), Util.RandInt(-25, 25)), Util.RandInt(-8, 8) / 100.0f));
                        }
                    }

                    /*LayeredBackground.Layer foregroundCloudLayer = new LayeredBackground.Layer(FOREGROUND_LAYER_CLOUDS, Color.White * Util.FOREGROUND_TRANSPARENCY, 0.1f);
                    for (int i = 0; i < area / FOREGROUND_PX_PER_CLOUD; i++)
                    {
                        switch (Util.RandInt(1, 6))
                        {
                            case 1:
                                foregroundCloudLayer.AddElement(new LayeredBackground.Element(Content.Load<Texture2D>(Paths.BACKGROUND_CLOUD_LG1), inflatedBounds, new Vector2(Util.RandInt(35, 42), 0)));
                                break;
                            case 2:
                                foregroundCloudLayer.AddElement(new LayeredBackground.Element(Content.Load<Texture2D>(Paths.BACKGROUND_CLOUD_LG2), inflatedBounds, new Vector2(Util.RandInt(35, 42), 0)));
                                break;
                            case 3:
                                foregroundCloudLayer.AddElement(new LayeredBackground.Element(Content.Load<Texture2D>(Paths.BACKGROUND_CLOUD_LG3), inflatedBounds, new Vector2(Util.RandInt(35, 42), 0)));
                                break;
                            case 4:
                                foregroundCloudLayer.AddElement(new LayeredBackground.Element(Content.Load<Texture2D>(Paths.BACKGROUND_CLOUD_LG4), inflatedBounds, new Vector2(Util.RandInt(35, 42), 0)));
                                break;
                            case 5:
                                foregroundCloudLayer.AddElement(new LayeredBackground.Element(Content.Load<Texture2D>(Paths.BACKGROUND_CLOUD_LG5), inflatedBounds, new Vector2(Util.RandInt(35, 42), 0)));
                                break;
                            case 6:
                                foregroundCloudLayer.AddElement(new LayeredBackground.Element(Content.Load<Texture2D>(Paths.BACKGROUND_CLOUD_LG6), inflatedBounds, new Vector2(Util.RandInt(35, 42), 0)));
                                break;
                        }
                    }*/

                    LayeredBackground.Layer filterLayer = new LayeredBackground.Layer(FILTER_LAYER, Color.White, 1.0f);
                    filterLayer.AddElement(new LayeredBackground.Element(Content.Load<Texture2D>(Paths.INTERFACE_BACKGROUND_BLACK), new Vector2(-9, -9), new Vector2(0, 0)));

                    this.AddLayer(foregroundParticleLayer);
                    //this.AddLayer(foregroundCloudLayer);
                    this.AddLayer(foregroundSnowLayer);
                    this.AddLayer(foregroundRainLayer);
                    this.AddLayer(filterLayer);
                    break;
            }
        }

        public void AddLayer(Layer layer)
        {
            layers[layer.GetName()] = layer;
        }

        public void TrySetTransparency(string name, float transparency)
        {
            if(layers.ContainsKey(name))
            {
                layers[name].SetTransparency(transparency);
            }
        }

        public Color TryGetTint(string name)
        {
            if(layers.ContainsKey(name))
            {
                return layers[name].GetColorTint();
            }
            return Color.White;
        }

        public void TrySetTint(string name, Color tint)
        {
            if (layers.ContainsKey(name))
            {
                layers[name].ChangeColorTint(tint);
            }
        }

        public void TryDisableLayer(string name)
        {
            if (layers.ContainsKey(name))
            {
                layers[name].Disable();
            }
        }

        public void TryEnableLayer(string name)
        {
            if (layers.ContainsKey(name))
            {
                layers[name].Enable();
            }
        }

        public Layer GetLayer(string name)
        {
            if(!layers.ContainsKey(name))
            {
                return null;
            }
            return layers[name];
        }

        private static float X_INFLATION = PlateauMain.NATIVE_RESOLUTION_WIDTH*2;
        private static float Y_INFLATION = PlateauMain.NATIVE_RESOLUTION_HEIGHT*2;

        public static RectangleF CalculateInflatedBoundsRect(RectangleF cameraBoundingBox)
        {
            return new RectangleF(cameraBoundingBox.X - X_INFLATION, cameraBoundingBox.Y - Y_INFLATION, cameraBoundingBox.Width + (2 * X_INFLATION), cameraBoundingBox.Height + (2 * Y_INFLATION));
        }

        public void Update(float deltaTime, RectangleF cameraBoundingBox, World.TimeData timeData, World.Weather areaWeather, World.Season areaSeason)
        {
            if(firstFrame)
            {
                firstFrame = !firstFrame;
                lastFrameCameraBox = cameraBoundingBox;
                Randomize(cameraBoundingBox); //needed, otherwise randomize is never called for the background of the very first area the player spawns into; causing odd behavior (such as no stars when sleeping)
            }

            Vector2 cameraMovementSinceLastFrame = new Vector2(cameraBoundingBox.X - lastFrameCameraBox.X, cameraBoundingBox.Y - lastFrameCameraBox.Y);
            lastFrameCameraBox = cameraBoundingBox;

            RectangleF inflatedBounds = CalculateInflatedBoundsRect(cameraBoundingBox);
            foreach (string layerName in layers.Keys)
            {
                if (!layers[layerName].IsDisabled())
                {
                    layers[layerName].Update(deltaTime, inflatedBounds, cameraBoundingBox, cameraMovementSinceLastFrame);
                }
            }

            //update background/foreground according to time...
            World.TimeOfDay mainTime = timeData.timeOfDay;
            World.TimeOfDay blendTime = World.NextTimeOfDay(mainTime);
            int minsTillTransition = World.MinutesUntilTransition(timeData.hour, timeData.minute);

            switch (parameters.type)
            {
                case BackgroundParams.Type.BACKGROUND_SKY:
                case BackgroundParams.Type.BACKGROUND_SPACE:
                    TryDisableLayer(BACKGROUND_LAYER_SKY_MORNING);
                    TrySetTransparency(BACKGROUND_LAYER_SKY_MORNING, 1.0f);
                    TryDisableLayer(BACKGROUND_LAYER_SKY_DAY);
                    TrySetTransparency(BACKGROUND_LAYER_SKY_DAY, 1.0f);
                    TryDisableLayer(BACKGROUND_LAYER_SKY_EVENING);
                    TrySetTransparency(BACKGROUND_LAYER_SKY_EVENING, 1.0f);
                    TryDisableLayer(BACKGROUND_LAYER_SKY_NIGHT);
                    TrySetTransparency(BACKGROUND_LAYER_SKY_NIGHT, 1.0f);
                    TryDisableLayer(BACKGROUND_LAYER_RAIN_BACK);
                    TryDisableLayer(BACKGROUND_LAYER_RAIN_FRONT);
                    TryDisableLayer(BACKGROUND_LAYER_SNOW_BACK);
                    TryDisableLayer(BACKGROUND_LAYER_SNOW_FRONT);
                    TryEnableLayer(BACKGROUND_LAYER_CLOUDS_FRONT);
                    TryEnableLayer(BACKGROUND_LAYER_CLOUDS_MIDDLE);
                    TryEnableLayer(BACKGROUND_LAYER_CLOUDS_BACK);
                    TryEnableLayer(BACKGROUND_LAYER_PARTICLES);
                    TryEnableLayer(BACKGROUND_LAYER_SUN);
                    TryDisableLayer(BACKGROUND_LAYER_STARS);
                    TryEnableLayer(BACKGROUND_LAYER_CLOUDS_MIDDLE);
                    switch (timeData.timeOfDay)
                    {
                        case World.TimeOfDay.MORNING:
                            TryEnableLayer(BACKGROUND_LAYER_SKY_MORNING);
                            TrySetTransparency(BACKGROUND_LAYER_SUN, 1.0f);
                            TrySetTransparency(BACKGROUND_LAYER_MOON, 0.0f);
                            TrySetTint(BACKGROUND_LAYER_CLOUDS_FRONT, Util.CLOUD_FRONT_DAY.color);
                            TrySetTint(BACKGROUND_LAYER_CLOUDS_MIDDLE, Util.CLOUD_MIDDLE_DAY.color);
                            TrySetTint(BACKGROUND_LAYER_CLOUDS_BACK, Util.CLOUD_BACK_DAY.color);
                            if (minsTillTransition <= BLEND_MINUTES)
                            {
                                TryEnableLayer(BACKGROUND_LAYER_SKY_DAY);
                                TrySetTransparency(BACKGROUND_LAYER_SKY_DAY, 1.0f - (minsTillTransition / BLEND_MINUTES));

                            }
                            break;
                        case World.TimeOfDay.DAY:
                            TryEnableLayer(BACKGROUND_LAYER_SKY_DAY);
                            TrySetTransparency(BACKGROUND_LAYER_SUN, 1.0f);
                            TrySetTransparency(BACKGROUND_LAYER_MOON, 0.0f);
                            TrySetTint(BACKGROUND_LAYER_CLOUDS_FRONT, Util.CLOUD_FRONT_DAY.color);
                            TrySetTint(BACKGROUND_LAYER_CLOUDS_MIDDLE, Util.CLOUD_MIDDLE_DAY.color);
                            TrySetTint(BACKGROUND_LAYER_CLOUDS_BACK, Util.CLOUD_BACK_DAY.color);
                            if (minsTillTransition <= BLEND_MINUTES)
                            {
                                TryEnableLayer(BACKGROUND_LAYER_SKY_EVENING);
                                TrySetTransparency(BACKGROUND_LAYER_SKY_EVENING, 1.0f - (minsTillTransition / BLEND_MINUTES));
                                TrySetTint(BACKGROUND_LAYER_CLOUDS_FRONT, Util.BlendColors(Util.CLOUD_FRONT_DAY.color, Util.CLOUD_FRONT_EVENING.color, 1.0f - (minsTillTransition / BLEND_MINUTES)));
                                TrySetTint(BACKGROUND_LAYER_CLOUDS_MIDDLE, Util.BlendColors(Util.CLOUD_MIDDLE_DAY.color, Util.CLOUD_MIDDLE_EVENING.color, 1.0f - (minsTillTransition / BLEND_MINUTES)));
                                TrySetTint(BACKGROUND_LAYER_CLOUDS_BACK, Util.BlendColors(Util.CLOUD_BACK_DAY.color, Util.CLOUD_BACK_EVENING.color, 1.0f - (minsTillTransition / BLEND_MINUTES)));
                            }
                            break;
                        case World.TimeOfDay.EVENING:
                            TryEnableLayer(BACKGROUND_LAYER_SKY_EVENING);
                            TrySetTransparency(BACKGROUND_LAYER_SUN, 1.0f);
                            TrySetTransparency(BACKGROUND_LAYER_MOON, 0.0f);
                            TrySetTint(BACKGROUND_LAYER_CLOUDS_FRONT, Util.CLOUD_FRONT_EVENING.color);
                            TrySetTint(BACKGROUND_LAYER_CLOUDS_MIDDLE, Util.CLOUD_MIDDLE_EVENING.color);
                            TrySetTint(BACKGROUND_LAYER_CLOUDS_BACK, Util.CLOUD_BACK_EVENING.color);
                            if (minsTillTransition <= BLEND_MINUTES)
                            {
                                TryEnableLayer(BACKGROUND_LAYER_SKY_NIGHT);
                                TrySetTransparency(BACKGROUND_LAYER_SKY_NIGHT, 1.0f - (minsTillTransition / BLEND_MINUTES));
                                TrySetTint(BACKGROUND_LAYER_CLOUDS_FRONT, Util.BlendColors(Util.CLOUD_FRONT_EVENING.color, Util.CLOUD_FRONT_NIGHT.color, 1.0f - (minsTillTransition / BLEND_MINUTES)));
                                TrySetTint(BACKGROUND_LAYER_CLOUDS_MIDDLE, Util.BlendColors(Util.CLOUD_MIDDLE_EVENING.color, Util.CLOUD_MIDDLE_NIGHT.color, 1.0f - (minsTillTransition / BLEND_MINUTES)));
                                TrySetTint(BACKGROUND_LAYER_CLOUDS_BACK, Util.BlendColors(Util.CLOUD_BACK_EVENING.color, Util.CLOUD_BACK_NIGHT.color, 1.0f - (minsTillTransition / BLEND_MINUTES)));
                                TrySetTransparency(BACKGROUND_LAYER_SUN, minsTillTransition / BLEND_MINUTES);
                            }
                            break;
                        case World.TimeOfDay.NIGHT:
                            TryEnableLayer(BACKGROUND_LAYER_SKY_NIGHT);
                            TrySetTint(BACKGROUND_LAYER_CLOUDS_FRONT, Util.CLOUD_FRONT_NIGHT.color);
                            TrySetTint(BACKGROUND_LAYER_CLOUDS_MIDDLE, Util.CLOUD_MIDDLE_NIGHT.color);
                            TrySetTint(BACKGROUND_LAYER_CLOUDS_BACK, Util.CLOUD_BACK_NIGHT.color);
                            TrySetTransparency(BACKGROUND_LAYER_SUN, 0.0f);
                            TrySetTransparency(BACKGROUND_LAYER_MOON, ((timeData.hour * 60 + timeData.minute) - World.EVENING_END_HOUR * 60) / 60.0f);
                            TryEnableLayer(BACKGROUND_LAYER_STARS);
                            TrySetTransparency(BACKGROUND_LAYER_STARS, ((timeData.hour * 60 + timeData.minute) - World.EVENING_END_HOUR * 60) / 60.0f);
                            break;
                    }
                    if(parameters.type == BackgroundParams.Type.BACKGROUND_SPACE)
                    {
                        TryEnableLayer(BACKGROUND_LAYER_STARS);
                        TryEnableLayer(BACKGROUND_LAYER_PLANETS);
                    }
                    switch (areaWeather)
                    {
                        case World.Weather.CLOUDY:
                            break;
                        case World.Weather.RAINY:
                            //disable particles
                            TryDisableLayer(BACKGROUND_LAYER_PARTICLES);
                            //rain fx
                            TryEnableLayer(BACKGROUND_LAYER_RAIN_BACK);
                            TryEnableLayer(BACKGROUND_LAYER_RAIN_FRONT);
                            //color clouds gray
                            TrySetTint(BACKGROUND_LAYER_CLOUDS_BACK, Util.BlendColors(TryGetTint(BACKGROUND_LAYER_CLOUDS_BACK), Util.CLOUD_RAIN.color, 0.5f));
                            TrySetTint(BACKGROUND_LAYER_CLOUDS_MIDDLE, Util.BlendColors(TryGetTint(BACKGROUND_LAYER_CLOUDS_MIDDLE), Util.CLOUD_RAIN.color, 0.5f));
                            TrySetTint(BACKGROUND_LAYER_CLOUDS_FRONT, Util.BlendColors(TryGetTint(BACKGROUND_LAYER_CLOUDS_FRONT), Util.CLOUD_RAIN.color, 0.5f));
                            //disable sun
                            TryDisableLayer(BACKGROUND_LAYER_SUN);
                            break;
                        case World.Weather.SNOWY:
                            TryDisableLayer(BACKGROUND_LAYER_PARTICLES);
                            TryEnableLayer(BACKGROUND_LAYER_SNOW_BACK);
                            TryEnableLayer(BACKGROUND_LAYER_SNOW_FRONT);
                            //disable sun
                            TryDisableLayer(BACKGROUND_LAYER_SUN);
                            break;
                        case World.Weather.SUNNY:
                            TryDisableLayer(BACKGROUND_LAYER_CLOUDS_MIDDLE);
                            TryDisableLayer(BACKGROUND_LAYER_CLOUDS_BACK);
                            break;
                    }

                    switch (areaSeason)
                    {
                        case World.Season.SPRING:
                            TrySetTint(BACKGROUND_LAYER_PARTICLES, Util.PARTICLE_SPRING_PETAL_BACKGROUND.color);
                            break;
                        case World.Season.SUMMER:
                            TrySetTint(BACKGROUND_LAYER_PARTICLES, Util.PARTICLE_SUMMER_LEAF_BACKGROUND.color);
                            break;
                        case World.Season.AUTUMN:
                            TrySetTint(BACKGROUND_LAYER_PARTICLES, Util.PARTICLE_FALL_LEAF_BACKGROUND.color);
                            break;
                        case World.Season.WINTER:
                            TrySetTint(BACKGROUND_LAYER_PARTICLES, Util.PARTICLE_WINTER_SNOW_BACKGROUND.color);
                            break;
                    }
                    break;
                case BackgroundParams.Type.FOREGROUND_SKY:
                case BackgroundParams.Type.FOREGROUND_SPACE:
                    TryDisableLayer(FOREGROUND_LAYER_RAIN);
                    TryDisableLayer(FOREGROUND_LAYER_SNOW);
                    TryDisableLayer(FOREGROUND_LAYER_CLOUDS);
                    TryDisableLayer(FILTER_LAYER);
                    TryEnableLayer(FOREGROUND_LAYER_PARTICLES);
                    switch (areaWeather)
                    {
                        case World.Weather.CLOUDY:
                            TryEnableLayer(FILTER_LAYER);
                            TrySetTint(FILTER_LAYER, Util.CLOUDY_FILTER.color);
                            TryEnableLayer(FOREGROUND_LAYER_CLOUDS);
                            break;
                        case World.Weather.RAINY:
                            TryEnableLayer(FILTER_LAYER);
                            TrySetTint(FILTER_LAYER, Util.RAIN_FILTER.color);
                            //disable particles
                            TryDisableLayer(FOREGROUND_LAYER_PARTICLES);
                            //rain fx
                            TryEnableLayer(FOREGROUND_LAYER_RAIN);
                            break;
                        case World.Weather.SNOWY:
                            TryEnableLayer(FILTER_LAYER);
                            TrySetTint(FILTER_LAYER, Util.SNOWY_FILTER.color);
                            TryDisableLayer(FOREGROUND_LAYER_PARTICLES);
                            TryEnableLayer(FOREGROUND_LAYER_SNOW);
                            break;
                        case World.Weather.SUNNY:
                            break;
                    }

                    switch (areaSeason)
                    {
                        case World.Season.SPRING:
                            TrySetTint(FOREGROUND_LAYER_PARTICLES, Util.PARTICLE_SPRING_PETAL_FOREGROUND.color);
                            break;
                        case World.Season.SUMMER:
                            TrySetTint(FOREGROUND_LAYER_PARTICLES, Util.PARTICLE_SUMMER_LEAF_FOREGROUND.color);
                            break;
                        case World.Season.AUTUMN:
                            TrySetTint(FOREGROUND_LAYER_PARTICLES, Util.PARTICLE_FALL_LEAF_FOREGROUND.color);
                            break;
                        case World.Season.WINTER:
                            TrySetTint(FOREGROUND_LAYER_PARTICLES, Util.PARTICLE_WINTER_SNOW_FOREGROUND.color);
                            break;
                    }
                    break;
            }


        }

        public void Draw(SpriteBatch sb, RectangleF cameraBoundingBox, float transparency)
        {
            //Vector2 cameraPosition = cameraBoundingBox.Center;
            foreach(string layerName in layers.Keys)
            {
                if (!layers[layerName].IsDisabled())
                {
                    layers[layerName].Draw(sb, cameraBoundingBox, transparency);
                }
            }
        }

        public void Randomize(RectangleF cameraBoundingBox)
        {
            RectangleF bounds = CalculateInflatedBoundsRect(cameraBoundingBox);
            foreach(string layerName in layers.Keys)
            {
                layers[layerName].Randomize(bounds);
            }
        }
    }
}
