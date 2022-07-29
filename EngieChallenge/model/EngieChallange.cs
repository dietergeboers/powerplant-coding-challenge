namespace Engie.model
{
    public class EngieChallenge
    {
        public IDictionary<string, float> fuels { get; set; }
        public int Load { get; set; }

        public PowerPlant[] PowerPlants { get; set; }
        public float CO2Price { get; internal set; }

        override public string ToString()
        {
            string rv = $"Requested load:{Load}\n\n";
            rv += "Fuels:\n";
            foreach (var kvp in fuels)
            {
                rv += " " + kvp.Key + ": " + kvp.Value + "\n";
            }
            rv += "Power plants:\n";
            foreach (PowerPlant p in PowerPlants)
            {
                rv += " " + p.ToString();
            }
            return rv;
        }
    }
}
