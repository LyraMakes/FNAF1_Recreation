using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace FNAF1_Recreation
{
    static class Input
    {
        public static MouseState MouseState { get; private set; }
        public static MouseState PrevMouseState { get; private set; }

        private static KeyboardState KeyboardState;
        private static KeyboardState PrevKeyboardState;

        public static bool GetMouseDown() => MouseState.LeftButton == ButtonState.Pressed && PrevMouseState.LeftButton != ButtonState.Pressed;
        public static bool IsMouseDown() => MouseState.LeftButton == ButtonState.Pressed;
        public static bool GetMouseUp() => MouseState.LeftButton == ButtonState.Released && PrevMouseState.LeftButton != ButtonState.Released;

        public static Vector2 MousePos { get { return new Vector2(MouseState.X, MouseState.Y); } }

        public static bool GetKeyDown(Keys k) => KeyboardState.IsKeyDown(k) && !PrevKeyboardState.IsKeyDown(k);
        public static bool IsKeyDown(Keys k) => KeyboardState.IsKeyDown(k);
        public static bool GetKeyUp(Keys k) => !KeyboardState.IsKeyDown(k) && PrevKeyboardState.IsKeyDown(k);

        public static void Initialize()
        {
            MouseState = Mouse.GetState();
            PrevMouseState = MouseState;

            KeyboardState = Keyboard.GetState();
            PrevKeyboardState = KeyboardState;
        }

        public static void Latch()
        {
            PrevMouseState = MouseState;
            MouseState = Mouse.GetState();

            PrevKeyboardState = KeyboardState;
            KeyboardState = Keyboard.GetState();
        }
    }
}
