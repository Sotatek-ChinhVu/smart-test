namespace Helper.Mapping.Attributes
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class EntityIgnoreMappingAttribute : Attribute
    {
    }
}
