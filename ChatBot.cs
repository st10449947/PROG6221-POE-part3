using System;
using System.Collections.Generic;
using System.Linq;

namespace CybersecurityChatbot
{
    internal class ChatBot
    {
        private KeywordResponder _keywordResponder;
        private SentimentDetector _sentimentDetector;
        private MemoryStore _memoryStore;
        private TaskManager _taskManager;
        private QuizManager _quizManager;
        private ActivityLogger _activityLogger;

        private bool _awaitingName = true;
        private string _lastTopic = "";
        private Random _random = new Random();

        private List<string> _fallbackResponses = new List<string>()
        {
            "I'm not sure I understood that, but I can help with cybersecurity topics.",
            "Try asking about passwords, phishing, scams, or privacy.",
            "I didn't catch that clearly — ask me about online safety topics.",
            "Could you rephrase that? I can help with cybersecurity advice.",
            "I'm here to help with cybersecurity. Try asking about passwords, phishing, or safe browsing.",
            "Not quite sure what you mean. I can help with cybersecurity topics and tasks."
        };

        // CONSTRUCTOR
        public ChatBot()
        {
            _keywordResponder = new KeywordResponder();
            _sentimentDetector = new SentimentDetector();
            _memoryStore = new MemoryStore();
            _activityLogger = new ActivityLogger(); 
            _taskManager = new TaskManager(_activityLogger);
            _quizManager = new QuizManager(_activityLogger);

            // Log startup
            _activityLogger.Log("Chatbot initialized");
        }

