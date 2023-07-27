using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;
using logsmall.Metrics.DataTypes;

namespace logsmall.Metrics
{
    internal class Basic {
		public static Similarity GetFileSimilarity(byte[] goal, byte[] current) {
			var sim = new Similarity() {
				Length = goal.Length,
				SecondLength = current.Length,
			};

			// TODO: Check the ISL to see if we need to "optimize" the i< variable
			var smallestLength = sim.SmallestLength;

			for (int i = 0; i < smallestLength; i++) {
				if (goal[i] == current[i]) {
					sim.Same++;
				} else if (current[i] == 0) {
					sim.SecondIsZero++;
				} else {
					sim.SecondIsDifferent++;
				}

				sim.Total++;
			}

			return sim;
		}
	}
}
