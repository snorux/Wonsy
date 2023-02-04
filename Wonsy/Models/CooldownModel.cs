namespace Wonsy.Models
{
    public class CooldownModel
    {
        private string _map;

        public string Map 
        {
            get { return _map.Replace("_cooldown", ""); }
            set { _map = value; }
        }

        public string Cooldown { get; set; }
    }
}
