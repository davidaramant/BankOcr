using System.Linq;
using System.Text;

namespace BankOcr
{
    public sealed class AccountNumber
    {
        private readonly string _num;

        public AccountNumber(int num)
        {
            _num = num.ToString("D9");
        }

        public AccountNumber(Digits[,] digitOptions)
        {
            var sb = new StringBuilder(9);
            foreach (int position in Enumerable.Range(0, 9))
            {
                var digit = digitOptions[0, position] & digitOptions[1, position] & digitOptions[2, position];

                sb.Append(digit.ToChar());
            }

            _num = sb.ToString();
        }

        public bool IsValid() => Enumerable.Range(1, 9).Select(d => d * (_num[9 - d]-'0')).Sum() % 11 == 0;

        public override string ToString() => _num;
    }
}