        // MAIN METHOD TO PROCESS USER INPUT AND GENERATE RESPONSE
        public string ProcessInput(string input)
        {
            string originalInput = input;
            input = input.ToLower().Trim();

           
            // PART 3: NLP INTENT DETECTION - CHECK FIRST
          

            // 1. CHECK FOR TASK INTENT
            if (_keywordResponder.IsTaskIntent(input))
            {
                string taskName = ExtractTaskName(originalInput);
                if (!string.IsNullOrEmpty(taskName))
                {
                    string result = _taskManager.AddTask(taskName, "", "");
                    _activityLogger.Log($"NLP recognised task intent from: '{originalInput}'");
                    return result + " Would you like to set a reminder for this task? (Say 'Yes, remind me in X days' or 'No')";
                }
                return "Please specify what task you'd like to add. For example: 'Add task - Review privacy settings'";
            }

            // 2. CHECK FOR REMINDER INTENT
            if (_keywordResponder.IsReminderIntent(input))
            {
                string reminderText = ExtractReminderText(originalInput);
                if (!string.IsNullOrEmpty(reminderText))
                {
                    _activityLogger.Log($"Reminder set: '{reminderText}'");
                    return $"✅ Reminder set for '{reminderText}'. I'll make sure to remind you!";
                }
                return "What would you like me to remind you about? For example: 'Remind me to update my password tomorrow'";
            }

            // 3. CHECK FOR QUIZ INTENT
            if (_keywordResponder.IsQuizIntent(input))
            {
                _quizManager.ResetQuiz();
                _activityLogger.Log("Quiz started from chat");
                return "🎯 Starting cybersecurity quiz! Let's test your knowledge.\n\n" + GetNextQuizQuestion();
            }

            // 4. CHECK FOR ACTIVITY LOG INTENT
            if (_keywordResponder.IsLogIntent(input))
            {
                string log = _activityLogger.GetRecentLog(10);
                if (_activityLogger.GetCount() > 10)
                {
                    log += "\n\n📋 Type 'show more' to see all entries.";
                }
                return log;
            }

            // 5. CHECK FOR SHOW MORE INTENT
            if (_keywordResponder.IsShowMoreIntent(input))
            {
                return _activityLogger.GetFullLog();
            }

            // 6. CHECK FOR QUIZ ANSWER (if quiz is active)
            if (_quizManager.GetCurrentQuestion() != null && !_quizManager.IsFinished())
            {
                // Check if input looks like an answer (A, B, C, D, True, False)
                string answer = ExtractQuizAnswer(input);
                if (!string.IsNullOrEmpty(answer))
                {
                    return ProcessQuizAnswer(answer);
                }
            }

            // PART 2: EXISTING LOGIC CONTINUES BELOW
            // USER NAME HANDLING
            if (_awaitingName)
            {
                _awaitingName = false;
                _memoryStore.Username = originalInput;
                _activityLogger.Log($"User registered: {originalInput}");
                return $"{originalInput}! I'm your Cybersecurity Assistant. How can I help you today?";
            }

            // FOLLOW-UP QUESTIONS
            if (input.Contains("tell me more") ||
                input.Contains("explain more") ||
                input.Contains("more details") ||
                input.Contains("another tip"))
            {
                if (!string.IsNullOrEmpty(_lastTopic))
                {
                    string followUp = _keywordResponder.GetFollowUpResponse();
                    _activityLogger.Log($"Follow-up provided for topic: {_lastTopic}");
                    return $"{followUp}\n\nWould you like more details about {_lastTopic}, {_memoryStore.Username}?";
                }
                return "Can you tell me what topic you'd like more information about?";
            }

            // SENTIMENT DETECTION
            Sentiment sentiment = _sentimentDetector.Detect(input);
            string sentimentIntro = "";

            if (sentiment != Sentiment.Neutral)
            {
                sentimentIntro = _sentimentDetector.GetSentimentResponse(sentiment) + "\n";
                if (sentiment == Sentiment.Worried || sentiment == Sentiment.Frustrated)
                {
                    _activityLogger.Log($"Sentiment detected: {sentiment} from user");
                }
            }

            // GREETING CHECK
            if (input.Contains("how are you") ||
                input.Contains("how are you doing") ||
                input.Contains("what's up"))
            {
                return "I'm doing great! Cybersecurity awareness is always important. How can I help you today?";
            }

            // HELP / PURPOSE CHECK
            if (input.Contains("what can you do") ||
                input.Contains("help") ||
                input.Contains("purpose") ||
                input.Contains("topics") ||
                input.Contains("what can i ask"))
            {
                string topics = "I can help you with:\n";
                topics += "• Cybersecurity topics (passwords, phishing, scams, malware, VPNs, privacy, safe browsing)\n";
                topics += "• Task management (add, complete, delete tasks)\n";
                topics += "• Cybersecurity quizzes to test your knowledge\n";
                topics += "• Activity log to see what I've done for you\n\n";
                topics += "Try typing: 'Add task - Enable 2FA' or 'Start quiz' or 'Show activity log'";
                return topics;
            }

            // KEYWORD RESPONSE
            string keywordResponse = _keywordResponder.GetResponse(input);

            // TRACK TOPIC FOR MEMORY
            bool topicFound = false;
            foreach (var keyword in _keywordResponder.GetAllKeywords())
            {
                if (input.Contains(keyword))
                {
                    _lastTopic = keyword;
                    _memoryStore.Store("last topic", keyword);
                    topicFound = true;
                    _activityLogger.Log($"Keyword matched: {keyword}");
                    break;
                }
            }

            // FALLBACK RESPONSE IF NO KEYWORDS MATCHED
            bool noKeywordMatch = keywordResponse.Contains("I am not sure about that") ||
                                  keywordResponse.Contains("not sure");

            if (noKeywordMatch)
            {
                string fallback = _fallbackResponses[_random.Next(_fallbackResponses.Count)];
                _activityLogger.Log($"Fallback response used for: '{originalInput}'");
                return fallback + "\n\nYou can also ask me to add tasks, start a quiz, or show the activity log!";
            }

            // FULL RESPONSE WITH PERSONALIZATION
            string memoryIntro = _memoryStore.GetPersonalisedOpener();

            return memoryIntro +
                   sentimentIntro +
                   keywordResponse +
                   $"\n\nFeel free to ask another question, {_memoryStore.Username}!";
        }

    
        // HELPER METHODS FOR PART 3
        private string ExtractTaskName(string input)
        {
            // Remove common task phrases
            string[] removePhrases = {
                "add task", "add a task", "create task", "new task",
                "create a task", "make a task", "add new task",
                "i need to", "i have to", "i should", "i must",
                "task", "add"
            };

            string result = input;
            foreach (string phrase in removePhrases)
            {
                result = result.Replace(phrase, "", StringComparison.OrdinalIgnoreCase);
            }

            // Remove extra punctuation and trim
            result = result.Trim(' ', '-', ':', '.', ';', ',');

            // If result is empty, try to extract from the whole input
            if (string.IsNullOrEmpty(result))
            {
                // Look for patterns like "Add task - Task Name"
                if (input.Contains("-") || input.Contains(":"))
                {
                    string[] parts = input.Split(new char[] { '-', ':' }, 2);
                    if (parts.Length > 1)
                    {
                        result = parts[1].Trim();
                    }
                }
                else
                {
                    // Take the last part after common phrases
                    foreach (string phrase in removePhrases)
                    {
                        int index = input.IndexOf(phrase, StringComparison.OrdinalIgnoreCase);
                        if (index >= 0)
                        {
                            result = input.Substring(index + phrase.Length).Trim();
                            break;
                        }
                    }
                }
            }

            return string.IsNullOrEmpty(result) ? input.Trim() : result;
        }

