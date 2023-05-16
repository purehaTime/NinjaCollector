using System;

namespace ModelsHelper.Models
{
    public class PosterSettings : Settings
    {
        public bool UseScheduling { get; set; }
        public int ScheduleInterval { get; set; }
        public bool UseRandom { get; set; }
        public bool IgnoreHistory { get; set; }
        public bool UseImagesOnly { get; set; }
        public bool ContinuePosting { get; set; }

    }
}
