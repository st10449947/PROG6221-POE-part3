using System;
using System.Collections.Generic;
using System.Linq;

namespace CybersecurityChatbot
{
    internal class KeywordResponder
    {
        // MAIN RESPONSES
        private Dictionary<string, List<string>> _responses;

        // FOLLOW-UP RESPONSES
        private Dictionary<string, List<string>> _followUps;

        // RANDOM OBJECT
        private Random _random = new Random();

        // CURRENT TOPIC MEMORY
        public string CurrentTopic { get; private set; }

        // CONSTRUCTOR
        public KeywordResponder()
        {
            //  MAIN RESPONSES 
            _responses = new Dictionary<string, List<string>>()
            {
                // PASSWORDS
                {
                    "password",
                    new List<string>()
                    {
                        "Strong passwords help protect your accounts from hackers.",
                        "Use different passwords for every account.",
                        "Enable multi-factor authentication for extra security.",
                        "Avoid using names or birthdays in passwords.",
                        "Your password should be at least 12 characters with numbers and symbols.",
                        "Using a password manager helps generate and store strong passwords.",
                        "Never share your password with anyone, even if they claim to be from tech support.",
                        "Change your passwords regularly, especially after a data breach."
                    }
                },

                // PHISHING
                {
                    "phishing",
                    new List<string>()
                    {
                        "Phishing attacks try to steal sensitive information.",
                        "Always verify suspicious emails before clicking links.",
                        "Phishing scams often pretend to be trusted companies.",
                        "Never enter passwords into suspicious websites.",
                        "Look for suspicious sender addresses in emails.",
                        "Never click on links in unsolicited emails.",
                        "Phishing emails often contain spelling and grammar mistakes.",
                        "If something seems too urgent or too good to be true, it's probably a phishing attempt."
                    }
                },

                // PRIVACY
                {
                    "privacy",
                    new List<string>()
                    {
                        "Protecting your online privacy is very important.",
                        "Avoid sharing personal information publicly.",
                        "Review app permissions carefully.",
                        "Use privacy settings on social media.",
                        "Be cautious about what you post online - it stays forever.",
                        "Use a VPN to protect your privacy on public networks.",
                        "Clear your browser cookies and cache regularly.",
                        "Consider using privacy-focused browsers like Brave or Firefox."
                    }
                },

                // SCAMS
                {
                    "scam",
                    new List<string>()
                    {
                        "Scammers often pressure victims into acting quickly.",
                        "Be cautious of offers that seem too good to be true.",
                        "Never send money to unknown people online.",
                        "Verify websites before making payments.",
                        "Scammers impersonate government agencies and well-known companies.",
                        "If you receive an unexpected call asking for money, hang up.",
                        "Research any investment opportunity thoroughly before committing.",
                        "Scammers use fear tactics to manipulate victims."
                    }
                },

                // MALWARE
                {
                    "malware",
                    new List<string>()
                    {
                        "Malware is harmful software designed to damage systems.",
                        "Avoid downloading unknown files.",
                        "Keep your antivirus software updated.",
                        "Malware can steal personal information.",
                        "Ransomware is a dangerous form of malware that locks your files.",
                        "Malware spreads through infected email attachments.",
                        "Software updates help reduce malware risks.",
                        "USB devices can also spread malware."
                    }
                },

                // VPN
                {
                    "vpn",
                    new List<string>()
                    {
                        "VPNs help protect your online privacy.",
                        "A VPN encrypts your internet connection.",
                        "VPNs are useful on public Wi-Fi.",
                        "VPNs help hide your IP address.",
                        "Businesses use VPNs for secure remote work.",
                        "VPNs improve privacy on public networks.",
                        "Free VPNs are not always secure.",
                        "VPNs protect your browsing activity from your ISP."
                    }
                },

                // SAFE BROWSING
                {
                    "safe browsing",
                    new List<string>()
                    {
                        "Always browse websites using HTTPS.",
                        "Avoid clicking suspicious advertisements.",
                        "Keep your browser updated regularly.",
                        "Avoid downloading software from unknown websites.",
                        "Fake websites can steal login information.",
                        "Never ignore browser security warnings.",
                        "Ad blockers can improve browsing safety.",
                        "HTTPS websites encrypt your information."
                    }
                },

                // TWO-FACTOR AUTHENTICATION
                {
                    "2fa",
                    new List<string>()
                    {
                        "Two-factor authentication adds an extra layer of security.",
                        "2FA requires a second form of verification like a text code.",
                        "Always enable 2FA on important accounts like email and banking.",
                        "2FA protects your account even if your password is compromised.",
                        "Use an authenticator app instead of SMS for better security.",
                        "2FA is one of the most effective ways to prevent unauthorized access.",
                        "Some services offer backup codes - store them securely.",
                        "Without 2FA, your accounts are much more vulnerable to attacks."
                    }
                },

                // DATA BACKUP
                {
                    "backup",
                    new List<string>()
                    {
                        "Regular backups protect your data from loss.",
                        "Use the 3-2-1 backup rule: 3 copies, 2 different media, 1 offsite.",
                        "Cloud backups are convenient but ensure they're encrypted.",
                        "Test your backups regularly to ensure they work.",
                        "Backups protect against ransomware attacks.",
                        "Important documents should be backed up immediately.",
                        "Use automatic backup solutions to ensure consistency.",
                        "Don't forget to backup your phone data too."
                    }
                },

                // SOCIAL ENGINEERING
                {
                    "social engineering",
                    new List<string>()
                    {
                        "Social engineering attacks manipulate people, not computers.",
                        "Attackers use psychological tricks to get information.",
                        "Be suspicious of unsolicited calls asking for personal details.",
                        "Verify the identity of anyone asking for sensitive information.",
                        "Social engineering often involves impersonation and trust building.",
                        "If something feels off, trust your instincts.",
                        "Attackers exploit human nature to bypass security.",
                        "Always verify requests through official channels."
                    }
                },

                // TASK-RELATED KEYWORDS
                {
                    "add task",
                    new List<string>()
                    {
                        "I'll help you add that task. What would you like to call it?",
                        "Task addition initiated. Please provide a title.",
                        "Ready to add a new task. What's the task name?",
                        "Let's create a new task for you.",
                        "I can help you organize that task. What should it be called?"
                    }
                },
                {
                    "remind me",
                    new List<string>()
                    {
                        "I'll set a reminder for you. What would you like to be reminded about?",
                        "Reminder system activated. What task should I remind you about?",
                        "Let me help you set that reminder.",
                        "I can remind you about that. What's the task?",
                        "Reminder ready. What should I remind you to do?"
                    }
                },
                {
                    "quiz",
                    new List<string>()
                    {
                        "I can start a cybersecurity quiz for you. Type 'start quiz' to begin!",
                        "Ready to test your cybersecurity knowledge? Type 'start quiz'!",
                        "The quiz is ready whenever you are! Say 'start quiz' to begin.",
                        "Want to challenge yourself? Type 'start quiz' for a cybersecurity test!",
                        "Let's test your skills! Type 'start quiz' to begin."
                    }
                },
                {
                    "activity log",
                    new List<string>()
                    {
                        "I'll show you the activity log. Type 'show activity log' to see recent actions.",
                        "Your activity history is ready. Say 'show activity log' to view it.",
                        "All your activities are tracked. Type 'show activity log' to see them.",
                        "I can show you what I've done. Type 'show activity log'.",
                        "Your log is ready. Say 'show activity log' to see recent actions."
                    }
                }
            };

            //  FOLLOW-UP RESPONSES 
            _followUps = new Dictionary<string, List<string>>()
            {
                // PASSWORDS
                {
                    "password",
                    new List<string>()
                    {
                        "A strong password should include symbols, numbers, and uppercase letters.",
                        "Password managers can generate secure passwords.",
                        "Changing passwords regularly improves security.",
                        "Avoid reusing passwords across multiple websites.",
                        "Consider using a passphrase - a sentence that's easy to remember.",
                        "Enable 2FA wherever possible for extra protection.",
                        "Don't use personal information like birthdays in passwords.",
                        "Your password should be unique for every important account."
                    }
                },

                // PHISHING
                {
                    "phishing",
                    new List<string>()
                    {
                        "Hover over links before clicking them.",
                        "Phishing can happen through SMS messages too.",
                        "Fake banking emails are common phishing attacks.",
                        "Urgent language is a common phishing tactic.",
                        "Always check the sender's email address carefully.",
                        "Look for signs of urgency or threats in the message.",
                        "Don't click on attachments from unknown senders.",
                        "When in doubt, contact the company directly through their official website."
                    }
                },

                // PRIVACY
                {
                    "privacy",
                    new List<string>()
                    {
                        "Public Wi-Fi can expose your personal data.",
                        "Limit how much personal information you share online.",
                        "Always log out from shared computers.",
                        "Use strong privacy settings on your accounts.",
                        "Review your social media privacy settings regularly.",
                        "Use different email addresses for different purposes.",
                        "Consider using a privacy-focused search engine.",
                        "Your digital footprint stays forever - think before you post."
                    }
                },

                // SCAMS
                {
                    "scam",
                    new List<string>()
                    {
                        "Online shopping scams are becoming more common.",
                        "Scammers sometimes impersonate technical support.",
                        "Always double-check website URLs.",
                        "Do not trust random investment opportunities.",
                        "Scammers often target the elderly and vulnerable.",
                        "Report scams to the appropriate authorities.",
                        "Don't rush - scammers create false urgency.",
                        "Verify any unsolicited offers through official channels."
                    }
                },

                // MALWARE
                {
                    "malware",
                    new List<string>()
                    {
                        "Ransomware is a dangerous form of malware.",
                        "Malware spreads through infected email attachments.",
                        "Software updates help reduce malware risks.",
                        "USB devices can also spread malware.",
                        "Use a reputable antivirus solution.",
                        "Be cautious of 'free' software downloads.",
                        "Malware can hide in seemingly legitimate files.",
                        "Regular system scans help detect malware early."
                    }
                },

                // VPN
                {
                    "vpn",
                    new List<string>()
                    {
                        "Businesses use VPNs for secure remote work.",
                        "VPNs improve privacy on public networks.",
                        "Free VPNs are not always secure.",
                        "VPNs protect your browsing activity.",
                        "VPNs encrypt all data transmitted over the internet.",
                        "Choose a VPN with a no-logs policy.",
                        "VPNs can help bypass regional content restrictions.",
                        "Not all VPNs are created equal - research before choosing one."
                    }
                },

                // SAFE BROWSING
                {
                    "safe browsing",
                    new List<string>()
                    {
                        "Fake websites can steal login information.",
                        "Never ignore browser security warnings.",
                        "Ad blockers can improve browsing safety.",
                        "HTTPS websites encrypt your information.",
                        "Check for the padlock icon in the address bar.",
                        "Clear your browser history and cookies regularly.",
                        "Use incognito mode for sensitive browsing.",
                        "Be cautious of pop-ups asking for personal information."
                    }
                },

                // TWO-FACTOR AUTHENTICATION
                {
                    "2fa",
                    new List<string>()
                    {
                        "App-based 2FA is more secure than SMS.",
                        "Keep backup codes in a safe place.",
                        "2FA significantly reduces account compromise risk.",
                        "Some services allow hardware keys for 2FA.",
                        "Biometric authentication is also a form of 2FA.",
                        "2FA should be enabled on all important accounts.",
                        "Don't share your 2FA codes with anyone.",
                        "If you get an unexpected 2FA request, your password may be compromised."
                    }
                },

                // DATA BACKUP
                {
                    "backup",
                    new List<string>()
                    {
                        "Encrypt your backups for better security.",
                        "Backup automation helps ensure consistency.",
                        "Consider backing up to multiple locations.",
                        "Test your backup restoration process regularly.",
                        "Cloud backups are convenient but ensure data privacy.",
                        "Important files should have multiple backup versions.",
                        "External hard drives are good for local backups.",
                        "Don't wait until it's too late to start backing up."
                    }
                },

                // SOCIAL ENGINEERING
                {
                    "social engineering",
                    new List<string>()
                    {
                        "Be skeptical of unsolicited contact.",
                        "Verify identities through official channels.",
                        "Don't share sensitive information over the phone.",
                        "Watch for emotional manipulation tactics.",
                        "Trust your instincts - if something feels wrong, it probably is.",
                        "Social engineers often research their targets beforehand.",
                        "Always ask for identification when dealing with strangers.",
                        "Educate family members about social engineering tactics."
                    }
                }
            };
        }

