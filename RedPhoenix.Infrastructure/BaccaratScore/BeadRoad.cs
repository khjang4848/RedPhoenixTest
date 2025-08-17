namespace RedPhoenix.Infrastructure.BaccaratScore;

using System.Collections.Immutable;

public class BeadRoad : Road<BeadRoadItem>
{
    public BeadRoad(int row, int column, IEnumerable<RoundResult> result) : base(row, column, result)
    {
        ResultItems = GenerateBeadRoadGraph();
    }

    private List<List<BeadRoadItem>> GenerateBeadRoadGraph()
    {
        //row.RemoveRange(0, row.Count - columnCount);
        var removeStartCount = RoundResults.Count() > Row * Column ? RoundResults.Count() - (Row * Column) : RoundResults.Count();
        var data = RoundResults.ToImmutableArray().RemoveRange(removeStartCount, RoundResults.Count() - removeStartCount);
        var result = new List<BeadRoadItem>[Row];

        for (var i = 0; i < result.Length; i++)
        {
            var tempItem = new BeadRoadItem[Column];
            for (var j = 0; j < tempItem.Length; j++)
            {
                var dataIndex = j * Row + i;

                if (dataIndex >= data.Length)
                {
                    break;
                }

                tempItem[j] = new BeadRoadItem
                {
                    Result = data[dataIndex].Result,
                    GameResult = data[dataIndex].GameResult,
                    PairResult = data[dataIndex].PairResult,
                    IsDecide = true
                };
            }

            result[i] = tempItem.ToList();
        }

        return result.ToList();

    }
    public List<List<BeadRoadItem>> ResultItems { get; }
}