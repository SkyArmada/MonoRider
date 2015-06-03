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
        bool moveRight = false;
        bool moveLeft = false;
        bool moveCenter = false;
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

                player.LoadContent("Graphics\\car2", CM_Play);
                player._Position = new Vector2(GraphicsDevice.Viewport.Width / 2, 320);
                GameObjectList.Add(player);

                wheel.LoadContent("Graphics\\wheel", CM_Play);
                wheel._Position = new Vector2(160, 520);
                
                for (int i = 0; i < 30; i++)
                {
                    Gear gear = new Gear();
                    gear.LoadContent("Graphics\\gear1", CM_Play);
                    gear._Position = new Vector2(Ranum.Next(320), -10 * Ranum.Next(250));

                    if (i > 3)
                    {
                        gear._CurrentState = Sprite.SpriteState.kStateDead;
                    }
                    GameObjectList.Add(gear);
                }

                for (int i = 0; i <= 30; i++)
                {
                    EnemyCar car = new EnemyCar();
                    car.LoadContent("Graphics\\car2", CM_Play);

                    if (i < 6)
                    {
                        car._CurrentState = Sprite.SpriteState.kStateActive;
                        car._Position.Y = -10 * Ranum.Next(250);
                        if (Ranum.Next(0, 2) == 0)
                        {
                            car._Position.X = midPoint + Ranum.Next(85 - car._Texture.Width/2);
                        }
                        else
                        {
                            car._Position.X = midPoint - Ranum.Next(85 + car._Texture.Width/2);
                        }
                        //car.ChangeColor(new Color(213, 255, 28, 255), new Color(num.Next(255),num.Next(255),num.Next(255),255));
                    }
                    else
                    {
                        car._CurrentState = Sprite.SpriteState.kStateInActive;
                        car._Position = new Vector2(-500, -500);
                    }
                    GameObjectList.Add(car);
                }
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
                // TODO: Add your update logic here
                foreach (Sprite obj in GameObjectList)
                {
                    bool createGear = false;
                    if(Ranum.Next(0,600) == 0)
                    {
                        createGear = true;
                    }
                    if(createGear && obj._Tag.Equals("gear") && obj._CurrentState == Sprite.SpriteState.kStateDead)
                    {
                        obj.Live();
                        createGear = false;
                    }
                    if(obj._Tag.Equals("player"))
                    {
                        playerSpeed = obj.speed;
                    }
                    else
                    {
                        obj.speed = playerSpeed;
                    }
                    if(obj._Tag.Equals("enemycar"))
                    {
                        if(moveRight)
                        {
                            obj._Position.X++;
                        }
                        else if(moveLeft)
                        {
                            obj._Position.X--;
                        }
                        else if(moveCenter)
                        {
                            if(obj._Center.X < GraphicsDevice.Viewport.Width/2)
                            {
                                obj._Position.X++;
                            }
                            else if(obj._Center.X > GraphicsDevice.Viewport.Width/2)
                            {
                                obj._Position.X--;
                            }
                        }
                    }
                    obj.midpoint = midPoint;
                    obj.Update(gameTime, GameObjectList);
                }
                wheel.Update(gameTime, player.momentum);
                if(Ranum.Next(0,1200) == 0 && moveRight == false)
                {
                    moveRight = true;
                }
                if(Ranum.Next(0,1200) == 0 && moveLeft == false)
                {
                    moveLeft = true;
                }
                if (Ranum.Next(0, 1200) == 0 && moveCenter == false)
                {
                    moveCenter = true;
                }
                if(moveRight)
                {
                    int test = (midPoint + 85);
                    if( test <= GraphicsDevice.Viewport.Width)
                    {
                        midPoint++;
                    }
                    else
                    {
                        moveRight = false;
                    }
                }
                else if(moveLeft)
                {
                    int test = (midPoint - 85);
                    if ( test >= 0)
                    {
                        midPoint--;
                    }
                    else
                    {
                        moveLeft = false;
                    }
                }
                else if(moveCenter)
                {
                    if(midPoint < GraphicsDevice.Viewport.Width/2)
                    {
                        midPoint++;
                    }
                    else if(midPoint > GraphicsDevice.Viewport.Width/2)
                    {
                        midPoint--;
                    }
                    else if(midPoint == GraphicsDevice.Viewport.Width/2)
                    {
                        moveCenter = false;
                    }
                }
                base.Update(gameTime);
            }
            if(player._CurrentState == Sprite.SpriteState.kStateDead)
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
            midPoint = -85;
            moveRight = false;
            moveLeft = false;
            Ranum = new Random();
            this.LoadContent();
        }
    }
}
