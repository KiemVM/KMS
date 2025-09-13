using System.Globalization;

namespace KMS.Common.Helper
{
    public static class DateTimeHelper
    {
        /// <summary>
        /// Chuyển đổi string sang datetime
        /// </summary>
        public static DateTime ToDateTime(this string strDate, string pre = "yyyy-MM-dd HH:mm:ss")
        {
            try
            {
                return DateTime.ParseExact(strDate, pre, System.Globalization.CultureInfo.InvariantCulture);
            }
            catch
            {
                return DateTime.Now;
            }
        }

        public static DateTime ConvertStringToDateTime(this string strDate, string pre = "dd/MM/yyyy HH:mm:ss")
        {
            try
            {
                return DateTime.ParseExact(strDate, pre, System.Globalization.CultureInfo.GetCultureInfo("vi-VN"));
            }
            catch
            {
                return DateTime.Now;
            }
        }

        public static string Convertdatetostring(this DateTime strDate)
        {
            try
            {
                return strDate.ToString("yyyy-MM-dd HH:mm:ss");
            }
            catch
            {
                return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            }
        }

        public static DateTime ToDateTime(this string strDate)
        {
            try
            {
                var obj = strDate.Split('/').ToArray();
                return DateTime.ParseExact($"{obj[2]}-{obj[1].ToNumber(0):00}-{obj[0].ToNumber(0):00}", "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
            }
            catch
            {
                return DateTime.Now;
            }
        }

        public static string ConvertDateTimeToString(this DateTime dt)
        {
            try
            {
                return $"{dt:dd/MM/yyyy HH:mm}";
            }
            catch
            {
                return "07/07/1996 00:00";
            }
        }

        public static string ConvertTimeDateToString(this DateTime dt, string? pre = "HH:mm dd/MM/yyyy")
        {
            try
            {
                return $"{dt:pre}";
            }
            catch
            {
                return "07/07/1996 00:00";
            }
        }

        public static string ConvertDateToString(this DateTime dt)
        {
            try
            {
                return $"{dt:dd/MM/yyyy}";
            }
            catch
            {
                return "07/07/1996";
            }
        }

        public static string ConvertDateToString(this DateTime? dt, string pre = "dd/MM/yyyy")
        {
            try
            {
                if (dt == null) return "";
                return dt?.ToString(pre);
            }
            catch
            {
                return "";
            }
        }

        public static string ConvertDateToString(this DateTime dt, string pre = "dd/MM/yyyy")
        {
            try
            {
                return dt.ToString(pre);
            }
            catch
            {
                return "";
            }
        }

        public static string ConvertDateToString(this DateTime? dt)
        {
            DateTime dateTimeTimeSpan = dt ?? DateTime.Now;
            return dateTimeTimeSpan.ConvertDateToString();
        }

        public static string ConvertDateNullToString(this DateTime? dt)
        {
            if (dt == null) return "";
            return dt.ConvertDateToString();
        }

        public static string ConvertDateToStringEmpty(this DateTime? dt, string pre = "dd/MM/yyyy")
        {
            try
            {
                if (dt == null) return "";
                return dt?.ToString(pre);
            }
            catch
            {
                return "";
            }
        }

        public static string ConvertDateTimeToString(this DateTime? dt)
        {
            DateTime dateTimeTimeSpan = dt ?? DateTime.Now;
            return dateTimeTimeSpan.ConvertDateTimeToString();
        }

        public static string ConvertTimeDateToString(this DateTime? dt, string? pre = "HH:mm dd/MM/yyyy")
        {
            DateTime dateTimeTimeSpan = dt ?? DateTime.Now;
            return dateTimeTimeSpan.ToString(pre);
        }

        public static double GetTime(this DateTime? dt)
        {
            DateTime dateTimeTimeSpan = dt ?? DateTime.Now;
            dateTimeTimeSpan = new DateTime(dateTimeTimeSpan.Year, dateTimeTimeSpan.Month, dateTimeTimeSpan.Day, dateTimeTimeSpan.Hour, dateTimeTimeSpan.Minute, 0);
            var time = (dateTimeTimeSpan.ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0).AddHours(-7));
            return (double)(time.TotalMilliseconds);
        }

        public static string ConvertTimeToString(this DateTime? dt)
        {
            DateTime dateTimeTimeSpan = dt ?? DateTime.Now;
            return dateTimeTimeSpan.ConvertTimeToString();
        }

        public static string ConvertDayToString(this DateTime? dt)
        {
            DateTime dateTimeTimeSpan = dt ?? DateTime.Now;
            return dateTimeTimeSpan.Day.ToString();
        }