        //  MAIN RESPONSE
        public string GetResponse(string input)
        {
            input = input.ToLower();

            // SEARCH FOR KEYWORDS
            foreach (var keyword in _responses.Keys)
            {
                if (input.Contains(keyword))
                {
                    // SAVE CURRENT TOPIC
                    CurrentTopic = keyword;

                    List<string> responses = _responses[keyword];

                    int index = _random.Next(responses.Count);

                    return responses[index];
                }
            }

            return "I am not sure about that. Try asking about passwords, phishing, scams, malware, privacy, VPNs, safe browsing, 2FA, backups, or social engineering.";
        }

        // FOLLOW-UP RESPONSE 
        public string GetFollowUpResponse()
        {
            if (string.IsNullOrEmpty(CurrentTopic))
            {
                return "Please ask about a cybersecurity topic first.";
            }

            if (_followUps.ContainsKey(CurrentTopic))
            {
                List<string> details = _followUps[CurrentTopic];
                int index = _random.Next(details.Count);
                return details[index];
            }

            return "I can provide more information on that topic. What specifically would you like to know?";
        }

        //  GET ALL TOPICS 
        public List<string> GetAllKeywords()
        {
            return _responses.Keys.ToList();
        }

        // GET SPECIFIC RESPONSES FOR INTENT DETECTION
        public List<string> GetResponsesForKeyword(string keyword)
        {
            keyword = keyword.ToLower();
            if (_responses.ContainsKey(keyword))
            {
                return _responses[keyword];
            }
            return new List<string>();
        }

