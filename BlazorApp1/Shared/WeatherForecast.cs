﻿using System;
using Google.Protobuf.WellKnownTypes;

namespace BlazorApp1.Shared
{
    public partial class WeatherForecast
    {
        // Properties for the underlying data are generated from the .proto file
        // This partial class just adds some extra convenience properties

        public DateTime Date
        {
            get => DateTimeStamp.ToDateTime();
            set { DateTimeStamp = Timestamp.FromDateTime(value.ToUniversalTime()); }
        }

        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
    }
}
