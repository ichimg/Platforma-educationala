using EducationalPlatform.Domain.Models;
using System;
using System.Globalization;
using System.Windows.Data;

namespace EducationalPlatform.EnumConverters
{
        public class ERoleToStringConverter : IValueConverter
        {
            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                if (value is ERole role)
                {
                    switch (role)
                    {
                    case ERole.Student:
                        return "Elev";

                    case ERole.Teacher:
                        return "Profesor";

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
