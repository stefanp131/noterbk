using Noter.DAL.Context;
using Noter.DAL.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Noter.DAL.Extensions
{
    public static class NoterContextExtensions
    {
        public static void EnsureData(this NoterContext context)
        {
            if (context.Topics.Any())
            {
                return;
            }
            var topics = new List<Topic>
            {
                new Topic
                {
                    Title = "Personal Page",
                    Description = "This topic is about me",
                    Commentaries = new List<Commentary>
                    {
                        new Commentary
                        {
                            Title = "I know how to prepare guacamole",
                            Content = "First you prepare the blyat.. hahaha cyka blyat. I am just kidding.",
                            Approval = 3
                        },
                                                new Commentary
                        {
                            Title = "Hihihi",
                            Content = "you are such a moron, you are such an idiot",
                            Approval = 1
                        }
                    }
                },
                new Topic
                {
                    Title = "How to prepare cake?",
                    Description = "This topic is about how to prepare cake fast and in a proper manner.",
                    Commentaries = new List<Commentary>
                    {
                        new Commentary
                        {
                            Title = "I know how to prepare cake instead",
                            Content = "Boooring",
                            Approval = 5
                        },
                                                new Commentary
                        {
                            Title = "Hahaha",
                            Content = "you are such a moron twoo.. tooo.. get it ????",
                            Approval = 4
                        }
                    }
                }
            };

            context.Topics.AddRange(topics);
            context.SaveChanges(); ;
        }
    }
}
