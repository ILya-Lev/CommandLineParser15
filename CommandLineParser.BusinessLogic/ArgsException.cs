using System;
using System.Collections.Generic;

namespace CommandLineParser.BusinessLogic
{
	public class ArgsException : Exception
	{
		public ErrorCode Code { get; set; }
		public string ErrorParameter { get; set; }
		public char ErrorArgumentId { get; set; }

		public ArgsException(string message) : base(message) { }

		public ArgsException(ErrorCode errorCode)
		{
			Code = errorCode;
		}

		public ArgsException(ErrorCode errorCode, string errorParameter) : this(errorCode)
		{
			ErrorParameter = errorParameter;
		}

		public ArgsException(ErrorCode errorCode, char errorArgumentId, string errorParameter) : this(errorCode, errorParameter)
		{
			ErrorArgumentId = errorArgumentId;
		}

		private static readonly Dictionary<ErrorCode, Func<ArgsException, string>> _codeToMessage = new Dictionary<ErrorCode, Func<ArgsException, string>>
		{
			[ErrorCode.Ok] = exc => "TILT: Should not get here!",
			[ErrorCode.UnexpectedArgument] = exc => $"Argument {exc.ErrorArgumentId} unexpected!",
			[ErrorCode.MissingString] = exc => $"Could not find string parameter for {exc.ErrorArgumentId}.",
			[ErrorCode.InvalidInteger] = exc => $"Argument {exc.ErrorArgumentId} expects an integer but was '{exc.ErrorParameter}'.",
			[ErrorCode.MissingInteger] = exc => $"Could not find integer parameter for {exc.ErrorArgumentId}.",
			[ErrorCode.InvalidDouble] = exc => $"Argument {exc.ErrorArgumentId} expects a double but was '{exc.ErrorParameter}'.",
			[ErrorCode.MissingDouble] = exc => $"Could not find double parameter for {exc.ErrorArgumentId}.",
			[ErrorCode.InvalidArgumentFormat] = exc => $"'{exc.ErrorArgumentId}' is not a valid argument name.",
			[ErrorCode.InvalidArgumentFormat] = exc => $"'{exc.ErrorParameter}' is not a valid argument format."
		};

		public string ErrorMessage() => _codeToMessage[Code](this);
	}

	public enum ErrorCode
	{
		Ok,
		InvalidArgumentFormat,
		UnexpectedArgument,
		InvalidArgumentName,
		MissingString,
		MissingInteger, InvalidInteger,
		MissingDouble, InvalidDouble,
	}
}