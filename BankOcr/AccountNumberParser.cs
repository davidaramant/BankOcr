using System.Linq;

namespace BankOcr
{
    public static class AccountNumberParser
    {
        public static string Parse(string input)
        {
            var accountNum = AccountNumber.Parse(input);

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
    }
}
