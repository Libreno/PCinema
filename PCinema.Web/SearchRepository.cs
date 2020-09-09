using Elasticsearch.Net;
using Microsoft.Extensions.Logging;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PCinema.Web
{
	public class SearchRepository
	{
		private readonly ElasticClient client;
		private readonly ILogger<SearchRepository> logger;

		public SearchRepository(string url, string username, string passw, ILogger<SearchRepository> logger)
		{
			var settings = new ConnectionSettings(new Uri(url)).DefaultIndex("pcinema").BasicAuthentication(username, passw);
			client = new ElasticClient(settings);
			this.logger = logger;
		}

		public IndexResponse AddEntry(Person entry)
		{
			var indexResponse = client.IndexDocument(entry);
			logger.LogInformation("Add entry result: " + indexResponse.Result);
			return indexResponse;
		}

		public IEnumerable<ResultItem> Search(string query)
		{
			var res = client.Search<Person>(s =>
				s.Query(q =>
					q.Match(m =>
						m.Field(f =>
							f.FullText)
						.Query(query)))
				.Highlight(h =>
					h.HighlightQuery(q => q
						.Match(m => m
							.Field(f =>
							f.FullText)
						.Query(query))
					)
					.Fields(
						fs => fs
							.Field(f => f.FullText)
						)
					));

			logger.LogInformation("Search result: " + JsonSerializer.Serialize(res.Documents));
			var list = new List<ResultItem>();
			var hits = res.Hits.ToArray();
			var i = 0;
			foreach (var doc in res.Documents)
			{
				list.Add(new ResultItem() { FullText = doc.FullText, Highlights = hits[i].Highlight.Single().Value.ToArray() });
				i++;
			};
			return list;
		}
	}
}
