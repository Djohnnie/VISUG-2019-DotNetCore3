using System;

namespace _09_BuiltInJsonSupport
{
    public class DataPoint
    {
        public Guid Id { get; set; }
        public String Title { get; set; }
        public String Description { get; set; }
        public DataPoint LeftData { get; set; }
        public DataPoint RightData { get; set; }
    }
}