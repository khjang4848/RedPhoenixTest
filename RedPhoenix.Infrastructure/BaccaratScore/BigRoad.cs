namespace RedPhoenix.Infrastructure.BaccaratScore;

public class BigRoad : Road<BigRoadItem>
{
    public BigRoad(int row, int column, IEnumerable<RoundResult> result) : base(row, column, result)
    {
        ResultItems = GenerateBigRoadGraph();
    }

    private List<List<BigRoadItem>> GenerateBigRoadGraph()
    {

        var column = WrapColumn(GenerateBigRoadItemList(), (x, y) => x.GameResult == y.GameResult);
        return WrapRow(column, Row, Column);
    }

    private List<BigRoadItem> GenerateBigRoadItemList()
    {
        return RoundResults.Where(x => x.GameResult != GameResult.Tie)
            .Select(x => new BigRoadItem(x.Order, x.Result, x.GameResult, x.PairResult)
            {
                IsDecide = true
            }).ToList();
    }
    public List<List<BigRoadItem>> ResultItems { get; }
}