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

            Distance[pos[0], pos[1]] = 0;

            while(true)
            {
                int min = int.MaxValue;
                int[] current = null;

                for(int row = 0; row < rows; row++)
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
                // visitedPositions.Enqueue(current);

                if (Row == maze.End[0] && Col == maze.End[1]) break;

                foreach(var move in maze.moves)
                {
                    int newCol = Col + move[0];
                    int newRow = Row + move[1];

                    if (!maze.IsValidMove(newRow, newCol))
                        continue;

                    if (VisitedNodes[newRow, newCol])
                        continue;
                    int alt = Distance[Row, Col] + 1;
                    if (alt < Distance[newRow, newCol])
                    {
                        Distance[newRow, newCol] = alt;
                        Previous[newRow, newCol] = Row * cols + Col;
                    }
                }
            }

            // Reconstruct path backwards from End to Begin
            int[] goal = new int[] { maze.End[0], maze.End[1] }; // op deze manier want anders overwrite je de maze.End waarde

            Stack<int[]> Path = new Stack<int[]>();

            while (!(goal[0] == pos[0] && goal[1] == pos[1]))
            {
                Path.Push(new int[] { goal[0], goal[1] });
                int prevIndex = Previous[goal[0], goal[1]];
                if (prevIndex == -1) break; // No path found
                goal[0] = prevIndex / cols;
                goal[1] = prevIndex % cols;
            }

            Path.Push(pos); // Add the starting position

            while(Path.Count > 0)
            {
                visitedPositions.Enqueue(Path.Pop());
            }
        }
   }
}