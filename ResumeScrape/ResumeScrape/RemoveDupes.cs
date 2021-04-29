using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using Oden.Mongo;
using Oden.Talent;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ResumeScrape
{
    static partial class Program
    {
        private static void RemoveAllDuplicates()
        {
            var watch = new System.Diagnostics.Stopwatch();
            watch.Start();

            MongoClient dbClient = new MongoClient(Connection.LOCAL);
            IMongoDatabase database = dbClient.GetDatabase(DB.TALENT);
            var raw_collection = database.GetCollection<BsonDocument>(Collection.RESUME);

            const string UniqueFieldToQuery = "FirstName";

            var unique_names = raw_collection.Distinct<string>(UniqueFieldToQuery, FilterDefinition<BsonDocument>.Empty).ToList();

            int complete_counter = 0;
            int delete_counter = 0;

            Oden.ConsoleIO.printTimeStatus(watch.Elapsed, "Setup Complete: ");

            foreach (string firstName in unique_names)
            {
                var jdsToDelete = new List<ObjectId>();

                var filter2 = Builders<BsonDocument>.Filter.Eq(UniqueFieldToQuery, firstName);
                var rawData = raw_collection.Find(filter2).ToList();

                if (rawData.Count > 1)
                {
                    List<Talent> rawTalent = new List<Talent>();

                    foreach (var a in rawData)
                    {
                        rawTalent.Add(BsonSerializer.Deserialize<Talent>(a));
                    }

                    //for each job description, check if there are any duplicates
                    for (int i = 0; i < rawTalent.Count - 1; i++)
                    {
                        for (int j = i + 1; j < rawTalent.Count; j++)
                        {
                            if (rawTalent[i].Url.Equals(rawTalent[j].Url))
                            {
                                // if i is newer/more recent, delete j so swap the items
                                if (DateTime.Compare(rawTalent[i].ID.CreationTime, rawTalent[j].ID.CreationTime) > 0)
                                {
                                    Console.WriteLine($"Swap {rawTalent[i].ToString()} // and // {rawTalent[j].ToString()}");
                                    //swap the two JDs
                                    var tmp = rawTalent[i];
                                    rawTalent[i] = rawTalent[j];
                                    rawTalent[j] = tmp;
                                }

                                // delete the first one
                                jdsToDelete.Add(rawTalent[i].ID);
                                break; //break out of this loop because we've handled the [i] JD
                            }
                        }
                    }

                    if (jdsToDelete.Count > 0)
                    {
                        delete_counter += jdsToDelete.Count;
                        var filter3 = Builders<BsonDocument>.Filter.In("_id", jdsToDelete);
                        raw_collection.DeleteMany(filter3);
                    }
                }

                //update the console
                complete_counter++;

                Oden.ConsoleIO.printTimeStatus(watch.Elapsed, Math.Round(complete_counter * 100.0 / unique_names.Count, 1).ToString() + "%, " + delete_counter.ToString() + " Deleted, " + complete_counter.ToString() + "/" + unique_names.Count.ToString() + " " + firstName + " Complete in ");
            }
        }
    }
}
