using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using Microsoft.VisualBasic;
using Coursework_Retake.World;

namespace Coursework_Retake
{
    class Enemy_1
    {
        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }
        private Vector2 position;
        private Color colour;

        public Level Level
        {
            get { return level; }
        }
        Level level;

        public bool IsSpotted
        {
            get { return IsSpotted; }
            set { IsSpotted = value; }
        }

        //private FSM fsm;

        private Rectangle Bounds;
        public Rectangle Bounding_Rectangle
        {
            get
            {
                int left = (int)Math.Round(Position.X - enemy_1.Origin.X) + Bounds.X;
                int top = (int)Math.Round(Position.Y - enemy_1.Origin.Y) + Bounds.Y;

                return new Rectangle(left - 10, top - 20, Bounds.Width * 2, (int)(Bounds.Height * 1.5f));
            }
        }

        private Rectangle Eyes;

        public float speed = 100.0f;
        public int health = 1;

        private PlayerAnimator enemy_1;
        private Animation Idle;

        enum Direction
        {
            Left = -1,
            Right = 1,
        }

        private Direction direction = Direction.Left;

        private float waiting;

        private const float maxWaiting = 0.5f;

        public Enemy_1(Level level, Vector2 position)
        {
            colour = new Color(1, 1, 1, 1f);
            this.level = level;
            this.position = position;

            //fsm = new FSM(this);

            //Initialise();
            Load();
        }



        public void Load()
        {
            Idle = new Animation(level.Content.Load<Texture2D>("Enemy/mageL"), 0.5f, true);
            enemy_1.Play(Idle);

            int width = (int)(Idle.FrameWidth * 0.35);
            int left = (Idle.FrameWidth - width) / 2;
            int height = (int)(Idle.FrameWidth * 0.7);
            int top = Idle.FrameHeight - height;
            Bounds = new Rectangle(left, top, width, height);

            Eyes = new Rectangle(left * 2, top * 2, width * 2, height * 2);
        }
        public void Initialise()
        {
            /*Idlestate idle = new Idlestate();
            Chasestate chase = new Chasestate();

            idle.AddTransition(new Transition(chase, () => Spotted));
            chase.AddTransition(new Transition(idle, () => !Spotted));

            fsm.AddState(idle);
            fsm.AddState(chase);

            fsm.Initialise("Idle");*/
        }
        public void Update(GameTime gameTime)
        {
            IdleAction(gameTime);
        }

        public void IdleAction(GameTime gameTime)
        {
            float Time_Passed_Since_Last_Update = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Calculate tile position based on the side we are walking towards.
            float posX = Position.X + Bounds.Width / 2 * (int)direction;
            int tileX = (int)Math.Floor(posX / Tiles.TileWidth) - (int)direction;
            int tileY = (int)Math.Floor(Position.Y / Tiles.TileHeight);

            if (waiting > 0)
            {
                // Wait for some amount of time.
                waiting = Math.Max(0.0f, waiting - (float)gameTime.ElapsedGameTime.TotalSeconds);
                if (waiting <= 0.0f)
                {
                    // Then turn around.
                    direction = (Direction)(-(int)direction);
                }
            }
            else
            {
                // If we are about to run into a wall or off a cliff, start waiting.
                if (level.Collision(tileX + (int)direction, tileY - 1) == Tiles_Collision.Impassable ||
                    level.Collision(tileX + (int)direction, tileY) == Tiles_Collision.Passable)
                {
                    waiting = maxWaiting;
                }
                else
                {
                    // Move in the current direction.
                    Vector2 velocity = new Vector2((int)direction * speed * Time_Passed_Since_Last_Update, 0.0f);
                    Position += velocity;

                }
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteB)
        {
            // Draw facing the way the enemy is moving.
            SpriteEffects flip = direction > 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            enemy_1.Draw(gameTime, spriteB, Position, flip, colour);
        }
    }
}


