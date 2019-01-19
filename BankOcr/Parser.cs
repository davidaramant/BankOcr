using System;
using System.Collections.Generic;
using System.Linq;

namespace BankOcr
{
    public static class Parser
    {
        private static readonly Dictionary<string, Digits> TopRowOptions = new Dictionary<string, Digits>
        {
            {"   ", Digits.D1 | Digits.D4},
            {" _ ", Digits.D0 | Digits.D2 | Digits.D3 | Digits.D5 | Digits.D6 | Digits.D7 | Digits.D8 | Digits.D9 },
        };

        private static readonly Dictionary<string, Digits> MiddleRowOptions = new Dictionary<string, Digits>
        {
            {"| |", Digits.D0},
            {"  |", Digits.D1|Digits.D7},
            {" _|", Digits.D2|Digits.D3},
            {"|_|", Digits.D4|Digits.D8|Digits.D9},
            {"|_ ", Digits.D5|Digits.D6},
        };

        private static readonly Dictionary<string, Digits> BottomRowOptions = new Dictionary<string, Digits>
        {
            {"|_|", Digits.D0|Digits.D6|Digits.D8},
            {"  |", Digits.D1|Digits.D4|Digits.D7},
            {"|_ ", Digits.D2},
            {" _|", Digits.D3|Digits.D5|Digits.D9},
        };

        public static AccountNumber Parse(string input)
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

            return new AccountNumber(digitOptions);
        }
    }
}
