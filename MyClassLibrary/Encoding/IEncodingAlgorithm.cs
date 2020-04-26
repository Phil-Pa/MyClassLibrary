namespace MyClassLibrary.Encoding
{
	/// <summary>
	/// Can encode and decode a string
	/// </summary>
	public interface IEncodingAlgorithm
	{
		string Encode(string str);

		string Decode(string str);
	}
}