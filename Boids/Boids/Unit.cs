using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aiv.Fast2D;
using OpenTK;

namespace Boids
{

    class Unit
    {
        private Texture texture;
        private Sprite sprite;
        private float maxSpeed;
        private Vector2 velocity;
        public List<Unit> Neighbours;

        private float separateRange;

        private float steerFactor;

        private float halfConeAngle = MathHelper.DegreesToRadians(120);
        
        public Vector2 Position { get { return sprite.position; } }
        public Vector2 Velocity { get { return velocity; } }
        public float NeighbourRange { get; }

        private static int index;

        public Vector2 Forward
        {
            get
            {
                return new Vector2((float)Math.Cos(sprite.Rotation), (float)Math.Sin(sprite.Rotation));
            }
            set
            {
                sprite.Rotation = (float)Math.Atan2(value.Y, value.X);
            }
        }

        public Unit()
        {
            texture = new Texture("Assets/boid.png");
            sprite = new Sprite(texture.Width, texture.Height);
            sprite.position = new Vector2(App.MouseX, App.MouseY);
            sprite.scale = new Vector2(0.3f, 0.3f);
            sprite.pivot = new Vector2(sprite.Width * 0.5f, sprite.Height * 0.5f);

            maxSpeed = 500.0f;
            velocity = new Vector2(RandomGenerator.GetRandomInt(-180, 180), RandomGenerator.GetRandomInt(-180, 180));

            NeighbourRange = 200f;
            separateRange = 35f;

            Neighbours = new List<Unit>();

            steerFactor = 0.05f;        

            index++;
        }

        public void Update()
        {
            PacManEffect();

            if(Neighbours.Count >= 1)
            {
                Vector2 align = Align();

                Vector2 cohere = Cohere();

                Vector2 separate = Separate();

                Vector2 final = (align + cohere + separate);

                if(final!= Vector2.Zero)
                {
                    velocity += final.Normalized() * steerFactor;
                }
            }

            velocity.Normalize();

            sprite.position += velocity.Normalized() * maxSpeed * App.DeltaTime;

            Forward = velocity.Normalized();
        }

        public void Draw()
        {
            sprite.DrawTexture(texture);
        }

        public void AddNeighbour(Unit u)
        {
            if (!Neighbours.Contains(u))
            {
                Vector2 distVect = u.Position - Position;

                double angleCos = MathHelper.Clamp(Vector2.Dot(Forward, distVect.Normalized()), -1, 1);

                float boidAngle = (float)Math.Acos(angleCos);

                if (boidAngle < halfConeAngle)
                {
                    Neighbours.Add(u);
                }
            }
                     
        }

        public void RemoveNeighbour(Unit u)
        {
            Neighbours.Remove(u);
        }

        public void PacManEffect()
        {
            if (Position.X < 0)
            {
                sprite.position.X = App.Width;
            }
            else if (Position.X > App.Width)
            {
                sprite.position.X = 0;
            }

            if(Position.Y < 0)
            {
                sprite.position.Y = App.Height;
            }
            else if(Position.Y > App.Height)
            {
                sprite.position.Y = 0;
            }
        }

        private Vector2 Align()
        {
            Vector2 align = Vector2.Zero;

            for (int i = 0; i < Neighbours.Count; i++)
            {
                align += (Neighbours[i].Velocity).Normalized();
            }

            align /= Neighbours.Count;

            if(App.Align >= 0.1f)
            {
                return (align).Normalized() * App.Align;
            }

            return Vector2.Zero;
        }

        private Vector2 Cohere()
        {
            Vector2 cohere = Vector2.Zero;

            for (int i = 0; i < Neighbours.Count; i++)
            {
                cohere += (Neighbours[i].Position - Position).Normalized();
            }

            cohere /= Neighbours.Count;

            if (App.Cohere >= 0.1f)
            {
                return (cohere).Normalized() * App.Cohere;
            }

            return Vector2.Zero;
        }

        private Vector2 Separate()
        {
            Vector2 cohere = Vector2.Zero;
            int closerCount = 0;

            for (int i = 0; i < Neighbours.Count; i++)
            {
                if((Position - Neighbours[i].Position).LengthSquared <= separateRange * separateRange)
                {
                    cohere += (Position - Neighbours[i].Position).Normalized();
                    closerCount++;
                }                             
            }

            if(closerCount >= 1 && App.Separate >= 0.1f)
            {
                cohere /= closerCount;
                return (cohere).Normalized() * App.Separate;
            }

            return Vector2.Zero;
        }
    }
}
