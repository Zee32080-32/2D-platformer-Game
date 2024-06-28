using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Coursework_Retake.World;
using Coursework_Retake.items;

namespace Coursework_Retake
{
    class Level
    {
        //Structure of the level
        private Tiles[,] tiles;
        private Loader loader;
        //Sounds
        private readonly SoundEffect nextLevel;
        private readonly SoundEffect win;
        double elapsedTime = 0.0;

        private Random random = new Random();
        public int RandPotion = -1;

        //Entities in the level
        public Player Player
        {
            get { return player; }
        }
        Player player;

        public List<Enemy_1> enemy_1 = new List<Enemy_1>();

        //Start and end
        private Vector2 start;
        private Point exit = InvalidPosition;
        private static readonly Point InvalidPosition = new Point(-1, -1);

        //Levels
        public bool LevelCompleted
        {
            get { return levelCompleted; }
        }
        bool levelCompleted;

        public bool isLevel1;
        public bool isLevel2;
        public bool isLevel3;
        public bool isLevel4;
        public bool isLevel5;
        public bool isLevel6;
        public bool isLevel7;
        public bool isLevel8;
        public bool isLevel9;
        public bool isBoss;

        //Random Game State
        private Random randomrandom = new Random(354668);

        //Content Manager
        public ContentManager Content
        {
            get { return content; }
        }
        ContentManager content;

        //Level Score System
        public int Score
        {
            get { return score; }
        }
        int score;

        private const int timerpoints = 5;

        //Time Managemer
        public TimeSpan TimeLeft
        {
            get { return timeleft; }
        }
        TimeSpan timeleft;

        public TimeSpan CountDownTimer
        {
            get { return cdt; }
        }
        TimeSpan cdt;

        public int Width
        {
            get { return tiles.GetLength(0); }
        }

        public int Height
        {
            get { return tiles.GetLength(1); }
        }

        //Lists for Diamonds, Coins and chests
        public List<Diamonds> diamonds = new List<Diamonds>();
        public List<Coins> coins = new List<Coins>();
        public List<RandomPowerUp> randomPowerUp = new List<RandomPowerUp>();
        public List<Health_Boost> HealthPotions = new List<Health_Boost>();
        public List<Time_Boost> TimePotions = new List<Time_Boost>();
        public List<Invisibility_Potion> InvisibilityPotions = new List<Invisibility_Potion>();



        public Level(IServiceProvider sp, Stream filestream, int LevelInd)
        {
            content = new ContentManager(sp, "Content");
            loader = new Loader(filestream);
            loader.ReadXML("content/info.xml");

            timeleft = TimeSpan.FromMinutes(1.0);

            LoadTiles(filestream);

            nextLevel = Content.Load<SoundEffect>("Audio/teleport");
            win = Content.Load<SoundEffect>("Audio/Win");

        }


        private void LoadTiles(Stream fileStream)
        {
            int width;
            List<string> lines = new List<string>();
            using (StreamReader reader = new StreamReader(fileStream))
            {
                string line = reader.ReadLine();
                width = line.Length;
                while (line != null)
                {
                    lines.Add(line);
                    if (line.Length != width)
                    {
                        throw new Exception($"The length of line {lines.Count} is different from all preceding lines.");
                    }
                    line = reader.ReadLine();
                }
            }

            tiles = new Tiles[width, lines.Count];

            for (int y = 0; y < Height; ++y)
            {
                for (int x = 0; x < Width; ++x)
                {
                    // to load each tile.
                    char tileType = lines[y][x];
                    tiles[x, y] = LoadTile(tileType, x, y);
                }
            }

            //checks if the game has a start and end point
            if (Player == null)
                throw new NotSupportedException("A level must have a starting point.");
            if (exit == InvalidPosition)
                throw new NotSupportedException("A level must have an exit point.");

        }
        private Tiles LoadTile(string name, Tiles_Collision collision)
        {
            if (isLevel1)
                return new Tiles(Content.Load<Texture2D>("Tiles/Level1/" + name), collision);
            else if (isLevel2)
                return new Tiles(Content.Load<Texture2D>("Tiles/Level2/" + name), collision);
            else if (isLevel3)
                return new Tiles(Content.Load<Texture2D>("Tiles/Level3/" + name), collision);
            else
                return new Tiles(Content.Load<Texture2D>("Tiles/Level1/" + name), collision);
        }

        private Tiles LoadOtherTiles(string name, Tiles_Collision collision)
        {
            return new Tiles(Content.Load<Texture2D>("Tiles/Others/" + name), collision);
        }

