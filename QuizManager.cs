using System;
using System.Collections.Generic;

namespace CybersecurityChatbot
{
    public class QuizQuestion
    {
        public string Question { get; set; }
        public List<string> Options { get; set; }
        public string CorrectAnswer { get; set; }
        public string Explanation { get; set; }
        public bool IsTrueFalse { get; set; }
    }

    public class QuizManager
    {
        private List<QuizQuestion> _questions;
        private int _currentIndex = 0;
        private int _score = 0;
        private ActivityLogger _logger;

        public QuizManager(ActivityLogger logger)
        {
            _logger = logger;
            _questions = InitializeQuestions();
        }

        private List<QuizQuestion> InitializeQuestions()
        {
            return new List<QuizQuestion>
            {
                // Phishing Questions
                new QuizQuestion
                {
                    Question = "What should you do if you receive an email asking for your password?",
                    Options = new List<string> { "A) Reply with your password", "B) Delete the email", "C) Report the email as phishing", "D) Ignore it" },
                    CorrectAnswer = "C",
                    Explanation = "Reporting phishing emails helps prevent scams and protects others.",
                    IsTrueFalse = false
                },
                new QuizQuestion
                {
                    Question = "Phishing attacks can only happen through email.",
                    Options = new List<string> { "True", "False" },
                    CorrectAnswer = "False",
                    Explanation = "Phishing can happen through SMS, phone calls, social media, and websites too.",
                    IsTrueFalse = true
                },
                new QuizQuestion
                {
                    Question = "What is a common sign of a phishing email?",
                    Options = new List<string> { "A) Correct spelling and grammar", "B) Urgent language demanding immediate action", "C) A known sender's email address", "D) Professional design" },
                    CorrectAnswer = "B",
                    Explanation = "Phishing emails often use urgent language to pressure victims into acting quickly without thinking.",
                    IsTrueFalse = false
                },

                // Password Safety
                new QuizQuestion
                {
                    Question = "Using the same password for multiple accounts is safe.",
                    Options = new List<string> { "True", "False" },
                    CorrectAnswer = "False",
                    Explanation = "If one account is compromised, all accounts with the same password are at risk.",
                    IsTrueFalse = true
                },
                new QuizQuestion
                {
                    Question = "What makes a strong password?",
                    Options = new List<string> { "A) Your birthday", "B) A common word", "C) A mix of uppercase, lowercase, numbers, and symbols", "D) Your pet's name" },
                    CorrectAnswer = "C",
                    Explanation = "Strong passwords use a combination of character types and are at least 12 characters long.",
                    IsTrueFalse = false
                },
                new QuizQuestion
                {
                    Question = "Two-factor authentication (2FA) adds an extra layer of security.",
                    Options = new List<string> { "True", "False" },
                    CorrectAnswer = "True",
                    Explanation = "2FA requires a second form of verification, making it much harder for attackers to access your accounts.",
                    IsTrueFalse = true
                },

                // Safe Browsing
                new QuizQuestion
                {
                    Question = "When browsing online, what does HTTPS indicate?",
                    Options = new List<string> { "A) The website is safe", "B) Your connection is encrypted", "C) The website is from a trusted source", "D) The website has no viruses" },
                    CorrectAnswer = "B",
                    Explanation = "HTTPS encrypts data between your browser and the website, protecting your information from interception.",
                    IsTrueFalse = false
                },
                new QuizQuestion
                {
                    Question = "Using public Wi-Fi without a VPN is safe for banking.",
                    Options = new List<string> { "True", "False" },
                    CorrectAnswer = "False",
                    Explanation = "Public Wi-Fi networks are unsecured and hackers can intercept your data. Always use a VPN for sensitive activities.",
                    IsTrueFalse = true
                },
                new QuizQuestion
                {
                    Question = "What should you do if a website asks for too much personal information?",
                    Options = new List<string> { "A) Provide it anyway", "B) Close the website and be cautious", "C) Share it with friends", "D) Ignore the request" },
                    CorrectAnswer = "B",
                    Explanation = "Legitimate websites only ask for necessary information. Be cautious of sites requesting excessive personal data.",
                    IsTrueFalse = false
                },

                // Social Engineering
                new QuizQuestion
                {
                    Question = "Social engineering attacks manipulate people into revealing information.",
                    Options = new List<string> { "True", "False" },
                    CorrectAnswer = "True",
                    Explanation = "Social engineering exploits human psychology rather than technical vulnerabilities to gain access to information.",
                    IsTrueFalse = true
                },
                new QuizQuestion
                {
                    Question = "What is a common social engineering tactic?",
                    Options = new List<string> { "A) Sending viruses", "B) Pretending to be technical support", "C) Breaking into servers", "D) Cracking passwords" },
                    CorrectAnswer = "B",
                    Explanation = "Attackers often impersonate trusted figures like IT support to trick people into revealing sensitive information.",
                    IsTrueFalse = false
                }
            };
        }

        public QuizQuestion GetCurrentQuestion()
        {
            if (_currentIndex < _questions.Count)
                return _questions[_currentIndex];
            return null;
        }

        public bool SubmitAnswer(string answer)
        {
            QuizQuestion current = GetCurrentQuestion();
            if (current == null)
                return false;

            bool isCorrect = answer.ToUpper() == current.CorrectAnswer.ToUpper();
            if (isCorrect)
                _score++;

            _currentIndex++;
            return isCorrect;
        }

        public string GetFeedback(bool correct)
        {
            QuizQuestion current = _questions[_currentIndex - 1];
            if (correct)
                return $"✅ Correct! {current.Explanation}";
            else
                return $"❌ Incorrect. The correct answer was {current.CorrectAnswer}. {current.Explanation}";
        }

        public bool IsFinished()
        {
            return _currentIndex >= _questions.Count;
        }

        public string GetFinalScore()
        {
            _logger.Log($"Quiz completed - score: {_score} out of {_questions.Count}");
            return $"You scored {_score} out of {_questions.Count}!";
        }

        public string GetFinalMessage()
        {
            double percentage = (double)_score / _questions.Count * 100;
            if (percentage >= 80)
                return "🌟 Excellent! You're a cybersecurity expert!";
            else if (percentage >= 60)
                return "👍 Good job! Keep learning to improve your cybersecurity knowledge!";
            else
                return "📚 Keep studying cybersecurity topics. Practice makes perfect!";
        }

        public void ResetQuiz()
        {
            _currentIndex = 0;
            _score = 0;
            _logger.Log("Quiz started");
        }

        public int GetCurrentIndex()
        {
            return _currentIndex;
        }

        public int GetTotalQuestions()
        {
            return _questions.Count;
        }

        public int GetScore()
        {
            return _score;
        }
    }
}
