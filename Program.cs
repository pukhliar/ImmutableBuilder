using System;
using System.Collections.Generic;
using System.Linq;

namespace ImmutableBuilder
{
    class Program
    {
        static void Main(string[] args)
        {
            var dest1 = new SingleDestinationBuilder();
            dest1 = dest1.WithDestination("destination 1").WithSource("source 1") as SingleDestinationBuilder;

            var dest2 = dest1.WithSource("source 2") as SingleDestinationBuilder;
            //dest2.WithDestination("destination 2");

        }

        static IReadOnlyDictionary<string, string> BuildSingleDestination()
        {
            var singleDestinationBuilder = new SingleDestinationBuilder();

            return singleDestinationBuilder
               .WithDestination("destination")
               .WithSource("source 1")
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

    public class SingleDestinationBuilder : BuilderBase
    {
        //private new string _source;

        private string _destination;

        public SingleDestinationBuilder()
        {
        }

        public SingleDestinationBuilder(string source, string destination)
        {
            base.WithSource(source);
            _destination = destination;
        }

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

        public new BuilderBase WithSource(string source)
        {
            return new SingleDestinationBuilder(source, _destination);
        }
    }

    public class MultipleDestinationBuilder : BuilderBase
    {
        private IReadOnlyCollection<string> _destinations;

        public MultipleDestinationBuilder WithDestinations(params string[] destinations)
        {
            throw new NotImplementedException();
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

    public abstract class BuilderBase
    {
        private string _source;

        public BuilderBase WithSource(string source)
        {
            _source = source;
            return this;
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