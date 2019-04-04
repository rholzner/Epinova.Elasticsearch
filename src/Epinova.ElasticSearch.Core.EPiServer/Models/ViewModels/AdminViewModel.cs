using System.Collections.Generic;

namespace Epinova.ElasticSearch.Core.EPiServer.Models.ViewModels
{
    public class AdminViewModel
    {
        public AdminViewModel(IEnumerable<Microsoft.Azure.Search.Models.Index> allIndexes)
        {
            AllIndexes = allIndexes;
        }

        public IEnumerable<Microsoft.Azure.Search.Models.Index> AllIndexes { get; }
    }
}