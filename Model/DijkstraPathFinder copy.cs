// function FindPathDijkstra(
//             maze,
//             source,
//             visitedPositions)

//     rows ← maze.MazeArray.Length
//     cols ← maze.MazeArray[0].Length

//     create Distance[rows,cols]
//     create Previous[rows,cols]

//     create VisitedNodes[maze.CountNotVisited()][]
//     visitedCount ← 0

//     // initialization
//     for row ← 0 to rows-1
//         for col ← 0 to cols-1

//             Distance[row,col] ← INFINITY
//             Previous[row,col] ← UNDEFINED

//     Distance[source[0],source[1]] ← 0

//     while visitedCount < maze.CountNotVisited()

//         min ← INFINITY
//         minPos ← UNDEFINED

//         // find closest unvisited node
//         for row ← 0 to rows-1
//             for col ← 0 to cols-1

//                 pos ← [row,col]

//                 if maze.IsValidMove(row,col)
//                     AND NOT AlreadyVisited(
//                         VisitedNodes,
//                         pos)
//                     AND Distance[row,col] < min

//                     min ← Distance[row,col]
//                     minPos ← pos

//         if minPos = UNDEFINED
//             break

//         // add to visited
//         VisitedNodes[visitedCount]
//             ← minPos

//         visitedCount++

//         visitedPositions.Enqueue(minPos)

//         currentRow ← minPos[0]
//         currentCol ← minPos[1]

//         // stop when end reached
//         if currentRow = maze.End[0]
//            AND currentCol = maze.End[1]

//             break

//         // process neighbors
//         for each move in maze.moves

//             newRow ← currentRow + move[0]
//             newCol ← currentCol + move[1]

//             neighbor ← [newRow,newCol]

//             if maze.IsValidMove(
//                 newRow,newCol)

//                 AND NOT AlreadyVisited(
//                     VisitedNodes,
//                     neighbor)

//                 alt ←
//                 Distance[currentRow,currentCol]
//                 + 1

//                 if alt <
//                    Distance[newRow,newCol]

//                     Distance[newRow,newCol]
//                         ← alt

//                     Previous[newRow,newCol]
//                         ← minPos

//     return Distance, Previous