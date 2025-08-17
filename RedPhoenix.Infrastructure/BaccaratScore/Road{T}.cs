namespace RedPhoenix.Infrastructure.BaccaratScore;
public abstract class Road<T>(int row, int column, IEnumerable<RoundResult> result)
    where T : RoadBase, new()
{
    protected List<List<T?>>? _array;

    public int Row { get; } = row;
    public int Column { get; } = column;
    public IEnumerable<RoundResult> RoundResults { get; } = result;


    public T? GetItem(int rowIndex, int columnIndex)
        => _array?[rowIndex][columnIndex];

    private static void TruncateColumn(List<List<T>> roadItemGraph, int columnCount)
    {
        var tempRowCount = roadItemGraph.Max(x => x.Count);

        foreach (var row in roadItemGraph)
        {
            if (columnCount < row.Count)
            {
                row.RemoveRange(0, row.Count - columnCount);
            }
            else
            {
                row.AddRange(new List<T>(columnCount - row.Count));
            }
        }
    }

    public List<List<T>> WrapColumn(IEnumerable<T> roadItemList, Func<T, T, bool> shouldNoWrapFn)
    {
        var resultGraph = new List<List<T>>();
        var tempColumn = new List<T>();

        foreach (var item in roadItemList)
        {
            if (tempColumn.Count == 0 || shouldNoWrapFn(tempColumn[^1], item))
            {
                tempColumn.Add(item);
            }
            else
            {
                resultGraph.Add(tempColumn);
                tempColumn = [item];
            }
        }

        resultGraph.Add(tempColumn);

        return resultGraph;
    }

    public List<List<T>> WrapRow(List<List<T>> roadItemList, int rowCount, int columnCount)
    {
        var resultGraph = new List<T>[rowCount];
        var count = 0;
        var countTie = 0;
        var currentRowIndex = 0;
        var currentColumnIndex = 0;
        var isTurn = false;
        var temp = false;
        //var tempcolumnCount = Math.Max(0, roadItemList.Max(x => x.Where(y => y != null).Count()));
        var tempcolumnCount = Math.Max(0, roadItemList.Max(x => x.Count));

        for (var i = 0; i < resultGraph.Length; i++)
        {
            resultGraph[i] = new T[tempcolumnCount].ToList();
        }


        for (var c = 0; c < roadItemList.Count(); c++)
        {
            if (isTurn && currentRowIndex == 0)
            {
                temp = true;
            }

            count = temp switch
            {
                true when currentRowIndex == 0 => currentColumnIndex + 1,
                true => currentColumnIndex - countTie + 1,
                _ => c
            };

            isTurn = false;
            countTie = 0;

            for (var r = 0; r < roadItemList[c].Count; r++)
            {
                if (roadItemList[c][r].IsDecide == true)
                {
                    if (r < rowCount)
                    {
                        if ((resultGraph[r][count] == null) && !isTurn)
                        {
                            resultGraph[r][count] = roadItemList[c][r];
                            currentRowIndex = r;
                            currentColumnIndex = count;
                        }
                        else
                        {
                            resultGraph[currentRowIndex][++currentColumnIndex] =
                                roadItemList[c][r];
                            isTurn = true;
                            countTie++;
                        }
                    }
                    else
                    {
                        isTurn = true;
                        resultGraph[currentRowIndex][++currentColumnIndex] =
                            roadItemList[c][r];
                        countTie++;
                    }
                }
                else
                {
                    break;
                }


            }

        }

        var maxColumnCount = (resultGraph.Max(row => row.IndexOf(row.Last(x => x != null))) + 1);

        foreach (var result in resultGraph)
        {
            //while (result.Count < maxColumnCount)
            //{
            //    result.Add(new T());
            //}

            if (result.Count > maxColumnCount)
            {
                result.RemoveRange(maxColumnCount, result.Count - maxColumnCount);
            }
        }

        TruncateColumn(resultGraph.ToList(), columnCount);

        return resultGraph.ToList();


    }

    private List<T> AddItem(List<T> source, int index)
    {


        var diff = source.Count - index - 1;

        if (diff >= 0)
            return source;

        for (var i = 0; i < Math.Abs(diff); i++)
        {
            source.Add(new T());
        }

        return source;

    }
}
