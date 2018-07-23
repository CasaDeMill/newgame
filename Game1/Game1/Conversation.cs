using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Text.RegularExpressions;

namespace Game1
{
    class Conversation
    {
        public string[] Talk { get; set; }



        public bool PrintText { get; set; }

        public SpriteFont Font { get; set; }

        public Vector2 TextPosition { get; set; }

        public bool SpeedUp { get; set; }

        public bool IsReset { get; set; }

        public bool Close { get; set; }

        public long ConversationIndexNumber { get; set; }

        public int ConversationIndexNumber2 { get; set; }

        public bool Typing { get; set; }

        public GamePadState gamePadState;

        public int LastKey { get; set; }



        public int Count { get; set; }



        private int timer = 0;

        private int number = 0;

        private StringBuilder sb = new StringBuilder();

        private bool charPrinted = false;



        private int speed = 0;

        private List<string> conversations = new List<string>();

        private string merged = "";





        private string tempString = "";

        private bool charsRemoved = false;



        private int conversationCount = 0;

        private Char[] printChars;



        private bool clicked = false;

        private int choiceState = 1;

        private string[] choiceStrings;









        public Conversation(string[] talk, bool printText, SpriteFont font, Vector2 textPosition, bool speedUp, bool isReset, bool close, long conversationIndexNumber, int conversationIndexNumber2, int count, bool typing, int lastKey)

        {

            Talk = talk;

            Close = close;

            PrintText = printText;

            Font = font;

            TextPosition = textPosition;

            SpeedUp = speedUp;

            IsReset = isReset;

            ConversationIndexNumber = conversationIndexNumber;

            ConversationIndexNumber2 = conversationIndexNumber2;

            this.Typing = typing;

            Count = count;

            this.LastKey = lastKey;



        }





        public void MakeChars()

        {



            Close = false;

            if (IsReset == false)

                for (int k = 0; k < Talk.Length; k++)

                {



                    Talk[k] += "\r\n";





                }



            for (int i = 1; i < 99; i++)

            {

                ConversationIndexNumber2 = i;

                foreach (string s in Talk)

                {

                    if (s.Contains("%" + ConversationIndexNumber.ToString() + ";" + i))

                    {

                        tempString += s;



                    }

                }



                conversations.Add(tempString);

                tempString = "";



            }





        }



        public void SpeedUpText()

        {

            if (SpeedUp)

            {

                speed = 1;

            }

            if (!SpeedUp)

            {

                speed = 20;

            }

        }



        public void Reset()

        {

            conversations.Clear();

            conversationCount = 0;

            tempString = "";

            choiceStrings = null;

            Count = 0;

            merged = "";

            sb.Length = 0;

            number = 0;

            charsRemoved = false;

            Typing = false;



        }



        public virtual void Print(SpriteBatch spriteBatch, GameTime gameTime, KeyboardState keyboardState, KeyboardState prevKeyboardState)

