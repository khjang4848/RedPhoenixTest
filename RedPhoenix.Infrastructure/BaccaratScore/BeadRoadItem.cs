namespace RedPhoenix.Infrastructure.BaccaratScore;

public class BeadRoadItem : RoadBase
{
    public BeadRoadItem()
    {

    }

    public BeadRoadItem(int result, GameResult gameResult, PairResult pairResult)
    {
        Result = result;
        GameResult = gameResult;
        PairResult = pairResult;
    }

    public int Result { get; set; }
    public GameResult GameResult { get; set; }
    public PairResult PairResult { get; set; }
}
