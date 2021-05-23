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
    public enum GameState { INTRO, MENU, NIGHT, NIGHT_INTRO, CLOCK, FINISH, EVENT }

    public class Game1 : Game
    {
        private Rand rand;
        private UI ui;

        public GameState gameState;

        SpriteFont textFont;
        SpriteFont titleFont;
        SpriteFont labelFont;

        SoundEffect titleSong;
        SoundEffect menuSelectionSFX;
        SoundEffect titleStatic;

        SoundEffect DoorSFX;
        SoundEffect BoopSFX;
        SoundEffect camFlipSFX;

        AudioChannel sfx;
        AudioChannel bgMus;

        //Animatronics
        Freddy freddy;
        Bonnie bonnie;
        Chica chica;
        Foxy foxy;


        private readonly GraphicsDeviceManager _graphics;
        private SpriteBatch[] _spriteLayer;

        private int introAlpha;
        private int loadAlpha;
        private int trueNight;
        private double startTime;

        private string SavePath
        {
            get
            {
                string asmName = Assembly.GetExecutingAssembly().GetName().Name;
                string appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                return Path.Combine(appData, $"LyraMakes\\{asmName}\\save.dat");
            }
        }
        
        public int SliceWidth { get { return _graphics.PreferredBackBufferWidth / Office.numSlices + 1; } }

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

        // Make sure to remove Debug Texture
        protected override void Initialize()
        {
            //DEBUG ONLY
            int dbgWid = 10;
            int dbgHgt = 10;
            Debug.tex = new Texture2D(_graphics.GraphicsDevice, dbgWid, dbgHgt);
            Color[] dbgClr = new Color[dbgWid * dbgHgt];
            for (int i = 0; i < dbgClr.Length; i++)
            {
                dbgClr[i] = new Color(255, 0, 255, 255);
            }
            Debug.tex.SetData(dbgClr);

            sfx = new AudioChannel(1.0f);
            bgMus = new AudioChannel(1.0f);



            //Initializes the Program-Wide RNG
            int seed = (int)DateTime.UtcNow.ToFileTimeUtc();
            rand = new Rand(seed);

            ui = new UI();

            ui._usageTexMap = new Texture2D[5];

            Input.Initialize();
            Input.Latch();

            gameState = GameState.INTRO;
            introAlpha = 0;

            // Create Static Arrays and Lists
            Audio.activesfx = new List<AudioEffect>();
            _spriteLayer = new SpriteBatch[5];
            
            Camera._onTex = new Texture2D[11];
            Camera._offTex = new Texture2D[11];
            
            TitleScreen._texMap = new Texture2D[4];
            TitleScreen._staticTexMap = new Texture2D[8];
            TitleScreen._staticBTexMap = new Texture2D[7];
            TitleScreen.menuOptions = new List<string>();

            Office._officeTexMap = new Texture2D[5];
            Office.srcPosMap = new Rectangle[Office.numSlices + 1];
            Office.yPosMap = new double[(Office.numSlices / 2) + 1];
            
            for (int i = 0; i < Office.yPosMap.Length; i++)
            {
                Office.yPosMap[i] = 0.008 * 0.06 * i * i;
            }

            Office._leftControlTexMap =  new Texture2D[4];
            Office._rightControlTexMap = new Texture2D[4];

            Office._leftDoorTexMap = new Texture2D[16];
            Office._rightDoorTexMap = new Texture2D[16];

            Camera.TabletTexMap = new Texture2D[11];

            Office.LeftControl = new Prop();
            Office.RightControl = new Prop();

            Office.LeftDoor = new Prop();
            Office.RightDoor = new Prop();

            InitTitleMenu();

            Jumpscare._bonnieTexMap  = new Texture2D[11];
            Jumpscare._chicaTexMap   = new Texture2D[16];
            Jumpscare._foxyTexMap    = new Texture2D[18];
            Jumpscare._freddyATexMap = new Texture2D[31];
            Jumpscare._freddyBTexMap = new Texture2D[21];

            Room.InitRooms();

            Camera.CurrentRoom = Room.rooms[0];


            //Animatronic Setup
            freddy = new Freddy(rand);
            bonnie = new Bonnie(rand);
            chica = new Chica(rand);
            foxy = new Foxy(rand);


            // Check if save file is present
            if (!File.Exists(SavePath))
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
            textFont  = Content.Load<SpriteFont>("FNAFTitleFont");
            titleFont = Content.Load<SpriteFont>("FNAFBigTitleFont");
            labelFont = Content.Load<SpriteFont>("FNAFFontA");
                        
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

            TitleScreen.staticLineTex = Content.Load<Texture2D>("Images\\staticBar");
            TitleScreen.starTex       = Content.Load<Texture2D>("Icons\\star");


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

            // Jumpscares
            Jumpscare._bonnieTexMap[0]  = Content.Load<Texture2D>("Jumpscares\\bonnie1" );
            Jumpscare._bonnieTexMap[1]  = Content.Load<Texture2D>("Jumpscares\\bonnie2" );
            Jumpscare._bonnieTexMap[2]  = Content.Load<Texture2D>("Jumpscares\\bonnie3" );
            Jumpscare._bonnieTexMap[3]  = Content.Load<Texture2D>("Jumpscares\\bonnie4" );
            Jumpscare._bonnieTexMap[4]  = Content.Load<Texture2D>("Jumpscares\\bonnie5" );
            Jumpscare._bonnieTexMap[5]  = Content.Load<Texture2D>("Jumpscares\\bonnie6" );
            Jumpscare._bonnieTexMap[6]  = Content.Load<Texture2D>("Jumpscares\\bonnie7" );
            Jumpscare._bonnieTexMap[7]  = Content.Load<Texture2D>("Jumpscares\\bonnie8" );
            Jumpscare._bonnieTexMap[8]  = Content.Load<Texture2D>("Jumpscares\\bonnie9" );
            Jumpscare._bonnieTexMap[9]  = Content.Load<Texture2D>("Jumpscares\\bonnie10");
            Jumpscare._bonnieTexMap[10] = Content.Load<Texture2D>("Jumpscares\\bonnie11");
            
            Jumpscare._chicaTexMap[0]  = Content.Load<Texture2D>("Jumpscares\\chica1" );
            Jumpscare._chicaTexMap[1]  = Content.Load<Texture2D>("Jumpscares\\chica2");
            Jumpscare._chicaTexMap[2]  = Content.Load<Texture2D>("Jumpscares\\chica3");
            Jumpscare._chicaTexMap[3]  = Content.Load<Texture2D>("Jumpscares\\chica4");
            Jumpscare._chicaTexMap[4]  = Content.Load<Texture2D>("Jumpscares\\chica5");
            Jumpscare._chicaTexMap[5]  = Content.Load<Texture2D>("Jumpscares\\chica6");
            Jumpscare._chicaTexMap[6]  = Content.Load<Texture2D>("Jumpscares\\chica7");
            Jumpscare._chicaTexMap[7]  = Content.Load<Texture2D>("Jumpscares\\chica8");
            Jumpscare._chicaTexMap[8]  = Content.Load<Texture2D>("Jumpscares\\chica9");
            Jumpscare._chicaTexMap[9]  = Content.Load<Texture2D>("Jumpscares\\chica10");
            Jumpscare._chicaTexMap[10] = Content.Load<Texture2D>("Jumpscares\\chica11");
            Jumpscare._chicaTexMap[11] = Content.Load<Texture2D>("Jumpscares\\chica12");
            Jumpscare._chicaTexMap[12] = Content.Load<Texture2D>("Jumpscares\\chica13");
            Jumpscare._chicaTexMap[13] = Content.Load<Texture2D>("Jumpscares\\chica14");
            Jumpscare._chicaTexMap[14] = Content.Load<Texture2D>("Jumpscares\\chica15");
            Jumpscare._chicaTexMap[15] = Content.Load<Texture2D>("Jumpscares\\chica16");
            
            Jumpscare._foxyTexMap[0]  = Content.Load<Texture2D>("Jumpscares\\foxy1" );
            Jumpscare._foxyTexMap[1]  = Content.Load<Texture2D>("Jumpscares\\foxy2");
            Jumpscare._foxyTexMap[2]  = Content.Load<Texture2D>("Jumpscares\\foxy3");
            Jumpscare._foxyTexMap[3]  = Content.Load<Texture2D>("Jumpscares\\foxy4");
            Jumpscare._foxyTexMap[4]  = Content.Load<Texture2D>("Jumpscares\\foxy5");
            Jumpscare._foxyTexMap[5]  = Content.Load<Texture2D>("Jumpscares\\foxy6");
            Jumpscare._foxyTexMap[6]  = Content.Load<Texture2D>("Jumpscares\\foxy7");
            Jumpscare._foxyTexMap[7]  = Content.Load<Texture2D>("Jumpscares\\foxy8");
            Jumpscare._foxyTexMap[8]  = Content.Load<Texture2D>("Jumpscares\\foxy9");
            Jumpscare._foxyTexMap[9]  = Content.Load<Texture2D>("Jumpscares\\foxy10");
            Jumpscare._foxyTexMap[10] = Content.Load<Texture2D>("Jumpscares\\foxy11");
            Jumpscare._foxyTexMap[11] = Content.Load<Texture2D>("Jumpscares\\foxy12");
            Jumpscare._foxyTexMap[12] = Content.Load<Texture2D>("Jumpscares\\foxy13");
            Jumpscare._foxyTexMap[13] = Content.Load<Texture2D>("Jumpscares\\foxy14");
            Jumpscare._foxyTexMap[14] = Content.Load<Texture2D>("Jumpscares\\foxy15");
            Jumpscare._foxyTexMap[15] = Content.Load<Texture2D>("Jumpscares\\foxy16");
            Jumpscare._foxyTexMap[16] = Content.Load<Texture2D>("Jumpscares\\foxy17");
            Jumpscare._foxyTexMap[17] = Content.Load<Texture2D>("Jumpscares\\foxy18");


            Jumpscare._freddyATexMap[0]  = Content.Load<Texture2D>("Jumpscares\\freddyA1");
            Jumpscare._freddyATexMap[1]  = Content.Load<Texture2D>("Jumpscares\\freddyA2");
            Jumpscare._freddyATexMap[2]  = Content.Load<Texture2D>("Jumpscares\\freddyA3");
            Jumpscare._freddyATexMap[3]  = Content.Load<Texture2D>("Jumpscares\\freddyA4");
            Jumpscare._freddyATexMap[4]  = Content.Load<Texture2D>("Jumpscares\\freddyA5");
            Jumpscare._freddyATexMap[5]  = Content.Load<Texture2D>("Jumpscares\\freddyA6");
            Jumpscare._freddyATexMap[6]  = Content.Load<Texture2D>("Jumpscares\\freddyA7");
            Jumpscare._freddyATexMap[7]  = Content.Load<Texture2D>("Jumpscares\\freddyA8");
            Jumpscare._freddyATexMap[8]  = Content.Load<Texture2D>("Jumpscares\\freddyA9");
            Jumpscare._freddyATexMap[9]  = Content.Load<Texture2D>("Jumpscares\\freddyA10");
            Jumpscare._freddyATexMap[10] = Content.Load<Texture2D>("Jumpscares\\freddyA11");
            Jumpscare._freddyATexMap[11] = Content.Load<Texture2D>("Jumpscares\\freddyA12");
            Jumpscare._freddyATexMap[12] = Content.Load<Texture2D>("Jumpscares\\freddyA13");
            Jumpscare._freddyATexMap[13] = Content.Load<Texture2D>("Jumpscares\\freddyA14");
            Jumpscare._freddyATexMap[14] = Content.Load<Texture2D>("Jumpscares\\freddyA15");
            Jumpscare._freddyATexMap[15] = Content.Load<Texture2D>("Jumpscares\\freddyA16");
            Jumpscare._freddyATexMap[16] = Content.Load<Texture2D>("Jumpscares\\freddyA17");
            Jumpscare._freddyATexMap[17] = Content.Load<Texture2D>("Jumpscares\\freddyA18");
            Jumpscare._freddyATexMap[18] = Content.Load<Texture2D>("Jumpscares\\freddyA19");
            Jumpscare._freddyATexMap[19] = Content.Load<Texture2D>("Jumpscares\\freddyA20");
            Jumpscare._freddyATexMap[20] = Content.Load<Texture2D>("Jumpscares\\freddyA21");
            Jumpscare._freddyATexMap[21] = Content.Load<Texture2D>("Jumpscares\\freddyA22");
            Jumpscare._freddyATexMap[22] = Content.Load<Texture2D>("Jumpscares\\freddyA23");
            Jumpscare._freddyATexMap[23] = Content.Load<Texture2D>("Jumpscares\\freddyA24");
            Jumpscare._freddyATexMap[24] = Content.Load<Texture2D>("Jumpscares\\freddyA25");
            Jumpscare._freddyATexMap[25] = Content.Load<Texture2D>("Jumpscares\\freddyA26");
            Jumpscare._freddyATexMap[26] = Content.Load<Texture2D>("Jumpscares\\freddyA27");
            Jumpscare._freddyATexMap[27] = Content.Load<Texture2D>("Jumpscares\\freddyA28");
            Jumpscare._freddyATexMap[28] = Content.Load<Texture2D>("Jumpscares\\freddyA29");
            Jumpscare._freddyATexMap[29] = Content.Load<Texture2D>("Jumpscares\\freddyA30");
            Jumpscare._freddyATexMap[30] = Content.Load<Texture2D>("Jumpscares\\freddyA31");

            Jumpscare._freddyBTexMap[0]  = Content.Load<Texture2D>("Jumpscares\\freddyB1");
            Jumpscare._freddyBTexMap[1]  = Content.Load<Texture2D>("Jumpscares\\freddyB2");
            Jumpscare._freddyBTexMap[2]  = Content.Load<Texture2D>("Jumpscares\\freddyB3");
            Jumpscare._freddyBTexMap[3]  = Content.Load<Texture2D>("Jumpscares\\freddyB4");
            Jumpscare._freddyBTexMap[4]  = Content.Load<Texture2D>("Jumpscares\\freddyB5");
            Jumpscare._freddyBTexMap[5]  = Content.Load<Texture2D>("Jumpscares\\freddyB6");
            Jumpscare._freddyBTexMap[6]  = Content.Load<Texture2D>("Jumpscares\\freddyB7");
            Jumpscare._freddyBTexMap[7]  = Content.Load<Texture2D>("Jumpscares\\freddyB8");
            Jumpscare._freddyBTexMap[8]  = Content.Load<Texture2D>("Jumpscares\\freddyB9");
            Jumpscare._freddyBTexMap[9]  = Content.Load<Texture2D>("Jumpscares\\freddyB10");
            Jumpscare._freddyBTexMap[10] = Content.Load<Texture2D>("Jumpscares\\freddyB11");
            Jumpscare._freddyBTexMap[11] = Content.Load<Texture2D>("Jumpscares\\freddyB12");
            Jumpscare._freddyBTexMap[12] = Content.Load<Texture2D>("Jumpscares\\freddyB13");
            Jumpscare._freddyBTexMap[13] = Content.Load<Texture2D>("Jumpscares\\freddyB14");
            Jumpscare._freddyBTexMap[14] = Content.Load<Texture2D>("Jumpscares\\freddyB15");
            Jumpscare._freddyBTexMap[15] = Content.Load<Texture2D>("Jumpscares\\freddyB16");
            Jumpscare._freddyBTexMap[16] = Content.Load<Texture2D>("Jumpscares\\freddyB17");
            Jumpscare._freddyBTexMap[17] = Content.Load<Texture2D>("Jumpscares\\freddyB18");
            Jumpscare._freddyBTexMap[18] = Content.Load<Texture2D>("Jumpscares\\freddyB19");
            Jumpscare._freddyBTexMap[19] = Content.Load<Texture2D>("Jumpscares\\freddyB20");
            Jumpscare._freddyBTexMap[20] = Content.Load<Texture2D>("Jumpscares\\freddyB21");

            Office._officeTexMap[0] = Content.Load<Texture2D>("Rooms\\officeN");
            Office._officeTexMap[1] = Content.Load<Texture2D>("Rooms\\officeL");
            Office._officeTexMap[2] = Content.Load<Texture2D>("Rooms\\officeR");
            Office._officeTexMap[3] = Content.Load<Texture2D>("Rooms\\officeB");
            Office._officeTexMap[4] = Content.Load<Texture2D>("Rooms\\officeC");

            // UI Textures
            ui._usageTexMap[0] = Content.Load<Texture2D>("Icons\\usage1");
            ui._usageTexMap[1] = Content.Load<Texture2D>("Icons\\usage2");
            ui._usageTexMap[2] = Content.Load<Texture2D>("Icons\\usage3");
            ui._usageTexMap[3] = Content.Load<Texture2D>("Icons\\usage4");
            ui._usageTexMap[4] = Content.Load<Texture2D>("Icons\\usage5");

            ui.camTex = Content.Load<Texture2D>("Icons\\camFlipUI");
            
            ui.loadTex = Content.Load<Texture2D>("Icons\\loading");

            Office._leftControlTexMap[0]  = Content.Load<Texture2D>("Props\\LFF");
            Office._leftControlTexMap[1]  = Content.Load<Texture2D>("Props\\LFN");
            Office._leftControlTexMap[2]  = Content.Load<Texture2D>("Props\\LNF");
            Office._leftControlTexMap[3]  = Content.Load<Texture2D>("Props\\LNN");
            
            Office._rightControlTexMap[0] = Content.Load<Texture2D>("Props\\RFF");
            Office._rightControlTexMap[1] = Content.Load<Texture2D>("Props\\RFN");
            Office._rightControlTexMap[2] = Content.Load<Texture2D>("Props\\RNF");
            Office._rightControlTexMap[3] = Content.Load<Texture2D>("Props\\RNN");

            Office._leftDoorTexMap[0]  = Content.Load<Texture2D>("Props\\LDoor1");
            Office._leftDoorTexMap[1]  = Content.Load<Texture2D>("Props\\LDoor2");
            Office._leftDoorTexMap[2]  = Content.Load<Texture2D>("Props\\LDoor3");
            Office._leftDoorTexMap[3]  = Content.Load<Texture2D>("Props\\LDoor4");
            Office._leftDoorTexMap[4]  = Content.Load<Texture2D>("Props\\LDoor5");
            Office._leftDoorTexMap[5]  = Content.Load<Texture2D>("Props\\LDoor6");
            Office._leftDoorTexMap[6]  = Content.Load<Texture2D>("Props\\LDoor7");
            Office._leftDoorTexMap[7]  = Content.Load<Texture2D>("Props\\LDoor8");
            Office._leftDoorTexMap[8]  = Content.Load<Texture2D>("Props\\LDoor9");
            Office._leftDoorTexMap[9]  = Content.Load<Texture2D>("Props\\LDoor10");
            Office._leftDoorTexMap[10] = Content.Load<Texture2D>("Props\\LDoor11");
            Office._leftDoorTexMap[11] = Content.Load<Texture2D>("Props\\LDoor12");
            Office._leftDoorTexMap[12] = Content.Load<Texture2D>("Props\\LDoor13");
            Office._leftDoorTexMap[13] = Content.Load<Texture2D>("Props\\LDoor14");
            Office._leftDoorTexMap[14] = Content.Load<Texture2D>("Props\\LDoor15");
            Office._leftDoorTexMap[15] = Content.Load<Texture2D>("Props\\LDoor16");
            
            Office._rightDoorTexMap[0]  = Content.Load<Texture2D>("Props\\RDoor1");
            Office._rightDoorTexMap[1]  = Content.Load<Texture2D>("Props\\RDoor2");
            Office._rightDoorTexMap[2]  = Content.Load<Texture2D>("Props\\RDoor3");
            Office._rightDoorTexMap[3]  = Content.Load<Texture2D>("Props\\RDoor4");
            Office._rightDoorTexMap[4]  = Content.Load<Texture2D>("Props\\RDoor5");
            Office._rightDoorTexMap[5]  = Content.Load<Texture2D>("Props\\RDoor6");
            Office._rightDoorTexMap[6]  = Content.Load<Texture2D>("Props\\RDoor7");
            Office._rightDoorTexMap[7]  = Content.Load<Texture2D>("Props\\RDoor8");
            Office._rightDoorTexMap[8]  = Content.Load<Texture2D>("Props\\RDoor9");
            Office._rightDoorTexMap[9]  = Content.Load<Texture2D>("Props\\RDoor10");
            Office._rightDoorTexMap[10] = Content.Load<Texture2D>("Props\\RDoor11");
            Office._rightDoorTexMap[11] = Content.Load<Texture2D>("Props\\RDoor12");
            Office._rightDoorTexMap[12] = Content.Load<Texture2D>("Props\\RDoor13");
            Office._rightDoorTexMap[13] = Content.Load<Texture2D>("Props\\RDoor14");
            Office._rightDoorTexMap[14] = Content.Load<Texture2D>("Props\\RDoor15");
            Office._rightDoorTexMap[15] = Content.Load<Texture2D>("Props\\RDoor16");

            Camera.TabletTexMap[0] = Content.Load<Texture2D>("Images\\tablet0");
            Camera.TabletTexMap[1] = Content.Load<Texture2D>("Images\\tablet1");
            Camera.TabletTexMap[2] = Content.Load<Texture2D>("Images\\tablet2");
            Camera.TabletTexMap[3] = Content.Load<Texture2D>("Images\\tablet3");
            Camera.TabletTexMap[4] = Content.Load<Texture2D>("Images\\tablet4");
            Camera.TabletTexMap[5] = Content.Load<Texture2D>("Images\\tablet5");
            Camera.TabletTexMap[6] = Content.Load<Texture2D>("Images\\tablet6");
            Camera.TabletTexMap[7] = Content.Load<Texture2D>("Images\\tablet7");
            Camera.TabletTexMap[8] = Content.Load<Texture2D>("Images\\tablet8");
            Camera.TabletTexMap[9] = Content.Load<Texture2D>("Images\\tablet9");
            Camera.TabletTexMap[10] = Content.Load<Texture2D>("Images\\tablet10");


            Room.rooms[0].roomTexMap = new Texture2D[7];
            Room.rooms[0].roomTexMap[0] = Content.Load<Texture2D>("Rooms\\CAM1A-");
            Room.rooms[0].roomTexMap[1] = Content.Load<Texture2D>("Rooms\\CAM1A-FBC");
            Room.rooms[0].roomTexMap[2] = Content.Load<Texture2D>("Rooms\\CAM1A-FBC-S");
            Room.rooms[0].roomTexMap[3] = Content.Load<Texture2D>("Rooms\\CAM1A-FB");
            Room.rooms[0].roomTexMap[4] = Content.Load<Texture2D>("Rooms\\CAM1A-FC");
            Room.rooms[0].roomTexMap[5] = Content.Load<Texture2D>("Rooms\\CAM1A-F");
            Room.rooms[0].roomTexMap[6] = Content.Load<Texture2D>("Rooms\\CAM1A-F-S");

            Room.rooms[1].roomTexMap = new Texture2D[6];
            Room.rooms[1].roomTexMap[1] = Content.Load<Texture2D>("Rooms\\CAM1B-");
            Room.rooms[1].roomTexMap[2] = Content.Load<Texture2D>("Rooms\\CAM1B-B");
            Room.rooms[1].roomTexMap[3] = Content.Load<Texture2D>("Rooms\\CAM1B-B-S");
            Room.rooms[1].roomTexMap[4] = Content.Load<Texture2D>("Rooms\\CAM1B-C");
            Room.rooms[1].roomTexMap[5] = Content.Load<Texture2D>("Rooms\\CAM1B-C-S");
            Room.rooms[1].roomTexMap[6] = Content.Load<Texture2D>("Rooms\\CAM1B-F");

            Room.rooms[4].roomTexMap = new Texture2D[6];
            Room.rooms[4].roomTexMap[0] = Content.Load<Texture2D>("Rooms\\CAM2B-");
            Room.rooms[4].roomTexMap[1] = Content.Load<Texture2D>("Rooms\\CAM2B-S1");
            Room.rooms[4].roomTexMap[2] = Content.Load<Texture2D>("Rooms\\CAM2B-S2");
            Room.rooms[4].roomTexMap[3] = Content.Load<Texture2D>("Rooms\\CAM2B-B");
            Room.rooms[4].roomTexMap[4] = Content.Load<Texture2D>("Rooms\\CAM2B-B-S1");
            Room.rooms[4].roomTexMap[5] = Content.Load<Texture2D>("Rooms\\CAM2B-B-S2");

            Room.rooms[5].roomTexMap = new Texture2D[2];
            Room.rooms[5].roomTexMap[0] = Content.Load<Texture2D>("Rooms\\CAM3-");
            Room.rooms[5].roomTexMap[1] = Content.Load<Texture2D>("Rooms\\CAM3-B");

            Room.rooms[6].roomTexMap = new Texture2D[6];
            Room.rooms[6].roomTexMap[0] = Content.Load<Texture2D>("Rooms\\CAM4A-");
            Room.rooms[6].roomTexMap[1] = Content.Load<Texture2D>("Rooms\\CAM4A-S");
            Room.rooms[6].roomTexMap[2] = Content.Load<Texture2D>("Rooms\\CAM4A-S2");
            Room.rooms[6].roomTexMap[3] = Content.Load<Texture2D>("Rooms\\CAM4A-C");
            Room.rooms[6].roomTexMap[4] = Content.Load<Texture2D>("Rooms\\CAM4A-C-S");
            Room.rooms[6].roomTexMap[5] = Content.Load<Texture2D>("Rooms\\CAM4A-F");

            Room.rooms[8].roomTexMap = new Texture2D[4];
            Room.rooms[8].roomTexMap[0] = Content.Load<Texture2D>("Rooms\\CAM5-");
            Room.rooms[8].roomTexMap[1] = Content.Load<Texture2D>("Rooms\\CAM5-S");
            Room.rooms[8].roomTexMap[2] = Content.Load<Texture2D>("Rooms\\CAM5-B");
            Room.rooms[8].roomTexMap[3] = Content.Load<Texture2D>("Rooms\\CAM5-B-S");

            Room.rooms[10].roomTexMap = new Texture2D[4];
            Room.rooms[10].roomTexMap[0] = Content.Load<Texture2D>("Rooms\\CAM7-");
            Room.rooms[10].roomTexMap[1] = Content.Load<Texture2D>("Rooms\\CAM7-C");
            Room.rooms[10].roomTexMap[2] = Content.Load<Texture2D>("Rooms\\CAM7-C-S");
            Room.rooms[10].roomTexMap[3] = Content.Load<Texture2D>("Rooms\\CAM7-F");




            // Audio
            titleSong        = Content.Load<SoundEffect>("Audio\\titleScreen");
            menuSelectionSFX = Content.Load<SoundEffect>("Audio\\blip1");
            titleStatic      = Content.Load<SoundEffect>("Audio\\static2");

            DoorSFX = Content.Load<SoundEffect>("Audio\\doorToggle");
            BoopSFX = Content.Load<SoundEffect>("Audio\\noseBoop");
            camFlipSFX = Content.Load<SoundEffect>("Audio\\monitorFlip");

            // Post Content Initialization
            for (int i = 0; i < Office.numSlices + 1; i++)
            {
                Office.srcPosMap[i] = new Rectangle(i * SliceWidth, 0, SliceWidth, Office._officeTexMap[0].Height);
            }

            Office.LeftControl.SetTex(Office.LeftControlTex, SliceWidth);
            Office.RightControl.SetTex(Office.RightControlTex, SliceWidth);

            Office.LeftDoor.SetTex(Office._leftDoorTexMap[Office.leftDoorTex], SliceWidth);
            Office.RightDoor.SetTex(Office._rightDoorTexMap[Office.rightDoorTex], SliceWidth);
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
                    UpdateNight(gameTime);
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

        // HEY DONT FORGET ABOUT THE TESTING LINE
        private void UpdateIntro(GameTime gameTime)
        {
            //USED FOR TESTING --- MAKE FUCKING SURE THIS IS REMOVED BEFORE COMPLETION
            StartNight(1, gameTime);

            // Controls the fade of the opening disclaimer
            if (1 <= gameTime.TotalGameTime.TotalSeconds && gameTime.TotalGameTime.TotalSeconds <= 2 && introAlpha < 100) introAlpha += 10;
            if (2 <= gameTime.TotalGameTime.TotalSeconds && gameTime.TotalGameTime.TotalSeconds <= 3 && introAlpha < 100) introAlpha = 100;
            if (3 <= gameTime.TotalGameTime.TotalSeconds && gameTime.TotalGameTime.TotalSeconds <= 4 && introAlpha > 0  ) introAlpha -= 10;

            // After 5 Seconds, Go to menu
            if (gameTime.TotalGameTime.TotalSeconds >= 5)
            {
                gameState = GameState.MENU;
                Audio.Play(titleStatic, bgMus);
                Audio.Play(titleSong, bgMus, true);
            }
        }

        // TODO - New Game Fade to newspaper
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
                bool res = Collide(Input.MousePos, 200, 400 + i * 80, 300, 50);
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
            if (gameTime.TotalGameTime.TotalSeconds >= startTime + 2 && gameTime.TotalGameTime.TotalSeconds < startTime + 3) introAlpha -= 2;
            if (gameTime.TotalGameTime.TotalSeconds >= startTime + 3 && gameTime.TotalGameTime.TotalSeconds < startTime + 8)
            {
                introAlpha = 0;
                loadAlpha = 1;
            }

            if (gameTime.TotalGameTime.TotalSeconds >= startTime + 8)
            {
                UpdateNight(gameTime);
                gameState = GameState.NIGHT;
            }
        }

        // TODO - The actual game logic... not just the ui updates and visuals
        // (>_<) ~(>_<~) (T_T) >>_<<
        private void UpdateNight(GameTime gameTime)
        {
            ui.OnTick(gameTime);

            double gTTGTTS = gameTime.TotalGameTime.TotalSeconds;
            if (freddy.moveStartTime + freddy.movementOffset < gTTGTTS) freddy.MovementOpportunity();
            if (bonnie.moveStartTime + bonnie.movementOffset < gTTGTTS) bonnie.MovementOpportunity();
            if (chica.moveStartTime + chica.movementOffset < gTTGTTS) chica.MovementOpportunity();
            if (foxy.moveStartTime + foxy.movementOffset < gTTGTTS) foxy.MovementOpportunity();


            UpdateCameraFlip();

            if (!Office.isCamUp)
            {
                UpdateStandardOffice();
                UpdateOfficeDoors();
            }
            else
            {
                UpdateCameras();
            }
        }

        private void UpdateCameras()
        {
            // Freddy, Bonnie, Chica, Foxy
            //if (Camera.CurrentRoom == animatronics[0])
            
        }

        private void UpdateStandardOffice()
        {
            // Scrolling logic -------
            int gPBBW = _graphics.PreferredBackBufferWidth;
            int gPBBH = _graphics.PreferredBackBufferHeight;

            if (Collide(Input.MousePos, 0, 0, 500, gPBBH)) Office.xScroll -= 10;
            if (Collide(Input.MousePos, gPBBW - 500, 0, 500, gPBBH)) Office.xScroll += 10;

            if (Input.GetMouseDown() && Collide(Input.MousePos, 675 - Office.xScroll, 215, 10, 30))
                Audio.Play(BoopSFX, sfx);

            int scrollMax = -1 * (gPBBW - Office.OfficeTex.Width);
            if (Office.xScroll < 0) Office.xScroll = 0;
            if (Office.xScroll > scrollMax) Office.xScroll = scrollMax;

            int width = gPBBW / Office.numSlices + 1;
            for (int i = 0; i < Office.numSlices + 1; i++)
            {
                Office.srcPosMap[i].X = Office.xScroll + (i * width);
            }

            if (Office.isLeftDoorClosed && Office.leftDoorTex < 15)
            {
                if (Office.changeLeftDoor) Office.leftDoorTex++;
                Office.changeLeftDoor = !Office.changeLeftDoor;
            }
            if (!Office.isLeftDoorClosed && Office.leftDoorTex > 0)
            {
                if (Office.changeLeftDoor) Office.leftDoorTex--;
                Office.changeLeftDoor = !Office.changeLeftDoor;
            }

            if (Office.isRightDoorClosed && Office.rightDoorTex < 15)
            {
                if (Office.changeRightDoor) Office.rightDoorTex++;
                Office.changeRightDoor = !Office.changeRightDoor;
            }
            if (!Office.isRightDoorClosed && Office.rightDoorTex > 0)
            {
                if (Office.changeRightDoor) Office.rightDoorTex--;
                Office.changeRightDoor = !Office.changeRightDoor;
            }

            Office.LeftDoor.SetTex(Office._leftDoorTexMap[Office.leftDoorTex], SliceWidth);
            Office.RightDoor.SetTex(Office._rightDoorTexMap[Office.rightDoorTex], SliceWidth);
        }

        private void UpdateOfficeDoors()
        {
            if (Office.leftDoorTimer > 0) Office.leftDoorTimer--;
            if (Office.rightDoorTimer > 0) Office.rightDoorTimer--;

            if (Office.leftDoorTimer == 0 && Input.GetMouseDown() && Collide(Input.MousePos, Office.leftXPos - Office.xScroll + 20, Office.controlHeight + 15, 55, 90))
            {
                Office.leftDoorTimer = 20;
                Audio.Play(DoorSFX, sfx);
                Office.isLeftDoorClosed = !Office.isLeftDoorClosed;
            }
            if (Input.GetMouseDown() && Collide(Input.MousePos, Office.leftXPos - Office.xScroll + 20, Office.controlHeight + 135, 55, 90))
            {
                Office.isLeftLightOn = !Office.isLeftLightOn;
                Office.isRightLightOn = false;
            }

            if (Office.rightDoorTimer == 0 && Input.GetMouseDown() && Collide(Input.MousePos, Office.rightXPos - Office.xScroll + 20, Office.controlHeight + 15, 55, 90))
            {
                Office.rightDoorTimer = 20;
                Audio.Play(DoorSFX, sfx);
                Office.isRightDoorClosed = !Office.isRightDoorClosed;
            }
            if (Input.GetMouseDown() && Collide(Input.MousePos, Office.rightXPos - Office.xScroll + 20, Office.controlHeight + 135, 55, 90))
            {
                Office.isRightLightOn = !Office.isRightLightOn;
                Office.isLeftLightOn = false;
            }

            Office.LeftControl.SetTex(Office.LeftControlTex, SliceWidth);
            Office.RightControl.SetTex(Office.RightControlTex, SliceWidth);
        }

        private void UpdateCameraFlip()
        {
            if (Collide(Input.MousePos, 270, 653, 600, 60))
            {
                if (Office.hasLeftCamBox)
                {
                    Office.hasLeftCamBox = false;
                    if (!Office.isCamUp && Camera.TabletTexOffset == -1) Office.ToggleCam(camFlipSFX, sfx);
                    if (Office.isCamUp && Camera.TabletTexOffset == 11) Office.ToggleCam(camFlipSFX, sfx);
                }
            }
            else
            {
                Office.hasLeftCamBox = true;
            }

            if (Office.isCamUp && Camera.TabletTexOffset < 11) Camera.TabletTexOffset++;
            if (!Office.isCamUp && Camera.TabletTexOffset > -1) Camera.TabletTexOffset--;
        }

        private void DrawIntro()
        {
            _spriteLayer[3].Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            _spriteLayer[3].DrawString(titleFont, Program.Disclaimer, new Vector2(150, 200), Color.White * (introAlpha / 100f));
            _spriteLayer[3].End();
        }

        // TODO - New Game Fade to newspaper
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

            for (int i = 0; i < TitleScreen.numStars; i++)
            {
                _spriteLayer[4].Draw(TitleScreen.starTex, new Vector2(200 + (i * 80), 300), Color.White);
            }

            for (int i = 0; i < TitleScreen.menuOptions.Count; i++)
            {
                _spriteLayer[4].DrawString(titleFont, TitleScreen.menuOptions[i], new Vector2(200, 400 + (i * 80)), Color.White);
            }

            if (TitleScreen.selected == 1)
                _spriteLayer[4].DrawString(textFont, $"Night {TitleScreen.currentNight}", new Vector2(200, 530), Color.White);

            _spriteLayer[4].End();
        }

        private void DrawNightIntro()
        {
            if (TitleScreen.staticOn)
            {
                _spriteLayer[2].Begin();
                _spriteLayer[2].Draw(TitleScreen.staticBTex, Vector2.Zero, Color.White);
                _spriteLayer[2].End();
            }

            _spriteLayer[3].Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            _spriteLayer[3].DrawString(titleFont, $"12:00 AM\n Night {TitleScreen.currentNight}", new Vector2(530, 300), Color.White * (introAlpha / 100f));
            _spriteLayer[3].Draw(ui.loadTex, new Vector2(1230, 670), Color.White * loadAlpha);
            _spriteLayer[3].End();
        }

        // TODO - Drawing of cameras & camera UI, Jumpscares
        private void DrawNight()
        {
            // Drawing the BG Layer
            if (Camera.TabletTexOffset < 11)
            {
                _spriteLayer[0].Begin();
                DrawBackgroundTex(Office.OfficeTex, Office.xScroll);
                _spriteLayer[0].End();

                float gPBBW = _graphics.PreferredBackBufferWidth;

                // Draw the Doors, Door Controls, and Fan
                _spriteLayer[1].Begin();
                Office.LeftDoor.Draw(_spriteLayer[1], new Vector2(Office.leftXPos - Office.xScroll + 60, 0), gPBBW);
                Office.RightDoor.Draw(_spriteLayer[1], new Vector2(Office.rightXPos - Office.xScroll - 210, 0), gPBBW);
                Office.LeftControl.Draw(_spriteLayer[1], new Vector2(Office.leftXPos - Office.xScroll, Office.controlHeight), gPBBW);
                Office.RightControl.Draw(_spriteLayer[1], new Vector2(Office.rightXPos - Office.xScroll, Office.controlHeight), gPBBW);
                _spriteLayer[1].End();
            }
            else
            {
                _spriteLayer[0].Begin();
                DrawBackgroundTex(Camera.CurrentRoom.roomTex, Camera.scroll);
            }


            // Draw the Night UI
            string time = $"{ui.time}";
            int timeOffset = _graphics.PreferredBackBufferWidth - 120 - (15 * time.Length);
            int bottomYPos = _graphics.PreferredBackBufferHeight - labelFont.LineSpacing - 10;

            _spriteLayer[3].Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            _spriteLayer[3].DrawString(titleFont, "AM", new Vector2(1180, 30), Color.White);
            _spriteLayer[3].DrawString(titleFont, time, new Vector2(timeOffset, 30), Color.White);
            _spriteLayer[3].DrawString(labelFont, $"Night {TitleScreen.currentNight}", new Vector2(1160, 80), Color.White);

            _spriteLayer[3].DrawString(labelFont, $"Power left: {ui.power / 10}%", new Vector2(20, bottomYPos - 40), Color.White);

            _spriteLayer[3].DrawString(labelFont, "Usage:", new Vector2(20, bottomYPos), Color.White);
            _spriteLayer[3].Draw(ui._usageTexMap[ui.powerUsage - 1], new Vector2(115, bottomYPos), Color.White);
            _spriteLayer[3].Draw(ui.camTex, new Vector2(270, bottomYPos - 30), Color.White);
            _spriteLayer[3].End();

            if (Camera.DrawCam)
            {
                _spriteLayer[4].Begin();
                _spriteLayer[4].Draw(Camera.TabletTex, Vector2.Zero, Color.White);
                _spriteLayer[4].End();
            }

        }

        /// <summary>
        /// Draws a the provided <c>Texture2D</c> at the specified scroll to the background layer
        /// </summary>
        /// <param name="tex">The texture to use</param>
        /// <param name="scroll">Horizontal scroll</param>
        private void DrawBackgroundTex(Texture2D tex, int scroll)
        {
            for (int i = 0; i <= Office.numSlices; i++)
            {
                float gPBBW = _graphics.PreferredBackBufferWidth;
                Vector2 pos = new Vector2(Office.srcPosMap[i].X - scroll, tex.Height / 2);
                float yPos = Math.Abs((gPBBW / 2f) - pos.X) / gPBBW;
                _spriteLayer[0].Draw(
                    tex, // Base Texture
                    pos,                   // Position
                    Office.srcPosMap[i],   // Source on Tex
                    Color.White,           // Color Mask, Unused in base Begin() mode
                    0,                     // Rotation
                    new Vector2(0, tex.Height / 2),         // Origin
                    new Vector2(1, LMath.GetYScale(yPos)), // Scale
                    SpriteEffects.None,    // Sprite Effects
                    0                      // Layer Depth - Unused with my layering system
                );
            }
        }


        private void InitTitleMenu()
        {
            TitleScreen.menuOptions.Clear();
            TitleScreen.menuOptions.Add("New Game");
            TitleScreen.menuOptions.Add("Continue");
            if (TitleScreen.Night6Unlocked) TitleScreen.menuOptions.Add("6th Night");
            if (TitleScreen.Night7Unlocked) TitleScreen.menuOptions.Add("7th Night");

            trueNight = TitleScreen.currentNight;
        }

        private bool Collide(Vector2 pos, int x, int y, int width, int height)
        {
            return (x <= pos.X && pos.X <= x + width) && (y <= pos.Y && pos.Y <= y + height);
        }

        private void Save(byte data)
        {
            string fileName = SavePath;

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
            string filename = SavePath;
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
            AnimatronicLevel[] levels = AnimatronicLevel.NightPreset(night, rand);

            bonnie.SetAILevel(levels[1]);

            gameState = GameState.NIGHT_INTRO;
            TitleScreen.currentNight = night;
            Audio.StopAll();
            menuSelectionSFX.Play();
            TitleScreen.staticOn = true;
            TitleScreen.staticTimer = 5;
            startTime = gameTime.TotalGameTime.TotalSeconds;
            ui.SetStartTime(gameTime);
            ui.Reset();
        }
    }
}
