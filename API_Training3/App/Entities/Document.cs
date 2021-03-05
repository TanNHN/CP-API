using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_Training3.Entities
{
    public class Document
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public TimeDate CreateAt { get; set; } = new TimeDate();
        public TimeDate UpdateAt { get; set; } = new TimeDate();
        public class TimeDate
        {
            public int Year { get; set; } = DateTime.Now.Year;
            public int Month { get; set; } = DateTime.Now.Month;
            public int Day { get; set; } = DateTime.Now.Day;
            public int Hour { get; set; } = DateTime.Now.Hour;
            public int Minute { get; set; } = DateTime.Now.Minute;
            public int Second { get; set; } = DateTime.Now.Second;
            public TimeDate()
            {
                Year = DateTime.Now.Year;
                Month = DateTime.Now.Month;
                Day = DateTime.Now.Day;
                Hour = DateTime.Now.Hour;
                Minute = DateTime.Now.Minute;
                Second = DateTime.Now.Second;
            }
            public override string ToString()
            {
                return this.Day + "-" + this.Month + "-" + this.Year + " "
                    + this.Hour + ":" + this.Minute + ":" + this.Second;
            }

        }
    }
}
