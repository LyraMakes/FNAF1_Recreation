using System;
using System.Collections.Generic;
using System.Reflection;
using System.IO;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using FNAF1_Recreation.Animatronics;

namespace FNAF1_Recreation
{
    public enum GameState { INTRO, MENU, NIGHT, NIGHT_INTRO ,CLOCK, FINISH, EVENT }

    public class Game1 : Game
    {
        private Rand rand;
        private UI ui;

        public GameState gameState;

        Animatronic[] animatronics;
        SpriteFont textFont;
        SpriteFont titleFont;

        SoundEffect titleSong;
        SoundEffect menuSelectionSFX;
        SoundEffect titleStatic;

        private readonly GraphicsDeviceManager _graphics;
        private SpriteBatch[] _spriteLayer;

        private int introAlpha;
        private int loadAlpha;
        private int trueNight;
        private double startTime;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            //Resolution Fixing
            _graphics.PreferredBackBufferWidth = 1280;
            _graphics.PreferredBackBufferHeight = 720;
            _graphics.ApplyChanges();
        }

        protected override void Initialize()
        {
            //Initializes the Program-Wide RNG
            int seed = (int)DateTime.UtcNow.ToFileTimeUtc();
            rand = new Rand(seed);

            ui = new UI();

            Input.Initialize();
            Input.Latch();

            gameState = GameState.INTRO;
            introAlpha = 0;
            

            // Create Static Arrays and Lists
            Audio.channels = new List<AudioChannel>();
            _spriteLayer = new SpriteBatch[5];
            
            Camera._onTex = new Texture2D[11];
            Camera._offTex = new Texture2D[11];
            
            TitleScreen._texMap = new Texture2D[4];
            TitleScreen._staticTexMap = new Texture2D[8];
            TitleScreen._staticBTexMap = new Texture2D[7];
            TitleScreen.menuOptions = new List<string>();
            
            InitTitleMenu();


            //Initialize Rooms
            Room.rooms = new Room[15];
            Room.rooms[0] = new Room("Show Stage");
            Room.rooms[1] = new Room("Dining Area");
            Room.rooms[2] = new Room("Pirate Cove");
            Room.rooms[3] = new Room("West Hall");
            Room.rooms[4] = new Room("W. Hall Corner");
            Room.rooms[5] = new Room("Supply Closet");
            Room.rooms[6] = new Room("East Hall");
            Room.rooms[7] = new Room("E. Hall Corner");
            Room.rooms[8] = new Room("Backstage");
            Room.rooms[9] = new Room("Kitchen");
            Room.rooms[10] = new Room("Restrooms");

            //Non-Visible
            Room.rooms[11] = new Room("Left Door");
            Room.rooms[12] = new Room("Right Door");
            Room.rooms[13] = new Room("Left Office In");
            Room.rooms[14] = new Room("Right Office In");


            //Animatronic Setup
            animatronics = new Animatronic[4];
            animatronics[0] = new Animatronic("Freddy", 1, new FreddyBehavior(), rand);
            animatronics[1] = new Animatronic("Bonnie", 2, new BonnieBehavior(rand), rand);
            animatronics[2] = new Animatronic("Chica", 3, new ChicaBehavior(), rand);
            animatronics[3] = new Animatronic("Foxy", 4, new FoxyBehavior(), rand);

            animatronics[0].SetMovemetOffset(3.02);
            animatronics[1].SetMovemetOffset(4.97);
            animatronics[2].SetMovemetOffset(4.98);
            animatronics[3].SetMovemetOffset(5.01);


            // Check if save file is present
            if(!File.Exists(GetSavePath()))
            {
                //If not, create one
                Save();
            }
            else
            {
                //If so, load it and refresh title menu
                Load();
                InitTitleMenu();
            }

            trueNight = TitleScreen.currentNight;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Create Each Sprite Layer
            _spriteLayer[0] = new SpriteBatch(GraphicsDevice);
            _spriteLayer[1] = new SpriteBatch(GraphicsDevice);
            _spriteLayer[2] = new SpriteBatch(GraphicsDevice);
            _spriteLayer[3] = new SpriteBatch(GraphicsDevice);
            _spriteLayer[4] = new SpriteBatch(GraphicsDevice);
            
            // Fonts
            textFont = Content.Load<SpriteFont>("FNAFTitleFont");
            titleFont = Content.Load<SpriteFont>("FNAFBigTitleFont");
            
            // Title Textures
            TitleScreen._texMap[0] = Content.Load<Texture2D>("Images\\MM-1");
            TitleScreen._texMap[1] = Content.Load<Texture2D>("Images\\MM-2");
            TitleScreen._texMap[2] = Content.Load<Texture2D>("Images\\MM-3");
            TitleScreen._texMap[3] = Content.Load<Texture2D>("Images\\MM-4");
            TitleScreen.bgTex = TitleScreen._texMap[0];

            TitleScreen._staticTexMap[0] = Content.Load<Texture2D>("Images\\staticOverlay1");
            TitleScreen._staticTexMap[1] = Content.Load<Texture2D>("Images\\staticOverlay2");
            TitleScreen._staticTexMap[2] = Content.Load<Texture2D>("Images\\staticOverlay3");
            TitleScreen._staticTexMap[3] = Content.Load<Texture2D>("Images\\staticOverlay4");
            TitleScreen._staticTexMap[4] = Content.Load<Texture2D>("Images\\staticOverlay5");
            TitleScreen._staticTexMap[5] = Content.Load<Texture2D>("Images\\staticOverlay6");
            TitleScreen._staticTexMap[6] = Content.Load<Texture2D>("Images\\staticOverlay7");
            TitleScreen._staticTexMap[7] = Content.Load<Texture2D>("Images\\staticOverlay8");
            TitleScreen.staticTex = TitleScreen._staticTexMap[0];

            TitleScreen._staticBTexMap[0] = Content.Load<Texture2D>("Images\\staticOverlayB1");
            TitleScreen._staticBTexMap[1] = Content.Load<Texture2D>("Images\\staticOverlayB2");
            TitleScreen._staticBTexMap[2] = Content.Load<Texture2D>("Images\\staticOverlayB3");
            TitleScreen._staticBTexMap[3] = Content.Load<Texture2D>("Images\\staticOverlayB4");
            TitleScreen._staticBTexMap[4] = Content.Load<Texture2D>("Images\\staticOverlayB5");
            TitleScreen._staticBTexMap[5] = Content.Load<Texture2D>("Images\\staticOverlayB6");
            TitleScreen._staticBTexMap[6] = Content.Load<Texture2D>("Images\\staticOverlayB7");
            TitleScreen.staticBTex = TitleScreen._staticBTexMap[0];

            TitleScreen.staticLineTex     = Content.Load<Texture2D>("Images\\staticBar");


            // Camera Textures
            Camera._onTex[0]   = Content.Load<Texture2D>("Images\\CAM-1A-ON");
            Camera._onTex[1]   = Content.Load<Texture2D>("Images\\CAM-1B-ON");
            Camera._onTex[2]   = Content.Load<Texture2D>("Images\\CAM-1C-ON");
            Camera._onTex[3]   = Content.Load<Texture2D>("Images\\CAM-2A-ON");
            Camera._onTex[4]   = Content.Load<Texture2D>("Images\\CAM-2B-ON");
            Camera._onTex[5]   = Content.Load<Texture2D>("Images\\CAM-3-ON" );
            Camera._onTex[6]   = Content.Load<Texture2D>("Images\\CAM-4A-ON");
            Camera._onTex[7]   = Content.Load<Texture2D>("Images\\CAM-4B-ON");
            Camera._onTex[8]   = Content.Load<Texture2D>("Images\\CAM-5-ON" );
            Camera._onTex[9]   = Content.Load<Texture2D>("Images\\CAM-6-ON" );
            Camera._onTex[10]  = Content.Load<Texture2D>("Images\\CAM-7-ON" );
            
            Camera._offTex[0]  = Content.Load<Texture2D>("Images\\CAM-1A-OFF");
            Camera._offTex[1]  = Content.Load<Texture2D>("Images\\CAM-1B-OFF");
            Camera._offTex[2]  = Content.Load<Texture2D>("Images\\CAM-1C-OFF");
            Camera._offTex[3]  = Content.Load<Texture2D>("Images\\CAM-2A-OFF");
            Camera._offTex[4]  = Content.Load<Texture2D>("Images\\CAM-2B-OFF");
            Camera._offTex[5]  = Content.Load<Texture2D>("Images\\CAM-3-OFF" );
            Camera._offTex[6]  = Content.Load<Texture2D>("Images\\CAM-4A-OFF");
            Camera._offTex[7]  = Content.Load<Texture2D>("Images\\CAM-4B-OFF");
            Camera._offTex[8]  = Content.Load<Texture2D>("Images\\CAM-5-OFF" );
            Camera._offTex[9]  = Content.Load<Texture2D>("Images\\CAM-6-OFF" );
            Camera._offTex[10] = Content.Load<Texture2D>("Images\\CAM-7-OFF" );

            // UI Textures
            ui.loadTex         = Content.Load<Texture2D>("Icons\\loading");

            // Audio
            titleSong        = Content.Load<SoundEffect>("Audio\\titleScreen");
            menuSelectionSFX = Content.Load<SoundEffect>("Audio\\blip1");
            titleStatic      = Content.Load<SoundEffect>("Audio\\static2");
        }

