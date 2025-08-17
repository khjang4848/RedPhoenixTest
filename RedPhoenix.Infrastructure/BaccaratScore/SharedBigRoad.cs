namespace RedPhoenix.Infrastructure.BaccaratScore;
public class SharedBigRoad : BigRoad
{
    public SharedBigRoad(int row, int column, IEnumerable<RoundResult> result)
        : base(row, column, result)
    {
    }
}