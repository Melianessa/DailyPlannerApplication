using System.Collections.Generic;
using System.Linq;
using DailyPlanner.Helpers;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace DailyPlanner.Web.Filters
{
    public class ApplyIgnoreRelationshipsInNamespace<TNsType>: ISchemaFilter
    {
        public void Apply(Schema model, SchemaFilterContext context)
        {
            if (model.Properties == null)
                return;
            var excludeList = new List<string>();

            if (context.SystemType.Namespace == typeof(TNsType).Namespace)
            {
                excludeList.AddRange(
                    from prop in context.SystemType.GetProperties()
                    where prop.PropertyType.Namespace == typeof(TNsType).Namespace
                    select prop.Name.ToCamelCase());
            }

            foreach (var prop in excludeList)
            {
                if (model.Properties.ContainsKey(prop))
                    model.Properties.Remove(prop);
            }
        }
    }
}
