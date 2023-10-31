using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursework_Retake
{
    public class Button : Component_Skeleton
    {

        //Button settings
        private SpriteFont Font;
        private Texture2D Texture;

        //Mouse settings
        private MouseState currentMS;
        private MouseState previousMS;
        private bool target;
        public Color textcolor { get; set; }

        //Checks whether player has pressed the button.
        public event EventHandler Press;
        public bool Pressed
        {
            get; private set;
        }

        //Position
        public Vector2 Position
        {
            get; set;
        }

        //Specifies the size of a button.
        public Rectangle button
        {
            get { return new Rectangle((int)Position.X, (int)Position.Y, Texture.Width, Texture.Height); }
        }

        public string text { get; set; }


        public Button(SpriteFont font, Texture2D text)
        {
            Texture = text;
            Font = font;
            textcolor = Color.Red;
        }

        public override void Update(GameTime dt)
        {
            //previousMS = currentMS;
            //currentMS = Mouse.GetState();

            //var mouseBox = new Rectangle(currentMS.X, currentMS.Y, 1, 1);

            //target = false;

            //if (mouseBox.Intersects(button))
            //{
            //    target = true;
            //    if (currentMS.LeftButton == ButtonState.Released && previousMS.LeftButton == ButtonState.Pressed)
            //        Press?.Invoke(this, new EventArgs());
            //}

            previousMS = currentMS;
            currentMS = Mouse.GetState();

            var mouseBox = new Rectangle(currentMS.X, currentMS.Y, 1, 1);

            target = mouseBox.Intersects(button);

            if (target && currentMS.LeftButton == ButtonState.Released && previousMS.LeftButton == ButtonState.Pressed)
            {
                Press?.Invoke(this, new EventArgs());
            }
        }

        public override void Draw(GameTime dt, SpriteBatch spriteB)
        {
            ////Color used for highlighting of the button
            //Color color = Color.Blue;

            ////if mouse will hover the button, the button should get highlighted.
            //if (target)
            //{
            //    color = Color.Red;
            //}

            ////Used for Text
            //if (!string.IsNullOrEmpty(text))
            //{
            //    float x = button.X + button.Width / 2 - Font.MeasureString(text).X / 2;
            //    float y = button.Y + button.Height / 2 - Font.MeasureString(text).Y / 2;

            //    spriteB.Draw(Texture, button, color);
            //    spriteB.DrawString(Font, text, new Vector2(x, y), textcolor);
            //}

            // Set the default color to be used for the button.
            Color color = Color.White;

            // If the mouse hovers over the button, change the color to red for highlighting.
            if (target)
            {
                color = Color.DarkCyan;
            }

            // Check if the button has any text.
            if (!string.IsNullOrEmpty(text))
            {
                // Calculate the position to center the text on the button.
                float x = button.X + button.Width / 2 - Font.MeasureString(text).X / 2;
                float y = button.Y + button.Height / 2 - Font.MeasureString(text).Y / 2;

                // Draw the button with the calculated color.
                spriteB.Draw(Texture, button, color);

                // Draw the text on the button.
                spriteB.DrawString(Font, text, new Vector2(x, y), textcolor);
            }
        }
    }
}
