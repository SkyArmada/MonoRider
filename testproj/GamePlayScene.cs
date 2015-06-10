using System.Collections.Generic;
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
namespace MonoRider
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class GamePlayScene : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        ContentManager CM_Play;
        //ContentManager CM_GO;
        Player player;
        List<Sprite> GameObjectList;
        Texture2D background;
        SteeringWheel wheel;
        bool debug = false;
        bool paused = false;
        bool pausedPressed = false;
        float playerSpeed = 0f;
        int midPoint;
        public int gearsCollected = 0;
        private SpriteFont font;
        //bool moveRight = false;
        //bool moveLeft = false;
        //bool moveCenter = false;
        Random Ranum = new Random();

        enum GamePlayState
        {
            kStatePlay,
            kStateGO
        };
        GamePlayState currentState = GamePlayState.kStatePlay;

        public GamePlayScene()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 320;
            graphics.PreferredBackBufferHeight = 480;
            graphics.ApplyChanges();
            CM_Play = new ContentManager(Content.ServiceProvider);
            CM_Play.RootDirectory = "Content";
            this.IsMouseVisible = true;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            player = new Player();
            wheel = new SteeringWheel();
            GameObjectList = new List<Sprite>();
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            if (currentState == GamePlayState.kStatePlay)
            {
                // Create a new SpriteBatch, which can be used to draw textures.
                spriteBatch = new SpriteBatch(GraphicsDevice);

                // TODO: use this.Content to load your game content here
                background = CM_Play.Load<Texture2D>("Graphics\\grassBackground");
                midPoint = (GraphicsDevice.Viewport.Width / 2);

                player.LoadContent("Graphics\\car1", CM_Play);
                player._Position = new Vector2(GraphicsDevice.Viewport.Width / 2, 320);
                player.parentScene = this;
                GameObjectList.Add(player);

                wheel.LoadContent("Graphics\\wheel", CM_Play);
                wheel._Position = new Vector2(160, 520);
                
                for (int i = 0; i < 40; i++)
                {
                    Gear gear = new Gear();
                    gear.LoadContent("Graphics\\gear1", CM_Play);
                    gear.parentScene = this;
                    GameObjectList.Add(gear);
                }

                for (int i = 0; i <= 30; i++)
                {
                    EnemyCar car = new EnemyCar();
                    car.LoadContent("Graphics\\car2", CM_Play);
                    car.parentScene = this;
                    GameObjectList.Add(car);
                }
                for(int i = 0; i <= 30; i++)
                {
                    Rock rock = new Rock();
                    rock.LoadContent("Graphics\\rock", CM_Play);
                    rock.parentScene = this;
                    GameObjectList.Add(rock);
                }
                for (int i = 0; i <= 30; i++)
                {
                    Shield shield = new Shield();
                    shield.LoadContent("Graphics\\car1", CM_Play);
                    shield.parentScene = this;
                    GameObjectList.Add(shield);
                }
                for (int i = 0; i <= 30; i++)
                {
                    OilSlick slick = new OilSlick();
                    slick.LoadContent("Graphics\\oil", CM_Play);
                    slick.parentScene = this;
                    GameObjectList.Add(slick);
                }
                //font = Content.Load<SpriteFont>("Graphics\\Fipps-Regular");
            }
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
            CM_Play.Unload();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            HandleInput();
            if (!paused)
            {
                int chancePerSecond = 101 - ((int)playerSpeed / 10);
                if(chancePerSecond <= 15)
                {
                    chancePerSecond = 15;
                }
                if(Ranum.Next(0, chancePerSecond) == 0)
                {
                    int ran = Ranum.Next(0, 100);
                    if(ran >= 0 && ran <= 10)
                    {
                        ran = Ranum.Next(0, 5);
                        if (ran == 0)
                        {
                            PlacePattern("gear>");
                        }
                        else if (ran == 1)
                        {
                            PlacePattern("CGC");
                        }
                        else if (ran == 2)
                        {
                            PlacePattern("Hi");
                        }
                        else if (ran == 3)
                        {
                            PlacePattern("carV");
                        }
                        else if (ran == 4)
                        {
                            PlacePattern("giantRock");
                        }
                    }
                    else if (ran >= 11 && ran <= 24)
                    {
                        PlaceObject(Sprite.SpriteType.kGearType);
                    }
                    else if (ran >= 25 && ran <= 64)
                    {
                        PlaceObject(Sprite.SpriteType.kCarType);
                    }

                    else if (ran >= 65 && ran <= 80)
                    {
                        PlaceObject(Sprite.SpriteType.kRockType);
                    }
                    else if (ran >= 91 && ran <= 94)
                    {
                        PlaceObject(Sprite.SpriteType.kOilType);
                    }
                    else if (ran >= 95)
                    {
                        PlaceObject(Sprite.SpriteType.kShieldType);
                    }
                }
                // TODO: Add your update logic here
                foreach (Sprite obj in GameObjectList)
                {
                    if(obj._Tag == Sprite.SpriteType.kPlayerType)
                    {
                        playerSpeed = obj.speed;
                    }
                    else
                    {
                        obj.speed = playerSpeed;
                    }
                    obj.Update(gameTime, GameObjectList);
                }
                wheel.Update(gameTime, player.momentum);
                base.Update(gameTime);
            }

            if(player._CurrentState == Sprite.SpriteState.kStateInActive)
            {
                this.ResetGame();
                
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            // Start drawing
            if(debug)
            {
                spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Opaque);
                RasterizerState state = new RasterizerState();
                state.FillMode = FillMode.WireFrame;
                spriteBatch.GraphicsDevice.RasterizerState = state;
            }
            else
            {
                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);

                // Sprite effects! https://msdn.microsoft.com/en-us/library/bb203872(v=xnagamestudio.40).aspx
            }
            //spriteBatch.Draw(background, new Rectangle(midPoint-160, 0, 320, 480), Color.White);
            float test = midPoint - (background.Width/2);
            spriteBatch.Draw(background, new Vector2(test, 0), Color.White);

            // Draw the Player
            foreach (Sprite obj in GameObjectList)
            {
                obj.Draw(spriteBatch);
            }

            wheel.Draw(spriteBatch);

            //spriteBatch.DrawString(font, "Score: " + gearsCollected, new Vector2(20, 20), Color.Black);

            // Stop drawing

            spriteBatch.End();

            base.Draw(gameTime);
        }

        private void HandleInput()
        {
            KeyboardState state = Keyboard.GetState();
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || state.IsKeyDown(Keys.Escape))
                Exit();
            if(state.IsKeyDown(Keys.Enter))
            {
                if(!pausedPressed)
                {
                    if(paused)
                    {
                        paused = false;
                    }
                    else
                    {
                        paused = true;
                    }
                }
                pausedPressed = true;
            }
            if(state.IsKeyUp(Keys.Enter))
            {
                pausedPressed = false;
            }
            if(state.IsKeyDown(Keys.End))
            {
                ResetGame();
            }
        }

        public void PlaceObject(Sprite.SpriteType type)
        {
            foreach (Sprite obj in GameObjectList)
            {
                if (obj._Tag != type) continue;
                if (obj._CurrentState != Sprite.SpriteState.kStateInActive) continue;
                obj.Activate();
                return;
            }
            //sprite not found. gotta create one.
        }

        public void PlaceObject(Sprite.SpriteType type, Vector2 pos)
        {
            foreach (Sprite obj in GameObjectList)
            {
                if (obj._Tag != type) continue;
                if (obj._CurrentState != Sprite.SpriteState.kStateInActive) continue;
                obj.Activate(pos);
                return;
            }
            //sprite not found. gotta create one.
        }

        public void PlacePattern(string name)
        {
            if(name.Equals("gear>"))
            {
                PlaceObject(Sprite.SpriteType.kGearType, new Vector2(20, -30));
                PlaceObject(Sprite.SpriteType.kGearType, new Vector2(80, -70));
                PlaceObject(Sprite.SpriteType.kGearType, new Vector2(140, -110));
                PlaceObject(Sprite.SpriteType.kGearType, new Vector2(200, -150));
                PlaceObject(Sprite.SpriteType.kGearType, new Vector2(260, -190));
                PlaceObject(Sprite.SpriteType.kGearType, new Vector2(200, -230));
                PlaceObject(Sprite.SpriteType.kGearType, new Vector2(140, -270));
                PlaceObject(Sprite.SpriteType.kGearType, new Vector2(80, -310));
                PlaceObject(Sprite.SpriteType.kGearType, new Vector2(20, -350));
            }
            else if(name.Equals("CGC"))
            {
                PlaceObject(Sprite.SpriteType.kCarType, new Vector2(80, -30));
                PlaceObject(Sprite.SpriteType.kCarType, new Vector2(80, -90));
                PlaceObject(Sprite.SpriteType.kCarType, new Vector2(80, -150));
                PlaceObject(Sprite.SpriteType.kCarType, new Vector2(80, -210));
                PlaceObject(Sprite.SpriteType.kGearType, new Vector2(160, -60));
                PlaceObject(Sprite.SpriteType.kGearType, new Vector2(160, -90));
                PlaceObject(Sprite.SpriteType.kGearType, new Vector2(160, -120));
                PlaceObject(Sprite.SpriteType.kCarType, new Vector2(200, -30));
                PlaceObject(Sprite.SpriteType.kCarType, new Vector2(200, -90));
                PlaceObject(Sprite.SpriteType.kCarType, new Vector2(200, -150));
                PlaceObject(Sprite.SpriteType.kCarType, new Vector2(200, -210));
            }
            else if(name.Equals("Hi"))
            {
                PlaceObject(Sprite.SpriteType.kGearType, new Vector2(20, -20));
                PlaceObject(Sprite.SpriteType.kGearType, new Vector2(20, -60));
                PlaceObject(Sprite.SpriteType.kGearType, new Vector2(20, -100));
                PlaceObject(Sprite.SpriteType.kGearType, new Vector2(20, -140));
                PlaceObject(Sprite.SpriteType.kGearType, new Vector2(20, -180));
                PlaceObject(Sprite.SpriteType.kGearType, new Vector2(140, -20));
                PlaceObject(Sprite.SpriteType.kGearType, new Vector2(140, -60));
                PlaceObject(Sprite.SpriteType.kGearType, new Vector2(140, -100));
                PlaceObject(Sprite.SpriteType.kGearType, new Vector2(140, -140));
                PlaceObject(Sprite.SpriteType.kGearType, new Vector2(140, -180));
                PlaceObject(Sprite.SpriteType.kGearType, new Vector2(260, -20));
                PlaceObject(Sprite.SpriteType.kGearType, new Vector2(260, -60));
                PlaceObject(Sprite.SpriteType.kGearType, new Vector2(260, -100));
                PlaceObject(Sprite.SpriteType.kGearType, new Vector2(260, -140));
                PlaceObject(Sprite.SpriteType.kGearType, new Vector2(260, -180));
                PlaceObject(Sprite.SpriteType.kGearType, new Vector2(60, -100));
                PlaceObject(Sprite.SpriteType.kGearType, new Vector2(100, -100));
                PlaceObject(Sprite.SpriteType.kGearType, new Vector2(220, -20));
                PlaceObject(Sprite.SpriteType.kGearType, new Vector2(300, -200));
                PlaceObject(Sprite.SpriteType.kGearType, new Vector2(220, -180));
                PlaceObject(Sprite.SpriteType.kGearType, new Vector2(300, -180));
            }
            else if (name.Equals("carV"))
            {
                PlaceObject(Sprite.SpriteType.kCarType, new Vector2(80, -80));
                PlaceObject(Sprite.SpriteType.kCarType, new Vector2(120, -60));
                PlaceObject(Sprite.SpriteType.kCarType, new Vector2(160, -40));
                PlaceObject(Sprite.SpriteType.kCarType, new Vector2(200, -60));
                PlaceObject(Sprite.SpriteType.kCarType, new Vector2(240, -80));
            }
            else if (name.Equals("giantRock"))
            {
                PlaceObject(Sprite.SpriteType.kRockType, new Vector2(160, -40));

                PlaceObject(Sprite.SpriteType.kRockType, new Vector2(160, -30));
                PlaceObject(Sprite.SpriteType.kRockType, new Vector2(150, -30));
                PlaceObject(Sprite.SpriteType.kRockType, new Vector2(170, -30));

                PlaceObject(Sprite.SpriteType.kRockType, new Vector2(160, -20));
                PlaceObject(Sprite.SpriteType.kRockType, new Vector2(150, -20));
                PlaceObject(Sprite.SpriteType.kRockType, new Vector2(140, -20));
                PlaceObject(Sprite.SpriteType.kRockType, new Vector2(170, -20));
                PlaceObject(Sprite.SpriteType.kRockType, new Vector2(180, -20));
            }
        }

        private void ResetGame()
        {
            foreach(Sprite obj in GameObjectList)
            {
                obj.ResetSelf();
            }
            this.UnloadContent();
            if (GameObjectList.Count >= 1)
            {
                GameObjectList.Clear();
            }
            paused = false;
            pausedPressed = false;
            playerSpeed = 0f;
            midPoint = GraphicsDevice.Viewport.Width / 2;
            //moveRight = false;
            //moveLeft = false;
            Ranum = new Random();
            this.LoadContent();
        }
    }
}
