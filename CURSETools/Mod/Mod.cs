using System;

namespace CURSETools.Mod
{
    public class Mod
    {
        public string game_name { get; set; }
        public string game_id { get; set; }
        public int average_downloads { get; set; }
        public double supports { get; set; }
        public int downloads { get; set; }
        public DateTime updated { get; set; }
        public DateTime created { get; set; }
        public int favorited { get; set; }
        public string project_url { get; set; }
        public string release_type { get; set; }
        public string license { get; set; }
        public string newest_file { get; set; }
        public string[] project_manager { get; set; }
        public string[] contributor { get; set; }
    }
}
