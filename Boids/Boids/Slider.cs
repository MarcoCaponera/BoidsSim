using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aiv.Fast2D;
using OpenTK;

namespace Boids
{
    class Slider
    {
        private Texture barTexture;
        private Sprite barSprite;

        private Texture innerTexture;
        private Sprite innerSprite;

        private Texture ballTexture;
        private Sprite ballSprite;

        private Vector2 innerOffset;
        private Vector2 ballOffset;

        public Vector2 Position { get { return barSprite.position; } set { barSprite.position = value;
                                                                           innerSprite.position = value + innerOffset; } }

        public Vector2 ballPosition { get { return innerSprite.position; } set { ballSprite.position = value + ballOffset; } }
        public float barWidth { get { return innerSprite.Width * 0.5f; } }

        public float Scale { get; protected set; }

        public Slider()
        {
            barTexture = new Texture("Assets/loadingBar_frame.png");
            innerTexture = new Texture("Assets/loadingBar_bar.png");
            ballTexture = new Texture("Assets/ball.png");

            barSprite = new Sprite(barTexture.Width, barTexture.Height);
            innerSprite = new Sprite(innerTexture.Width, innerTexture.Height);
            ballSprite = new Sprite(ballTexture.Width, ballTexture.Height);
            ballSprite.pivot = new Vector2(ballSprite.Width * 0.5f, ballSprite.Width*0.5f);

            ballOffset = new Vector2(ballSprite.Width * 0.5f, ballSprite.Width * 0.5f - 2);
            innerOffset = new Vector2(4f, 4f);
        }

        public bool Input()
        {
            if ((App.MouseX >= innerSprite.position.X && App.MouseX <= innerSprite.position.X + innerSprite.Width &&
                App.MouseY >= innerSprite.position.Y && App.MouseY <= innerSprite.position.Y + innerSprite.Height))
            {
                ballSprite.position.X = App.MouseX;
                Scale = (ballSprite.position.X - innerSprite.position.X) / innerSprite.Width;
                return true;
            }

            return false;
        }

        public void Update()
        {
            innerSprite.scale = new Vector2(Scale, 1);
        }

        public void Draw()
        {
            barSprite.DrawTexture(barTexture);
            innerSprite.DrawTexture(innerTexture);
            ballSprite.DrawTexture(ballTexture);
        }
    }
}
