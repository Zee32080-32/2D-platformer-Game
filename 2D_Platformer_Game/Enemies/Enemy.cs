using System;
using System.Numerics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;


namespace Coursework_Retake.Enemies
{
    class Enemy
    {
        ContentManager content;

        //Texture
        Texture2D texture;

        //Level to calculate the distance. 
        Game_State gs;

        //Basic settings for the enemy. 
        private float speed;
        private float stop;
        private float range;
        private float retreat;
        private float health;

        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }
        Vector2 position;

        //States
        AIstate currentState;

        public enum AIstate
        {
            idle,
            chase,
            attack,
            retreat,
            death
        }

        public Enemy()
        {
            texture = content.Load<Texture2D>("Enemy\\mageL");
            speed = 5.0f;
            stop = 3.0f;
            retreat = 1.0f;
            range = 6.0f;
            health = 1.0f;
        }


        public void Update()
        {
            StateUpdates();

            StateDecisions();
        }

        private void StateUpdates()
        {
            if ((gs.level.Player.Position.X - this.Position.X) > range)
            {
                currentState = AIstate.idle;
            }
            else if ((gs.level.Player.Position.X - this.Position.X) <= range && (gs.level.Player.Position.X - this.Position.X) > stop)
            {
                currentState = AIstate.chase;
            }
            else if ((gs.level.Player.Position.X - this.Position.X) <= range && (gs.level.Player.Position.X - this.Position.X) <= stop && (gs.level.Player.Position.X - this.Position.X) > retreat)
            {
                currentState = AIstate.attack;
            }
            else if ((gs.level.Player.Position.X - this.Position.X) <= range && (gs.level.Player.Position.X - this.Position.X) <= stop && (gs.level.Player.Position.X - this.Position.X) <= retreat)
            {
                currentState = AIstate.retreat;
            }
            else if (this.health <= 0)
            {
                currentState = AIstate.death;
            }
        }
        private void StateDecisions()
        {
            switch (currentState)
            {
                case AIstate.idle:

                    break;
                case AIstate.chase:
                    break;
                case AIstate.attack:
                    break;
                case AIstate.retreat:
                    break;
                case AIstate.death:
                    break;
                default:
                    break;
            }
        }
    }
}

