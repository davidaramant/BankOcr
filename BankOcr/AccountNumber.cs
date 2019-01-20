using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace BankOcr
{
    public sealed class AccountNumber
    {
        private const int Length = 9;
        private readonly ImmutableArray<Segments> _digits;

        private AccountNumber(ImmutableArray<Segments> digits) { _digits = digits; }

        public static AccountNumber Parse(string input) =>
            new AccountNumber(
                Enumerable.Range(0, Length).
                Select(position => GetSegmentsForPosition(input, position)).
                ToImmutableArray());

        private static Segments GetSegmentsForPosition(string input, int position)
        {
            var s = Segments.None;

            var positionOffset = position * 3;
            var middleOffset = 3 * Length;
            var bottomOffset = 2 * middleOffset;

            void SetFlag(Segments flag, int index)
            {
                if (input[index] != ' ')
                {
                    s |= flag;
                }
            }

            SetFlag(Segments.TopBar, positionOffset + 1);

            SetFlag(Segments.MiddleLeftPipe, positionOffset + middleOffset);
            SetFlag(Segments.MiddleBar, positionOffset + middleOffset + 1);
            SetFlag(Segments.MiddleRightPipe, positionOffset + middleOffset + 2);

            SetFlag(Segments.BottomLeftPipe, positionOffset + bottomOffset);
            SetFlag(Segments.BottomBar, positionOffset + bottomOffset + 1);
            SetFlag(Segments.BottomRightPipe, positionOffset + bottomOffset + 2);

            return s;
        }

        public bool IsValid() =>
            Enumerable.Range(1, Length).
            Aggregate((int?)0, (sum, position) => sum + position * _digits[Length - position].ToNumber()) % 11 == 0;

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