        // CHECK IF INPUT MATCHES ANY TASK INTENT
        public bool IsTaskIntent(string input)
        {
            input = input.ToLower();
            string[] taskPhrases = {
                "add task", "add a task", "create task", "new task",
                "create a task", "make a task", "add new task",
                "i need to", "i have to", "i should", "i must"
            };

            foreach (string phrase in taskPhrases)
            {
                if (input.Contains(phrase))
                    return true;
            }
            return false;
        }

        // CHECK IF INPUT MATCHES ANY REMINDER INTENT
        public bool IsReminderIntent(string input)
        {
            input = input.ToLower();
            string[] reminderPhrases = {
                "remind me", "set a reminder", "reminder",
                "don't forget", "remember to", "remind me to",
                "set reminder", "create reminder", "make a reminder"
            };

            foreach (string phrase in reminderPhrases)
            {
                if (input.Contains(phrase))
                    return true;
            }
            return false;
        }

        // CHECK IF INPUT MATCHES ANY QUIZ INTENT
        public bool IsQuizIntent(string input)
        {
            input = input.ToLower();
            string[] quizPhrases = {
                "start quiz", "take quiz", "test my knowledge",
                "quiz me", "play the game", "begin quiz",
                "do quiz", "take a quiz", "start the quiz"
            };

            foreach (string phrase in quizPhrases)
            {
                if (input.Contains(phrase))
                    return true;
            }
            return false;
        }

        // CHECK IF INPUT MATCHES ANY LOG INTENT
        public bool IsLogIntent(string input)
        {
            input = input.ToLower();
            string[] logPhrases = {
                "show activity log", "what have you done",
                "what did you do", "show log", "recent actions",
                "show me the log", "view activities", "what have you been doing",
                "activity log", "show history", "what have you done for me"
            };

            foreach (string phrase in logPhrases)
            {
                if (input.Contains(phrase))
                    return true;
            }
            return false;
        }

        // CHECK IF INPUT MATCHES SHOW MORE INTENT
        public bool IsShowMoreIntent(string input)
        {
            input = input.ToLower();
            string[] showMorePhrases = {
                "show more", "view all", "full log",
                "see all", "show everything", "all entries",
                "more entries", "see full log", "complete log"
            };

            foreach (string phrase in showMorePhrases)
            {
                if (input.Contains(phrase))
                    return true;
            }
            return false;
        }
    }
}