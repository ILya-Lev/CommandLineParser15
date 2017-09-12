using System;
using System.Collections.Generic;
using System.Linq;

namespace CommandLineParser.BusinessLogic
{
	public class Args
	{
		private readonly Dictionary<char, IArgumentMarshaler> _marshalers = new Dictionary<char, IArgumentMarshaler>();
		private readonly HashSet<char> _argsFound = new HashSet<char>();
		private readonly string[] _args;
		private int _currentArgumentIndex;

		public Args(string schema, string[] args)
		{
			_args = args;
			//todo: I'm usually told: logic in ctor is a bad practice ?!?
			ParseSchema(schema);
			ParseArgumentStrings(args.ToList());
		}

		private void ParseSchema(string schema)
		{
			foreach (var element in schema.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
			{
				ParseSchemaElement(element.Trim());
			}
		}

		private void ParseSchemaElement(string element)
		{
			var elementId = element[0];
			ValidateSchemaElementId(elementId);

			var elementTail = element.Substring(1);
			if (elementTail.Length == 0)
				_marshalers.Add(elementId, new BooleanArgumentMarshaler());
			else if (elementTail == "*")
				_marshalers.Add(elementId, new StringArgumentMarshaler());
			else if (elementTail == "#")
				_marshalers.Add(elementId, new IntegerArgumentMarshaler());
			else if (elementTail == "##")
				_marshalers.Add(elementId, new DoubleArgumentMarshaler());
			else if (elementTail == "[*]")
				_marshalers.Add(elementId, new StringArrayArgumentMarshaler());
			else
				throw new ArgsException(InvalidArgumentFormat, elementId, elementTail);
		}

		private void ValidateSchemaElementId(char elementId)
		{
			if (!char.IsLetter(elementId))
				throw new ArgsException(InvalidArgumentName, elementId, null);
		}

		private void ParseArgumentStrings(List<string> argsList)
		{
			for (_currentArgumentIndex = 0; _currentArgumentIndex < argsList.Count; _currentArgumentIndex++)
			{
				if (argsList[_currentArgumentIndex].StartsWith("-"))
					ParseArgumentCharacters(argsList[_currentArgumentIndex].Substring(1));
				else
				{
					//_currentArgumentIndex--;
					break;
				}
			}
		}

		private void ParseArgumentCharacters(string argChars)
		{
			foreach (var argChar in argChars)
			{
				ParseArgumentCharacter(argChar);
			}
		}

		private void ParseArgumentCharacter(char argChar)
		{
			IArgumentMarshaler m;
			if (!_marshalers.TryGetValue(argChar, out m))
				throw new ArgsException(UnexpectedArgument, argChar, null);
			_argsFound.Add(argChar);
			try
			{
				m.Set(_args[_currentArgumentIndex]);
			}
			catch (ArgsException e)
			{
				e.SetErrorArgumentId(argChar);
				throw;
			}
		}

		public bool Has(char arg) => _argsFound.Contains(arg);
		public int NextArgument()
		{
			return _currentArgumentIndex + 1 < _args.Length ? _currentArgumentIndex + 1 : 0;
		}

		public bool GetBoolean(char arg) => BooleanArgumentMarshaler.GetValue(_marshalers[arg]);
	}

	internal interface IArgumentMarshaler
	{
	}
}
