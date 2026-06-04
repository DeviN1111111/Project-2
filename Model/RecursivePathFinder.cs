namespace Model
{
    public class RecursivePathFinder : IPathFinder
    {
        PathFinderType _algType = PathFinderType.Recursive;
        public PathFinderType algType { get => _algType; set {} }

        public void FindPath(Maze maze, int[] pos, Queue<int[]> visitedPositions)
        {
            int row = pos[0];
            int col = pos[1];

            if(!maze.IsValidMove(row, col)) return; // als je buiten de maze bent of tegen een muur aanloopt
            if(AlreadyVisited(visitedPositions, pos)) return; // als je al op deze positie bent geweest

            visitedPositions.Enqueue(pos);

            if(AlreadyVisited(visitedPositions, maze.End)) return; // als je het eind hebt gevonden

            foreach(var move in maze.moves)
            {
                int newRow = row + move[0];
                int newCol = col + move[1];

                FindPath(maze, new int[] { newRow, newCol }, visitedPositions);

                if(AlreadyVisited(visitedPositions, maze.End)) return; // als je het eind hebt gevonden
            }
        }

        bool AlreadyVisited(Queue<int[]> visited, int[] pos) // checkt of je al op deze positie bent geweest
        {
            foreach (var p in visited)
            {
                if (p[0] == pos[0] && p[1] == pos[1])
                    return true;
            }
            return false;
        }
    }
}
