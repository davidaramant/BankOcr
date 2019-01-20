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
            var positionOffset = position * 3;
            var middleOffset = 3 * Length;
            var bottomOffset = 2 * middleOffset;

            Segments GetFlag(Segments flag, int index) =>
                input[index] == ' ' ? Segments.None : flag;

            return
                GetFlag(Segments.TopBar, positionOffset + 1) |
                GetFlag(Segments.MiddleLeftPipe, positionOffset + middleOffset) |
                GetFlag(Segments.MiddleBar, positionOffset + middleOffset + 1) |
                GetFlag(Segments.MiddleRightPipe, positionOffset + middleOffset + 2) |
                GetFlag(Segments.BottomLeftPipe, positionOffset + bottomOffset) |
                GetFlag(Segments.BottomBar, positionOffset + bottomOffset + 1) |
                GetFlag(Segments.BottomRightPipe, positionOffset + bottomOffset + 2);
        }

        public bool IsValid() =>
            Enumerable.Range(1, Length).
            Aggregate((int?)0, 
                (sum, position) => sum + position * _digits[Length - position].ToNumber()) % 11 == 0;

        private AccountNumber WithDigitAtIndex(Segments digit, int index)
            => new AccountNumber(_digits.SetItem(index, digit));

        public IEnumerable<AccountNumber> GetAllValidVariations() =>
            from index in Enumerable.Range(0, Length).AsParallel()
            from digitVariation in _digits[index].GetAllOneOffs()
            let variation = this.WithDigitAtIndex(digitVariation, index)
            where variation.IsValid()
            orderby variation.ToString() 
            select variation;

        public override string ToString() =>
            new string(_digits.Select(d => (char?)(d.ToNumber() + '0') ?? '?').ToArray());
    }
}
