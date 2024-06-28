using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using Coursework_Retake.Collision_Types;
using Coursework_Retake.World;
using Coursework_Retake.items;

namespace Coursework_Retake
{
    class Player
    {
        //Animations
        private Animation idle;
        private Animation run;
        private Animation jump;
        private Animation celebrate;
        private Animation die;
        public  PlayerAnimator sprite;
        private Color colour = new Color();


        public int Health
        {
            get { return health; }
        }
        int health;
        public float Speed;
        public bool isAlive;
        public bool hasBought;
        public bool hasBoughtpowerup;
        public bool HasDrinkInvisibilityPotion;
        public bool HasDrinkTimePotion;
        public bool HasDrinkHealthPotion;

        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }
        Vector2 position;
        public Vector2 Velocity
        {
            get { return velocity; }
            set { velocity = value; }
        }
        Vector2 velocity;


        private SpriteEffects flip = SpriteEffects.None;

        //Sounds
        private SoundEffect lose;
        private SoundEffect damage;
        private SoundEffect healthPotion;

        //Level
        public Level Level
        {
            get { return level; }
        }
        Level level;


        //Input
        KeyboardState currentKS;
        KeyboardState previousKS;
        MouseState currentMS;
        MouseState previousMS;

        //Horizontal movement
        private const float Acceleration = 13000.0f;
        private const float MaxSpeed = 1750.0f;

        //Vertical Movement
        private const float MaxJump = 0.35f;
        private const float JumpVel = -3500.0f;
        private const float GravityAcceleration = 3400.0f;
        private const float MaxFallingSpeed = 550.0f;
        private const float JumpControlPower = 0.14f;

        //Drag Factors
        private const float GroundDragFactor = 0.48f;
        private const float AirDragFactor = 0.58f;

        public bool isJumping;
        private bool wasJumping;
        private float jumpTime;
        public bool IsGround
        {
            get { return isGround; }
        }
        bool isGround;

        private Rectangle Bounds;

        public Rectangle BoundingRect
        {
            get
            {
                int left = (int)Math.Round(Position.X - sprite.Origin.X) + Bounds.X;
                int top = (int)Math.Round(Position.Y - sprite.Origin.Y) + Bounds.Y;
                int width = Bounds.Width;
                int height = Bounds.Height;

                return new Rectangle(left, top, width, height);
            }
        }

        private float PrevBottom;
        public Player(Level level, Vector2 pos)
        {
            this.level = level;
            LoadContent();
            Initialise();
            ResetLevel(pos);
        }

        public void ResetLevel(Vector2 position)
        {
            Position = position;
            Velocity = Vector2.Zero;
            isAlive = true;
            sprite.Play(idle);
        }

        public void Initialise()
        {
            //Initialises texture and attributes of the player
            health = 3;
            Speed = 5f;
            isAlive = true;
            hasBought = false;
            hasBoughtpowerup = false;
            HasDrinkHealthPotion = false;
            HasDrinkInvisibilityPotion = false;
            HasDrinkTimePotion = false;
        }

        public void LoadContent()
        {
            // Load animations
            idle = new Animation(Level.Content.Load<Texture2D>("Player/Idle"), 0.1f, true);
            celebrate = new Animation(Level.Content.Load<Texture2D>("Player/Celebrate"), 0.1f, false);
            jump = new Animation(Level.Content.Load<Texture2D>("Player/Jump"), 0.1f, false);
            run = new Animation(Level.Content.Load<Texture2D>("Player/Run"), 0.1f, true);
            die = new Animation(Level.Content.Load<Texture2D>("Player/Die"), 0.1f, false);

            //Load Sounds
            lose = Level.Content.Load<SoundEffect>("Audio/Lose");
            //damage = Level.Content.Load<SoundEffect>("Audio/Dmgtaken");
            healthPotion = Level.Content.Load<SoundEffect>("Audio/healthpotion");

            // Calculate bounds within texture size.            
            int width = (int)(idle.FrameWidth * 0.4);
            int left = (idle.FrameWidth - width) / 2;
            int height = (int)(idle.FrameWidth * 0.8);
            int top = idle.FrameHeight - height;

            Bounds = new Rectangle(left, top, width, height);
        }

        public void Update(GameTime gameTime, KeyboardState ks, DisplayOrientation orientation)
        {
            currentKS = Keyboard.GetState();
            Physics(gameTime);

            // Check if the player is alive and on the ground.
            if (isAlive && isGround)
            {
                if (Math.Abs(Velocity.X) - 0.1f > 0)
                {
                    sprite.Play(run); // Play the running animation.
                }
                else
                {
                    sprite.Play(idle); // Play the idle animation when not moving.
                }
            }

            // Reset speed and jumping status for the next update.
            Speed = 0.0f;
            isJumping = false;
        }



