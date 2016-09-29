<Query Kind="Program" />

void Main()
{
	
	string assessmentXml = "<Wizard><Title>History</Title><Description>About World War II</Description><FeedbackMessage>&lt;h1&gt;Thank you for participating.&lt;/h1&gt;</FeedbackMessage><MaximumRetakeLimit>0</MaximumRetakeLimit><Feedback><Grades><Grade><Name>Fail</Name><IsDefault>true</IsDefault><Message>Sorry, You've failed! Try next time.</Message></Grade><Grade><Name>Pass</Name><IsDefault>false</IsDefault><Score>10</Score><Message>Congratulations. You've passed.</Message></Grade></Grades></Feedback><StagingMethod>one-by-one</StagingMethod><Scoring ScoringMethod=\"any\" /><Page><Section><Question ID=\"1\" Required=\"true\" RandomizeChoices=\"true\" ScoringAt=\"question\" Score=\"10\" ChoiceType=\"checkbox\"><Title>Adolf Hitler is from which country ?</Title><Choice ID=\"1\" Correct=\"false\" Score=\"0\"><Title>USA</Title><Value>USA</Value></Choice><Choice ID=\"2\" Correct=\"true\" Score=\"0\"><Title>Germany</Title><Value>Germany</Value></Choice><Choice ID=\"3\" Correct=\"false\" Score=\"0\"><Title>India</Title><Value>India</Value></Choice></Question><Question Rhetorical=\"false\" ID=\"2\" Required=\"true\" RandomizeChoices=\"true\" ScoringAt=\"question\" Score=\"10\" ChoiceType=\"radiobutton\"><Title>India is in which continent?</Title><Choice ID=\"1\" Correct=\"true\" Score=\"0\"><Title>ASIA</Title><Value>ASIA</Value></Choice><Choice ID=\"2\" Correct=\"false\" Score=\"0\"><Title>EUROPE</Title><Value>EUROPE</Value></Choice></Question></Section></Page></Wizard>";
	
	string responseXml = "<Response><Question ID=\"1\"><SelectedChoices><Choice><ID>1</ID></Choice></SelectedChoices></Question><Question ID=\"2\"><SelectedChoices><Choice><ID>1</ID></Choice></SelectedChoices></Question></Response>";
	
	string scoreSheetXml = "<ScoreSheet YourScore=\"2.6\"><Question ID=\"1\" IsAnswered=\"true\"><Score>1.3</Score></Question><Question ID=\"2\" IsAnswered=\"true\"><Score>1.3</Score></Question></ScoreSheet>";
	
	XElement assessment = XElement.Parse(assessmentXml);
	
	XElement response = XElement.Parse(responseXml);
	
	XElement scoreSheet = XElement.Parse(scoreSheetXml);
	
	var questions = assessment.Descendants("Question");
	
	XElement reportRootElement = new XElement("Report");
	
	if(scoreSheet != null)
		reportRootElement.Add(new XElement("YourScore", scoreSheet.Attribute("YourScore").Value));
	
	foreach(var _question in questions)
	{
		int _questionId = Convert.ToInt32(_question.Attribute("ID").Value);
		
		// Try to find the response for this question & its score, despite its availability 
		// The GenerateReportForQuestion method will put appropriate content in it and returns 
		// the proper xml. We don't need to worry about to check if the response & score is
		// present for this question or not, here in this place.
		
		var qResponseXml = response.Elements("Question")
								.Where(_rQuestion => Convert.ToInt32(_rQuestion.Attribute("ID").Value) == _questionId)
								.Select(_rQuestion => _rQuestion).FirstOrDefault();
								
		var qScoreSheetXml = scoreSheet.Elements("Question")
								.Where(_rQuestion => Convert.ToInt32(_rQuestion.Attribute("ID").Value) == _questionId)
								.Select(_rQuestion => _rQuestion).FirstOrDefault();
								
								
		string questionXml = _question.ToString();
		
		string _responseXml = string.Empty; string _scoreSheetXml = string.Empty;
		
		if(qResponseXml != null) _responseXml = qResponseXml.ToString();
		
		if(qScoreSheetXml != null) _scoreSheetXml = qScoreSheetXml.ToString();
		
		string reportXml = GetReportForQuestion(questionXml, _responseXml, _scoreSheetXml);
		
		XElement reportElement = XElement.Parse(reportXml);
		
		
		reportRootElement.Add(reportElement);
		
	}
	
	Console.WriteLine(reportRootElement);
	
	
	
}

// Define other methods and classes here

public string GetReportForQuestion(string questionXml, string responseXml, string scoreSheetXml = "")
{
	return "<Question><Title><![CDATA[Adolf Hitler is from which country ?]]></Title><YourAnswer><![CDATA[Germany]]></YourAnswer><YourAnswer><![CDATA[USA]]></YourAnswer><Rhetorical>true</Rhetorical></Question>";
}