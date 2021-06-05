using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace SecuringSynoptic
{
    public class DecryptTextAndEmbed
    {
        public static string DecryptAndExtractText(string pass, Bitmap img)
        {
            string encryptedData = Steganography.extractText(img); 
            string decryptedData = CipherHelper.Decrypt(encryptedData, pass);
            return decryptedData;
            
         
        }
    }
}
