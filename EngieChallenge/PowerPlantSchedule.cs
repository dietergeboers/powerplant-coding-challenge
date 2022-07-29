using Engie.model;

namespace Engie
{
    public class PowerPlantSchedule
    {
        private readonly EngieChallenge challenge;
        //The powerusage of each plant
        private double[] power;
        //the set of plants that won't be considered 
        private bool[] disabled;
        public PowerPlantSchedule(EngieChallenge challenge)
        {
            this.challenge = challenge;
            power = new double[challenge.PowerPlants.Length]; //0 by default
            disabled = new bool[challenge.PowerPlants.Length];//false by default
            
        }
        private void disable(int i)
        {
            disabled[i] = true;
            power[i] = 0;
        }

        private double pmax(int i)
        {
            return challenge.PowerPlants[i].Pmax;
        }

        /**
         * Set enabled powerplants to their maximum load and once the total provided power
         * exceeds the requested Load shrink the usage of all plants to match the 
         * requsted Load exactly. 
         *  return true if this scheme finds a sollution
         *  return false otherwise.
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
                    while(i >=0)
                        Shrink(i--);
                    if (Power() == challenge.Load)
                        return true;
                    else
                        return false;
                }
            }
            return false;

        }
        /***
         * Calculates the total cost of using the powerplants in this schedule
         */
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
        /***
         *Total amount of power used in this schedule:
         *aka sum of all elements in power[] 
         */
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
                rv += $"    \"p\": \"{Math.Round(power[i],2)}\"\n";
                rv += "  }" + (i == power.Length - 1 ? "" : ",") + "\n";

            }
            rv += "]";
            return rv;
        }
        private double pmin(int i)
        {
            return challenge.PowerPlants[i].Pmin;
        }

        /***
         *shrink the usage of plant i to try and match the requested load as close as possible
         */
        private void Shrink(int i)
        {
            if (i < 0 || challenge.PowerPlants[i].isWind())
            {
                return;
            }
            var Extra = Power() - challenge.Load;
            power[i] = Math.Max(pmin(i), Math.Min(power[i] - Extra,pmax(i)));
        }
        /***
         *All powerplant schedules where one plant that was running in this instance is disabled in the list of neighbours.
         *All returned powerplant schedules satisfy the requested load.
         */
        public List<PowerPlantSchedule> Neighbours()
        {
            List<PowerPlantSchedule> result = new List<PowerPlantSchedule>();
            for (int i = 0; i < power.Length; i++)
            {
                if (!disabled[i] && power[i] > 0)
                {
                    PowerPlantSchedule newSchedule = Clone();
                    newSchedule.disable(i);
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
