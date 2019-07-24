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

        public Terminal(SpriteBatch spriteBatch = null, GameTime gameTime = null, SpriteFont font = null)
        {
            SpriteBatch = spriteBatch;
            GameTime = gameTime;
            Font = font;
        }

        private StringBuilder input = new StringBuilder();
        private int startUp = 0;
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
            if (startUp == 0)
            {
                input.Append(Reply("", "Hello I'm STEFAN. The 100% guarented non sentient companion" + Environment.NewLine + Environment.NewLine));
                Print(input.ToString());
                startUp++;
            }
        }

        private void Print(string input, int position = 0)
        {
            SpriteBatch.Begin();
            SpriteBatch.GraphicsDevice.Clear(Color.SlateGray);
            if (startUp == 0)
            {
                SpriteBatch.DrawString(Font, input, new Vector2(20, 20), Color.SeaShell);
            }
            else
            {
                SpriteBatch.DrawString(Font, "> " + input, new Vector2(20, 20), Color.SeaShell);
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
            return Environment.NewLine + reply;
        }
    }
}
