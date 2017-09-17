namespace CommandLineParser.BusinessLogic.ArgumentMarshalers
{
	public sealed class StringArrayArgumentMarshaler : ArgumentMarshaler<string[]>
	{
		public override void Set(string[] args, int currentArgumentIndex)
		{
			if (currentArgumentIndex >= args.Length)
				throw new ArgsException(ErrorCode.MissingString);

			_value = args[currentArgumentIndex].Split(',');
		}
	}
}