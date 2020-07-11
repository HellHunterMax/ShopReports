using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System;
using System.IO;
using Xunit;

namespace BootCamp.Chapter.Tests
{
    // You don't have to be here for a long time.
    public class ProgramTests : IDisposable
    {
        private const string ValidTransactionsFile = "Input/Transactions";
        private readonly string OutputFile = Guid.NewGuid().ToString();

        [Theory]
        [InlineData("Input/Empty.json")]
        [InlineData("nonExisting.json")]
        public void Main_When_Transactions_File_Empty_Or_Not_Found_Throws_NoTransactionsException(string input)
        {
            Action action = () => Program.Main(new[] { input, "time", OutputFile + ".json" });

            action.Should().Throw<NoTransactionsFoundException>();
        }

        [Fact]
        public void Main_When_Command_Invalid_Throws_InvalidCommandException()
        {
            const string cmd = "blablabla";

            Action action = () => Program.Main(new[] { cmd });

            action.Should().Throw<InvalidCommandException>();
        }

        [Theory]
        [InlineData(".json")]
        [InlineData(".xml")]
        public void Main_When_Valid_Time_Command_Creates_File_And_Writes_Stats_For_Every_Hour(string extension)
        {
            const string cmd = "time";

            Program.Main(new[] { ValidTransactionsFile + extension, cmd, OutputFile + extension });

            var expectedOutput = "Expected/FullDay" + extension;
            AssertMatchingContents(expectedOutput, OutputFile + extension);
        }

        [Theory]
        [InlineData(".json")]
        [InlineData(".xml")]
        public void Main_When_Valid_Time_Command_With_Range_Creates_File_And_Writes_Stats_For_Every_Hour_Belonging_To_Range(string extension)
        {
            const string cmd = "time 20:00-00:00";

            Program.Main(new[] { ValidTransactionsFile + extension, cmd, OutputFile + extension });

            var expectedOutput = "Expected/Night" + extension;
            AssertMatchingContents(expectedOutput, OutputFile + extension);
        }

        [Theory]
        [InlineData(".json")]
        [InlineData(".xml")]
        public void Main_When_Valid_DailyRevenue_Command_Creates_File_And_Writes_Revenue_For_Each_Day_Of_Week(string extension)
        {
            const string cmd = "daily Kwiki Mart";

            Program.Main(new[] { ValidTransactionsFile + extension, cmd, OutputFile + extension });

            var expectedOutput = "Expected/DailyKwiki" + extension;
            AssertMatchingContents(expectedOutput, OutputFile + extension);
        }

        [Theory]
        [InlineData("city -items -max", @"expected\CityItemsMax", ".json")]
        [InlineData("city -items -min", @"expected\CityItemsMin", ".json")]
        [InlineData("city -money -max", @"expected\CityMoneyMax", ".json")]
        [InlineData("city -money -min", @"expected\CityMoneyMin", ".json")]
        [InlineData("city -items -max", @"expected\CityItemsMax", ".xml")]
        [InlineData("city -items -min", @"expected\CityItemsMin", ".xml")]
        [InlineData("city -money -max", @"expected\CityMoneyMax", ".xml")]
        [InlineData("city -money -min", @"expected\CityMoneyMin", ".xml")]
        public void Main_When_Valid_MinMax_Command_With_Returns_Expected_Cities_With_Min_Max(string cmd, string expectedOutput, string extension)
        {
            Program.Main(new[] { ValidTransactionsFile + extension, cmd, OutputFile + extension });

            AssertMatchingContents(expectedOutput, OutputFile + extension);
        }

        [Theory]
        [InlineData(".json")]
        [InlineData(".xml")]
        public void Main_When_Valid_Full_Command_Creates_Files_Based_On_Shop_With_All_Transactions(string extension)
        {
            const string cmd = "full";

            Program.Main(new[] { ValidTransactionsFile + extension, cmd });

            using (new AssertionScope())
            {
                AssertMatchingContents("Expected/Aibe" + extension, "Aibe" + extension);
                AssertMatchingContents("Expected/Kwiki Mart" + extension, "Kwiki Mart" + extension);
                AssertMatchingContents("Expected/Wallmart" + extension, "Wallmart" + extension);
            }
        }

        private void AssertMatchingContents(string expected, string actual)
        {
            File.ReadAllText(actual).Should()
                .Be(File.ReadAllText(expected));
        }

        public void Dispose()
        {
            if (File.Exists(OutputFile))
            {
                File.Delete(OutputFile);
            }
        }
    }
}