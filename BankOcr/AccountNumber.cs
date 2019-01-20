using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace BankOcr
{
    public sealed class AccountNumber
    {
        public static string Parse(string input)
        {
            var accountNum = AccountNumber.FromString(input);

            if (accountNum.IsValid())
            {
                return accountNum.ToString();
            }

            var allValidVariations = accountNum.GetAllValidVariations().ToArray();

            switch (allValidVariations.Length)
            {
                case 0 when accountNum.ToString().Contains('?'):
                    return $"{accountNum} ILL";

                case 0:
                    return $"{accountNum} ERR";

                case 1:
                    return allValidVariations.First().ToString();

                default:
                    var variations = string.Join(", ", allValidVariations.Select(ac => $"'{ac}'"));
                    return $"{accountNum} AMB [{variations}]";
            }
        }


        private const int Length = 9;
        private readonly ImmutableArray<Segments> _digits;

        private AccountNumber(ImmutableArray<Segments> digits) { _digits = digits; }

        private static AccountNumber FromString(string input) =>
            new AccountNumber(
                Enumerable.Range(0, Length).
                Select(position => GetSegmentsForPosition(input, position)).
                ToImmutableArray());

        private static Segments GetSegmentsForPosition(string input, int position)
        {
            var positionOffset = position * 3;
            var middleOffset = 3 * Length;
            var bottomOffset = 2 * middleOffset;

            Segments CheckSegment(Segments flag, int index) =>
                input[index] == ' ' ? Segments.None : flag;

            return
                CheckSegment(Segments.TopBar, positionOffset + 1)
                | CheckSegment(Segments.MiddleLeftPipe, positionOffset + middleOffset)
                | CheckSegment(Segments.MiddleBar, positionOffset + middleOffset + 1)
                | CheckSegment(Segments.MiddleRightPipe, positionOffset + middleOffset + 2)
                | CheckSegment(Segments.BottomLeftPipe, positionOffset + bottomOffset)
                | CheckSegment(Segments.BottomBar, positionOffset + bottomOffset + 1)
                | CheckSegment(Segments.BottomRightPipe, positionOffset + bottomOffset + 2);
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