        private Tiles RandomTile()
        {
            Random rand = new Random();
            int x = rand.Next(1, 2);

            if (x == 1)
                return LoadTile("Ground", Tiles_Collision.Impassable);
            else
                return LoadTile("Platform", Tiles_Collision.Passable);
        }

        private Tiles LoadStart(int x, int y)
        {
            if (Player != null)
                throw new NotSupportedException("The level can only have one starting point.");

            start = RectangleExtensions.BottomCentre(Bounds(x, y));
            player = new Player(this, start);

            return new Tiles(null, Tiles_Collision.Passable);
        }

        private Tiles LoadExit(int x, int y)
        {
            if (exit != InvalidPosition)
                throw new NotSupportedException("The level can have only one one exit point.");

            exit = Bounds(x, y).Center;

            return LoadTile("Exit", Tiles_Collision.Passable);
        }

        private Tiles LoadDiamonds(int x, int y)
        {
            Point position = Bounds(x, y).Center;
            diamonds.Add(new Diamonds(this, new Vector2(position.X, position.Y)));
            return new Tiles(null, Tiles_Collision.Passable);

        }

        private Tiles LoadCoins(int x, int y)
        {
            Point position = Bounds(x, y).Center;
            coins.Add(new Coins(this, new Vector2(position.X, position.Y)));
            return new Tiles(null, Tiles_Collision.Passable);


        }

        private Tiles LoadHealthPotions(int x, int y)
        {
            Point position = Bounds(x, y).Center;
            HealthPotions.Add(new Health_Boost(this, new Vector2(position.X, position.Y)));
            return new Tiles(null, Tiles_Collision.Passable);


        }

        private Tiles LoadTimePotions(int x, int y)
        {
            Point position = Bounds(x, y).Center;
            TimePotions.Add(new Time_Boost(this, new Vector2(position.X, position.Y)));
            return new Tiles(null, Tiles_Collision.Passable);


        }

        private Tiles LoadInvisibilityPotion(int x, int y)
        {
            Point position = Bounds(x, y).Center;
            InvisibilityPotions.Add(new Invisibility_Potion(this, new Vector2(position.X, position.Y)));
            return new Tiles(null, Tiles_Collision.Passable);


        }


        private Tiles LoadRandPowerUp(int x, int y)
        {
            Point position = Bounds(x, y).Center;
            randomPowerUp.Add(new RandomPowerUp(this, new Vector2(position.X, position.Y)));
            return new Tiles(null, Tiles_Collision.Passable);

        }

        private Tiles LoadEnemyTile(int x, int y)
        {
            Vector2 position = RectangleExtensions.BottomCentre(Bounds(x, y));
            enemy_1.Add(new Enemy_1(this, position));
            return new Tiles(null, Tiles_Collision.Passable);
        }

        private Tiles LoadTile(char tileType, int x, int y)
        {
            switch (tileType)
            {
                // Blank space
                case '.':
                    return new Tiles(null, Tiles_Collision.Passable);

                // Start point
                case 'S':
                    return LoadStart(x, y);

                // Exit Point
                case 'X':
                    return LoadExit(x, y);

                // Diamonds
                case 'D':
                    return LoadDiamonds(x, y);

                // Coins
                case 'C':
                    return LoadCoins(x, y);

                case 'O':
                    return LoadHealthPotions(x, y);

                case 'T':
                    return LoadTimePotions(x, y);

                case 'I':
                    return LoadInvisibilityPotion(x, y);
                // Platform
                case '-':
                    return LoadTile("Platform", Tiles_Collision.Platform);

                // Impassable block
                case '#':
                    return LoadTile("Ground", Tiles_Collision.Impassable);

                case 'L':
                    return LoadOtherTiles("Lava", Tiles_Collision.Lava);

                case 'P':
                    return LoadRandPowerUp(x, y);

                case 'R':
                    return RandomTile();

                // enemies
                case 'M':
                    return LoadEnemyTile(x, y);
                // Unknown tile type character
                default:
                    throw new NotSupportedException(String.Format("Unsupported tile type character '{0}' at position {1}, {2}.", tileType, x, y));
            }
        }

        public void Clear()
        {
            Content.Unload();
        }

        public Rectangle Bounds(int x, int y)
        {
            return new Rectangle(x * Tiles.TileWidth, y * Tiles.TileHeight, Tiles.TileWidth, Tiles.TileHeight);
        }

        public Tiles_Collision Collision(int x, int y)
        {
            if (x < 0 || x >= Width)
                return Tiles_Collision.Impassable;

            if (y < 0 || y >= Height)
                return Tiles_Collision.Passable;

            return tiles[x, y].Collision;
        }