        public static double GetHourWork(DateTime dateTimeStart, DateTime dateTimeEnd)
        {
            //Cùng ngày
            if (dateTimeStart.Date == dateTimeEnd.Date)
            {
                if (dateTimeStart.TimeOfDay <= TimeSpan.Parse("12:00") && dateTimeEnd.TimeOfDay <= TimeSpan.Parse("12:00") || dateTimeStart.TimeOfDay >= TimeSpan.Parse("13:30"))
                    return (dateTimeEnd - dateTimeStart).TotalHours;
                return (dateTimeEnd - dateTimeStart).TotalHours - 1.5;
            }
            //Khác ngày
            var hoursStart = (TimeSpan.Parse("17:30") - dateTimeStart.TimeOfDay).TotalHours;
            var hoursEnd = (dateTimeEnd.TimeOfDay - TimeSpan.Parse("08:00")).TotalHours;
            return (hoursStart <= 4 ? hoursStart : hoursStart - 1.5) + (hoursEnd <= 4 ? hoursEnd : hoursEnd - 1.5) + ((dateTimeEnd.Date - dateTimeStart.Date).TotalDays - 1) * 8;
        }

        public static string ConvertTimeToString(this DateTime dt)
        {
            try
            {
                return $"{dt:HH:mm}";
            }
            catch
            {
                return "00:00";
            }
        }

        /// <summary>
        /// Lấy ra thời gian
        /// </summary>
        public static string GetTimeAgo(this string strDate, string pre = "yyyy-MM-dd HH:mm:ss")
        {
            return GetTimeAgo(strDate.ToDateTime(pre));
        }

        public static int ProgressDeadline(this DateTime? dateStart, DateTime? dateEnd)
        {
            DateTime start = dateStart ?? DateTime.Now;
            DateTime end = dateEnd ?? DateTime.Now;
            double sum = (end - start).TotalMinutes;
            double sum1 = (DateTime.Now - start).TotalMinutes;
            return (int)Math.Floor((DateTime.Now - start).TotalMinutes * 100 / (end - start).TotalMinutes);
        }

        /// <summary>
        /// Lấy ra thời gian
        /// </summary>
        public static string GetTimeAgo(DateTime? dateTime)
        {
            DateTime dateTimeTimeSpan = dateTime ?? DateTime.Now;
            const int SECOND = 1;
            const int MINUTE = 60 * SECOND;
            const int HOUR = 60 * MINUTE;
            const int DAY = 24 * HOUR;
            const int MONTH = 30 * DAY;
            TimeSpan ts = DateTime.Now - dateTimeTimeSpan;
            double delta = Math.Abs(ts.TotalSeconds);
            if (delta < 1 * MINUTE)
                return ts.Seconds == 1 ? "1 giây trước" : ts.Seconds + " giây trước";

            if (delta < 2 * MINUTE)
                return "1 phút trước";

            if (delta < 45 * MINUTE)
                return ts.Minutes + " phút trước";

            if (delta < 90 * MINUTE)
                return "1 giờ trước";

            if (delta < 24 * HOUR)
                return ts.Hours + " giờ trước";

            if (delta < 48 * HOUR)
                return "Hôm qua";

            if (delta < 30 * DAY)
                return ts.Days + " ngày trước";

            if (delta < 12 * MONTH)
            {
                int months = Convert.ToInt32(Math.Floor((double)ts.Days / 30));
                return months <= 1 ? "1 tháng trước" : months + " tháng trước";
            }
            else
            {
                int years = Convert.ToInt32(Math.Floor((double)ts.Days / 365));
                return years <= 1 ? "1 năm trước" : years + " năm trước";
            }
        }

        public static string DaysOfWeek(this DateTime dateTime)
        {
            if (dateTime.DayOfWeek == DayOfWeek.Monday) return "Thứ Hai";
            if (dateTime.DayOfWeek == DayOfWeek.Tuesday) return "Thứ Ba";
            if (dateTime.DayOfWeek == DayOfWeek.Wednesday) return "Thứ Tư";
            if (dateTime.DayOfWeek == DayOfWeek.Thursday) return "Thứ Năm";
            if (dateTime.DayOfWeek == DayOfWeek.Friday) return "Thứ Sáu";
            if (dateTime.DayOfWeek == DayOfWeek.Saturday) return "Thứ Bảy";
            return "Chủ Nhật";
        }

        public static DateTime GetFirstDateOfMonth()
        {
            return new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
        }

        public static DateTime GetEndDateOfMonth()
        {
            return GetFirstDateOfMonth().AddMonths(1).AddSeconds(-1);
        }

        public static DateTime FirstDayOfMonth_AddMethod(this DateTime value)
        {
            return value.Date.AddDays(1 - value.Day);
        }

        public static DateTime FirstDayOfMonth_NewMethod(this DateTime value)
        {
            return new DateTime(value.Year, value.Month, 1);
        }

        public static DateTime LastDayOfMonth_AddMethod(this DateTime value)
        {
            return value.FirstDayOfMonth_AddMethod().AddMonths(1).AddSeconds(-1);
        }

        public static DateTime LastDayOfMonth_AddMethodWithDaysInMonth(this DateTime value)
        {
            return value.Date.AddDays(DateTime.DaysInMonth(value.Year, value.Month) - value.Day);
        }

