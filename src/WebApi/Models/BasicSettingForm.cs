﻿namespace WebApi.Models
{
    public class BasicSettingForm
    {
        public int Latency { get; set; }

        public string UserIdField { get; set; }

        public string Endpoint { get; set; }

        public int DataKeepDays { get; set; }
    }
}
