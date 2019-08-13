using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HumanErrorProject.Data.DataAccess;
using HumanErrorProject.Data.Models;
using HumanErrorProject.Ui.Constants;
using HumanErrorProject.Ui.DataVisuals;
using HumanErrorProject.Ui.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace HumanErrorProject.Ui.Pages.Analysis.Markov
{
    [Authorize(Roles = IdentityRoleConstants.Admin)]
    public class DetailsModel : PageModel
    {
        public HumanErrorProjectContext Context;
        public DbSet<MarkovModel> MarkovModels;
        public DbSet<Snapshot> Snapshots;
        public ColorHelper ColorHelper;

        public DetailsModel(HumanErrorProjectContext context, ColorHelper colorHelper)
        {
            Context = context;
            MarkovModels = context.Set<MarkovModel>();
            Snapshots = context.Set<Snapshot>();
            ColorHelper = colorHelper;
        }

        [FromRoute]
        public int Id { get; set; }

        public MarkovModel MarkovModel { get; set; }

        public GraphChart GraphChart { get; set; }

        public string ReturnUrl { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            MarkovModel = await MarkovModels.FindAsync(Id);

            if (MarkovModel == null) return NotFound();

            Context.Entry(MarkovModel).Reference(x => x.Assignment)
                .Query().Include(x => x.CourseClass)
                .Load();

            Context.Entry(MarkovModel).Collection(x => x.States)
                .Query().Include(x => x.Snapshots)
                .Include(x => x.Transitions).Load();

            GraphChart = GetGraphChart();

            ReturnUrl = Url.Page("/Analysis/Markov/Details", new
            {
                Id,
            });

            return Page();
        }

        public GraphChart GetGraphChart()
        {
            if (!MarkovModel.Finished) return null;

            var nodes = MarkovModel.States.Select(s => new GraphChartNode()
            {
                Id = s.Number,
                Color = DataVisualConstants.PrimaryColor,
                Radius = 20 + s.Snapshots.Count * 2,
                Url = $"#{s.AnchorTag}",
                Html = $"<p><strong>State {s.Number}</strong></p>" +
                       s.Transitions.Select(t => $"<p>To State {t.To} - {t.Probability * 100:F0}%</p>").Join("")
            }).ToList();

            var links = MarkovModel.States.Select(s =>
                s.Transitions.Select(t => new GraphCartLink()
                {
                    Color = DataVisualConstants.DarkColor,
                    Source = s.Number,
                    Target = t.To,
                })).SelectMany(x => x).ToList();

            return new GraphChart()
            {
                Id = "overall_graph",
                GraphCartLinks = links,
                GraphChartNodes = nodes
            };
        }

    }
}