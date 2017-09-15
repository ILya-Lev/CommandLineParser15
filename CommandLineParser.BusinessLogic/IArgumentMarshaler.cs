namespace CommandLineParser.BusinessLogic {
	public interface IArgumentMarshaler
	{
		void Set(string[] args, int currentArgumentIndex);
	}
}