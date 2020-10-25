using MongoDB.Bson;
using MongoDB.Driver;
using Oden.Mongo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ResumeScrape
{
    static partial class Program
    {
        private static List<string> GetSearchTerms_Alphabet(int skipSearchTerms)
        {
            List<string> SearchTerms = new List<string>();

            const string alphabet = "abcdefghijklmnopqrstuvwxyz";

            foreach (char d in alphabet)
                SearchTerms.Add(d.ToString());

            foreach (char c in alphabet)
                foreach (char d in alphabet)
                    SearchTerms.Add(c.ToString() + d.ToString());

            if (skipSearchTerms > 0)
                SearchTerms.RemoveRange(0, skipSearchTerms);

            return SearchTerms;
        }

        private static List<string> GetSearchTerms_FirstName(int skipSearchTerms)
        {
            MongoClient dbClient = new MongoClient(Connection.LOCAL);
            IMongoDatabase database = dbClient.GetDatabase(DB.TALENT);
            var raw_collection = database.GetCollection<BsonDocument>(Collection.RESUME);

            var unique_names = raw_collection.Distinct<string>("FirstName", FilterDefinition<BsonDocument>.Empty).ToList();
            var searchTerms = new List<string>();

            foreach (string name in unique_names)
                if (name.Length > 2 && !name.Contains(".") && !name.Contains("?") && !name.Any(char.IsDigit) && !name.Contains("$") && !name.Contains(";"))
                    searchTerms.Add(name);

            if (skipSearchTerms > 0)
                searchTerms.RemoveRange(0, skipSearchTerms);

            return searchTerms;
        }
    }
}
