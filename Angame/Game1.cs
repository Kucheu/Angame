using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using System;

namespace Angame
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        //obkiety tekstur okna gry
        private Texture2D potato;
        private bool potatoisclicked = false;
        private Texture2D potatoclick;
        private Texture2D background;
        private Texture2D buybutton;

        //obiekty fontow 
        private SpriteFont points;
        private SpriteFont buyscreenprice;
        private SpriteFont buyscreencount;


        //obiekty okienek ekranu sklepu
        private Texture2D buyscreen1;
        private bool buyscreen1isclicked = false;
        private Texture2D buyscreen2;
        private bool buyscreen2isclicked = false;
        private Texture2D buyscreen3;
        private bool buyscreen3isclicked = false;
        private Texture2D buyscreen4;
        private bool buyscreen4isclicked = false;
        private Texture2D buyscreen5;
        private bool buyscreen5isclicked = false;

        private Texture2D backbutton;

       
        // stworzenie stanu gry
        private Upgrades statusGry = Upgrades.LoadGame();
        
        //
        int klatki = 0;
        int zapis = 0;
        bool zapise = false;
        private Song songtheme;
        
        
        private enum Gamestate
        {
            Gameplay,
            BuyScreen,
        }


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.IsFullScreen = true;
            graphics.PreferredBackBufferWidth = 0;
            graphics.PreferredBackBufferHeight = 0; ;
            graphics.ApplyChanges();
        }

        //status okna
        Gamestate _state = Gamestate.Gameplay;

        
        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

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

            //za³adowanie tekstur g³ównego okna
            potato = Content.Load<Texture2D>("potato");
            potatoclick = Content.Load<Texture2D>("Potato/potatoclick2");
            background = Content.Load<Texture2D>("backgorund");
            


            

            //muzyka
            songtheme = Content.Load<Song>("Duet_Musette");

            //za³adowanie ustawieñ fontów
            buyscreencount = Content.Load<SpriteFont>("BuyScreen/countfont");
            points = Content.Load<SpriteFont>("default");
            buyscreenprice = Content.Load<SpriteFont>("BuyScreen/pricefont");


            //za³adowanie okno sklepu
            buyscreen1 = Content.Load<Texture2D>("BuyScreen/ramka2");
            buyscreen2 = Content.Load<Texture2D>("BuyScreen/ramka3");
            buyscreen3 = Content.Load<Texture2D>("BuyScreen/ramka1");
            buyscreen4 = Content.Load<Texture2D>("BuyScreen/ramka5");
            buyscreen5 = Content.Load<Texture2D>("BuyScreen/ramka4");
            //przycisk sklepu
            buybutton = Content.Load<Texture2D>("shopbutton");
            backbutton = Content.Load<Texture2D>("BuyScreen/cofnij");


            //ustawienia muzyki
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Play(songtheme);
            // TODO: use this.Content to load your game content here
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
            //sprawdzanie minionego czasu
            klatki++;
            if (klatki >= 60)
            {
                klatki -= 60;
                statusGry.Idle();
                zapis++;
            }
            if(zapis >= 5)
            {
                
               if( !Upgrades.SaveGame(statusGry))
                {
                    zapise = true;
                }
                
            }
            //sprawdzanie aktywnego okna
            switch (_state)
            {
                case Gamestate.Gameplay:
                    
                    UpdateGameplay(gameTime);
                break;
                case Gamestate.BuyScreen:
                    UpdateBuyScreen(gameTime);
                break;
            }
        }

        void UpdateGameplay(GameTime gameTime) //okno gry
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                Exit();

            TouchCollection touch = TouchPanel.GetState();
            
            foreach (TouchLocation t in touch)
            {
                if (t.State != TouchLocationState.Released)
                continue;

                float scaleX = (float)GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width / 480;
                float scaleY = (float)GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height / 800;

                if ( t.Position.Y > 700 * scaleY )
                {
                    _state = Gamestate.BuyScreen;
                }

                if (t.Position.X > 130 && t.Position.X < 650 && t.Position.Y > 360 && t.Position.Y < 800)
                {
                    statusGry.Click();
                    potatoisclicked = true;
                }
                
                if (!t.TryGetPreviousLocation(out TouchLocation prevLoc) || prevLoc.State != TouchLocationState.Moved)
                    continue;

                var delta = t.Position - prevLoc.Position;

                if (delta.LengthSquared() < 50)
                    continue;

                if (delta.X < 0)
                    _state = Gamestate.BuyScreen;
            }
            base.Update(gameTime);
        }

        void UpdateBuyScreen(GameTime gameTime) //okno sklepu
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                Exit();
            TouchCollection touch = TouchPanel.GetState();
            foreach (TouchLocation t in touch)
            {
                if (t.State != TouchLocationState.Released)
                    continue;

                float scaleX = (float)GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width / 480;
                float scaleY = (float)GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height / 800;

                if ( t.Position.X < 50 * scaleX && t.Position.Y > 750 * scaleY)
                {
                    _state = Gamestate.Gameplay;
                }

                    if (t.Position.X > 49 * scaleX && t.Position.X < 451 * scaleX && t.Position.Y > 29 * scaleY && t.Position.Y < 131 * scaleY)
                {
                    buyscreen1isclicked = true;
                    if (statusGry.Pieniadze >= statusGry.MotykaCena)
                    {
                        statusGry.KupMotyka();
                        statusGry.KosztMotyka(statusGry.Motyka);
                    }
                }

                if (t.Position.X > 49 * scaleX && t.Position.X < 451 * scaleX && t.Position.Y > 159 * scaleY && t.Position.Y < 261 * scaleY)
                {
                    buyscreen2isclicked = true;
                    if (statusGry.Pieniadze >= statusGry.NawozCena)
                    {
                        statusGry.KupNawoz();
                        statusGry.KosztNawoz(statusGry.Nawoz);
                    }
                }

                if (t.Position.X > 49 * scaleX && t.Position.X < 451 * scaleX && t.Position.Y > 289 * scaleY && t.Position.Y < 391 * scaleY)
                {
                    buyscreen3isclicked = true;
                    if (statusGry.Pieniadze >= statusGry.RolnikCena)
                    {
                        statusGry.KupRolnik();
                        statusGry.KosztRolnik(statusGry.Rolnik);
                    }
                }

                if (t.Position.X > 49 * scaleX && t.Position.X < 451 * scaleX && t.Position.Y > 419 * scaleY && t.Position.Y < 521 * scaleY)
                {
                    buyscreen4isclicked = true;
                    if (statusGry.Pieniadze >= statusGry.OpryskiCena)
                    {
                        statusGry.KupOpryski();
                        statusGry.KosztOpryski(statusGry.Opryski);
                    }
                }

                if (t.Position.X > 49 * scaleX && t.Position.X < 451 * scaleX && t.Position.Y > 549 * scaleY && t.Position.Y < 651 * scaleY)
                {
                    buyscreen5isclicked = true;
                    if (statusGry.Pieniadze >= statusGry.TraktorCena)
                    {
                        statusGry.KupTraktor();
                        statusGry.KosztTraktor(statusGry.Traktor);
                    }
                }

                if (!t.TryGetPreviousLocation(out TouchLocation prevLoc) || prevLoc.State != TouchLocationState.Moved)
                    continue;

                var delta = t.Position - prevLoc.Position;

                if (delta.LengthSquared() < 50)
                    continue;

                if (delta.X > 0)
                    _state = Gamestate.Gameplay; ;
            }
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            switch (_state)
            {
                case Gamestate.Gameplay:
                    DrawGameplay(gameTime);
                    break;
                case Gamestate.BuyScreen:
                    DrawBuyScreen(gameTime);
                    break;
            }
        }

        void DrawGameplay(GameTime gameTime)
        {
            
            var scaleX = (float)GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width / 480;
            var scaleY = (float)GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height / 800;
            var matrix = Matrix.CreateScale(scaleX, scaleY, 1.0f);

            
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin(transformMatrix: matrix);
            spriteBatch.Draw(background, Vector2.Zero, Color.White);
            Vector2 potatoCenter = new Vector2(240 - (potato.Width / 2), 400 - (potato.Height) / 2);
            if (potatoisclicked == false)
            {
                spriteBatch.Draw(potato, potatoCenter, Color.White);
            }
            else
            {
                spriteBatch.Draw(potatoclick, potatoCenter, Color.White);
                potatoisclicked = false;
            }

            // Stan pieniedzy
            Vector2 pointsCenter = new Vector2(240 - (points.MeasureString(statusGry.Pieniadze.ToString()).X) / 2, 0f);
            
            
            spriteBatch.DrawString(points, statusGry.Pieniadze.ToString(), pointsCenter, Color.Black);

            // Przychód pieniedzy na sekunde
            pointsCenter = new Vector2(240 - (points.MeasureString("$" + statusGry.Pieniadzeidle.ToString() + "/s").X ) / 2, 700 - points.MeasureString(statusGry.Pieniadzeidle.ToString()).Y);
            spriteBatch.DrawString(points, "$" + statusGry.Pieniadzeidle.ToString() + "/s", pointsCenter, Color.Black);
            //Button sklepu
            spriteBatch.Draw(buybutton, new Vector2(0, 700), Color.White);
            spriteBatch.End();
            base.Draw(gameTime);
        }

        void DrawBuyScreen(GameTime gameTime)
        {
            
            var scaleX = (float)GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width / 480;
            var scaleY = (float)GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height / 800;
            var matrix = Matrix.CreateScale(scaleX, scaleY, 1.0f);
            GraphicsDevice.Clear(Color.Wheat);
            spriteBatch.Begin(transformMatrix: matrix);
            //motyka
            if (buyscreen1isclicked)
            {
                buyscreen1isclicked = false;
            }
            else
            {
                spriteBatch.Draw(buyscreen1, new Vector2(49, 29), Color.White);
                spriteBatch.DrawString(buyscreenprice, "$" + statusGry.MotykaCena.ToString(), new Vector2(268, 80), Color.Black);
                spriteBatch.DrawString(buyscreencount, statusGry.Motyka.ToString(), new Vector2(145, 40), Color.Black);
            }
            //nawóz
            if (buyscreen2isclicked)
            {
                buyscreen2isclicked = false;
            }
            else
            {
                spriteBatch.Draw(buyscreen2, new Vector2(49, 159), Color.White);
                spriteBatch.DrawString(buyscreenprice, "$" + statusGry.NawozCena.ToString(), new Vector2(268, 210), Color.Black);
                spriteBatch.DrawString(buyscreencount, statusGry.Nawoz.ToString(), new Vector2(145, 170), Color.Black);

            }
            //rolnik
            if (buyscreen3isclicked)
            {
                buyscreen3isclicked = false;
            }
            else
            {
                spriteBatch.Draw(buyscreen3, new Vector2(49, 289), Color.White);
                spriteBatch.DrawString(buyscreenprice, "$" + statusGry.RolnikCena.ToString(), new Vector2(268, 340), Color.Black);
                spriteBatch.DrawString(buyscreencount, statusGry.Rolnik.ToString(), new Vector2(145, 300), Color.Black);
            }
            //opryski
            if (buyscreen4isclicked)
            {
                buyscreen4isclicked = false;
            }
            else
            {
                spriteBatch.Draw(buyscreen4, new Vector2(49, 419), Color.White);
                spriteBatch.DrawString(buyscreenprice, "$" + statusGry.OpryskiCena.ToString(), new Vector2(268, 470), Color.Black);
                spriteBatch.DrawString(buyscreencount, statusGry.Opryski.ToString(), new Vector2(145, 430), Color.Black);
            }
            //traktor
            if (buyscreen5isclicked)
            {
                buyscreen5isclicked = false;
            }
            else
            {
                spriteBatch.Draw(buyscreen5, new Vector2(49, 549), Color.White);
                spriteBatch.DrawString(buyscreenprice, "$" + statusGry.TraktorCena.ToString(), new Vector2(268, 600), Color.Black);
                spriteBatch.DrawString(buyscreencount, statusGry.Traktor.ToString(), new Vector2(145, 560), Color.Black);
            }
            Vector2 pointsCenter = new Vector2(240 - (points.MeasureString(statusGry.Pieniadze.ToString()).X) / 2, 800-points.MeasureString(statusGry.Pieniadze.ToString()).Y);
            spriteBatch.DrawString(points, "$" + statusGry.Pieniadze.ToString(), pointsCenter, Color.Black);

            spriteBatch.Draw(backbutton, new Vector2(0, 750), Color.White);
            //cofnij


            base.Draw(gameTime);
            spriteBatch.End();
        }
    }
}