        protected override void Update(GameTime gameTime)
        {
            Input.Latch();
            if (Input.GetKeyDown(Keys.Escape)) Exit();

            // Determine Game Loop based on gameState
            switch (gameState)
            {
                case GameState.INTRO:
                    UpdateIntro(gameTime);
                    break;
                case GameState.MENU:
                    UpdateTitleScreen(gameTime);
                    break;
                case GameState.NIGHT_INTRO:
                    UpdateNightIntro(gameTime);
                    break;
                case GameState.NIGHT:
                    break;
                case GameState.CLOCK:
                    break;
                case GameState.FINISH:
                    break;
                case GameState.EVENT:
                    break;
                default:
                    break;
            }

            // Clean Up unused resources:
            Audio.Clean(); // Removes any stopped channels from the queue

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // Determine Game Loop based on gameState
            switch (gameState)
            {
                case GameState.INTRO:
                    DrawIntro();
                    break;
                case GameState.MENU:
                    DrawTitle();
                    break;
                case GameState.NIGHT_INTRO:
                    DrawNightIntro();
                    break;
                case GameState.NIGHT:
                    DrawNight();
                    break;
                case GameState.CLOCK:
                    break;
                case GameState.FINISH:
                    break;
                case GameState.EVENT:
                    break;
                default:
                    break;
            }

            base.Draw(gameTime);
        }

