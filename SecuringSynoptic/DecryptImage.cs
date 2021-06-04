using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace SecuringSynoptic
{
    public class DecryptImage
    {
        public static void Decrypt()
        {
            string _PASSWORD = "password";

            // The path to the image that contains the hidden information
            string pathImageWithHiddenInformation = @"C:\Users\matth\Desktop\nevera_with_hidden.png";

            // Create an instance of the SteganographyHelper
            SteganographyHelper helper = new SteganographyHelper();

            // Retrieve the encrypted data from the image
            string encryptedData = SteganographyHelper.ExtractText(
                new Bitmap(
                    Image.FromFile(pathImageWithHiddenInformation)
                )
            );

            // Decrypt the retrieven data on the image
            string decryptedData = CipherHelper.Decrypt(encryptedData, _PASSWORD);

            // Display the secret text in the console or in a messagebox
            // In our case is "Hello, no one should know that my password is 12345"
            Console.WriteLine(decryptedData);
            //MessageBox.Show(decryptedData);
        }
    }
}
