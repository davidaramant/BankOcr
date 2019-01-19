using System.Linq;
using System.Text;

namespace BankOcr
{
    public static class AccountNumberParser
    {
        private static readonly DigitSegmentLookup TopRowOptions = new DigitSegmentLookup
        {
            {"   ", Digits.D1 | Digits.D4},
            {" _ ", Digits.D0 | Digits.D2 | Digits.D3 | Digits.D5 | Digits.D6 | Digits.D7 | Digits.D8 | Digits.D9 },
        };

        private static readonly DigitSegmentLookup MiddleRowOptions = new DigitSegmentLookup
        {
            {"| |", Digits.D0},
            {"  |", Digits.D1|Digits.D7},
            {" _|", Digits.D2|Digits.D3},
            {"|_|", Digits.D4|Digits.D8|Digits.D9},
            {"|_ ", Digits.D5|Digits.D6},
        };

        private static readonly DigitSegmentLookup BottomRowOptions = new DigitSegmentLookup
        {
            {"|_|", Digits.D0|Digits.D6|Digits.D8},
            {"  |", Digits.D1|Digits.D4|Digits.D7},
            {"|_ ", Digits.D2},
            {" _|", Digits.D3|Digits.D5|Digits.D9},
        };

        public static string Parse(string input)
        {
            var lines = new[]
            {
                input.Substring(0,27),
                input.Substring(27,27),
                input.Substring(27+27,27),
            };

            var digitSectionLookups = new[]
            {
                TopRowOptions,
                MiddleRowOptions,
                BottomRowOptions
            };

            var digitOptions = new Digits[3, 9];

            foreach (int position in Enumerable.Range(0, 9))
            {
                foreach (int line in Enumerable.Range(0, 3))
                {
                    digitOptions[line, position] = digitSectionLookups[line][lines[line].Substring(position * 3, 3)];
                }
            }

            return DetermineAccountNumber(digitOptions);
        }

        private static string DetermineAccountNumber(Digits[,] digitOptions)
        {
            bool illformed = false;

            var sb = new StringBuilder(9);
            foreach (int position in Enumerable.Range(0, 9))
            {
                var digit = digitOptions[0, position] & digitOptions[1, position] & digitOptions[2, position];

                illformed |= digit == Digits.Unknown;
                sb.Append(digit.ToChar());
            }

            return sb.ToString() + (illformed ? " ILL" : string.Empty);
        }


        public static bool IsValid(int accountNumber) => IsValid(accountNumber.ToString("D9"));
        public static bool IsValid(string accountNumber) => Enumerable.Range(1, 9).Select(d => d * (accountNumber[9 - d] - '0')).Sum() % 11 == 0;
    }
}
