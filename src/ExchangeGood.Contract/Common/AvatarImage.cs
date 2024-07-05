using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeGood.Contract.Common {
    public class AvatarImage {
        public static string GetImage(string feId) {
            int number = int.Parse(feId.Substring(feId.Length - 1));
            string imgUrl;
            switch (number) {
                case 0:
                    imgUrl = "https://cdn-icons-png.flaticon.com/512/1326/1326405.png"; // chicken
                    break;
                case 1:
                    imgUrl = "https://cdn-icons-png.flaticon.com/512/4322/4322991.png"; // cat
                    break;
                case 2:
                    imgUrl = "https://cdn-icons-png.flaticon.com/512/3940/3940417.png"; // rabbit
                    break;
                case 3:
                    imgUrl = "https://cdn-icons-png.flaticon.com/512/9308/9308861.png"; // cat2
                    break;
                case 4:
                    imgUrl = "https://cdn-icons-png.flaticon.com/512/1326/1326377.png"; // panda
                    break;
                case 5:
                    imgUrl = "https://cdn-icons-png.flaticon.com/512/3940/3940403.png"; // bear
                    break;
                case 6: 
                    imgUrl = "https://cdn-icons-png.flaticon.com/512/10738/10738692.png"; // huu
                    break;
                case 7:
                    imgUrl = "https://cdn-icons-png.flaticon.com/512/9306/9306924.png"; // chim 
                    break;
                case 8:
                    imgUrl = "https://cdn-icons-png.flaticon.com/512/10564/10564814.png"; // buom
                    break;
                default:
                    imgUrl = "https://cdn-icons-png.flaticon.com/512/9377/9377899.png"; // ca
                    break;
            }
            return imgUrl;  
        }
    }
}
