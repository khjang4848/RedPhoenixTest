namespace RedPhoenix.Infrastructure.BaccaratScore;

public class CockroachRoad : DownRoad
{
    public CockroachRoad(int row, int column, IEnumerable<RoundResult> result)
        : base(row, column, result)
    {
        var bigRoad = new BigRoad(Row, RoundResults.Count(), RoundResults);
        var cockroachRoadData = GenerateCockroachRoadItemList(bigRoad.ResultItems);
        ResultItems = GenerateCockroachRoadGraph(cockroachRoadData);

    }

    public override DownRoadItem BankerPrediction()
    {
        var fakeNextRound = new RoundResult(RoundResults.Count(), 0,
            GameResult.BankerWin, PairResult.NoPair);

        return GetPrediction(fakeNextRound, DownRoadGap.CockroachRoadGap);
    }

    public override DownRoadItem PlayerPrediction()
    {
        var fakeNextRound = new RoundResult(RoundResults.Count(), 0,
            GameResult.PlayerWin, PairResult.NoPair);

        return GetPrediction(fakeNextRound, DownRoadGap.CockroachRoadGap);
    }

    public List<List<DownRoadItem>> ResultItems { get; }

    private List<DownRoadItem> GenerateCockroachRoadItemList(
        List<List<BigRoadItem>> bigRoadItemGraph)
        => GenerateDownRoadData(bigRoadItemGraph, DownRoadGap.CockroachRoadGap);

    private List<List<DownRoadItem>> GenerateCockroachRoadGraph(
        List<DownRoadItem> CockroachRoadData)
        => WrapRow(
            WrapColumn(CockroachRoadData,
                (previousItem, currentItem) => previousItem.Repetition == currentItem.Repetition),
            Row, Column);
}
