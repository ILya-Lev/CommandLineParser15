namespace CommandLineParser.BusinessLogic {
	public sealed class BooleanArgumentMarshaler : ArgumentMarshaler<bool>
	{
		public override void Set(string[] args, int currentArgumentIndex) => _value = true;
	}
}