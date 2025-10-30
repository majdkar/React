using System;
using System.ComponentModel;
using System.Linq;

namespace SchoolV01.Application.Extensions
{
    public static class EnumExtensions
    {
        public static string ToDescriptionString(this Enum val)
        {
            var attributes = (DescriptionAttribute[])val.GetType().GetField(val.ToString()).GetCustomAttributes(typeof(DescriptionAttribute), false);

            return attributes.Length > 0
                ? attributes[0].Description
                : val.ToString();
        }
    }
    public static class IntExtensions
    {
        public static string ToIndianNumber(this int val)
        {
            return val.ToString().ToIndianNumber();
        }
        public static string ToIndianNumber(this string val)
        {
            string[] digits = ["٠", "١", "٢", "٣", "٤", "٥", "٦", "٧", "٨", "٩"];
            return string.Concat(val
                .Select(c => char.IsDigit(c)
                ? digits[(int)(char.GetNumericValue(c) + 0.5)]
                : c.ToString()));
        }
    }
    public static class Service
    {
        private static readonly string[] Ones = ["", "واحد", "اثنان", "ثلاثة", "أربعة", "خمسة", "ستة", "سبعة", "ثمانية", "تسعة"];
        private static readonly string[] Teens = ["عشرة", "أحد عشر", "اثنا عشر", "ثلاثة عشر", "أربعة عشر", "خمسة عشر", "ستة عشر", "سبعة عشر", "ثمانية عشر", "تسعة عشر"];
        private static readonly string[] Tens = ["", "", "عشرون", "ثلاثون", "أربعون", "خمسون", "ستون", "سبعون", "ثمانون", "تسعون"];
        private static readonly string[] Hundreds = ["", "مئة", "مئتان", "ثلاثمئة", "أربعمئة", "خمسمئة", "ستمئة", "سبعمئة", "ثمانمئة", "تسعمئة"];
        private static readonly string[] Thousand = ["", "ألف", "مليون", "مليار"];
        private static readonly string[] Thousand2 = ["", "ألفان", "مليونان", "ملياران"];
        private static readonly string[] Thousands = ["", "آلاف", "ملايين", "مليارات"];

        public static string ToArabicWords(this int number1)
        {
            long number = Convert.ToInt64(number1);
            if (number == 0)
                return "صفر";

            string words = "";

            for (int i = 0; number > 0; i++)
            {

                words = (number % 1000) switch
                {
                    > 10 => ConvertHundreds(number % 1000) + Thousand[i] + "-" + words,
                    > 2 => ConvertHundreds(number % 1000) + Thousands[i] + "-" + words,
                    2 when i > 0 => Thousand2[i] + "-" + words,
                    1 when i > 0 => Thousand[i] + "-" + words,
                    _ => number % 1000 == 0 ? words : ConvertHundreds(number % 1000) + "-" + words
                };

                number /= 1000;
            }

            return words.Trim('-').Replace("-", " و");
        }

        private static string ConvertHundreds(long number)
        {
            string words = "";

            if (number >= 100)
            {
                words += Hundreds[number / 100] + "-";
                number %= 100;
            }

            if (number >= 10 && number <= 19)
            {
                words += Teens[number % 10] + "-";
            }
            else
            {
                words += number % 10 == 0 ? "" : Ones[number % 10] + "-";
                words += number / 10 == 0 ? "" : Tens[number / 10] + "-";
            }

            return words.Trim('-').Replace("-", " و");
        }
    }
    public readonly struct DateTimeGlobally
    {
        public static DateTime Now => DateTime.UtcNow.AddHours(3);
    }

}