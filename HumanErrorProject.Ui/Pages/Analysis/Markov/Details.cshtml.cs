using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HumanErrorProject.Data.DataAccess;
using HumanErrorProject.Data.Models;
using HumanErrorProject.Ui.Constants;
using HumanErrorProject.Ui.DataVisuals;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace HumanErrorProject.Ui.Pages.Analysis.Markov
{
    [Authorize(Roles = IdentityRoleConstants.Admin)]
    public class DetailsModel : PageModel
    {
        public HumanErrorProjectContext Context;
        public DbSet<MarkovModel> MarkovModels;

        public DetailsModel(HumanErrorProjectContext context)
        {
            Context = context;
            MarkovModels = context.Set<MarkovModel>();
        }

        [FromRoute]
        public int Id { get; set; }

        public MarkovModel MarkovModel { get; set; }

        public GraphChart GraphChart { get; set; }

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

            return Page();
        }

        public GraphChart GetGraphChart()
        {
            if (!MarkovModel.Finished) return null;

            return new GraphChart()
            {
                Id = "overall_graph",
                GraphChartNodes = new List<GraphChartNode>()
                {
                    new GraphChartNode() { Id = 1, Name = "A", Color = DataVisualConstants.PassedColor },
                    new GraphChartNode() { Id = 2, Name = "B", Color = DataVisualConstants.PassedColor },
                    new GraphChartNode() { Id = 3, Name = "C", Color = DataVisualConstants.PassedColor },
                    new GraphChartNode() { Id = 4, Name = "D", Color = DataVisualConstants.PassedColor },
                    new GraphChartNode() { Id = 5, Name = "E", Color = DataVisualConstants.PassedColor },
                    new GraphChartNode() { Id = 6, Name = "F", Color = DataVisualConstants.PassedColor },
                    new GraphChartNode() { Id = 7, Name = "G", Color = DataVisualConstants.PassedColor },
                    new GraphChartNode() { Id = 8, Name = "I", Color = DataVisualConstants.PassedColor },
                    new GraphChartNode() { Id = 9, Name = "J", Color = DataVisualConstants.PassedColor },
                },
                GraphCartLinks = new List<GraphCartLink>()
                {
                    new GraphCartLink() { Source = 1, Target = 2, Color = DataVisualConstants.PrimaryColor },
                    new GraphCartLink() { Source = 1, Target = 5, Color = DataVisualConstants.PrimaryColor },
                    new GraphCartLink() { Source = 1, Target = 6, Color = DataVisualConstants.PrimaryColor },
                    new GraphCartLink() { Source = 2, Target = 3, Color = DataVisualConstants.PrimaryColor },
                    new GraphCartLink() { Source = 2, Target = 7, Color = DataVisualConstants.PrimaryColor },
                    new GraphCartLink() { Source = 3, Target = 4, Color = DataVisualConstants.PrimaryColor },
                    new GraphCartLink() { Source = 8, Target = 3, Color = DataVisualConstants.PrimaryColor },
                    new GraphCartLink() { Source = 4, Target = 5, Color = DataVisualConstants.PrimaryColor },
                    new GraphCartLink() { Source = 5, Target = 9, Color = DataVisualConstants.PrimaryColor },
                }
            };

            throw new NotImplementedException();
        }

    }
}