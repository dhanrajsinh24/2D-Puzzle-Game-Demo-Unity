using System;

namespace IG.Controller 
{
    /// <summary>
    /// Manages the scoring based on the number of moves taken to complete a level.
    /// </summary>
    public class ScoreManager
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="minMoves">Minimum number of moves for a perfect score</param>
        /// <param name="maxMoves">Maximum number of moves after which the score will be 1</param>
        /// <param name="playerMoves">Number of moves taken by the player</param>
        /// <returns></returns>
        public int CalculateScore(int minMoves, int maxMoves, int playerMoves) 
        {
            if (playerMoves <= minMoves)
            {
                // Perfect score for completing in the minimum number of moves
                return 100;
            }
            
            if (playerMoves >= maxMoves)
            {
                // Minimum score for completing in the maximum number of moves or more
                return 1;
            }

            // Linear interpolation of the score between 1 and 100
            float t = (float)(playerMoves - minMoves) / (maxMoves - minMoves);
            return (int)Math.Round(100 - (99 * t));
        }
    }
}
