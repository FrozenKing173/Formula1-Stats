using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestCode;
using TestCode.Models;

namespace TestCodeTests
{
    [TestFixture]
    public class F1StatsTests
    {
        F1Stats sut = null;

        [Test]
        public void DummyTestThatWillNotRunAsWeHaveNoTestFrameworkHookedUp()
        {
            sut = new F1Stats(SampleData.Teams, new F1StatsWeightingStub());

            var result = sut.DriverByCarNo(-1);

            Assert.Null(result);
        }

        [Test]
        public void TestDriverByCarNo_GivenWith_TeamDataAndCarNo1_ShouldReturn_aDriver()
        {
            sut = new F1Stats(SampleData.Teams, new F1StatsWeightingStub());

            var result = sut.DriverByCarNo(1);

            Assert.NotNull(result);
        }

        [Test]
        public void TestDriverByCarNo_GivenWith_TeamDataAndCarNo2_ShouldReturn_DriverWithCarNo2()
        {
            sut = new F1Stats(SampleData.Teams, new F1StatsWeightingStub());

            var result = sut.DriverByCarNo(2);

            Assert.AreEqual(result.CarNumber, 2);
        }

        [Test]
        public void TestDriverByCarNo_GivenWith_TeamDataAndCarNumNotLocated_ShouldBe_null ()
        {
            sut = new F1Stats(SampleData.Teams, new F1StatsWeightingStub());

            var result = sut.DriverByCarNo(9999);

            Assert.Null(result);
        }

        [Test]
        public void TestTeamWinPercentage_GivenWith_TeamDataAndTeam9999_ShouldBe_EmptyList()
        {
            sut = new F1Stats(SampleData.Teams, new F1StatsWeightingStub());

            var resultCollection = sut.TeamWinPercentage(9999);

            Assert.IsEmpty(resultCollection);
        }

        [Test]
        public void TestTeamWinPercentage_GivenWith_TeamDataAndTeam5_ShouldBe_1TeamValue()
        {
            sut = new F1Stats(SampleData.Teams, new F1StatsWeightingStub());

            var resultCollection = sut.TeamWinPercentage(5);

            Assert.AreEqual(resultCollection.Count(), 1);
        }

        [Test]
        public void TestTeamWinPercentage_GivenWith_TeamDataAndTeam5_ShouldBe_1TeamValueWithTeamWinsPercentage()
        {
            sut = new F1Stats(SampleData.Teams, new F1StatsWeightingStub());

            var resultCollection = sut.TeamWinPercentage(5);

            Assert.IsTrue(((List<TeamValue>)resultCollection)[0].DriverWinPercentage > 0);
        }

        [Test]
        public void TestTeamWinPercentage_GivenWith_TeamDataAndTeam5_ShouldBe_1TeamValueWithDriverWeighting()
        {
            sut = new F1Stats(SampleData.Teams, new F1StatsWeightingStub());

            var resultCollection = sut.TeamWinPercentage(5);

            Assert.IsTrue(((List<TeamValue>)resultCollection)[0].DriverWeighting == 1);
        }

        [Test]
        public void TestTeamWinPercentage_GivenWith_TeamDataAndTeam5_ShouldBe_1TeamValueWithDriverWinPercentage()
        {
            sut = new F1Stats(SampleData.Teams, new F1StatsWeightingStub());

            var resultCollection = sut.TeamWinPercentage(5);

            Assert.IsTrue(((List<TeamValue>)resultCollection)[0].DriverWinPercentage > 0);
        }

        [Test]
        public void TestTeamWinPercentage_GivenWith_TeamData_ShouldBe_ManyTeamValues()
        {
            sut = new F1Stats(SampleData.Teams, new F1StatsWeightingStub());

            var resultCollection = sut.TeamWinPercentage();

            Assert.IsTrue(resultCollection.Count() > 1);
        }

        [Test]
        public void TestTeamWinPercentage_GivenWith_TeamData_ShouldBe_ManyTeamValuesAndSortedDescendingByTeamWinsPercentage()
        {
            sut = new F1Stats(SampleData.Teams, new F1StatsWeightingStub());

            var resultCollection = sut.TeamWinPercentage();

            double highest = 0, current = 0;
            foreach(TeamValue team in resultCollection)
            {
                if (highest == 0)
                {
                    highest = team.TeamWinsPercentage;
                }

                current = team.TeamWinsPercentage;
               
                if (highest != current)
                {
                    Assert.IsTrue(highest > current);
                }

                highest = current;
            }
           
        }
    }
}
