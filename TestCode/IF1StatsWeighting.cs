namespace TestCode
{
    public interface IF1StatsWeighting
    {
        /// <summary>
        ///   Returns a weighting of between 0 and 100 basing on the parameters supplied.
        /// </summary>
        ///<remarks>
        ///  The numberOfRaces must be >= 100 otherwise a exception will be thrown
        ///  The win % must be >= 0
        /// </remarks>
        double Apply(double winPercentage, int numberOfRaces);
    }
}
