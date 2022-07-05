using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    internal class CustomAttribute
    {
        public sealed class DefaultValueAttribute : Attribute
        {
            public object DefaultValue { get; set; }

            public DefaultValueAttribute(object defaultValue)
            {
                DefaultValue = defaultValue;
            }
        }

        public sealed class DefaultValueSqlAttribute : Attribute
        {
            public string DefaultValue { get; set; }

            public DefaultValueSqlAttribute(string defaultValue)
            {
                DefaultValue = defaultValue;
            }
        }

        //public class DefaultValueAttributeConvention : PrimitivePropertyAttributeConfigurationConvention<DefaultValueAttribute>
        //{
        //    public override void Apply(ConventionPrimitivePropertyConfiguration configuration, DefaultValueAttribute attribute)
        //    {
        //        configuration.HasColumnAnnotation("DefaultValue", attribute.DefaultValue);
        //    }
        //}

        //public class DefaultValueSqlAttributeConvention : PrimitivePropertyAttributeConfigurationConvention<DefaultValueSqlAttribute>
        //{
        //    public override void Apply(ConventionPrimitivePropertyConfiguration configuration, DefaultValueSqlAttribute attribute)
        //    {
        //        configuration.HasColumnAnnotation("DefaultValueSql", attribute.DefaultValue);
        //    }
        //}
    }
}
