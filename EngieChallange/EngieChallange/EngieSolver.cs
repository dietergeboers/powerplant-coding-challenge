using Engie.model;

namespace Engie
{
    public class I
    {
        public readonly int i;

        public I(int i)
        {
            this.i = i;
        }

    }
    public class CompareI : IComparer<I>
    {
        public int Compare(I? x, I? y)
        {
            return x.i - y.i;
        }
    }
    public class EngieSolver
    {
        private readonly EngieChallange input;
        public EngieSolver(EngieChallange input)
        {
            Console.WriteLine(input);
            this.input = input;

        }
        class PlantCostPerMWComparer : IComparer<PowerPlant>
        {
            public int Compare(PowerPlant? x, PowerPlant? y)
            {
                var diff = x.CostPerMW() - y.CostPerMW();
                if (diff == 0)
                    return 0;
                if (diff > 0)
                    return 1;
                return -1;
            }
        }
        class PowerPlantScheduleComparer : IComparer<PowerPlantSchedule>
        {
            public int Compare(PowerPlantSchedule? x, PowerPlantSchedule? y)
            {
                if (x.Cost() == y.Cost())
                    return 0;
                if (x.Cost() < y.Cost())
                    return 1;
                return -1;

            }
        }
        /**
         *maxRuns denotes the amount of powerplant schedules to be considered
         *no argument or any negative value for maxRuns results in running forever ( untill there is no more memory)
         */
        public PowerPlantSchedule solve(int maxRuns = -1)
        {
            Array.Sort(input.PowerPlants, new PlantCostPerMWComparer());
            //
            TopX<PowerPlantSchedule> top = new TopX<PowerPlantSchedule>(new PowerPlantScheduleComparer(), 10);
            Queue<PowerPlantSchedule> queue = new Queue<PowerPlantSchedule>();

            PowerPlantSchedule initialSchedule = InitialSchedule(input);

            top.Add(initialSchedule);
            queue.Enqueue(initialSchedule);
            try
            {
                while ((maxRuns < 0 || maxRuns-- > 0) && queue.Count > 0)
                {
                    PowerPlantSchedule schedule = queue.Dequeue();
                   // Console.WriteLine(schedule);
                    foreach (var neighbour in schedule.Neighbours())
                    {
                        queue.Enqueue(neighbour);
                        top.Add(neighbour);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("aborted search: " + ex.Message);
            }
            
            while (top.Count > 1)
            {
                top.Dequeue();
            }
            Console.WriteLine("---best---");
            return top.Dequeue();
        }

        private PowerPlantSchedule InitialSchedule(EngieChallange input)
        {
            PowerPlantSchedule rv = new PowerPlantSchedule(input);

            if (rv.SatisifyLoad())
            {
                return rv;
            }

            throw new ScheduleException("no satisfying initial schedule found");
        }
    }
}
