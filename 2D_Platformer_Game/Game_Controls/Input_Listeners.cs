using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursework_Retake.Game_Controls
{
    class Input_Listeners
    {
        //List of Keys
        public HashSet<Keys> KeyboardButtons;
        public HashSet<MouseButton> MouseButtons;

        //Keyboard States
        private KeyboardState PreviousKeyState { get; set; }
        private KeyboardState CurrentKeyState { get; set; }

        //Mouse States
        private MouseState PrevMS { get; set; }
        private MouseState CurrMS { get; set; }

        //Event Handlers
        public event EventHandler<Keyboard_Events> KeyDown = delegate { };
        public event EventHandler<Keyboard_Events> KeyPressed = delegate { };
        public event EventHandler<Keyboard_Events> KeyUp = delegate { };
        public event EventHandler<Mouse_Events> MouseButtonDown = delegate { };

        public Input_Listeners()
        {
            //Keyboard
            CurrentKeyState = Keyboard.GetState();
            PreviousKeyState = CurrentKeyState;

            KeyboardButtons = new HashSet<Keys>();

            CurrMS = Mouse.GetState();
            PrevMS = CurrMS;

            MouseButtons = new HashSet<MouseButton>();
        }

        public void AddKButton(Keys key)
        {
            KeyboardButtons.Add(key);
        }

        public void AddClick(MouseButton button)
        {
            MouseButtons.Add(button);
        }

        private void Keyboard_Events()
        {
            foreach (Keys key in KeyboardButtons)
            {
                //Checks whether key is currently pressed down
                if (CurrentKeyState.IsKeyDown(key))
                {
                    if (KeyDown != null)
                    {
                        KeyDown(this, new Keyboard_Events(key, CurrentKeyState, PreviousKeyState));
                    }
                }

                //Checks whether key was released
                if (PreviousKeyState.IsKeyDown(key) && CurrentKeyState.IsKeyUp(key))
                {
                    if (KeyUp != null)
                    {
                        KeyUp(this, new Keyboard_Events(key, CurrentKeyState, PreviousKeyState));
                    }
                }

                //Checks whether key was pressed 
                if (PreviousKeyState.IsKeyUp(key) && CurrentKeyState.IsKeyDown(key))
                {
                    if (KeyPressed != null)
                    {
                        KeyPressed(this, new Keyboard_Events(key, CurrentKeyState, PreviousKeyState));
                    }
                }
            }
        }

        private void Mouse_Events()
        {
            foreach (MouseButton mouse in MouseButtons)
            {
                if (CurrMS.LeftButton == ButtonState.Pressed)
                {
                    if (MouseButtonDown != null)
                        MouseButtonDown(this, new Mouse_Events(mouse, CurrMS, PrevMS));
                }
            }
        }

        public void Update()
        {
            PreviousKeyState = CurrentKeyState;
            CurrentKeyState = Keyboard.GetState();
            PrevMS = CurrMS;
            CurrMS = Mouse.GetState();

            Keyboard_Events();
            Mouse_Events();
        }
    }
}
