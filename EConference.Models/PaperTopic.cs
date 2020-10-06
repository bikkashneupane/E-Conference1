using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EConference.Models
{
    /* PaperTopic class represents the topic of the paper.
     * Add new categories in the static List<PaperTopic> attribute.
     * Store the ID in the Paper class.
     * 
     * To allow the user to select a topic, display the List and use the ID.
     */
    public class PaperTopic
    {
        public int ID { get; }
        public int? ParentID { get; }
        public string Name { get; }

        public static List<PaperTopic> TopicList { get; } = new List<PaperTopic>()
        {
            new PaperTopic(0, null, "Physics"),
            new PaperTopic(1, null, "Computer Science"),
            new PaperTopic(2, null, "Biology"),
            new PaperTopic(3, null, "Psychology"),
            new PaperTopic(4, 1, "Blockchain"),
            new PaperTopic(5, 1, "IoT")
        };

        private PaperTopic(int ID, int? ParentID, string Name)
        {
            this.ID = ID;
            this.ParentID = ParentID;
            this.Name = Name;
        }

        // returns the paper topic with the given ID
        public static PaperTopic Get(int? ID) => TopicList.Where(p => ID == p.ID).First();

        // returns the parent, or the topic itself if it is null (is a parent)
        public static PaperTopic GetCategory(int ID)
        {
            PaperTopic topic = Get(ID);
            return topic.ParentID == null ? topic : Get(topic.ParentID);
        }
        public PaperTopic GetCategory() => ParentID == null ? this : Get(ParentID);

        public override string ToString()
        {
            return Name;
        }
    }
}
