using Engie.model;

namespace Engie
{
    public class PowerPlantSchedule
    {
        private readonly EngieChallenge challenge;

        private double[] power;
        private bool[] disabled;
        public PowerPlantSchedule(EngieChallenge challenge)
        {
            this.challenge = challenge;
            power = new double[challenge.PowerPlants.Length]; //0 by default
            disabled = new bool[challenge.PowerPlants.Length];//false by default
            
        }
        private void eliminate(int i)
        {
            disabled[i] = true;
            power[i] = 0;
        }

        private double pmax(int i)
        {
            return challenge.PowerPlants[i].Pmax;
        }

        /**
         * Try to increase the total power of this schedule to satisfy the demanded Load.
         */
        public bool SatisifyLoad()
        {
            for (var i = 0; i < power.Length; i++)
            {
                if (disabled[i])
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
                if (disabled[i])
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
        /***
         *All powerplant schedules where one plant that was running in this instance is disabled in the list of neighbours.
         *All returned powerplant schedules satisfy the requested load.
         */
        public List<PowerPlantSchedule> Neighbours()
        {
            List<PowerPlantSchedule> result = new List<PowerPlantSchedule>();
            for (int i = 0; i < disabled.Length; i++)
            {
                if (!disabled[i] && power[i] > 0)
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
            for (int i = 0; i < disabled.Length; i++)
            {
                rv.disabled[i] = disabled[i];
                rv.power[i] = power[i];
            }
            return rv;
        }
        
    }
}
