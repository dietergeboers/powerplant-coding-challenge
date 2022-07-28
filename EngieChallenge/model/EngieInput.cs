namespace Engie.model
{
    public class EngieInput
    {
        public int load { get; set; }
        public IDictionary<string, float> Fuels { get; set; }
        public PowerPlantInput[] Powerplants { get; set; }


        public EngieChallenge Create()
        {
            return new EngieChallenge()
            {
                CO2Price = Fuels["co2(euro/ton)"],
                Load = load,
                PowerPlants = fromInputF(Powerplants),
                fuels = Fuels
            };
        }

        private PowerPlant[] fromInputF(PowerPlantInput[] powerplants)
        {
            PowerPlant[] rv = new PowerPlant[powerplants.Count()];
            for (int i = 0; i < rv.Count(); i++)
            {
                rv[i] = new PowerPlant(powerplants[i], this);
                rv[i].IncludeCo2(Fuels["co2(euro/ton)"]);

            }
            return rv;
        }
    }



}
