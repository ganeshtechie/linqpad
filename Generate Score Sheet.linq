<Query Kind="Program" />

void Main()
{
	string assessmentXml = "<Wizard><Title>History</Title><Description>About World War II</Description><FeedbackMessage>&lt;h1&gt;Thank you for participating.&lt;/h1&gt;</FeedbackMessage><MaximumRetakeLimit>0</MaximumRetakeLimit><Feedback><Grades><Grade><Name>Fail</Name><IsDefault>true</IsDefault><Message>Sorry, You've failed! Try next time.</Message></Grade><Grade><Name>Pass</Name><IsDefault>false</IsDefault><Score>10</Score><Message>Congratulations. You've passed.</Message></Grade></Grades></Feedback><StagingMethod>one-by-one</StagingMethod><Scoring ScoringMethod=\"any\" /><Page><Section><Question ID=\"1\" Required=\"true\" RandomizeChoices=\"true\" ScoringAt=\"question\" Score=\"10\" ChoiceType=\"checkbox\"><Title>Adolf Hitler is from which country ?</Title><Choice ID=\"1\" Correct=\"false\" Score=\"0\"><Title>USA</Title><Value>USA</Value></Choice><Choice ID=\"2\" Correct=\"true\" Score=\"0\"><Title>Germany</Title><Value>Germany</Value></Choice><Choice ID=\"3\" Correct=\"false\" Score=\"0\"><Title>India</Title><Value>India</Value></Choice></Question><Question Rhetorical=\"false\" ID=\"2\" Required=\"true\" RandomizeChoices=\"true\" ScoringAt=\"question\" Score=\"10\" ChoiceType=\"radiobutton\"><Title>India is in which continent?</Title><Choice ID=\"1\" Correct=\"true\" Score=\"0\"><Title>ASIA</Title><Value>ASIA</Value></Choice><Choice ID=\"2\" Correct=\"false\" Score=\"0\"><Title>EUROPE</Title><Value>EUROPE</Value></Choice></Question></Section></Page></Wizard>";
	
	string responseXml = "<Response><Question ID=\"1\"><SelectedChoices><Choice><ID>1</ID></Choice></SelectedChoices></Question><Question ID=\"2\"><SelectedChoices><Choice><ID>1</ID></Choice></SelectedChoices></Question></Response>";
	
	
	XElement assessment = XElement.Parse(assessmentXml);
	
	XElement response = XElement.Parse(responseXml);
	
	
	// "nonRhetoricalQuestions" are the questions for which we need to calculate scores
	// Rhetorical questions are excluded from the scoring system
	var nonRhetoricalQuestions = assessment.Descendants("Question")
									.Where(question => question.Attribute("Rhetorical") == null || question.Attribute("Rhetorical").Value == "false")
									.Select(question => question);
	
	var usersResponseForAllQuestions = response.Elements("Question");
	
	// QuestionID, IsAnswered, Score
	
	XElement answerSheet = new XElement("ScoreSheet");
	
	decimal yourScore = 0.0m;
	
	foreach(var question in nonRhetoricalQuestions)
	{
	
		string questionid = question.Attribute("ID").Value;
		
		decimal score = 0.0m;
		
		bool isAnswered = false;
		
		var _response = usersResponseForAllQuestions.Where(_question => _question.Attribute("ID").Value == questionid)
							.Select(_question => _question).FirstOrDefault();
		
		isAnswered = _response != null;
		
		if(isAnswered)
		{
			score = GetScore(question.ToString(), _response.ToString());
			
			yourScore += score;
		}
		
		XElement asQuestion = new XElement("Question", new XAttribute("ID", questionid), new XAttribute("IsAnswered", isAnswered));
		
		if(isAnswered){
			asQuestion.Add(new XElement("Score", score));
		}
		
		answerSheet.Add(asQuestion);
		
	}
	
	answerSheet.Add(new XAttribute("YourScore", yourScore));
	
	Console.WriteLine(answerSheet);
	
	
	//Console.WriteLine(userSelectedChoiceIds);
	
	//rhetorical
	
}

// Define other methods and classes here
public decimal GetScore(string questionXml, string responseXml)
{
	///Console.WriteLine(questionXml); Console.WriteLine(responseXml);
	return 1.3m;
}