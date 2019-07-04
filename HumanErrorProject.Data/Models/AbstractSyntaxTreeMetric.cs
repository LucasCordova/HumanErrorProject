using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HumanErrorProject.Data.Models
{
    public class AbstractSyntaxTreeMetric : IdentityModel<int>
    {
        [Required, DisplayName("Rotations")]
        public int Rotations { get; set; } = 0;

        [Required, DisplayName("Insertations")]
        public int Insertations { get; set; } = 0;

        [Required, DisplayName("Deletions")]
        public int Deletions { get; set; } = 0;

        public double Distance()
        {
            return Math.Pow(Rotations * Rotations + Insertations * Insertations + Deletions * Deletions, 0.5);
        }

        public override bool Equals(object obj)
        {
            var metric = obj as AbstractSyntaxTreeMetric;
            return metric != null &&
                   Rotations == metric.Rotations &&
                   Insertations == metric.Insertations &&
                   Deletions == metric.Deletions;
        }

        protected bool Equals(AbstractSyntaxTreeMetric other)
        {
            return Rotations == other.Rotations && Insertations == other.Insertations && Deletions == other.Deletions;
        }

        public static bool operator ==(AbstractSyntaxTreeMetric metric1, AbstractSyntaxTreeMetric metric2)
        {
            return EqualityComparer<AbstractSyntaxTreeMetric>.Default.Equals(metric1, metric2);
        }

        public static bool operator !=(AbstractSyntaxTreeMetric metric1, AbstractSyntaxTreeMetric metric2)
        {
            return !(metric1 == metric2);
        }
    }
}
