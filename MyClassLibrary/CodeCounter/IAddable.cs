namespace MyClassLibrary.CodeCounter
{
	public interface IAddable<T>
	{

		IAddable<T> Add(IAddable<T> other); // TODO: make it so, that we dont have to do a type check in the impl like CodeStats

	}
}