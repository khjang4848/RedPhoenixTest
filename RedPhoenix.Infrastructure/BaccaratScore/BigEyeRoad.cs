namespace RedPhoenix.Infrastructure.BaccaratScore;

public class BigEyeRoad : DownRoad
{
    public BigEyeRoad(int row, int column, IEnumerable<RoundResult> result) : base(row, column, result)
    {
        var bigRoad = new BigRoad(Row, RoundResults.Count(), RoundResults);
        var bigEyeRoadData = GenerateBigEyeRoadItemList(bigRoad.ResultItems);
        ResultItems = GenerateBigEyeRoadGraph(bigEyeRoadData);
    }

    public override DownRoadItem BankerPrediction()
    {
        var fakeNextRound = new RoundResult(RoundResults.Count(), 0,
            GameResult.BankerWin, PairResult.NoPair);

        return GetPrediction(fakeNextRound, DownRoadGap.BigEyeRoadGap);
    }

    public override DownRoadItem PlayerPrediction()
    {
        var fakeNextRound = new RoundResult(RoundResults.Count(), 0,
            GameResult.PlayerWin, PairResult.NoPair);

        return GetPrediction(fakeNextRound, DownRoadGap.BigEyeRoadGap);
    }

    public List<List<DownRoadItem>> ResultItems { get; }

    private List<DownRoadItem> GenerateBigEyeRoadItemList(
        List<List<BigRoadItem>> bigRoadItemGraph)
        => GenerateDownRoadData(bigRoadItemGraph, DownRoadGap.BigEyeRoadGap);

    private List<List<DownRoadItem>> GenerateBigEyeRoadGraph(
        List<DownRoadItem> bigEyeRoadData)
        => WrapRow(
            WrapColumn(bigEyeRoadData,
                (previousItem, currentItem) => previousItem.Repetition == currentItem.Repetition),
            Row, Column);
}
