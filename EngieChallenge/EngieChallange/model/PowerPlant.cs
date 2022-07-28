namespace Engie.model
{
    public class PowerPlant
    {
        private bool includeCo2;
        private double co2Price;

        private float cost { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }

        public double Efficiency { get; set; }
        public double Pmin { get; set; }
        public double Pmax { get; set; }

        public PowerPlant(PowerPlantInput input, EngieInput engieInput)
        {
            Name = input.name;
            Type = input.type;
            Efficiency = input.efficiency;
            Pmin = input.pmin;
            Pmax = input.pmax;
            if (input.type.Equals("windturbine"))
            {
                cost = 0;
                Pmax = Pmax * engieInput.Fuels["wind(%)"] / 100;
            }
            else
            {

                cost = engieInput.Fuels[input.Type()];
            }

            includeCo2 = false;
        }
        public void IncludeCo2(double co2Price)
        {
            includeCo2 = true;
            this.co2Price = co2Price;
        }
        public double Cost(double power)
        {
            double TotalCost = 0;
            if (!isWind() && includeCo2)
                TotalCost += 0.3 * power * co2Price;
            TotalCost += power * cost / Efficiency;

            return TotalCost;
        }
        public double CostPerMW()
        {
            return Cost(1);
        }
        public override string ToString()
        {
            return $"{Type}:\t [{Pmin}...{Pmax}]\te:{Efficiency}\t#{Name}\n";
        }

        internal bool isWind()
        {
            return Type.Equals("windturbine");
        }
    }
}
