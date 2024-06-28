using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursework_Retake
{
    class Animation
    {
        //Frames or textures
        public Texture2D Texture
        {
            get { return texture; }
        }
        Texture2D texture;

        //Duration of each frame.
        public float Time
        {
            get { return time; }
        }
        float time;

        public bool IsLooping
        {
            get { return isLooping; }
        }
        bool isLooping;

        //Amount of frames per animation.
        public int FrameCount
        {
            get { return Texture.Width / FrameWidth; }
        }

        public int FrameWidth
        {
            get { return Texture.Height; }
        }

        public int FrameHeight
        {
            get { return Texture.Height; }
        }

        public Animation(Texture2D texture, float timer, bool loop)
        {
            this.texture = texture;
            this.time = timer;
            this.isLooping = loop;
        }
    }
}
