namespace RedPhoenix.Infrastructure.BaccaratScore;

public class BigRoadItem : RoadBase
{
    public BigRoadItem()
    {

    }

    public BigRoadItem(int order, int result, GameResult gameResult, PairResult pairResult)
    {
        Order = order;
        Result = result;
        GameResult = gameResult;
        PairResult = pairResult;
        IsDecide = true;
    }

    public int Order { get; set; }
    public int Result { get; set; }
    public GameResult GameResult { get; set; }
    public PairResult PairResult { get; set; }
}