using System.Collections.Generic;
using TestCode.Models;
using System.Linq;
using TestCode.Comparer;

namespace TestCode
{
    public class F1Stats
    {
        public IEnumerable<Team> TeamReferenceData { get; set; }
        public IF1StatsWeighting StatsWeighting { get; set; }

        public F1Stats(IEnumerable<Team> teamReferenceData, IF1StatsWeighting statsWeighting)
        {
            TeamReferenceData = teamReferenceData;
            StatsWeighting = statsWeighting;
        }

        // TODO: Return the driver for the specified car number, or null if not located.
        // The carNo parameter must be > 0. If it is not then return a null result.
        // Note
        //   Team.Drivers has the drivers for the team.
        //   Driver.CarNumber contains the car number
        public Driver DriverByCarNo(int carNo)
        {
            if (carNo <= 0)
                return null;

            var driver = TeamReferenceData.ToList().SelectMany(team => team.Drivers.Where(racer => racer.CarNumber == carNo))
                .FirstOrDefault(racer => racer.CarNumber == carNo);

            if (driver == null)
                return null;
            else
                return driver;

        }

        // TODO: For each team return their win % as well as their drivers win %, sorted by the team 'win %' highest to lowest.
        
        // If a teamId is specified then return data for only that team i.e. method result list will only a single entry

        // otherwise if the teamId=0 return item data for each team in TeamReferenceData supplied in the constructor
        // If a team is specified and cannot be located then return a empty list.

        // NB! If a teams drivers has done a total of 100 or more races (sum of all driver races) then
        //     IF1StatsWeighting must be invoked with the required parameters.
        //              winPercentage is the teams drivers win % (summed result of each driver)
        //              numberOfRaces is the teams drivers races (sum of all driver races)
        //    ONLY make this call if the drivers total races >= 100.

        //    The result must be stored in the DriverWeighting field of team value.
        //    If the drivers have done less than 100 races the DriverWeighting must be set to 0.

        // Note
        //   Team Win % is Team.Victories over Team.Races
        //   Driver Win % is Driver.Wins over Driver.Races
        //   Only calculate win % for drivers. Exclude test drivers
        public IEnumerable<TeamValue> TeamWinPercentage(int teamId = 0)
        {
            IEnumerable<TeamValue> teamValueCollection = new List<TeamValue>();

            if (teamId != 0)
            {
                // Get team for teamId
                Team team = TeamReferenceData.FirstOrDefault(t => t.Id == teamId);
                if (team == null)
                    return teamValueCollection;

                // I assume the client want these default values set
                TeamValue teamValue = new TeamValue();
                teamValue.Id = team.Id;
                teamValue.Name = team.Name;

                // Team teamWinsPercentage
                teamValue.TeamWinsPercentage = (((double)team.Victories) / team.Races) * 100;
               
                // Team Drivers
                IEnumerable<Driver> teamDrivers = team.Drivers.Where(driver => driver.GetType().Name != "TestDriver");

                // Team Drivers Wins
                double teamsDriversWins = teamDrivers.Sum(driverWins => driverWins.Wins);

                // Team Drivers Races
                double teamsDriversRaces = teamDrivers.Sum(driverWins => driverWins.Races);

                // Team DriverWeighting
                if (teamsDriversRaces >= 100)
                {
                    teamValue.DriverWeighting = StatsWeighting.Apply(teamValue.TeamWinsPercentage, (int)teamsDriversRaces); ;
                }
                else
                {
                    teamValue.DriverWeighting = 0;
                }

                // Team DriverWinPercentage
                teamValue.DriverWinPercentage = ((double)teamsDriversWins / teamsDriversRaces) * 100;

                ((List<TeamValue>)teamValueCollection).Add(teamValue);
                return teamValueCollection;
            }
            else
            {
                int maxTeams = TeamReferenceData.Count();
                IEnumerator<Team> teamIterator = TeamReferenceData.GetEnumerator();
                TeamValue teamValue;

                while (teamIterator.MoveNext())
                {
                    teamValue = new TeamValue();
                    teamValue.Id = teamIterator.Current.Id;
                    teamValue.Name = teamIterator.Current.Name;

                    teamValue.TeamWinsPercentage = (((double)teamIterator.Current.Victories) / teamIterator.Current.Races) * 100;

                    IEnumerable<Driver> currentTeamDrivers = teamIterator.Current.Drivers.Where(driver => driver.GetType().Name != "TestDriver");

                    double teamsDriversWins = currentTeamDrivers.Sum(driverWins => driverWins.Wins);

                    double teamsDriversRaces = currentTeamDrivers.Sum(driverWins => driverWins.Races);

                    if (teamsDriversRaces >= 100)
                    {
                        teamValue.DriverWeighting = StatsWeighting.Apply(teamValue.TeamWinsPercentage, (int)teamsDriversRaces); ;
                    }
                    else
                    {
                        teamValue.DriverWeighting = 0;
                    }

                    teamValue.DriverWinPercentage = (((double)teamsDriversWins) / teamsDriversRaces) * 100;

                    ((List<TeamValue>)teamValueCollection).Add(teamValue);
                }

                ((List<TeamValue>)teamValueCollection).Sort(new TeamValueComparer());
                return teamValueCollection;
            }
        }

    }
}
