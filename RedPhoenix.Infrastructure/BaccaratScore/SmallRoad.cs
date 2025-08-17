namespace RedPhoenix.Infrastructure.BaccaratScore;
public class SmallRoad : DownRoad
{
    public SmallRoad(int row, int column, IEnumerable<RoundResult> result) : base(row, column, result)
    {
        var bigRoad = new BigRoad(Row, RoundResults.Count(), RoundResults);
        var bigEyeRoadData = GenerateBigEyeRoadItemList(bigRoad.ResultItems);
        ResultItems = GenerateSmallRoadGraph(bigEyeRoadData);
    }

    public override DownRoadItem BankerPrediction()
    {
        var fakeNextRound = new RoundResult(RoundResults.Count(), 0,
            GameResult.BankerWin, PairResult.NoPair);

        return GetPrediction(fakeNextRound, DownRoadGap.SmallRoadGap);
    }

    public override DownRoadItem PlayerPrediction()
    {
        var fakeNextRound = new RoundResult(RoundResults.Count(), 0,
            GameResult.PlayerWin, PairResult.NoPair);

        return GetPrediction(fakeNextRound, DownRoadGap.SmallRoadGap);
    }

    public List<List<DownRoadItem>> ResultItems { get; }

    private List<DownRoadItem> GenerateBigEyeRoadItemList(
        List<List<BigRoadItem>> bigRoadItemGraph)
        => GenerateDownRoadData(bigRoadItemGraph, DownRoadGap.SmallRoadGap);

    private List<List<DownRoadItem>> GenerateSmallRoadGraph(List<DownRoadItem> smallRoadData)
        => WrapRow(
            WrapColumn(smallRoadData,
                (previousItem, currentItem) => previousItem.Repetition == currentItem.Repetition),
            Row, Column);

}