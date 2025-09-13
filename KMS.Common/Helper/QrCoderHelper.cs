using QRCoder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMS.Common.Helper
{
    public static class QrCoderHelper
    {
        public static string CreateByteQrCode(string? content, int pixels, string pathImg)
        {
            try
            {
                QRCodeGenerator qrGenerator = new QRCodeGenerator();
                QRCodeData qrCodeData = qrGenerator.CreateQrCode(content, QRCodeGenerator.ECCLevel.H);
                BitmapByteQRCode qrCode = new BitmapByteQRCode(qrCodeData);
                byte[] qrCodeAsBitmapByteArr = qrCode.GetGraphic(pixels);
                using var fileStream = new FileStream(pathImg, FileMode.Create);
                fileStream.Write(qrCodeAsBitmapByteArr, 0, qrCodeAsBitmapByteArr.Length);
                return pathImg;
            }
            catch (Exception)
            {
                return "";
            }
        }
    }
}