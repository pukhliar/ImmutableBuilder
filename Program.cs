using System;
using System.Collections.Generic;
using System.Linq;

namespace ImmutableBuilder
{
    class Program
    {
        static void Main(string[] args)
        {
            var builder = new SingleDestinationBuilder();
            builder = builder.WithDestination("dest");

            var src1 = builder.WithSource("src1").WithDestination("dest2");

        }

        static IReadOnlyDictionary<string, string> BuildSingleDestination()
        {
            var singleDestinationBuilder = new SingleDestinationBuilder();

            return singleDestinationBuilder
               .WithSource("source 1")
               .WithDestination("destination")
               .Build();
        }

        static IReadOnlyDictionary<string, string> BuildMultipleDestination()
        {
            var multipleDestinationBuilder = new MultipleDestinationBuilder();

            return multipleDestinationBuilder
                .WithDestinations("destination 1", "destination 2")
                .WithSource("source")
                .Build();
        }
    }

    public class SingleDestinationBuilder : BuilderBase<SingleDestinationBuilder>
    {
        private string _destination;

        public SingleDestinationBuilder WithDestination(string destination)
        {
            _destination = destination;
            return this;
        }

        protected override IEnumerable<KeyValuePair<string, string>> EnumerateEntries()
        {
            var baseEntries = base.EnumerateEntries();
            foreach (var baseEntry in baseEntries)
            {
                yield return baseEntry;
            }

            yield return KeyValuePair.Create("Destination", _destination);
        }
    }

    public class MultipleDestinationBuilder : BuilderBase<MultipleDestinationBuilder>
    {
        private IReadOnlyCollection<string> _destinations;

        public MultipleDestinationBuilder WithDestinations(params string[] destinations)
        {
            _destinations = destinations;
            return this;
        }

        protected override IEnumerable<KeyValuePair<string, string>> EnumerateEntries()
        {
            var baseEntries = base.EnumerateEntries();
            foreach (var baseEntry in baseEntries)
            {
                yield return baseEntry;
            }

            yield return KeyValuePair.Create("Destinations", string.Join(";", _destinations));
        }
    }

    public abstract class BuilderBase<T> where T : BuilderBase<T>
    {
        private string _source;

        public T WithSource(string source)
        {
            var builder = (T)this.MemberwiseClone();
            builder._source = source;
            return builder;
        }

        protected virtual IEnumerable<KeyValuePair<string, string>> EnumerateEntries()
        {
            yield return KeyValuePair.Create("Source", _source);
        }

        public IReadOnlyDictionary<string, string> Build()
        {
            var entries = EnumerateEntries();
            return new Dictionary<string, string>(entries);
        }

    }



}