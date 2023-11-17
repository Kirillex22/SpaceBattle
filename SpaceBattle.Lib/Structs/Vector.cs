using System.ComponentModel;

namespace SpaceBattleLib;

 public class Vector
    {
        public int[] Coords {get; }

        public Vector(int[] coords)
        {
            Coords = coords;
        }

        public static Vector operator +(Vector v1, Vector v2)
        {
            var coords1 = v1.Coords;
            var coords2 = v2.Coords;

            int[] res = coords1.Select((value, index) => value + coords2[index]).ToArray();

            return new Vector(res);
                             
        }
    }
    