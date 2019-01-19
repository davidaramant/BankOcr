using System;

namespace BankOcr
{
    [Flags]
    public enum Digits : short
    {
        Unknown = 0,
        D0 = 1 << 0,
        D1 = 1 << 1,
        D2 = 1 << 2,
        D3 = 1 << 3,
        D4 = 1 << 4,
        D5 = 1 << 5,
        D6 = 1 << 6,
        D7 = 1 << 7,
        D8 = 1 << 8,
        D9 = 1 << 9,
    }

    public static class DigitsExtensions
    {
        public static char ToChar(this Digits d)
        {
            switch (d)
            {
                case Digits.D0: return '0';
                case Digits.D1: return '1';
                case Digits.D2: return '2';
                case Digits.D3: return '3';
                case Digits.D4: return '4';
                case Digits.D5: return '5';
                case Digits.D6: return '6';
                case Digits.D7: return '7';
                case Digits.D8: return '8';
                case Digits.D9: return '9';
                default: return '?';
            }
        }
    }
}
