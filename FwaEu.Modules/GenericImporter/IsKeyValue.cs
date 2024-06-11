using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Modules.GenericImporter
{
	// NOTE: Les valeurs d'enum ci-dessous doivent voir le 3e bit à 1 pour être considéré comme IsKey
	[Flags]
	public enum IsKeyValue
	{
		False = 0b000,
		True = 0b0010,
		TrueAllowNull = 0b0011
	}
}
