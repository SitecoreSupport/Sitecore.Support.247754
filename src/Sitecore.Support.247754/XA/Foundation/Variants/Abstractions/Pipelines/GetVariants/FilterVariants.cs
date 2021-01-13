using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.XA.Foundation.SitecoreExtensions.Extensions;
using Sitecore.XA.Foundation.SitecoreExtensions.Repositories;
using System.Linq;

namespace Sitecore.Support.XA.Foundation.Variants.Abstractions.Pipelines.GetVariants
{
    public class FilterVariants : Sitecore.XA.Foundation.Variants.Abstractions.Pipelines.GetVariants.FilterVariants
    {
        public FilterVariants(IContentRepository contentRepository) :base(contentRepository)
        {

        }

        protected override bool AllowedInTemplate(Item item, string pageTemplateId)
        {
            var field = item.Fields[Sitecore.XA.Foundation.Variants.Abstractions.Templates.IVariantDefinition.Fields.AllowedInTemplates];
            if (field != null)
            {
                if (field.Value == string.Empty && InheritsFromAllowedTemplate(pageTemplateId))
                {
                    return true;
                }
                return field.Value.Contains(pageTemplateId) || InheritsFromSelectedTemplates(field, pageTemplateId);
            }
            return true;
        }

        protected virtual bool InheritsFromSelectedTemplates(MultilistField field, string pageTemplateId)
        {
            var templateItem = ContentRepository.GetTemplate(new ID(pageTemplateId));
            return field.TargetIDs.Any(id => templateItem.DoesTemplateInheritFrom(id));
        }
    }
}