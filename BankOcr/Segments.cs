using System;
using System.Collections.Generic;

namespace BankOcr
{
    [Flags]
    public enum Segments : byte
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

    public static class SegmentsExtensions
    {
        public static IEnumerable<Segments> GetAllOneOffs(this Segments input)
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

        public static int? ToNumber(this Segments s)
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

    }
}
