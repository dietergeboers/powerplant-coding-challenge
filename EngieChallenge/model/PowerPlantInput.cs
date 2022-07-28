namespace Engie.model
{
    public class PowerPlantInput
    {
        public string name { get; set; }
        public string type { get; set; }
        public float efficiency { get; set; }
        public float pmin { get; set; }
        public float pmax { get; set; }

        internal string Type()
        {
            switch (type)
            {
                case "gasfired": return "gas(euro/MWh)";
                case "turbojet": return "kerosine(euro/MWh)";
                case "windturbine": return "wind(%)";
            }
            return "error";
        }
    }
}
