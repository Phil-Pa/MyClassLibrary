using MyClassLibrary;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Xunit.Abstractions;

namespace UnitTestProject
{
    public class FormattedOutputTest
    {

        private readonly ITestOutputHelper outputHelper;

        public FormattedOutputTest(ITestOutputHelper outputHelper)
        {
            this.outputHelper = outputHelper;
        }

        // TODO: refactor

        [Fact]
        public void TestOutput1()
        {

            var output = new FormattedOutput(8);

            output.AddRow("hallo", "was", "geht", "bei", "dir");
            output.AddRow("ich", "mache", "hier", "nur", "etwas");

            var result = output.ToString();

            Assert.Equal("hallo   was     geht    bei     dir     \nich     mache   hier    nur     etwas   \n", result);

            outputHelper.WriteLine(result);
        }

        [Fact]
        public void TestOutput2()
        {

            var output = new FormattedOutput(8);

            output.AddRow("hallo", "was", "geht", "bei", null);
            output.AddRow("ich", "mache", "hier", "nur", "etwas");

            var result = output.ToString();

            Assert.Equal("hallo   was     geht    bei             \nich     mache   hier    nur     etwas   \n", result);

            outputHelper.WriteLine(result);
        }

        [Fact]
        public void TestOutput3()
        {

            var output = new FormattedOutput(8);

            output.AddRow("hallo", "was", "geht", "bei", string.Empty);
            output.AddRow("ich", "mache", "hier", "nur", "etwas");

            var result = output.ToString();

            Assert.Equal("hallo   was     geht    bei             \nich     mache   hier    nur     etwas   \n", result);

            outputHelper.WriteLine(result);
        }

        [Fact]
        public void TestOutput4()
        {
            var output = new FormattedOutput(8);

            output.AddRow(null, "was", "geht", "bei", "dir");
            output.AddRow("ich", "mache", string.Empty, "nur", "etwas");

            var result = output.ToString();

            Assert.Equal("        was     geht    bei     dir     \nich     mache           nur     etwas   \n", result);

            outputHelper.WriteLine(result);
        }

    }
}
