
using System.Data;
using System.Security.Cryptography;

namespace Model
{
    public class Maze
    {
        public int[][] MazeArray { get; private set; }
        public int[,] MazeMDArray { get; private set; }
        public int[] Begin { get; private set; }
        public int[] End { get; private set; }

        public readonly int[][] moves = {           
            new int[] {  1,  0 },  //down
            new int[] { -1,  0 },  //up
            new int[] {  0, -1 },  //left
            new int[] {  0,  1 },  //right
            };
        
        public Maze() => GenerateMaze();
        public Maze(bool automatic = true) {if(automatic) GenerateMaze(); else GenerateFromText(MazeGrids.mazeText);}
        public Maze(int rows, int cols) {if(rows <= 0 && cols <= 0) GenerateFromText(MazeGrids.mazeText); else GenerateMaze(rows, cols);}
        public Maze(string lines) => GenerateFromText(lines);

        void GenerateFromText(string lines){
            MazeArray = ToMazeArray(lines);
            MazeMDArray = ToMazeMDArray(lines);
        }
    void GenerateMaze(int rows = 21, int cols = 41)
    {
        // if(rows < 4 || cols < 4) {rows = 20; cols = 40;}
        // if(rows % 2 != 0) {rows++;}
        // if(cols % 2 != 0) {cols++;}

        //ToDo...

        if (rows % 2 == 0) rows++;
        if (cols % 2 == 0) cols++;


        // Creates maze filled with walls
        MazeArray = new int[rows][];

        for (int r = 0; r < rows; r++)
        {
            MazeArray[r] = new int[cols];
            for (int c = 0; c < cols; c++)
            {
                MazeArray[r][c] = -1;
            }
        }

        // Set random starting room
        // Walls are even numbers and rooms odd numbers
        var rng = new Random();

        // Formula for total amount of rooms (rows/cols - 1) / 2
        int roomRows = (rows - 1) / 2;
        int roomCols = (cols - 1) / 2;
        // Formula to get index of room number 2 * room number + 1
        // Room numbers start at 0!!!!
        int startRow = 2 * rng.Next(roomRows) + 1; 
        int startCol = 2 * rng.Next(roomCols) + 1;

        // Set start position as room
        MazeArray[startRow][startCol] = 0;

        // Put start room in stack
        var stack = new Stack<(int row, int col)>();
        stack.Push((startRow, startCol));

        // Carve the paths
        while (stack.Count > 0)
        {
            var current = stack.Peek();

            // directions to next rooom
            var directions = new (int dRow, int dCol) []
            {
                (-2, 0), //up
                (2, 0), //down
                (0, -2), //left
                (0, 2) //right
            };

            // shuffle the directions array
            for (int i = directions.Length -1; i > 0; i--) // count downwards
            {
                int j = rng.Next(i + 1); // pick random index to swap
                var temp = directions[i];
                directions[i] = directions[j];
                directions[j] = temp;
            }

            bool moved = false;

            foreach (var dir in directions)
            {
                // choose the neighbour
                int nRow = current.row + dir.dRow;
                int nCol = current.col + dir.dCol;

                // check if it is inside maze and not outerwal
                bool insideGrid = nRow > 0 && nRow < rows -1
                                && nCol > 0 && nCol < cols -1;

                // check if it is a wall
                bool isWall = insideGrid && MazeArray[nRow][nCol] == -1;

                if (isWall)
                {
                    // carve the wall between the current and neighbour
                    int wallRow = current.row + dir.dRow / 2;
                    int wallCol = current.col + dir.dCol / 2;
                    MazeArray[wallRow][wallCol] = 0;
                    // mark neighbour as passaeg
                    MazeArray[nRow][nCol] = 0;
                    
                    stack.Push((nRow, nCol));
                    moved = true;
                    break;
                }
            }
            if (!moved)
            {
                stack.Pop();
            }
        }

        // pick random end position
        int endRow, endCol;
        do
        {
            endRow = 2 * rng.Next(roomRows) + 1;
            endCol = 2 * rng.Next(roomCols) + 1;
        }
        while (endRow == startRow && endCol == startCol);
        // assign start and end positions
        MazeArray[startRow][startCol] = 1;
        MazeArray[endRow][endCol] = 2;
        Begin = new int[] { startRow, startCol };
        End = new int[] { endRow, endCol };

        // fix view array
        int displayRows = rows - 1;
        int displayCols = cols - 1;

        int[][] croppedMazeArray = new int[displayRows][];

        for (int r = 0; r < displayRows; r++)
        {
            croppedMazeArray[r] = new int[displayCols];

            for (int c = 0; c < displayCols; c++)
            {
                croppedMazeArray[r][c] = MazeArray[r][c];
            }
        }

        MazeArray = croppedMazeArray;

        MazeMDArray = new int[displayRows, displayCols];

        for (int r = 0; r < displayRows; r++)
        {
            for (int c = 0; c < displayCols; c++)
            {
                MazeMDArray[r, c] = MazeArray[r][c];
            }
        }
    }

