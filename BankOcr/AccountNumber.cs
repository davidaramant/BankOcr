using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BankOcr
{
    public sealed class AccountNumber
    {
        private readonly string _num;

        public AccountNumber(Digits[,] digitOptions)
        {
            var sb = new StringBuilder(9);
            foreach (int position in Enumerable.Range(0, 9))
            {
                var digit = digitOptions[0, position] & digitOptions[1, position] & digitOptions[2, position];

                sb.Append(digit.ToString().Substring(1, 1));
            }

            _num = sb.ToString();
        }

        public override string ToString() => _num;
    }
}
