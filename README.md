# Test Maker

Test Maker is a web application that allows users to create and administer multiple-choice tests. It is built using the ASP.NET MVC framework and jQuery, and stores test data in a SQL Server database.

## Features

- Create multiple-choice tests with any number of questions and choices
- Edit or delete existing tests
- Take tests and receive a score based on the number of correct answers
- View test results and scores for all previous tests

## Installation

1. Clone the repository to your local machine using `git clone https://github.com/sinbaddoraji/QuizMaker.git`.
2. Open the solution file (`TestMaker.sln`) in Visual Studio.
3. Modify the database connection string in the `Web.config` file to point to your local SQL Server instance.
4. Build and run the application in Visual Studio.

## Usage

To create a new test, click the "New Test" button on the home page and fill out the test name, description, questions, and choices. To edit or delete an existing test, navigate to the "Tests" page and click the corresponding button.

To take a test, click the "Take Test" button on the home page and select the desired test from the dropdown menu. Answer each question by selecting the radio button corresponding to the desired choice, then click the "Submit" button to receive your score.

To view test results and scores, navigate to the "Test Results" page. You can view scores for individual tests or for all tests in the database.

## Screenshots