        public static DateTime LastDayOfMonth_SpecialCase(this DateTime value)
        {
            return value.AddDays(DateTime.DaysInMonth(value.Year, value.Month) - 1);
        }

        public static int DaysInMonth(this DateTime value)
        {
            return DateTime.DaysInMonth(value.Year, value.Month);
        }

        public static DateTime LastDayOfMonth_NewMethod(this DateTime value)
        {
            return new DateTime(value.Year, value.Month, DateTime.DaysInMonth(value.Year, value.Month));
        }

        public static DateTime LastDayOfMonth_NewMethodWithReuseOfExtMethod(this DateTime value)
        {
            return new DateTime(value.Year, value.Month, value.DaysInMonth());
        }

        /// <summary>
        /// Gets the 12:00:00 instance of a DateTime
        /// </summary>
        public static DateTime AbsoluteStart(this DateTime dateTime)
        {
            return dateTime.Date;
        }

        /// <summary>
        /// Gets the 11:59:59 instance of a DateTime
        /// </summary>
        public static DateTime AbsoluteEnd(this DateTime dateTime)
        {
            return AbsoluteStart(dateTime).AddDays(1).AddTicks(-1);
        }

        /// <summary>
        ///  lấy thời thời gian chênh lệch
        /// </summary>
        public static string GetTimeDiff(DateTime dateTime)
        {
            TimeSpan timeSpan;
            if (dateTime < DateTime.Now)
            {
                timeSpan = DateTime.Now - dateTime;
            }
            else
            {
                timeSpan = dateTime - DateTime.Now;
            }
            if (((int)(timeSpan.TotalDays / 365)) == DateTime.Now.Year)
            {
                return "";
            }
            if (timeSpan.TotalDays > 365)
            {
                return ((int)(timeSpan.TotalDays / 365)).ToString() + " năm";
            }
            if (timeSpan.TotalDays > 30)
            {
                return ((int)(timeSpan.TotalDays / 30)).ToString() + " tháng";
            }
            if ((int)timeSpan.TotalDays > 0)
            {
                return ((int)timeSpan.TotalDays).ToString() + " ngày";
            }
            else if ((int)timeSpan.TotalDays == 0 && (int)timeSpan.TotalHours > 0)
            {
                if ((int)timeSpan.TotalMinutes == 0)
                {
                    return ((int)timeSpan.TotalHours).ToString() + " giờ";
                }
                if (((int)((timeSpan.TotalHours - ((int)timeSpan.TotalHours)) * 60)) == 0)
                {
                    return ((int)timeSpan.TotalHours).ToString() + " giờ ";
                }
                return ((int)timeSpan.TotalHours).ToString() + " giờ ";
            }
            else if ((int)timeSpan.TotalDays == 0 && (int)timeSpan.TotalHours == 0)
            {
                return ((int)timeSpan.TotalMinutes).ToString() + " phút";
            }
            return "";
        }

        /// <summary>
        /// Kiểm tra 1 TimeSpan và trả về giá trị
        /// </summary>
        public static bool CheckTimeSpan(this string? input, out TimeSpan result)
        {
            if (string.IsNullOrEmpty(input)) input = "";
            if (!input.Contains(":"))
            {
                result = DateTime.Now.TimeOfDay;
                return false;
            }
            try
            {
                result = TimeSpan.Parse(input);
                return true;
            }
            catch
            {
                result = DateTime.Now.TimeOfDay;
                return false;
            }
        }

        /// <summary>
        /// hỗ trợ chỉ cần điền 3/4/2023 8:00-> dd/MM/yyyy HH:mm C#
        /// </summary>
        public static DateTime? ConvertStringToDateTimeNotFull(string strDay, string strTime)
        {
            try
            {
                var arrStrDay = strDay.Split('/');
                var arrStrTime = strTime.Split(':');
                var strDate = $"{arrStrDay[0].ToNumber(0):00}/{arrStrDay[1].ToNumber(0):00}/{arrStrDay[2].ToNumber(0):0000} {arrStrTime[0].ToNumber(0):00}:{arrStrTime[1].ToNumber(0):00}";
                var isParse = DateTime.TryParseExact(strDate, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dateTimeReal);
                if (isParse) return dateTimeReal;
                return null;
            }
            catch
            {
                return null;
            }

        }

        /// <summary>
        /// hỗ trợ chỉ cần điền 3/4/2023 8:00-> dd/MM/yyyy HH:mm C#
        /// </summary>
        public static DateTime? ConvertStringToDateNotFull(string strDay)
        {
            try
            {
                var arrStrDay = strDay.Split('/');
                var strDate = $"{arrStrDay[0].ToNumber(0):00}/{arrStrDay[1].ToNumber(0):00}/{arrStrDay[2].ToNumber(0):0000}";
                var isParse = DateTime.TryParseExact(strDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dateTimeReal);
                if (isParse) return dateTimeReal;
                return null;
            }
            catch
            {
                return null;
            }
        }
    }
}