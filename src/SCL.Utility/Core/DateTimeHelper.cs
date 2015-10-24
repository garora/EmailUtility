using System;

namespace Utility.Core
{
    public class DateTimeHelper
    {
        public static DateTime GetStartOfQuarter(int Year, Quarter Qtr)
        {
            if (Qtr == Quarter.First)
                return new DateTime(Year, 1, 1, 0, 0, 0, 0);
            if (Qtr == Quarter.Second)
                return new DateTime(Year, 4, 1, 0, 0, 0, 0);
            if (Qtr == Quarter.Third)
                return new DateTime(Year, 7, 1, 0, 0, 0, 0);
            return new DateTime(Year, 10, 1, 0, 0, 0, 0);
        }

        public static DateTime GetEndOfQuarter(int Year, Quarter Qtr)
        {
            if (Qtr == Quarter.First)
                return new DateTime(Year, 3, DateTime.DaysInMonth(Year, 3), 23, 59, 59, 999);
            if (Qtr == Quarter.Second)
                return new DateTime(Year, 6, DateTime.DaysInMonth(Year, 6), 23, 59, 59, 999);
            if (Qtr == Quarter.Third)
                return new DateTime(Year, 9, DateTime.DaysInMonth(Year, 9), 23, 59, 59, 999);
            return new DateTime(Year, 12, DateTime.DaysInMonth(Year, 12), 23, 59, 59, 999);
        }

        public static Quarter GetQuarter(Month month)
        {
            if (month <= Month.March)
                return Quarter.First;
            if (month >= Month.April && month <= Month.June)
                return Quarter.Second;
            return month >= Month.July && month <= Month.September ? Quarter.Third : Quarter.Fourth;
        }

        public static DateTime GetEndOfLastQuarter()
        {
            if (DateTime.Now.Month <= 3)
                return GetEndOfQuarter(DateTime.Now.Year - 1, GetQuarter(Month.December));
            var now = DateTime.Now;
            var year = now.Year;
            now = DateTime.Now;
            var num = (int) GetQuarter((Month) now.Month);
            return GetEndOfQuarter(year, (Quarter) num);
        }

        public static DateTime GetStartOfLastQuarter()
        {
            if (DateTime.Now.Month <= 3)
                return GetStartOfQuarter(DateTime.Now.Year - 1, GetQuarter(Month.December));
            var now = DateTime.Now;
            var year = now.Year;
            now = DateTime.Now;
            var num = (int) GetQuarter((Month) now.Month);
            return GetStartOfQuarter(year, (Quarter) num);
        }

        public static DateTime GetStartOfCurrentQuarter()
        {
            var now = DateTime.Now;
            var year = now.Year;
            now = DateTime.Now;
            var num = (int) GetQuarter((Month) now.Month);
            return GetStartOfQuarter(year, (Quarter) num);
        }

        public static DateTime GetEndOfCurrentQuarter()
        {
            var now = DateTime.Now;
            var year = now.Year;
            now = DateTime.Now;
            var num = (int) GetQuarter((Month) now.Month);
            return GetEndOfQuarter(year, (Quarter) num);
        }

        public static DateTime GetStartOfLastWeek()
        {
            var dateTime = DateTime.Now.Subtract(TimeSpan.FromDays((double) (DateTime.Now.DayOfWeek + 7)));
            return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 0, 0, 0, 0);
        }

        public static DateTime GetEndOfLastWeek()
        {
            var dateTime = GetStartOfLastWeek().AddDays(6.0);
            return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 23, 59, 59, 999);
        }

        public static DateTime GetStartOfCurrentWeek()
        {
            var dateTime = DateTime.Now.Subtract(TimeSpan.FromDays((double) DateTime.Now.DayOfWeek));
            return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 0, 0, 0, 0);
        }

        public static DateTime GetEndOfCurrentWeek()
        {
            var dateTime = GetStartOfCurrentWeek().AddDays(6.0);
            return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 23, 59, 59, 999);
        }

        public static DateTime GetStartOfMonth(int Month, int Year)
        {
            return new DateTime(Year, Month, 1, 0, 0, 0, 0);
        }

        public static DateTime GetEndOfMonth(int Month, int Year)
        {
            return new DateTime(Year, Month, DateTime.DaysInMonth(Year, Month), 23, 59, 59, 999);
        }

        public static DateTime GetStartOfLastMonth()
        {
            if (DateTime.Now.Month == 1)
                return GetStartOfMonth(12, DateTime.Now.Year - 1);
            var now = DateTime.Now;
            var Month = now.Month - 1;
            now = DateTime.Now;
            var year = now.Year;
            return GetStartOfMonth(Month, year);
        }

        public static DateTime GetEndOfLastMonth()
        {
            if (DateTime.Now.Month == 1)
                return GetEndOfMonth(12, DateTime.Now.Year - 1);
            var now = DateTime.Now;
            var Month = now.Month - 1;
            now = DateTime.Now;
            var year = now.Year;
            return GetEndOfMonth(Month, year);
        }

        public static DateTime GetStartOfCurrentMonth()
        {
            var now = DateTime.Now;
            var month = now.Month;
            now = DateTime.Now;
            var year = now.Year;
            return GetStartOfMonth(month, year);
        }

        public static DateTime GetEndOfCurrentMonth()
        {
            var now = DateTime.Now;
            var month = now.Month;
            now = DateTime.Now;
            var year = now.Year;
            return GetEndOfMonth(month, year);
        }

        public static DateTime GetStartOfYear(int Year)
        {
            return new DateTime(Year, 1, 1, 0, 0, 0, 0);
        }

        public static DateTime GetEndOfYear(int Year)
        {
            return new DateTime(Year, 12, DateTime.DaysInMonth(Year, 12), 23, 59, 59, 999);
        }

        public static DateTime GetStartOfLastYear()
        {
            return GetStartOfYear(DateTime.Now.Year - 1);
        }

        public static DateTime GetEndOfLastYear()
        {
            return GetEndOfYear(DateTime.Now.Year - 1);
        }

        public static DateTime GetStartOfCurrentYear()
        {
            return GetStartOfYear(DateTime.Now.Year);
        }

        public static DateTime GetEndOfCurrentYear()
        {
            return GetEndOfYear(DateTime.Now.Year);
        }

        public static DateTime GetStartOfDay(DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day, 0, 0, 0, 0);
        }

        public static DateTime GetEndOfDay(DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day, 23, 59, 59, 999);
        }
    }
}