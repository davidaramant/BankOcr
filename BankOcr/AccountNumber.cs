using System.Collections.Generic;
using System.Linq;

namespace BankOcr
{
    public sealed class AccountNumber
    {
        private const int Length = 9;
        private readonly Segments[] _digits;

        private AccountNumber(Segments[] digits)
        {
            _digits = digits;
        }

        public static AccountNumber Parse(string input)
        {
            var digits = new Segments[Length];
            foreach (var position in Enumerable.Range(0, Length))
            {
                digits[position] = GetSegmentsForIndex(input, position);
            }
            return new AccountNumber(digits);
        }

        public bool IsValid()
        {
            int? checkSum = 0;

            foreach (var position in Enumerable.Range(1, Length))
            {
                var index = Length - position;
                checkSum += position * _digits[index].ToNumber();
            }

            return checkSum % 11 == 0;
        }

        private AccountNumber WithDigitAtIndex(Segments digit, int index)
        {
            var digitsCopy = _digits.ToArray();
            digitsCopy[index] = digit;
            return new AccountNumber(digitsCopy);
        }

        public IEnumerable<AccountNumber> GetAllValidVariations() =>
            from index in Enumerable.Range(0, Length)
            from digitVariation in _digits[index].GetAllOneOffs()
            let variation = this.WithDigitAtIndex(digitVariation, index)
            where variation.IsValid()
            select variation;

        public override string ToString() => new string(_digits.Select(d =>
        {
            var num = d.ToNumber();
            return num != null ? (char)('0' + num.Value) : '?';
        }).ToArray());

        private static Segments GetSegmentsForIndex(string input, int index)
        {
            Segments s = Segments.None;

            var indexOffset = index * 3;
            var middleOffset = 3 * Length;
            var bottomOffset = 2 * middleOffset;

            if (input[indexOffset + 1] == '_')
            {
                s |= Segments.TopBar;
            }

            if (input[indexOffset + middleOffset] == '|')
            {
                s |= Segments.MiddleLeftPipe;
            }
            if (input[indexOffset + middleOffset + 1] == '_')
            {
                s |= Segments.MiddleBar;
            }
            if (input[indexOffset + middleOffset + 2] == '|')
            {
                s |= Segments.MiddleRightPipe;
            }

            if (input[indexOffset + bottomOffset] == '|')
            {
                s |= Segments.BottomLeftPipe;
            }
            if (input[indexOffset + bottomOffset + 1] == '_')
            {
                s |= Segments.BottomBar;
            }
            if (input[indexOffset + bottomOffset + 2] == '|')
            {
                s |= Segments.BottomRightPipe;
            }

            return s;
        }
    }
}
