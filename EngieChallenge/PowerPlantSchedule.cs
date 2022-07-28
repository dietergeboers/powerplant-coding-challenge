using Engie.model;

namespace Engie
{
    public class PowerPlantSchedule
    {
        private readonly EngieChallenge challenge;

        private double[] power;
        private bool[] eliminated;
        public PowerPlantSchedule(EngieChallenge challenge)
        {
            this.challenge = challenge;
            power = new double[challenge.PowerPlants.Length];
            eliminated = new bool[challenge.PowerPlants.Length];
            
        }
        private void eliminate(int i)
        {
            eliminated[i] = true;
            power[i] = 0;
        }

        private double pmax(int i)
        {
            return challenge.PowerPlants[i].Pmax;
        }

        private double[] CostsVector()
        {
            double[] rv = new double[this.power.Length];
            for (int i = 0; i < this.power.Length; i++)
            {
                rv[i] = this.challenge.PowerPlants[i].CostPerMW();
            }
            return rv;
        }

        public bool SatisifyLoad()
        {
            for (var i = 0; i < challenge.PowerPlants.Length; i++)
            {
                if (eliminated[i])
                    continue;

                SetMax(i);
                if (Power() >= challenge.Load)
                {
                    Shrink(i);
                    return true;
                }
            }
            return false;

        }
        public double Cost()
        {
            double TotalPower = 0;
            for (int i = 0; i < power.Length; i++)
            {
                if (eliminated[i])
                    continue;
                TotalPower += challenge.PowerPlants[i].Cost(power[i]);
            }
            return TotalPower;
        }

        public double Power()
        {
            return power.Sum();

        }

        internal void SetMax(int i)
        {
            power[i] = challenge.PowerPlants[i].Pmax;
        }
        public override string ToString()
        {
            string rv = $"schedule: \n {Power()} MW\n {Cost()} $\n";
            for (int i = 0; i < power.Length; i++)
            {
                rv += $" {challenge.PowerPlants[i].Name} {power[i]}\n";
            }
            return rv;
        }
        public string ToJson()
        {
            string rv = "[\n";
            for (int i = 0; i < power.Length; i++)
            {
                rv += "  {\n";
                rv += $"    \"Name\": \"{challenge.PowerPlants[i].Name}\",\n";
                rv += $"    \"p\": \"{power[i]}\"\n";
                rv += "  }" + (i == power.Length - 1 ? "" : ",") + "\n";

            }
            rv += "]";
            return rv;
        }
        private double pmin(int i)
        {
            return challenge.PowerPlants[i].Pmin;
        }
        internal void Shrink(int i)
        {
            if (i < 0 || challenge.PowerPlants[i].isWind())
            {
                return;
            }
            var Extra = Power() - challenge.Load;
            power[i] = Math.Max(pmin(i), power[i] - Extra);
        }
        public List<PowerPlantSchedule> Neighbours()
        {
            List<PowerPlantSchedule> result = new List<PowerPlantSchedule>();
            for (int i = 0; i < eliminated.Length; i++)
            {
                if (!eliminated[i] && power[i] > 0)
                {
                    PowerPlantSchedule newSchedule = Clone();
                    newSchedule.eliminate(i);
                    if (newSchedule.SatisifyLoad())
                        result.Add(newSchedule);
                }
            }
            return result;
        }

        private PowerPlantSchedule Clone() 
        {
            PowerPlantSchedule rv = new PowerPlantSchedule(challenge);
            for (int i = 0; i < eliminated.Length; i++)
            {
                rv.eliminated[i] = eliminated[i];
                rv.power[i] = power[i];
            }
            return rv;
        }
        
    }
}
