namespace VaporStore.Data.Models
{
    public class GameTag
    {
        public virtual int GameId { get; set; } // – integer, Primary Key, foreign key(required)
        public virtual int TagId { get; set; } //– integer, Primary Key, foreign key(required)
        public virtual Game Game { get; set; } // – ExportGame
        public virtual Tag Tag { get; set; } // – Tag
    }
}