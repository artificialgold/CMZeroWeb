using System;
using System.Linq;

namespace CMZero.Web.Models.ViewModels
{
    public class BaseLabelsViewModel
    {
        public LabelCollection Labels { get; set; }

        public string GetLabel(string labelName)
        {
            try
            {
                var contentAreas = Labels.ContentAreas.ToList();

                return (from ca in contentAreas where ca.Name == labelName select ca.Content).First();
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }
    }
}