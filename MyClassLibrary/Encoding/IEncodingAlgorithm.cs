namespace MyClassLibrary.Encoding
{
	/// <summary>
	/// can encode and decode a string
	/// </summary>
	public interface IEncodingAlgorithm
	{
		string Encode(string str);
		string Decode(string str);
	}
}
