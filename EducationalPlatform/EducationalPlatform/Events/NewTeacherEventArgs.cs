using EducationalPlatform.DataAccess.Models;
using System;

namespace EducationalPlatform.Events
{
    public class NewTeacherEventArgs : EventArgs
    {
        public Teacher Data { get; }

        public NewTeacherEventArgs(Teacher data)
        {
            Data = data;
        }

    }
}
