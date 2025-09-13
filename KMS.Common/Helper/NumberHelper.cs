using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace KMS.Common.Helper
{
    public static class NumberHelper
    {
        public static long ToNumber(this string obj, long defaultvalue = 0)
        {
            try
            {
                return Convert.ToInt64(obj);
            }
            catch
            {
                return defaultvalue;
            }
        }

        public static int ToNumber(this string? obj, int defaultvalue = 0)
        {
            try
            {
                return Convert.ToInt32(obj);
            }
            catch
            {
                return defaultvalue;
            }
        }

        public static byte ToNumber(this string? obj, byte defaultvalue = 0)
        {
            try
            {
                return Convert.ToByte(obj);
            }
            catch
            {
                return defaultvalue;
            }
        }

        public static double ToNumber(this string obj, double defaultvalue = 0)
        {
            try
            {
                obj = obj.Replace(",", "");
                return Convert.ToDouble(obj);
            }
            catch
            {
                return defaultvalue;
            }
        }

        /// <summary>
        /// Convert tiền
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ConvertMoney(this object? obj)
        {
            if (obj == null) return "0";
            double number = Convert.ToDouble(obj);
            if (number == 0) return "0";
            bool negative = number < 0;

            try
            {
                number = Math.Abs(number);
                if (number < 1) return (negative ? "-" : "") + Math.Round(number, 3).ToString("0.###", new CultureInfo("en-US"));
                if (number is < 1000 and >= 1) return (negative ? "-" : "") + Math.Round(number, 3).ToString("##.###", new CultureInfo("en-US"));
                return (negative ? "-" : "") + $"{Math.Round(number, 3).ToString("0,0.###", new CultureInfo("en-US"))}";
            }
            catch
            {
                return "0";
            }
        }

        /// <summary>
        /// Convert 2 dấu phẩy 3 -> 3.00
        /// </summary>
        public static string ConvertMoneyComma2(this object? obj)
        {
            if (obj == null) return "0";
            double number = Convert.ToDouble(obj);
            if (number == 0) return "0";
            bool negative = number < 0;

            try
            {
                number = Math.Abs(number);
                var isInterget = number - Math.Truncate(number) == 0;

                if (isInterget) return Math.Round(number, 3).ToString("N");

                if (number < 1) return (negative ? "-" : "") + Math.Round(number, 3).ToString("0.###", new CultureInfo("en-US"));
                if (number is < 1000 and >= 1) return (negative ? "-" : "") + Math.Round(number, 3).ToString("##.###", new CultureInfo("en-US"));
                return (negative ? "-" : "") + $"{Math.Round(number, 3).ToString("0,0.###", new CultureInfo("en-US"))}";
            }
            catch
            {
                return "0";
            }
        }

        public static string ToEmptyZero(this string obj)
        {
            if (obj.ToString().Trim() == "0") return "";
            return obj;
        }

        /// <summary>
        /// Lấy số ngẫu nhiên
        /// </summary>
        public static int RandomNumber(int start, int end)
        {
            try
            {
                Random rnd = new Random();
                return rnd.Next(start, end);
            }
            catch
            {
                return 0;
            }
        }

        public static string RandomNumber(int length)
        {
            Random random = new Random();
            const string chars = "0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        /// <summary>
        /// Kiểu tra có phải là sô
        /// </summary>
        public static bool CheckNumber(this string value, out int n)
        {
            return int.TryParse(value, out n);
        }

        public static bool CheckNumber(this string value, out double n)
        {
            return double.TryParse(value, out n);
        }

        public static double CheckNumber(this string value)
        {
            try
            {
                double rs = 0;
                double.TryParse(value, out rs);
                return rs;
            }
            catch (Exception e)
            {
                return 0;
            }
        }

        public static bool CheckPhoneNumber(this string value)
        {
            if (string.IsNullOrEmpty(value)) return false;
            if (value.Length < 9) return false;
            Regex rgx = new Regex(@"[^0-9.()#; +]");
            return !rgx.IsMatch(value);
        }

        public static string ConvertStringToMoney(this string value)
        {
            if (string.IsNullOrEmpty(value)) return "";
            try
            {
                string result = "";
                for (int i = 0; i < value.Length; i++)
                {
                    if (value[i] == ',') continue;
                    else result += value[i].ToString();
                }
                return result;
            }
            catch (Exception)
            {
                return "";
            }
        }

        /// <summary>
        /// Tính tổng vat
        /// </summary>
        /// <returns></returns>
        public static double CalTotalVat(this long cost, int vat)
        {
            double vatReal = 0;
            if (vat == 5) vatReal = 100 / 19.0;
            else if (vat == 4) vatReal = 5;
            else if (vat == 1000) vatReal = 0;
            else vatReal = vat;

            return cost * (100 + vatReal) / 100;
        }

        /// <summary>
        /// Tính  vat
        /// </summary>
        /// <returns></returns>
        public static double CalVat(this long cost, int vat)
        {
            double vatReal = 0;
            if (vat == 5) vatReal = 100 / 19.0;
            else if (vat == 4) vatReal = 5;
            else if (vat == 1000) vatReal = 0;
            else vatReal = vat;
            return cost * vatReal / 100;
        }

        /// <summary>
        /// Tính tổng vat
        /// </summary>
        public static double CalTotalVat(this double cost, int vat)
        {
            double vatReal = 0.0;
            if (vat == 5) vatReal = 100 / 19.0;
            else if (vat == 4) vatReal = 5;
            else if (vat == 1000) vatReal = 0;
            else vatReal = vat;

            return cost * (100 + vatReal) / 100;
        }

        /// <summary>
        /// Tính tổng không vat khi tổng là val
        /// </summary>
        public static double CalTotalNotVal(this double costVat, int vat)
        {
            double vatReal = 0;
            if (vat == 5) vatReal = 100 / 19.0;
            else if (vat == 4) vatReal = 5;
            else if (vat == 1000) vatReal = 0;
            else vatReal = vat;
            return costVat * 100 / (100 + vatReal);
        }

        /// <summary>
        /// Tính  vat
        /// </summary>
        public static double CalVat(this double cost, int vat)
        {
            double vatReal = 0;
            if (vat == 5) vatReal = 100 / 19.0;
            else if (vat == 4) vatReal = 5;
            else if (vat == 1000) vatReal = 0;
            else vatReal = vat;
            return cost * vatReal / 100;
        }

        // hàm  làm tròn số thâp phân 5.555 = 6  theo digits
        public static double RoundApproximate(double dbl, int digits, double margin, MidpointRounding mode)
        {
            double fraction = dbl * Math.Pow(10, digits);
            double value = Math.Truncate(fraction);
            fraction = fraction - value;
            if (fraction == 0)
                return dbl;

            double tolerance = margin * dbl;
            // Determine whether this is a midpoint value.
            if ((fraction >= .5 - tolerance) & (fraction <= .5 + tolerance))
            {
                if (mode == MidpointRounding.AwayFromZero)
                    return (value + 1) / Math.Pow(10, digits);
                else
                if (value % 2 != 0)
                    return (value + 1) / Math.Pow(10, digits);
                else
                    return value / Math.Pow(10, digits);
            }
            // Any remaining fractional value greater than .5 is not a midpoint value.
            if (fraction > .5)
                return (value + 1) / Math.Pow(10, digits);
            else
                return value / Math.Pow(10, digits);
        }

        /// <summary>
        /// Đối với Số lượng và đơn giá là VNĐ thì làm tròn tối đa 3 số thập phân
        /// </summary>
        public static double RoundQuantityRateVND(this double input)
        {
            return Math.Round(input, 3, MidpointRounding.AwayFromZero);
        }

        /// <summary>
        /// Đối với thuế GTGT, tiền trước thuế, thành tiền là VNĐ thì làm tròn tối đa 0 số thập phân
        /// </summary>
        public static double RoundCostVND(this double input)
        {
            return Math.Round(input, 0, MidpointRounding.AwayFromZero);
        }

        public static double RoundToTwo(this double input)
        {
            return Math.Round(input, 2, MidpointRounding.AwayFromZero);
        }

        public static double RoundNumber(this double input, int number)
        {
            return Math.Round(input, number, MidpointRounding.AwayFromZero);
        }

        public static int GetValueVat(this string input)
        {
            input = input.Trim('%').Trim();
            if (input.ToUpper() == "K-HĐ" || string.IsNullOrEmpty(input) || input.ToUpper() == "KHD") return 1000;
            var vat = input.ToNumber(0.0);
            return vat switch
            {
                5.26 => 5,
                5 => 4,
                7 => 7,
                8 => 8,
                10 => 10,
                _ => 0
            };
        }

        /// 
        /// Chuyển phần nguyên của số thành chữ
        /// 
        /// Số double cần chuyển thành chữ
        /// Chuỗi kết quả chuyển từ số
        public static string NumberToTextVnd(double inputNumber, bool suffix = true)
        {
            string[] unitNumbers = new string[] { "không", "một", "hai", "ba", "bốn", "năm", "sáu", "bảy", "tám", "chín" };
            string[] placeValues = new string[] { "", "nghìn,", "triệu,", "tỷ" };
            bool isNegative = false;

            // -12345678.3445435 => "-12345678"
            if (inputNumber == 0) return "Không";
            string sNumber = inputNumber.ToString("#");
            double number = Convert.ToDouble(sNumber);
            if (number < 0)
            {
                number = -number;
                sNumber = number.ToString();
                isNegative = true;
            }


            int ones, tens, hundreds;

            int positionDigit = sNumber.Length;   // last -> first

            string result = " ";


            if (positionDigit == 0)
                result = unitNumbers[0].FirstCharToUpper() + result;
            else
            {
                // 0:       ###
                // 1: nghìn ###,###
                // 2: triệu ###,###,###
                // 3: tỷ    ###,###,###,###
                int placeValue = 0;
                while (positionDigit > 0)
                {
                    // Check last 3 digits remain ### (hundreds tens ones)
                    tens = hundreds = -1;
                    ones = Convert.ToInt32(sNumber.Substring(positionDigit - 1, 1));
                    positionDigit--;
                    if (positionDigit > 0)
                    {
                        tens = Convert.ToInt32(sNumber.Substring(positionDigit - 1, 1));
                        positionDigit--;
                        if (positionDigit > 0)
                        {
                            hundreds = Convert.ToInt32(sNumber.Substring(positionDigit - 1, 1));
                            positionDigit--;
                        }
                    }

                    if ((ones > 0) || (tens > 0) || (hundreds > 0) || (placeValue == 3))
                        result = placeValues[placeValue] + result;

                    placeValue++;
                    if (placeValue > 3) placeValue = 1;

                    if ((ones == 1) && (tens > 1))
                        result = "một " + result;
                    else
                    {
                        if ((ones == 5) && (tens > 0))
                            result = "lăm " + result;
                        else if (ones > 0)
                            result = unitNumbers[ones] + " " + result;
                    }
                    if (tens < 0)
                        break;
                    else
                    {
                        if ((tens == 0) && (ones > 0)) result = "lẻ " + result;
                        if (tens == 1) result = "mười " + result;
                        if (tens > 1) result = unitNumbers[tens] + " mươi " + result;
                    }
                    if (hundreds < 0) break;
                    else
                    {
                        if ((hundreds > 0) || (tens > 0) || (ones > 0))
                            result = unitNumbers[hundreds] + " trăm " + result;
                    }
                    result = " " + result;
                }
            }
            result = result.Trim();
            if (isNegative) result = "Âm " + result;
            var list = result.Split(' ');
            list[0] = list[0].FirstCharToUpper();
            var kq = string.Join(" ", list) + (suffix ? " đồng" : "");
            if (kq.Count(x => x.Equals(',')) == 1) kq = kq.Replace(",", "");
            return kq;
        }

        /// 
        /// Chuyển phần nguyên của số thành chữ
        /// 
        /// Số double cần chuyển thành chữ
        /// Chuỗi kết quả chuyển từ số
        public static string NumberToTextUsd(double inputNumber, bool suffix = true)
        {
            if (inputNumber == 0) return "";
            string[] unitNumbers = new string[] { "không", "một", "hai", "ba", "bốn", "năm", "sáu", "bảy", "tám", "chín" };
            string[] placeValues = new string[] { "", "nghìn", "triệu", "tỷ" };
            bool isNegative = false;

            // -12345678.3445435 => "-12345678"
            double number = inputNumber.RoundToTwo();
            string sNumber = "";
            string sDecimal = "";
            var listDecimal = inputNumber.RoundToTwo().ToString().Split('.');
            if (listDecimal.Count() > 1)
            {
                sNumber = inputNumber.RoundToTwo().ToString().Split('.')[0];
                sDecimal = inputNumber.RoundToTwo().ToString().Split('.')[1];
            }
            else sNumber = inputNumber.RoundToTwo().ToString().Split('.')[0];


            var check = int.TryParse(sDecimal, out int nDecimal);

            if (!check || nDecimal == 0) sDecimal = "";
            if (number < 0)
            {
                number = -number;
                sNumber = number.ToString();
                isNegative = true;
            }


            int ones, tens, hundreds;

            int positionDigit = sNumber.Length;   // last -> first
            int positionDecimal = sDecimal.Length;   // last -> first

            string result = " ";
            string resultDecimal = " ";


            if (positionDigit == 0)
                result = unitNumbers[0] + result;
            else
            {
                // 0:       ###
                // 1: nghìn ###,###
                // 2: triệu ###,###,###
                // 3: tỷ    ###,###,###,###
                int placeValue = 0;

                while (positionDigit > 0)
                {
                    // Check last 3 digits remain ### (hundreds tens ones)
                    tens = hundreds = -1;
                    ones = Convert.ToInt32(sNumber.Substring(positionDigit - 1, 1));
                    positionDigit--;
                    if (positionDigit > 0)
                    {
                        tens = Convert.ToInt32(sNumber.Substring(positionDigit - 1, 1));
                        positionDigit--;
                        if (positionDigit > 0)
                        {
                            hundreds = Convert.ToInt32(sNumber.Substring(positionDigit - 1, 1));
                            positionDigit--;
                        }
                    }

                    if ((ones > 0) || (tens > 0) || (hundreds > 0) || (placeValue == 3))
                        result = placeValues[placeValue] + result;

                    placeValue++;
                    if (placeValue > 3) placeValue = 1;

                    if ((ones == 1) && (tens > 1))
                        result = "một " + result;
                    else
                    {
                        if ((ones == 5) && (tens > 0))
                            result = "lăm " + result;
                        else if (ones > 0)
                            result = unitNumbers[ones] + " " + result;
                    }
                    if (tens < 0)
                        break;
                    else
                    {
                        if ((tens == 0) && (ones > 0)) result = "lẻ " + result;
                        if (tens == 1) result = "mười " + result;
                        if (tens > 1) result = unitNumbers[tens] + " mươi " + result;
                    }
                    if (hundreds < 0) break;
                    else
                    {
                        if ((hundreds > 0) || (tens > 0) || (ones > 0))
                            result = unitNumbers[hundreds] + " trăm " + result;
                    }
                    result = " " + result;
                }
            }

            if (string.IsNullOrEmpty(sDecimal))
                resultDecimal = "";
            else
            {
                // 0:       ###
                // 1: nghìn ###,###
                // 2: triệu ###,###,###
                // 3: tỷ    ###,###,###,###
                int placeValue = 0;

                while (positionDecimal > 0)
                {
                    // Check last 3 digits remain ### (hundreds tens ones)
                    tens = hundreds = -1;
                    ones = Convert.ToInt32(sDecimal.Substring(positionDecimal - 1, 1));
                    positionDecimal--;
                    if (positionDecimal > 0)
                    {
                        tens = Convert.ToInt32(sDecimal.Substring(positionDecimal - 1, 1));
                        positionDecimal--;
                        if (positionDecimal > 0)
                        {
                            hundreds = Convert.ToInt32(sDecimal.Substring(positionDecimal - 1, 1));
                            positionDecimal--;
                        }
                    }

                    if ((ones > 0) || (tens > 0) || (hundreds > 0) || (placeValue == 3))
                        resultDecimal = placeValues[placeValue] + resultDecimal;

                    placeValue++;
                    if (placeValue > 3) placeValue = 1;

                    if ((ones == 1) && (tens > 1))
                        resultDecimal = "một " + resultDecimal;
                    else
                    {
                        if ((ones == 5) && (tens > 0))
                            resultDecimal = "lăm " + resultDecimal;
                        else if (ones > 0)
                            resultDecimal = unitNumbers[ones] + " " + resultDecimal;
                    }
                    if (tens < 0)
                        break;
                    else
                    {
                        if ((tens == 0) && (ones > 0)) resultDecimal = "lẻ " + resultDecimal;
                        if (tens == 1) resultDecimal = "mười " + resultDecimal;
                        if (tens > 1) resultDecimal = unitNumbers[tens] + " mươi " + resultDecimal;
                    }
                    if (hundreds < 0) break;
                    else
                    {
                        if ((hundreds > 0) || (tens > 0) || (ones > 0))
                            resultDecimal = unitNumbers[hundreds] + " trăm " + resultDecimal;
                    }
                    resultDecimal = " " + resultDecimal;
                }
            }


            result = result.Trim();
            resultDecimal = resultDecimal.Trim();
            if (isNegative) result = "Âm " + result;
            return result + (suffix ? " đô la mỹ" : "") + (string.IsNullOrEmpty(resultDecimal) ? "" : " và " + resultDecimal + " cent");
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
                words += NumberToWords(number / 100) + " hundred ";
                number %= 100;
            }

            words = SmallNumberToWord(number, words, false);

            return words;
        }

        private static string SmallNumberToWord(int number, string words, bool isPoint)
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
        public static string NumberToWordsUSDEng(double doubleNumber)
        {
            doubleNumber = doubleNumber.RoundToTwo();
            int beforeFloatingPoint = (int)Math.Truncate(doubleNumber);
            var beforeFloatingPointWord = $"{NumberToWords(beforeFloatingPoint)} dollars";
            var afterFloatingPointNumber = (int)Math.Round((doubleNumber - beforeFloatingPoint) * 100, 0, MidpointRounding.AwayFromZero);
            var afterFloatingPointWord = SmallNumberToWord(afterFloatingPointNumber, "", true);
            if (string.IsNullOrEmpty(afterFloatingPointWord))
                return beforeFloatingPointWord;
            return $"{beforeFloatingPointWord} and {afterFloatingPointWord} cents";
        }

        public static string NumberToWordsVndEng(double doubleNumber)
        {
            int beforeFloatingPoint = (int)doubleNumber.RoundNumber(0);
            var beforeFloatingPointWord = $"{NumberToWords(beforeFloatingPoint)}";
            return $"{beforeFloatingPointWord} VND";
        }
    }
}
