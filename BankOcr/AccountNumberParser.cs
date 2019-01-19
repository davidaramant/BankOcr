using System.Linq;
using System.Text;

namespace BankOcr
{
    public static class AccountNumberParser
    {
        public static string Parse(string input)
        {
            var originalAccountNum = AccountNumber.Parse(input);

            var result = new StringBuilder();

            if (originalAccountNum.IsValid())
            {
                result.Append(originalAccountNum);
            }
            else
            {
                var allValidVariations = originalAccountNum.GetAllValidVariations().ToArray();

                switch (allValidVariations.Length)
                {
                    case 0:
                        var str = originalAccountNum.ToString();
                        result.Append(str);

                        result.Append(str.Contains('?') ? " ILL" : " ERR");
                        break;
                    
                    case 1:
                        result.Append(allValidVariations.First());
                        break;

                    default:
                        result.Append(originalAccountNum);
                        result.Append(" AMB [");
                        result.Append(string.Join(", ", allValidVariations.OrderBy(ac => ac.ToString()).Select(ac => $"'{ac}'")));
                        result.Append("]");
                        break;
                }
            }

            return result.ToString();
        }
    }
}
