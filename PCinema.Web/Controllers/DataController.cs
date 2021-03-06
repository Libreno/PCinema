﻿using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PCinema.Web.Controllers
{
	[Route("api/[controller]/{action}")]
	[ApiController]
	public class DataController : ControllerBase
	{
		private readonly ILogger<DataController> logger;
		private readonly SearchRepository repo;

		public DataController(ILogger<DataController> logger, SearchRepository repo)
		{
			this.logger = logger;
			this.repo = repo;
		}

		// POST api/<DataController>/addentry
		[HttpPost]
		public IActionResult AddEntry([ModelBinder(Name = "fullText")] Person entry)
		{
			try
			{
				logger.LogInformation("AddEntry " + entry.FullText);
				var res = repo.AddEntry(entry);
				switch (res.Result)
				{
					case Nest.Result.Created:
						return new CreatedResult(res.Id, new { Result = "Created", Id = res.Id });
					case Nest.Result.Error:
						return new StatusCodeResult(500);
					case Nest.Result.NotFound:
						return new NotFoundResult();
					default:
						return new StatusCodeResult(520);
				}
			}
			catch (Exception e)
			{
				return new ContentResult()
				{
					StatusCode = 520,
					Content = e.Message,
					ContentType = "text/plain"
				};
			}
		}

		// GET api/<DataController>/search
		[HttpGet()]
		public IActionResult Search([FromQuery] string query)
		{
			try
			{
				logger.LogInformation("Search '" + query + "'");
				if (string.IsNullOrWhiteSpace(query)) {
					return new OkObjectResult(new List<ResultItem>());
				};
				var res = repo.Search(query);
				return new OkObjectResult(res);
			}
			catch (Exception e)
			{
				return new ContentResult() { 
					StatusCode = 520,
					Content = e.Message,
					ContentType = "text/plain"
				};
			}
		}
	}
}
