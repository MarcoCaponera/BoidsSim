using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace Boids
{
    static class UnitMngr
    {
        private static List<Unit> units;

        public static void Init()
        {
            units = new List<Unit>();
        }

        public static void Update()
        {
            for (int i = 0; i < units.Count; i++)
            {
                for (int t = i + 1; t < units.Count; t++)
                {
                    if((units[i].Position - units[t].Position).LengthSquared <= Math.Pow(units[i].NeighbourRange, 2))
                    {
                        units[i].AddNeighbour(units[t]);
                        units[t].AddNeighbour(units[i]);
                    }
                    else
                    {
                        units[i].RemoveNeighbour(units[t]);
                        units[t].RemoveNeighbour(units[i]);
                    }
                }

                units[i].Update();
            }
        }

        public static void Draw()
        {
            for (int i = 0; i < units.Count; i++)
            {
                units[i].Draw();
            }
        }

        public static void CreateUnit()
        {
            Unit u = new Unit();
            units.Add(u);
        }
    }
}