        private void UpdateIntro(GameTime gameTime)
        {
            // Controls the fade of the opening disclaimer
            if (1 <= gameTime.TotalGameTime.TotalSeconds && gameTime.TotalGameTime.TotalSeconds <= 2 && introAlpha < 100) introAlpha += 10;
            if (2 <= gameTime.TotalGameTime.TotalSeconds && gameTime.TotalGameTime.TotalSeconds <= 3 && introAlpha < 100) introAlpha = 100;
            if (3 <= gameTime.TotalGameTime.TotalSeconds && gameTime.TotalGameTime.TotalSeconds <= 4 && introAlpha > 0  ) introAlpha -= 10;

            // After 5 Seconds, Go to menu
            if (gameTime.TotalGameTime.TotalSeconds >= 5)
            {
                gameState = GameState.MENU;
                Audio.Play(titleStatic);
                Audio.Play(titleSong, true);
            }
        }

        private void UpdateTitleScreen(GameTime gameTime)
        {
            // Randomize Static Texture
            TitleScreen.staticTex = TitleScreen._staticTexMap[rand.RandInt(0, 7)];

            // Decrement timer for bg changing
            if (TitleScreen.texChanged > 0) TitleScreen.texChanged--;

            TitleScreen.barPos++;
            if (TitleScreen.barPos > _graphics.PreferredBackBufferHeight + 20) TitleScreen.barPos = -20;

            // These next two if statements handle the overlay static on the title screen
            if (rand.RandInt(0, 500) < 3 && !TitleScreen.staticOn)
            {
                TitleScreen.staticTimer = 30;
                TitleScreen.staticOn = true;
            }

            if (TitleScreen.staticOn)
            {
                TitleScreen.staticTimer--;

                if (rand.RandInt(0, 4) == 0)
                    TitleScreen.staticBTex = TitleScreen._staticBTexMap[TitleScreen.staticTimer % TitleScreen._staticBTexMap.Length];

                TitleScreen.staticOn = TitleScreen.staticTimer > 0;
            }
            
            
            int indx = rand.RandInt(0, 300);
            indx = (indx >= TitleScreen._texMap.Length) ? 0 : indx;

            // If Background is ready to change and is going to be different...
            if (TitleScreen.texChanged == 0 && TitleScreen.bgTex != TitleScreen._texMap[indx])
            {
                // Then change and reset the timer
                TitleScreen.bgTex = TitleScreen._texMap[indx];
                TitleScreen.texChanged = 10;
            }

            // Iterate through each Menu option and Check for Mouse Hover and Clicks
            for (int i = 0; i < TitleScreen.menuOptions.Count; i++)
            {
                // res would be whether the mouse is above menu option "i"
                bool res = Collide(Input.GetMousePos(), 200, 400 + i * 80, 300, 50);
                if (res)
                {
                    if (TitleScreen.selected != i)
                    {
                        TitleScreen.selected = i;
                        menuSelectionSFX.Play();
                    }

                    // OnClick handler
                    if (Input.GetMouseDown())
                    {
                        if (i == 0) // New Game
                        {
                            StartNight(1, gameTime);
                        }
                        else if (i > 1) // 6th Night | 7th Night
                        {
                            StartNight(4 + i, gameTime); //Only two i values are 2&3, add 4 to get Night 6&7
                        }
                        else // Continue
                        {
                            StartNight(TitleScreen.currentNight, gameTime);
                        }
                    }
                }
            }
        }