        public void Physics(GameTime dt)
        {
            float timePassedSinceLastFrame = (float)dt.ElapsedGameTime.TotalSeconds;
            Vector2 previousPosition = Position;
            velocity.X += Speed * Acceleration * timePassedSinceLastFrame;

            velocity.Y = MathHelper.Clamp(velocity.Y + GravityAcceleration * timePassedSinceLastFrame, -MaxFallingSpeed, MaxFallingSpeed);

            velocity.Y = Jumping(dt, velocity.Y);

            float dragFactor = IsGround ? GroundDragFactor : AirDragFactor;
            velocity.X *= dragFactor;

            velocity.X = MathHelper.Clamp(velocity.X, -MaxSpeed, MaxSpeed);

            // Apply velocity to update the player's position.
            Position += velocity * timePassedSinceLastFrame;
            Position = new Vector2((float)Math.Round(Position.X), (float)Math.Round(Position.Y));

            // Check for collisions and adjust position accordingly.
            Collisions();

            // Check if the position was updated in both X and Y directions. If not, reset the corresponding velocity.
            if (Position.X == previousPosition.X)
                velocity.X = 0;

            if (Position.Y == previousPosition.Y)
                velocity.Y = 0;
        }

        private void Collisions()
        {
            Rectangle bounds = BoundingRect;
            int left = (int)Math.Floor((float)bounds.Left / Tiles.TileWidth);
            int right = (int)Math.Ceiling(((float)bounds.Right / Tiles.TileWidth)) - 1;
            int top = (int)Math.Floor((float)bounds.Top / Tiles.TileHeight);
            int bottom = (int)Math.Ceiling(((float)bounds.Bottom / Tiles.TileHeight)) - 1;



            isGround = false;

            for (int y = top; y <= bottom; ++y)
            {
                for (int x = left; x <= right; ++x)
                {
                    Tiles_Collision collision = Level.Collision(x, y);
                    if (collision != Tiles_Collision.Passable)
                    {
                        
                        Rectangle tileBounds = Level.Bounds(x, y);
                        Vector2 depth = RectangleExtensions.Intersection_Depth(bounds, tileBounds);
                        if (depth != Vector2.Zero)
                        {
                            float absDepthX = Math.Abs(depth.X);
                            float absDepthY = Math.Abs(depth.Y);

                            if (absDepthY < absDepthX || collision == Tiles_Collision.Platform)
                            {
                                if (PrevBottom <= tileBounds.Top)
                                    isGround = true;

                                if (collision == Tiles_Collision.Impassable || isGround)
                                {
                                    Position = new Vector2(Position.X, Position.Y + depth.Y);

                                    bounds = BoundingRect;
                                }
                            }
                            else if (collision == Tiles_Collision.Impassable) // Ignore platforms.
                            {
                                Position = new Vector2(Position.X + depth.X, Position.Y);

                                bounds = BoundingRect;
                            }
                            else if (collision == Tiles_Collision.Lava)
                            {
                                level.Restart();
                                health--;

                                if (health <= 0)
                                {
                                    Kill(null);
                                }
                            }
                        }
                    }
                }
            }
            PrevBottom = bounds.Bottom;
        }

        public void Kill(Enemy_1 by)
        {
            isAlive = false;

            if (by != null)
            {
                sprite.Play(die);
            }
            lose.Play();
        }

        public void LevelCompleted()
        {
            hasBought = false;
            hasBoughtpowerup = false;
            HasDrinkInvisibilityPotion = false;
            sprite.Play(celebrate);
        }

        private float Jumping(GameTime gameTime, float vel)
        {
            if (isJumping)
            {
                if ((!wasJumping && IsGround) || jumpTime > 0.0f)
                {
                    jumpTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
                    sprite.Play(jump);
                }

                if (0.0f < jumpTime && jumpTime <= MaxJump)
                {
                    vel = JumpVel * (1.0f - (float)Math.Pow(jumpTime / MaxJump, JumpControlPower));
                }
                else
                {
                    jumpTime = 0.0f;
                }
            }
            else
            {
                jumpTime = 0.0f;
            }

            wasJumping = isJumping;
            return vel;
        }

        public void DrankHealthBoost(Health_Boost potion)
        {
            health += 1;
        }


        public void DrankHealthBoostRand(int HealthInc)
        {
            health += HealthInc;
        }



        public void Draw(GameTime gameTime, SpriteBatch spriteB)
        {
            SpriteEffects flip = Velocity.X > 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;


            // Draw the sprite with the appropriate flip direction at the current position
            if (HasDrinkInvisibilityPotion == true)
            {
                colour = new Color(1, 1, 1, 0.5f);
                sprite.Draw(gameTime, spriteB, Position, flip, colour);

            }
            else 
            {
                colour = new Color(1, 1, 1, 1f);
                sprite.Draw(gameTime, spriteB, Position, flip, colour);
            }


        }
    }
}
