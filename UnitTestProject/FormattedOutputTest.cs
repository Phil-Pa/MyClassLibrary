using MyClassLibrary;
using Xunit;
using Xunit.Abstractions;

namespace UnitTestProject
{
    public class FormattedOutputTest
    {

        private readonly ITestOutputHelper _outputHelper;

        public FormattedOutputTest(ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
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

            _outputHelper.WriteLine(result);
        }

        [Fact]
        public void TestOutput2()
        {

            var output = new FormattedOutput(8);

            output.AddRow("hallo", "was", "geht", "bei");
            output.AddRow("ich", "mache", "hier", "nur", "etwas");

            var result = output.ToString();

            Assert.Equal("hallo   was     geht    bei     \nich     mache   hier    nur     etwas   \n", result);

            _outputHelper.WriteLine(result);
        }

        [Fact]
        public void TestOutput3()
        {
            var output = new FormattedOutput(8);

            output.AddRow(string.Empty, "was", "geht", "bei", "dir");
            output.AddRow("ich", "mache", string.Empty, "nur", "etwas");

            var result = output.ToString();

            Assert.Equal("        was     geht    bei     dir     \nich     mache           nur     etwas   \n", result);

            _outputHelper.WriteLine(result);
        }

        [Fact]
        public void TestOutputRowOverflowSimple()
        {
            var output = new FormattedOutput(4);

            output.AddRow("fdsd", "hfds", "kr");

            Assert.Equal("fdsd    hfds    kr      \n", output.ToString());
        }

        [Fact]
        public void TestOutputRowOverflowLonger()
        {
            var output = new FormattedOutput(4);

            output.AddRow("fdsdff", "hfdsff", "krff");

            Assert.Equal("fdsdff    hfdsff    krff      \n", output.ToString());
        }

        [Fact]
        public void TestOutputRowOverflowComplex()
        {
            var output = new FormattedOutput(4);

            output.AddRow("fdsdff", "hfdsff", "krff");
            output.AddRow("123456789A", "123456789ABCDEF");

            Assert.Equal("fdsdff             hfdsff             krff               \n123456789A         123456789ABCDEF                       \n", output.ToString());
        }

    }
}
