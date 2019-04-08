using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Radio.Core.Domain.MasterData.Model;

namespace Radio.Core.Domain.Voting.Model
{
    public class VotingCandidate : EntityBase
    {
        public VotingCandidate()
        {
            IsActive = true;
            Votes = new Collection<Vote>();
        }

        public Guid SongId { get; set; }

        public virtual Song Song { get; set; }

        public int DisplayOrder { get; set; }

        public bool IsActive { get; set; }

        public virtual ICollection<Vote> Votes { get; set; }

        public void Map(Song song, int displayOrder)
        {
            SongId = song.Id;
            Song = song;
            DisplayOrder = displayOrder;
        }
    }
}