        public void Update(GameTime dt, KeyboardState ks, DisplayOrientation orientation)
        {
            if (!Player.isAlive || TimeLeft == TimeSpan.Zero)
                Player.Physics(dt);
            else if (LevelCompleted)
            {
                //Collect points for the seconds left on the timer.
                int seconds = (int)Math.Round(dt.ElapsedGameTime.TotalSeconds * 100.0f);
                seconds = Math.Min(seconds, (int)Math.Ceiling(TimeLeft.TotalSeconds));
                timeleft -= TimeSpan.FromSeconds(seconds);
                score += seconds * timerpoints;
            }
            else
            {
                timeleft -= dt.ElapsedGameTime;
                Player.Update(dt, ks, orientation);
                UpdateDiamonds(dt);
                UpdateCoins(dt);
                UpdateHealthPotions(dt);
                UpdateTimePotions(dt);
                UpdateInvisibilityPotion(dt);
                UpdateEnemy_1(dt);
                UpdateRandomPowerUp(dt);

                if (Player.BoundingRect.Top >= Height * Tiles.TileHeight)
                    Kill(null);

                if (Player.isAlive && Player.IsGround && Player.BoundingRect.Contains(exit))
                    ExitReached();
            }

            //makes sure the timer does not reach a negative number
            if (timeleft < TimeSpan.Zero)
                timeleft = TimeSpan.Zero;

            if (cdt > TimeSpan.Zero)
            {
                cdt -= dt.ElapsedGameTime;
            }

            if (Player.hasBoughtpowerup && cdt == TimeSpan.Zero)
            {
                player.hasBoughtpowerup = false;
            }
        }

        private void UpdateDiamonds(GameTime dt)
        {
            for (int i = 0; i < diamonds.Count; ++i)
            {
                Diamonds diam = diamonds[i];
                diam.Update(dt);

                if (diam.BoundingCircle.IntersectsWithRectangle(Player.BoundingRect))
                {
                    diamonds.RemoveAt(i--);
                    CollectDiamond(diam, Player);
                }
            }
        }

        private void UpdateCoins(GameTime dt)
        {
            for (int i = 0; i < coins.Count; ++i)
            {
                Coins coin = coins[i];
                coin.Update(dt);

                if (coin.BoundingCircle.IntersectsWithRectangle(Player.BoundingRect))
                {
                    coins.RemoveAt(i--);
                    CollectCoins(coin, Player);
                }
            }
        }

        private void UpdateHealthPotions(GameTime dt)
        {

            if (RandPotion == 0 && player.HasDrinkHealthPotion == true)
            {
                player.DrankHealthBoostRand(2);
                player.HasDrinkHealthPotion = false;
            }
            for (int i = 0; i < HealthPotions.Count; ++i)
            {
                Health_Boost health = HealthPotions[i];
                health.Update(dt);

                if (health.BoundingCircle.IntersectsWithRectangle(Player.BoundingRect))
                {
                    HealthPotions.RemoveAt(i--);
                    player.HasDrinkHealthPotion = true;

                    if(player.HasDrinkHealthPotion == true) 
                    {
                        CollectHealthPot(health, Player);
                        player.HasDrinkHealthPotion = false;

                    }
                }
            }
        }


        private void UpdateTimePotions(GameTime dt)
        {
            if (RandPotion == 1 && player.HasDrinkTimePotion == true)
            {
                timeleft += TimeSpan.FromSeconds(20);
                player.HasDrinkTimePotion = false;
            }
            for (int i = 0; i < TimePotions.Count; ++i)
            {
                Time_Boost Time = TimePotions[i];
                Time.Update(dt);

                if (Time.BoundingCircle.IntersectsWithRectangle(Player.BoundingRect))
                {
                    TimePotions.RemoveAt(i--);
                    timeleft += TimeSpan.FromSeconds(20);

                }
            }
        }

