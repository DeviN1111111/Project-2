
namespace Model
{
    public class StackPathFinder : IPathFinder
    {
        PathFinderType _algType = PathFinderType.Stack;
        public PathFinderType algType { get => _algType; set {} }

        public void FindPath(Maze maze, int[] pos, Queue<int[]> visitedPositions)
        {
            var visitedStack = new Stack<int[]>();
            visitedStack.Push(pos);

            
            while(visitedStack.Count > 0)
            {
                int[] nextMove = visitedStack.Pop();

                int row = nextMove[0];
                int col = nextMove[1];

                if(!maze.IsValidMove(row, col)) continue; // als je buiten de maze bent of tegen een muur aanloopt
                if(visitedPositions.Any(p => p[0] == row && p[1] == col)) continue; // als je al op deze positie bent geweest
                visitedPositions.Enqueue(nextMove);
                if(row == maze.End[0] && col == maze.End[1]) return; // eind gevonden

                foreach(var move in maze.moves)
                {
                    var newPosition = new int[] { row + move[0], col + move[1]};
                    visitedStack.Push(newPosition);
                
                }
            }
            return;
        }     
    }
}

            

