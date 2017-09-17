namespace CommandLineParser.BusinessLogic.ArgumentMarshalers {
	public interface IArgumentMarshaler
	{
		void Set(string[] args, int currentArgumentIndex);
	}
}