        {

            Count = Regex.Matches(conversations[conversationCount], "\r\n").Count;



            if (charsRemoved == false)

            {

                if (conversations[conversationCount].Contains("%"))

                {

                    conversations[conversationCount] = Regex.Replace(conversations[conversationCount], "%" + ConversationIndexNumber.ToString() + ";" + @"[\d-]" + ":", "");

                    charsRemoved = true;

                }



            }

            printChars = conversations[conversationCount].ToCharArray();



            if (number <= printChars.Length - 1)

            {

                Typing = true;

                timer += gameTime.ElapsedGameTime.Milliseconds;

                if (charPrinted == false)

                {



                    sb.Append(printChars[number]);

                    charPrinted = true;







                }





                spriteBatch.DrawString(Font, sb.ToString(), TextPosition, Color.Red);





                if (timer > speed)

                {











                    number++;

                    charPrinted = false;



                    timer = 0;









                }









            }

            else

            {







                Typing = false;









                merged = new string(printChars);

                if (merged == "")

                {

                    Close = true;

                }

                choiceStrings = Regex.Split(merged, @"[\d-]" + ".");

                timer += gameTime.ElapsedGameTime.Milliseconds;

                if (timer > 300) { clicked = false; }

                #region Keyboard Choice control(Up down and space and not using number keys)



                gamePadState = GamePad.GetState(PlayerIndex.One);



                if (conversations[conversationCount].Contains("1."))

                {



                    if (keyboardState.IsKeyDown(Keys.Down) && !clicked) { choiceState += 1; clicked = true; timer = 0; }

                    if (keyboardState.IsKeyDown(Keys.Up) && !clicked) { choiceState -= 1; clicked = true; timer = 0; }

                    if (keyboardState.IsKeyDown(Keys.S) && !clicked) { choiceState += 1; clicked = true; timer = 0; }

                    if (keyboardState.IsKeyDown(Keys.W) && !clicked) { choiceState -= 1; clicked = true; timer = 0; }

                    if (gamePadState.ThumbSticks.Left.Y < -0.1f && !clicked) { choiceState += 1; clicked = true; timer = 0; }

                    if (gamePadState.ThumbSticks.Left.X > 0.1f && !clicked) { choiceState -= 1; clicked = true; timer = 0; }





                    if (choiceState > choiceStrings.Length - 1) { choiceState = 1; }

                    if (choiceState < 1) { choiceState = choiceStrings.Length - 1; }



                }





                #endregion

                #region CHOICES (VERY COMPLEX ASK MARTIN)

                //INTITIAL CHOICE

                if (conversations[conversationCount].Contains("1."))

                {



                    if ((keyboardState.IsKeyDown(Keys.D1) && !clicked && LastKey == 0) || (keyboardState.IsKeyDown(Keys.Space) && !clicked && LastKey == 0 && choiceState == 1 && prevKeyboardState.IsKeyUp(Keys.Space)))

                    {

                        LastKey = 1;

                        ConversationIndexNumber = 2;

                        IsReset = true;

                        Reset();

                        MakeChars();



                        clicked = true;

                        timer = 0;



                    }

                    if ((keyboardState.IsKeyDown(Keys.D2) && !clicked && LastKey == 0) || (keyboardState.IsKeyDown(Keys.Space) && !clicked && LastKey == 0 && choiceState == 2 && prevKeyboardState.IsKeyUp(Keys.Space)))

                    {

                        LastKey = 2;

                        ConversationIndexNumber = 3;

                        IsReset = true;

                        Reset();

                        MakeChars();



                        clicked = true;

                        timer = 0;

                    }

                    if ((keyboardState.IsKeyDown(Keys.D3) && !clicked && LastKey == 0) || (keyboardState.IsKeyDown(Keys.Space) && !clicked && LastKey == 0 && choiceState == 3 && prevKeyboardState.IsKeyUp(Keys.Space)))

                    {

                        LastKey = 3;

                        ConversationIndexNumber = 4;

                        IsReset = true;

                        Reset();

                        MakeChars();



                        clicked = true;

                        timer = 0;

                    }



                    //1



                    if ((keyboardState.IsKeyDown(Keys.D1) && LastKey == 1 && !clicked) || (keyboardState.IsKeyDown(Keys.Space) && !clicked && LastKey == 1 && choiceState == 1 && prevKeyboardState.IsKeyUp(Keys.Space)))

                    {

                        LastKey = 1;

                        ConversationIndexNumber = int.Parse((ConversationIndexNumber.ToString()) + 1);

                        //ConversationIndexNumber = 21;

                        IsReset = true;

                        Reset();

                        MakeChars();



                        clicked = true;

                        timer = 0;

                    }

                    if ((keyboardState.IsKeyDown(Keys.D1) && LastKey == 2 && !clicked) || (keyboardState.IsKeyDown(Keys.Space) && !clicked && LastKey == 2 && choiceState == 1 && prevKeyboardState.IsKeyUp(Keys.Space)))

                    {

                        LastKey = 1;

                        ConversationIndexNumber = int.Parse((ConversationIndexNumber.ToString()) + 1);

                        //ConversationIndexNumber = 31;

                        IsReset = true;

                        Reset();

                        MakeChars();



                        clicked = true;

                        timer = 0;

                    }

                    if ((keyboardState.IsKeyDown(Keys.D1) && LastKey == 3 && !clicked) || (keyboardState.IsKeyDown(Keys.Space) && !clicked && LastKey == 3 && choiceState == 1 && prevKeyboardState.IsKeyUp(Keys.Space)))

                    {

                        LastKey = 1;

                        ConversationIndexNumber = int.Parse((ConversationIndexNumber.ToString()) + 1);

                        //ConversationIndexNumber = 41;

                        IsReset = true;

                        Reset();

                        MakeChars();



                        clicked = true;

                        timer = 0;

                    }



                    //2



                    if ((keyboardState.IsKeyDown(Keys.D2) && LastKey == 1 && !clicked) || (keyboardState.IsKeyDown(Keys.Space) && !clicked && LastKey == 1 && choiceState == 2 && prevKeyboardState.IsKeyUp(Keys.Space)))

                    {

                        LastKey = 2;

                        ConversationIndexNumber = int.Parse((ConversationIndexNumber.ToString()) + 2);

                        //ConversationIndexNumber = 22;

                        IsReset = true;

                        Reset();

                        MakeChars();



                        clicked = true;

                        timer = 0;

                    }

                    if ((keyboardState.IsKeyDown(Keys.D2) && LastKey == 2 && !clicked) || (keyboardState.IsKeyDown(Keys.Space) && !clicked && LastKey == 2 && choiceState == 2 && prevKeyboardState.IsKeyUp(Keys.Space)))

                    {

                        LastKey = 2;

                        ConversationIndexNumber = int.Parse((ConversationIndexNumber.ToString()) + 2);

                        //ConversationIndexNumber = 32;

                        IsReset = true;

                        Reset();

                        MakeChars();



                        clicked = true;

                        timer = 0;

                    }

                    if ((keyboardState.IsKeyDown(Keys.D2) && LastKey == 3 && !clicked) || (keyboardState.IsKeyDown(Keys.Space) && !clicked && LastKey == 3 && choiceState == 2 && prevKeyboardState.IsKeyUp(Keys.Space)))

                    {

                        LastKey = 2;

                        ConversationIndexNumber = int.Parse((ConversationIndexNumber.ToString()) + 2);

                        //ConversationIndexNumber = 42;

                        IsReset = true;

                        Reset();

                        MakeChars();



                        clicked = true;

                        timer = 0;

                    }



                    //3



                    if ((keyboardState.IsKeyDown(Keys.D3) && LastKey == 1 && !clicked) || (keyboardState.IsKeyDown(Keys.Space) && !clicked && LastKey == 1 && choiceState == 3 && prevKeyboardState.IsKeyUp(Keys.Space)))

                    {

                        LastKey = 3;

                        ConversationIndexNumber = int.Parse((ConversationIndexNumber.ToString()) + 3);

                        //ConversationIndexNumber = 23;

                        IsReset = true;

                        Reset();

                        MakeChars();



                        clicked = true;

                        timer = 0;

                    }

                    if ((keyboardState.IsKeyDown(Keys.D3) && LastKey == 2 && !clicked) || (keyboardState.IsKeyDown(Keys.Space) && !clicked && LastKey == 2 && choiceState == 3 && prevKeyboardState.IsKeyUp(Keys.Space)))

                    {

                        LastKey = 3;

                        ConversationIndexNumber = int.Parse((ConversationIndexNumber.ToString()) + 3);

                        //ConversationIndexNumber = 33;

                        IsReset = true;

                        Reset();

                        MakeChars();



                        clicked = true;

                        timer = 0;

                    }

                    if ((keyboardState.IsKeyDown(Keys.D3) && LastKey == 3 && !clicked) || (keyboardState.IsKeyDown(Keys.Space) && !clicked && LastKey == 3 && choiceState == 3 && prevKeyboardState.IsKeyUp(Keys.Space)))

                    {

                        LastKey = 3;

                        ConversationIndexNumber = int.Parse((ConversationIndexNumber.ToString()) + 3);

                        //ConversationIndexNumber = 43;

                        IsReset = true;

                        Reset();

                        MakeChars();



                        clicked = true;

                        timer = 0;

                    }

                    //4



                    if ((keyboardState.IsKeyDown(Keys.D4) && LastKey == 1 && !clicked) || (keyboardState.IsKeyDown(Keys.Space) && !clicked && LastKey == 1 && choiceState == 4 && prevKeyboardState.IsKeyUp(Keys.Space)))

                    {

                        LastKey = 4;

                        ConversationIndexNumber = int.Parse((ConversationIndexNumber.ToString()) + 4);

                        //ConversationIndexNumber = 23;

                        IsReset = true;

                        Reset();

                        MakeChars();



                        clicked = true;

                        timer = 0;

                    }

                    if ((keyboardState.IsKeyDown(Keys.D4) && LastKey == 2 && !clicked) || (keyboardState.IsKeyDown(Keys.Space) && !clicked && LastKey == 2 && choiceState == 4 && prevKeyboardState.IsKeyUp(Keys.Space)))

                    {

                        LastKey = 4;

                        ConversationIndexNumber = int.Parse((ConversationIndexNumber.ToString()) + 4);

                        //ConversationIndexNumber = 33;

                        IsReset = true;

                        Reset();

                        MakeChars();



                        clicked = true;

                        timer = 0;

                    }

                    if ((keyboardState.IsKeyDown(Keys.D4) && LastKey == 3 && !clicked) || (keyboardState.IsKeyDown(Keys.Space) && !clicked && LastKey == 3 && choiceState == 4 && prevKeyboardState.IsKeyUp(Keys.Space)))

                    {

                        LastKey = 4;

                        ConversationIndexNumber = int.Parse((ConversationIndexNumber.ToString()) + 4);

                        //ConversationIndexNumber = 43;

                        IsReset = true;

                        Reset();

                        MakeChars();



                        clicked = true;

                        timer = 0;

                    }

                    //5



                    if ((keyboardState.IsKeyDown(Keys.D5) && LastKey == 1 && !clicked) || (keyboardState.IsKeyDown(Keys.Space) && !clicked && LastKey == 1 && choiceState == 5 && prevKeyboardState.IsKeyUp(Keys.Space)))

                    {

                        LastKey = 5;

                        ConversationIndexNumber = int.Parse((ConversationIndexNumber.ToString()) + 5);

                        //ConversationIndexNumber = 23;

                        IsReset = true;

                        Reset();

                        MakeChars();



                        clicked = true;

                        timer = 0;

                    }

                    if ((keyboardState.IsKeyDown(Keys.D5) && LastKey == 2 && !clicked) || (keyboardState.IsKeyDown(Keys.Space) && !clicked && LastKey == 2 && choiceState == 5 && prevKeyboardState.IsKeyUp(Keys.Space)))

                    {

                        LastKey = 5;

                        ConversationIndexNumber = int.Parse((ConversationIndexNumber.ToString()) + 5);

                        //ConversationIndexNumber = 33;

                        IsReset = true;

                        Reset();

                        MakeChars();



                        clicked = true;

                        timer = 0;

                    }

                    if ((keyboardState.IsKeyDown(Keys.D5) && LastKey == 3 && !clicked) || (keyboardState.IsKeyDown(Keys.Space) && !clicked && LastKey == 3 && choiceState == 5 && prevKeyboardState.IsKeyUp(Keys.Space)))

                    {

                        LastKey = 5;

                        ConversationIndexNumber = int.Parse((ConversationIndexNumber.ToString()) + 5);

                        //ConversationIndexNumber = 43;

                        IsReset = true;

                        Reset();

                        MakeChars();



                        clicked = true;

                        timer = 0;

                    }

                }

                #endregion

                if ((merged.Contains("1.") == false))

                {

                    spriteBatch.DrawString(Font, merged, TextPosition, Color.Red);

                }

                if (merged.Contains("1."))

                {


                    choiceStrings = Regex.Split(merged, @"(?=[\d-])").Where(s => s != String.Empty).ToArray();
        
                    for (int i = 0; i < choiceStrings.Length; i++)

                    {

                        if (choiceState == i + 1)

                        {

                            if (i > 0 && Regex.Matches(choiceStrings[i - 1], "\r\n").Count == 2)

                            {

                                spriteBatch.DrawString(Font, choiceStrings[i], new Vector2(TextPosition.X, TextPosition.Y + (i * 82)), Color.Yellow);

                            }

                            if (i == 0)

                            {

                                spriteBatch.DrawString(Font, choiceStrings[i], new Vector2(TextPosition.X, TextPosition.Y + (i * 41)), Color.Yellow);

                            }

                            if (i > 0 && Regex.Matches(choiceStrings[i - 1], "\r\n").Count == 1)

                            {

                                spriteBatch.DrawString(Font, choiceStrings[i], new Vector2(TextPosition.X, TextPosition.Y + (i * 62)), Color.Yellow);

                            }



                        }

                        else

                        {

                            if (i > 0 && Regex.Matches(choiceStrings[i - 1], "\r\n").Count == 2)

                            {

                                spriteBatch.DrawString(Font, choiceStrings[i], new Vector2(TextPosition.X, TextPosition.Y + (i * 82)), Color.Red);

                            }

                            if (i == 0)

                            {

                                spriteBatch.DrawString(Font, choiceStrings[i], new Vector2(TextPosition.X, TextPosition.Y + (i * 41)), Color.Red);

                            }

                            if (i > 0 && Regex.Matches(choiceStrings[i - 1], "\r\n").Count == 1)

                            {

                                spriteBatch.DrawString(Font, choiceStrings[i], new Vector2(TextPosition.X, TextPosition.Y + (i * 62)), Color.Red);

                            }

                        }



                    }

                }

                if (keyboardState.IsKeyDown(Keys.Space) && prevKeyboardState.IsKeyUp(Keys.Space) && (conversations[conversationCount].Contains("1.") == false) && timer > 300)

                {

                    //CAN'T USE NORMAL RESET... SADLY



                    Count = 0;

                    merged = "";

                    sb.Length = 0;

                    number = 0;

                    tempString = "";

                    choiceStrings = null;

                    conversationCount++;

                    charsRemoved = false;

                    Typing = false;

                    if (conversationCount > conversations.Count() - 1)

                    {

                        conversationCount = conversations.Count() - 1;

                    }

                }
            }
        }
    }
}
