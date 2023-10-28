using System.ComponentModel;

namespace SpaceBattleLib;

 public class VectorModificated
    {
        public double[] coords;

        public VectorModificated(double[] coords)
        {
            this.coords = coords;
        }

        public static VectorModificated operator +(VectorModificated v1, VectorModificated v2)
        {
            if (v1.coords.Length != v2.coords.Length)
                throw new ArgumentException("Vectors have different dimensions");
            if (v1.coords.Contains(double.NaN) || v2.coords.Contains(double.NaN))
                throw new ArgumentException("One of the vectors contains some NaN value");

            var coords1 = v1.coords;
            var coords2 = v2.coords;
            var result = new double[v1.coords.Length];

            for(int i = 0; i < v1.coords.Length; i++)
                result[i] = coords1[i] + coords2[i];

            return new VectorModificated(result);
                             
        }

        public List<double> ToList()
        {
            return coords.ToList();
        }
    }