        private void UpdateInvisibilityPotion(GameTime dt)
        {
            double Duration = 10.0;
            // if the randPotion is equal to 2 the player is invisible 
            if (RandPotion == 2 && player.HasDrinkInvisibilityPotion == true)
            {
                //timer to time the length of powerup
                elapsedTime += dt.ElapsedGameTime.TotalSeconds;
                if (elapsedTime >= Duration)
                {
                    //powerup deactivated.
                    Player.HasDrinkInvisibilityPotion = false;
                    elapsedTime = 0;

                }
            }

            for (int i = 0; i < InvisibilityPotions.Count; ++i)
            {
                Invisibility_Potion invisibility = InvisibilityPotions[i];
                invisibility.Update(dt);

                if (invisibility.BoundingCircle.IntersectsWithRectangle(Player.BoundingRect))
                {
                    InvisibilityPotions.RemoveAt(i--);
                    Player.HasDrinkInvisibilityPotion = true;
                }
            }

            if (Player.HasDrinkInvisibilityPotion == true)
            {
                elapsedTime += dt.ElapsedGameTime.TotalSeconds;
                if (elapsedTime >= Duration)
                {

                    Player.HasDrinkInvisibilityPotion = false;
                    elapsedTime = 0;
                }
            }
        }

        private void UpdateEnemy_1(GameTime dt)
        {
            foreach (Enemy_1 Enemy_1 in enemy_1)
            {
                Enemy_1.Update(dt);
                // if the player is invisible theyre imune 
                if (Enemy_1.Bounding_Rectangle.Intersects(player.BoundingRect) && Player.HasDrinkInvisibilityPotion)
                {

                }
                else if(Enemy_1.Bounding_Rectangle.Intersects(player.BoundingRect) && Player.HasDrinkInvisibilityPotion == false)
                {
                    Kill(Enemy_1);
                }
            }
        }

        private void UpdateRandomPowerUp(GameTime dt)
        {
            for (int i = 0; i < randomPowerUp.Count; ++i)
            {
                RandomPowerUp randomPowerUps = randomPowerUp[i];
                randomPowerUps.Update(dt);
                // checks which power up to assign using boolean
                if (randomPowerUps.BoundingCircle.IntersectsWithRectangle(Player.BoundingRect) && Player.hasBoughtpowerup == true)
                {
                    RandPotion = random.Next(0, 3);
                    randomPowerUp.RemoveAt(i--);
                    GetRandomPowerUp(randomPowerUps, Player);
                    player.hasBought = true;

                    if (RandPotion == 0)
                    {
                        player.HasDrinkHealthPotion = true;

                    }
                    else if (RandPotion == 1)
                    {
                        player.HasDrinkTimePotion = true;
                    }
                    else if (RandPotion == 2)
                    {
                        player.HasDrinkInvisibilityPotion = true;
                    }

                }
            }
        }



        private void CollectDiamond(Diamonds diam, Player pl)
        {
            score += Diamonds.AddPoints;
            diam.Collected(pl);
        }

        private void CollectCoins(Coins coin, Player pl)
        {
            score += Coins.AddPoints;
            coin.CollectedCoins(pl);
        }

        private void GetRandomPowerUp(RandomPowerUp randomPowerUp, Player pl)
        {
            if (player.hasBoughtpowerup)
            {
                cdt = TimeSpan.FromMinutes(0.15f);
                randomPowerUp.GotRandomPowerUp(pl);
            }
        }

        private void CollectHealthPot(Health_Boost potion, Player pl)
        {
            pl.DrankHealthBoost(potion);

        }




        private void Kill(Enemy_1 by)
        {
            Player.Kill(by);
        }

        private void ExitReached()
        {
            Player.LevelCompleted();
            nextLevel.Play();
            levelCompleted = true;
        }

        public void Restart()
        {
            Player.ResetLevel(start);
        }

        public void Draw(GameTime dt, SpriteBatch spriteB)
        {
            DrawTiles(spriteB);

            foreach (Diamonds diam in diamonds)
                diam.Draw(dt, spriteB);

            foreach (Coins coin in coins)
                coin.Draw(dt, spriteB);



            foreach (Health_Boost health in HealthPotions)
                health.Draw(dt, spriteB);



            foreach (Time_Boost Time in TimePotions)
                Time.Draw(dt, spriteB);

            foreach (Invisibility_Potion invisible in InvisibilityPotions)
                invisible.Draw(dt, spriteB);


            foreach (RandomPowerUp rUP in randomPowerUp)
                rUP.Draw(dt, spriteB);


            foreach (Enemy_1 Enemy_1 in enemy_1)
                Enemy_1.Draw(dt, spriteB);


            Player.Draw(dt, spriteB);


        }

        private void DrawTiles(SpriteBatch spriteB)
        {
            for (int i = 0; i < Height; ++i)
            {
                for (int j = 0; j < Width; ++j)
                {
                    Texture2D texture = tiles[j, i].text;
                    if (texture != null)
                    {
                        Vector2 pos = new Vector2(j, i) * Tiles.Tilesize;
                        spriteB.Draw(texture, pos, Color.White);
                    }
                }
            }
        }
    }
}
