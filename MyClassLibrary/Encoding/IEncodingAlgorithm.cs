namespace MyClassLibrary.Encoding
{
	public interface IEncodingAlgorithm
	{
		string Encode(string str);
		string Decode(string str);
	}
}