        int[][] ToMazeArray(string maze)
        {
            // substrings from the maze string
            var arrayLines = maze.Split(new char[] { '.', '\n', '\r' },
                StringSplitOptions.RemoveEmptyEntries);

            int[][] outArray = new int[arrayLines.Length][];

            for (var rowIdx = 0; rowIdx < arrayLines.Length; rowIdx++)
            {
                var line = arrayLines[rowIdx];
                // row array:
                var row = new int[line.Length];
                for (int colIdx = 0; colIdx < line.Length; colIdx++)
                {
                    //from chars to integers
                    switch (line[colIdx])
                    {
                        case 'x':
                            row[colIdx] = -1;  //walls
                            break;
                        case '1':
                            row[colIdx] = 1;   //begin
                            Begin = [rowIdx, colIdx];
                            break;
                        case '2':
                            row[colIdx] = 2;   //end 
                            End = [rowIdx, colIdx];
                            break;
                        default:
                            row[colIdx] = 0;   //not visited
                            break;
                    }
                }
                // row in the output jagged array.
                outArray[rowIdx] = row;
            }

            return outArray;
            
        }

        int[,] ToMazeMDArray(string maze)
        {
            // substrings from the maze string
            var arrayLines = maze.Split(new char[] { '.', '\n', '\r' },
                StringSplitOptions.RemoveEmptyEntries);

            var lineLength = 0;
            if (arrayLines != null && arrayLines.Length > 0)
                lineLength = arrayLines[0].Length;
            else
            throw new Exception($"Maze incorrect");
            
            for (var rowIdx = 0; arrayLines != null && rowIdx < arrayLines.Length; rowIdx++)
            {
                var line = arrayLines[rowIdx];
                if (arrayLines[rowIdx] == null || line.Length != lineLength)
                    throw new Exception($"Not same line length for rows in maze:\n at row 0: {lineLength}, at row {rowIdx}: {line.Length}");
            }
            
            int[,] outArray = new int[arrayLines.Length, lineLength];

            for (var rowIdx = 0; rowIdx < arrayLines.Length; rowIdx++)
            {
                var line = arrayLines[rowIdx];

                for (int colIdx = 0; colIdx < line.Length; colIdx++)
                {
                    //from chars to integers
                    switch (line[colIdx])
                    {
                        case 'x':
                            outArray[rowIdx, colIdx] = -1;  //walls
                            break;
                        case '1':
                            outArray[rowIdx, colIdx] = 1;   //begin
                            Begin = [rowIdx, colIdx];
                            break;
                        case '2':
                            outArray[rowIdx, colIdx] = 2;   //end 
                            End = [rowIdx, colIdx];
                            break;
                        default:
                            outArray[rowIdx, colIdx] = 0;   //not visited
                            break;
                    }
                }
            }
            return outArray;
        }

        static int CountNotVisited(int[][] maze)
        {
            int cnt = 0;
            if (maze != null && maze.Length > 0)
            {
                for (int rowIdx = 0; rowIdx < maze.Length; rowIdx++)
                {
                    for (int colIdx = 0; maze[rowIdx] != null && colIdx < maze[rowIdx].Length; colIdx++)
                    {
                        cnt = maze[rowIdx][colIdx] == 0 ? cnt + 1 : cnt;
                    }
                }
            }
            return cnt;
        }

        public int CountNotVisited() => CountNotVisited(MazeArray);

        static bool IsValidPos(int[][] array, int newRow, int newColumn)
        {
            // ... Ensure position is within the array bounds.
            /*
            if (newRow < 0) return false;
            if (newColumn < 0) return false;
            if (newRow >= array.Length) return false;
            if (newColumn >= array[newRow].Length) return false;
            return true;
            */
            return !(newRow < 0)
                    && !(newColumn < 0)
                    && !(newRow >= array.Length)
                    && !(newColumn >= array[newRow].Length);
        }
        
        // Make sure the position is within the maze array bounds.
        // no walls
        public bool IsValidMove(int newRow, int newColumn) => 
            IsValidPos(MazeArray, newRow, newColumn) &&
            !(MazeArray[newRow][newColumn] == -1); //no walls 

        //Marking strategy
        public bool IsValidMove(int newRow, int newColumn, bool notVisited = true)
        {
            // Make sure the position is within the maze array bounds.
            // no walls, not yet visited ? (flag notVisited: false)
            return notVisited ?
                    IsValidPos(MazeArray, newRow, newColumn) &&
                    !(MazeArray[newRow][newColumn] == -1)  //no walls, but already visited -> ok
                    :
                    IsValidPos(MazeArray, newRow, newColumn) &&
                    !(MazeArray[newRow][newColumn] == -1 || MazeArray[newRow][newColumn] == 4); //no walls, not yet visited 
        }
        
    }

    public static class MazeGrids
    {
      public static string mazeText = @"
xxxxxx1xxxxxxxxxxxxxxxxxxxxxxx.
 x   x   x                    .
xx2x xxx   x xxxxxxxx    x xx .
x  x xxxxxxx xxxxxxxxxxxxx xxx.
 x x xx      x                .
x  x xx xxxxx  x xxxx xxxxx  x.
xx    x xxx   xx xxx  xxx   xx.
xxx   xxx   x xxxx   xx   x xx.
xx     xx   x xxxx   xx   x xx.
xxxx    xxxxx xx xxxx xxxxx xx.
xx            xx            xx.";
    }
}
