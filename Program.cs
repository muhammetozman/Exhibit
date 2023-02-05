using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Exhibit
{
    class Program
    {
        static void Main(string[] args)
        {
            List<ExhibitModel> models = new List<ExhibitModel>();

            using (var reader = new StreamReader(@"C:\Users\hymprduser1\Desktop\exhibitA-input.csv"))
            {
                reader.ReadLine(); // to seperate header
                while (!reader.EndOfStream)
                {

                    var line = reader.ReadLine();

                    string sep = "\t";
                    var values = line.Split(sep.ToCharArray());

                    models.Add(new ExhibitModel
                    {
                        PLAY_ID = values[0],
                        SONG_ID = Convert.ToInt32(values[1]),
                        CLIENT_ID = Convert.ToInt32(values[2]),
                        PLAY_TS = Convert.ToDateTime(values[3])
                    });
                }

            }

            var disticntExhhibit = models.Where(x => x.PLAY_TS.ToShortDateString() == "2016-10-08").Select(m => new { m.CLIENT_ID, m.SONG_ID }).Distinct().ToList();

            var result = disticntExhhibit.GroupBy(x => new { x.CLIENT_ID }).Select(gcs => new
            {
                ClientId = gcs.Key.CLIENT_ID,
                TotalCount = gcs.Count()
            }).GroupBy(x => new { x.TotalCount }).Select(gcs => new
            {
                DistinctPlayCount = gcs.Key.TotalCount,
                ClientCount = gcs.Count()
            }).OrderBy(x=>x.DistinctPlayCount).ToList();


            //to write a csv file for output

            var filepath = @"C:\Users\hymprduser1\Desktop\exhibitA-output.csv";
            using (StreamWriter writer = new StreamWriter(new FileStream(filepath,
            FileMode.Create, FileAccess.Write)))
            {
                
                writer.WriteLine("DISTINCT_PLAY_COUNT,CLIENT_COUNT");
                foreach (var item in result)
                {
                    writer.WriteLine($"{item.DistinctPlayCount},{item.ClientCount}");
                }
            }

        }
    }
}