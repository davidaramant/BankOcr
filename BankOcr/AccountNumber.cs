using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace BankOcr
{
    public sealed class AccountNumber
    {
        private const int Length = 9;
        private readonly ImmutableArray<Segments> _digits;

        private AccountNumber(ImmutableArray<Segments> digits)
        {
            _digits = digits;
        }

        public static AccountNumber Parse(string input) =>
            new AccountNumber(
                Enumerable.Range(0, Length).
                Select(position => GetSegmentsForIndex(input, position)).
                ToImmutableArray());

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

        public bool IsValid() =>
            Enumerable.Range(1, Length).
            Aggregate((int?) 0, (sum, position) => sum + position * _digits[Length - position].ToNumber()) % 11 == 0;

        private AccountNumber WithDigitAtIndex(Segments digit, int index)
            => new AccountNumber(_digits.SetItem(index, digit));

        public IEnumerable<AccountNumber> GetAllValidVariations() =>
            from index in Enumerable.Range(0, Length).AsParallel()
            from digitVariation in _digits[index].GetAllOneOffs()
            let variation = this.WithDigitAtIndex(digitVariation, index)
            where variation.IsValid()
            select variation;

        public override string ToString() =>
            new string(_digits.Select(d => (char?)(d.ToNumber() + '0') ?? '?').ToArray());
    }
}
