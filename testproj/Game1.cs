using System.Collections.Generic;
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MonoRider
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Player player;
        List<GameCharacterBase> GameObjectList;
        Texture2D background;
        SteeringWheel wheel;
        bool debug = false;
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 320;
            graphics.PreferredBackBufferHeight = 480;
            graphics.ApplyChanges();
            Content.RootDirectory = "Content";
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
            GameObjectList = new List<GameCharacterBase>();
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            background = Content.Load<Texture2D>("Graphics\\grassBackground");
            Vector2 playerPosition = new Vector2(GraphicsDevice.Viewport.Width / 2, 320);
            Vector2 wheelPos = new Vector2(160, 520);

            player.Initialize(Content.Load<Texture2D>("Graphics\\car2"), playerPosition);
            GameObjectList.Add(player);
            wheel.Initialize(Content.Load<Texture2D>("Graphics\\wheel"), wheelPos);

            Random num = new Random();

            for (int i = 0; i < 30; i++)
            {
                Gear gear = new Gear();
                float gearXPos = num.Next(320);
                Vector2 gearPos;
                gearPos.X = gearXPos;
                gearPos.Y = -10 * num.Next(250);
                gear.Initialize(Content.Load<Texture2D>("Graphics\\gear1"), gearPos);

                if(i > 10)
                {
                    gear._Active = false;
                }
                GameObjectList.Add(gear);
            }

            for(int i = 0; i <= 30; i++)
            {
                EnemyCar car = new EnemyCar();
                float carXPos = num.Next(320);
                Vector2 carPos = new Vector2(carXPos, -10 * num.Next(250));
                car.Initialize(Content.Load<Texture2D>("Graphics//car2"), carPos);

                if(i < 6)
                {
                    car._Active = true;
                    //car.ChangeColor(new Color(213, 255, 28, 255), new Color(num.Next(255),num.Next(255),num.Next(255),255));
                }
                else
                {
                    car._Active = false;
                }
                GameObjectList.Add(car);
            }
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            wheel.Update(gameTime, player);

            foreach (GameCharacterBase obj in GameObjectList)
            {
                if (obj._Active)
                {
                    obj.Update(gameTime, GameObjectList);
                }
            }

            base.Update(gameTime);
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
            }
            spriteBatch.Draw(background, new Rectangle(0, 0, 320, 480), Color.White);


            // Draw the Player
            foreach (GameCharacterBase obj in GameObjectList)
            {
                obj.Draw(spriteBatch);
            }

            wheel.Draw(spriteBatch);
            // Stop drawing

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
