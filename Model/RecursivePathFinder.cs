namespace Model
{
    public class RecursivePathFinder : IPathFinder
    {
        PathFinderType _algType = PathFinderType.Recursive;
        public PathFinderType algType { get => _algType; set {} }
        private bool endFound = false;
        public void FindPath(Maze maze, int[] pos, Queue<int[]> visitedPositions)
        {
            if(endFound) return; // als het eind al gevonden is, stop met zoeken
            int row = pos[0];
            int col = pos[1];

            if(!maze.IsValidMove(row, col)) return; // als je buiten de maze bent of tegen een muur aanloopt
            if(visitedPositions.Any(p => p[0] == pos[0] && p[1] == pos[1])) return; // als je al op deze positie bent geweest

            visitedPositions.Enqueue(pos);

            if(pos[0] == maze.End[0] && pos[1] == maze.End[1])
                endFound = true;

            foreach(var move in maze.moves)
            {
                int newRow = row + move[0];
                int newCol = col + move[1];
            
                FindPath(maze, new int[] { newRow, newCol }, visitedPositions);
            }
        }
    }
}
