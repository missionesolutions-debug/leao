namespace Framework.Services.Mapper
{
    public static class MappingExtensions
    {
        public static TDestination MapTo<TSource, TDestination>(this TSource source)
            where TDestination : new()
        {
            var mapper = new GenericMapper<TSource, TDestination>();
            return mapper.Map(source);
        }
    }

}
