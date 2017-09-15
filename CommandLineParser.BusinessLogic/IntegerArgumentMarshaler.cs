namespace CommandLineParser.BusinessLogic
{
	public sealed class IntegerArgumentMarshaler : ArgumentMarshaler<int>
	{
		public override void Set(string[] args, int currentArgumentIndex)
		{
			if (currentArgumentIndex >= args.Length)
				throw new ArgsException(ErrorCode.MissingInteger);

			int valueCandidate;
			if (int.TryParse(args[currentArgumentIndex], out valueCandidate))
				_value = valueCandidate;
			else
				throw new ArgsException(ErrorCode.InvalidInteger, args[currentArgumentIndex]);
		}
	}
}