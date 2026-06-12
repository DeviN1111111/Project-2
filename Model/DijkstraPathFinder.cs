using System.Data;

namespace Model
{
    public class DijkstraPathFinder : IPathFinder
    {
        PathFinderType _algType = PathFinderType.Dijkstra;
        public PathFinderType algType { get => _algType; set {} }
        
        public void FindPath(Maze maze, int[] pos, Queue<int[]> visitedPositions)
        {
            // de grote van de maze
            int rows = maze.MazeArray.Length;
            int cols = maze.MazeArray[0].Length;
            
            // setup
            var Distance = new int[rows, cols];
            var Previous = new int[rows, cols];
            bool[,] VisitedNodes = new bool[rows, cols];
            
            // Initialiseer alle posities met oneindig, geen vorige positie en niet bezocht
            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < cols; col++)
                {
                    Distance[row, col] = int.MaxValue;

                    Previous[row, col] = -1;

                    VisitedNodes[row, col] = false;
                }
            }

            while(true)
            {
                // Zoek de onbezochte positie met de kleinste afstand (kortste weg tot nu toe)
                int min = int.MaxValue;
                int[] current = null!;

                for(int row = 0; row < rows; rows++)
                {
                    for(int col = 0; col < cols; col++)
                    {
                        if(!VisitedNodes[row, col] && Distance[row, col] < min)
                        {
                            min = Distance[row, col];
                            current = new int[] { row, col };
                        }
                    }
                }

                if(current == null) break;

                int Row = current[0];
                int Col = current[1];

                // zet als visited
                VisitedNodes[Row, Col] = true;
                visitedPositions.Enqueue(current);

                if (Row == maze.End[0] && Col == maze.End[1]) break;

                // Check elk mogelijke move van de huidige positie
                foreach(var move in maze.moves)
                {
                    int newCol = Col + move[0];
                    int newRow = Row + move[1];

                    if (!maze.IsValidMove(newCol, newRow))
                        continue;

                    if (VisitedNodes[newRow, newCol])
                        continue;
                }
            }
        }
   }
}
