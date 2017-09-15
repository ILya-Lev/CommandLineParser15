namespace CommandLineParser.BusinessLogic
{
	public abstract class ArgumentMarshaler<TValue> : IArgumentMarshaler
	{
		protected TValue _value;

		public abstract void Set(string[] args, int currentArgumentIndex);
		public static TValue GetValue(IArgumentMarshaler argumentMarshaler)
		{
			var desiredTypeMarshaler = argumentMarshaler as ArgumentMarshaler<TValue>;
			return desiredTypeMarshaler != null ? desiredTypeMarshaler._value : default(TValue);
		}
	}
}