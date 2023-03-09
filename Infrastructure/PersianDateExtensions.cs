using System;
using System.Globalization;

namespace Bpms.WorkflowEngine.Infrastructure
{

    public static class PersianDateExtensions
    {
        public static string ToMonthDay(this DateTime dateTime)
        {
            var array = Persia.Calendar.ConvertToPersian(dateTime).ArrayType;

            return array[1] + "-" + FormatDateTimeNumber(array[2]);
        }

        public static string ToPersianDate(this DateTime dateTime, DateOutPut dateoutput = DateOutPut.Ymd)
        {


            var array = Persia.Calendar.ConvertToPersian(dateTime).ArrayType;

            if (dateoutput == DateOutPut.Y)
                return array[0].ToString();
            else if (dateoutput == DateOutPut.M)
                return array[1].ToString();
            else if (dateoutput == DateOutPut.D)
                return array[2].ToString();
            else if (dateoutput == DateOutPut.Ym)
                return array[0] + "-" + array[1];
            else if (dateoutput == DateOutPut.Md)
                return array[1] + "-" + array[2];
            else
                return array[0] + "-" + FormatDateTimeNumber(array[1]) + "-" + FormatDateTimeNumber(array[2]);
        }

        public static string ToPersianDateTime(this DateTime dateTime)
        {
            var array = Persia.Calendar.ConvertToPersian(dateTime).ArrayType;
            return array[0] + "-" + FormatDateTimeNumber(array[1]) + "-" + FormatDateTimeNumber(array[2]) + " " +
                FormatDateTimeNumber(array[3]) + ":" + FormatDateTimeNumber(array[4]);
        }

        private static string FormatDateTimeNumber(int value)
        {
            return value < 10 ? "0" + value : value.ToString();
        }

        public static string ToPersianDateTime(this DateTime? dateTime)
        {
            if (dateTime == null)
                return "";
            else
            {
                var array = Persia.Calendar.ConvertToPersian(dateTime.Value).ArrayType;
                return array[0] + "-" + FormatDateTimeNumber(array[1]) + "-" + FormatDateTimeNumber(array[2]) + " " +
                    FormatDateTimeNumber(array[3]) + ":" + FormatDateTimeNumber(array[4]);
            }

        }

        public static DateTime ToDateTime(this string persianDate)
        {
            var dateTime = persianDate.Split(' ');
            var dates = dateTime[0].Split('-');
            var h = 0;
            var m = 0;
            var s = 0;
            if (dateTime.Length > 1)
            {
                var times = dateTime[1].Split(':');
                h = int.Parse(times[0]);
                m = int.Parse(times[1]);
                if (times.Length > 2)
                    s = int.Parse(times[2]);
            }
            return new DateTime(int.Parse(dates[0]), int.Parse(dates[1]), int.Parse(dates[2]), h, m, s, new PersianCalendar());
        }
        public static DateTime PickerResultToDateTime(this string persianDate)
        {
            var dateTime = persianDate.Split(' ');
            var dates = dateTime[0].Split('-');
            var h = 0;
            var m = 0;
            var s = 0;
            if (dateTime.Length > 1)
            {
                var times = dateTime[1].Split(':');
                h = int.Parse(times[0]);
                m = int.Parse(times[1]);
                if (times.Length > 2)
                    s = int.Parse(times[2]);
            }
            return new DateTime(int.Parse(dates[2]), int.Parse(dates[1]), int.Parse(dates[0]), h, m, s, new PersianCalendar());
        }


        public static DateTime NowDateTime()
        {
            System.Globalization.CultureInfo.CurrentCulture.ClearCachedData();
            return DateTime.Now;
        }


    }

    public enum DateOutPut
    {
        Y,
        M,
        D,
        Ym,
        Md,
        Ymd
    }
}
