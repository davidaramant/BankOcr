using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace BankOcr
{
    public static class AccountNumberParser
    {
        [Flags]
        private enum Segments : byte
        {
            None = 0,
            TopBar = 1 << 0,
            MiddleLeftPipe = 1 << 1,
            MiddleBar = 1 << 2,
            MiddleRightPipe = 1 << 3,
            BottomLeftPipe = 1 << 4,
            BottomBar = 1 << 5,
            BottomRightPipe = 1 << 6,

            D0 = TopBar | MiddleLeftPipe | MiddleRightPipe | BottomLeftPipe | BottomBar | BottomRightPipe,
            D1 = MiddleRightPipe | BottomRightPipe,
            D2 = TopBar | MiddleBar | MiddleRightPipe | BottomLeftPipe | BottomBar,
            D3 = TopBar | MiddleBar | MiddleRightPipe | BottomBar | BottomRightPipe,
            D4 = MiddleLeftPipe | MiddleBar | MiddleRightPipe | BottomRightPipe,
            D5 = TopBar | MiddleLeftPipe | MiddleBar | BottomBar | BottomRightPipe,
            D6 = TopBar | MiddleLeftPipe | MiddleBar | BottomLeftPipe | BottomBar | BottomRightPipe,
            D7 = TopBar | MiddleRightPipe | BottomRightPipe,
            D8 = TopBar | MiddleLeftPipe | MiddleBar | MiddleRightPipe | BottomLeftPipe | BottomBar | BottomRightPipe,
            D9 = TopBar | MiddleLeftPipe | MiddleBar | MiddleRightPipe | BottomBar | BottomRightPipe,
        }

        private static IEnumerable<Segments> AllOneOffs(Segments input)
        {
            var allSegments = new[]
            {
                Segments.TopBar,
                Segments.MiddleLeftPipe,
                Segments.MiddleBar,
                Segments.MiddleRightPipe,
                Segments.BottomLeftPipe,
                Segments.BottomBar,
                Segments.BottomRightPipe
            };

            foreach (var segmentToFlip in allSegments)
            {
                yield return input ^ segmentToFlip;
            }
        }

        private static int? SegmentsToNum(Segments s)
        {
            switch (s)
            {
                case Segments.D0: return 0;
                case Segments.D1: return 1;
                case Segments.D2: return 2;
                case Segments.D3: return 3;
                case Segments.D4: return 4;
                case Segments.D5: return 5;
                case Segments.D6: return 6;
                case Segments.D7: return 7;
                case Segments.D8: return 8;
                case Segments.D9: return 9;
                default: return null;
            }
        }

        private sealed class AccountNumber
        {
            private readonly Segments[] _digits;

            public AccountNumber()
            {
                _digits = new Segments[9];
            }

            public AccountNumber(Segments[] digits)
            {
                _digits = digits;
            }

            public Segments this[int index]
            {
                get => _digits[index];
                set => _digits[index] = value;
            }

            public bool IsValid()
            {
                int? checkSum = 0;

                foreach (var position in Enumerable.Range(1, 9))
                {
                    var index = 9 - position;
                    checkSum += position * SegmentsToNum(_digits[index]);
                }

                return checkSum % 11 == 0;
            }

            public IEnumerable<AccountNumber> GetAllValidVariations()
            {
                foreach (var index in Enumerable.Range(0, 9))
                {
                    foreach (var digitVariation in AllOneOffs(_digits[index]))
                    {
                        var variation = new AccountNumber(_digits.ToArray());
                        variation._digits[index] = digitVariation;
                        if (variation.IsValid())
                        {
                            yield return variation;
                        }
                    }
                }
            }

            public override string ToString() => new string(_digits.Select(d =>
                {
                    var num = SegmentsToNum(d);
                    return num != null ? (char)('0' + num.Value) : '?';
                }).ToArray());
        }

        private static Segments GetSegmentsForPosition(string input, int position)
        {
            Segments s = Segments.None;

            var posOffset = position * 3;
            var middleOffset = 3 * 9;
            var bottomOffset = 2 * middleOffset;

            if (input[posOffset + 1] == '_')
            {
                s |= Segments.TopBar;
            }

            if (input[posOffset + middleOffset] == '|')
            {
                s |= Segments.MiddleLeftPipe;
            }
            if (input[posOffset + middleOffset + 1] == '_')
            {
                s |= Segments.MiddleBar;
            }
            if (input[posOffset + middleOffset + 2] == '|')
            {
                s |= Segments.MiddleRightPipe;
            }

            if (input[posOffset + bottomOffset] == '|')
            {
                s |= Segments.BottomLeftPipe;
            }
            if (input[posOffset + bottomOffset + 1] == '_')
            {
                s |= Segments.BottomBar;
            }
            if (input[posOffset + bottomOffset + 2] == '|')
            {
                s |= Segments.BottomRightPipe;
            }

            return s;
        }

        public static string Parse(string input)
        {
            var originalAccountNum = new AccountNumber();

            foreach (var position in Enumerable.Range(0, 9))
            {
                originalAccountNum[position] = GetSegmentsForPosition(input, position);
            }

            var result = new StringBuilder();

            if (originalAccountNum.IsValid())
            {
                result.Append(originalAccountNum);
            }
            else
            {
                var allValidVariations = originalAccountNum.GetAllValidVariations().ToArray();

                if (allValidVariations.Length == 0)
                {
                    var str = originalAccountNum.ToString();
                    result.Append(str);

                    if (str.Contains('?'))
                    {
                        result.Append(" ILL");
                    }
                    else
                    {
                        result.Append(" ERR");
                    }
                }
                else if (allValidVariations.Length == 1)
                {
                    result.Append(allValidVariations.First());
                }
                else
                {
                    result.Append(originalAccountNum);
                    result.Append(" AMB [");
                    result.Append(string.Join(", ", allValidVariations.OrderBy(ac => ac.ToString()).Select(ac => $"'{ac}'")));
                    result.Append("]");
                }
            }

            return result.ToString();
        }
    }
}
