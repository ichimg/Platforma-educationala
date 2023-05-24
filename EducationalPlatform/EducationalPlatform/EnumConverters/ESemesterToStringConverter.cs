using EducationalPlatform.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
