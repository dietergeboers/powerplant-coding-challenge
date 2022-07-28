using Engie.model;

namespace Engie
{
    public class PowerPlantSchedule
    {
        private readonly EngieChallange challange;

        private double[] power;
        private bool[] eliminated;
        public PowerPlantSchedule(EngieChallange challange)
        {
            this.challange = challange;
            power = new double[challange.PowerPlants.Length];
            eliminated = new bool[challange.PowerPlants.Length];
            for (int i = 0; i < challange.PowerPlants.Length; i++)
            {
                power[i] = pmin(i);
                if (challange.PowerPlants[i].isWind())
                {
                    SetMax(i);
                }
            }
        }
        private void eliminate(int i)
        {
            eliminated[i] = true;
            power[i] = 0;
        }

        private double pmax(int i)
        {
            return challange.PowerPlants[i].Pmax;
        }

        private double[] CostsVector()
        {
            double[] rv = new double[this.power.Length];
            for (int i = 0; i < this.power.Length; i++)
            {
                rv[i] = this.challange.PowerPlants[i].CostPerMW();
            }
            return rv;
        }

        public bool SatisifyLoad()
        {
            for (var i = 0; i < challange.PowerPlants.Length; i++)
            {
                if (eliminated[i])
                    continue;

                SetMax(i);
                if (Power() >= challange.Load)
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
                TotalPower += challange.PowerPlants[i].Cost(power[i]);
            }
            return TotalPower;
        }

        public double Power()
        {
            return power.Sum();

        }

        internal void SetMax(int i)
        {
            power[i] = challange.PowerPlants[i].Pmax;
        }
        public override string ToString()
        {
            string rv = $"schedule: \n {Power()} MW\n {Cost()} $\n";
            for (int i = 0; i < power.Length; i++)
            {
                rv += $" {challange.PowerPlants[i].Name} {power[i]}\n";
            }
            return rv;
        }
        public string ToJson()
        {
            string rv = "[\n";
            for (int i = 0; i < power.Length; i++)
            {
                rv += "  {\n";
                rv += $"    \"Name\": \"{challange.PowerPlants[i].Name}\",\n";
                rv += $"    \"p\": \"{power[i]}\"\n";
                rv += "  }" + (i == power.Length - 1 ? "" : ",") + "\n";

            }
            rv += "]";
            return rv;
        }
        private double pmin(int i)
        {
            return challange.PowerPlants[i].Pmin;
        }
        internal void Shrink(int i)
        {
            if (i < 0 || challange.PowerPlants[i].isWind())
            {
                return;
            }
            var Extra = Power() - challange.Load;
            power[i] = Math.Max(pmin(i), power[i] - Extra);
        }
        public List<PowerPlantSchedule> Neighbours()
        {
            List<PowerPlantSchedule> result = new List<PowerPlantSchedule>();
            for (int i = 0; i < eliminated.Length; i++)
                if (!eliminated[i] && power[i] > 0)
                {
                    PowerPlantSchedule newSchedule = Clone();
                    newSchedule.eliminate(i);
                    if (newSchedule.SatisifyLoad())
                        result.Add(newSchedule);
                }
            return result;
        }

        private PowerPlantSchedule Clone() 
        {
            PowerPlantSchedule rv = new PowerPlantSchedule(challange);
            for (int i = 0; i < eliminated.Length; i++)
            {
                rv.eliminated[i] = eliminated[i];
                rv.power[i] = power[i];
            }
            return rv;
        }
        /*        public bool SatisfyLoadSimplex()
                {
                    TablauBuilder T = new TablauBuilder();
                    double[] costs = CostsVector();
                    T.setGoal(costs, false);
                    Console.WriteLine("minimize:");
                    for (int i = 0; i < costs.Length; i++)
                    {
                        Console.Write($"p{i}\t");
                    }
                    Console.WriteLine();
                    foreach (var co in costs)
                        Console.Write(co.ToString("0.00") + "\t");
                    Console.WriteLine();
                    Console.WriteLine();
                    for (int i = 0; i < costs.Length; i++)
                    {
                        Console.Write($"1\t");
                    }
                    Console.WriteLine($">={challange.Load}");
                    T.addInequality2(Enumerable.Repeat<double>(1, costs.Length).ToArray(), challange.Load, TablauBuilder.inequality.geq);


                    for (int i = 0; i < costs.Length; i++)
                    {
                        for (int j = 0; j < costs.Length; j++)
                        {
                            if (j == i)
                            {
                                Console.Write("1\t");
                            }
                            else
                                Console.Write("\t");
                        }
                        if (!eliminated[i] && !challange.PowerPlants[i].isWind())
                            T.addInequality(i, Pmin(i), TablauBuilder.inequality.geq);

                        Console.WriteLine($">={Pmin(i)}");
                    }
                    for (int i = 0; i < costs.Length; i++)
                    {
                        for (int j = 0; j < costs.Length; j++)
                        {
                            if (j == i)
                                Console.Write("1\t");
                            else
                                Console.Write("\t");
                        }
                        if (!eliminated[i] && !challange.PowerPlants[i].isWind())
                        {
                            T.addInequality(i, Pmax(i), TablauBuilder.inequality.leq);
                        }
                        Console.WriteLine($"<={Pmax(i)}");
                    }
                    for (int i = 0; i < costs.Length; i++)
                    {
                        if (!eliminated[i] && challange.PowerPlants[i].isWind())
                        {
                            T.addInequality(i, Pmax(i), TablauBuilder.inequality.eq);
                        }
                    }
                    //Console.WriteLine(T.Build());


                    T.Build().solve();
                    return true;
                }
                
                public Tablau BuildTableau()
                {
                    var builder = new TablauBuilder();


                    return builder.Build();
                }


         */
    }
}
