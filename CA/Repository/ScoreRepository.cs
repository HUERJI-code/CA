using CA.Models;
using CA.Services;
using Microsoft.EntityFrameworkCore;

namespace CA.Repository
{
    public class ScoreRepository
    {

        public MyDbContext _context;
        public ScoreRepository(MyDbContext myDbContext)
        {
            _context = myDbContext;
        }

        public Score GetScoreById(String scoreId)
        {
            Score score = _context.Score.FirstOrDefault(s => s.Id == scoreId);
            if (score == null)
            {
                return null;
            }
            else
            {
                return score;
            }
        }

        public List<Score> GetScoreByUserId(string userId)
        {
            var scores = _context.Score
                .Include(s => s.User)
                .Where(s => s.UserId == userId)
                .ToList();

            foreach (var score in scores)
            {
                var _ = score.User?.Username;
            }

            return scores;
        }



        public String AddScore(String userId, String usertime)
        {
            User user = _context.User.FirstOrDefault(u => u.Id == userId);
            if (user == null)
            {
                return "User not found";
            }
            else
            {
                Score score = new Score(usertime);
                score.User = user;
                _context.Score.Add(score);
                _context.SaveChanges();
                return "Add Success";
            }

        }

        public List<Score> GetTop5FastestScores()
        {
            return _context.Score
                .Include(s => s.User)
                .OrderBy(score => score.UserTime)     
                .Take(5)                               
                .ToList();                             
        }

    }
}
