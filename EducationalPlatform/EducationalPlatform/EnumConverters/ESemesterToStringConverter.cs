﻿using EducationalPlatform.Domain.Models;
using System;
using System.Globalization;
using System.Windows.Data;

namespace EducationalPlatform.EnumConverters
{
    public class ESemesterToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is ESemester role)
            {
                switch (role)
                {
                    case ESemester.First:
                        return "1";

                    case ESemester.Second:
                        return "2";

                    default:
                        break;
                }

            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