        private void UpdateNightIntro(GameTime gameTime)
        {
            if (TitleScreen.staticOn)
            {
                TitleScreen.staticBTex = TitleScreen._staticBTexMap[TitleScreen.staticTimer % TitleScreen._staticBTexMap.Length];
                TitleScreen.staticTimer--;
                TitleScreen.staticOn = TitleScreen.staticTimer >= 0;
            }

            loadAlpha = 0;
            if (gameTime.TotalGameTime.TotalSeconds <= startTime + 2) introAlpha = 100;
            if (gameTime.TotalGameTime.TotalSeconds >= startTime + 2 && gameTime.TotalGameTime.TotalSeconds < startTime + 3) introAlpha -= 10;
            if (gameTime.TotalGameTime.TotalSeconds >= startTime + 3 && gameTime.TotalGameTime.TotalSeconds < startTime + 8)
            {
                introAlpha = 0;
                loadAlpha = 1;
            }

            if (gameTime.TotalGameTime.TotalSeconds >= startTime + 8)
            {
                gameState = GameState.NIGHT;
            }
        }


        private void DrawIntro()
        {
            _spriteLayer[3].Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            _spriteLayer[3].DrawString(titleFont, Program.Disclaimer, new Vector2(150, 200), Color.White * (introAlpha / 100f));
            _spriteLayer[3].End();
        }

        // TODO - Add Drawing Completion Stars
        private void DrawTitle()
        {
            _spriteLayer[0].Begin();
            _spriteLayer[0].Draw(TitleScreen.bgTex, Vector2.Zero, Color.White);
            _spriteLayer[0].End();

            _spriteLayer[1].Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            _spriteLayer[1].Draw(TitleScreen.staticTex, Vector2.Zero, Color.White * 0.5f);
            _spriteLayer[1].End();

            _spriteLayer[2].Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            _spriteLayer[2].Draw(TitleScreen.staticLineTex, new Vector2(0, TitleScreen.barPos), Color.White * 0.2f);
            _spriteLayer[2].End();

            if (TitleScreen.staticOn)
            {
                _spriteLayer[3].Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
                _spriteLayer[3].Draw(TitleScreen.staticBTex, Vector2.Zero, Color.White * 0.5f);
                _spriteLayer[3].End();
            }

            int bottomYPos = _graphics.PreferredBackBufferHeight - textFont.LineSpacing - 10;
            int cRightPos = _graphics.PreferredBackBufferWidth - (Program.Copyright.Length * 10) - 10;
            int selYPos = 400 + 80 * TitleScreen.selected;

            //UI Layer
            _spriteLayer[4].Begin();
            _spriteLayer[4].DrawString(titleFont, Program.Title, new Vector2(200, 60), Color.White);
            _spriteLayer[4].DrawString(textFont, Program.Version, new Vector2(20, bottomYPos), Color.White);
            _spriteLayer[4].DrawString(textFont, Program.Copyright, new Vector2(cRightPos, bottomYPos), Color.White);
            
            _spriteLayer[4].DrawString(titleFont, ">>", new Vector2(125, selYPos), Color.White);

            for (int i = 0; i < TitleScreen.menuOptions.Count; i++)
            {
                _spriteLayer[4].DrawString(titleFont, TitleScreen.menuOptions[i], new Vector2(200, 400 + i * 80), Color.White);
            }

            if (TitleScreen.selected == 1)
                _spriteLayer[4].DrawString(textFont, $"Night {TitleScreen.currentNight}", new Vector2(200, 530), Color.White);

            _spriteLayer[4].End();
        }

