using System.Collections;
using System.Collections.Generic;

namespace BankOcr
{
    public sealed class DigitSegmentLookup : IEnumerable<KeyValuePair<string, Digits>>
    {
        private readonly Dictionary<string, Digits> _lookup = new Dictionary<string, Digits>();

        public Digits this[string input] => _lookup.TryGetValue(input, out var value) ? value : Digits.Unknown;

        public void Add(string pattern, Digits possibleDigits) => _lookup.Add(pattern, possibleDigits);

        public IEnumerator<KeyValuePair<string, Digits>> GetEnumerator() => _lookup.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
