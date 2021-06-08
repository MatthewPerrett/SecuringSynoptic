using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace SecuringSynoptic
{
    public class EncryptTextAndEmbed
    {
        public static Bitmap EncryptAndEmbedText(string pass, string data, Bitmap img)
        {
            string encryptedData = EncryptionHelper.Encrypt(data, pass);
            Bitmap imageWithHiddenData = Steganography.embedText(encryptedData, img);
            return imageWithHiddenData;
        }
    }
}
