using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace KMS.Common.Helper
{
    public static class TextHelper
    {
        /// <summary>
        /// Lấy ra đường dẫn SourceUrl
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string convertSourceUrl(this string url)
        {
            try
            {
                string resutl = "";
                var ar = url.Split("//");
                if (ar.Length > 0)
                {
                    var arurl = ar[1].Split("/");
                    if (arurl.Length > 0)
                    {
                        for (int i = 1; i < arurl.Length; i++)
                        {
                            resutl += "/" + arurl[i];
                        }
                    }
                }
                return resutl.DecodeURL();
            }
            catch (Exception)
            {
                return "/";
            }
        }

        /// <summary>
        /// Kiểm tra thẻ Tag
        /// 1, Thẻ tag là chữ thường
        /// 2, Độ dài tối đa là 45 ký tự
        /// 3, Thẻ Tag phải có từ 2 đến 6 từ
        /// 4, Không có ký tự đặc biệt(trừ -)
        /// </summary>
        /// <param name="input"></param>
        /// <param name="result"></param>
        /// <returns>true: Hợp lệ, false: Không hợp lệ</returns>
        public static bool CheckTag(this string input, out string result)
        {
            string check = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyzÀáÂãÈÉÊÌÍÒóÔÕÙÚÝàáâãèéêìíòóôõùúýĂăĐđĨĩŨũƠơƯưẠạẢảẤấẦầẨẩẪẫẬậẴắẰằẲẳẴẵẶặẸẹẺẻẼẽẾếỀềỂểỄễỆệỈỉỊịỌọỎỏỐốỒồỔổỖỗỘộỚớỜờỞởỠỡỢợỤụỦủỨứỪừỬửỮữỰựỲỳỴỵỶỷỸỹ- ";
            input = input.Trim().ToLower();
            int numberWord = input.Split(' ').Length;
            if (input.Length > 50)
            {
                result = "Thẻ tag có độ dài tối đa là 50 ký tự.";
                return false;
            }
            if (input.Length < 3)
            {
                result = "Thẻ tag có độ dài tối thiểu là 4 ký tự.";
                return false;
            }

            if (CheckAllChar(input, check))
            {
                result = "Thẻ tag có ký tự đặc biệt (trừ '-').";
                return false;
            }
            result = input;
            return true;
        }

        public static bool IsPasswordStrong(string password)

        {
            return Regex.IsMatch(password, @"^(?=.{8,})(?=.*[a-z])(?=.*[A-Z])(?!.*\s).*$");
        }

        /// <summary>
        /// Lấy ra chuỗi ngẫu nhiên
        /// </summary>
        public static string RandomString(int length)
        {
            Random random = new Random();
            const string chars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        /// <summary>
        /// Lấy ra chuỗi ngẫu nhiên
        /// </summary>
        public static string RandomStringNotNumber(int length)
        {
            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        /// <summary>
        /// Lấy ra chuỗi ngẫu nhiên
        /// </summary>
        public static string RandomStringNumber(int length)
        {
            Random random = new Random();
            const string chars = "123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        /// <summary>
        /// Lấy ra chuỗi ngẫu nhiên có cả số và chữ cái hoa và thường, kí tự đặc biệt
        /// </summary>
        public static string RandomStringCharAndNumber(int length)
        {
            if (length <= 4) return "";
            Random random = new Random();
            const string charNumbers = "123456789";
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            const string charUppers = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string charSpecials = @"!@#$%^&*()<>?/\|";
            return new string(Enumerable.Repeat(chars, length - 4)
                .Select(s => s[random.Next(s.Length)]).ToArray())
                + new String(Enumerable.Repeat(charNumbers, 2)
                .Select(s => s[random.Next(s.Length)]).ToArray())
                + new String(Enumerable.Repeat(charSpecials, 1)
                .Select(s => s[random.Next(s.Length)]).ToArray())
                + new String(Enumerable.Repeat(charUppers, 1)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        /// <summary>
        /// Kiểm tra ký tự tồn tại trong chuỗi cho trước
        /// </summary>
        /// <param name="input">Chuỗi cần check</param>
        /// <param name="check">Chuỗi luật</param>
        /// <returns>true: có tồn tại, false: không tồn tại</returns>
        public static bool CheckAllChar(string input, string check)
        {
            return input.Any(t => check.IndexOf(t) < 0);
        }

        public static string RenameFileName(this string input)
        {
            try
            {
                if (string.IsNullOrEmpty(input)) return "";
                var arrName = input.Split('-');
                if (arrName.Length >= 3)
                {
                    return arrName[0] + "-" + arrName[1] + " ..." + arrName[^1];
                }
                if (arrName.Length == 2)
                {
                    return arrName[0] + "-" + arrName[1];
                }
                if (arrName.Length < 2)
                {
                    return arrName[0];
                }
                return input;
            }
            catch (Exception e)
            {
                return input;
            }
        }

        public static string ConvertListToString(this List<string> input, string pre)
        {
            try
            {
                if (input is null) return "";
                return string.Join(pre, input);
            }
            catch (Exception e)
            {
                return "";
            }
        }

        /// <summary>
        /// Xử lý Slug của Url theo Name
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string ToSlugUrl(this string? input)
        {
            try
            {
                if (string.IsNullOrEmpty(input)) return "";
                input = input.ToLower();
                if (input.Length > 250) input = input.Substring(0, 250);
                MatchCollection matches = Regex.Matches(input, "\\w+");
                input = "";
                foreach (var match in matches)
                {
                    input += match + "-";
                }
                input = Regex.Replace(input, "(\\d{2})/(\\d{2})/(\\d{4})|(\\d{2})-(\\d{2})-(\\d{4})", "");
                input = Regex.Replace(input, "á|à|ả|ạ|ã|ă|ắ|ằ|ẳ|ẵ|ặ|â|ấ|ầ|ẩ|ẫ|ậ", "a");
                input = Regex.Replace(input, "é|è|ẻ|ẽ|ẹ|ê|ế|ề|ể|ễ|ệ", "e");
                input = Regex.Replace(input, "i|í|ì|ỉ|ĩ|ị", "i");
                input = Regex.Replace(input, "ó|ò|ỏ|õ|ọ|ô|ố|ồ|ổ|ỗ|ộ|ơ|ớ|ờ|ở|ỡ|ợ", "o");
                input = Regex.Replace(input, "ú|ù|ủ|ũ|ụ|ư|ứ|ừ|ử|ữ|ự", "u");
                input = Regex.Replace(input, "ý|ỳ|ỷ|ỹ|ỵ", "y");
                input = Regex.Replace(input, "đ", "d");
                input = Regex.Replace(input, "–|”|’|“|ω|♀|λ|→|♂|Ω|´|`", "");
                Regex regex = new Regex(@"\p{IsCombiningDiacriticalMarks}+");
                input = regex.Replace(input.Normalize(NormalizationForm.FormD), string.Empty);
                //Xóa các ký tự đặt biệt
                for (int i = 0x20; i < 0x30; i++)
                {
                    input = input.Replace(((char)i).ToString(), " ");
                }
                for (int i = 0x3A; i < 0x40; i++)
                {
                    input = input.Replace(((char)i).ToString(), " ");
                }
                for (int i = 0x5B; i < 0x60; i++)
                {
                    input = input.Replace(((char)i).ToString(), " ");
                }
                for (int i = 0x7B; i < 0x7E; i++)
                {
                    input = input.Replace(((char)i).ToString(), " ");
                }
                input = input.Replace("\n", "-");
                //Đổi khoảng trắng thành ký tự gạch ngang
                input = Regex.Replace(input, " ", "-");
                //Đổi nhiều ký tự gạch ngang liên tiếp thành 1 ký tự gạch ngang
                //Phòng trường hợp người nhập vào quá nhiều ký tự trắng
                while (input.Contains("--"))
                {
                    input = input.Replace("--", "-");
                }
                while (input.Contains(" "))
                {
                    input = input.Replace(" ", "");
                }
                if (input.Length > 96) input = input.Substring(0, 96);

                //Xóa các ký tự gạch ngang ở đầu và cuối
                input = '@' + input + '@';
                input = Regex.Replace(input, "@-|-@|@", "");
                input = input.Replace("?", "");
                return input;
            }
            catch
            {
                return "not-slug-url";
            }
        }

        public static string GetStringStar(double starAvg)
        {
            var str1 = "<i class='fa fa-star'></i>";
            var str2 = "<i class='fa fa-star-half-o'></i>";
            var str3 = "<i class='fa fa-star-o'></i>";
            if (starAvg <= 0.5) { return str3 + str3 + str3 + str3 + str3; }
            if (0.5 < starAvg && starAvg <= 1) { return str1 + str3 + str3 + str3 + str3; }
            if (1 < starAvg && starAvg <= 1.7) { return str1 + str2 + str3 + str3 + str3; }
            if (1.7 < starAvg && starAvg <= 2.2) { return str1 + str1 + str3 + str3 + str3; }
            if (2.2 < starAvg && starAvg <= 2.7) { return str1 + str1 + str2 + str3 + str3; }
            if (2.7 < starAvg && starAvg <= 3.2) { return str1 + str1 + str1 + str3 + str3; }
            if (3.2 < starAvg && starAvg <= 3.7) { return str1 + str1 + str1 + str2 + str3; }
            if (3.7 < starAvg && starAvg <= 4.2) { return str1 + str1 + str1 + str1 + str3; }
            if (4.2 < starAvg && starAvg <= 4.7) { return str1 + str1 + str1 + str1 + str2; }
            if (4.7 < starAvg) { return str1 + str1 + str1 + str1 + str1; }
            return str3 + str3 + str3 + str3 + str3;
        }

        /// <summary>
        /// Xử lý biển số xe
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string ExecuteLicensePlate(this string input)
        {
            try
            {
                input = input.ToLower();
                if (string.IsNullOrEmpty(input)) return "";
                input = Regex.Replace(input, "\\W+", "");
                input = Regex.Replace(input, " ", "");
                return input;
            }
            catch
            {
                return input;
            }
        }

        public static string Utf8Convert(this string input)
        {
            try
            {
                if (string.IsNullOrEmpty(input)) return "";
                Regex regex = new Regex("\\p{IsCombiningDiacriticalMarks}+");
                string temp = input.Normalize(NormalizationForm.FormD);
                input = regex.Replace(temp, string.Empty).Replace('\u0111', 'd').Replace('\u0110', 'D');
                return input;
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// Xóa định dạng Font
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string StripFont(this string input)
        {
            if (string.IsNullOrEmpty(input)) return "";
            input = Regex.Replace(input, $@"font-size:\s*(\d+(\.\d+)*)(pt|px)\s*;*", "");
            input = Regex.Replace(input, "(\\w+\\-)*font\\-family:(\\s*)((\'|(\\&quot\\;))?(\\w+(\\s*)|\\-)+(\'|(\\&quot\\;))?\\s*\\,*\\s*)+(\\;*)", "");
            input = Regex.Replace(input, "(\\w+\\-)*font\\-family:(\\s*)(\"(\\w+(\\s*)|\\-)+\"\\s*\\,*\\s*)+(\\;*)", "");
            input = Regex.Replace(input, "((\\w+\\-)*font\\-family:)", "");
            return input;
        }

        /// <summary>
        /// Lấy ra HTML
        /// </summary>
        public static string ToStringPageSize(this int pageSize)
        {
            StringBuilder htmlText = new StringBuilder();
            if (pageSize == 10)
            {
                htmlText.AppendLine("<option value=\"10\" selected=\"selected\">10</option>");
                htmlText.AppendLine("<option value=\"20\">20</option>");
                htmlText.AppendLine("<option value=\"50\">50</option>");
                htmlText.AppendLine("<option value=\"100\">100</option>");
                htmlText.AppendLine("<option value=\"150\">150</option>");
            }
            else if (pageSize == 20)
            {
                htmlText.AppendLine("<option value=\"10\">10</option>");
                htmlText.AppendLine("<option value=\"20\" selected=\"selected\">20</option>");
                htmlText.AppendLine("<option value=\"50\">50</option>");
                htmlText.AppendLine("<option value=\"100\">100</option>");
                htmlText.AppendLine("<option value=\"150\">150</option>");
            }
            else if (pageSize == 50)
            {
                htmlText.AppendLine("<option value=\"10\">10</option>");
                htmlText.AppendLine("<option value=\"20\">20</option>");
                htmlText.AppendLine("<option value=\"50\" selected=\"selected\">50</option>");
                htmlText.AppendLine("<option value=\"100\">100</option>");
                htmlText.AppendLine("<option value=\"150\">150</option>");
            }
            else if (pageSize == 100)
            {
                htmlText.AppendLine("<option value=\"10\">10</option>");
                htmlText.AppendLine("<option value=\"20\">20</option>");
                htmlText.AppendLine("<option value=\"50\">50</option>");
                htmlText.AppendLine("<option value=\"100\" selected=\"selected\">100</option>");
                htmlText.AppendLine("<option value=\"150\">150</option>");
            }
            else if (pageSize == 150)
            {
                htmlText.AppendLine("<option value=\"10\">10</option>");
                htmlText.AppendLine("<option value=\"20\">20</option>");
                htmlText.AppendLine("<option value=\"50\">50</option>");
                htmlText.AppendLine("<option value=\"100\">100</option>");
                htmlText.AppendLine("<option value=\"150\" selected=\"selected\">150</option>");
            }
            else
            {
                htmlText.AppendLine("<option value=\"10\">10</option>");
                htmlText.AppendLine("<option value=\"20\">20</option>");
                htmlText.AppendLine("<option value=\"50\">50</option>");
                htmlText.AppendLine("<option value=\"100\">100</option>");
                htmlText.AppendLine("<option value=\"" + pageSize + "\" selected=\"selected\">" + pageSize + "</option>");
            }
            return htmlText.ToString();
        }

        /// <summary>
        /// Làm sạch text, xóa nhiều khoảng trắng
        /// </summary>
        public static string ClearText(this string input)
        {
            while (input.Contains("...."))
            {
                input = input.Replace("....", "-");
            }
            while (input.Contains("---"))
            {
                input = input.Replace("---", "=");
            }
            input = Regex.Replace(input, @"(&nbsp;)+", " ");
            input = input.Replace(@"\n", "");
            input = Regex.Replace(input, @"\s+", " ");
            return input.DecodeHtml().Trim();
        }

        /// <summary>
        /// Decode HTML
        /// </summary>
        public static string DecodeHtml(this string? input)
        {
            if (string.IsNullOrEmpty(input)) return "";
            return HttpUtility.HtmlDecode(input ?? "");
        }

        /// <summary>
        /// Thực hiện EncodeBase64
        /// </summary>
        public static string EncodeBase64(this string plainText)
        {
            try
            {
                var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
                return Convert.ToBase64String(plainTextBytes);
            }
            catch
            {
                return "";
            }
        }

        /// <authors>
        /// Thực hiện DecodeBase64
        /// </authors>
        public static string DecodeBase64(this string base64EncodedData)
        {
            try
            {
                var base64EncodedBytes = Convert.FromBase64String(base64EncodedData);
                return Encoding.UTF8.GetString(base64EncodedBytes);
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// Xóa ký tự đặc biệt
        /// </summary>
        public static string RemoveSpecialChar(this string input)
        {
            try
            {
                input = Regex.Replace(input, @"\W+", " ");
                return input;
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// Xóa ký tự đặc biệt
        /// </summary>
        public static string CreateNickNameRandom()
        {
            try
            {
                string[] myName = new string[] { "Cà rốt", "Táo Tàu", "Su hào", "Bắp cải", "Cà chua", "Bí ngô", "Cu Tí", "Cu Tèo", "Nhỏ Xuka", "Thằng Tũn", "Thằng Tít", "Thằng Bờm", "Shin", "Tẹt", "Mén", "Jerry", "Cái Bống" };
                Random rnd = new Random();
                return myName[rnd.Next(1, myName.Length)];
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// Decode chuỗi URL
        /// </summary>
        public static string DecodeURL(this string input)
        {
            if (string.IsNullOrEmpty(input)) return "";
            return HttpUtility.UrlDecode(input);
        }

        /// <summary>
        /// Encode chuỗi URL
        /// </summary>
        public static string EncodeURL(this string input)
        {
            if (string.IsNullOrEmpty(input)) return "";
            return HttpUtility.UrlEncode(input);
        }

        /// <summary>
        /// Thực hiện Md5
        /// </summary>
        public static string GetMd5Hash(MD5 md5Hash, string input)
        {
            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            // Return the hexadecimal string.
            return sBuilder.ToString();
        }

        public static string Md5Hash(this string input)
        {
            using (MD5 md5Hash = MD5.Create())
            {
                // Convert the input string to a byte array and compute the hash.
                byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

                // Create a new Stringbuilder to collect the bytes
                // and create a string.
                StringBuilder sBuilder = new StringBuilder();

                // Loop through each byte of the hashed data
                // and format each one as a hexadecimal string.
                for (int i = 0; i < data.Length; i++)
                {
                    sBuilder.Append(data[i].ToString("x2"));
                }
                // Return the hexadecimal string.
                return sBuilder.ToString();
            }
        }

        public static string ToMd5Hash(this string input)
        {
            using (MD5 md5Hash = MD5.Create())
            {
                byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));
                StringBuilder sBuilder = new StringBuilder();
                for (int i = 0; i < data.Length; i++)
                {
                    sBuilder.Append(data[i].ToString("x2"));
                }
                return sBuilder.ToString();
            }
        }

        /***********Encrypt - Decrypt*****************/

        public static string EncryptString(this string? plainText)
        {
            if (string.IsNullOrEmpty(plainText)) return "";
            try
            {
                var bytesToBeEncrypted = Encoding.UTF8.GetBytes(plainText);
                var passwordBytes = Encoding.UTF8.GetBytes("phudx1996");
                passwordBytes = SHA256.Create().ComputeHash(passwordBytes);
                var bytesEncrypted = Encrypt(bytesToBeEncrypted, passwordBytes);
                return Convert.ToBase64String(bytesEncrypted);
            }
            catch
            {
                return "";
            }
        }

        public static string DecryptString(this string encryptedText)
        {
            try
            {
                if (string.IsNullOrEmpty(encryptedText)) return "";
                var bytesToBeDecrypted = Convert.FromBase64String(encryptedText);
                var passwordBytes = Encoding.UTF8.GetBytes("phudx1996");
                passwordBytes = SHA256.Create().ComputeHash(passwordBytes);
                var bytesDecrypted = Decrypt(bytesToBeDecrypted, passwordBytes);
                return Encoding.UTF8.GetString(bytesDecrypted);
            }
            catch
            {
                return "";
            }
        }

        private static byte[] Encrypt(byte[] bytesToBeEncrypted, byte[] passwordBytes)
        {
            byte[] encryptedBytes;
            var saltBytes = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };

            using (MemoryStream ms = new MemoryStream())
            {
                using (RijndaelManaged AES = new RijndaelManaged())
                {
                    var key = new Rfc2898DeriveBytes(passwordBytes, saltBytes, 1000);

                    AES.KeySize = 256;
                    AES.BlockSize = 128;
                    AES.Key = key.GetBytes(AES.KeySize / 8);
                    AES.IV = key.GetBytes(AES.BlockSize / 8);
                    AES.Mode = CipherMode.CBC;
                    using (var cs = new CryptoStream(ms, AES.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(bytesToBeEncrypted, 0, bytesToBeEncrypted.Length);
                        cs.Close();
                    }

                    encryptedBytes = ms.ToArray();
                }
            }

            return encryptedBytes;
        }

        private static byte[] Decrypt(byte[] bytesToBeDecrypted, byte[] passwordBytes)
        {
            try
            {
                byte[] decryptedBytes;
                var saltBytes = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };
                using (MemoryStream ms = new MemoryStream())
                {
                    using (RijndaelManaged AES = new RijndaelManaged())
                    {
                        var key = new Rfc2898DeriveBytes(passwordBytes, saltBytes, 1000);

                        AES.KeySize = 256;
                        AES.BlockSize = 128;
                        AES.Key = key.GetBytes(AES.KeySize / 8);
                        AES.IV = key.GetBytes(AES.BlockSize / 8);
                        AES.Mode = CipherMode.CBC;

                        using (var cs = new CryptoStream(ms, AES.CreateDecryptor(), CryptoStreamMode.Write))
                        {
                            cs.Write(bytesToBeDecrypted, 0, bytesToBeDecrypted.Length);
                            cs.Close();
                        }

                        decryptedBytes = ms.ToArray();
                    }
                }

                return decryptedBytes;
            }
            catch
            {
                throw;
            }
        }

        //GUID
        public static string StringToGuid(string guid, out Guid value)
        {
            return ObjectToGuid(guid, out value);
        }

        public static string ReadMoneyToText(string str)
        {
            try
            {
                str = str.Replace("-", "");
                string[] word = { "", " Một", " Hai", " Ba", " Bốn", " Năm", " Sáu", " Bẩy", " Tám", " Chín" };
                string[] million = { "", " Mươi", " Trăm", "" };
                string[] billion = { "", "", "", " Nghìn", "", "", " Triệu", "", "" };
                string result = "{0}";
                int count = 0;
                for (int i = str.Length - 1; i >= 0; i--)
                {
                    if (count > 0 && count % 9 == 0)
                        result = string.Format(result, "{0} Tỷ");
                    if (!(count < str.Length - 3 && count > 2 && str[i].Equals('0') && str[i - 1].Equals('0') && str[i - 2].Equals('0')))
                        result = string.Format(result, "{0}" + billion[count % 9]);
                    if (!str[i].Equals('0'))
                        result = string.Format(result, "{0}" + million[count % 3]);
                    else if (count % 3 == 1 && count > 1 && !str[i - 1].Equals('0') && !str[i + 1].Equals('0'))
                        result = string.Format(result, "{0} Lẻ");
                    var num = Convert.ToInt16(str[i].ToString());
                    result = string.Format(result, "{0}" + word[num]);
                    count++;
                }
                result = result.Replace("{0}", "");
                result = result.Replace("Một Mươi", "Mười");
                return result.Trim() + " Đồng";
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// Convert số ra tiền (đô)
        /// </summary>
        public static string NumberToWords(double doubleNumber)
        {
            var beforeFloatingPoint = (int)Math.Floor(doubleNumber);
            var beforeFloatingPointWord = $"{NumberToWords(beforeFloatingPoint)}";
            //check phần thập phân
            var valueAfterPoint = 0;
            var array = Math.Round(doubleNumber, 2, MidpointRounding.AwayFromZero).ToString().Split('.');
            var afterFloatingPointWord = "";
            if (array.Length > 1)
                valueAfterPoint = int.Parse(array[1]);
            if (valueAfterPoint != 0)
                afterFloatingPointWord = $"{NumberToWords(valueAfterPoint)}";
            return valueAfterPoint != 0 ? $"{beforeFloatingPointWord} and {afterFloatingPointWord}" : $"{beforeFloatingPointWord}";
        }

        private static string NumberToWords(int number)
        {
            if (number == 0)
                return "zero";

            if (number < 0)
                return "minus " + NumberToWords(Math.Abs(number));

            var words = "";

            if (number / 1000000000 > 0)
            {
                words += NumberToWords(number / 1000000000) + " billion ";
                number %= 1000000000;
            }
            if (number / 1000000 > 0)
            {
                words += NumberToWords(number / 1000000) + " million ";
                number %= 1000000;
            }
            if (number / 1000 > 0)
            {
                words += NumberToWords(number / 1000) + " thousand ";
                number %= 1000;
            }

            if (number / 100 > 0)
            {
                words += NumberToWords(number / 100) + " hundred";
                number %= 100;
            }
            words = SmallNumberToWord(number, words);
            return words;
        }

        private static string SmallNumberToWord(int number, string words)
        {
            if (number <= 0) return words;
            if (words != "")
                words += " ";
            var unitsMap = new[] { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine", "ten", "eleven", "twelve", "thirteen", "fourteen", "fifteen", "sixteen", "seventeen", "eighteen", "nineteen" };
            var tensMap = new[] { "zero", "ten", "twenty", "thirty", "forty", "fifty", "sixty", "seventy", "eighty", "ninety" };
            if (number < 20)
                words += unitsMap[number];
            else
            {
                words += tensMap[number / 10];
                if ((number % 10) > 0)
                    words += "-" + unitsMap[number % 10];
            }
            return words;
        }

        /// <summary>
        /// Thống kê đầu - đuôi kết quả xổ số
        /// </summary>
        public static string LotteryByFirstChacracter(this string input, string chacracter, string jackpots)
        {
            var result = "";
            bool flagJackpots = false;
            jackpots = jackpots.Substring(jackpots.Length - 2, 2);
            var inputArr = input.Split(',');
            foreach (var item in inputArr)
            {
                if (item.Substring(0, 1) == chacracter)
                    if (item == jackpots && !flagJackpots)
                    {
                        flagJackpots = true;
                        result += " <span style=\"color: #f5402d\">" + item + "</span> ,";
                    }
                    else
                        result += " " + item + ",";
            }

            result = (result + ",").Replace(",,", "").Trim();
            if (result == ",") return "";
            return result;
        }

        /// <summary>
        /// Thống kê đầu - đuôi kết quả xổ số. Nhưng chỉ lấy 1 số
        /// </summary>
        public static string LotteryByFirst1Chacracter(this string input, string chacracter, string jackpots)
        {
            var result = "";
            bool flagJackpots = false;
            jackpots = jackpots.Substring(jackpots.Length - 2, 2);
            var inputArr = input.Split(',');
            foreach (var item in inputArr)
            {
                if (item.Substring(0, 1) == chacracter)
                    if (item == jackpots && !flagJackpots)
                    {
                        flagJackpots = true;
                        result += " <span style=\"color: #f5402d\">" + item.Substring(item.Length - 1, 1) + "</span> ,";
                    }
                    else
                        result += " " + item.Substring(item.Length - 1, 1) + ",";
            }

            result = (result + ",").Replace(",,", "").Trim();
            if (result == ",") return "";
            return result;
        }

        /// <summary>
        /// Thống kê đuôi - đầu kết quả xổ số
        /// </summary>
        public static string LotteryByEndChacracter(this string input, string chacracter, string jackpots)
        {
            jackpots = jackpots.Substring(jackpots.Length - 2, 2);
            var result = "";
            bool flagJackpots = false;
            var inputArr = input.Split(',');
            foreach (var item in inputArr)
            {
                if (item.Substring(item.Length - 1, 1) == chacracter)
                {
                    if (item == jackpots && !flagJackpots)
                    {
                        flagJackpots = true;
                        result += " <span style=\"color: #f5402d\">" + item + "</span> ,";
                    }
                    else
                        result += " " + item + ",";
                }
            }

            result = (result + ",").Replace(",,", "").Trim();
            if (result == ",") return "";
            return result;
        }

        public static string ObjectToGuid(object guid, out Guid value)
        {
            value = Guid.Empty;
            try
            {
                value = new Guid(guid.ToString() ?? string.Empty);
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }

            return "";
        }

        /// <summary>
        ///
        /// </summary>

        public static string HashSha1(string input)
        {
            var hash = new SHA1Managed().ComputeHash(Encoding.UTF8.GetBytes(input));
            return string.Concat(hash.Select(b => b.ToString("x2")));
        }

        public static string FirstCharToUpper(this string input)
        {
            try
            {
                return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(input.ToLower());
            }
            catch
            {
                return input;
            }
        }

        public static string HexDecode(this string hex)
        {
            hex = hex.Replace("\\x22", "\"");
            hex = hex.Replace("\\x7b", "{");
            hex = hex.Replace("\\x5b", "[");
            hex = hex.Replace("\\x7d", "}");
            hex = hex.Replace("\\x5d", "]");
            hex = hex.Replace("\\x3d", "=");
            return hex;
        }

        public static byte[] FromHex(string hex)
        {
            hex = hex.Replace("-", "");
            byte[] raw = new byte[hex.Length / 2];
            for (int i = 0; i < raw.Length; i++)
            {
                raw[i] = Convert.ToByte(hex.Substring(i * 2, 2), 16);
            }
            return raw;
        }

        public static string HextoString(this string inputText)
        {
            byte[] bb = Enumerable.Range(0, inputText.Length)
                .Where(x => x % 2 == 0)
                .Select(x => Convert.ToByte(inputText.Substring(x, 2), 16))
                .ToArray();
            //return Convert.ToBase64String(bb);
            char[] chars = new char[bb.Length / sizeof(char)];
            System.Buffer.BlockCopy(bb, 0, chars, 0, bb.Length);
            return new string(chars);
        }

        public static bool NullOrEmpty(this string text)
        {
            return string.IsNullOrEmpty(text);
        }

        /// <summary>
        /// Lấy từ viết tắt từ chữ cái đầu của 1 chuỗi
        /// I Am Dev -> IAD
        /// </summary>
        public static string GetAbbreviation(this string inputString)
        {
            var result = "";
            if (string.IsNullOrEmpty(inputString) == false)
            {
                var array = inputString.Split(' ');
                foreach (var item in array)
                {
                    result += item.Trim().Substring(0, 1).ToUpper();
                }
            }
            return "";
        }

        public static int GetVatFromString(string? inputString)
        {
            if (string.IsNullOrWhiteSpace(inputString)) return 0;
            inputString = inputString.Replace("%", string.Empty);
            _ = int.TryParse(inputString, out int _vat);
            return _vat;
        }
    }
}
