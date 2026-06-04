
namespace Model
{
    public class AStarPathFinder : IPathFinder
    {
        PathFinderType _algType = PathFinderType.Astar;
        public PathFinderType algType { get => _algType; set {} }

        public void FindPath(Maze maze, int[] pos, Queue<int[]> visitedPositions)
        {
            // de grote van de maze
            int rows = maze.MazeArray.Length;
            int cols = maze.MazeArray[0].Length;

            // setup
            var g_score = new int[rows, cols]; // dit moet f_score en g_score zijn, astar heeft geen distance array
            var f_score = new int[rows, cols];
            var Previous = new int[rows, cols];
            bool[,] VisitedNodes = new bool[rows, cols];

            g_score[pos[0], pos[1]] = 0;
            f_score[pos[0], pos[1]] = Heuristic(pos[0], pos[1], maze);

            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < cols; col++)
                {
                    g_score[row, col] = int.MaxValue;
                    f_score[row, col] = int.MaxValue;

                    Previous[row, col] = -1;

                    VisitedNodes[row, col] = false;
                }
            }

            while(true)
            {
                int min = int.MaxValue;
                int[] current = null!;

                for(int row = 0; row < rows; row++)
                {
                    for(int col = 0; col < cols; col++)
                    {
                        int fScore = g_score[row, col] + Heuristic(row, col, maze);

                        if (!VisitedNodes[row, col] && fScore < min)
                        {
                            min = fScore;
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

                foreach(var move in maze.moves)
                {
                    int newCol = Col + move[0];
                    int newRow = Row + move[1];

                    if (!maze.IsValidMove(newCol, newRow))
                        continue;

                    if (VisitedNodes[newRow, newCol])
                        continue;

                    int newGScore = g_score[Row, Col] + 1;
                    int newFScore = newGScore + Heuristic(newRow, newCol, maze);

                    if (newGScore < g_score[newRow, newCol])
                    {
                        g_score[newRow, newCol] = newGScore;
                        f_score[newRow, newCol] = newFScore;
                        Previous[newRow, newCol] = Row * cols + Col;
                    }
                }
            }
        }

        public int Heuristic(int row, int col, Maze maze)
        {
            return Math.Abs(row - maze.End[0]) + Math.Abs(col - maze.End[1]);
        }
    }
}