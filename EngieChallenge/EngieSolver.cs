using Engie.model;

namespace Engie
{
    
    public class EngieSolver
    {
        private readonly EngieChallenge input;
        public EngieSolver(EngieChallenge input)
        {
           
            this.input = input;

        }
        //compares 2 powerplants and indicates which is cheapsest
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
        /***
         * Compare 2 plant schedules based on their cost
         */
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
         * 
         *maxRuns denotes the amount of powerplant schedules to be considered
         *no argument or any negative value for maxRuns results in running forever ( untill there is no more memory)
         */
        public PowerPlantSchedule solve(int maxRuns = -1)
        {
            //cheapest first 
            Array.Sort(input.PowerPlants, new PlantCostPerMWComparer());
            //collection to keep the best schedules (only 1 is actully needed in my scheme)
            TopX<PowerPlantSchedule> bestSchedules = new TopX<PowerPlantSchedule>(new PowerPlantScheduleComparer(), 10);
            //the fifo collection representing the schedules being considered
            Queue<PowerPlantSchedule> queue = new Queue<PowerPlantSchedule>();
            //start an initial schedule that satisfies the load
            PowerPlantSchedule initialSchedule = InitialSchedule(input);
            //include the initial schedule in your best schedules
            if(initialSchedule.Power() == input.Load)
                bestSchedules.Add(initialSchedule);
            //start the search with initial schedule
            queue.Enqueue(initialSchedule);
           
            //breadth first search of all powerplant schedules derivable from the initial schedule
            
            try
            {

                //maxRuns limits the time spend on searching, it is unused right now
                // once queue.Count == 0 there are no more schedules to be considered.
                while ((maxRuns < 0 || maxRuns-- > 0) && queue.Count > 0)
                {
                    PowerPlantSchedule schedule = queue.Dequeue();
                    //add all neighbouring schedules, see Neighbours 
                    foreach (var neighbour in schedule.Neighbours())
                    {
                        queue.Enqueue(neighbour);
                        if(neighbour.Power() == input.Load)
                            bestSchedules.Add(neighbour);
                        
                    }
                }
            }
            catch (Exception ex)
            {
                //expecting out of memory on sufficiently large problems
                Console.WriteLine("aborted search: " + ex.Message);
            }
            
            while (bestSchedules.Count > 1)
            {
                bestSchedules.Dequeue();
            }
            
            return bestSchedules.Dequeue();
        }

        private PowerPlantSchedule InitialSchedule(EngieChallenge input)
        {
            PowerPlantSchedule rv = new PowerPlantSchedule(input);

            if (rv.SatisfyLoad())
            {
                return rv;
            }

            throw new ScheduleException("no satisfying initial schedule found");
        }
    }
}
