using System;
using System.Linq;
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

        private static char SegmentsToChar(Segments s)
        {
            switch (s)
            {
                case Segments.D0: return '0';
                case Segments.D1: return '1';
                case Segments.D2: return '2';
                case Segments.D3: return '3';
                case Segments.D4: return '4';
                case Segments.D5: return '5';
                case Segments.D6: return '6';
                case Segments.D7: return '7';
                case Segments.D8: return '8';
                case Segments.D9: return '9';
                default: return '?';
            }

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
            bool malformed = false;
            var result = new StringBuilder(9);

            foreach (var position in Enumerable.Range(0, 9))
            {
                var segments = GetSegmentsForPosition(input, position);

                var c = SegmentsToChar(segments);
                malformed |= c == '?';
                result.Append(c);
            }

            if (malformed)
            {
                result.Append(" ILL");
            }
            else if (!IsValid(result.ToString()))
            {
                result.Append(" ERR");
            }

            return result.ToString();
        }

        public static bool IsValid(int accountNumber) => IsValid(accountNumber.ToString("D9"));
        public static bool IsValid(string accountNumber) => Enumerable.Range(1, 9).Select(d => d * (accountNumber[9 - d] - '0')).Sum() % 11 == 0;
    }
}
