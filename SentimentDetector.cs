using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CybersecurityChatbot
{
    public enum Sentiment { Neutral, Worried, Curious, Frustrated, Happy }
    internal class SentimentDetector
    {
        private Dictionary<Sentiment, List<string>> sentimentKeywords;

        public SentimentDetector()
        { 
            sentimentKeywords = new Dictionary<Sentiment, List<string>>()
            { 
                { 
                    Sentiment.Worried, new List<string>()
                    {
                        "worried",
                        "scared",
                        "afraid",
                        "nervous",
                        "unsafe", 
                        "concerned",
                        "anxious"
                    }
                },
                
                { 
                    Sentiment.Curious, new List<string>()
                    
                    {
                        "curious",
                        "interested", 
                        "want to know", 
                        "how does",
                        "why", 
                        "learn"
                    }
                }, 
                
                {
                    Sentiment.Frustrated, new List<string>() 

                    {
                        "angry",
                        "frustrated", 
                        "annoyed",
                        "confused",
                        "upset",
                        "mad",
                        "irritated"
                    } 
                }, 
                
                { 
                    Sentiment.Happy, new List<string>() 
                    {
                        "happy",
                        "good",
                        "great",
                        "awesome",
                        "excited",
                        "fine", "fantastic"
                    } 
                }
            };
        }
        
        //  DETECT SENTIMENT 
          public Sentiment Detect(string input) 
        { 
            input = input.ToLower(); 

            foreach (var entry in sentimentKeywords)
            {
                foreach (string word in entry.Value)
                {
                    if (input.Contains(word))
                    {
                        return entry.Key; 
                    
                    }
                }
            } return Sentiment.Neutral;
        }
        // EMPATHETIC RESPONSE 
        public string GetSentimentResponse(Sentiment sentiment)
        {
            
            switch (sentiment)
            {
                case Sentiment.Worried: 
                    return "I understand your concern. Cybersecurity can feel overwhelming sometimes.";
                case Sentiment.Curious: 
                    return "That’s great! Staying curious helps you learn how to stay safe online."; 
                case Sentiment.Frustrated:
                    return "I understand this can be frustrating, but I’ll do my best to help you.";
                case Sentiment.Happy:
                    return "I’m glad to hear that! Staying positive is always good."; 
                default: 
                    return "Thanks for sharing.";
             
            } 
        }
    }
}
