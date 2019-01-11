using Epinova.ElasticSearch.Core.Enums;

namespace Epinova.ElasticSearch.Core.Models.Query
{
    internal class MatchBestBet : MatchWithBoost
    {
        private const int BestBetMultiplier = 10000; //TODO: Expose in config?

        public MatchBestBet(string query, Operator @operator) : base(DefaultFields.BestBets, query, BestBetMultiplier, @operator)
        {
        }
    }
}
