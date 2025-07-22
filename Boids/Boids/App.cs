using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aiv.Fast2D;
using OpenTK;

namespace Boids
{
    static class App
    {
        private static Window win;
        private static float inputTimer;
        private static float inputTime;
        private static Slider align;
        private static Slider cohere;
        private static Slider separate;

        public static float Align;
        public static float Cohere;
        public static float Separate;

        private static bool sliderPress;

        public static float MouseX { get { return win.MouseX; } }
        public static float MouseY { get { return win.MouseY; } }
        public static float DeltaTime { get { return win.DeltaTime; } }
        public static int Width { get { return win.Width; } }
        public static int Height { get { return win.Height; } }


        public static void Init()
        {
            win = new Window(1600, 900, "Boids");

            inputTime = 0.1f;

            align = new Slider();
            align.Position = new Vector2(Width * 0.80f, Height * 0.1f);
            align.ballPosition = align.Position + new Vector2(align.barWidth, 0);

            cohere = new Slider();
            cohere.Position = new Vector2(Width * 0.80f, Height * 0.15f);
            cohere.ballPosition = cohere.Position + new Vector2(align.barWidth, 0);

            separate = new Slider();
            separate.Position = new Vector2(Width * 0.80f, Height * 0.2f);
            separate.ballPosition = separate.Position + new Vector2(align.barWidth, 0);

            Align = 0.5f;
            Cohere = 0.5f;
            Separate = 0.5f;

            UnitMngr.Init();
        }

        public static void Execute()
        {
            while (win.IsOpened)
            {
                if (win.GetKey(KeyCode.Esc))
                {
                    break;
                }

                inputTimer -= win.DeltaTime;

                if (win.MouseLeft && inputTimer <= 0)
                {
                    if (align.Input() || cohere.Input() || separate.Input())
                    {
                        sliderPress = true;
                    }
                    else if(sliderPress == false)
                    {
                        UnitMngr.CreateUnit();
                        inputTimer = inputTime;                       
                    }                            
                }
                else
                {
                    sliderPress = false;
                }

                Align = align.Scale;
                Cohere = cohere.Scale;
                Separate = separate.Scale;

                align.Update();
                cohere.Update();
                separate.Update();

                UnitMngr.Update();

                UnitMngr.Draw();

                align.Draw();
                cohere.Draw();
                separate.Draw();

                win.Update();
            }
        }
    }
}
