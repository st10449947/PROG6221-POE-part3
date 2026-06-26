using System;
using System.IO;
using System.Media;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace CybersecurityChatbot
{
    public partial class MainWindow : Window
    {
        // Part 3 Components
        private TaskManager _taskManager;
        private QuizManager _quizManager;
        private ActivityLogger _activityLogger;
        private ChatBot _chatBot;

        private string userName = "";
        private bool _quizAnswered = false;

        // CONSTRUCTOR 
        public MainWindow()
        {
            InitializeComponent();

            // Initialize Part 3 components
            _activityLogger = new ActivityLogger();
            _taskManager = new TaskManager(_activityLogger);
            _quizManager = new QuizManager(_activityLogger);

            // Initialize ChatBot
            _chatBot = new ChatBot();

            // Log startup
            _activityLogger.Log("Chatbot started");

            PlayGreeting();
            StartChatbot();

            // Load tasks on startup
            LoadTasks();

            // Refresh log display
            RefreshLogDisplay();

            // Set up placeholder behavior for task text boxes
            SetupPlaceholderBehavior();
        }

        // Setup placeholder behavior for text boxes
        private void SetupPlaceholderBehavior()
        {
            // Task Title
            TaskTitleBox.GotFocus += (s, e) =>
            {
                if (TaskTitleBox.Text == "Enter task title...")
                {
                    TaskTitleBox.Text = "";
                    TaskTitleBox.Foreground = System.Windows.Media.Brushes.White;
                }
            };
            TaskTitleBox.LostFocus += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(TaskTitleBox.Text))
                {
                    TaskTitleBox.Text = "Enter task title...";
                    TaskTitleBox.Foreground = System.Windows.Media.Brushes.Gray;
                }
            };

            // Task Description
            TaskDescBox.GotFocus += (s, e) =>
            {
                if (TaskDescBox.Text == "Enter description...")
                {
                    TaskDescBox.Text = "";
                    TaskDescBox.Foreground = System.Windows.Media.Brushes.White;
                }
            };
            TaskDescBox.LostFocus += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(TaskDescBox.Text))
                {
                    TaskDescBox.Text = "Enter description...";
                    TaskDescBox.Foreground = System.Windows.Media.Brushes.Gray;
                }
            };

            // Task Reminder
            TaskReminderBox.GotFocus += (s, e) =>
            {
                if (TaskReminderBox.Text == "Reminder (optional)...")
                {
                    TaskReminderBox.Text = "";
                    TaskReminderBox.Foreground = System.Windows.Media.Brushes.White;
                }
            };
            TaskReminderBox.LostFocus += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(TaskReminderBox.Text))
                {
                    TaskReminderBox.Text = "Reminder (optional)...";
                    TaskReminderBox.Foreground = System.Windows.Media.Brushes.Gray;
                }
            };
        }

        // START CHATBOT 
        private void StartChatbot()
        {
            AppendBotMessage("Welcome to the Cybersecurity Bot!");
            AppendBotMessage("Please enter your name:");
        }

        // PLAY GREETING AUDIO
        private void PlayGreeting()
        {
            try
            {
                string audioPath = Path.Combine(
                    AppDomain.CurrentDomain.BaseDirectory,
                    "greeting.wav"
                );

                SoundPlayer player = new SoundPlayer(audioPath);
                player.Play();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error playing audio: " + ex.Message);
            }
        }

        // SEND MESSAGE
        private void SendMessage()
        {
            string message = UserInput.Text.Trim();

            if (string.IsNullOrWhiteSpace(message))
                return;

            AppendUserMessage(message);

            // Process through ChatBot
            string response = _chatBot.ProcessInput(message);

            AppendBotMessage(response);

            UserInput.Clear();

            ChatScroller.ScrollToEnd();

            // Refresh task list and log if needed
            if (message.ToLower().Contains("add task") ||
                message.ToLower().Contains("complete") ||
                message.ToLower().Contains("delete"))
            {
                LoadTasks();
                RefreshLogDisplay();
            }
            else if (message.ToLower().Contains("show activity log") ||
                     message.ToLower().Contains("what have you done"))
            {
                RefreshLogDisplay();
            }
        }

        // USER MESSAGE
        private void AppendUserMessage(string message)
        {
            Border bubble = new Border
            {
                Background = Brushes.White,
                CornerRadius = new CornerRadius(15),
                Padding = new Thickness(10),
                Margin = new Thickness(10),
                HorizontalAlignment = HorizontalAlignment.Left,
                MaxWidth = 300
            };

            TextBlock text = new TextBlock
            {
                Text = message,
                Foreground = Brushes.Black,
                TextWrapping = TextWrapping.Wrap,
                FontSize = 14
            };

            bubble.Child = text;

            ChatPanel.Children.Add(bubble);

            ChatScroller.ScrollToEnd();
        }

        // BOT MESSAGE 
        private void AppendBotMessage(string message)
        {
            Border bubble = new Border
            {
                Background = new SolidColorBrush(
                    Color.FromRgb(37, 211, 102)),
                CornerRadius = new CornerRadius(15),
                Padding = new Thickness(10),
                Margin = new Thickness(10),
                HorizontalAlignment = HorizontalAlignment.Right,
                MaxWidth = 350
            };

            TextBlock text = new TextBlock
            {
                Text = message,
                Foreground = Brushes.White,
                TextWrapping = TextWrapping.Wrap,
                FontSize = 14,
                FontFamily = new FontFamily("Consolas")
            };

            bubble.Child = text;

            ChatPanel.Children.Add(bubble);

            ChatScroller.ScrollToEnd();
        }

        // ENTER KEY
        private void UserInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                SendMessage();
                e.Handled = true;
            }
        }

        // SEND BUTTON 
        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            SendMessage();
        }

        // PART 3 (TASK MANAGEMENT EVENT HANDLERS) 
    

        private void LoadTasks()
        {
            try
            {
                var tasks = _taskManager.GetAllTasks();
                TaskListBox.ItemsSource = null;
                TaskListBox.ItemsSource = tasks;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading tasks: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AddTaskButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Get the actual text from the text boxes
                string title = TaskTitleBox.Text.Trim();
                string description = TaskDescBox.Text.Trim();
                string reminder = TaskReminderBox.Text.Trim();

                // Check if the text is placeholder or empty
                if (title == "Enter task title..." || string.IsNullOrEmpty(title))
                {
                    MessageBox.Show("Please enter a task title.", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // If description is placeholder or empty, set to empty string
                if (description == "Enter description..." || string.IsNullOrEmpty(description))
                {
                    description = "";
                }

                // If reminder is placeholder or empty, set to empty string
                if (reminder == "Reminder (optional)..." || string.IsNullOrEmpty(reminder))
                {
                    reminder = "";
                }

                // Add the task
                string result = _taskManager.AddTask(title, description, reminder);
                AppendBotMessage(result);

                // Clear the text boxes and reset placeholders
                TaskTitleBox.Text = "Enter task title...";
                TaskTitleBox.Foreground = System.Windows.Media.Brushes.Gray;
                TaskDescBox.Text = "Enter description...";
                TaskDescBox.Foreground = System.Windows.Media.Brushes.Gray;
                TaskReminderBox.Text = "Reminder (optional)...";
                TaskReminderBox.Foreground = System.Windows.Media.Brushes.Gray;

                // Refresh the task list
                LoadTasks();
                RefreshLogDisplay();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding task: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CompleteTaskButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                CyberTask selected = TaskListBox.SelectedItem as CyberTask;
                if (selected == null)
                {
                    MessageBox.Show("Please select a task to complete.", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (selected.IsComplete)
                {
                    MessageBox.Show("This task is already complete.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                string result = _taskManager.MarkAsComplete(selected.Id);
                AppendBotMessage(result);
                LoadTasks();
                RefreshLogDisplay();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error completing task: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DeleteTaskButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                CyberTask selected = TaskListBox.SelectedItem as CyberTask;
                if (selected == null)
                {
                    MessageBox.Show("Please select a task to delete.", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (MessageBox.Show($"Delete task '{selected.Title}'?", "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    string result = _taskManager.DeleteTask(selected.Id);
                    AppendBotMessage(result);
                    LoadTasks();
                    RefreshLogDisplay();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting task: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void RefreshTasksButton_Click(object sender, RoutedEventArgs e)
        {
            LoadTasks();
            AppendBotMessage("Tasks refreshed!");
        }

        private void UserInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Optional: Auto-resize or clear placeholder text
        }

        
        // PART 3: QUIZ EVENT HANDLERS
        private void StartQuizButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _quizManager.ResetQuiz();
                _quizAnswered = false;
                QuizResultPanel.Visibility = Visibility.Collapsed;
                QuizFeedbackText.Visibility = Visibility.Collapsed;
                StartQuizButton.Visibility = Visibility.Collapsed;
                SubmitQuizButton.Visibility = Visibility.Visible;
                NextQuizButton.Visibility = Visibility.Collapsed;
                ResetQuizButton.Visibility = Visibility.Visible;

                ShowQuizQuestion();
                AppendBotMessage("🎯 Quiz started! Test your cybersecurity knowledge.");
                _activityLogger.Log("Quiz started from GUI");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error starting quiz: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ShowQuizQuestion()
        {
            var question = _quizManager.GetCurrentQuestion();
            if (question == null || _quizManager.IsFinished())
            {
                ShowQuizResults();
                return;
            }

            QuizQuestionText.Text = $"Question {_quizManager.GetCurrentIndex() + 1}/{_quizManager.GetTotalQuestions()}: {question.Question}";
            QuizScoreText.Text = $"🏆 Score: {_quizManager.GetScore()}/{_quizManager.GetCurrentIndex()}";
            QuizProgressText.Text = $"📊 Question: {_quizManager.GetCurrentIndex() + 1}/{_quizManager.GetTotalQuestions()}";
            QuizFeedbackText.Visibility = Visibility.Collapsed;
            _quizAnswered = false;
            SubmitQuizButton.Visibility = Visibility.Visible;
            NextQuizButton.Visibility = Visibility.Collapsed;
            QuizResultPanel.Visibility = Visibility.Collapsed;

            // Create option buttons
            QuizOptionsPanel.Children.Clear();

            if (question.IsTrueFalse)
            {
                CreateOptionButton("True");
                CreateOptionButton("False");
            }
            else
            {
                foreach (string option in question.Options)
                {
                    CreateOptionButton(option);
                }
            }
        }

        private void CreateOptionButton(string text)
        {
            RadioButton rb = new RadioButton
            {
                Content = text,
                Foreground = System.Windows.Media.Brushes.White,
                Margin = new Thickness(0, 5, 0, 5),
                FontSize = 14,
                GroupName = "QuizOptions",
                Background = System.Windows.Media.Brushes.Transparent,
                BorderBrush = System.Windows.Media.Brushes.Gray,
                BorderThickness = new Thickness(1),
                Padding = new Thickness(10, 5, 10, 5)
            };

            QuizOptionsPanel.Children.Add(rb);
        }

        private void SubmitQuizButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_quizAnswered)
                {
                    MessageBox.Show("You've already answered this question! Click 'Next Question' to continue.",
                                    "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                // Get selected answer
                string selectedAnswer = "";
                foreach (RadioButton rb in QuizOptionsPanel.Children)
                {
                    if (rb.IsChecked == true)
                    {
                        selectedAnswer = rb.Content.ToString();
                        break;
                    }
                }

                if (string.IsNullOrEmpty(selectedAnswer))
                {
                    MessageBox.Show("Please select an answer before submitting.",
                                    "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Submit answer
                bool isCorrect = _quizManager.SubmitAnswer(selectedAnswer);
                _quizAnswered = true;
                SubmitQuizButton.Visibility = Visibility.Collapsed;
                NextQuizButton.Visibility = Visibility.Visible;

                // Show feedback
                string feedback = _quizManager.GetFeedback(isCorrect);
                QuizFeedbackText.Text = feedback;
                QuizFeedbackText.Visibility = Visibility.Visible;
                QuizScoreText.Text = $"🏆 Score: {_quizManager.GetScore()}/{_quizManager.GetCurrentIndex()}";

                if (isCorrect)
                {
                    QuizFeedbackText.Foreground = System.Windows.Media.Brushes.LightGreen;
                }
                else
                {
                    QuizFeedbackText.Foreground = System.Windows.Media.Brushes.LightCoral;
                }

                // Log the result
                if (isCorrect)
                {
                    _activityLogger.Log($"Quiz answer correct: {selectedAnswer}");
                }
                else
                {
                    _activityLogger.Log($"Quiz answer incorrect: {selectedAnswer}");
                }

                // Check if quiz is finished
                if (_quizManager.IsFinished())
                {
                    ShowQuizResults();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error submitting answer: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void NextQuizButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_quizManager.IsFinished())
                {
                    ShowQuizResults();
                    return;
                }

                _quizAnswered = false;
                SubmitQuizButton.Visibility = Visibility.Visible;
                NextQuizButton.Visibility = Visibility.Collapsed;
                QuizFeedbackText.Visibility = Visibility.Collapsed;

                ShowQuizQuestion();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading next question: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ShowQuizResults()
        {
            string score = _quizManager.GetFinalScore();
            string message = _quizManager.GetFinalMessage();

            QuizResultPanel.Visibility = Visibility.Visible;
            QuizFinalScoreText.Text = $"🎉 {score}";
            QuizFinalMessageText.Text = message;

            SubmitQuizButton.Visibility = Visibility.Collapsed;
            NextQuizButton.Visibility = Visibility.Collapsed;
            QuizFeedbackText.Visibility = Visibility.Collapsed;
            QuizQuestionText.Text = "🎯 Quiz Complete!";
            QuizOptionsPanel.Children.Clear();

            AppendBotMessage($"Quiz completed! Score: {_quizManager.GetScore()}/{_quizManager.GetTotalQuestions()}");
            _activityLogger.Log($"Quiz completed - score: {_quizManager.GetScore()}/{_quizManager.GetTotalQuestions()}");
        }

        private void ResetQuizButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _quizManager.ResetQuiz();
                QuizResultPanel.Visibility = Visibility.Collapsed;
                StartQuizButton.Visibility = Visibility.Visible;
                SubmitQuizButton.Visibility = Visibility.Collapsed;
                NextQuizButton.Visibility = Visibility.Collapsed;
                ResetQuizButton.Visibility = Visibility.Collapsed;
                QuizFeedbackText.Visibility = Visibility.Collapsed;
                QuizQuestionText.Text = "Quiz has been reset. Press 'Start Quiz' to begin!";
                QuizOptionsPanel.Children.Clear();
                QuizScoreText.Text = "🏆 Score: 0/0";
                QuizProgressText.Text = "📊 Question: 0/0";
                AppendBotMessage("Quiz has been reset.");
                _activityLogger.Log("Quiz reset");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error resetting quiz: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

      
        // PART 3: ACTIVITY LOG EVENT HANDLERS
        private void RefreshLogButton_Click(object sender, RoutedEventArgs e)
        {
            RefreshLogDisplay();
            AppendBotMessage("Log refreshed!");
        }

        private void ShowFullLogButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string fullLog = _activityLogger.GetFullLog();
                LogDisplayText.Text = fullLog;
                ShowFullLogButton.Visibility = Visibility.Collapsed;
                AppendBotMessage("Showing full activity log.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error showing full log: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void RefreshLogDisplay()
        {
            try
            {
                string log = _activityLogger.GetRecentLog(10);
                LogDisplayText.Text = log;
                LogCountText.Text = $"Total entries: {_activityLogger.GetCount()}";

                if (_activityLogger.GetCount() > 10)
                {
                    ShowFullLogButton.Visibility = Visibility.Visible;
                }
                else
                {
                    ShowFullLogButton.Visibility = Visibility.Collapsed;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error refreshing log: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}