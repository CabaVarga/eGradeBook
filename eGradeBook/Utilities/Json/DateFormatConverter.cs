using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eGradeBook.Utilities.Json
{
    public class DateFormatConverter : IsoDateTimeConverter
    {
        // From https://stackoverflow.com/questions/18635599/specifying-a-custom-datetime-format-when-serializing-with-json-net
        public DateFormatConverter(string format)
        {
            DateTimeFormat = format;
        }

        public DateFormatConverter()
        {
            base.DateTimeFormat = "yyyy-MM-dd";
        }
    }
}