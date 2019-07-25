using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Newtonsoft.Json;
using System.Text.RegularExpressions;

namespace Game1.Terminal
{
    class Terminal
    {
        public SpriteBatch SpriteBatch { get; set; }
        public GameTime GameTime { get; set; }
        public SpriteFont Font { get; set; }

        private Texture2D terminalWindow = null;
        private Vector2 terminalPosition = new Vector2(20, 20);
        private RoundedRectangle roundedRec = new RoundedRectangle();
        public Terminal(SpriteBatch spriteBatch = null, GameTime gameTime = null, SpriteFont font = null)
        {
            SpriteBatch = spriteBatch;
            GameTime = gameTime;
            Font = font;
            if (SpriteBatch != null)
            {
                terminalWindow = roundedRec.CreateRoundedRectangleTexture(SpriteBatch.GraphicsDevice, 1560, 760, 10, 20, 2, new List<Color> { Color.SlateGray }, new List<Color> { Color.SlateBlue }, 1, 1);
            }
        }

        private StringBuilder input = new StringBuilder();
        private bool startUp = true;
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
                        input.Append(Environment.NewLine + Environment.NewLine + "> ");
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
                Print(input.ToString());
            }
            if (startUp && input.Length == 0)
            {
                input.Append(Reply("", "Hello I'm STEFAN. The 100% guarented non sentient companion" + Environment.NewLine + Environment.NewLine));
                Print(input.ToString());
                startUp = false;
            }
        }
        private void Print(string input)
        {
            SpriteBatch.Begin();
            if (terminalWindow != null)
            {
                SpriteBatch.Draw(terminalWindow, terminalPosition, Color.White);
            }
            if (startUp)
            {
                SpriteBatch.DrawString(Font, input, terminalPosition + new Vector2(20, 20), Color.SeaShell);
            }
            else
            {
                SpriteBatch.DrawString(Font, "> " + input, terminalPosition + new Vector2(20, 20), Color.SeaShell);
            }
            SpriteBatch.End();
        }

        private string Reply(string input, string replyOverride = null)
        {
            if (replyOverride != "" && replyOverride != null)
            {
                return Environment.NewLine + replyOverride;
            }
            string slicedInput = input.Split('>').Last().Replace(" ", "").ToLower();
            StreamReader r = new StreamReader(System.Reflection.Assembly.GetExecutingAssembly().Location.Replace("Game1.exe", "")  + "Terminal" + Path.DirectorySeparatorChar + "Conversation.Json");
            string json = r.ReadToEnd();
            List<TerminalConversation> tc= JsonConvert.DeserializeObject<List<TerminalConversation>>(json);
            string reply = "";
            foreach (TerminalConversation c in tc)
            {
                foreach (string i in c.Input.Value)
                {
                    if (i.Contains(slicedInput) && slicedInput != "")
                    {
                        Random rnd = new Random();
                        int replyIndex = rnd.Next(0, c.Reply.Value.Length);
                        reply = c.Reply.Value[replyIndex];
                    }
                }
            }
            if (reply == "")
            {
                reply = "That is not a known function or command. You can always try saying hello!";
            }
            return Environment.NewLine + Environment.NewLine + reply;
        }
    }
}
