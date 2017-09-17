namespace CommandLineParser.BusinessLogic.ArgumentMarshalers {
	public sealed class BooleanArgumentMarshaler : ArgumentMarshaler<bool>
	{
		public override void Set(string[] args, int currentArgumentIndex) => _value = true;
	}
}