using BankOcr;
using NUnit.Framework;

namespace Tests
{
    public class AccountNumberTests
    {
        // NOTE: The expected result is written first purely to make the tests look nicer in the runner
        [TestCase(
            "000000000",
            " _  _  _  _  _  _  _  _  _ " +
            "| || || || || || || || || |" +
            "|_||_||_||_||_||_||_||_||_|")]
        [TestCase(
            "711111111",
            "                           " +
            "  |  |  |  |  |  |  |  |  |" +
            "  |  |  |  |  |  |  |  |  |")]
        [TestCase(
            "222222222 ERR",
            " _  _  _  _  _  _  _  _  _ " +
            " _| _| _| _| _| _| _| _| _|" +
            "|_ |_ |_ |_ |_ |_ |_ |_ |_ ")]
        [TestCase(
            "333393333",
            " _  _  _  _  _  _  _  _  _ " +
            " _| _| _| _| _| _| _| _| _|" +
            " _| _| _| _| _| _| _| _| _|")]
        [TestCase(
            "444444444 ERR",
            "                           " +
            "|_||_||_||_||_||_||_||_||_|" +
            "  |  |  |  |  |  |  |  |  |")]
        [TestCase(
            "555555555 AMB ['555655555', '559555555']",
            " _  _  _  _  _  _  _  _  _ " +
            "|_ |_ |_ |_ |_ |_ |_ |_ |_ " +
            " _| _| _| _| _| _| _| _| _|")]
        [TestCase(
            "666666666 AMB ['666566666', '686666666']",
            " _  _  _  _  _  _  _  _  _ " +
            "|_ |_ |_ |_ |_ |_ |_ |_ |_ " +
            "|_||_||_||_||_||_||_||_||_|")]
        [TestCase(
            "777777177",
            " _  _  _  _  _  _  _  _  _ " +
            "  |  |  |  |  |  |  |  |  |" +
            "  |  |  |  |  |  |  |  |  |")]
        [TestCase(
            "888888888 AMB ['888886888', '888888880', '888888988']",
            " _  _  _  _  _  _  _  _  _ " +
            "|_||_||_||_||_||_||_||_||_|" +
            "|_||_||_||_||_||_||_||_||_|")]
        [TestCase(
            "999999999 AMB ['899999999', '993999999', '999959999']",
            " _  _  _  _  _  _  _  _  _ " +
            "|_||_||_||_||_||_||_||_||_|" +
            " _| _| _| _| _| _| _| _| _|")]
        [TestCase(
            "123456789",
            "    _  _     _  _  _  _  _ " +
            "  | _| _||_||_ |_   ||_||_|" +
            "  ||_  _|  | _||_|  ||_| _|")]
        [TestCase(
            "000000051",
            " _  _  _  _  _  _  _  _    " +
            "| || || || || || || ||_   |" +
            "|_||_||_||_||_||_||_| _|  |")]
        [TestCase(
            "49006771? ILL",
            "    _  _  _  _  _  _     _ " +
            "|_||_|| || ||_   |  |  | _ " +
            "  | _||_||_||_|  |  |  | _|")]
        [TestCase(
            "1234?678? ILL",
            "    _  _     _  _  _  _  _ " +
            "  | _| _||_| _ |_   ||_||_|" +
            "  ||_  _|  | _||_|  ||_| _ ")]
        [TestCase(
            "664371485",
            " _  _     _  _        _  _ " +
            "|_ |_ |_| _|  |  ||_||_||_ " +
            "|_||_|  | _|  |  |  | _| _|")]
        [TestCase(
            "200800000",
            " _  _  _  _  _  _  _  _  _ " +
            " _|| || || || || || || || |" +
            "|_ |_||_||_||_||_||_||_||_|")]
        [TestCase(
            "490867715",
            "    _  _  _  _  _  _     _ " +
            "|_||_|| ||_||_   |  |  | _ " +
            "  | _||_||_||_|  |  |  | _|")]
        [TestCase(
            "490067715 AMB ['490067115', '490067719', '490867715']",
            "    _  _  _  _  _  _     _ " +
            "|_||_|| || ||_   |  |  ||_ " +
            "  | _||_||_||_|  |  |  | _|")]
        public void ShouldProcessAccountNumber(string result, string input) =>
            Assert.That(AccountNumber.Process(input), Is.EqualTo(result));
    }
}