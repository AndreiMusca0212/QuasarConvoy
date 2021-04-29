using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace QuasarConvoy.Models
{
    public class Input
    {
        static KeyboardState currentState;
        static KeyboardState prevState;
        public Keys Up { set; get; }
        public Keys Down { set; get; }
        public Keys Left { set; get; }
        public Keys Right { set; get; }
        public Keys Shoot { set; get; }
        public Keys Reset { set; get; }
        public Keys NextShip { set; get; }
        public Keys ZoomIN { set; get; }
        public Keys ZoomOUT { set; get; }

        #region static

        public Input()
        { }
        public static bool IsPressed(Keys key)
        {
            prevState = currentState;
            currentState = Keyboard.GetState();
            return Keyboard.GetState().IsKeyDown(key);
        }

        public static bool WasPressed(Keys key)
        {
            return IsPressed(key) && prevState.IsKeyUp(key);
        }

        public static bool WasPressedOnce(Keys key)
        {
            return !IsPressed(key) && prevState.IsKeyDown(key);
        }

        
        #endregion

        #region non-static
        public KeyboardState _prevState;

        public Input(KeyboardState ks)
        {
            _prevState = ks;
        }

        public void Refresh()
        {
            _prevState = Keyboard.GetState();
        }

        public bool IsPressed(Keys key,KeyboardState ks)
        {
            return ks.IsKeyDown(key);
        }

        public bool WasPressed(Keys key, KeyboardState ks)
        {
            return IsPressed(key,ks) && _prevState.IsKeyUp(key);
        }

        public bool WasPressedOnce(Keys key, KeyboardState ks)
        {
            return !IsPressed(key,ks) && _prevState.IsKeyDown(key);
        }
        #endregion
    }
}
