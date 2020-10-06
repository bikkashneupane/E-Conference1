using EConference.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EConference.Models
{
    public static class PaperSorter
    {
        // the number of groups the time zones are divided into (3 groups = 8 hour periods)
        private const int timeZoneGroups = 3;
        // the maximum number of papers in one group
        private const int papersPerGroup = 5;
        // the minimum number of papers it will try to put in one group
        private const int minPapersPerGroup = 4;

        public static List<List<Papers>> GroupPapers(List<Papers> papers)
        {
            Random r = new Random();
            papers.OrderBy(p => r.Next());

            List<List<Papers>> timeGroups = new List<List<Papers>>();

            TimeSpan timeZoneStart = TimeSpan.FromHours(-12);
            TimeSpan timePeriod = TimeSpan.FromHours(24).Divide(timeZoneGroups);

            for (int i = 0; i < timeZoneGroups; i++)
            {
                timeGroups.Add(new List<Papers>());
            }
            foreach (Papers p in papers)
            {
                TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById(p.TimeZone);
                TimeSpan ts = tzi.BaseUtcOffset;

                for (int i = 0; i < timeZoneGroups; i++)
                {
                    if (ts.TotalMinutes <= timeZoneStart.Add(timePeriod.Multiply(i + 1)).TotalMinutes || ts.TotalMinutes > TimeSpan.FromHours(12).TotalMinutes)
                    {
                        timeGroups[i].Add(p);
                        break;
                    }
                }
            }

            List<Dictionary<int, List<Papers>>> topicGroups = new List<Dictionary<int, List<Papers>>>();

            for (int i = 0; i < timeZoneGroups; i++)
            {
                topicGroups.Add(new Dictionary<int, List<Papers>>());
                foreach (Papers p in timeGroups[i])
                {
                    int key = PaperTopic.GetCategory(p.PaperTopic).ID;
                    if (!topicGroups[i].ContainsKey(key))
                        topicGroups[i].Add(key, new List<Papers>());
                    topicGroups[i][key].Add(p);
                }
            }

            List<List<List<Papers>>> finalGroups = new List<List<List<Papers>>>();

            for (int i = 0; i < timeZoneGroups; i++)
            {
                finalGroups.Add(new List<List<Papers>>());
                int j = 0;
                foreach (List<Papers> topicGroup in topicGroups[i].Values)
                {
                    finalGroups[i].Add(new List<Papers>());
                    foreach (Papers p in topicGroup)
                    {
                        if (finalGroups[i][j].Count >= papersPerGroup)
                        {
                            j++;
                            finalGroups[i].Add(new List<Papers>());
                        }
                        finalGroups[i][j].Add(p);
                    }

                    List<List<Papers>> groupsToRemove = new List<List<Papers>>();
                    foreach (List<Papers> fg in finalGroups[i])
                    {
                        if (fg.Count < minPapersPerGroup)
                        {
                            groupsToRemove.Add(fg);
                        }
                    }

                    foreach (List<Papers> gtr in groupsToRemove) { finalGroups[i].Remove(gtr); }

                    foreach (Papers p in finalGroups[i].SelectMany(p => p)) { topicGroup.Remove(p); }
                }
                if (finalGroups[i].Count == 0)
                    finalGroups[i].Add(new List<Papers>());

                int k = 0;
                foreach (Papers p in topicGroups[i].Values.SelectMany(l => l).OrderBy(p => r.Next()))
                {
                    while (finalGroups[i][k].Count >= papersPerGroup)
                    {
                        k++;
                        finalGroups[i].Add(new List<Papers>());
                    }
                    finalGroups[i][k].Add(p);
                }
            }

            return finalGroups.SelectMany(fg => fg).Where(fg => fg.Count > 0).ToList();
        }

        // I was considering a tree structure for holding the papers.
        // I probably won't use it anymore

        // paper tree node class
        // nodes have List<List<Paper>>, stores all those papers
        // 
        // e.g. 3 nodes at the base, 3 periods of time zones

        //private class PaperTreeNode
        //{
        //    public List<Paper> Papers { get; set; } = new List<Paper>();
        //    public List<PaperTreeNode> Children { get; set; } = new List<PaperTreeNode>();
        //    public PaperTreeNode Parent { get; set; }
        //    public string Label { get; set; }
        //}
    }
}
