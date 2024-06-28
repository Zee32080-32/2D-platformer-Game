using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Coursework_Retake
{
    struct PlayerAnimator
    {
        private float time;
        private Animation animation;
        int frameIndex;
        public Animation Animation => animation;
        public int FrameIndex => frameIndex;

        public Vector2 Origin => new Vector2(Animation.FrameWidth / 2.0f, Animation.FrameHeight);

        public int Width => Animation.FrameWidth;
        public int Height => Animation.FrameHeight;

        public void Play(Animation animation)
        {
            if (Animation == animation)
                return;

            // Start the new animation.
            this.animation = animation;
            this.frameIndex = 0;
            this.time = 0.0f;
        }

        public void Draw(GameTime gameTime, SpriteBatch sprite, Vector2 pos, SpriteEffects spriteEffects, Color colour)
        {

            //Throws error if no animation is playing.
            if (Animation == null)
                throw new NotSupportedException("No animation is currently playing.");

            time += (float)gameTime.ElapsedGameTime.TotalSeconds;
            while (time > Animation.Time)
            {
                time -= Animation.Time;

                if (Animation.IsLooping)
                {
                    frameIndex = (frameIndex + 1) % Animation.FrameCount;
                }
                else
                {
                    frameIndex = Math.Min(frameIndex + 1, Animation.FrameCount - 1);
                }
            }
            // Calculate the source rectangle of the current frame.
            Rectangle source = new Rectangle(FrameIndex * Animation.Texture.Height, 0, Animation.Texture.Height, Animation.Texture.Height);

            // Draw the current frame.
            sprite.Draw(Animation.Texture, pos, source, colour, 0.0f, Origin, 1.0f, spriteEffects, 0.0f);
        
        }
    }
}

