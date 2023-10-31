using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Coursework_Retake.Game_Controls
{
    public delegate void Action(Button_States Button_State, Vector2 amount);

    class Command_Manager
    {
        private Input_Listeners Input;

        private Dictionary<Keys, Action> m_KeyBindings = new Dictionary<Keys, Action>();
        private Dictionary<MouseButton, Action> m_MouseButtonBindings = new Dictionary<MouseButton, Action>();

        public Command_Manager()
        {
            Input = new Input_Listeners();

            Input.KeyDown += KeyDown;
            Input.KeyPressed += KeyPressed;
            Input.KeyUp += KeyUp;
            Input.MouseButtonDown += MouseButtonDown;

        }

        public void Update()
        {

            Input.Update();
        }

        public void KeyDown(object sender, Keyboard_Events a)
        {
            Action action = m_KeyBindings[a.Keys];

            if (action != null)
            {
                action(Button_States.DOWN, new Vector2(1.0f));
            }
        }

        public void KeyUp(object sender, Keyboard_Events a)
        {
            Action action = m_KeyBindings[a.Keys];

            if (action != null)
            {
                action(Button_States.UP, new Vector2(1.0f));
            }
        }

        public void KeyPressed(object sender, Keyboard_Events a)
        {
            Action action = m_KeyBindings[a.Keys];

            if (action != null)
            {
                action(Button_States.PRESSED, new Vector2(1.0f));
            }
        }

        public void MouseButtonDown(object sender, Mouse_Events a)
        {
            Action action = m_MouseButtonBindings[a.Button];

            if (action != null)
            {
                action(Button_States.DOWN, new Vector2(a.CurrState.X, a.CurrState.Y));
            }
        }

        public void AddKeyboardBinding(Keys key, Action action)
        {
            Input.AddKButton(key);

            // Add the binding to the command map
            m_KeyBindings.Add(key, action);
        }

        public void AddMouseBinding(MouseButton button, Action action)
        {

            Input.AddClick(button);

            // Add the binding to the command map
            m_MouseButtonBindings.Add(button, action);
        }
    }

}
