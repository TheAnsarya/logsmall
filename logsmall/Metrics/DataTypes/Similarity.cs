using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace logsmall.Metrics.DataTypes
{
    internal class Similarity
    {
        public int Length { get; set; }
        public int SecondLength { get; set; }
        public int SmallestLength { get => Math.Min(Length, SecondLength); }
        public bool IsSameSize { get => Length == SecondLength; }
        public int Total { get; set; }

        public int Same { get; set; }
        public double PercentSame { get => Same * 100.0 / Length; }

        public int Different { get => SecondIsDifferent + SecondIsZero; }
        public double PercentDifferent { get => Different * 100.0 / Length; }

        public int SecondIsZero { get; set; }
        public double PercentZero { get => Different * 100.0 / Length; }

        public int SecondIsDifferent { get; set; }
        public double PercentSecondDifferent { get => SecondIsDifferent * 100.0 / Length; }
    }
}
