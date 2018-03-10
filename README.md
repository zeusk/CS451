Project title : 
Checkers

Author: 
Shantanu Gupta, Guruansh Singh, Yuwei Zhou

Project overview: 

This the homework project for Drexel CS451. The Checkers is a game which follows the American Checkers rule and allows different users to connect to a server and play against each other. 

Project Setup and Execution:

Download and Extract CheckersHost.zip, open a terminal instance and navigate to the extracted files. Using mono or dotnet execute CheckersHost.dll.
Click on the Checker.exe to start the game. 

Notes: 

1. The current code coverage for our project is at 61%. A screenshot detailing test results is available in the project root directory.
The coverage for the class implementing the game logic, CheckerBoard, is 86%. The numeric value for coverage in  the game logic class is high, and we recognize that the the remaining 16% is mostly debugging/console output related code which is not critical to the implementation of this project. All relevant methods for applying and validating different kinds of moves, and different kinds of pieces were covered. Therefore, we have confidence in the 84% figure.
All UI test cases are run manually and passes, so we excluded the UI component on the code coverage tool. 
All Network component test cases are also run manually since the socket operations are synchronous, so we cannot listen on client and the host side simultaneously unless we mock socket functionality. This would involve testing integrations between two components, the Client and the Server, and integration testing is beyond the scope of this testing process.
Cumulatively then, the 61% and the accompanying reasoning presented above allow us the confidence to assert that our implementation follows the rules of the game and reasonably implements the requirements discussed in the Requirements document. 


2. We used git for version control in our project. In specific, we used the GitHub service.

3. We used IntelliSense which is built into Visual Studio 2017 as our Static Analysis tool. A screenshot of the result is attached to the project submission. 

4. We used dotCover, a code coverage tool provided by jetBrains, that recognizes unit tests implemented in NUnit.

5. GitHub allows for issue tracking within a set up project. A list of closed issues can be found at https://github.com/zeusk/CS451/issues?utf8=%E2%9C%93&q=is%3Aissue

6. The milestones RC 1 contain all the change requests that led to release candidate 1.0.


