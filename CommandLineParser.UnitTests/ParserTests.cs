using CommandLineParser.BusinessLogic;
using FluentAssertions;
using Xunit;

namespace CommandLineParser.UnitTests
{
	public class ParserTests
	{
		[Theory]
		[InlineData("-help", "help")]
		[InlineData("-h", "h")]
		[InlineData("-somethingLongEnoughToBeAwfull", "somethingLongEnoughToBeAwfull")]
		[InlineData("-help ", "help")]
		[InlineData("-he-lp ", "he-lp")]
		[InlineData(@"-he""lp ", @"he""lp")]
		[InlineData("-", "")]
		public void Arguments_OneArgumentWithoutValue_FindOneArgument(string input, string commandName)
		{
			var args = new Parser().Arguments(input);

			args.Should().HaveCount(1);
			args.Should().ContainKey(commandName);
			args.Should().ContainValue(null);
		}

		[Theory]
		[InlineData("-file -folder", "file", "folder")]
		[InlineData("-file    -folder   ", "file", "folder")]
		public void Arguments_TwoArgumentWithoutValue_FindTwoArguments(string input, string firstKey, string secondKey)
		{
			var args = new Parser().Arguments(input);

			args.Should().HaveCount(2);
			args.Should().ContainKey(firstKey);
			args.Should().ContainKey(secondKey);
		}

		[Theory]
		[InlineData("-file story.txt", "file", "story.txt")]
		[InlineData("-f story.txt", "f", "story.txt")]
		[InlineData("-file story.txt  ", "file", "story.txt")]
		[InlineData("-f     story.txt   ", "f", "story.txt")]
		[InlineData("-file st-ory.txt", "file", "st-ory.txt")]
		[InlineData("-file s", "file", "s")]
		[InlineData("- s", "", "s")]                            //todo: not sure this should be a valid case
		public void Arguments_OneArgumentAndValue_FindArgumentAndValue(string input, string key, string value)
		{
			var args = new Parser().Arguments(input);

			args.Should().HaveCount(1);
			args.Should().ContainKey(key);
			args.Should().ContainValue(value);
		}

		[Theory]
		[InlineData("-file story.txt -folder ProgramFiles", "file", "story.txt", "folder", "ProgramFiles")]
		[InlineData("-f story.txt -folder ProgramFiles", "f", "story.txt", "folder", "ProgramFiles")]
		[InlineData("-file story.txt     -folder ProgramFiles", "file", "story.txt", "folder", "ProgramFiles")]
		[InlineData("-file sto-ry.txt -folder Program-Files", "file", "sto-ry.txt", "folder", "Program-Files")]
		[InlineData("-file s -folder ProgramFiles", "file", "s", "folder", "ProgramFiles")]
		[InlineData("-file story.txt -folder p", "file", "story.txt", "folder", "p")]
		[InlineData("-file story.txt -folder  ", "file", "story.txt", "folder", null)]
		public void Arguments_TwoArgumentsAndValue_FindAll(string input, string firstKey, string firstValue, string secondKey, string secondValue)
		{
			var args = new Parser().Arguments(input);

			args.Should().HaveCount(2);
			args.Should().ContainKey(firstKey);
			args.Should().ContainValue(firstValue);
			args.Should().ContainKey(secondKey);
			args.Should().ContainValue(secondValue);
		}

		[Theory]
		[InlineData(@"-f ""C:\User\Vasya Pupkin\story.txt""", "f", @"C:\User\Vasya Pupkin\story.txt")]
		public void Arguments_QuotedValue_FindsValue(string input, string key, string value)
		{
			var args = new Parser().Arguments(input);

			args.Should().HaveCount(1);
			args.Should().ContainKey(key);
			args.Should().ContainValue(value);
		}
	}
}
