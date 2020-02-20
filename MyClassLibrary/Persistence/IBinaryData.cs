using System;
using System.Collections.Generic;
using System.Text;

namespace MyClassLibrary.Persistence
{
	public interface IBinaryData<T>
	{

		void Load(IPersistenceReader reader);
		void Save(IPersistenceWriter writer);

	}
}
