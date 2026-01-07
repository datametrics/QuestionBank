# FRM Question Bank

A short description of what this project does and why it exists.

## 🚀 Features
- Clear and simple structure
- Easy to extend
- Ready for development and deployment

## 📦 Requirements
- .NET 9 (or your version)
- EF Core (SQLLite)
- MudBlazor

## 🛠️ Setup

Clone the repository:

```bash
git clone https://github.com/yourname/yourrepo.git
cd yourrepo




prompt in GPT:

use this pdf and create 20 questions in json format like:

 [
   {
     "QuestionBody": "What is the correct answer to this question?",
     "Answer1": "Option A",
     "Answer2": "Option B",
     "Answer3": "Option C",
     "Answer4": "Option D",
     "Explanation": "This is the reason why B is true",
     "CorrectAnswerId": 2
   },
   {
     "QuestionBody": "What is the correct answer to this second question?",
     "Answer1": "Option A",
     "Answer2": "Option B",
     "Answer3": "Option C",
     "Answer4": "Option D",
     "Explanation": "This is the reason why A is true",
     "CorrectAnswerId": 1
   },
   {
    "QuestionBody": "What is the correct answer to this third question?",
    "Answer1": "Option A",
    "Answer2": "Option B",
    "Answer3": "Option C",
    "Answer4": "Option D",
    "Explanation": "This is the reason why C is true",
    "CorrectAnswerId": 3
   },
   {
    "QuestionBody": "What is the correct answer to this forth question?",
    "Answer1": "Option A",
    "Answer2": "Option B",
    "Answer3": "Option C",
    "Answer4": "Option D",
    "Explanation": "This is the reason why D is true",
    "CorrectAnswerId": 4
   }
 ] 
with:
-almost balanced answerids, 
-3 sentences context in questionbody
-good explanation with at least two sentences
-explanations in "explanation" why the other options are false
-with mixed-difficulty sets
-add line break between the answer explanations eg: Answer 2 is correct because.... <br> Answer 1 is false because...
-use 1,2,3,4 as answer ids
-----------------
