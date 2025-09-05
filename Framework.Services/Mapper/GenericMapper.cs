using System.Linq;

namespace Framework.Services.Mapper
{
    public class GenericMapper<TSource, TDestination>
      where TDestination : new()
    {
        public TDestination Map(TSource source)
        {
            var destination = new TDestination();
            var sourceProps = typeof(TSource).GetProperties();
            var destinationProps = typeof(TDestination).GetProperties();

            foreach (var sourceProp in sourceProps)
            {
                var destinationProp = destinationProps.FirstOrDefault(p => p.Name == sourceProp.Name && p.CanWrite);

                if (destinationProp != null)
                {
                    var value = sourceProp.GetValue(source);
                    if (value != null)
                    {
                        var convertedValue = Convert.ChangeType(value, Nullable.GetUnderlyingType(destinationProp.PropertyType) ?? destinationProp.PropertyType);
                        destinationProp.SetValue(destination, convertedValue);
                    }
                }
            }

            return destination;
        }
    }

}
