using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace logsmall {
	interface ILine {
		string Address { get; set; }
		string Op { get; set; }
		string Parameters { get; set; }
		string Bytecode { get; }

		ILine ToLine();

		bool IsBranch { get; }
		bool IsCall { get; }
		bool IsJump { get; }
	}
}
