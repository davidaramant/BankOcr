using BankOcr;
using NUnit.Framework;

namespace Tests
{
    public class ParserTests
    {
        [TestCase(
            " _  _  _  _  _  _  _  _  _ " +
            "| || || || || || || || || |" +
            "|_||_||_||_||_||_||_||_||_|",
            000000000)]
        [TestCase(
            "                           " +
            "  |  |  |  |  |  |  |  |  |" +
            "  |  |  |  |  |  |  |  |  |",
            111111111)]
        [TestCase(
            " _  _  _  _  _  _  _  _  _ " +
            " _| _| _| _| _| _| _| _| _|" +
            "|_ |_ |_ |_ |_ |_ |_ |_ |_ ",
            222222222)]
        [TestCase(
            " _  _  _  _  _  _  _  _  _ " +
            " _| _| _| _| _| _| _| _| _|" +
            " _| _| _| _| _| _| _| _| _|",
            333333333)]
        [TestCase(
            "                           " +
            "|_||_||_||_||_||_||_||_||_|" +
            "  |  |  |  |  |  |  |  |  |",
            444444444)]
        [TestCase(
            " _  _  _  _  _  _  _  _  _ " +
            "|_ |_ |_ |_ |_ |_ |_ |_ |_ " +
            " _| _| _| _| _| _| _| _| _|",
            555555555)]
        [TestCase(
            " _  _  _  _  _  _  _  _  _ " +
            "|_ |_ |_ |_ |_ |_ |_ |_ |_ " +
            "|_||_||_||_||_||_||_||_||_|",
            666666666)]
        [TestCase(
            " _  _  _  _  _  _  _  _  _ " +
            "  |  |  |  |  |  |  |  |  |" +
            "  |  |  |  |  |  |  |  |  |",
            777777777)]
        [TestCase(
            " _  _  _  _  _  _  _  _  _ " +
            "|_||_||_||_||_||_||_||_||_|" +
            "|_||_||_||_||_||_||_||_||_|",
            888888888)]
        [TestCase(
            " _  _  _  _  _  _  _  _  _ " +
            "|_||_||_||_||_||_||_||_||_|" +
            " _| _| _| _| _| _| _| _| _|",
            999999999)]
        [TestCase(
            "    _  _     _  _  _  _  _ " +
            "  | _| _||_||_ |_   ||_||_|" +
            "  ||_  _|  | _||_|  ||_| _|",
            123456789)]
        public void ShouldParseNormalNumbers(string input, int result)
        {
            Assert.That(AccountNumberParser.Parse(input), Is.EqualTo(result.ToString("D9")));
        }

        [TestCase(711111111, true)]
        [TestCase(777777177, true)]
        [TestCase(200800000, true)]
        [TestCase(333393333, true)]
        [TestCase(000000001, false)]
        [TestCase(111111111, false)]
        [TestCase(457508000, true)]
        [TestCase(664371495, false)]
        public void ShouldDetermineIfValid(int accountNumber, bool expectedValidity)
        {
            Assert.That(AccountNumberParser.IsValid(accountNumber), Is.EqualTo(expectedValidity));
        }

        [TestCase(
            " _  _  _  _  _  _  _  _    " +
            "| || || || || || || ||_   |" +
            "|_||_||_||_||_||_||_| _|  |",
            "000000051")]
        [TestCase(
            "    _  _  _  _  _  _     _ " +
            "|_||_|| || ||_   |  |  | _ " +
            "  | _||_||_||_|  |  |  | _|",
            "49006771? ILL")]
        [TestCase(
            "    _  _     _  _  _  _  _ " +
            "  | _| _||_| _ |_   ||_||_|" +
            "  ||_  _|  | _||_|  ||_| _ ",
            "1234?678? ILL")]
        public void ShouldHandleIllformedNumbers(string input, string result)
        {
            Assert.That(AccountNumberParser.Parse(input), Is.EqualTo(result));
        }

        [TestCase(
            "                           " +
            "  |  |  |  |  |  |  |  |  |" +
            "  |  |  |  |  |  |  |  |  |",
            "711111111")]
        [TestCase(
            "_  _  _  _  _  _  _  _  _ " +
            " |  |  |  |  |  |  |  |  |" +
            " |  |  |  |  |  |  |  |  |",
            "777777177")]
        [TestCase(
            " _  _  _  _  _  _  _  _  _ " +
            " _|| || || || || || || || |" +
            "|_ |_||_||_||_||_||_||_||_|",
            "200800000")]
        [TestCase(
            " _  _  _  _  _  _  _  _  _ " +
            " _| _| _| _| _| _| _| _| _|" +
            " _| _| _| _| _| _| _| _| _|",
            "333393333")]
        [TestCase(
            "    _  _     _  _  _  _  _ " +
            " _| _| _||_||_ |_   ||_||_|" +
            "  ||_  _|  | _||_|  ||_| _|",
            "123456789")]
        [TestCase(
            " _     _  _  _  _  _  _    " +
            "| || || || || || || ||_   |" +
            "|_||_||_||_||_||_||_| _|  |",
            "000000051")]
        [TestCase(
            "    _  _  _  _  _  _     _ " +
            "|_||_|| ||_||_   |  |  | _ " +
            "  | _||_||_||_|  |  |  | _|",
            "490867715")]
        public void ShouldFixUnambiguousIllformedNumbers(string input, string result)
        {
            Assert.That(AccountNumberParser.Parse(input), Is.EqualTo(result));
        }

        [TestCase(
            " _  _  _  _  _  _  _  _  _ " +
            "|_||_||_||_||_||_||_||_||_|" +
            "|_||_||_||_||_||_||_||_||_|",
            "888888888 AMB ['888886888', '888888880', '888888988']")]
        [TestCase(
            " _  _  _  _  _  _  _  _  _ " +
            "|_ |_ |_ |_ |_ |_ |_ |_ |_ " +
            " _| _| _| _| _| _| _| _| _|",
            "555555555 AMB ['555655555', '559555555']")]
        [TestCase(
            " _  _  _  _  _  _  _  _  _ " +
            "|_ |_ |_ |_ |_ |_ |_ |_ |_ " +
            "|_||_||_||_||_||_||_||_||_|",
            "666666666 AMB ['666566666', '686666666']")]
        [TestCase(
            " _  _  _  _  _  _  _  _  _ " +
            "|_||_||_||_||_||_||_||_||_|" +
            " _| _| _| _| _| _| _| _| _|",
            "999999999 AMB ['899999999', '993999999', '999959999']")]
        [TestCase(
            "    _  _  _  _  _  _     _ " +
            "|_||_|| || ||_   |  |  ||_ " +
            "  | _||_||_||_|  |  |  | _|",
            "490067715 AMB ['490067115', '490067719', '490867715']")]
        public void ShouldDeterminePossibilitiesForAmbiguousNumbers(string input, string result)
        {
            Assert.That(AccountNumberParser.Parse(input), Is.EqualTo(result));
        }

    }
}