using System;

namespace BankOcr
{
    [Flags]
    public enum Digits : short
    {
        Nothing = 0,
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
}
