using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Text.RegularExpressions;

namespace Game1.Terminal
{
    class Terminal
    {
        public SpriteBatch SpriteBatch { get; set; }
        public GameTime GameTime { get; set; }
        public SpriteFont Font { get; set; }

        public Terminal(SpriteBatch spriteBatch = null, GameTime gameTime = null, SpriteFont font = null)
        {
            SpriteBatch = spriteBatch;
            GameTime = gameTime;
            Font = font;
        }

        private StringBuilder input = new StringBuilder();

        public void Listen(KeyboardState keyboardState, KeyboardState previousKeyboardState)
        {
            foreach (Keys key in keyboardState.GetPressedKeys())
            {
                if (keyboardState.IsKeyDown(key) && previousKeyboardState.IsKeyUp(key))
                {
                    if (key == Keys.Space)
                    {
                        input.Append(" ");
                    }
                    else if (key == Keys.Enter)
                    {
                        input.Append(Reply(input.ToString()));
                        input.Append(Environment.NewLine);
                        input.Append("> ");
                    }
                    else if (key == Keys.Back)
                    {
                        if(input.Length > 0)
                        {
                            input.Length--;
                        }
                    }
                    else
                    {
                        input.Append(key);
                    }
                }
            }
            if (input.Length >= 0)
            {
                Print(input.ToString(), input.Length);
            }
        }

        private void Print(string input, int position = 0)
        {
            SpriteBatch.Begin();
            SpriteBatch.GraphicsDevice.Clear(Color.SlateGray);
            SpriteBatch.DrawString(Font, "> " + input, new Vector2(20, 20), Color.SeaShell);
            SpriteBatch.End();
        }

        private string Reply(string input)
        {
            return Environment.NewLine + "Ah yes of course my lord";
        }
    }
}
