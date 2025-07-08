using CA.Models;
using CA.Repository;
using Microsoft.AspNetCore.Mvc;

namespace CA.Controllers
{
    public class ScoreController : Controller
    {
        private ScoreRepository _scoreRepository;
        private UserRepository _userRepository;

        public ScoreController(ScoreRepository scoreRepository, UserRepository userRepository)
        {
            _scoreRepository = scoreRepository;
            _userRepository = userRepository;
        }

        [HttpPost("/Score/AddScore")]
        public String AddScore([FromBody]AddScore addScore)
        {
            Console.WriteLine(addScore.UserName);
            User user = _userRepository.GetUserByName(addScore.UserName);
            if (user == null)
            {
                return "User not found";
            }
            return _scoreRepository.AddScore(user.Id, int.Parse(addScore.Usertime));
        }

        [HttpGet("/Score/GetScoreByUserId")]
        public List<Score> GetScoreByUserId(String userId)
        {
            List<Score> scores = _scoreRepository.GetScoreByUserId(userId);
            if (scores== null)
            {
                return null;
            }
            else
            {
                return scores;
            }
        }

        [HttpGet("/Score/Ranking")]
        public List<Ranking> rankings()
        {
            List<Ranking> rankings = new List<Ranking>();
            var scores = _scoreRepository.GetTop5FastestScores();
            foreach (var score in scores)
            {
                rankings.Add(new Ranking
                {
                    username = score.User.Username,
                    //username = "1",
                    usertime = score.UserTime.ToString()
                });
            }
            return rankings;
        }

    }
}
