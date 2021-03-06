﻿using System.Collections.Generic;
using System.Linq;
using Epinova.ElasticSearch.Core.Contracts;
using Epinova.ElasticSearch.Core.EPiServer.Contracts;
using Epinova.ElasticSearch.Core.EPiServer.Plugin;
using Epinova.ElasticSearch.Core.Settings;
using EPiServer;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.PlugIn;
using Mediachase.Commerce.Catalog;

namespace Epinova.ElasticSearch.Core.EPiServer.Commerce.Plugin
{
    [ScheduledPlugIn(
        SortIndex = 100001,
        DisplayName = "Elasticsearch: Index Commerce content",
        Description = "Indexes Episerver Commerce content in Elasticsearch.")]
    public class IndexEpiserverCommerceContent : IndexEPiServerContent
    {
        private readonly IContentLoader _contentLoader;
        private readonly IElasticSearchSettings _settings;
        private readonly ReferenceConverter _referenceConverter;

        public IndexEpiserverCommerceContent(
            IContentLoader contentLoader,
            ICoreIndexer coreIndexer,
            IIndexer indexer,
            ILanguageBranchRepository languageBranchRepository,
            IElasticSearchSettings settings,
            ReferenceConverter referenceConverter)
            : base(contentLoader, coreIndexer, indexer, languageBranchRepository, settings)
        {
            _contentLoader = contentLoader;
            _referenceConverter = referenceConverter;
            _settings = settings;
            CustomIndexName = $"{_settings.Index}-{Core.Constants.CommerceProviderName}";
        }

        protected override List<ContentReference> GetContentReferences()
        {
            OnStatusChanged("Loading all references from database...");
            return _contentLoader.GetDescendents(_referenceConverter.GetRootLink()).ToList();
        }
    }
}