        private void DrawNightIntro()
        {
            if (TitleScreen.staticOn)
            {
                _spriteLayer[2].Begin(/* SpriteSortMode.Immediate, BlendState.AlphaBlend */);
                _spriteLayer[2].Draw(TitleScreen.staticBTex, Vector2.Zero, Color.White/* * 0.7f */);
                _spriteLayer[2].End();
            }

            _spriteLayer[3].Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            _spriteLayer[3].DrawString(titleFont, $"12:00 AM\n Night {TitleScreen.currentNight}", new Vector2(530, 300), Color.White * (introAlpha / 100f));
            _spriteLayer[3].Draw(ui.loadTex, new Vector2(1230, 670), Color.White * loadAlpha);
            _spriteLayer[3].End();
        }

        // TODO - HNNNNNNG EVERYTHING
        private void DrawNight()
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

        }

        private static void InitTitleMenu()
        {
            TitleScreen.menuOptions.Clear();
            TitleScreen.menuOptions.Add("New Game");
            TitleScreen.menuOptions.Add("Continue");
            if (TitleScreen.Night6Unlocked) TitleScreen.menuOptions.Add("6th Night");
            if (TitleScreen.Night7Unlocked) TitleScreen.menuOptions.Add("7th Night");
        }

        private bool Collide(Vector2 pos, int x, int y, int width, int height)
        {
            return (x <= pos.X && pos.X <= x + width) && (y <= pos.Y && pos.Y <= y + height);
        }

        private string GetSavePath()
        {
            string asmName = Assembly.GetExecutingAssembly().GetName().Name;
            string appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            return Path.Combine(appData, $"LyraMakes\\{asmName}\\save.dat");
        }

        private void Save(byte data)
        {
            string fileName = GetSavePath();

            if (!File.Exists(fileName)) Directory.CreateDirectory(Path.GetDirectoryName(fileName));

            File.WriteAllBytes(fileName, new byte[] { data });
        }

        private void Save()
        {
            byte data = 0b00000000;

            if (TitleScreen.currentNight != trueNight) TitleScreen.currentNight = trueNight;

            if (TitleScreen.currentNight == 1) data = (byte)(data | 0b001_00_00_0);
            if (TitleScreen.currentNight == 2) data = (byte)(data | 0b010_00_00_0);
            if (TitleScreen.currentNight == 3) data = (byte)(data | 0b011_00_00_0);
            if (TitleScreen.currentNight == 4) data = (byte)(data | 0b100_00_00_0);
            if (TitleScreen.currentNight == 5) data = (byte)(data | 0b101_00_00_0);
            
            if (TitleScreen.numStars     == 1) data = (byte)(data | 0b000_01_00_0);
            if (TitleScreen.numStars     == 2) data = (byte)(data | 0b000_10_00_0);
            if (TitleScreen.numStars     == 3) data = (byte)(data | 0b000_11_00_0);
            
            if (TitleScreen.Night6Unlocked   ) data = (byte)(data | 0b000_00_10_0);
            if (TitleScreen.Night7Unlocked   ) data = (byte)(data | 0b000_00_01_0);

            Save(data);
        }

        private void Load()
        {
            string filename = GetSavePath();
            byte[] dataArr = File.ReadAllBytes(filename);
            if (dataArr.Length != 1) Save(0b001_00_00_0);

            byte data = dataArr[0];

            TitleScreen.currentNight = (data >> 5) & 0b0000_0111;
            TitleScreen.numStars = (data >> 3) & 0b0000_0011;
            TitleScreen.Night6Unlocked = ((data >> 2) & 0b0000_0001) == 1;
            TitleScreen.Night7Unlocked = ((data >> 1) & 0b0000_0001) == 1;
        }

        private void StartNight(int night, GameTime gameTime)
        {
            gameState = GameState.NIGHT_INTRO;
            TitleScreen.currentNight = night;
            Audio.StopAll();
            menuSelectionSFX.Play();
            TitleScreen.staticOn = true;
            TitleScreen.staticTimer = 5;
            startTime = gameTime.TotalGameTime.TotalSeconds;
            ui.SetStartTime(gameTime);
        }
    }
}
