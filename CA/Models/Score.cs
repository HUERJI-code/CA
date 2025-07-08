using System.ComponentModel.DataAnnotations.Schema;

namespace CA.Models
{
    public class Score
    {
        public Score() { }
        public Score(int userTime) 
        { 
            Id = System.Guid.NewGuid().ToString();
            UserTime = userTime;
        }

        public String Id { get; set; }

        public int UserTime { get; set; }

        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }
    }
}
