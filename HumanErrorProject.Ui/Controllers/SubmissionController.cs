using System.Collections.Generic;
using Hangfire;
using HumanErrorProject.Data.Dtos;
using HumanErrorProject.Ui.Services;
using Microsoft.AspNetCore.Mvc;

namespace HumanErrorProject.Ui.Controllers
{
    [Route("api/Submission")]
    public class SubmissionController : Controller
    {
        public IEngineService EngineService;
        public SubmissionController(IEngineService engineService)
        {
            EngineService = engineService;
        }

        [HttpPost]
        public IActionResult Post([FromBody] StudentSubmissionDto submission)
        {
            if (submission.SnapshotFolder != null && submission.SnapshotFolder.Length > 0)
            {
                BackgroundJob.Enqueue(() => EngineService.RunSubmission(submission));
                return Ok();
            }
            return BadRequest();
        }
    }
}
