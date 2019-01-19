using BankOcr;
using NUnit.Framework;

namespace Tests
{
    public class ParserTests
    {
        [TestCase(
            " _  _  _  _  _  _  _  _  _ " +
            "| || || || || || || || || |" +
            "|_||_||_||_||_||_||_||_||_|",
            000000000
        )]
        [TestCase(
            "                           " +
            "  |  |  |  |  |  |  |  |  |" +
            "  |  |  |  |  |  |  |  |  |",
            111111111
        )]
        [TestCase(
            " _  _  _  _  _  _  _  _  _ " +
            " _| _| _| _| _| _| _| _| _|" +
            "|_ |_ |_ |_ |_ |_ |_ |_ |_ ",
            222222222
        )]
        [TestCase(
            " _  _  _  _  _  _  _  _  _ " +
            " _| _| _| _| _| _| _| _| _|" +
            " _| _| _| _| _| _| _| _| _|",
            333333333
        )]
        [TestCase(
            "                           " +
            "|_||_||_||_||_||_||_||_||_|" +
            "  |  |  |  |  |  |  |  |  |",
            444444444
        )]
        [TestCase(
            " _  _  _  _  _  _  _  _  _ " +
            "|_ |_ |_ |_ |_ |_ |_ |_ |_ " +
            " _| _| _| _| _| _| _| _| _|",
            555555555
        )]
        [TestCase(
            " _  _  _  _  _  _  _  _  _ " +
            "|_ |_ |_ |_ |_ |_ |_ |_ |_ " +
            "|_||_||_||_||_||_||_||_||_|",
            666666666
        )]
        [TestCase(
            " _  _  _  _  _  _  _  _  _ " +
            "  |  |  |  |  |  |  |  |  |" +
            "  |  |  |  |  |  |  |  |  |",
            777777777
        )]
        [TestCase(
            " _  _  _  _  _  _  _  _  _ " +
            "|_||_||_||_||_||_||_||_||_|" +
            "|_||_||_||_||_||_||_||_||_|",
            888888888
        )]
        [TestCase(
            " _  _  _  _  _  _  _  _  _ " +
            "|_||_||_||_||_||_||_||_||_|" +
            " _| _| _| _| _| _| _| _| _|",
            999999999
        )]
        [TestCase(
            "    _  _     _  _  _  _  _ " +
            "  | _| _||_||_ |_   ||_||_|" +
            "  ||_  _|  | _||_|  ||_| _|",
            123456789
        )]
        public void ShouldParseNormalNumbers(string input, int result)
        {
            Assert.That(Parser.Parse(input).ToString(), Is.EqualTo(result.ToString("D9")));
        }


        //use case 3
        // _  _  _  _  _  _  _  _    
        //| || || || || || || ||_   |
        //|_||_||_||_||_||_||_| _|  |

        //=> 000000051
        //    _  _  _  _  _  _     _ 
        //|_||_|| || ||_   |  |  | _ 
        //  | _||_||_||_|  |  |  | _|

        //=> 49006771? ILL
        //    _  _     _  _  _  _  _ 
        //  | _| _||_| _ |_   ||_||_|
        //  ||_  _|  | _||_|  ||_| _ 

        //=> 1234?678? ILL

        //use case 4

        //  |  |  |  |  |  |  |  |  |
        //  |  |  |  |  |  |  |  |  |

        //=> 711111111
        // _  _  _  _  _  _  _  _  _ 
        //  |  |  |  |  |  |  |  |  |
        //  |  |  |  |  |  |  |  |  |

        //=> 777777177
        // _  _  _  _  _  _  _  _  _ 
        // _|| || || || || || || || |
        //|_ |_||_||_||_||_||_||_||_|

        //=> 200800000
        // _  _  _  _  _  _  _  _  _ 
        // _| _| _| _| _| _| _| _| _|
        // _| _| _| _| _| _| _| _| _|

        //=> 333393333 
        // _  _  _  _  _  _  _  _  _ 
        //|_||_||_||_||_||_||_||_||_|
        //|_||_||_||_||_||_||_||_||_|

        //=> 888888888 AMB ['888886888', '888888880', '888888988']
        // _  _  _  _  _  _  _  _  _ 
        //|_ |_ |_ |_ |_ |_ |_ |_ |_ 
        // _| _| _| _| _| _| _| _| _|

        //=> 555555555 AMB ['555655555', '559555555']
        // _  _  _  _  _  _  _  _  _ 
        //|_ |_ |_ |_ |_ |_ |_ |_ |_ 
        //|_||_||_||_||_||_||_||_||_|

        //=> 666666666 AMB ['666566666', '686666666']
        // _  _  _  _  _  _  _  _  _ 
        //|_||_||_||_||_||_||_||_||_|
        // _| _| _| _| _| _| _| _| _|

        //=> 999999999 AMB ['899999999', '993999999', '999959999']
        //    _  _  _  _  _  _     _ 
        //|_||_|| || ||_   |  |  ||_ 
        //  | _||_||_||_|  |  |  | _|

        //=> 490067715 AMB ['490067115', '490067719', '490867715']
        //    _  _     _  _  _  _  _ 
        // _| _| _||_||_ |_   ||_||_|
        //  ||_  _|  | _||_|  ||_| _|

        //=> 123456789
        // _     _  _  _  _  _  _    
        //| || || || || || || ||_   |
        //|_||_||_||_||_||_||_| _|  |

        //=> 000000051
        //    _  _  _  _  _  _     _ 
        //|_||_|| ||_||_   |  |  | _ 
        //  | _||_||_||_|  |  |  | _|

        //=> 490867715 

    }
}