        private string ExtractReminderText(string input)
        {
            string[] removePhrases = {
                "remind me", "set a reminder", "reminder",
                "to", "please", "can you", "could you",
                "set reminder", "create reminder", "make a reminder"
            };

            string result = input;
            foreach (string phrase in removePhrases)
            {
                result = result.Replace(phrase, "", StringComparison.OrdinalIgnoreCase);
            }

            result = result.Trim(' ', '-', ':', '.', ';', ',');

            // Handle "remind me to X" pattern
            if (input.Contains("remind me to"))
            {
                int index = input.IndexOf("remind me to", StringComparison.OrdinalIgnoreCase);
                if (index >= 0)
                {
                    result = input.Substring(index + "remind me to".Length).Trim();
                }
            }

            return string.IsNullOrEmpty(result) ? input.Trim() : result;
        }

        private string ExtractQuizAnswer(string input)
        {
            input = input.Trim().ToUpper();

            // Check for letter answers
            if (input.Length == 1 && input[0] >= 'A' && input[0] <= 'D')
            {
                return input[0].ToString();
            }

            // Check for True/False
            if (input == "TRUE" || input == "FALSE")
            {
                return input;
            }

            // Check if it contains a letter answer
            string[] possibleAnswers = { "A", "B", "C", "D" };
            foreach (string answer in possibleAnswers)
            {
                if (input.Contains(answer + ")") || input.Contains(answer + ".") || input == answer)
                {
                    return answer;
                }
            }

            return null;
        }

        private string GetNextQuizQuestion()
        {
            var question = _quizManager.GetCurrentQuestion();
            if (question == null)
                return "No more questions!";

            string output = $"📝 Question {_quizManager.GetCurrentIndex() + 1}/{_quizManager.GetTotalQuestions()}: ";
            output += question.Question + "\n\n";

            if (question.IsTrueFalse)
            {
                output += "Options: True or False";
            }
            else
            {
                foreach (string option in question.Options)
                {
                    output += option + "\n";
                }
            }

            output += "\nType your answer (A, B, C, D, True, or False)";
            return output;
        }

        private string ProcessQuizAnswer(string answer)
        {
            bool isCorrect = _quizManager.SubmitAnswer(answer);
            string feedback = _quizManager.GetFeedback(isCorrect);

            // Log the result
            if (isCorrect)
            {
                _activityLogger.Log($"Quiz answer correct: {answer}");
            }
            else
            {
                _activityLogger.Log($"Quiz answer incorrect: {answer}");
            }

            if (_quizManager.IsFinished())
            {
                string score = _quizManager.GetFinalScore();
                string message = _quizManager.GetFinalMessage();
                return $"{feedback}\n\n{score}\n{message}\n\n🎉 Quiz complete! Type 'start quiz' to try again.";
            }

            return $"{feedback}\n\n{GetNextQuizQuestion()}";
        }

        // PUBLIC PROPERTIES FOR GUI INTEGRATION

        public TaskManager GetTaskManager()
        {
            return _taskManager;
        }

        public QuizManager GetQuizManager()
        {
            return _quizManager;
        }

        public ActivityLogger GetActivityLogger()
        {
            return _activityLogger;
        }

        public MemoryStore GetMemoryStore()
        {
            return _memoryStore;
        }

        public KeywordResponder GetKeywordResponder()
        {
            return _keywordResponder;
        }

        public void ResetQuiz()
        {
            _quizManager.ResetQuiz();
        }
    }
}
