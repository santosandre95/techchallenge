﻿namespace TechChallengeApi.Events
{
    public class BuscaIdEvent
    {
        public Guid? Id { get; }
        public BuscaIdEvent(Guid id)
        {
            this.Id = id;
        }
    }
}
