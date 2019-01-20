using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace BankOcr
{
    public sealed class AccountNumber
    {
        public static string Process(string input)
        {
            var accountNum = Parse(input);

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

        const int Length = 9;
        readonly ImmutableArray<Segments> _digits;

        AccountNumber(ImmutableArray<Segments> digits) { _digits = digits; }

        static AccountNumber Parse(string input) =>
            new AccountNumber(
                Enumerable.Range(0, Length).
                Select(position => GetSegmentsForPosition(input, position)).
                ToImmutableArray());

        static Segments GetSegmentsForPosition(string input, int position)
        {
            var positionOffset = position * 3;
            var middleOffset = 3 * Length;
            var bottomOffset = 2 * middleOffset;

            Segments CheckSegment(int index, Segments flag) =>
                input[index] == ' ' ? Segments.None : flag;

            return
                CheckSegment(positionOffset + 1, Segments.TopBar)
                | CheckSegment(positionOffset + middleOffset, Segments.MiddleLeftPipe)
                | CheckSegment(positionOffset + middleOffset + 1, Segments.MiddleBar)
                | CheckSegment(positionOffset + middleOffset + 2, Segments.MiddleRightPipe)
                | CheckSegment(positionOffset + bottomOffset, Segments.BottomLeftPipe)
                | CheckSegment(positionOffset + bottomOffset + 1, Segments.BottomBar)
                | CheckSegment(positionOffset + bottomOffset + 2, Segments.BottomRightPipe);
        }

        bool IsValid() =>
            Enumerable.Range(1, Length).
            Aggregate((int?)0,
                (sum, position) => sum + position * _digits[Length - position].ToNumber()) % 11 == 0;

        AccountNumber WithDigitAtIndex(Segments digit, int index)
            => new AccountNumber(_digits.SetItem(index, digit));

        IEnumerable<AccountNumber> GetAllValidVariations() =>
            from index in Enumerable.Range(0, Length).AsParallel()
            from digitVariation in _digits[index].GetAllOneOffs()
            let accountVariation = this.WithDigitAtIndex(digitVariation, index)
            where accountVariation.IsValid()
            orderby accountVariation.ToString()
            select accountVariation;

        public override string ToString() =>
            new string(_digits.Select(d => d.ToChar()).ToArray());
    }
}
