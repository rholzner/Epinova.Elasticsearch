using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Epinova.ElasticSearch.Core.EPiServer.Controllers.Abstractions;
using EPiServer.DataAbstraction;
using Microsoft.Azure.Search;

namespace Epinova.ElasticSearch.Core.EPiServer.Controllers
{
    public class ElasticAdminController : ElasticSearchControllerBase
    {
        private readonly ILanguageBranchRepository _languageBranchRepository;
        private static SearchServiceClient _serviceClient;


        public ElasticAdminController(ILanguageBranchRepository languageBranchRepository)
        {
            _languageBranchRepository = languageBranchRepository;
            _serviceClient = CreateSearchServiceClient();
        }

        [Authorize(Roles = "ElasticsearchAdmins")]
        public ActionResult Index()
        {
            return View("~/Views/ElasticSearchAdmin/Admin/Index.cshtml", _serviceClient.Indexes.List().Indexes);
        }

        private IEnumerable<string> GetIndexNames()
        {
            //ElasticSearchSection config = ElasticSearchSection.GetConfiguration();

            IEnumerable<string> languages = _languageBranchRepository
                .ListEnabled()
                .Select(lang => lang.LanguageID);

            var result = new List<string>();

            foreach (string lang in languages)
            {
                var indexName = CreateIndexName("test", lang);
                result.Add(indexName);
            }
            return result;
        }

        private static string CreateIndexName(string index, string language)
        {
            if (String.IsNullOrWhiteSpace(index))
                throw new InvalidOperationException("Index must be specified");
            if (String.IsNullOrWhiteSpace(language))
                throw new InvalidOperationException("Language must be specified");

            return $"{index}-{language}".ToLower();
        }

        [Authorize(Roles = "ElasticsearchAdmins")]
        public ActionResult AddNewIndex()
        {
            //if (Core.Server.Info.Version.Major < 5)
            //    throw new Exception("Elasticsearch version 5 or higher required");



            return RedirectToAction("Index");
        }

        private SearchServiceClient CreateSearchServiceClient()
        {
            //TODO: change to use not hardcoded value
            string searchServiceName = "SearchServiceName";
            string adminApiKey = "SearchServiceAdminApiKey";

            SearchServiceClient serviceClient = new SearchServiceClient(searchServiceName, new SearchCredentials(adminApiKey));
            return serviceClient;
        }
    }
}
