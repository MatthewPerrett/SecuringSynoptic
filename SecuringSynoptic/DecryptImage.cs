using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace SecuringSynoptic
{
    public class DecryptImage
    {
        public static string Decrypt(string pass, Bitmap img)
        {
            string encryptedData = SteganographyHelper.ExtractText(img); 
            /*
            string encryptedData = SteganographyHelper.ExtractText(
                new Bitmap(
                    Image.FromFile(path)
                )
            );
            */
            string decryptedData = CipherHelper.Decrypt(encryptedData, pass);
            return decryptedData;
            
         
        }
    }
}
