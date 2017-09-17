namespace CommandLineParser.BusinessLogic.ArgumentMarshalers
{
	public sealed class DoubleArgumentMarshaler : ArgumentMarshaler<double>
	{
		public override void Set(string[] args, int currentArgumentIndex)
		{
			if (currentArgumentIndex >= args.Length)
				throw new ArgsException(ErrorCode.MissingDouble);

			double valueCandidate;
			if (double.TryParse(args[currentArgumentIndex], out valueCandidate))
				_value = valueCandidate;
			else
				throw new ArgsException(ErrorCode.InvalidDouble, args[currentArgumentIndex]);
		}
	}
}