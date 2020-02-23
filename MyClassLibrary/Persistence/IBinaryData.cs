using System;
using System.Collections.Generic;
using System.Text;

namespace MyClassLibrary.Persistence
{
	public interface IBinaryData<T>
	{

		void Load(IPersistenceReader<T> reader);
		void Save(IPersistenceWriter<T> writer);

	}
}
