
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game1
{
    class Dialogue
    {
        public Texture2D DialogueSquareTexture { get; set; }





        public bool Show { get; set; }

        public Vector2 Position { get; set; }



        public Dialogue(Texture2D dialogueSquareTexture, Vector2 position, bool show)

        {

            DialogueSquareTexture = dialogueSquareTexture;



            Position = position;

            Show = show;

        }



        public void print(SpriteBatch spriteBatch)

        {

            if (Show)

            {

                spriteBatch.Draw(DialogueSquareTexture, Position, Color.White);

            }

        }
    }
}
